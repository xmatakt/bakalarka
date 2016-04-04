using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace VolumeRendering
{
    class Volume
    {
        private string file;
        private int width, height, texWidth, texHeight;
        private float scale, stepSize, AlphaReduce;
        private Shaders.Shader bfVertShader, bfFragShader, rcVertShader, rcFragShader;
        private Shaders.ShaderProgram spMain;
        private int VAO, indicesID, verticesID;
        private int bfTexID, tffTexID, volTexID, frameBufferID, depthBufferID;
        private Matrix4 modelViewMatrix, projectionMatrix;
        private Matrix4 Current, ScaleMatrix, TranslationMatrix, RotationMatrix, MatrixStore_Translations, MatrixStore_Rotations, MatrixStore_Scales;
        bool tff;
        private TransferFunction transferFunction;

        public Volume(string pathToFile, int w, int h)
        {
            List<Vector2> tmp2 = new List<Vector2>();
            tmp2.Add(new Vector2(0.0f, 0));
            tmp2.Add(new Vector2(0.0f, 40));
            tmp2.Add(new Vector2(0.2f, 60));
            tmp2.Add(new Vector2(0.05f, 63));
            tmp2.Add(new Vector2(0.0f, 80));
            tmp2.Add(new Vector2(0.9f, 82));
            tmp2.Add(new Vector2(1f, 256));


            List<Vector4> tmp4 = new List<Vector4>();
            tmp4.Add(new Vector4(.91f, .7f, .61f, 0));
            tmp4.Add(new Vector4(.91f, .7f, .61f, 80));
            tmp4.Add(new Vector4(1.0f, 1.0f, .85f, 82));
            tmp4.Add(new Vector4(1.0f, 1.0f, .85f, 256));

            transferFunction = new TransferFunction(tmp2, tmp4);


            file = pathToFile;
            width = texWidth = w;
            height = texHeight = h;

            //pri ciernobielom vykreslovani treba zlovilt mensi stepsize
            stepSize = 0.001f;
            AlphaReduce = 0.5f;

            //pre farebnicke obrazky
            //stepSize = 0.001f;

            bfVertShader = new Shaders.Shader();
            bfFragShader = new Shaders.Shader();
            rcFragShader = new Shaders.Shader();
            rcVertShader = new Shaders.Shader();
            spMain = new Shaders.ShaderProgram();
            spMain.CreateProgram();

            Current = modelViewMatrix = Matrix4.LookAt(0.0f, 0.0f, 2.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, width / (float)height, 0.01f, 300.0f);
            ScaleMatrix = Matrix4.CreateScale(scale, scale, scale);
            TranslationMatrix = Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f);
            RotationMatrix = Matrix4.Identity;
            MatrixStore_Rotations = Matrix4.Identity;
            MatrixStore_Translations = Matrix4.Identity;
            MatrixStore_Scales = Matrix4.Identity;

            Init();
        }

        private void Init()
        {
            InitVBO();
            InitShaders();
            InitTFF1DTex();
            InitFace2DTex(texWidth, texHeight);
            InitVol3DTex(file);
            InitFrameBuffer(bfTexID, texWidth, texHeight);
        }

        private void InitVBO()
        {
            float tmp = -0.0f;
            float[] vertices = 
             {
	            0.0f+tmp, 0.0f+tmp, 0.0f+tmp,
	            0.0f+tmp, 0.0f+tmp, 1.0f+tmp,
	            0.0f+tmp, 1.0f+tmp, 0.0f+tmp,
	            0.0f+tmp, 1.0f+tmp, 1.0f+tmp,
	            1.0f+tmp, 0.0f+tmp, 0.0f+tmp,
	            1.0f+tmp, 0.0f+tmp, 1.0f+tmp,
	            1.0f+tmp, 1.0f+tmp, 0.0f+tmp,
	            1.0f+tmp, 1.0f+tmp, 1.0f+tmp
             };
            // draw the six faces of the boundbox by drawing triangles
            // draw it contra-clockwise
            // front: 1 5 7 3
            // back: 0 2 6 4
            // left: 0 1 3 2
            // right:7 5 4 6    
            // up: 2 3 7 6
            // down: 1 0 4 5
            int[] indices = 
                {
	                1,5,7,
	                7,3,1,
	                0,2,6,
                    6,4,0,
	                0,1,3,
	                3,2,0,
	                7,5,4,
	                4,6,7,
	                2,3,7,
                    7,6,2,
                    1,0,4,
                    4,5,1
                };

            int[] gbo = new int[2];

            VAO = GL.GenVertexArray();
            GL.GenBuffers(2, gbo);
            verticesID = gbo[0];
            indicesID = gbo[1];

            GL.BindVertexArray(VAO);

            //pozicia + farba
            GL.BindBuffer(BufferTarget.ArrayBuffer, verticesID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(24 * sizeof(float)), vertices, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0); //for vertex location (pozicia je rovnaka ako farba)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);//for vertex color
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);

            //indices
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indicesID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(int) * 36), indices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(0);
        }

        private void InitShaders()
        {
            //backface shaders
            if (!bfVertShader.LoadShaderS(VolumeRendering.Properties.Resources.bfVert, ShaderType.VertexShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");
            if (!bfFragShader.LoadShaderS(VolumeRendering.Properties.Resources.bfFrag, ShaderType.FragmentShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");

            //ray-casting shaders
            if (!rcVertShader.LoadShaderS(VolumeRendering.Properties.Resources.rcVert, ShaderType.VertexShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");

            //farebne
            //if (!rcFragShader.LoadShaderS(VolumeRendering.Properties.Resources.rcFrag, ShaderType.FragmentShader))
            //    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");
            //tff = true;

            //cjernobjele
            //if (!rcFragShader.LoadShaderS(VolumeRendering.Properties.Resources.rcgsFrag, ShaderType.FragmentShader))
            //    //if (!rcFragShader.LoadShader("..\\..\\Properties\\data\\shaders\\raycastingNew.frag", ShaderType.FragmentShader))
            //    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");
            //tff = false;

            //dalsi nateraz finalny
            //string path = string.Format("..{0}..{0}Properties{0}data{0}shaders{0}raycastingTff.frag", Path.DirectorySeparatorChar);
            if (!rcFragShader.LoadShaderS(VolumeRendering.Properties.Resources.rcCustomTffFrag, ShaderType.FragmentShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");
            tff = true;
        }

        //inicializacia 1D textury pre transfer function
        private void InitTFF1DTex()
        {
            try
            {
                byte[] tff = transferFunction.GetTransferFunction();
                //vytvorim 1D texturu z tff[]
                tffTexID = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture1D, tffTexID);
                GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
                GL.TexImage1D(TextureTarget.Texture1D, 0, PixelInternalFormat.Rgba8, 256, 0, PixelFormat.Rgba, PixelType.UnsignedByte, tff);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Vyskytla sa chyba neocakavana chyba pri nastavovani prenosovej funkcie!");
            }
        }

        private void InitTFF1DTex(string pathToFile)
        {
            //zistim pocet - asi by nebolo zle to nahradit dacim inym koli strate casu, v povodnom
            // programe bola napevno dana velkost 10000
            try
            {
                int count = File.ReadAllBytes(pathToFile).Length;
                byte[] tff = new byte[count];

                BinaryReader br = new BinaryReader(File.Open(pathToFile, FileMode.Open));
                //naplnim tff[] 
                br.Read(tff, 0, count);
                br.Close();

                //vytvorim 1D texturu z tff[]
                tffTexID = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture1D, tffTexID);
                GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
                GL.TexImage1D(TextureTarget.Texture1D, 0, PixelInternalFormat.Rgba8, 256, 0, PixelFormat.Rgba, PixelType.UnsignedByte, tff);
            }
            catch (System.IO.FileNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa najst subor: " + pathToFile);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Vyskytla sa chyba pri nacitani suboru: " + pathToFile);
            }
        }

        //inicializacia 2D textury pre vykreslovanie backface-u
        private void InitFace2DTex(int texWidth, int texHeight)
        {
            bfTexID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, bfTexID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Nearest);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //nenaplna sa datami, do nej sa bude kreslit
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16f, texWidth, texHeight, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
        }

        // init 3D texture to store the volume data used for ray casting
        private void InitVol3DTex(string pathToFile, int w, int h, int d)
        {
            int max = 0;
            if (max <= w)
                max = w;
            if (max < h)
                max = h;
            if (max < d)
                max = d;

            int count = w * h * d;

            byte[] data = new byte[count];
            try
            {
                BinaryReader br = new BinaryReader(File.Open(pathToFile, FileMode.Open));
                br.Read(data, 0, count);
                br.Close();

                volTexID = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture3D, volTexID);
                //GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Nearest);
                //GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapR, (int)TextureWrapMode.Repeat);
                GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

                //centrovanie - ale bude fungovat iba ak je odlisna iba hodnota hlbky
                byte[] centrovane = new byte[max * max * max];
                int tmp_dz = max - d;

                //ak treba centrovat
                if (tmp_dz != 0)
                {
                    try
                    {
                        int dolna_hranica = tmp_dz / 2;
                        int counter = 0;
                        for (int k = dolna_hranica; k < dolna_hranica + d; k++)
                        {
                            for (int j = 0; j < h; j++)
                            {
                                for (int i = 0; i < w; i++)
                                {
                                    centrovane[i + w * (j + h * k)] = data[counter++];
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        System.Windows.Forms.MessageBox.Show("Chyba pri nacitacvanie RAW suboru. \nData sa nepodarilo vycentrovat.");
                    }
                    GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Intensity, max, max, max, 0, PixelFormat.Luminance, PixelType.UnsignedByte, centrovane);
                }
                // v pripade ze centrovat netreba
                else
                    GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Intensity, max, max, max, 0, PixelFormat.Luminance, PixelType.UnsignedByte, data);
            }
            catch (System.IO.FileNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa najst subor: " + pathToFile);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Vyskytla sa chyba pri nacitani suboru: " + pathToFile);
            }
        }

        private void InitVol3DTex(string pathToFile)
        {
            if (pathToFile.EndsWith(".vtk"))
            {
                VtkReader reader = new VtkReader();
                int max = 0;
                try
                {
                    byte[] data = reader.ReadVTK(pathToFile);
                    max = reader.Max();

                    volTexID = GL.GenTexture();
                    GL.BindTexture(TextureTarget.Texture3D, volTexID);
                    //GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Nearest);
                    //GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                    GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                    GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
                    GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapR, (int)TextureWrapMode.Repeat);
                    GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
                    GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Intensity, max, max, max, 0, PixelFormat.Luminance, PixelType.Byte, data);
                }
                catch (System.IO.FileNotFoundException)
                {
                    System.Windows.Forms.MessageBox.Show("Nepodarilo sa najst subor: " + pathToFile);
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Vyskytla sa chyba pri nacitani suboru: " + pathToFile);
                }
            }
            if (pathToFile.EndsWith(".tsf"))
            {
                int w, h, d; w = h = d = 0;
                string rawFile = "";
                char[] separator = { ' ' };
                StreamReader sr = new StreamReader(pathToFile);
                string firstLine = sr.ReadLine();

                if (firstLine.Contains("Dimensions:"))
                {
                    string[] arr = firstLine.Split(separator);
                    try
                    {
                        w = int.Parse(arr[1]);
                        h = int.Parse(arr[2]);
                        d = int.Parse(arr[3]);
                        System.Diagnostics.Debug.WriteLine(w + "," + h + "," + d);
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("Nespravny format suboru " + pathToFile, "Vnimanie!",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }

                firstLine = sr.ReadLine();
                if (firstLine.Contains("raw file:"))
                {
                    string[] arr = firstLine.Split(separator);
                    try
                    {
                        if (arr[2].Contains(".raw"))
                            rawFile = arr[2];
                        else
                            throw new Exception();
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("Nespravny format suboru " + pathToFile, "Vnimanie!",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                sr.Close();
                InitVol3DTex(rawFile, w, h, d);
            }

        }

        private void InitFrameBuffer(int texID, int texWidth, int texHeight)
        {
            // create a depth buffer for our framebuffer
            depthBufferID = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBufferID);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, texWidth, texHeight);

            // attach the texture and the depth buffer to the framebuffer
            frameBufferID = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferID);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, texID, 0);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthBufferID);

            GL.Enable(EnableCap.DepthTest);
        }

        // link the shader objects using the shader program
        private void LinkShader(int programID, int vertShaderID, int fragShaderID)
        {
            //najprv zistim, ake shadere su pripojene v shader programe a odpojim ich
            const int maxCount = 2;
            int count;
            int[] attachedShaders = new int[maxCount];
            GL.GetAttachedShaders(programID, maxCount, out count, attachedShaders);
            for (int i = 0; i < count; i++)
                GL.DetachShader(programID, attachedShaders[i]);

            //toto tu bolo ale neviem preco, po odstraneni sa nic nedeje
            // Bind index 0 to the shader input variable "VerPos"
            GL.BindAttribLocation(programID, 0, "VerPos");
            // Bind index 1 to the shader input variable "VerClr"
            GL.BindAttribLocation(programID, 1, "VerClr");

            //pripojim zvolene shadere a zlinkujem program
            spMain.AddShaderToProgram(vertShaderID);
            spMain.AddShaderToProgram(fragShaderID);
            spMain.LinkProgram();
        }

        private void SetUniforms()
        {
            spMain.SetUniform("ScreenSize", (float)width, (float)height);
            spMain.SetUniform("StepSize", stepSize);

            if (tff)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture1D, tffTexID);
                spMain.SetUniform("TransferFunc", 0);
                spMain.SetUniform("AlphaReduce", AlphaReduce);
            }

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, bfTexID);
            spMain.SetUniform("ExitPoints", 1);

            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture3D, volTexID);
            spMain.SetUniform("VolumeTex", 2);
        }

        private void DrawBox(CullFaceMode mode)
        {
            // --> Face culling allows non-visible triangles of closed surfaces to be culled before expensive Rasterization and Fragment Shader operations.
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(mode);
            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
            GL.Disable(EnableCap.CullFace);
            spMain.UseProgram(0);//zapnuty bol v Render() ktora DrawBox zavolala
        }

        private void Render(CullFaceMode mode)
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            spMain.UseProgram();
            spMain.SetUniform("modelViewMatrix", Current);
            spMain.SetUniform("projectionMatrix", projectionMatrix);
            DrawBox(mode);
        }

        public void Display()
        {
            // the color of the vertex in the back face is also the location
            // of the vertex
            // save the back face to the user defined framebuffer bound
            // with a 2D texture named `g_bfTexObj`
            // draw the front face of the box
            // in the rendering process, i.e. the ray marching process
            // loading the volume `g_volTexObj` as well as the `g_bfTexObj`
            // after vertex shader processing we got the color as well as the location of
            // the vertex (in the object coordinates, before transformation).
            // and the vertex assemblied into primitives before entering
            // fragment shader processing stage.
            // in fragment shader processing stage. we got `g_bfTexObj`
            // (correspond to 'VolumeTex' in glsl)and `g_volTexObj`(correspond to 'ExitPoints')
            // as well as the location of primitives.
            // the most important is that we got the GLSL to exec the logic. Here we go!

            // draw the back face of the box
            GL.Enable(EnableCap.DepthTest);

            //"vykreslim" front || back face objemu do framebuffru --> teda do 2D textury s ID bfTexID 
            //(pomocou backface.frag &.vert)
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferID);
            GL.Viewport(0, 0, width, height);
            LinkShader(spMain.GetProgramHandle(), bfVertShader.GetShaderHandle(), bfFragShader.GetShaderHandle());
            spMain.UseProgram();
            //cull front face
            Render(CullFaceMode.Front);
            spMain.UseProgram(0);
            //klasicky framebuffer --> "obrazovka"
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            GL.Viewport(0, 0, width, height);
            LinkShader(spMain.GetProgramHandle(), rcVertShader.GetShaderHandle(), rcFragShader.GetShaderHandle());
            spMain.UseProgram();
            SetUniforms();
            Render(CullFaceMode.Back);
            spMain.UseProgram(0);

            GL.Disable(EnableCap.DepthTest);
        }

        #region ovladanie

        public void Rotate(float x, float y, float angle)
        {
            RotationMatrix = Matrix4.CreateFromAxisAngle(new Vector3(y, x, 0.0f), angle);
            Current = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix) * (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            spMain.SetUniform("modelViewMatrix", Current);
        }

        public void Scale(float s)
        {
            scale = s;
            ScaleMatrix = Matrix4.CreateScale(scale, scale, scale);
            Current = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix) * (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            spMain.SetUniform("modelViewMatrix", Current);
        }

        public void Transalte(float x, float y)
        {
            TranslationMatrix = Matrix4.CreateTranslation(x, y, 0.0f);
            Current = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix) * (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            spMain.SetUniform("modelViewMatrix", Current);
        }

        public void SetAlphaReduce(float AlphaReduce) { this.AlphaReduce = AlphaReduce; }

        public void SetStepSize(float step) { stepSize = step; }

        public void Ende()
        {
            MatrixStore_Translations = MatrixStore_Translations * TranslationMatrix;
            MatrixStore_Rotations = MatrixStore_Rotations * RotationMatrix;

            spMain.SetUniform("modelViewMatrix", Current);
            ScaleMatrix = Matrix4.Identity;
            RotationMatrix = Matrix4.Identity;
            TranslationMatrix = Matrix4.Identity;
        }

        public void Resize(int w, int h)
        {
            width = texWidth = w;
            height = texHeight = h;
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, width / (float)height, 0.01f, 300.0f);
            GL.BindTexture(TextureTarget.Texture2D, bfTexID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16f, texWidth, texHeight, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.DeleteFramebuffer(frameBufferID);
            GL.DeleteRenderbuffer(depthBufferID);
            InitFrameBuffer(bfTexID, texWidth, texHeight);
        }

        public void ChangeOpacity(List<Vector2> list)
        {
            transferFunction.ChangeOpacity(list);
            try
            {
                byte[] tf = transferFunction.GetTransferFunction();
                GL.BindTexture(TextureTarget.Texture1D, tffTexID);
                //prepise data, nemusim vytvarat novu texturu a nicit staru
                GL.TexSubImage1D(TextureTarget.Texture1D, 0, 0, 256, PixelFormat.Rgba, PixelType.UnsignedByte, tf);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Vyskytla sa chyba neocakavana chyba pri nastavovani prenosovej funkcie!");
            }
            if (tff)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture1D, tffTexID);
                spMain.SetUniform("TransferFunc", 0);
            }
        }

        public void ChangeColors(List<Vector4> list)
        {
            transferFunction.ChangeColors(list);
            try
            {
                byte[] tf = transferFunction.GetTransferFunction();
                GL.BindTexture(TextureTarget.Texture1D, tffTexID);
                //prepise data, nemusim vytvarat novu texturu a nicit staru
                GL.TexSubImage1D(TextureTarget.Texture1D, 0, 0, 256, PixelFormat.Rgba, PixelType.UnsignedByte, tf);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Vyskytla sa chyba neocakavana chyba pri nastavovani prenosovej funkcie!");
            }
            if (tff)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture1D, tffTexID);
                spMain.SetUniform("TransferFunc", 0);
            }
        }

        public void SaveTransferFunction(string path)
        {
            byte[] arr = transferFunction.GetTransferFunction();
            BinaryWriter br = new BinaryWriter(new FileStream(path, FileMode.Create), Encoding.ASCII);
            for (int i = 0; i < arr.Length; i++)
                br.Write(arr[i]);
            br.Flush();
            br.Close();
        }

        public void LoadTransferFunction(string pathToFile)
        {
            try
            {
                int count = File.ReadAllBytes(pathToFile).Length;
                byte[] tff = new byte[count];

                BinaryReader br = new BinaryReader(File.Open(pathToFile, FileMode.Open));
                //naplnim tff[] 
                br.Read(tff, 0, count);
                br.Close();

                GL.BindTexture(TextureTarget.Texture1D, tffTexID);
                //prepise data, nemusim vytvarat novu texturu a nicit staru
                GL.TexSubImage1D(TextureTarget.Texture1D, 0, 0, 256, PixelFormat.Rgba, PixelType.UnsignedByte, tff);
            }
            catch (System.IO.FileNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa najst subor: " + pathToFile);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Vyskytla sa chyba pri nacitani suboru: " + pathToFile);
            }
        }

        #endregion

        public void Delete()
        {
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DeleteBuffer(verticesID);
            GL.DeleteBuffer(indicesID);
            GL.DeleteVertexArray(VAO);
            GL.DeleteTexture(bfTexID);
            GL.DeleteTexture(tffTexID);
            GL.DeleteTexture(volTexID);
            GL.DeleteRenderbuffer(depthBufferID);
            GL.DeleteFramebuffer(frameBufferID);
            //najprv detach
            bfFragShader.DeleteShader();
            bfVertShader.DeleteShader();
            rcFragShader.DeleteShader();
            rcVertShader.DeleteShader();
            spMain.DeleteProgram();
        }
    }
}
