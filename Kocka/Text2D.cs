using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Kocka
{
    class Text2D
    {
        private Shaders.Shader VertexShader, FragmentShader;
        private Shaders.ShaderProgram spMain;
        private Matrix4 projectionMatrix, modelViewMatrix;

        private int TextureID;
        private int VertexBufferID;
        private int UVBufferID;
        private int vaoID;
        private int width, height, pocet;
        private List<Vector3> vertices;
        private List<Vector2> uvs;
        //slovnik na ulozenie polohy a velkosti jednotlivych pismen
        private Dictionary<char, Vector4> charMap;

        public Text2D(int width, int height)
        {
            this.width = width;
            this.height = height;
            modelViewMatrix = Matrix4.LookAt(0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
            projectionMatrix = Matrix4.CreateOrthographic(width, height, 1, 100);
            charMap = new Dictionary<char, Vector4>();

            GL.GenBuffers(1, out VertexBufferID);
            GL.GenBuffers(1, out UVBufferID);
            GL.GenVertexArrays(1, out vaoID);

            // "fontBig.fnt" --> tu su ulozene pozicie znakov z fontBig.png
            string pngPath = string.Format("..{0}..{0}Properties{0}data{0}Fonts{0}fontBig.png", Path.DirectorySeparatorChar);
            string fntPath = string.Format("..{0}..{0}Properties{0}data{0}Fonts{0}fontBig.fnt", Path.DirectorySeparatorChar);
            LoadShaders();
            LoadTexture(pngPath);
            MakeCharacterDictionary(fntPath);
        }

        private void LoadShaders()
        {
            VertexShader = new Shaders.Shader();
            FragmentShader = new Shaders.Shader();
            spMain = new Shaders.ShaderProgram();


            string vspath = string.Format("..{0}..{0}Properties{0}data{0}shaders{0}textShader.vert", Path.DirectorySeparatorChar);
            string fspath = string.Format("..{0}..{0}Properties{0}data{0}shaders{0}textShader.frag", Path.DirectorySeparatorChar);
            if (!VertexShader.LoadShader(vspath, ShaderType.VertexShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder (font rendering)!");
            if (!FragmentShader.LoadShader(fspath, ShaderType.FragmentShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder (font rendering)!");

            spMain.CreateProgram();
            spMain.AddShaderToProgram(VertexShader);
            spMain.AddShaderToProgram(FragmentShader);
            spMain.LinkProgram();

            //teraz mozem detachnut shadere a znicit ich
            VertexShader.DetachShader(spMain.GetProgramHandle());
            FragmentShader.DetachShader(spMain.GetProgramHandle());
            VertexShader.DeleteShader();
            FragmentShader.DeleteShader();
            //

            //toto staci raz, pozicia sa nebude menit
            spMain.UseProgram();
            spMain.SetUniform("gSampler", 0);
            spMain.SetUniform("projectionMatrix", projectionMatrix);
            spMain.SetUniform("modelViewMatrix", modelViewMatrix);
            spMain.UseProgram(0);
        }

        private void LoadTexture(string pathToFile)
        {
            TextureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, TextureID);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            Bitmap bmp = new Bitmap(pathToFile);
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);
            bmp.Dispose();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void MakeCharacterDictionary(string pathToFile)
        {
            char[] separator = { '\n' };
            char[] separator2 = { '=' };
            StreamReader sr = new StreamReader(pathToFile);
            string[] fnt = sr.ReadToEnd().Split(separator);
            for (int i = 0; i < fnt.Length; i++)
            {
                if (fnt[i].StartsWith("char id="))
                {
                    string[] line = fnt[i].Split(separator2);
                    char id = (char)int.Parse(line[1].Substring(0, line[1].IndexOf(" ")));
                    int x = int.Parse(line[2].Substring(0, line[2].IndexOf(" ")));
                    int y = int.Parse(line[3].Substring(0, line[3].IndexOf(" ")));
                    int w = int.Parse(line[4].Substring(0, line[4].IndexOf(" ")));
                    int h = int.Parse(line[5].Substring(0, line[5].IndexOf(" ")));
                    if (!charMap.ContainsKey(id))
                        charMap.Add(id, new Vector4(x, y, w, h));
                }
            }
            sr.Close();
        }

        // kluc -> slovo ktore ma byt vypisane, hodnota -> pozicia a velkost
        private void LoadText2D(Dictionary<string, Vector4> dic)
        {
            bool pravda = true;
            vertices = new List<Vector3>();
            uvs = new List<Vector2>();
            string[] arr = dic.Keys.ToArray();

            for (int j = 0; j < arr.Length; j++)
            {
                float dx = dic[arr[j]].X;
                float dy = dic[arr[j]].Y;
                float size_x = dic[arr[j]].Z;
                float size_y = dic[arr[j]].W;
                string text = arr[j];

                for (int i = 0; i < text.Length; i++)
                {
                    if (charMap.ContainsKey(text[i]))
                    {
                        float char_x = charMap[text[i]].X;
                        float char_y = charMap[text[i]].Y;
                        float char_w = charMap[text[i]].Z;
                        float char_h = charMap[text[i]].W;

                        Vector3 vert_DownLeft = new Vector3(dx, dy, 0.0f);
                        Vector3 vert_DownRight = new Vector3(dx + size_x * char_w, dy, 0.0f);
                        Vector3 vert_UpRight = new Vector3(dx + size_x * char_w, dy + size_y * char_h, 0.0f);
                        Vector3 vert_UpLeft = new Vector3(dx, dy + size_y * char_h, 0.0f);
                        dx += size_x * char_w;

                        vertices.Add(vert_UpLeft);
                        vertices.Add(vert_UpRight);
                        vertices.Add(vert_DownLeft);

                        vertices.Add(vert_DownLeft);
                        vertices.Add(vert_DownRight);
                        vertices.Add(vert_UpRight);

                        //
                        char_x = char_x / 256.0f;
                        char_y = char_y / 256.0f;
                        char_w = char_w / 256.0f;
                        char_h = char_h / 256.0f;

                        Vector2 uv_UpLeft = new Vector2(char_x, char_y);
                        Vector2 uv_UpRight = new Vector2(char_x + char_w, char_y);
                        Vector2 uv_DownRight = new Vector2(char_x + char_w, char_y + char_h);
                        Vector2 uv_DownLeft = new Vector2(char_x, char_y + char_h);

                        uvs.Add(uv_UpLeft);
                        uvs.Add(uv_UpRight);
                        uvs.Add(uv_DownLeft);

                        uvs.Add(uv_DownLeft);
                        uvs.Add(uv_DownRight);
                        uvs.Add(uv_UpRight);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Znak " + text[i] + " nema podporu!", "Vnimanie!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        i = text.Length;
                        pravda = false;
                    }
                }
            }

            if (pravda)
            {
                //spMain.UseProgram();
                GL.BindVertexArray(vaoID);
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D,TextureID);

                //vertices
                GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferID);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Count * Vector3.SizeInBytes), vertices.ToArray(), BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

                //UVs
                GL.BindBuffer(BufferTarget.ArrayBuffer, UVBufferID);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(uvs.Count * Vector2.SizeInBytes), uvs.ToArray(), BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

                //spMain.UseProgram(0);
                pocet = vertices.Count();
                vertices.Clear();
                uvs.Clear();
            }
        }

        //string s - co sa ma napisat; (x,y) kde to zacina (lavy dolny), size_x,size_y - ake velke
        private void LoadText2D(string text, int x, int y, int size_x, int size_y)
        {
            bool pravda = true;
            vertices = new List<Vector3>();
            uvs = new List<Vector2>();
            float dx = x;
            float dy = y;

            for (int i = 0; i < text.Length; i++)
            {
                if (charMap.ContainsKey(text[i]))
                {
                    float char_x = charMap[text[i]].X;
                    float char_y = charMap[text[i]].Y;
                    float char_w = charMap[text[i]].Z;
                    float char_h = charMap[text[i]].W;

                    Vector3 vert_DownLeft = new Vector3(dx, dy, 0.0f);
                    Vector3 vert_DownRight = new Vector3(dx + size_x * char_w, dy, 0.0f);
                    Vector3 vert_UpRight = new Vector3(dx + size_x * char_w, dy + size_y * char_h, 0.0f);
                    Vector3 vert_UpLeft = new Vector3(dx, dy + size_y * char_h, 0.0f);
                    dx += size_x * char_w;

                    vertices.Add(vert_UpLeft);
                    vertices.Add(vert_UpRight);
                    vertices.Add(vert_DownLeft);

                    vertices.Add(vert_DownLeft);
                    vertices.Add(vert_DownRight);
                    vertices.Add(vert_UpRight);

                    //
                    char_x = char_x / 256.0f;
                    char_y = char_y / 256.0f;
                    char_w = char_w / 256.0f;
                    char_h = char_h / 256.0f;

                    Vector2 uv_UpLeft = new Vector2(char_x, char_y);
                    Vector2 uv_UpRight = new Vector2(char_x + char_w, char_y);
                    Vector2 uv_DownRight = new Vector2(char_x + char_w, char_y + char_h);
                    Vector2 uv_DownLeft = new Vector2(char_x, char_y + char_h);

                    uvs.Add(uv_UpLeft);
                    uvs.Add(uv_UpRight);
                    uvs.Add(uv_DownLeft);

                    uvs.Add(uv_DownLeft);
                    uvs.Add(uv_DownRight);
                    uvs.Add(uv_UpRight);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Znak " + text[i] + " nema podporu!", "Vnimanie!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    i = text.Length;
                    pravda = false;
                }
            }

            if (pravda)
            {
                //toto by malo zobrazit iba text, bez cierneho pozadia
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, TextureID);

                //vertices
                GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferID);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Count * Vector3.SizeInBytes), vertices.ToArray(), BufferUsageHint.StaticDraw);
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

                //UVs
                GL.BindBuffer(BufferTarget.ArrayBuffer, UVBufferID);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(uvs.Count * Vector2.SizeInBytes), uvs.ToArray(), BufferUsageHint.StaticDraw);
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

                spMain.UseProgram();
                GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Count);
                spMain.UseProgram(0);

                //co si kto zapol, nech si aj vypne
                GL.Disable(EnableCap.Blend);
                GL.DisableVertexAttribArray(0);
                GL.DisableVertexAttribArray(1);
                GL.BindTexture(TextureTarget.Texture2D, 0);

                vertices.Clear();
                uvs.Clear();
            }
        }

        public void PrintText2D(string text, int x, int y, int size_x, int size_y)
        {
            LoadText2D(text, x, y, size_x, size_y);
        }

        public void PrintText2D(Dictionary<string, Vector4> dic)
        {
            LoadText2D(dic);
            RenderText();
        }

        public void ResizeText2D(int width,int height)
        {
            this.width = width;
            this.height = height;
            projectionMatrix = Matrix4.CreateOrthographic(width, height, 1, 100);
            spMain.UseProgram();
            spMain.SetUniform("projectionMatrix", projectionMatrix);
            spMain.SetUniform("modelViewMatrix", modelViewMatrix);
            spMain.UseProgram(0);
        }

        public void RenderText()
        {
            spMain.UseProgram();
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.BindTexture(TextureTarget.Texture2D, TextureID);

            GL.BindVertexArray(vaoID);
            GL.DrawArrays(PrimitiveType.Triangles, 0, pocet);
            GL.BindVertexArray(0);

            GL.Disable(EnableCap.Blend);
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            spMain.UseProgram(0);
        }

        public int GetTextureID()
        {
            return TextureID;
        }

        public void Delete()
        {
            spMain.DeleteProgram();
            GL.DeleteBuffers(1, ref VertexBufferID);
            GL.DeleteBuffers(1, ref UVBufferID);
            GL.DeleteTexture(TextureID);
            GL.DeleteVertexArray(vaoID);
        }
    }
}
