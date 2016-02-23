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
    class Sphere
    {
        private Vector3[] kocka;
        private Vector3[] farba;
        private Vector3[] normaly;
        List<Vector3> list;
        private int[] VBO;
        private int[] VAO;
        private bool flat;
        private Shaders.Shader VertexShader, FragmentShader;
        private Shaders.ShaderProgram spMain;
        private Matrix4 modelViewMatrix, projectionMatrix;
        private int width, height, Pi, DvaPi,len;
        private float scale;
        private Matrix4 Current, ScaleMatrix, TranslationMatrix, RotationMatrix, MatrixStore_Translations, MatrixStore_Rotations, MatrixStore_Scales;
        Kamera BigBrother; //iba pokusne, neriesil som nedostatky, chcel som sa len hybat
        DirectionalLight light;
        Material material;

        public Sphere(int w, int h, float s,bool flat)
        {
            this.flat = flat;
            Pi = 100; DvaPi = 100;//tieto hodnoty by timo mohol dat do konstruktora
            len = 2 * 3 * DvaPi + (Pi - 2) * DvaPi * 2 * 3;

            VBO = new int[3];
            VAO = new int[1];
            VertexShader = new Shaders.Shader();
            FragmentShader = new Shaders.Shader();
            spMain = new Shaders.ShaderProgram();
            list = new List<Vector3>();
            BigBrother = new Kamera(new Vector3(0.0f, 0.0f, 3.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            //svetlo - smer,ambient,specular,diffuse
            light = new DirectionalLight(new Vector3(0.0f, 0.0f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f));
            //material - ambient,specular,diffuse,koeficienty - ambient, specular, diffuse, shininess 
            material = new Material( 0.16f, 0.50f, 0.6f, 128);
           
            width = w;
            height = h;
            scale = s;
            Current = Matrix4.Identity;

            CalculateSphere();
            if (flat)
                SetFlatSphere();
            else
                SetGourandSphere();

            //Write();

            KresliSferu();
        }

        public Sphere(int w, int h, float s, string PathToSfere)
        {
            Pi = 100; DvaPi = 100;//tieto hodnoty by timo mohol dat do konstruktora
            len = 2 * 3 * DvaPi + (Pi - 2) * DvaPi * 2 * 3;

            VBO = new int[3];
            VAO = new int[1];
            VertexShader = new Shaders.Shader();
            FragmentShader = new Shaders.Shader();
            spMain = new Shaders.ShaderProgram();
            list = new List<Vector3>();
            BigBrother = new Kamera(new Vector3(0.0f, 0.0f, 3.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            //svetlo - smer,ambient,specular,diffuse
            light = new DirectionalLight(new Vector3(0.0f, 0.0f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f));
            //material - ambient,specular,diffuse,koeficienty - ambient, specular, diffuse, shininess 
            material = new Material(0.16f, 0.50f, 0.6f, 128);

            width = w;
            height = h;
            scale = s;
            Current = Matrix4.Identity;

            CalculateSphere();
            if (flat)
                SetFlatSphere();
            else
                SetGourandSphere();

            LoadSfereFromFile(PathToSfere);

            KresliSferu();
        }

        private void CalculateSphere()
        {
            float r = 1.0f;
            float x, y, z, sinB;

            for (double beta = 0.0f; beta <= Math.PI + 0.0001; beta += Math.PI / (double)Pi)
            {
                z = r * (float)Math.Cos(beta);
                sinB = (float)Math.Sin(beta);
                if (z == 1.0f || z == -1.0f)
                {
                    x = 0.0f;
                    y = 0.0f;
                    list.Add(new Vector3(x, y, z));
                    //System.Diagnostics.Debug.WriteLine(list[list.Count-1].ToString());
                }
                else
                {
                    for (double alfa = 0.0f; alfa < Math.PI * 2.0 - 0.0000001; alfa += Math.PI * 2.0f / (double)DvaPi)
                    {
                        x = r * (float)Math.Cos(alfa) * sinB;
                        y = r * (float)Math.Sin(alfa) * sinB;
                        list.Add(new Vector3(x, y, z));
                        //System.Diagnostics.Debug.WriteLine(list[list.Count - 1].ToString());
                    }
                }
            }
        }

        private void Write()
        {
            StreamWriter sw = new StreamWriter("..\\..\\Properties\\data\\sfera.txt");
            sw.WriteLine("#nv: "+kocka.Length);//pocet vrcholov
            foreach (var vrchol in kocka)
            {
                 sw.WriteLine("{0} {1} {2}",vrchol.X,vrchol.Y,vrchol.Z);
            }
            sw.WriteLine("#nn: " + normaly.Length);//pocet normal
            foreach (var normala in normaly)
            {
                sw.WriteLine("{0} {1} {2}", normala.X, normala.Y, normala.Z);
            }
            sw.Flush();
            sw.Close();
        }

        private void LoadSfereFromFile(string PathToSfere)
        {
            char[] separator = { ' ' };
            StreamReader sr = new StreamReader("..\\..\\Properties\\data\\"+PathToSfere);//kontrola by nemusel byt zla
            string CurrentLine = sr.ReadLine();
            if (CurrentLine.Contains("#nv:"))
                len = Int32.Parse(CurrentLine.Split(separator)[1]);

            kocka = new Vector3[len];

            string[] num;
            for (int i = 0; i < len; i++)
            {
               num = sr.ReadLine().Split(separator);//try
               kocka[i] = new Vector3(float.Parse(num[0]), float.Parse(num[1]), float.Parse(num[2]));
            }
            CurrentLine = sr.ReadLine();
            if (CurrentLine.Contains("#nn:"))
                if (len == Int32.Parse(CurrentLine.Split(separator)[1]))
                {
                    normaly = new Vector3[len];
                    farba=new Vector3[len];
                }
                //else vyhlas chybu

            //v pripade ze chyba nebola hlasena:
            for (int i = 0; i < len; i++)
            {
                num = sr.ReadLine().Split(separator);//try
                normaly[i] = new Vector3(float.Parse(num[0]), float.Parse(num[1]), float.Parse(num[2]));
            }

            for (int i = 0; i < len; i++)
            {
                farba[i] = new Vector3(1.0f,1.0f,1.0f);
            }

            sr.Close();
            InitSphere();
        }

        private void SetFlatSphere()
        {
            kocka = new Vector3[len];
            normaly = new Vector3[len];
            farba = new Vector3[len];

            int p = 0;
            //normaly inak - flat shading
            //vrchna cast
            for (int i = 1; i <= DvaPi; i++)
            {
                if (i == DvaPi)
                {
                    kocka[p] = list[0];
                    normaly[p] = Vector3.Cross(list[1] - list[0], list[0] - list[i]); p++;
                    kocka[p] = list[i];
                    normaly[p] = Vector3.Cross(list[i] - list[0], list[1] - list[i]); p++;
                    kocka[p] = list[1];
                    normaly[p] = Vector3.Cross(list[1] - list[i], list[0] - list[1]); p++;
                }
                else
                {
                    kocka[p] = list[0];
                    normaly[p] = Vector3.Cross(list[0] - list[i + 1], list[i] - list[0]); p++;
                    kocka[p] = list[i];
                    normaly[p] = Vector3.Cross(list[i] - list[0], list[i + 1] - list[i]); p++;
                    kocka[p] = list[i + 1];
                    normaly[p] = Vector3.Cross(list[i + 1] - list[i], list[0] - list[i + 1]); p++;
                }
            }

            //stred
            for (int j = 1; j <= Pi - 2; j++)
            {
                for (int i = ((j - 1) * DvaPi) + 1; i <= j * DvaPi; i++)
                {
                    if (i % DvaPi == 0)
                    {
                        kocka[p] = list[i];
                        normaly[p] = Vector3.Cross(list[i] - list[((j - 1) * DvaPi) + 1 + DvaPi], list[i + DvaPi] - list[i]); p++;
                        kocka[p] = list[i + DvaPi];
                        normaly[p] = Vector3.Cross(list[i + DvaPi] - list[i], list[((j - 1) * DvaPi) + 1 + DvaPi] - list[i + DvaPi]); p++;
                        kocka[p] = list[((j - 1) * DvaPi) + 1 + DvaPi];
                        normaly[p] = Vector3.Cross(list[((j - 1) * DvaPi) + 1 + DvaPi] - list[i + DvaPi], list[i] - list[((j - 1) * DvaPi) + 1 + DvaPi]); p++;

                        kocka[p] = list[i];
                        normaly[p] = Vector3.Cross(list[i] - list[((j - 1) * DvaPi) + 1], list[((j - 1) * DvaPi) + 1 + DvaPi] - list[i]);
                        p++;
                        kocka[p] = list[((j - 1) * DvaPi) + 1 + DvaPi];
                        normaly[p] = Vector3.Cross(list[((j - 1) * DvaPi) + 1 + DvaPi] - list[i], list[((j - 1) * DvaPi) + 1] - list[((j - 1) * DvaPi) + 1 + DvaPi]);
                        p++;
                        kocka[p] = list[((j - 1) * DvaPi) + 1];
                        normaly[p] = Vector3.Cross(list[((j - 1) * DvaPi) + 1] - list[((j - 1) * DvaPi) + 1 + DvaPi], list[i] - list[((j - 1) * DvaPi) + 1]);
                        p++;
                    }
                    else
                    {
                        kocka[p] = list[i];
                        normaly[p] = Vector3.Cross(list[i] - list[i + DvaPi + 1], list[i + DvaPi] - list[i]); p++;
                        kocka[p] = list[i + DvaPi];
                        normaly[p] = Vector3.Cross(list[i + DvaPi] - list[i], list[i + DvaPi + 1] - list[i + DvaPi]); p++;
                        kocka[p] = list[i + DvaPi + 1];
                        normaly[p] = Vector3.Cross(list[i + DvaPi + 1] - list[i + DvaPi], list[i] - list[i + DvaPi + 1]); p++;

                        kocka[p] = list[i];
                        normaly[p] = Vector3.Cross(list[i] - list[i + 1], list[i + DvaPi + 1] - list[i]); p++;
                        kocka[p] = list[i + DvaPi + 1];
                        normaly[p] = Vector3.Cross(list[i + DvaPi + 1] - list[i], list[i + 1] - list[i + DvaPi + 1]); p++;
                        kocka[p] = list[i + 1];
                        normaly[p] = Vector3.Cross(list[i + 1] - list[i + DvaPi + 1], list[i] - list[i + 1]); p++;
                    }
                }
            }

            //spodna cast
            int tmp = list.Count;
            for (int i = tmp - DvaPi - 1; i <= tmp - 2; i++)
            {
                if (i == tmp - 2)
                {
                    kocka[p] = list[i];
                    normaly[p] = Vector3.Cross(list[i] - list[tmp - DvaPi - 1], list[tmp - 1] - list[tmp - DvaPi - 1]); p++;
                    kocka[p] = list[tmp - 1];
                    normaly[p] = Vector3.Cross(list[tmp - 1] - list[i], list[tmp - DvaPi - 1] - list[tmp - 1]); p++;
                    kocka[p] = list[tmp - DvaPi - 1];
                    normaly[p] = Vector3.Cross(list[tmp - DvaPi - 1] - list[tmp - 1], list[i] - list[tmp - DvaPi - 1]); p++;
                }
                else
                {
                    kocka[p] = list[i];
                    normaly[p] = Vector3.Cross(list[i] - list[i + 1], list[tmp - 1] - list[i]); p++;
                    kocka[p] = list[tmp - 1];
                    normaly[p] = Vector3.Cross(list[tmp - 1] - list[i], list[i + 1] - list[tmp - 1]); p++;
                    kocka[p] = list[i + 1];
                    normaly[p] = Vector3.Cross(list[i + 1] - list[tmp - 1], list[i] - list[i + 1]); p++;
                }
            }

            for (int i = 0; i < p; i++)
            {
                //farba[i] = new Vector3(0.0f, 0.0f, (i + 1) / (float)p);
                farba[i] = new Vector3(1.0f, 1.0f, 1.0f);
                //normaly[i] = normaly[i].Normalized();
            }

            InitSphere();
        }

        private void SetGourandSphere()
        {
            kocka = new Vector3[len];
            normaly = new Vector3[len];
            farba = new Vector3[len];

            int p = 0;
            //gourand shading
            //vrchna cast
            for (int i = 1; i <= DvaPi; i++)
            {
                if (i == DvaPi)
                {
                    //System.Diagnostics.Debug.WriteLine("{0}->{1}->{2}",0,i,1);
                    kocka[p] = normaly[p] = list[0]; p++;
                    kocka[p] = normaly[p] = list[i]; p++;
                    kocka[p] = normaly[p] = list[1]; p++;
                }
                else
                {
                    //System.Diagnostics.Debug.WriteLine("{0}->{1}->{2}", 0, i, i+1);
                    kocka[p] = normaly[p] = list[0]; p++;
                    kocka[p] = normaly[p] = list[i]; p++;
                    kocka[p] = normaly[p] = list[i + 1]; p++;
                }
            }

            //stred
            for (int j = 1; j <= Pi - 2; j++)
            {
                for (int i = ((j - 1) * DvaPi) + 1; i <= j * DvaPi; i++)
                {
                    if (i % DvaPi == 0)
                    {
                        //System.Diagnostics.Debug.WriteLine("{0}->{1}->{2}", i, i + DvaPi,((j - 1) * DvaPi) + 1 + DvaPi);
                        kocka[p] = normaly[p] = list[i]; p++;
                        kocka[p] = normaly[p] = list[i + DvaPi]; p++;
                        kocka[p] = normaly[p] = list[((j - 1) * DvaPi) + 1 + DvaPi]; p++;

                        //System.Diagnostics.Debug.WriteLine("{0}->{1}->{2}", i, ((j - 1) * DvaPi) + 1 + DvaPi, ((j - 1) * DvaPi) + 1);
                        kocka[p] = normaly[p] = list[i]; p++;
                        kocka[p] = normaly[p] = list[((j - 1) * DvaPi) + 1 + DvaPi]; p++;
                        kocka[p] = normaly[p] = list[((j - 1) * DvaPi) + 1]; p++;
                    }
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine("{0}->{1}->{2}", i, i + DvaPi, i + DvaPi + 1);
                        kocka[p] = normaly[p] = list[i]; p++;
                        kocka[p] = normaly[p] = list[i + DvaPi]; p++;
                        kocka[p] = normaly[p] = list[i + DvaPi + 1]; p++;

                        //System.Diagnostics.Debug.WriteLine("{0}->{1}->{2}", i, i + DvaPi+1, i + 1);
                        kocka[p] = normaly[p] = list[i]; p++;
                        kocka[p] = normaly[p] = list[i + DvaPi + 1]; p++;
                        kocka[p] = normaly[p] = list[i + 1]; p++;
                    }
                }
            }

            //spodna cast
            int tmp = list.Count;
            for (int i = tmp - DvaPi - 1; i <= tmp - 2; i++)
            {
                if (i == tmp - 2)
                {
                    //System.Diagnostics.Debug.WriteLine("{0}->{1}->{2}", i, tmp - 1, tmp - DvaPi - 1);
                    kocka[p] = normaly[p] = list[i]; p++;
                    kocka[p] = normaly[p] = list[tmp - 1]; p++;
                    kocka[p] = normaly[p] = list[tmp - DvaPi - 1]; p++;
                }
                else
                {
                    //System.Diagnostics.Debug.WriteLine("{0}->{1}->{2}", i, tmp - 1, i + 1);
                    kocka[p] = normaly[p] = list[i]; p++;
                    kocka[p] = normaly[p] = list[tmp - 1]; p++;
                    kocka[p] = normaly[p] = list[i + 1]; p++;
                }
            }

            //System.Diagnostics.Debug.WriteLine("==========================");
            //foreach (var n in normaly)
            //{
            //    System.Diagnostics.Debug.WriteLine(n.ToString());
            //}
            //System.Diagnostics.Debug.WriteLine("==========================");

            for (int i = 0; i < p; i++)
            {
                //farba[i] = new Vector3(0.0f, 0.0f, (i + 1) / (float)p);
                farba[i] = new Vector3(1.0f, 1.0f, 1.0f);
                //normaly[i] = -normaly[i].Normalized();
            }

            InitSphere();
        }

        private void InitSphere()
        {
            NastavMatice(false);
            GL.GenBuffers(3, VBO);
            GL.GenVertexArrays(1, VAO);

            GL.BindVertexArray(VAO[0]);

            //vrcholy
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(len * Vector3.SizeInBytes), kocka, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            //farby
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(len * Vector3.SizeInBytes), farba, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);

            //normaly
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[2]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(len * Vector3.SizeInBytes), normaly, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);

            //vytvorenie shader programu--- pridana kontrola nacitania, zo Shaders.dll by som teda mohol odstranit vyhadzaovanie messageboxov
            //per vertex shaders
            //if (!VertexShader.LoadShader("..\\..\\Properties\\data\\shaders\\dirShader.vert", ShaderType.VertexShader))
            //    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");
            //if (!FragmentShader.LoadShader("..\\..\\Properties\\data\\shaders\\dirShader.frag", ShaderType.FragmentShader))
            //    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");

            //uplne prve shadere
            //if (!VertexShader.LoadShader("..\\..\\Properties\\data\\shaders\\l_shader.vert", ShaderType.VertexShader))
            //    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");
            //if (!FragmentShader.LoadShader("..\\..\\Properties\\data\\shaders\\l_shader.frag", ShaderType.FragmentShader))
            //    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");

            ////per pixel shaders - zatial nefunkcny, chyba je zrejme vo fragment shaderi
            if (!VertexShader.LoadShader("..\\..\\Properties\\data\\shaders\\dirPerPixelShader.vert", ShaderType.VertexShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");
            if (!FragmentShader.LoadShader("..\\..\\Properties\\data\\shaders\\dirPerPixelShader.frag", ShaderType.FragmentShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");

            spMain.CreateProgram();
            spMain.AddShaderToProgram(VertexShader);
            spMain.AddShaderToProgram(FragmentShader);
            spMain.LinkProgram();
            spMain.UseProgram();

            spMain.SetUniform("projectionMatrix", projectionMatrix);
            spMain.SetUniform("eye", new Vector3(0.0f, 0.0f, 3.5f));//dorobit pri zmene pozicie kamery update
            material.SetMaterialUniforms(spMain);
            light.SetDirectionalLightUniforms(spMain);

            //prvy shader pre rovnobezne svetlo
            ////farba svetla
            //spMain.SetUniform("sunLight.vColor", new Vector3(1.0f, 1.0f, 1.0f));
            ////nastavenie intenzity ambientneho svetla
            //spMain.SetUniform("sunLight.fAmbientIntensity", 0.15f);
            ////smer svetelnych lucov
            //spMain.SetUniform("sunLight.vDirection", new Vector3(1.0f, 0.0f, 0.0f));

            //GL.Enable(EnableCap.DepthTest);
            //GL.DepthFunc(DepthFunction.Less);
            //GL.ClearDepth(1.0);
        }

        public void SetLight(Vector3 specular,Vector3 ambient, Vector3 diffuse, Vector3 direction)
        {
            light = new DirectionalLight(direction, ambient, specular, diffuse);
            light.SetDirectionalLightUniforms(spMain);
        }

        public void SetMaterial(float SpecCoeff,float AmbCoeff,float DiffCoeff,int shin)
        {
            material = new Material(AmbCoeff,SpecCoeff,DiffCoeff,shin);
            material.SetMaterialUniforms(spMain);
        }

        private void NastavMatice(bool co)
        {
            //prvotny resize
            if (!co)
            {
                modelViewMatrix = Matrix4.LookAt(0.0f, 0.0f, 3.5f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
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

        private void KresliSferu()
        {
            Matrix4 mat = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix) * (TranslationMatrix * MatrixStore_Translations);
            Current = mat * modelViewMatrix;
            mat = mat.Inverted(); mat.Transpose();
            GL.BindVertexArray(VAO[0]);
            spMain.SetUniform("normalMatrix", mat);
            spMain.SetUniform("modelViewMatrix", Current);
        }

        public void DrawSphere()
        {
            GL.BindVertexArray(VAO[0]);
            GL.DrawArrays(PrimitiveType.Triangles, 0, len);
            //GL.DrawArrays(PrimitiveType.LineLoop, 0, len);
        }

        public void Transalte(float x, float y)
        {
            TranslationMatrix = Matrix4.CreateTranslation(x, y, 0.0f);
            Current = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix)* (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
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
            NastavMatice(true);
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

        public void Delete()
        {
            //zrejme by tu tiez mohla prist kontrola, ci znicenie programov prebehlo v poriadku, resp. ju zakomponovat do Shaders.dll
            GL.DeleteBuffers(3, VBO);
            GL.DeleteVertexArrays(1, VAO);
            list.Clear();
            spMain.DeleteProgram();
            VertexShader.DeleteShader();
            FragmentShader.DeleteShader();
        }

        public void Pohyb(float dd)
        {
            BigBrother.MoveCamera(dd);
            modelViewMatrix = BigBrother.ReturnCamera();
            Current = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix)* (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            spMain.SetUniform("modelViewMatrix", Current);
        }

        public void Natoc(float dd)
        {
            BigBrother.RotateCameraY(dd);
            modelViewMatrix = BigBrother.ReturnCamera();
            Current = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix) * (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            spMain.SetUniform("modelViewMatrix", Current);
        }
    }
}
