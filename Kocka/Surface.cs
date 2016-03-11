using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Kocka
{
    class Surface
    {
        private int width, height, NumOfVertices, NumOfTriangles, NumOfIndices, Xwidth, Ywidth;
        private Vector3[] vertices;
        private Vector3[] normals;
        private Vector3[] color;
        private int[] Indices;
        private List<Vector3> coords;
        //private List<Vector3> noormals;
        //private List<Vector3> coolors;

        private float scale, min, max;
        private int[] VBO;
        private int[] VAO;
        private Shaders.Shader VertexShader, FragmentShader;
        private Shaders.ShaderProgram spMain;
        private Matrix4 modelViewMatrix, projectionMatrix;
        private Matrix4 Current, ScaleMatrix, TranslationMatrix, RotationMatrix, MatrixStore_Translations, MatrixStore_Rotations, MatrixStore_Scales;
        DirectionalLight light;
        Material material;

        public Surface(int w, int h, string pathToFile)
        {
            min = float.MaxValue;
            max = float.MinValue;
            scale = 1.0f;
            width = w; height = h;
            NumOfTriangles = NumOfVertices = Xwidth = Ywidth = 0;
            //svetlo - smer,ambient,specular,diffuse
            light = new DirectionalLight(new Vector3(0.0f, 0.0f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f));
            //material - ambient,specular,diffuse,koeficienty - ambient, specular, diffuse, shininess 
            material = new Material(0.16f, 0.50f, 0.6f, 124);
            //BigBrother = new Kamera(new Vector3(0.0f, 0.0f, 3.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));

            coords = new List<Vector3>();
            //noormals = new List<Vector3>();
            //coolors = new List<Vector3>();
            VBO = new int[4];
            VAO = new int[1];
            VertexShader = new Shaders.Shader();
            FragmentShader = new Shaders.Shader();
            spMain = new Shaders.ShaderProgram();

            //nacitam data a zistim rozmery
            LoadData(pathToFile);
            GetWidth();

            InitSurface();
            InitScene();
            FirstDraw();
        }

        #region nacitanie/nastavenie plochy a fsetkeho potrebneho
        private void InitScene()
        {
            SetMatrices(false);

            GL.GenBuffers(4, VBO);
            GL.GenVertexArrays(1, VAO);

            GL.BindVertexArray(VAO[0]);

            //vrcholy
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(NumOfVertices * Vector3.SizeInBytes), vertices, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            //farby
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(NumOfVertices * Vector3.SizeInBytes), color, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);

            //normaly
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[2]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(NumOfVertices * Vector3.SizeInBytes), normals, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);

            //indices
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, VBO[3]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(int) * Indices.Length), Indices, BufferUsageHint.StaticDraw);
            GL.Enable(EnableCap.PrimitiveRestart);
            GL.PrimitiveRestartIndex(NumOfVertices);

            //prvy shader
            //if (!VertexShader.LoadShader("..\\..\\Properties\\data\\shaders\\shader.vert", ShaderType.VertexShader))
            //    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");
            //if (!FragmentShader.LoadShader("..\\..\\Properties\\data\\shaders\\shader.frag", ShaderType.FragmentShader))
            //    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");

            //per pixel
            string vspath = string.Format("..{0}..{0}Properties{0}data{0}shaders{0}dirPerPixelShader.vert", Path.DirectorySeparatorChar);
            string fspath = string.Format("..{0}..{0}Properties{0}data{0}shaders{0}dirPerPixelShader.frag", Path.DirectorySeparatorChar);
            if (!VertexShader.LoadShader(vspath, ShaderType.VertexShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");
            if (!FragmentShader.LoadShader(fspath, ShaderType.FragmentShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");

            ////per fragment
            //string vspath = string.Format("..{0}..{0}Properties{0}data{0}shaders{0}dirShader.vert", Path.DirectorySeparatorChar);
            //string fspath = string.Format("..{0}..{0}Properties{0}data{0}shaders{0}dirShader.frag", Path.DirectorySeparatorChar);
            //if (!VertexShader.LoadShader(vspath, ShaderType.VertexShader))
            //    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");
            //if (!FragmentShader.LoadShader(fspath, ShaderType.FragmentShader))
            //    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");

            spMain.CreateProgram();
            spMain.AddShaderToProgram(VertexShader);
            spMain.AddShaderToProgram(FragmentShader);
            spMain.LinkProgram();
            spMain.UseProgram();

            spMain.SetUniform("projectionMatrix", projectionMatrix);//iba toto pri jednoduchom
            spMain.SetUniform("eye", new Vector3(0.0f, 0.0f, 30.0f));//dorobit pri zmene pozicie kamery update
            material.SetMaterialUniforms(spMain);
            light.SetDirectionalLightUniforms(spMain);
        }

        private void LoadData(string pathToFile)
        {
            StreamReader sr;
            Vector3 tmp;
            char[] separator = { ' ', '\t' };
            string[] line;
            try
            {
                sr = new StreamReader(pathToFile);
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine().Split(separator);
                    tmp = new Vector3(float.Parse(line[0]), float.Parse(line[1]), float.Parse(line[2]));
                    coords.Add(tmp);
                    if (coords.Last().Z < min)
                        min = coords.Last().Z;
                    if (coords.Last().Z > max)
                        max = coords.Last().Z;
                }
                sr.Close();
                NumOfVertices = coords.Count;
            }
            catch (FileNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show("Subor sa nenasiel!");
            }
        }

        private void GetWidth()
        {
            int p = 0;
            while (coords[p].X == coords[p + 1].X)
                p++;
            p++;
            Xwidth = p;
            Ywidth = NumOfVertices / p;
            //NumOfIndices=(pocetRiadkov-1)*pocetIndicisNaRiadok + pocetRestartov
            NumOfIndices = (Xwidth - 1) * (Ywidth * 2) + (Xwidth - 2);

            System.Diagnostics.Debug.WriteLine("Xwidth = {0}", Xwidth);
            System.Diagnostics.Debug.WriteLine("Ywidth = {0}", Ywidth);
            System.Diagnostics.Debug.WriteLine("NumOfVertices = {0}", NumOfVertices);
        }

        private void InitSurface()
        {
            vertices = new Vector3[NumOfVertices];
            color = new Vector3[NumOfVertices];
            normals = new Vector3[NumOfVertices];
            Indices = new int[NumOfIndices];

            GetIndices();
            CalculateColor();
            ScaleHeights();
            vertices = coords.ToArray();
            CalculateNormals();
        }

        private void GetIndices()
        {
            //Xwidth = Ywidth = 5;
            //NumOfVertices = Xwidth * Ywidth;
            //NumOfIndices = (Xwidth - 1) * (Ywidth * 2) + (Xwidth - 2);
            //System.Diagnostics.Debug.WriteLine("NumOfIndices = {0}", NumOfIndices);

            int p = 0;
            for (int i = 0; i < NumOfVertices - Ywidth; i++)
            {
                Indices[p] = i; p++;
                //System.Diagnostics.Debug.Write(i + ",");
                Indices[p] = i + Ywidth; p++;
                //System.Diagnostics.Debug.Write((i + Ywidth) + ",");
                if (i % Ywidth == (Ywidth - 1) && i != NumOfVertices - Ywidth - 1)
                {
                    //System.Diagnostics.Debug.Write(NumOfVertices + "\n");
                    Indices[p] = NumOfVertices; p++;
                }
                //if(i<20)
                //{
                //    System.Diagnostics.Debug.Write(i + ",");
                //    System.Diagnostics.Debug.Write((i + Ywidth) + ",");
                //}
            }
            //System.Diagnostics.Debug.WriteLine("NumOfIndices vol2 = {0}", p);
        }

        private void CalculateNormals()
        {
            Vector3 n = new Vector3();
            int p = 0;

            #region spodna rada
            for (int i = 0; i < Ywidth; i++)
            {
                if (i == 0)
                {
                    n = Vector3.Cross(coords[0] - coords[1], coords[Ywidth] - coords[0]);
                    normals[p] = -n; p++;

                    //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", 1, i, Ywidth);
                }
                else if (i == Ywidth - 1)
                {
                    n = Vector3.Cross(coords[i] - coords[i - 1], coords[i + Ywidth - 1] - coords[i]);
                    n += Vector3.Cross(coords[i] - coords[i + Ywidth - 1], coords[i + Ywidth] - coords[i]);
                    normals[p] = -n; p++;

                    //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i - 1, i, i + Ywidth - 1);
                    //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i + Ywidth - 1, i, i + Ywidth);
                }
                else
                {
                    n = Vector3.Cross(coords[i] - coords[i - 1], coords[i + Ywidth - 1] - coords[i]);
                    n += Vector3.Cross(coords[i] - coords[i + Ywidth - 1], coords[i + Ywidth] - coords[i]);
                    n += Vector3.Cross(coords[i] - coords[i + 1], coords[i + Ywidth] - coords[i]);
                    normals[p] = -n; p++;

                    //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i - 1, i, i + Xwidth - 1);
                    //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i + Ywidth - 1, i, i + Ywidth);
                    //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i + 1, i, i + Ywidth);
                }
            }
            #endregion

            #region stred
            //System.Diagnostics.Debug.WriteLine("-----------STRED----------------");
            for (int i = Ywidth; i < NumOfVertices - Ywidth; i++)
            {
                //lavy okraj
                if (i % Ywidth == 0)
                {
                    //System.Diagnostics.Debug.WriteLine("-----------LAVY OKRAJ----------------");
                    n = Vector3.Cross(coords[i] - coords[i - Ywidth], coords[i - Ywidth + 1] - coords[i]);
                    n += Vector3.Cross(coords[i] - coords[i - Ywidth + 1], coords[i + 1] - coords[i]);
                    n += Vector3.Cross(coords[i] - coords[i + 1], coords[i + Ywidth] - coords[i]);
                    normals[p] = -n; p++;

                    //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i - Ywidth, i, i - Ywidth + 1);
                    //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i - Ywidth + 1, i, i + 1);
                    //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i + 1, i, i + Ywidth);
                }
                //pravy okraj
                else if (i % Ywidth == Ywidth - 1)
                {
                    //System.Diagnostics.Debug.WriteLine("-----------PRAVY OKRAJ----------------");
                    n = Vector3.Cross(coords[i] - coords[i - Ywidth], coords[i - 1] - coords[i]);
                    n += Vector3.Cross(coords[i] - coords[i - 1], coords[i + Ywidth - 1] - coords[i]);
                    n += Vector3.Cross(coords[i] - coords[i + Ywidth - 1], coords[i + Ywidth] - coords[i]);
                    normals[p] = -n; p++;

                    //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i - Ywidth, i, i - 1);
                    //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i - 1, i, i + Ywidth - 1);
                    //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i + Ywidth - 1, i, i + Ywidth);
                }
                //inak
                else
                {
                    n = Vector3.Cross(coords[i] - coords[i - Ywidth], coords[i - 1] - coords[i]);
                    n += Vector3.Cross(coords[i] - coords[i - 1], coords[i + Ywidth - 1] - coords[i]);
                    n += Vector3.Cross(coords[i] - coords[i + Ywidth - 1], coords[i + Ywidth] - coords[i]);

                    n += Vector3.Cross(coords[i] - coords[i + Ywidth], coords[i + 1] - coords[i]);
                    n += Vector3.Cross(coords[i] - coords[i + 1], coords[i - Ywidth + 1] - coords[i]);
                    n += Vector3.Cross(coords[i] - coords[i - Ywidth + 1], coords[i - Ywidth] - coords[i]);
                    normals[p] = -n; p++;

                    //    System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i - Ywidth, i, i - 1);
                    //    System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i - 1, i, i + Ywidth - 1);
                    //    System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i + Ywidth - 1, i, i + Ywidth);
                    //    System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i + Ywidth, i, i + 1);
                    //    System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i + 1, i, i - Ywidth + 1);
                    //    System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i - Ywidth + 1, i, i - Ywidth);
                }
            }
            #endregion

            #region vrchna rada
            for (int i = NumOfVertices - Ywidth; i < NumOfVertices; i++)
            {
                //lavy okraj
                if (i == NumOfVertices - Ywidth)
                {
                    n = Vector3.Cross(coords[i] - coords[i - Ywidth], coords[i - Ywidth + 1] - coords[i]);
                    n += Vector3.Cross(coords[i] - coords[i - Ywidth + 1], coords[i + 1] - coords[i]);
                    normals[p] = -n; p++;

                    //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i - Ywidth, i, i - Ywidth + 1);
                    //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i - Ywidth + 1, i, i + 1);
                }
                //pravy okraj
                else if (i == NumOfVertices - 1)
                {
                    n = Vector3.Cross(coords[i] - coords[i - 1], coords[i - Ywidth] - coords[i]);
                    normals[p] = -n; p++;

                    //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i - 1, i, i - Ywidth);
                }
                //inak
                else
                {
                    n = Vector3.Cross(coords[i] - coords[i - 1], coords[i - Ywidth] - coords[i]);
                    n += Vector3.Cross(coords[i] - coords[i - Ywidth], coords[i - Ywidth + 1] - coords[i]);
                    n += Vector3.Cross(coords[i] - coords[i - Ywidth + 1], coords[i + 1] - coords[i]);
                    normals[p] = -n; p++;

                    //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i - 1, i, i - Ywidth);
                    //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i - Ywidth, i, i - Ywidth + 1);
                    //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", i - Ywidth + 1, i, i + 1);
                }
            }
            #endregion
            //System.Diagnostics.Debug.WriteLine("MEGAPEEEEE = {0}", p);
        }

        private void CalculateColor()
        {
            for (int i = 0; i < NumOfVertices; i++)
            {
                color[i] = CalculateColor(coords[i].Z);
            }
        }

        private void ScaleHeights()
        {
            System.Diagnostics.Debug.WriteLine("min = {0}", min);
            System.Diagnostics.Debug.WriteLine("max = {0}", max);

            //float L = Math.Abs(max - min);
            float L = 10.0f;
            for (int i = 0; i < coords.Count; i++)
            {
                coords[i] = new Vector3(coords[i].X, coords[i].Y, (coords[i].Z - min) / (float)L);
                //coords[i] = new Vector3(coords[i].X, coords[i].Y, 0.0f);
            }
        }

        private Vector3 CalculateColor(float height)
        {
            Vector3 col = new Vector3(1.0f, 1.0f, 1.0f); 
            float dv;
            dv = max - min;
            #region funkcie
            LinearFunction R1 = new LinearFunction(min + 0.35f * dv, min + 0.65f * dv, 0.0f, 1.0f);
            LinearFunction R2 = new LinearFunction(min + 0.9f * dv, max, 1.0f, 0.5f);
            LinearFunction G1 = new LinearFunction(min + 0.1f * dv, min + 0.35f * dv, 0.0f, 1.0f);
            LinearFunction G2 = new LinearFunction(min + 0.65f * dv, min + 0.9f * dv, 1.0f, 0.0f);
            LinearFunction B1 = new LinearFunction(0.0f, min + 0.1f * dv, 0.5f, 1.0f);
            LinearFunction B2 = new LinearFunction(min + 0.35f * dv, min + 0.65f * dv, 1.0f, 0.0f);
            #endregion

            if (height < min + 0.1f * dv)
            {
                col.X = col.Y = 0.0f;
                col.Z = B1.Value(height);
            }
            else if (height < min + 0.35f * dv)
            {
                col.X = 0.0f;
                col.Y = G1.Value(height);
            }
            else if (height < min + 0.65f * dv)
            {
                col.X = R1.Value(height);
                col.Z = B2.Value(height);
            }
            else if (height < min + 0.9f * dv)
            {
                col.Y = G2.Value(height);
                col.Z = 0.0f;
            }
            else
            {
                col.X = R2.Value(height);
                col.Y = col.Z = 0.0f;
            }
            return (col);
        }

        #endregion

        #region ovladanie mysou
        public void Transalte(float x, float y)
        {
            TranslationMatrix = Matrix4.CreateTranslation(x, y, 0.0f);
            Current = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix) * (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            spMain.SetUniform("modelViewMatrix", Current);
        }

        public void Rotate(float x, float y, float angle)
        {
            RotationMatrix = Matrix4.CreateFromAxisAngle(new Vector3(y, x, 0.0f), angle);
            Matrix4 mat = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix);
            Current = mat * (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            mat = mat.Inverted(); mat = Matrix4.Transpose(mat);
            spMain.SetUniform("normalMatrix", mat);
            spMain.SetUniform("modelViewMatrix", Current);
        }

        public void Scale(float s)
        {
            scale = s;
            ScaleMatrix = Matrix4.CreateScale(scale, scale, scale);
            Matrix4 mat = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix);
            Current = mat * (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            mat = mat.Inverted(); mat = Matrix4.Transpose(mat);
            spMain.SetUniform("normalMatrix", mat);
            spMain.SetUniform("modelViewMatrix", Current);
        }

        public void Resize(int w, int h)
        {
            width = w; height = h;
            SetMatrices(true);
            GL.BindVertexArray(VAO[0]);
            spMain.SetUniform("projectionMatrix", projectionMatrix);
            spMain.SetUniform("modelViewMatrix", Current);
        }

        public void Ende()
        {
            MatrixStore_Translations = MatrixStore_Translations * TranslationMatrix;
            MatrixStore_Rotations = MatrixStore_Rotations * RotationMatrix;

            spMain.SetUniform("modelViewMatrix", Current);
            ScaleMatrix = Matrix4.Identity;
            RotationMatrix = Matrix4.Identity;
            TranslationMatrix = Matrix4.Identity;
        }

        //public void MoveCamera(float dd)
        //{
        //    BigBrother.MoveCamera(dd);
        //    modelViewMatrix = BigBrother.ReturnCamera();
        //    Current = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix) * (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
        //    spMain.SetUniform("modelViewMatrix", Current);
        //}

        #endregion

        private void FirstDraw()
        {
            Matrix4 mat = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix) * (TranslationMatrix * MatrixStore_Translations);
            Current = mat * modelViewMatrix;
            mat = mat.Inverted(); mat.Transpose();
            GL.BindVertexArray(VAO[0]);
            spMain.SetUniform("normalMatrix", mat);
            spMain.SetUniform("modelViewMatrix", Current);
        }

        public void DrawSurface()
        {
            GL.BindVertexArray(VAO[0]);
            GL.DrawElements(PrimitiveType.TriangleStrip, NumOfIndices, DrawElementsType.UnsignedInt, 0);
            //GL.DrawElements(PrimitiveType.LineStrip, NumOfIndices, DrawElementsType.UnsignedInt, 0);
        }

        private void SetMatrices(bool co)
        {
            //prvotny resize
            if (!co)
            {
                modelViewMatrix = Matrix4.LookAt(0.0f, 0.0f, 30.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
                projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, width / (float)height, 0.01f, 300.0f);

                ScaleMatrix = Matrix4.CreateScale(scale, scale, scale);
                TranslationMatrix = Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f);
                RotationMatrix = Matrix4.Identity;
                MatrixStore_Rotations = Matrix4.Identity;
                MatrixStore_Translations = Matrix4.Identity;
                MatrixStore_Scales = Matrix4.Identity;
                Current = Matrix4.Identity;
            }
            //resize vyvolany pouzivatelom
            else
                projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, width / (float)height, 0.01f, 300.0f);
        }

        public void Delete()
        {
            //zrejme by tu tiez mohla prist kontrola, ci znicenie programov prebehlo v poriadku, resp. ju zakomponovat do Shaders.dll
            GL.DeleteBuffers(3, VBO);
            GL.DeleteVertexArrays(1, VAO);
            coords.Clear();
            spMain.DeleteProgram();
            VertexShader.DeleteShader();
            FragmentShader.DeleteShader();
        }
    }
}
