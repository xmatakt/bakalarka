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
        private string file;//vstupny file od uzivatela
        private string gradFile;
        private int width, height, texWidth, texHeight;
        private int vol_max;
        private float scale, stepSize, AlphaReduce;
        private Shaders.Shader bfVertShader, bfFragShader, rcVertShader, rcFragShader;
        private Shaders.ShaderProgram spMain;
        private int VAO, indicesID, verticesID;
        private int bfTexID, tffTexID, volTexID, frameBufferID, depthBufferID;
        private Matrix4 modelViewMatrix, projectionMatrix;
        private Matrix4 Current, ScaleMatrix, TranslationMatrix, RotationMatrix,
            MatrixStore_Translations, MatrixStore_Rotations, MatrixStore_Scales;
        private bool shaded;//pre volbu uzivatela, ci chce tienovanie
        private bool gradient;//ci normaly treba pocitat, alebo nie
        private TransferFunction transferFunction;
        private List<KeyValuePair<byte,int>> IsoTable;
        private Form1 form;

        public Volume(string pathToFile, int w, int h, bool shaded, Form1 form)
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

            this.shaded = shaded;
            this.form = form;
            file = pathToFile;
            width = texWidth = w;
            height = texHeight = h;
            vol_max = 0;
            gradient = false;

            stepSize = 0.001f;
            AlphaReduce = 0.5f;

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
            bfVertShader = new Shaders.Shader();
            bfFragShader = new Shaders.Shader();
            rcFragShader = new Shaders.Shader();
            rcVertShader = new Shaders.Shader();
            spMain = new Shaders.ShaderProgram();
            spMain.CreateProgram();

            //backface shaders
            if (!bfVertShader.LoadShaderS(VolumeRendering.Properties.Resources.bfVert, ShaderType.VertexShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");
            if (!bfFragShader.LoadShaderS(VolumeRendering.Properties.Resources.bfFrag, ShaderType.FragmentShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");

            //ray-casting shaders
            if (!rcVertShader.LoadShaderS(VolumeRendering.Properties.Resources.rcVert, ShaderType.VertexShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");

            //dalsi nateraz finalny
            if (shaded)
            {
                if (!rcFragShader.LoadShaderS(VolumeRendering.Properties.Resources.rcCustomTffShadedFrag, ShaderType.FragmentShader))
                    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");
            }
            else
                if (!rcFragShader.LoadShaderS(VolumeRendering.Properties.Resources.rcCustomTffFrag, ShaderType.FragmentShader))
                    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");
        }

        #region inicializacia textur a framebuffra

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
            vol_max = max;
            byte[] data = new byte[count];

            try
            {
                form.AddTextToOutput("Loading volume data...");
                BinaryReader br = new BinaryReader(File.Open(pathToFile, FileMode.Open));
                br.Read(data, 0, count);
                br.Close();

                //musim vyriesit aj pre .vtk
                //SetIsoTable(data);

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
            }
            catch (System.IO.FileNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show("File can't be found: " + Path.GetFileName(pathToFile));
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Error occurred while reading file: " + Path.GetFileName(pathToFile));
            }

            try
            {
                //toto je tu v pripade ze netreba centrovat zbytocne a zabera cas, ale koli prehladnejsiumu kodu som zrusil if(tmp_dz != 0):
                //centrovanie - ale bude fungovat iba ak je odlisna iba hodnota hlbky
                byte[] centrovane = new byte[max * max * max];
                int tmp_dz = max - d;
                int dolna_hranica = tmp_dz / 2;
                int counter = 0;

                for (int k = dolna_hranica; k < dolna_hranica + d; k++)
                    for (int j = 0; j < h; j++)
                        for (int i = 0; i < w; i++)
                        {
                            centrovane[i + w * (j + h * k)] = data[counter++];
                        }

                //ak si uzivatel zvolil zobrazovanie bez shadingu
                if (!shaded)
                {
                    form.AddTextToOutput("Data was successfully loaded.");
                    GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Intensity, max, max, max, 0, PixelFormat.Luminance, PixelType.UnsignedByte, centrovane);
                }
                   

                //ak si uzivatel zvolil zobrazovanie so shadingom
                else
                {
                     Half[] arr;
                    //neexistuje file s napocitanymi hodnotami
                    if(!gradient)
                    {
                        arr = SetVolumeData(centrovane);
                        MakeGradFile(arr);
                    }
                        
                    else
                    {
                        //System.Windows.Forms.MessageBox.Show("ahahho");
                        arr = ReadGradFile(gradFile);
                    }

                    form.AddTextToOutput("Data was successfully loaded.");
                    GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Rgba8, max, max, max, 0, PixelFormat.Rgba, PixelType.HalfFloat, arr);
                }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Vyskytla sa neocakavana chyba pri tvorbe 3D textury.");
            }
        }

        private void InitVol3DTex(string pathToFile)
        {
            if (pathToFile.EndsWith(".vtk"))
            {
                VtkReader reader = new VtkReader();
                int max = 0;
                byte[] data;
                try
                {
                    form.AddTextToOutput("Loading volume data...");
                    data = reader.ReadVTK(pathToFile);
                    vol_max = max = reader.Max();

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

                    //ak si uzivatel zvolil zobrazovanie bez shadingu
                    if (!shaded)
                    {
                        form.AddTextToOutput("Data was successfully loaded.");
                        GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Intensity, max, max, max, 0, PixelFormat.Luminance, PixelType.UnsignedByte, data);
                    }

                    //ak si uzivatel zvolil zobrazovanie so shadingom
                    else
                    {
                        Half[] VolumeData = SetVolumeData(data);
                        form.AddTextToOutput("Data was successfully loaded.");
                        GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Rgba8, max, max, max, 0, PixelFormat.Rgba, PixelType.HalfFloat, VolumeData);
                    }

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
               
                //string path = pathToFile.Remove(pathToFile.IndexOf(,));

                int[] dimensions = new int[3];
                string rawFile = LoadTsfFile(pathToFile, dimensions);//zistim rozmery a nazor .raw suboru

                string tsfFile = Path.GetFileName(pathToFile);//vrati nazov_suboru.tsf
                rawFile = pathToFile.Replace(tsfFile, rawFile);//.raw subor musi byt v tom istom foldri ako .tsf
                if(gradient)
                    gradFile = pathToFile.Replace(tsfFile, gradFile);//vystupom bude cesta + gradFile 
                InitVol3DTex(rawFile, dimensions[0], dimensions[1], dimensions[2]);
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

        #endregion

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

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture1D, tffTexID);
            spMain.SetUniform("TransferFunc", 0);
            spMain.SetUniform("AlphaReduce", AlphaReduce);

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

        #region nacitavanie a zapisovanie suborov

        private string LoadTsfFile(string pathToFile, int[] dimensions)
        {
            string rawFile = "";
            char[] separator = { ' ' };
            StreamReader sr = new StreamReader(pathToFile);
            string firstLine = sr.ReadLine();

            if (firstLine.Contains("Dimensions:"))
            {
                string[] arr = firstLine.Split(separator);
                try
                {
                    dimensions[0] = int.Parse(arr[1]);
                    dimensions[1] = int.Parse(arr[2]);
                    dimensions[2] = int.Parse(arr[3]);
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

            if (sr.EndOfStream)
                firstLine = "";
            else
                firstLine = sr.ReadLine();

            if (firstLine.Contains("grad file:"))
            {
                string[] arr = firstLine.Split(separator);
                try
                {
                    if (gradient = arr[2].Contains(".grad"))
                        gradFile = arr[2];
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
            return rawFile;
        }

        private Half[] ReadGradFile(string pathToFile)
        {
            form.AddTextToOutput("Reading "+Path.GetFileName(pathToFile)+"...");

            Half[] tmp_grad = new Half[vol_max * vol_max * vol_max * 4];

            if(File.Exists(pathToFile))
            {
                try
                {
                    BinaryReader br = new BinaryReader(File.Open(pathToFile, FileMode.Open));
                    for (int i = 0; i < tmp_grad.Length; i++)
                        tmp_grad[i] = (Half)br.ReadSingle();
                    br.Close();
                    form.AddTextToOutput("Done.");
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("File " + Path.GetFileName(pathToFile) + " is corrupted!", "Vnimanie!",
                          System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    form.AddTextToOutput("Can't load " + Path.GetFileName(pathToFile) + " file.");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("File " + Path.GetFileName(pathToFile) + " doesn't exist!", "Vnimanie!",
                           System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return tmp_grad;
        }

        private void MakeGradFile(Half[] data)
        {
            //nazvem ho tak isto, ako sa vola .tsf file, iba s priponou .grad
            string tsfFile = Path.GetFileName(file);
            string gradfile = tsfFile.Replace(".tsf",".grad");
            gradfile = file.Replace(tsfFile,gradfile);

            //mozem zacat zapisovat
            using (MsgBox msg = new MsgBox("Do you want to make .grad file?", "Vnimanie!"))
            {
                System.Windows.Forms.DialogResult diagres = msg.ShowDialog();
                if (diagres == System.Windows.Forms.DialogResult.Yes)
                {
                    form.AddTextToOutput("Creating " + Path.GetFileName(gradfile) + " file...");

                    BinaryWriter bw = new BinaryWriter(File.Open(gradfile,FileMode.Create));
                    //StreamWriter sw = new StreamWriter(gradfile);
                    for (int i = 0; i < data.Length; i++)
                        bw.Write(data[i]);
                        //sw.WriteLine(data[i++] + " " + data[i++] + " " + data[i++] + " " + data[i]);
                    bw.Flush();
                    bw.Close();
                    //uz len zapisat do .tsf suboru nazov .grad suboru
                    StreamWriter sw = new StreamWriter(file,true);
                    sw.WriteLine("");
                    sw.WriteLine("grad file: " + Path.GetFileName(gradfile));
                    sw.Flush();
                    sw.Close();

                    form.AddTextToOutput("File " + Path.GetFileName(gradfile) + " was successfully created.");
                    form.AddTextToOutput("File " + Path.GetFileName(file) + " was edited.");
                }
                else if (diagres == System.Windows.Forms.DialogResult.No)
                    return;
            }
        }

        #endregion

        #region ovladanie

        public void Rotate(float x, float y, float angle)
        {
            RotationMatrix = Matrix4.CreateFromAxisAngle(new Vector3(y, x, 0.0f), angle);

            Current = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix) * (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            //spMain.SetUniform("modelViewMatrix", Current);
        }

        public void Scale(float s)
        {
            scale = s;
            ScaleMatrix = Matrix4.CreateScale(scale, scale, scale);
            Current = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix) * (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            //spMain.SetUniform("modelViewMatrix", Current);
        }

        public void Transalte(float x, float y)
        {
            TranslationMatrix = Matrix4.CreateTranslation(x, y, 0.0f);
            Current = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix) * (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            //spMain.SetUniform("modelViewMatrix", Current);
        }

        public void SetAlphaReduce(float AlphaReduce) { this.AlphaReduce = AlphaReduce; }

        public void SetStepSize(float step) { stepSize = step; }

        public void Ende()
        {
            MatrixStore_Translations = MatrixStore_Translations * TranslationMatrix;
            MatrixStore_Rotations = MatrixStore_Rotations * RotationMatrix;

            //spMain.SetUniform("modelViewMatrix", Current);
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
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture1D, tffTexID);
            spMain.SetUniform("TransferFunc", 0);
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
                System.Windows.Forms.MessageBox.Show("Vyskytla neocakavana chyba pri nastavovani prenosovej funkcie!");
            }

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture1D, tffTexID);
            spMain.SetUniform("TransferFunc", 0);
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

        #region funkcie pre shading

        private void GenerateGradients(int sampleSize, Half[] VolumeData)
        {
            form.AddTextToOutput("Calculating gradients...");
            form.AddTextToOutput("0%");

            int n = sampleSize;
            Vector3 normal = new Vector3(0.0f);
            Vector3 s1, s2;
            int len = VolumeData.Length/10;

            int index = 0;
            for (int z = 0; z < vol_max; z++)
            {
                for (int y = 0; y < vol_max; y++)
                {
                    for (int x = 0; x < vol_max; x++)
                    {
                        s1.X = SampleVolume(x - n, y, z, VolumeData);
                        s2.X = SampleVolume(x + n, y, z, VolumeData);
                        s1.Y = SampleVolume(x, y - n, z, VolumeData);
                        s2.Y = SampleVolume(x, y + n, z, VolumeData);
                        s1.Z = SampleVolume(x, y, z - n, VolumeData);
                        s2.Z = SampleVolume(x, y, z + n, VolumeData);

                        normal = Vector3.Normalize(s2 - s1);
                        VolumeData[index] = (Half)normal.X;//0
                        VolumeData[index + 1] = (Half)normal.Y;//1
                        VolumeData[index + 2] = (Half)normal.Z;//2

                        //Gradients[index++] = Vector3.Normalize(s2 - s1);
                        if (float.IsNaN(normal.X))
                        {
                            VolumeData[index] = (Half)0.0f;
                            VolumeData[index + 1] = (Half)0.0f;
                            VolumeData[index + 2] = (Half)0.0f;
                        }
                        index += 4;
                        if (index % len == 0)
                            form.RewriteLastItem(10*(index/len)+"%");
                    }
                }
            }
            form.AddTextToOutput("100%");
            form.AddTextToOutput("Done.");
        }

        private float SampleVolume(int x, int y, int z, Half[] isoValues)
        {
            x = (int)MathHelper.Clamp(x, 0, vol_max - 1);
            y = (int)MathHelper.Clamp(y, 0, vol_max - 1);
            z = (int)MathHelper.Clamp(z, 0, vol_max - 1);

            return isoValues[(x + (y * vol_max) + (z * vol_max * vol_max)) * 4 + 3];
        }

        private Half[] SetVolumeData(byte[] centrovaneData)
        {
            form.AddTextToOutput("Loading isovalues...");
            //pre potreby shadingu budu data ulozene v poli Half[](nx,ny,nz,isovalue)
            //cize obdoba pola pre 1D texturu. nx,ny,nz su zlozky normaly
            Half[] VolumeData = new Half[vol_max * vol_max * vol_max * 4];

            int max_val = centrovaneData.Max();

            int counter = 0;
            for (int i = 3; i < VolumeData.Length; i += 4)
                VolumeData[i] = (Half)(centrovaneData[counter++] / (float)max_val);
            
            form.AddTextToOutput("Done.");

            GenerateGradients(2, VolumeData);
            return VolumeData;
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
