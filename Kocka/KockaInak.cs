using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Kocka
{
    class KockaInak
    {
        private Vector3[] kocka;
        private Vector3[] farba;
        private Vector3[] normaly;
        private int[] VBO;
        private int[] VAO;
        private int[] Indices;
        private Shaders.Shader VertexShader, FragmentShader;
        private Shaders.ShaderProgram spMain;
        private Matrix4 modelViewMatrix, projectionMatrix;
        private int width, height;
        private float scale;
        private Matrix4 Current, ScaleMatrix, TranslationMatrix, RotationMatrix, MatrixStore_Translations, MatrixStore_Rotations, MatrixStore_Scales;
        Kamera BigBrother; //iba pokusne, neriesil som nedostatky, chcel som sa len hybat
        DirectionalLight light;
        Material material;

        public KockaInak(int w, int h, float s)
        {
            VBO = new int[4];
            VAO = new int[1];
            VertexShader = new Shaders.Shader();
            FragmentShader = new Shaders.Shader();
            spMain = new Shaders.ShaderProgram();
            BigBrother = new Kamera(new Vector3(0.0f, 0.0f, 3.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            //svetlo - smer,ambient,specular,diffuse
            light = new DirectionalLight(new Vector3(0.0f, 0.0f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f));
            //material - ambient,specular,diffuse,koeficienty - ambient, specular, diffuse, shininess 
            material = new Material(0.30f, 0.10f, 0.420f, 8);
           
            width = w;
            height = h;
            scale = s;
            Current = Matrix4.Identity;

            #region vrcholy
            Vector3[] k ={
                              //spodna stena
                              new Vector3(-1.0f,-1.0f,-1.0f),
                              new Vector3(-1.0f,-1.0f, 1.0f),
                              new Vector3( 1.0f,-1.0f, 1.0f),
                              new Vector3( 1.0f,-1.0f, 1.0f),
                              new Vector3( 1.0f,-1.0f,-1.0f),
                              new Vector3(-1.0f,-1.0f,-1.0f),
                              //vrchna stena
                              new Vector3(-1.0f,1.0f,-1.0f),
                              new Vector3(-1.0f,1.0f, 1.0f),
                              new Vector3( 1.0f,1.0f, 1.0f),
                              new Vector3( 1.0f,1.0f, 1.0f),
                              new Vector3( 1.0f,1.0f,-1.0f),
                              new Vector3(-1.0f,1.0f,-1.0f),
                              //lava stena
                              new Vector3(-1.0f,1.0f,-1.0f),
                              new Vector3(-1.0f,1.0f, 1.0f),
                              new Vector3(-1.0f,-1.0f, 1.0f),
                              new Vector3(-1.0f,-1.0f, 1.0f),
                              new Vector3(-1.0f,-1.0f,-1.0f),
                              new Vector3(-1.0f, 1.0f,-1.0f),
                              //prava stena
                              new Vector3( 1.0f,1.0f,-1.0f),
                              new Vector3( 1.0f,1.0f, 1.0f),
                              new Vector3( 1.0f,-1.0f, 1.0f),
                              new Vector3( 1.0f,-1.0f, 1.0f),
                              new Vector3( 1.0f,-1.0f,-1.0f),
                              new Vector3( 1.0f, 1.0f,-1.0f),
                              //predna stena
                              new Vector3(-1.0f, 1.0f, 1.0f),
                              new Vector3(-1.0f,-1.0f, 1.0f),
                              new Vector3( 1.0f,-1.0f, 1.0f),
                              new Vector3( 1.0f,-1.0f, 1.0f),
                              new Vector3( 1.0f, 1.0f, 1.0f),
                              new Vector3(-1.0f, 1.0f, 1.0f),
                              //zadna stena
                              new Vector3(-1.0f, 1.0f,-1.0f),
                              new Vector3(-1.0f,-1.0f,-1.0f),
                              new Vector3( 1.0f,-1.0f,-1.0f),
                              new Vector3( 1.0f,-1.0f,-1.0f),
                              new Vector3( 1.0f, 1.0f,-1.0f),
                              new Vector3(-1.0f, 1.0f,-1.0f)
                          };
            kocka = k;
            #endregion

            #region farby
            Vector3[] f ={
                               //spodna stena
                              new Vector3(1.0f,0.0f,1.0f),
                              new Vector3(1.0f,0.0f,1.0f),
                              new Vector3(1.0f,0.0f,1.0f),
                              new Vector3(1.0f,0.0f,1.0f),
                              new Vector3(1.0f,0.0f,1.0f),
                              new Vector3(1.0f,0.0f,1.0f),
                              //vrchna stena
                              new Vector3(1.0f,1.0f,1.0f),
                              new Vector3(1.0f,1.0f,1.0f),
                              new Vector3(1.0f,1.0f,1.0f),
                              new Vector3(1.0f,1.0f,1.0f),
                              new Vector3(1.0f,1.0f,1.0f),
                              new Vector3(1.0f,1.0f,1.0f),
                              //lava stena
                              new Vector3(0.0f,0.0f,1.0f),
                              new Vector3(0.0f,0.0f,1.0f),
                              new Vector3(0.0f,0.0f,1.0f),
                              new Vector3(0.0f,0.0f,1.0f),
                              new Vector3(0.0f,0.0f,1.0f),
                              new Vector3(0.0f,0.0f,1.0f),
                              //prava stena
                              new Vector3( 1.0f,1.0f,0.0f),
                              new Vector3( 1.0f,1.0f,0.0f),
                              new Vector3( 1.0f,1.0f,0.0f),
                              new Vector3( 1.0f,1.0f,0.0f),
                              new Vector3( 1.0f,1.0f,0.0f),
                              new Vector3( 1.0f,1.0f,0.0f),
                              //predna stena
                              new Vector3(1.0f, 0.0f, 0.0f),
                              new Vector3(1.0f, 0.0f, 0.0f),
                              new Vector3(1.0f, 0.0f, 0.0f),
                              new Vector3(1.0f, 0.0f, 0.0f),
                              new Vector3(1.0f, 0.0f, 0.0f),
                              new Vector3(1.0f, 0.0f, 0.0f),
                              //zadna stena
                              new Vector3(0.0f, 1.0f,0.0f),
                              new Vector3(0.0f, 1.0f,0.0f),
                              new Vector3(0.0f, 1.0f,0.0f),
                              new Vector3(0.0f, 1.0f,0.0f),
                              new Vector3(0.0f, 1.0f,0.0f),
                              new Vector3(0.0f, 1.0f,0.0f)
                          };
            farba = f;
            #endregion

            #region normaly
            Vector3[] n ={
                              //spodna stena
                              new Vector3(0.0f,-1.0f,0.0f),
                              new Vector3(0.0f,-1.0f,0.0f),
                              new Vector3(0.0f,-1.0f,0.0f),
                              new Vector3(0.0f,-1.0f,0.0f),
                              new Vector3(0.0f,-1.0f,0.0f),
                              new Vector3(0.0f,-1.0f,0.0f),
                              //vrchna stena
                              new Vector3(0.0f,1.0f,0.0f),
                              new Vector3(0.0f,1.0f,0.0f),
                              new Vector3(0.0f,1.0f,0.0f),
                              new Vector3(0.0f,1.0f,0.0f),
                              new Vector3(0.0f,1.0f,0.0f),
                              new Vector3(0.0f,1.0f,0.0f),
                              //lava stena
                              new Vector3(-1.0f,0.0f,0.0f),
                              new Vector3(-1.0f,0.0f,0.0f),
                              new Vector3(-1.0f,0.0f,0.0f),
                              new Vector3(-1.0f,0.0f,0.0f),
                              new Vector3(-1.0f,0.0f,0.0f),
                              new Vector3(-1.0f,0.0f,0.0f),
                              //prava stena
                              new Vector3( 1.0f,0.0f,0.0f),
                              new Vector3( 1.0f,0.0f,0.0f),
                              new Vector3( 1.0f,0.0f,0.0f),
                              new Vector3( 1.0f,0.0f,0.0f),
                              new Vector3( 1.0f,0.0f,0.0f),
                              new Vector3( 1.0f,0.0f,0.0f),
                              //predna stena
                              new Vector3(0.0f, 0.0f, 1.0f),
                              new Vector3(0.0f, 0.0f, 1.0f),
                              new Vector3(0.0f, 0.0f, 1.0f),
                              new Vector3(0.0f, 0.0f, 1.0f),
                              new Vector3(0.0f, 0.0f, 1.0f),
                              new Vector3(0.0f, 0.0f, 1.0f),
                              //zadna stena
                              new Vector3(0.0f, 0.0f,-1.0f),
                              new Vector3(0.0f, 0.0f,-1.0f),
                              new Vector3(0.0f, 0.0f,-1.0f),
                              new Vector3(0.0f, 0.0f,-1.0f),
                              new Vector3(0.0f, 0.0f,-1.0f),
                              new Vector3(0.0f, 0.0f,-1.0f)
                          };
            normaly = n;
            #endregion

            #region Indices
            int[] i =
            {
                0,1,2,36,
                3,4,5,36,
                6,7,8,36,
                9,10,11,36,
                12,13,14,36,
                15,16,17,36,
                18,19,20,36,
                21,22,23,36,
                24,25,26,36,
                27,28,29,36,
                30,31,32,36,
                33,34,35
            };
            Indices = i;
            #endregion

            NastavKocku();
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

        private void NastavKocku()
        {
            NastavMatice(false);
            GL.GenBuffers(4, VBO);
            GL.GenVertexArrays(1, VAO);

            GL.BindVertexArray(VAO[0]);

            //vrcholy
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(36 * Vector3.SizeInBytes), kocka, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            //farby
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(36 * Vector3.SizeInBytes), farba, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);

            //normaly
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[2]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(36 * Vector3.SizeInBytes), normaly, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);

            //indices
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, VBO[3]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(int)*Indices.Length), Indices, BufferUsageHint.StaticDraw);
            GL.Enable(EnableCap.PrimitiveRestart);
            GL.PrimitiveRestartIndex(36);

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
            //spMain.SetUniform("sunLight.fAmbientIntensity", 0.25f);
            ////smer svetelnych lucov
            //spMain.SetUniform("sunLight.vDirection", new Vector3(0.0f, 0.0f, -1.0f));

            //GL.Enable(EnableCap.DepthTest);
            //GL.DepthFunc(DepthFunction.Less);
            //GL.ClearDepth(1.0);

            KresliKocku();
        }

        public void SetLight(Vector3 specular, Vector3 ambient, Vector3 diffuse, Vector3 direction)
        {
            light = new DirectionalLight(direction, ambient, specular, diffuse);
            light.SetDirectionalLightUniforms(spMain);
        }

        public void SetMaterial(float SpecCoeff, float AmbCoeff, float DiffCoeff, int shin)
        {
            material = new Material(AmbCoeff, SpecCoeff, DiffCoeff, shin);
            material.SetMaterialUniforms(spMain);
        }

        private void KresliKocku()
        {
            Matrix4 mat = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix) * (TranslationMatrix * MatrixStore_Translations);
            Current = mat * modelViewMatrix;
            mat = mat.Inverted(); mat.Transpose();
            GL.BindVertexArray(VAO[0]);
            spMain.SetUniform("normalMatrix", mat);
            spMain.SetUniform("modelViewMatrix",Current);
        }

        public void PrekresliKocku()
        {
            GL.BindVertexArray(VAO[0]);
            GL.DrawElements(PrimitiveType.TriangleStrip, 47, DrawElementsType.UnsignedInt,0);
        }

        public void Translate(float x, float y)
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
            //pozriet sa ci deletujem vsetko co sa da
            GL.DeleteBuffers(4, VBO);
            GL.DeleteVertexArrays(1, VAO);
            GL.DisableVertexAttribArray(0);//pridavane zo zufalstva
            GL.DisableVertexAttribArray(1);//pridavane zo zufalstva
            GL.DisableVertexAttribArray(2);//pridavane zo zufalstva
            GL.Disable(EnableCap.PrimitiveRestart);//tak tento bazmek robil certy

            GL.DetachShader(spMain.GetProgramHandle(),VertexShader.GetShaderHandle());
            VertexShader.DeleteShader();
            GL.DetachShader(spMain.GetProgramHandle(), FragmentShader.GetShaderHandle());
            FragmentShader.DeleteShader();


            spMain.DeleteProgram();
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
