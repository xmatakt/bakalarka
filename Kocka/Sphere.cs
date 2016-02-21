using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private Shaders.Shader VertexShader, FragmentShader;
        private Shaders.ShaderProgram spMain;
        private Matrix4 modelViewMatrix, projectionMatrix;
        private int width, height, Pi, DvaPi,len;
        private float scale;
        private Matrix4 Current, ScaleMatrix, TranslationMatrix, RotationMatrix, MatrixStore_Translations, MatrixStore_Rotations, MatrixStore_Scales;
        Kamera BigBrother; //iba pokusne, neriesil som nedostatky, chcel som sa len hybat
        DirectionalLight light;
        Material material;

        public Sphere(int w, int h, float s)
        {
            Pi = 10; DvaPi = 15;
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
            material = new Material(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), 0.1f, 0.20f, 0.73f, 0);
           
            width = w;
            height = h;
            scale = s;
            Current = Matrix4.Identity;

            CalculateSphere();
            SetSphere();
            KresliSferu();
        }

        private void CalculateSphere()
        {
            float r = 1.0f;
            float x, y, z, sinB;
            int p = 0;//teba vymazem

            for (double beta = 0.0f; beta <= Math.PI + 0.0001; beta += Math.PI / (double)Pi)
            {
                z = r * (float)Math.Cos(beta);
                sinB = (float)Math.Sin(beta);
                if (z == 1.0f || z == -1.0f)
                {
                    x = 0.0f;
                    y = 0.0f;
                    list.Add(new Vector3(x, y, z));
                    p++;
                }
                else
                {
                    for (double alfa = 0.0f; alfa < Math.PI * 2.0 - 0.0000001; alfa += Math.PI * 2.0f / (double)DvaPi)
                    {
                        x = r * (float)Math.Cos(alfa) * sinB;
                        y = r * (float)Math.Sin(alfa) * sinB;
                        list.Add(new Vector3(x, y, z));
                        p++;
                    }
                }
            }
        }

        private void SetSphere()
        {
            kocka = new Vector3[len];
            normaly = new Vector3[len];
            farba = new Vector3[len];

            int p = 0;
            //vrchna cast
            for (int i = 1;  i <= DvaPi;  i++) 
            {
                if(i == DvaPi)
                {
                    kocka[p] = normaly[p] = list[0]; p++;
                    kocka[p] = normaly[p] = list[i]; p++;
                    kocka[p] = normaly[p] = list[1]; p++;
                }
                else
                {
                    kocka[p] = normaly[p] = list[0]; p++;
                    kocka[p] = normaly[p] = list[i]; p++;
                    kocka[p] = normaly[p] = list[i + 1]; p++;
                }
            }

            //stred
            for (int j = 1; j <= Pi-2 ; j++)
            {
                for (int i = ((j - 1) * DvaPi) + 1; i <= j * DvaPi; i++) 
                {
                    if (i % DvaPi == 0) 
                    {
                        kocka[p] = normaly[p] = list[i]; p++;
                        kocka[p] = normaly[p] = list[i + DvaPi]; p++;
                        kocka[p] = normaly[p] = list[((j - 1) * DvaPi) + 1 + DvaPi]; p++;

                        kocka[p] = normaly[p] = list[i]; p++;
                        kocka[p] = normaly[p] = list[((j - 1) * DvaPi) + 1 + DvaPi]; p++;
                        kocka[p] = normaly[p] = list[((j - 1) * DvaPi) + 1]; p++;
                    }
                    else
                    {
                        kocka[p] = normaly[p] = list[i]; p++;
                        kocka[p] = normaly[p] = list[i + DvaPi]; p++;
                        kocka[p] = normaly[p] = list[i + DvaPi + 1]; p++;

                        kocka[p] = normaly[p] = list[i]; p++;
                        kocka[p] = normaly[p] = list[i + DvaPi + 1]; p++;
                        kocka[p] = normaly[p] = list[i + 1]; p++;
                    }
                }
            }

            //spodna cast
            int tmp=list.Count;
            for (int i = tmp - DvaPi - 1; i <= tmp - 2; i++) 
            {
                if (i == tmp-2)
                {
                    kocka[p] = normaly[p] = list[i]; p++;
                    kocka[p] = normaly[p] = list[tmp-1]; p++;
                    kocka[p] = normaly[p] = list[tmp - DvaPi - 1]; p++;
                }
                else
                {
                    kocka[p] = normaly[p] = list[i]; p++;
                    kocka[p] = normaly[p] = list[tmp-1]; p++;
                    kocka[p] = normaly[p] = list[i+1]; p++;
                }
            }

            for (int i = 0; i < p; i++)
                //farba[i] = new Vector3(0.0f, 0.0f, (i + 1) / (float)p);
                farba[i] = new Vector3(1.0f, 1.0f, 1.0f);

            NastavMatice(false);
            GL.GenBuffers(4, VBO);
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
            if (!VertexShader.LoadShader("..\\..\\Properties\\data\\shaders\\dirShader.vert", ShaderType.VertexShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");
            if (!FragmentShader.LoadShader("..\\..\\Properties\\data\\shaders\\dirShader.frag", ShaderType.FragmentShader))
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

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.ClearDepth(1.0);
        }

        public void SetLight(Vector3 specular,Vector3 ambient, Vector3 diffuse, Vector3 direction)
        {
            light = new DirectionalLight(direction, ambient, specular, diffuse);
            light.SetDirectionalLightUniforms(spMain);
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
            VertexShader.DeleteShader();
            FragmentShader.DeleteShader();
            spMain.DeleteProgram();
            GL.DeleteBuffers(3, VBO);
            GL.DeleteVertexArrays(1, VAO);
            list.Clear();
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
