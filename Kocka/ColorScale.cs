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
    class ColorScale
    {
        private int[] VAO;
        private int[] VBO;
        private float[] keys;
        private int width, height, NumOfVertices;
        private float min, max;
        //odrazenie baru od spodu, z lava, dlzka,vyska;
        private float bottom, left, length, barHeight;
        private Dictionary<float, Vector3> cm;
        private Vector3[] vertices;
        private Vector3[] colors;
        private Shaders.Shader VertexShader, FragmentShader;
        private Shaders.ShaderProgram spMain;
        private Matrix4 projectionMatrix, modelViewMatrix;
        private Dictionary<string, Vector4> popis_dic;
        private Text2D popis_txt;

        public ColorScale(float min, float max, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.min = min;
            this.max = max;
            length = 400.0f;
            barHeight = 25.0f;
            bottom = -height / 2.0f + 30.0f;
            left = -length / 2.0f;
            NumOfVertices = 0;
            cm = new Dictionary<float, Vector3>();
            popis_txt = new Text2D(width,height);
            popis_dic = new Dictionary<string, Vector4>();

            VAO = new int[1];
            VBO = new int[2];
            projectionMatrix = Matrix4.Identity;
            modelViewMatrix = Matrix4.Identity;

            VertexShader = new Shaders.Shader();
            FragmentShader = new Shaders.Shader();
            spMain = new Shaders.ShaderProgram();

            SetColorScale();
            SetText();
            Init();
        }

        private void Init()
        {
            modelViewMatrix = Matrix4.LookAt(0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
            projectionMatrix = Matrix4.CreateOrthographic(width, height, 1, 100);

            GL.GenBuffers(2, VBO);
            GL.GenVertexArrays(1, VAO);

            GL.BindVertexArray(VAO[0]);

            //vrcholy
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(NumOfVertices * Vector3.SizeInBytes), vertices, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            //farby
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(NumOfVertices * Vector3.SizeInBytes), colors, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);

            if (!VertexShader.LoadShaderS(Kocka.Properties.Resources.colorScaleVert, ShaderType.VertexShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder (color bar)!");
            if (!FragmentShader.LoadShaderS(Kocka.Properties.Resources.colorScaleFrag, ShaderType.FragmentShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder (color bar)!");

            spMain.CreateProgram();
            spMain.AddShaderToProgram(VertexShader);
            spMain.AddShaderToProgram(FragmentShader);
            spMain.LinkProgram();

            VertexShader.DetachShader(spMain.GetProgramHandle());
            FragmentShader.DetachShader(spMain.GetProgramHandle());
            VertexShader.DeleteShader();
            FragmentShader.DeleteShader();

            spMain.UseProgram();
            spMain.SetUniform("projectionMatrix", projectionMatrix);
            spMain.SetUniform("modelViewMatrix", modelViewMatrix);
            spMain.UseProgram(0);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.BindVertexArray(0);
        }

        private void SetText()
        {
            float len = max - min;
            popis_dic.Add(min.ToString(), new Vector4(left-30.0f,bottom, 0.7f, 0.7f));
            popis_dic.Add((min + len * 0.25f).ToString(), new Vector4(left + 0.25f * length - 30.0f, bottom, 0.7f, 0.7f));
            popis_dic.Add((min + len * 0.5f).ToString(), new Vector4(left + 0.5f * length - 30.0f, bottom , 0.7f, 0.7f));
            popis_dic.Add((min + len * 0.75f).ToString(), new Vector4(left + 0.75f * length - 30.0f, bottom , 0.7f, 0.7f));
            popis_dic.Add(max.ToString(), new Vector4(left + length - 30.0f, bottom , 0.7f, 0.7f));
            popis_txt.PrintText2D(popis_dic);
        }

        private void SetColorScale()
        {
            //prvotne nastavenie colorbaru
            Vector3 f1 = new Vector3(0.0f, 0.0f, 0.5f); float x1 = 0.0f; cm.Add(x1, f1);
            Vector3 f2 = new Vector3(0.0f, 0.0f, 1.0f); float x2 = 0.1f; cm.Add(x2, f2);
            Vector3 f3 = new Vector3(0.0f, 1.0f, 1.0f); float x3 = 0.35f; cm.Add(x3, f3);
            Vector3 f4 = new Vector3(1.0f, 1.0f, 0.0f); float x4 = 0.65f; cm.Add(x4, f4);
            Vector3 f5 = new Vector3(1.0f, 0.0f, 0.0f); float x5 = 0.9f; cm.Add(x5, f5);
            Vector3 f6 = new Vector3(0.5f, 0.0f, 0.0f); float x6 = 1.0f; cm.Add(x6, f6);
            //

            keys = cm.Keys.ToArray();
            NumOfVertices = (cm.Count - 1) * 6;
            vertices = new Vector3[NumOfVertices];
            colors = new Vector3[NumOfVertices];

            float z = 0.0f, y1 = bottom, y2 = bottom + barHeight;
            for (int i = 0; i < cm.Count - 1; i++)
            {
                x1 = left + length * keys[i];
                x2 = left + length * keys[i + 1];
                int i1 = i * 6; int i2 = i * 6 + 1; int i3 = i * 6 + 2;
                int i4 = i * 6 + 3; int i5 = i * 6 + 4; int i6 = i * 6 + 5;
                //
                vertices[i1] = new Vector3(x1, y1, z);
                vertices[i2] = new Vector3(x2, y1, z);
                vertices[i3] = new Vector3(x2, y2, z);
                vertices[i4] = new Vector3(x1, y1, z);
                vertices[i5] = new Vector3(x1, y2, z);
                vertices[i6] = new Vector3(x2, y2, z);
                //
                colors[i1] = colors[i4] = colors[i5] = cm[keys[i]];
                colors[i2] = colors[i3] = colors[i6] = cm[keys[i + 1]];
            }
        }

        private void RecalculateVertices()
        {
            float y1 = bottom, z = 0.0f;
            float y2 = bottom + barHeight;
            for (int i = 0; i < cm.Count - 1; i++)
            {
                int i1 = i * 6; int i2 = i * 6 + 1; int i3 = i * 6 + 2;
                int i4 = i * 6 + 3; int i5 = i * 6 + 4; int i6 = i * 6 + 5;
                //
                vertices[i1] = new Vector3(vertices[i1].X, y1, z);
                vertices[i2] = new Vector3(vertices[i2].X, y1, z);
                vertices[i3] = new Vector3(vertices[i3].X, y2, z);
                vertices[i4] = new Vector3(vertices[i4].X, y1, z);
                vertices[i5] = new Vector3(vertices[i5].X, y2, z);
                vertices[i6] = new Vector3(vertices[i6].X, y2, z);
                //
            }
        }

        public void DrawColorScale()
        {
            spMain.UseProgram();
            GL.BindVertexArray(VAO[0]);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.DrawArrays(PrimitiveType.Triangles, 0, NumOfVertices);
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.BindVertexArray(0);
            spMain.UseProgram(0);
            popis_txt.RenderText();
        }

        public Vector3 SetColor(float height)
        {
            float dv = max - min;
            Vector3 col = new Vector3();
            for (int i = 1; i < keys.Length; i++)
            {
                if (height <= min + keys[i] * dv)
                {
                    LinearFunction R = new LinearFunction(min + keys[i - 1] * dv, min + keys[i] * dv, cm[keys[i - 1]].X, cm[keys[i]].X);
                    LinearFunction G = new LinearFunction(min + keys[i - 1] * dv, min + keys[i] * dv, cm[keys[i - 1]].Y, cm[keys[i]].Y);
                    LinearFunction B = new LinearFunction(min + keys[i - 1] * dv, min + keys[i] * dv, cm[keys[i - 1]].Z, cm[keys[i]].Z);
                    col = new Vector3(R.Value(height), G.Value(height), B.Value(height));
                    return col;
                }
            }
            return col;
        }

        public List<Vector3> SetColorList(List<Vector3> heights)
        {
            List<Vector3> colors = new List<Vector3>();
            Vector3 col = new Vector3();
            float dv = max - min;
            for (int j = 0; j < heights.Count; j++)
            {
                for (int i = 1; i < keys.Length; i++)
                {
                    if (heights[j].Z < min + keys[i] * dv)
                    {
                        LinearFunction R = new LinearFunction(min + keys[i - 1] * dv, min + keys[i] * dv, cm[keys[i - 1]].X, cm[keys[i]].X);
                        LinearFunction G = new LinearFunction(min + keys[i - 1] * dv, min + keys[i] * dv, cm[keys[i - 1]].Y, cm[keys[i]].Y);
                        LinearFunction B = new LinearFunction(min + keys[i - 1] * dv, min + keys[i] * dv, cm[keys[i - 1]].Z, cm[keys[i]].Z);
                        col = new Vector3(R.Value(heights[j].Z), G.Value(heights[j].Z), B.Value(heights[j].Z));
                        i = keys.Length;
                    }
                }
                colors.Add(col);
            }
            return colors;
        }

        public Vector3[] SetColorArray(Vector3[] heights)
        {
            Vector3[] colors = new Vector3[heights.Length];
            Vector3 col = new Vector3();
            float dv = max - min;
            for (int j = 0; j < heights.Length; j++)
            {
                for (int i = 1; i < keys.Length; i++)
                {
                    if (heights[j].Z < min + keys[i] * dv)
                    {
                        LinearFunction R = new LinearFunction(min + keys[i - 1] * dv, min + keys[i] * dv, cm[keys[i - 1]].X, cm[keys[i]].X);
                        LinearFunction G = new LinearFunction(min + keys[i - 1] * dv, min + keys[i] * dv, cm[keys[i - 1]].Y, cm[keys[i]].Y);
                        LinearFunction B = new LinearFunction(min + keys[i - 1] * dv, min + keys[i] * dv, cm[keys[i - 1]].Z, cm[keys[i]].Z);
                        col = new Vector3(R.Value(heights[j].Z), G.Value(heights[j].Z), B.Value(heights[j].Z));
                        i = keys.Length;
                    }
                }
                colors[j] = col;
            }
            return colors;
        }

        public void ResizeScale(int width, int height)
        {
            this.width = width;
            this.height = height;
            bottom = -height / 2.0f + 30.0f;
            projectionMatrix = Matrix4.CreateOrthographic(width, height, 1, 100);
            spMain.UseProgram();
            spMain.SetUniform("projectionMatrix", projectionMatrix);
            RecalculateVertices();
            spMain.UseProgram(0);

            popis_dic.Clear();
            popis_txt.ResizeText2D(width,height);
            SetText();
            
            GL.BindVertexArray(VAO[0]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(NumOfVertices * Vector3.SizeInBytes), vertices, BufferUsageHint.StaticDraw);
            GL.BindVertexArray(0);
        }

        private void SortDictionary()
        {
            var l = cm.OrderBy(key => key.Key);
            var dic = l.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
            cm = dic;
        }

        public void Delete()
        {
            GL.DeleteBuffers(2, VBO);
            GL.DeleteVertexArrays(1,VAO);
            cm.Clear();
            popis_dic.Clear();
            spMain.DeleteProgram();
            popis_txt.Delete();
        }
    }
}
