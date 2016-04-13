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
        private int width, height, NumOfVertices, NumOfIndexes, Xwidth, Ywidth, WhatToDraw;
        private Vector3[] vertices;
        private Vector3[] normals;
        private Vector3[] color;
        private List<int> Indices;
        private int[] Indexes;
        private List<Vector3> coords;
        private ColorScale colorScale;

        private float scale, min, max, minX, maxX, minY, maxY, angleX, angleY;
        private bool Status, colrscl, rotate, shaderOption;
        private int[] VBO;
        private int[] VAO;
        private Shaders.Shader VertexShader, FragmentShader;
        private Shaders.ShaderProgram spMain;
        private Matrix4 modelViewMatrix, projectionMatrix;
        private Matrix4 Current, ScaleMatrix, TranslationMatrix, RotationMatrix, MatrixStore_Translations, MatrixStore_Rotations, MatrixStore_Scales;
        DirectionalLight light;
        Material material;
        Form1 form;

        System.Windows.Forms.ToolStripProgressBar toolStripBar;
        System.Windows.Forms.ToolStripLabel toolStripLabel;

        public Surface(int w, int h, string pathToFile, System.Windows.Forms.ToolStripProgressBar bar = null, System.Windows.Forms.ToolStripLabel label = null, Form1 form = null, bool shaderOption = false)
        {
            this.shaderOption = shaderOption;
            this.form = form;
            toolStripBar = bar;
            toolStripLabel = label;
            WhatToDraw = 1;
            Status = colrscl = rotate = false;
            minX = minY = min = float.MaxValue;
            maxX = maxY = max = float.MinValue;
            scale = 1.0f;
            angleX = angleY = 0.0f;
            width = w; height = h;
            NumOfIndexes = NumOfVertices = Xwidth = Ywidth = 0;
            //svetlo - smer,ambient,specular,diffuse
            light = new DirectionalLight(new Vector3(0.0f, 0.0f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f));
            //material - ambient,specular,diffuse,koeficienty - ambient, specular, diffuse, shininess 
            material = new Material(0.29f, 0.86f, 0.57f, 128);
            //BigBrother = new Kamera(new Vector3(0.0f, 0.0f, 3.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));

            coords = new List<Vector3>();
            VBO = new int[4];
            VAO = new int[1];
            VertexShader = new Shaders.Shader();
            FragmentShader = new Shaders.Shader();
            spMain = new Shaders.ShaderProgram();


            var bw = new System.ComponentModel.BackgroundWorker();

            bw.DoWork += (sender, args) =>
            {
                // do your lengthy stuff here -- this will happen in a separate thread
                LoadData(pathToFile);
                if (Status = SetWidth())
                {
                    vertices = new Vector3[NumOfVertices];
                    color = new Vector3[NumOfVertices];
                    normals = new Vector3[NumOfVertices];
                    Indices = new List<int>();
                    Indexes = new int[NumOfIndexes];
                    //SetIndices();
                    SetIndexes();
                    if (Ywidth > Xwidth)
                    {
                        rotate = true;
                        MatrixStore_Rotations = Matrix4.CreateFromAxisAngle(new Vector3(0.0f, 0.0f, 1.0f), -90.0f * (float)Math.PI / 180.0f);
                    }
                }
                else
                {
                    //dorobit sem este resetovanie progressBaru/labelu
                    System.Windows.Forms.MessageBox.Show("Súbor " + pathToFile + " nemá podporu!", "Vnimanie!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    label lab = new label(SetToolStripLabel);
                    progres progres = new progres(SetProgressBar);
                    form.statusStrip1.Invoke(lab, "Súbor nemá podporu...");
                    form.statusStrip1.Invoke(progres, 0);
                }
            };

            bw.RunWorkerCompleted += (sender, args) =>
            {
                if (args.Error != null)  // if an exception occurred during DoWork,
                    System.Windows.Forms.MessageBox.Show(args.Error.ToString());  // do your error handling here

                if (Status)
                {
                    colorScale = new ColorScale(min, max, width, height);
                    CalculateColor();
                    ScaleHeights(1.0f);
                    CalculateNormals();
                    InitScene(false);
                    FirstDraw();
                    form.SetBoolean_surface(true);
                }
                else
                    form.SetMenuStrip_Enabled(true);
            };

            bw.RunWorkerAsync(); // starts the background worker
        }

        #region nacitanie/nastavenie plochy a fsetkeho potrebneho

        private void InitScene(bool b)
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
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(int) * Indices.Count), Indices.ToArray(), BufferUsageHint.StaticDraw);
            GL.Enable(EnableCap.PrimitiveRestart);
            GL.PrimitiveRestartIndex(coords.Count);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(int) * Indexes.Length), Indexes, BufferUsageHint.StaticDraw);

            if (shaderOption)
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
            spMain.SetUniform("eye", new Vector3(0.0f, 0.0f, 30.0f));
            material.SetMaterialUniforms(spMain);
            light.SetDirectionalLightUniforms(spMain);
        }

        private void LoadData(string pathToFile)
        {
            label label = new label(SetToolStripLabel);
            progres progres = new progres(SetProgressBar);
            form.statusStrip1.Invoke(label, "Prebieha načítavanie dát...");
            int count = File.ReadLines(pathToFile).Count();
            int d = count / 100;
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
                    if (coords.Last().X < minX)
                        minX = coords.Last().X;
                    if (coords.Last().X > maxX)
                        maxX = coords.Last().X;
                    if (coords.Last().Y < minY)
                        minY = coords.Last().Y;
                    if (coords.Last().Y > maxY)
                        maxY = coords.Last().Y;
                    if (coords.Last().Z < min)
                        min = coords.Last().Z;
                    if (coords.Last().Z > max)
                        max = coords.Last().Z;

                    if (coords.Count % d == 0)
                        form.statusStrip1.Invoke(progres, 100 * coords.Count / count);
                }
                sr.Close();
            }
            catch (FileNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show("Subor sa nenasiel!");
            }
        }

        public bool Loaded()
        {
            return Status;
        }

        #region nastavovanie bazmekov

        private bool SetWidth()
        {
            int p = 0;
            while (coords[p].X == coords[p + 1].X)
                p++;
            p++;

            if (p == 1)
                return false;
            else
            {
                NumOfVertices = coords.Count;
                Ywidth = p;
                Xwidth = NumOfVertices / p;
                //NumOfIndices=(pocetRiadkov-1)*pocetIndicisNaRiadok + pocetRestartov
                NumOfIndexes = (Xwidth - 1) * (Ywidth - 1) * 2 * 3;
                return true;
            }
        }

        private void SetIndexes()
        {
            int p = 0;
            for (int x = 0; x < Xwidth - 1; x++)
            {
                for (int y = 0; y < Ywidth - 1; y++)
                {
                    int pos1 = x * Ywidth + y;//0
                    int pos2 = (x + 1) * Ywidth + y;//4
                    int pos3 = x * Ywidth + y + 1;//1
                    int pos4 = (x + 1) * Ywidth + y + 1;//5

                    Indexes[p] = pos1; p++; Indices.Add(pos1);
                    Indexes[p] = pos3; p++; Indices.Add(pos3);
                    Indexes[p] = pos4; p++; Indices.Add(pos4);
                    Indexes[p] = pos2; p++; Indices.Add(pos2);
                    Indexes[p] = pos1; p++; Indices.Add(pos1);
                    Indexes[p] = pos4; p++; Indices.Add(pos4);

                    if (pos4 % Ywidth == Ywidth - 1)
                        Indices.Add(coords.Count);
                }
            }
        }

        private void CalculateNormals()
        {
            label label = new label(SetToolStripLabel);
            progres progres = new progres(SetProgressBar);
            form.statusStrip1.Invoke(label, "Prebieha výpočet normál...");
            int d = NumOfIndexes / 300;

            for (int i = 0; i < NumOfIndexes / 3; i++)
            {
                int index1 = Indexes[i * 3];
                int index2 = Indexes[i * 3 + 1];
                int index3 = Indexes[i * 3 + 2];

                Vector3 side1 = vertices[index1] - vertices[index3];
                Vector3 side2 = vertices[index2] - vertices[index1];
                Vector3 normal = Vector3.Cross(side1, side2);

                normals[index1] += normal;
                normals[index2] += normal;
                normals[index3] += normal;
                if (i % d == 0)
                {
                    float tmp = (3 * i / (float)NumOfIndexes);
                    form.statusStrip1.Invoke(progres, (int)(100 * tmp));
                }
            }
        }

        private void DrawNormals()
        {
            GL.Begin(PrimitiveType.Lines);
            GL.LineWidth(1.0f);

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 start = vertices[i];
                Vector3 end = vertices[i] + 0.01f * normals[i].Normalized();
                GL.Color3(1.0f, 0.0f, 0.0f);
                GL.Vertex3(start);
                GL.Color3(1.0f, 0.0f, 0.0f);
                GL.Vertex3(end);
            }
            GL.End();
        }

        private void CalculateColor()
        {
            label label = new label(SetToolStripLabel);
            progres progres = new progres(SetProgressBar);
            form.statusStrip1.Invoke(label, "Prebieha nastavovanie farieb...");
            color = colorScale.SetColorList(coords).ToArray();
            //for (int i = 0; i < NumOfVertices; i++)
            //color[i] = CalculateColor(coords[i].Z);
        }

        private void ScaleHeights(float L)
        {
            label label = new label(SetToolStripLabel);
            progres progres = new progres(SetProgressBar);
            form.statusStrip1.Invoke(label, "Prebieha škálovanie výšok...");
            LinearFunction map_z;
            LinearFunction percenta = new LinearFunction(1.0f, 50.0f, 0.5f, 0.01f);
            if((maxX-minX)>(maxY-minY))
                map_z = new LinearFunction(min, max, 0, (maxX - minX) * percenta.Value(L));
            else
                map_z = new LinearFunction(min, max, 0, (maxY - minY) * percenta.Value(L));

            int d = coords.Count / 100;
            float dx = minX + (maxX - minX) / 2.0f;
            float dy = minY + (maxY - minY) / 2.0f;
            float dz = min / L + (max - min) / (L * 2.0f);
            float dl = max - min;
            L = L / 10;

            for (int i = 0; i < coords.Count; i++)
            {
                //vertices[i] = new Vector3( -coords[i].X + dx, coords[i].Y - dy, coords[i].Z / (L * dl) - dz / (L * dl));
                vertices[i] = new Vector3(-coords[i].X + dx, coords[i].Y - dy, map_z.Value(coords[i].Z));
                if (i % d == 0)
                    form.statusStrip1.Invoke(progres, i * 100 / coords.Count);
            }
        }

        private void SetMatrices(bool co)
        {
            //prvotny resize
            if (!co)
            {
                modelViewMatrix = Matrix4.LookAt(0.0f, 0.0f, 50.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
                projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, width / (float)height, 0.01f, 300.0f);
                scale = 1.0f;
                ScaleMatrix = Matrix4.CreateScale(scale, scale, scale);
                TranslationMatrix = Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f);
                RotationMatrix = Matrix4.Identity;
                if (!rotate)
                    MatrixStore_Rotations = Matrix4.Identity;
                else
                    MatrixStore_Rotations = Matrix4.CreateFromAxisAngle(new Vector3(0.0f, 0.0f, 1.0f), -90.0f * (float)Math.PI / 180.0f);
                MatrixStore_Translations = Matrix4.Identity;
                MatrixStore_Scales = Matrix4.Identity;
                Current = Matrix4.Identity;
            }
            //resize vyvolany pouzivatelom
            else
                projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, width / (float)height, 0.01f, 300.0f);
        }
        #endregion
        #endregion

        private void SetToolStrip(int v)
        {
            toolStripBar.Value = v;
        }
        private void SetToolStrip(string s)
        {
            toolStripLabel.Text = s;
        }

        public void Rescale(float value)
        {
            this.Delete(false);
            VBO = new int[4];
            VAO = new int[1];
            Status = false;
            form.SetBoolean_surface(false);

            var bw = new System.ComponentModel.BackgroundWorker();
            bw.DoWork += (sender, args) =>
            {
                ScaleHeights(value);
                CalculateNormals();
            };

            bw.RunWorkerCompleted += (sender, args) =>
            {
                if (args.Error != null)
                    System.Windows.Forms.MessageBox.Show(args.Error.ToString());
                InitScene(true);
                //System.Windows.Forms.MessageBox.Show("rotateX = " + angleX +"\nrotateY = " + angleY);
                Rotate(angleX, angleY);
                //FirstDraw();
                Status = true;
                form.SetBoolean_surface(false);
            };
            bw.RunWorkerAsync();
        }

        #region ovladanie

        public void Transalte(float x, float y)
        {
            TranslationMatrix = Matrix4.CreateTranslation(x, y, 0.0f);
            Rotate(angleX, angleY);
        }

        public void Rotate(float angleX, float angleY)
        {
            this.angleX = angleX;
            this.angleY = angleY;
            RotationMatrix = Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.0f, 0.0f), angleY);
            RotationMatrix *= Matrix4.CreateFromAxisAngle(new Vector3(0.0f, 1.0f, 0.0f), angleX);
            Matrix4 mat = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix);
            Current = mat * (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            mat = mat.Inverted(); mat = Matrix4.Transpose(mat);
            GL.BindVertexArray(VAO[0]);
            spMain.SetUniform("normalMatrix", mat);
            spMain.SetUniform("modelViewMatrix", Current);
        }

        public void Scale(float s)
        {
            scale = s;
            ScaleMatrix = Matrix4.CreateScale(scale, scale, scale);
            Rotate(angleX, angleY);
        }

        public void ChangeMaterialProperties(float amb, float spec, float diff, int shin)
        {
            material = new Material(amb, spec, diff, shin);
            material.SetMaterialUniforms(spMain);
        }

        public void Resize(int w, int h)
        {
            width = w; height = h;
            SetMatrices(true);
            colorScale.ResizeScale(w, h);
            spMain.UseProgram();
            spMain.SetUniform("projectionMatrix", projectionMatrix);
            spMain.SetUniform("modelViewMatrix", Current);
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

        public void Ende()
        {
            MatrixStore_Translations = MatrixStore_Translations * TranslationMatrix;
            GL.BindVertexArray(VAO[0]);
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

        public void DrawSurface()
        {
            GL.BindVertexArray(VAO[0]);
            switch (WhatToDraw)
            {
                case 1:
                    GL.DrawElements(PrimitiveType.Triangles, Indices.Count, DrawElementsType.UnsignedInt, 0);
                    //GL.DrawElements(PrimitiveType.Triangles, NumOfIndexes, DrawElementsType.UnsignedInt, 0);
                    break;
                case 2:
                    GL.DrawElements(PrimitiveType.LineStrip, Indices.Count, DrawElementsType.UnsignedInt, 0);
                    //GL.DrawElements(PrimitiveType.LineStrip, NumOfIndexes, DrawElementsType.UnsignedInt, 0);
                    break;
                case 3:
                    GL.DrawElements(PrimitiveType.Points, Indices.Count, DrawElementsType.UnsignedInt, 0);
                    //GL.DrawElements(PrimitiveType.Points, NumOfIndexes, DrawElementsType.UnsignedInt, 0);
                    break;
                default:
                    break;
            }
            if (colrscl)
            {
                colorScale.DrawColorScale();
                spMain.UseProgram();
            }
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
        public delegate void menu(bool enabled);

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
