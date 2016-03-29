﻿using System;
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
        private float scale, stepSize;
        private Shaders.Shader bfVertShader, bfFragShader, rcVertShader, rcFragShader;
        private Shaders.ShaderProgram spMain;
        private int VAO, indicesID, verticesID;
        private int bfTexID, tffTexID, volTexID, frameBufferID;
        private Matrix4 modelViewMatrix, projectionMatrix;
        private Matrix4 Current, ScaleMatrix, TranslationMatrix, RotationMatrix, MatrixStore_Translations, MatrixStore_Rotations, MatrixStore_Scales;

        public Volume(string pathToFile, int w, int h)
        {
            file = pathToFile;
            width = texWidth = w ;
            height = texHeight = h ;
            stepSize = 0.001f;
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
            InitTFF1DTex("tff.dat");
            InitFace2DTex(texWidth, texHeight);
            InitVol3DTex(file, 256, 256, 225);
            //InitVol3DTex("BostonTeapot.raw", 256, 256, 178);
            //InitVol3DTex("foot.raw", 256, 256, 256);
            //InitVol3DTex("skull.raw", 256, 256, 256);
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
        }

        private void InitShaders()
        {
            //backface shaders
            if (!bfVertShader.LoadShader("backface.vert", ShaderType.VertexShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");
            if (!bfFragShader.LoadShader("backface.frag", ShaderType.FragmentShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");
            //ray-casting shaders
            if (!rcVertShader.LoadShader("raycasting.vert", ShaderType.VertexShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");
            if (!rcFragShader.LoadShader("raycasting.frag", ShaderType.FragmentShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");
        }

        //inicializacia 1D textury pre transfer function
        private void InitTFF1DTex(string pathToFile)
        {
            //zistim pocet - asi by nebolo zle to nahradit dacim inym koli strate casu, v povodnom
            // programe bola napevno dana velkost 10000
            int count = File.ReadAllBytes(pathToFile).Length;

            //naplnim tff[] 
            byte[] tff = new byte[count];
            try
            {
                BinaryReader br = new BinaryReader(File.Open(pathToFile, FileMode.Open));
                br.Read(tff, 0, count);
                br.Close();

                //vytvorim 1D texturu z tff[]
                tffTexID = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture1D,tffTexID);
                GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                GL.PixelStore(PixelStoreParameter.UnpackAlignment,1);
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
            GL.BindTexture(TextureTarget.Texture2D,bfTexID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16f, texWidth, texHeight, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
        }

        // init 3D texture to store the volume data used for ray casting
        private void InitVol3DTex(string pathToFile, int w, int h, int d)
        {
            int count = w * h * d;
            
            byte[] data = new byte[count];
            try
            {
                BinaryReader br = new BinaryReader(File.Open(pathToFile, FileMode.Open));
                br.Read(data, 0, count);
                br.Close();

                //vytvorim 1D texturu z tff[]
                volTexID = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture3D, volTexID);
                GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapR, (int)TextureWrapMode.Repeat);
                GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
                GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Intensity, w, h, d, 0, PixelFormat.Luminance, PixelType.UnsignedByte, data);
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

        private void InitFrameBuffer(int texID, int texWidth, int texHeight)
        {
            // create a depth buffer for our framebuffer
            int depthBuffer;
            depthBuffer = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer,depthBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, texWidth, texHeight);

            // attach the texture and the depth buffer to the framebuffer
            frameBufferID = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferID);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,FramebufferAttachment.ColorAttachment0,TextureTarget.Texture2D,texID,0);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer,FramebufferAttachment.DepthAttachment,RenderbufferTarget.Renderbuffer,depthBuffer);

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

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture1D, tffTexID);
            spMain.SetUniform("TransferFunc", 0);

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
            GL.Disable(EnableCap.CullFace);
        }

        private void Render(CullFaceMode mode)
        {
            GL.ClearColor(0.2f, 0.2f, 0.2f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //Matrix4 model = Matrix4.Identity;
            //model = model * Matrix4.CreateRotationY(10.0f * (float)Math.PI / 180.0f);
            //model = model * Matrix4.CreateRotationX(90.0f * (float)Math.PI / 180.0f);
            //model = model * Matrix4.CreateTranslation(new Vector3(-0.5f, -0.5f, -0.5f));
            ////modelViewMatrix = model * modelViewMatrix;
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

            GL.BindFramebuffer(FramebufferTarget.Framebuffer,frameBufferID);
            GL.Viewport(0, 0, width, height);
            LinkShader(spMain.GetProgramHandle(),bfVertShader.GetShaderHandle(),bfFragShader.GetShaderHandle());
            spMain.UseProgram();
            //cull front face
            Render(CullFaceMode.Front);
            spMain.UseProgram(0);
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
            Matrix4 mat = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix);
            Current = mat * (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            //mat = mat.Inverted(); mat = Matrix4.Transpose(mat);
            //spMain.SetUniform("normalMatrix", mat);
            spMain.SetUniform("modelViewMatrix", Current);
            System.Diagnostics.Debug.WriteLine("som tu");
        }

        public void Scale(float s)
        {
            scale = s;
            ScaleMatrix = Matrix4.CreateScale(scale, scale, scale);
            Matrix4 mat = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix);
            Current = mat * (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            //mat = mat.Inverted(); mat = Matrix4.Transpose(mat);
            //spMain.SetUniform("normalMatrix", mat);
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
        #endregion

        public void Resize(int w, int h)
        {
            width = texWidth = w;
            height = texHeight = h;
        }

        public void Delete()
        {
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DeleteBuffer(verticesID);
            GL.DeleteBuffer(indicesID);
            GL.DeleteVertexArray(VAO);
            bfFragShader.DeleteShader();
            bfVertShader.DeleteShader();
            rcFragShader.DeleteShader();
            rcVertShader.DeleteShader();
            spMain.DeleteProgram();
        }
    }
}