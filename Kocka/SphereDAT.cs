using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Kocka
{
    class Sphere
    {
        private int NumOfParallels;
        private int NumOfVertices;
        private int WhatToDraw;
        private int width, height, NumOfTriangles;
        private Vector3[] zaloha;
        private Vector3[] normals;
        private Vector3[] colors;
        private List<Vector3> coords;

        private float RAD, R, scale, min, max, value;
        private bool Status, colrscl, shaderOption;
        private int[] VBO;
        private int[] VAO;
        private int[] Indexes;
        private Shaders.Shader VertexShader, FragmentShader;
        private Shaders.ShaderProgram spMain;
        private Matrix4 modelViewMatrix, projectionMatrix;
        private Matrix4 Current, ScaleMatrix, TranslationMatrix, RotationMatrix, MatrixStore_Translations, MatrixStore_Rotations, MatrixStore_Scales;
        private ColorScale colorScale;
        private Form1 form;
        DirectionalLight light;
        Material material;
        Kamera BigBrother;
        System.Windows.Forms.ToolStripProgressBar toolStripBar;
        System.Windows.Forms.ToolStripLabel toolStripLabel;

        //nacitanie dat do listu + prevod
        public Sphere(int w, int h, string pathToFile, System.Windows.Forms.ToolStripProgressBar bar = null, System.Windows.Forms.ToolStripLabel label = null, Form1 form = null, bool shaderOption = false)
        {
            this.shaderOption = shaderOption;
            toolStripBar = bar;
            toolStripLabel = label;
            this.form = form;
            WhatToDraw = 1;
            Status = colrscl = false;
            min = float.MaxValue;
            max = float.MinValue;
            R = 1.0f;
            value = 0.0f;
            scale = 1.0f;
            width = w; height = h;
            RAD = (float)Math.PI / 180.0f;
            NumOfTriangles = NumOfParallels = NumOfVertices = 0;
            //svetlo - smer,ambient,specular,diffuse
            light = new DirectionalLight(new Vector3(0.0f, 0.0f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f));
            //material - ambient,specular,diffuse,koeficienty - ambient, specular, diffuse, shininess 
            material = new Material(0.16f, 0.50f, 0.6f, 124);
            //BigBrother = new Kamera(new Vector3(0.0f, 0.0f, 3.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            //colorScale = new ColorScale(-100, 100, width, height);

            coords = new List<Vector3>();

            VBO = new int[4];
            VAO = new int[1];
            VertexShader = new Shaders.Shader();
            FragmentShader = new Shaders.Shader();
            spMain = new Shaders.ShaderProgram();
            bool init = false;

            var bw = new System.ComponentModel.BackgroundWorker();

            bw.DoWork += (sender, args) =>
            {
                // do your lengthy stuff here -- this will happen in a separate thread
                init = Init(pathToFile);
            };

            bw.RunWorkerCompleted += (sender, args) =>
            {
                if (args.Error != null)  // if an exception occurred during DoWork,
                    System.Windows.Forms.MessageBox.Show(args.Error.ToString());  // do your error handling here

                // Do whatever else you want to do after the work completed.
                // This happens in the main UI thread.

                //toto musi byt az tu, pretoze GL.prikazy pre objekt Sphere musia byt volane v tom threade, v ktorom objekt vznikol
                if (init)
                {
                    colorScale = new ColorScale(min, max, width, height);//tu sa vytvaraju shadere, takze to nemoze byt v bw.DoWork
                    SetColors();//pracuje s colorScalemi
                    InitScene(false);//koli shaderom
                    FirstDraw();
                    form.SetBoolean_sfera(true);
                }
                else
                    form.SetMenuStrip_Enabled(true);
            };

            bw.RunWorkerAsync(); // starts the background worker
        }

        private bool Init(string pathToFile)
        {
            if (LoadData(pathToFile))
            {
                zaloha = new Vector3[coords.Count];
                zaloha = coords.ToArray();
                value = (max - min);
                ScaleHeights(10.0f);
                GeoToCartesianCoords();

                NumOfParallels = (int)Math.Sqrt(coords.Count - 2) + 1;
                GetNumberOfTriangles();
                NumOfVertices = 3 * NumOfTriangles;
                Indexes = new int[NumOfVertices];

                //triangulacia
                SetIndexes();
                CalculateNormals();
                return true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Súbor " + pathToFile + " nemá podporu!", "Vnimanie!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                label label = new label(SetToolStripLabel);
                progres progres = new progres(SetProgressBar);
                form.statusStrip1.Invoke(label, "Súbor nemá podporu...");
                form.statusStrip1.Invoke(progres, 0);
                return false;
            }
        }

        private bool LoadData(string pathToFile)
        {
            label label = new label(SetToolStripLabel);
            progres progres = new progres(SetProgressBar);
            form.statusStrip1.Invoke(label, "Prebieha načítavanie dát...");
            //SetToolStrip("Prebieha načítavanie dát...");
            StreamReader sr;
            Vector3 tmp;
            int count = File.ReadLines(pathToFile).Count();
            int d = count / 100;
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

                    if (coords.Count % d == 0)
                        form.statusStrip1.Invoke(progres, 100 * coords.Count / count);
                }
                sr.Close();
                if (coords[0].X == coords[1].X)
                    Status = false;
                else
                    Status = true;
                form.statusStrip1.Invoke(progres, 100);
                //SetToolStrip("");
                System.Diagnostics.Debug.WriteLine("cislo = " +  Vector3.SizeInBytes);
            }
            catch (FileNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show("Subor sa nenasiel!");
                Status = false;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Vyskytla sa neznáma chyba pri načítaní súboru!");
                Status = false;
            }
            return Status;
        }

        public bool Loaded()
        {
            return Status;
        }

        private void SetIndexes()
        {
            label label = new label(SetToolStripLabel);
            progres progres = new progres(SetProgressBar);
            form.statusStrip1.Invoke(label, "Prebieha nastavovanie indexov...");
            int p = 0;
            int startIndex = 0;
            int endIndex = 0;
            int tmp = 0;

            //Timova triangulacia
            #region cepicka
            Indexes[p] = 0; p++;
            Indexes[p] = 1; p++;
            Indexes[p] = 2; p++;

            Indexes[p] = 0; p++;
            Indexes[p] = 2; p++;
            Indexes[p] = 3; p++;

            Indexes[p] = 0; p++;
            Indexes[p] = 3; p++;
            Indexes[p] = 4; p++;

            Indexes[p] = 0; p++;
            Indexes[p] = 4; p++;
            Indexes[p] = 1; p++;
            #endregion

            #region horna polovica
            for (int i = 1; i < (NumOfParallels - 1) / 2; i++)
            {
                form.statusStrip1.Invoke(progres, 100 * i / NumOfParallels);
                startIndex = 2 * i * (i - 1) + 1;
                endIndex = startIndex + (i * 4) - 1;
                int increment = i * 4;
                for (int j = startIndex; j <= endIndex; j++)
                {
                    if (j == startIndex)
                    {
                        Indexes[p] = j; p++;
                        Indexes[p] = 2 * (i + 1) * i + (i + 1) * 4; p++;
                        Indexes[p] = j + increment; p++;

                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment; p++;
                        Indexes[p] = j + increment + 1; p++;

                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment + 1; p++;
                        Indexes[p] = j + 1; p++;
                    }
                    if (j == startIndex + i)
                    {
                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment; p++;
                        Indexes[p] = j + increment + 1; p++;

                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment + 1; p++;
                        Indexes[p] = j + increment + 2; p++;

                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment + 2; p++;
                        Indexes[p] = j + 1; p++;
                    }
                    if (j == startIndex + 2 * i)
                    {
                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment + 1; p++;
                        Indexes[p] = j + increment + 2; p++;

                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment + 2; p++;
                        Indexes[p] = j + increment + 3; p++;

                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment + 3; p++;
                        Indexes[p] = j + 1; p++;
                    }
                    if (j == startIndex + 3 * i)
                    {
                        if (j == endIndex)//toto by malo nastat 2 krat
                        {
                            Indexes[p] = j; p++;
                            Indexes[p] = j + increment + 2; p++;
                            Indexes[p] = j + increment + 3; p++;

                            Indexes[p] = j; p++;
                            Indexes[p] = j + increment + 3; p++;
                            Indexes[p] = j + increment + 4; p++;

                            Indexes[p] = j; p++;
                            Indexes[p] = j + increment + 4; p++;
                            Indexes[p] = startIndex; p++;
                        }
                        else
                        {
                            Indexes[p] = j; p++;
                            Indexes[p] = j + increment + 2; p++;
                            Indexes[p] = j + increment + 3; p++;

                            Indexes[p] = j; p++;
                            Indexes[p] = j + increment + 3; p++;
                            Indexes[p] = j + increment + 4; p++;

                            Indexes[p] = j; p++;
                            Indexes[p] = j + increment + 4; p++;
                            Indexes[p] = j + 1; p++;
                        }
                    }
                    if (j > startIndex && j < startIndex + i)
                    {
                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment; p++;
                        Indexes[p] = j + increment + 1; p++;

                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment + 1; p++;
                        Indexes[p] = j + 1; p++;
                    }
                    if (j > startIndex + i && j < startIndex + 2 * i)
                    {
                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment + 1; p++;
                        Indexes[p] = j + increment + 2; p++;

                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment + 2; p++;
                        Indexes[p] = j + 1; p++;
                    }
                    if (j > startIndex + 2 * i && j < startIndex + 3 * i)
                    {
                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment + 2; p++;
                        Indexes[p] = j + increment + 3; p++;

                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment + 3; p++;
                        Indexes[p] = j + 1; p++;
                    }
                    if (j > startIndex + 3 * i && j <= endIndex)
                    {
                        if (j != endIndex)
                        {
                            Indexes[p] = j; p++;
                            Indexes[p] = j + increment + 3; p++;
                            Indexes[p] = j + increment + 4; p++;

                            Indexes[p] = j; p++;
                            Indexes[p] = j + increment + 4; p++;
                            Indexes[p] = j + 1; p++;
                        }
                        else
                        {
                            Indexes[p] = j; p++;
                            Indexes[p] = j + increment + 3; p++;
                            Indexes[p] = j + increment + 4; p++;

                            Indexes[p] = j; p++;
                            Indexes[p] = j + increment + 4; p++;
                            Indexes[p] = startIndex; p++;
                        }
                    }
                }
            }
            #endregion

            #region dolna polovica
            for (int i = (NumOfParallels - 1) / 2; i >= 1; i--)
            {
                form.statusStrip1.Invoke(progres, 100 * i / NumOfParallels);
                if (i == (NumOfParallels - 1) / 2)//if i == prva iteracia
                {
                    startIndex = 2 * i * (i - 1) + 1;
                    endIndex = startIndex + (i * 4) - 1;
                    tmp = startIndex;
                }
                else
                {
                    startIndex = tmp + (i + 1) * 4;
                    endIndex = startIndex + (i * 4) - 1;
                    tmp = startIndex;
                }
                int increment = i * 4;
                for (int j = startIndex; j <= endIndex; j++)
                {
                    if (j == startIndex)
                    {
                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment; p++;
                        Indexes[p] = j + 1; p++;
                    }
                    if (j == startIndex + i)
                    {
                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment - 1; p++;
                        Indexes[p] = j + 1; p++;
                    }
                    if (j == startIndex + 2 * i)
                    {
                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment - 2; p++;
                        Indexes[p] = j + 1; p++;
                    }
                    if (j == startIndex + 3 * i)
                    {
                        if (j == endIndex)
                        {
                            Indexes[p] = j; p++;
                            Indexes[p] = j + 1; p++;
                            Indexes[p] = startIndex; p++;
                        }
                        else
                        {
                            Indexes[p] = j; p++;
                            Indexes[p] = j + increment - 3; p++;
                            Indexes[p] = j + 1; p++;
                        }
                    }
                    if (j > startIndex && j < startIndex + i)
                    {
                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment - 1; p++;
                        Indexes[p] = j + increment; p++;

                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment; p++;
                        Indexes[p] = j + 1; p++;
                    }
                    if (j > startIndex + i && j < startIndex + 2 * i)
                    {
                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment - 2; p++;
                        Indexes[p] = j + increment - 1; p++;

                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment - 1; p++;
                        Indexes[p] = j + 1; p++;
                    }
                    if (j > startIndex + 2 * i && j < startIndex + 3 * i)
                    {
                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment - 3; p++;
                        Indexes[p] = j + increment - 2; p++;

                        Indexes[p] = j; p++;
                        Indexes[p] = j + increment - 2; p++;
                        Indexes[p] = j + 1; p++;
                    }
                    if (j > startIndex + 3 * i && j <= endIndex)
                    {
                        if (j != endIndex)
                        {
                            Indexes[p] = j; p++;
                            Indexes[p] = j + increment - 4; p++;
                            Indexes[p] = j + increment - 3; p++;

                            Indexes[p] = j; p++;
                            Indexes[p] = j + increment - 3; p++;
                            Indexes[p] = j + 1; p++;
                        }
                        else
                        {
                            Indexes[p] = j; p++;
                            Indexes[p] = j + increment - 4; p++;
                            Indexes[p] = j + 1; p++;

                            Indexes[p] = j; p++;
                            Indexes[p] = j + 1; p++;
                            Indexes[p] = startIndex; p++;
                        }
                    }
                }
            }
            #endregion
        }

        private void CalculateNormals()
        {
            label label = new label(SetToolStripLabel);
            progres progres = new progres(SetProgressBar);
            form.statusStrip1.Invoke(label, "Prebieha výpočet normál...");
            normals = new Vector3[coords.Count];
            int d = NumOfVertices / 300;

            //for (int i = 0; i < NumOfVertices / 3; i++)
            for (int i = 0; i < NumOfTriangles; i++)
            {
                int index1 = Indexes[i * 3];
                int index2 = Indexes[i * 3 + 1];
                int index3 = Indexes[i * 3 + 2];

                Vector3 side1 = coords[index1] - coords[index3];
                Vector3 side2 = coords[index2] - coords[index1];
                Vector3 normal = Vector3.Cross(side1, side2);

                normals[index1] += normal;
                normals[index2] += normal;
                normals[index3] += normal;
                if (i % d == 0)
                {
                    float tmp = (3 * i / (float)NumOfVertices);
                    form.statusStrip1.Invoke(progres, (int)(100 * tmp));
                }
            }
        }

        private void SetColors()
        {
            label label = new label(SetToolStripLabel);
            form.statusStrip1.Invoke(label, "Prebieha nastavovanie farieb...");
            colors = new Vector3[coords.Count];
            colors = colorScale.SetColorArray(zaloha);
        }

        public void InitScene(bool b)
        {
            label label = new label(SetToolStripLabel);
            progres progres = new progres(SetProgressBar);
            form.statusStrip1.Invoke(label, "Prebieha nastavovanie scény...");
            form.statusStrip1.Invoke(progres, 0);

            SetMatrices(b);
            GL.GenBuffers(4, VBO);
            GL.GenVertexArrays(1, VAO);

            GL.BindVertexArray(VAO[0]);

            //vrcholy
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(coords.Count * Vector3.SizeInBytes), coords.ToArray(), BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            
            //farby
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colors.Length * Vector3.SizeInBytes), colors, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);

            //normaly
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[2]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normals.Length * Vector3.SizeInBytes), normals, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);

            //indices
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, VBO[3]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(int) * Indexes.Length), Indexes, BufferUsageHint.StaticDraw);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(int) * Indices.Count), Indices.ToArray(), BufferUsageHint.StaticDraw);
            //GL.Enable(EnableCap.PrimitiveRestart);
            //GL.PrimitiveRestartIndex(coords.Count);

            if(shaderOption)
            {
                //per pixel
                if (!VertexShader.LoadShaderS(Kocka.Properties.Resources.perPixelShaderVert, ShaderType.VertexShader))
                    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");
                if (!FragmentShader.LoadShaderS(Kocka.Properties.Resources.perPixelShaderFrag, ShaderType.FragmentShader))
                    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");
            }
            else
            {
                //per fragment
                if (!VertexShader.LoadShaderS(Kocka.Properties.Resources.perFragmentShaderVert, ShaderType.VertexShader))
                    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");
                if (!FragmentShader.LoadShaderS(Kocka.Properties.Resources.perFragmentShaderFrag, ShaderType.FragmentShader))
                    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");
            }

            spMain.CreateProgram();
            spMain.AddShaderToProgram(VertexShader);
            spMain.AddShaderToProgram(FragmentShader);
            spMain.LinkProgram();
            spMain.UseProgram();

            spMain.SetUniform("projectionMatrix", projectionMatrix);
            spMain.SetUniform("eye", new Vector3(0.0f, 0.0f, 3.5f));//dorobit pri zmene pozicie kamery update
            material.SetMaterialUniforms(spMain);
            light.SetDirectionalLightUniforms(spMain);
        }

        private void ScaleHeights(float value)
        {
            label label = new label(SetToolStripLabel);
            progres progres = new progres(SetProgressBar);
            form.statusStrip1.Invoke(label, "Prebieha škálovanie výšok...");
            LinearFunction map_z = new LinearFunction(min, max, -1 * value / 50.0f, 1 * value / 50.0f);

            int d = zaloha.Length / 100;
            for (int i = 0; i < zaloha.Length; i++)
            {
                //coords[i] = new Vector3(zaloha[i].X, zaloha[i].Y, zaloha[i].Z / (value * this.value));
                coords[i] = new Vector3(zaloha[i].X, zaloha[i].Y, map_z.Value(zaloha[i].Z));
                if (i % d == 0)
                    form.statusStrip1.Invoke(progres, i * 100 / zaloha.Length);
            }
        }

        private void GeoToCartesianCoords()
        {
            label label = new label(SetToolStripLabel);
            progres progres = new progres(SetProgressBar);
            form.statusStrip1.Invoke(label, "Prebieha prepočet koordinátov...");
            int d = coords.Count / 100;
            //pomocné premenné
            float rH, cosBxRAD, cosLxRAD, sinLxRAD, sinBxRAD;
            for (int i = 0; i < coords.Count; i++)
            {
                //R je polomer
                //RAD = Pi/180
                rH = R + coords[i].Z;
                cosBxRAD = (float)Math.Cos(coords[i].X * RAD);
                sinBxRAD = (float)Math.Sin(coords[i].X * RAD);
                cosLxRAD = (float)Math.Cos(coords[i].Y * RAD);
                sinLxRAD = (float)Math.Sin(coords[i].Y * RAD);
                coords[i] = new Vector3(rH * cosBxRAD * cosLxRAD, rH * cosBxRAD * sinLxRAD, rH * sinBxRAD);
                if (i % d == 0)
                    form.statusStrip1.Invoke(progres, i * 100 / coords.Count);
            }
        }

        private void GetNumberOfTriangles()
        {
            //for (int i = (NumOfParallels - 1) / 2; i >= 1; i--)
            for (int i = 1; i <= NumOfParallels / 2; i++)
                NumOfTriangles += 8 * (2 * i - 1);

            //NumOfTriangles = -2 * (NumOfParallels - 1) + 8 * NumOfTriangles;
            //NumOfTriangles *= 2;//dve polgule
            System.Diagnostics.Debug.WriteLine("NumOfTriangles = {0}", NumOfTriangles);
        }

        #region ovladanie mysou + rescale
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
            if (Status)
            {
                width = w; height = h;
                SetMatrices(true);
                colorScale.ResizeScale(w, h);
                spMain.UseProgram();
                spMain.SetUniform("projectionMatrix", projectionMatrix);
                spMain.SetUniform("modelViewMatrix", Current);
            }
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

        public void MoveCamera(float dd)
        {
            BigBrother.MoveCamera(dd);
            modelViewMatrix = BigBrother.ReturnCamera();
            Current = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix) * (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            spMain.SetUniform("modelViewMatrix", Current);
        }

        public void Rescale(float value)
        {
            this.Delete(false);
            VBO = new int[4];
            VAO = new int[1];
            Status = false;
            form.SetBoolean_sfera(false);
            var bw = new System.ComponentModel.BackgroundWorker();

            bw.DoWork += (sender, args) =>
            {
                ScaleHeights(value);
                GeoToCartesianCoords();
                CalculateNormals();
            };

            bw.RunWorkerCompleted += (sender, args) =>
            {
                if (args.Error != null)
                    System.Windows.Forms.MessageBox.Show(args.Error.ToString());
                InitScene(true);
                FirstDraw();
                Status = true;
                form.SetBoolean_sfera(false);
            };

            bw.RunWorkerAsync();
        }

        public void SetColorScaleOption(bool b)
        {
            colrscl = b;
        }

        public void SetWhatToDraw(int what)
        {
            //sem by mohla prist kontrola ci je what z {1,2,3}
            WhatToDraw = what;
        }

        public void ResetViewport()
        {
            SetMatrices(false);
            FirstDraw();
        }

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

        private void DrawNormals()
        {
            GL.Begin(PrimitiveType.Lines);
            GL.LineWidth(1.0f);

            for (int i = 0; i < coords.Count; i++)
            {
                Vector3 start = coords[i];
                Vector3 end = coords[i] + 0.01f * normals[i].Normalized();
                GL.Color3(1.0f, 1.0f, 1.0f);
                GL.Vertex3(start);
                GL.Color3(1.0f, 1.0f, 1.0f);
                GL.Vertex3(end);
            }
            GL.End();
        }

        public void DrawSphere()
        {
            //DrawNormals();
            spMain.UseProgram();
            GL.BindVertexArray(VAO[0]);
            GL.Enable(EnableCap.CullFace);//toto by nemuselo byt zle rozbehat
            GL.CullFace(CullFaceMode.Back);//zrejme by to urychlilo vykreslovanie
            switch (WhatToDraw)
            {
                case 1:
                    GL.DrawElements(PrimitiveType.Triangles, Indexes.Length, DrawElementsType.UnsignedInt, 0);
                    break;
                case 2:
                    GL.DrawElements(PrimitiveType.LineStrip, Indexes.Length, DrawElementsType.UnsignedInt, 0);
                    break;
                case 3:
                    GL.DrawElements(PrimitiveType.Points, Indexes.Length, DrawElementsType.UnsignedInt, 0);
                    break;
                default:
                    break;
            }
            GL.BindVertexArray(0);
            //GL.Disable(EnableCap.CullFace);

            if (colrscl)
            {
                colorScale.DrawColorScale();
                //colorScale.DrawText();
                spMain.UseProgram();
            }
        }

        public void ChangeMaterialProperties(float amb, float spec, float diff, int shin)
        {
            material = new Material(amb, spec, diff, shin);
            material.SetMaterialUniforms(spMain);
        }

        private void SetMatrices(bool co)
        {
            //prvotny resize
            if (!co)
            {
                scale = 1.0f;
                modelViewMatrix = Matrix4.LookAt(0.0f, 0.0f, 3.5f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
                projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, width / (float)height, 0.01f, 300.0f);
                //projectionMatrix = Matrix4.CreateOrthographic(10, 10, -100, 100);

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

        public void Delete(bool b)
        {
            //zrejme by tu tiez mohla prist kontrola, ci znicenie programov prebehlo v poriadku, resp. ju zakomponovat do Shaders.dll
            if (b)
            {
                GL.DeleteBuffers(4, VBO);
                GL.DeleteVertexArrays(1, VAO);
                coords.Clear();
                
                //zle
                spMain.DeleteProgram();
                VertexShader.DeleteShader();
                FragmentShader.DeleteShader();
                //
                colorScale.Delete();
            }
            else
            {
                GL.DeleteBuffers(4, VBO);
                GL.DeleteVertexArrays(1, VAO);
            }
        }

        public delegate void progres(int value);
        public delegate void label(string label);

        private void SetProgressBar(int v)
        {
            toolStripBar.Value = v;
        }
        private void SetToolStripLabel(string s)
        {
            toolStripLabel.Text = s;
        }
    }
}
