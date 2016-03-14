﻿using System;
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
    class SphereDAT
    {
        private int NumOfParallels;
        private int NumOfVertices;
        private int width, height, NumOfTriangles;
        private Vector3[] vertices;
        private Vector3[] normals;
        private Vector3[] color;
        private Vector3[] zaloha;
        private List<Vector3> coords;
        private List<Vector3> noormals;
        private List<Vector3> coolors;

        private float RAD, R, scale, min, max,value;
        private bool Status;
        private int[] VBO;
        private int[] VAO;
        private Shaders.Shader VertexShader, FragmentShader;
        private Shaders.ShaderProgram spMain;
        private Matrix4 modelViewMatrix, projectionMatrix;
        private Matrix4 Current, ScaleMatrix, TranslationMatrix, RotationMatrix, MatrixStore_Translations, MatrixStore_Rotations, MatrixStore_Scales;
        DirectionalLight light;
        Material material;
        Kamera BigBrother;

        //nacitanie dat do listu + prevod
        public SphereDAT(int w, int h, string pathToFile)
        {
            Status = false;
            min = float.MaxValue;
            max = float.MinValue;
            R = 1.0f;
            value = 0.0f;
            scale = 1.0f;
            width = w; height = h;
            RAD = (float)Math.PI / 180.0f;
            NumOfTriangles = NumOfParallels = NumOfTriangles = 0;
            //svetlo - smer,ambient,specular,diffuse
            light = new DirectionalLight(new Vector3(0.0f, 0.0f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f));
            //material - ambient,specular,diffuse,koeficienty - ambient, specular, diffuse, shininess 
            material = new Material(0.16f, 0.50f, 0.6f, 124);
            BigBrother = new Kamera(new Vector3(0.0f, 0.0f, 3.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));

            coords = new List<Vector3>();
            noormals = new List<Vector3>();
            coolors = new List<Vector3>();

            VBO = new int[3];
            VAO = new int[1];
            VertexShader = new Shaders.Shader();
            FragmentShader = new Shaders.Shader();
            spMain = new Shaders.ShaderProgram();

            Init(pathToFile);  
        }

        private void Init(string pathToFile)
        {
            if(LoadData(pathToFile))
            {
                zaloha = new Vector3[coords.Count];
                zaloha = coords.ToArray();
                //najprv nastavim farby
                SetColorList();
                //potom preskalujem vysky
                value = 0.5f*(max - min);
                ScaleHeights(10.0f);
                NumOfParallels = (int)Math.Sqrt(coords.Count - 2) + 1;
                //PustiToRychlejsie(0.033f);
                GeoToSpatialCoords();

                GetNumberOfTriangles();
                NumOfVertices = 3 * NumOfTriangles;
                vertices = new Vector3[NumOfVertices];
                color = new Vector3[NumOfVertices];
                normals = new Vector3[NumOfVertices];
                

                CalculateNormals();
                //triangulacia
                InitSphere();
                //nastavenie bufferov a shaderov
                InitScene(false);
                FirstDraw();
            }
            else System.Windows.Forms.MessageBox.Show("Súbor "+ pathToFile+ " nemá podporu!","Vnimanie!",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Warning);
        }

        private bool LoadData(string pathToFile)
        {
            StreamReader sr;
            Vector3 tmp;
            char[] separator = { ' ', '\t' };
            string[] line;
            try
            {
                sr = new StreamReader(pathToFile);
                while (!sr.EndOfStream)
                {
                    //System.Diagnostics.Debug.WriteLine(sr.ReadLine());
                    line = sr.ReadLine().Split(separator);
                    //System.Diagnostics.Debug.WriteLine("dlzka = {0}",line.Length);
                    tmp = new Vector3(float.Parse(line[0]), float.Parse(line[1]), float.Parse(line[2]));
                    coords.Add(tmp);
                    if (coords.Last().Z < min)
                        min = coords.Last().Z;
                    if (coords.Last().Z > max)
                        max = coords.Last().Z;
                }
                sr.Close();
                if (coords[0].X == coords[1].X)
                    Status = false;
                else
                    Status = true;
            }
            catch (FileNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show("Subor sa nenasiel!");
                Status = false;
            }
            return Status;
        }

        public bool Loaded()
        {
            return Status;
        }

        private void CalculateNormals()
        {
            int startIndex = 0;
            int endIndex = 0;
            int tmp = 0;
            Vector3 n = new Vector3(0.0f, 0.0f, 0.0f);

            //cepicka
            n = Vector3.Cross(coords[0] - coords[1], coords[2] - coords[0]);
            n += Vector3.Cross(coords[0] - coords[2], coords[3] - coords[0]);
            n += Vector3.Cross(coords[0] - coords[3], coords[4] - coords[0]);
            n += Vector3.Cross(coords[0] - coords[4], coords[1] - coords[0]);
            noormals.Add(-n);

            #region horna polovica
            for (int i = 1; i < (NumOfParallels - 1) / 2; i++)
            {
                startIndex = 2 * i * (i - 1) + 1;
                endIndex = startIndex + (i * 4) - 1;
                int increment = i * 4;
                //System.Diagnostics.Debug.WriteLine("{0} ", i);
                for (int j = startIndex; j <= endIndex; j++)
                {
                    //System.Diagnostics.Debug.WriteLine(j.ToString());
                    if (j == startIndex)
                    {
                        if (j == 1)
                        {
                            n = Vector3.Cross(coords[j] - coords[j + increment], coords[j + increment + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment + 1], coords[j + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + 1], coords[0] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[0], coords[endIndex] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[endIndex], coords[endIndex + (i + 1) * 4] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[endIndex + (i + 1) * 4], coords[j + increment] - coords[j]);
                            noormals.Add(-n);
                        }
                        else
                        {
                            n = Vector3.Cross(coords[j] - coords[j + increment], coords[j + increment + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment + 1], coords[j + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + 1], coords[j - (i - 1) * 4] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - (i - 1) * 4], coords[endIndex] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[endIndex], coords[endIndex + (i + 1) * 4] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[endIndex + (i + 1) * 4], coords[j + increment] - coords[j]);
                            noormals.Add(-n);
                        }
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, 2 * (i + 1) * i + (i + 1) * 4, j + increment);
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment, j + increment + 1);
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment + 1, j + 1);
                    }
                    if (j == startIndex + i)
                    {
                        if (j == 2)
                        {
                            n = Vector3.Cross(coords[j] - coords[j + increment + 1], coords[j + increment + 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment + 2], coords[j + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + 1], coords[0] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[0], coords[startIndex] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[startIndex], coords[j + increment] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment], coords[j + increment + 1] - coords[j]);
                            noormals.Add(-n);
                        }
                        else
                        {
                            n = Vector3.Cross(coords[j] - coords[j + increment + 1], coords[j + increment + 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment + 2], coords[j + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + 1], coords[j - 1 - (i - 1) * 4] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - 1 - (i - 1) * 4], coords[j - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - 1], coords[j + increment] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment], coords[j + increment + 1] - coords[j]);
                            noormals.Add(-n);
                        }
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment, j + increment + 1);
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment + 1, j + increment + 2);
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment + 2, j + 1);
                    }
                    if (j == startIndex + 2 * i)
                    {
                        if (j == 3)
                        {
                            n = Vector3.Cross(coords[j] - coords[j + increment + 1], coords[j + increment + 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment + 2], coords[j + increment + 3] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment + 3], coords[endIndex] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[endIndex], coords[0] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[0], coords[j - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - 1], coords[j + increment + 1] - coords[j]);
                            noormals.Add(-n);
                        }
                        else
                        {
                            n = Vector3.Cross(coords[j] - coords[j + increment + 1], coords[j + increment + 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment + 2], coords[j + increment + 3] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment + 3], coords[j + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + 1], coords[j - (i - 1) * 4 - 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - (i - 1) * 4 - 2], coords[j - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - 1], coords[j + increment + 1] - coords[j]);
                            noormals.Add(-n);
                        }
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment + 1, j + increment + 2);
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment + 2, j + increment + 3);
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment + 3, j + 1);
                    }
                    if (j == startIndex + 3 * i)
                    {
                        if (j == endIndex)
                        {
                            //n = Vector3.Cross(coords[j] - coords[0], coords[j - 1] - coords[j]);
                            //n += Vector3.Cross(coords[j] - coords[j - 1], coords[j + increment + 2] - coords[j]);
                            //n += Vector3.Cross(coords[j] - coords[j + increment + 2], coords[j + increment + 3] - coords[j]);
                            //n += Vector3.Cross(coords[j] - coords[j + increment + 3], coords[j + increment + 4] - coords[j]);
                            //n += Vector3.Cross(coords[j] - coords[j + increment + 4], coords[startIndex] - coords[j]);
                            //n += Vector3.Cross(coords[j] - coords[startIndex], coords[0] - coords[j]);
                            //noormals.Add(-n);
                            n = Vector3.Cross(coords[j] - coords[0], coords[j - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - 1], coords[j + increment + 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment + 2], coords[j + increment + 3] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment + 3], coords[j + increment + 4] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment + 4], coords[1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[1], coords[0] - coords[j]);
                            noormals.Add(-n);
                            //noormals.Add(new Vector3(0.0f, 0.0f, 0.0f));
                        }
                        else
                        {
                            //n = Vector3.Cross(coords[j] - coords[j - increment - 3], coords[j - 1] - coords[j]);
                            //n += Vector3.Cross(coords[j] - coords[j - 1], coords[j + increment + 2] - coords[j]);
                            //n += Vector3.Cross(coords[j] - coords[j + increment + 2], coords[j + increment + 3] - coords[j]);
                            //n += Vector3.Cross(coords[j] - coords[j + increment + 3], coords[j + increment + 4] - coords[j]);
                            //n += Vector3.Cross(coords[j] - coords[j + increment + 4], coords[j + 1] - coords[j]);
                            //n += Vector3.Cross(coords[j] - coords[j + 1], coords[j - increment - 3] - coords[j]);
                            //noormals.Add(-n);
                            n = Vector3.Cross(coords[j] - coords[j - (i - 1) * 4 - 3], coords[j - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - 1], coords[j + increment + 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment + 2], coords[j + increment + 3] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment + 3], coords[j + increment + 4] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment + 4], coords[j + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + 1], coords[j - (i - 1) * 4 - 3] - coords[j]);
                            noormals.Add(-n);
                            //noormals.Add(new Vector3(0.0f, 0.0f, 0.0f));
                            //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment + 2, j + increment + 3);
                            //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment + 3, j + increment + 4);
                            //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment + 4, j + 1);
                        }
                    }
                    if (j > startIndex && j < startIndex + i)
                    {
                        n = Vector3.Cross(coords[j] - coords[j + increment + 1], coords[j + 1] - coords[j]);
                        n += Vector3.Cross(coords[j] - coords[j + 1], coords[j - (i - 1) * 4] - coords[j]);
                        n += Vector3.Cross(coords[j] - coords[j - (i - 1) * 4], coords[j - (i - 1) * 4 - 1] - coords[j]);
                        n += Vector3.Cross(coords[j] - coords[j - (i - 1) * 4 - 1], coords[j - 1] - coords[j]);
                        n += Vector3.Cross(coords[j] - coords[j - 1], coords[j + increment] - coords[j]);
                        n += Vector3.Cross(coords[j] - coords[j + increment], coords[j + increment + 1] - coords[j]);
                        noormals.Add(-n);
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment, j + increment + 1);
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment + 1, j + 1);
                    }
                    if (j > startIndex + i && j < startIndex + 2 * i)
                    {
                        n = Vector3.Cross(coords[j] - coords[j + increment + 1], coords[j + increment + 2] - coords[j]);
                        n += Vector3.Cross(coords[j] - coords[j + increment + 2], coords[j + 1] - coords[j]);
                        n += Vector3.Cross(coords[j] - coords[j + 1], coords[j - (i - 1) * 4 - 1] - coords[j]);
                        n += Vector3.Cross(coords[j] - coords[j - (i - 1) * 4 - 1], coords[j - (i - 1) * 4 - 2] - coords[j]);
                        n += Vector3.Cross(coords[j] - coords[j - (i - 1) * 4 - 2], coords[j - 1] - coords[j]);
                        n += Vector3.Cross(coords[j] - coords[j - 1], coords[j + increment + 1] - coords[j]);
                        noormals.Add(-n);
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment + 1, j + increment + 2);
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment + 2, j + 1);
                    }
                    if (j > startIndex + 2 * i && j < startIndex + 3 * i)
                    {
                        n = Vector3.Cross(coords[j] - coords[j - (i - 1) * 4 - 3], coords[j - 1] - coords[j]);
                        n += Vector3.Cross(coords[j] - coords[j - 1], coords[j + increment + 2] - coords[j]);
                        n += Vector3.Cross(coords[j] - coords[j + increment + 2], coords[j + increment + 3] - coords[j]);
                        n += Vector3.Cross(coords[j] - coords[j + increment + 3], coords[j + 1] - coords[j]);
                        n += Vector3.Cross(coords[j] - coords[j + 1], coords[j - (i - 1) * 4 - 2] - coords[j]);
                        n += Vector3.Cross(coords[j] - coords[j - (i - 1) * 4 - 2], coords[j - (i - 1) * 4 - 3] - coords[j]);
                        noormals.Add(-n);
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment + 2, j + increment + 3);
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment + 3, j + 1);
                    }
                    if (j > startIndex + 3 * i && j <= endIndex)
                    {
                        if (j != endIndex)
                        {
                            n = Vector3.Cross(coords[j] - coords[j - (i - 1) * 4 - 3], coords[j - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - 1], coords[j + increment + 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment + 2], coords[j + increment + 3] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment + 3], coords[j + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + 1], coords[j - (i - 1) * 4 - 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - (i - 1) * 4 - 2], coords[j - (i - 1) * 4 - 3] - coords[j]);
                            noormals.Add(-n);
                            //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment + 3, j + increment + 4);
                            //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment + 4, j + 1);
                        }
                        else
                        {
                            n = Vector3.Cross(coords[j] - coords[j - increment + 1], coords[j - increment] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - increment], coords[j - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - 1], coords[j + increment + 3] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment + 3], coords[j + increment + 4] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment + 4], coords[j + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + 1], coords[j - increment + 1] - coords[j]);
                            noormals.Add(-n);
                            //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment + 3, j + increment + 4);
                            //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment + 4, startIndex);
                        }
                    }
                }
            }
            #endregion

            #region dolna polovica
            for (int i = (NumOfParallels - 1) / 2; i >= 1; i--)
            {
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
                        if (i == (NumOfParallels - 1) / 2)
                        {
                            n = Vector3.Cross(coords[j] - coords[j + 1], coords[j + increment] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment], coords[endIndex] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[endIndex], coords[j - (i - 1) * 4] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - (i - 1) * 4], coords[j + 1] - coords[j]);
                            noormals.Add(n);
                        }
                        else
                        {
                            n = Vector3.Cross(coords[j] - coords[j - increment - 4], coords[j - increment - 3] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - increment - 3], coords[j + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + 1], coords[j + increment] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment], coords[endIndex] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[endIndex], coords[j - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - 1], coords[j - increment - 4] - coords[j]);
                            noormals.Add(n);
                        }
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment, j + 1);
                    }
                    if (j == startIndex + i)
                    {
                        if (i == (NumOfParallels - 1) / 2)
                        {
                            n = Vector3.Cross(coords[j] - coords[j + 1], coords[j + increment - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment - 1], coords[j - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - 1], coords[j - (i - 1) * 4 - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - (i - 1) * 4 - 1], coords[j + 1] - coords[j]);
                            noormals.Add(n);
                        }
                        else
                        {
                            n = Vector3.Cross(coords[j] - coords[j - increment - 3], coords[j - increment - 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - increment - 2], coords[j + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + 1], coords[j + increment - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment - 1], coords[j - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - 1], coords[j - increment - 4] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - increment - 4], coords[j - increment - 3] - coords[j]);
                            noormals.Add(n);
                        }
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment - 1, j + 1);
                    }
                    if (j == startIndex + 2 * i)
                    {
                        if (i == (NumOfParallels - 1) / 2)
                        {
                            n = Vector3.Cross(coords[j] - coords[j + 1], coords[j + increment - 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment - 2], coords[j - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - 1], coords[j - increment + 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - increment + 2], coords[j + 1] - coords[j]);
                            noormals.Add(n);
                        }
                        else
                        {
                            n = Vector3.Cross(coords[j] - coords[j - increment - 2], coords[j - increment - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - increment - 1], coords[j + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + 1], coords[j + increment - 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment - 2], coords[j - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - 1], coords[j - increment - 3] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - increment - 3], coords[j - increment - 2] - coords[j]);
                            noormals.Add(n);
                        }
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment - 2, j + 1); 
                    }
                    if (j == startIndex + 3 * i)
                    {
                        if (i == (NumOfParallels - 1) / 2)
                        {
                            n = Vector3.Cross(coords[j] - coords[j + 1], coords[j + increment - 3] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment - 3], coords[j - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - 1], coords[j - increment + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - increment + 1], coords[j + 1] - coords[j]);
                            noormals.Add(n);
                        }
                        else
                        {
                            if (j != endIndex)
                            {
                                n = Vector3.Cross(coords[j] - coords[j - increment - 1], coords[j - increment] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[j - increment], coords[j + 1] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[j + 1], coords[j + increment - 3] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[j + increment - 3], coords[j - 1] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[j - 1], coords[j - increment - 2] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[j - increment - 2], coords[j - increment - 1] - coords[j]);
                                noormals.Add(n);
                            }
                            else
                            {
                                n = Vector3.Cross(coords[j] - coords[j - increment - 1], coords[j - increment] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[j - increment], coords[startIndex] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[startIndex], coords[j + 1] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[j + 1], coords[j - 1] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[j - 1], coords[j - increment - 2] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[j - increment - 2], coords[j - increment - 1] - coords[j]);
                                noormals.Add(n);
                            }
                        }
                    }
                    if (j > startIndex && j < startIndex + i)
                    {
                        if (i == (NumOfParallels - 1) / 2)
                        {
                            n = Vector3.Cross(coords[j] - coords[j - 1], coords[j - (i - 1) * 4 - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - (i - 1) * 4 - 1], coords[j - (i - 1) * 4] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - (i - 1) * 4], coords[j + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + 1], coords[j + increment] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment], coords[j + increment - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment - 1], coords[j - 1] - coords[j]);
                            noormals.Add(n);
                        }
                        else
                        {
                            n = Vector3.Cross(coords[j] - coords[j - (i + 1) * 4 + 1], coords[j + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + 1], coords[j + increment] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment], coords[j + increment - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment - 1], coords[j - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - 1], coords[j - (i + 1) * 4] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - (i + 1) * 4], coords[j - (i + 1) * 4 + 1] - coords[j]);
                            noormals.Add(n);
                        }
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment - 1, j + increment);
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment, j + 1);
                    }
                    if (j > startIndex + i && j < startIndex + 2 * i)
                    {
                        if (i == (NumOfParallels - 1) / 2)
                        {
                            n = Vector3.Cross(coords[j] - coords[j - 1], coords[j - (i - 1) * 4 - 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - (i - 1) * 4 - 2], coords[j - (i - 1) * 4 - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - (i - 1) * 4 - 1], coords[j + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + 1], coords[j + increment - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment - 1], coords[j + increment - 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment - 2], coords[j - 1] - coords[j]);
                            noormals.Add(n);
                        }
                        else
                        {
                            n = Vector3.Cross(coords[j] - coords[j - (i + 1) * 4 + 1], coords[j - (i + 1) * 4 + 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - (i + 1) * 4 + 2], coords[j + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + 1], coords[j + increment - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment - 1], coords[j + increment - 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment - 2], coords[j - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - 1], coords[j - (i + 1) * 4 + 1] - coords[j]);
                            noormals.Add(n);
                        }
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment - 2, j + increment - 1);
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment - 1, j + 1);
                    }
                    if (j > startIndex + 2 * i && j < startIndex + 3 * i)
                    {
                        if (i == (NumOfParallels - 1) / 2)
                        {
                            n = Vector3.Cross(coords[j] - coords[j - 1], coords[j - (i - 1) * 4 - 3] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - (i - 1) * 4 - 3], coords[j - (i - 1) * 4 - 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - (i - 1) * 4 - 2], coords[j + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + 1], coords[j + increment - 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment - 2], coords[j + increment - 3] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment - 3], coords[j - 1] - coords[j]);
                            noormals.Add(n);
                        }
                        else
                        {
                            n = Vector3.Cross(coords[j] - coords[j + increment - 3], coords[j - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - 1], coords[j - increment - 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - increment - 2], coords[j - increment - 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - increment - 1], coords[j + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + 1], coords[j + increment - 2] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment - 2], coords[j + increment - 3] - coords[j]);
                            noormals.Add(n);
                        }
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment - 3, j + increment - 2);
                        //System.Diagnostics.Debug.WriteLine("{0}-->{1}-->{2}", j, j + increment - 2, j + 1);
                    }
                    if (j > startIndex + 3 * i && j <= endIndex)
                    {
                        if (i == (NumOfParallels - 1) / 2)
                        {
                            n = Vector3.Cross(coords[j] - coords[j - 1], coords[j - increment] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - increment], coords[j - increment + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j - increment + 1], coords[j + 1] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + 1], coords[j + increment - 3] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment - 3], coords[j + increment - 4] - coords[j]);
                            n += Vector3.Cross(coords[j] - coords[j + increment - 4], coords[j - 1] - coords[j]);
                            noormals.Add(n);
                        }
                        else
                        {
                            if (j != endIndex)
                            {
                                n = Vector3.Cross(coords[j] - coords[j + increment - 3], coords[j + increment - 4] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[j + increment - 4], coords[j - 1] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[j - 1], coords[j - increment - 1] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[j - increment - 1], coords[j - increment] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[j - increment], coords[j + 1] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[j + 1], coords[j + increment - 3] - coords[j]);
                                noormals.Add(n);
                            }
                            else
                            {
                                n = Vector3.Cross(coords[j] - coords[j + 1], coords[j + increment - 4] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[j + increment - 4], coords[j - 1] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[j - 1], coords[j - increment - 1] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[j - increment - 1], coords[j - increment] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[j - increment], coords[startIndex] - coords[j]);
                                n += Vector3.Cross(coords[j] - coords[startIndex], coords[j + 1] - coords[j]);
                                noormals.Add(n);
                            }
                        }
                    }
                }
            }
            #endregion

            //koniec
            n = Vector3.Cross(coords[endIndex + 1] - coords[endIndex], coords[startIndex] - coords[endIndex + 1]);
            n += Vector3.Cross(coords[endIndex + 1] - coords[startIndex], coords[startIndex + 1] - coords[endIndex + 1]);
            n += Vector3.Cross(coords[endIndex + 1] - coords[startIndex + 1], coords[endIndex - 1] - coords[endIndex + 1]);
            n += Vector3.Cross(coords[endIndex + 1] - coords[endIndex - 1], coords[endIndex] - coords[endIndex + 1]);
            noormals.Add(n);
        }

        private void InitSphere()
        {
            int p = 0;
            int startIndex = 0;
            int endIndex = 0;
            int tmp = 0;

            //Timova triangulacia
            #region cepicka
            vertices[p] = coords[0];
            normals[p] = noormals[0];
            color[p] = coolors[0]; p++;

            vertices[p] = coords[1];
            normals[p] = noormals[1];
            color[p] = coolors[1]; p++;

            vertices[p] = coords[2];
            normals[p] = noormals[2];
            color[p] = coolors[2]; p++;

            vertices[p] = coords[0];
            normals[p] = noormals[0];
            color[p] = coolors[0]; p++;

            vertices[p] = coords[2];
            normals[p] = noormals[2];
            color[p] = coolors[2]; p++;

            vertices[p] = coords[3];
            normals[p] = noormals[3];
            color[p] = coolors[3]; p++;

            vertices[p] = coords[0];
            normals[p] = noormals[0];
            color[p] = coolors[0]; p++;

            vertices[p] = coords[3];
            normals[p] = noormals[3];
            color[p] = coolors[3]; p++;

            vertices[p] = coords[4];
            normals[p] = noormals[4];
            color[p] = coolors[4]; p++;

            vertices[p] = coords[0];
            normals[p] = noormals[0];
            color[p] = coolors[0]; p++;

            vertices[p] = coords[4];
            normals[p] = noormals[4];
            color[p] = coolors[4]; p++;

            vertices[p] = coords[1];
            normals[p] = noormals[1];
            color[p] = coolors[1]; p++;
            #endregion

            #region horna polovica
            for (int i = 1; i < (NumOfParallels - 1) / 2; i++)
            {
                startIndex = 2 * i * (i - 1) + 1;
                endIndex = startIndex + (i * 4) - 1;
                int increment = i * 4;
                for (int j = startIndex; j <= endIndex; j++)
                {
                    if (j == startIndex)
                    {
                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[2 * (i + 1) * i + (i + 1) * 4];
                        normals[p] = noormals[2 * (i + 1) * i + (i + 1) * 4];
                        color[p] = coolors[2 * (i + 1) * i + (i + 1) * 4]; p++;

                        vertices[p] = coords[j + increment];
                        normals[p] = noormals[j + increment];
                        color[p] = coolors[j + increment]; p++;

                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment];
                        normals[p] = noormals[j + increment];
                        color[p] = coolors[j + increment]; p++;

                        vertices[p] = coords[j + increment + 1];
                        normals[p] = noormals[j + increment + 1];
                        color[p] = coolors[j + increment + 1]; p++;

                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment + 1];
                        normals[p] = noormals[j + increment + 1];
                        color[p] = coolors[j + increment + 1]; p++;

                        vertices[p] = coords[j + 1];
                        normals[p] = noormals[j + 1];
                        color[p] = coolors[j + 1]; p++;
                    }
                    if (j == startIndex + i)
                    {
                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment];
                        normals[p] = noormals[j + increment];
                        color[p] = coolors[j + increment]; p++;

                        vertices[p] = coords[j + increment + 1];
                        normals[p] = noormals[j + increment + 1];
                        color[p] = coolors[j + increment + 1]; p++;

                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment + 1];
                        normals[p] = noormals[j + increment + 1];
                        color[p] = coolors[j + increment + 1]; p++;

                        vertices[p] = coords[j + increment + 2];
                        normals[p] = noormals[j + increment + 2];
                        color[p] = coolors[j + increment + 2]; p++;

                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment + 2];
                        normals[p] = noormals[j + increment + 2];
                        color[p] = coolors[j + increment + 2]; p++;

                        vertices[p] = coords[j + 1];
                        normals[p] = noormals[j + 1];
                        color[p] = coolors[j + 1]; p++;
                    }
                    if (j == startIndex + 2 * i)
                    {
                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment + 1];
                        normals[p] = noormals[j + increment + 1];
                        color[p] = coolors[j + increment + 1]; p++;

                        vertices[p] = coords[j + increment + 2];
                        normals[p] = noormals[j + increment + 2];
                        color[p] = coolors[j + increment + 2]; p++;

                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment + 2];
                        normals[p] = noormals[j + increment + 2];
                        color[p] = coolors[j + increment + 2]; p++;

                        vertices[p] = coords[j + increment + 3];
                        normals[p] = noormals[j + increment + 3];
                        color[p] = coolors[j + increment + 3]; p++;

                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment + 3];
                        normals[p] = noormals[j + increment + 3];
                        color[p] = coolors[j + increment + 3]; p++;

                        vertices[p] = coords[j + 1];
                        normals[p] = noormals[j + 1];
                        color[p] = coolors[j + 1]; p++;
                    }
                    if (j == startIndex + 3 * i)
                    {
                        if (j == endIndex)//toto by malo nastat 2 krat
                        {
                            vertices[p] = coords[j];
                            normals[p] = noormals[j];
                            color[p] = coolors[j]; p++;

                            vertices[p] = coords[j + increment + 2];
                            normals[p] = noormals[j + increment + 2];
                            color[p] = coolors[j + increment + 2]; p++;

                            vertices[p] = coords[j + increment + 3];
                            normals[p] = noormals[j + increment + 3];
                            color[p] = coolors[j + increment + 3]; p++;

                            vertices[p] = coords[j];
                            normals[p] = noormals[j];
                            color[p] = coolors[j]; p++;

                            vertices[p] = coords[j + increment + 3];
                            normals[p] = noormals[j + increment + 3];
                            color[p] = coolors[j + increment + 3]; p++;

                            vertices[p] = coords[j + increment + 4];
                            normals[p] = noormals[j + increment + 4];
                            color[p] = coolors[j + increment + 4]; p++;

                            vertices[p] = coords[j];
                            normals[p] = noormals[j];
                            color[p] = coolors[j]; p++;

                            vertices[p] = coords[j + increment + 4];
                            normals[p] = noormals[j + increment + 4];
                            color[p] = coolors[j + increment + 4]; p++;

                            vertices[p] = coords[startIndex];
                            normals[p] = noormals[startIndex];
                            color[p] = coolors[startIndex]; p++;
                        }
                        else
                        {
                            vertices[p] = coords[j];
                            normals[p] = noormals[j];
                            color[p] = coolors[j]; p++;

                            vertices[p] = coords[j + increment + 2];
                            normals[p] = noormals[j + increment + 2];
                            color[p] = coolors[j + increment + 2]; p++;

                            vertices[p] = coords[j + increment + 3];
                            normals[p] = noormals[j + increment + 3];
                            color[p] = coolors[j + increment + 3]; p++;

                            vertices[p] = coords[j];
                            normals[p] = noormals[j];
                            color[p] = coolors[j]; p++;

                            vertices[p] = coords[j + increment + 3];
                            normals[p] = noormals[j + increment + 3];
                            color[p] = coolors[j + increment + 3]; p++;

                            vertices[p] = coords[j + increment + 4];
                            normals[p] = noormals[j + increment + 4];
                            color[p] = coolors[j + increment + 4]; p++;

                            vertices[p] = coords[j];
                            normals[p] = noormals[j];
                            color[p] = coolors[j]; p++;

                            vertices[p] = coords[j + increment + 4];
                            normals[p] = noormals[j + increment + 4];
                            color[p] = coolors[j + increment + 4]; p++;

                            vertices[p] = coords[j + 1];
                            normals[p] = noormals[j + 1];
                            color[p] = coolors[j + 1]; p++;
                        }
                    }
                    if (j > startIndex && j < startIndex + i)
                    {
                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment];
                        normals[p] = noormals[j + increment];
                        color[p] = coolors[j + increment]; p++;

                        vertices[p] = coords[j + increment + 1];
                        normals[p] = noormals[j + increment + 1];
                        color[p] = coolors[j + increment + 1]; p++;

                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment + 1];
                        normals[p] = noormals[j + increment + 1];
                        color[p] = coolors[j + increment + 1]; p++;

                        vertices[p] = coords[j + 1];
                        normals[p] = noormals[j + 1];
                        color[p] = coolors[j + 1]; p++;
                    }
                    if (j > startIndex + i && j < startIndex + 2 * i)
                    {
                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment + 1];
                        normals[p] = noormals[j + increment + 1];
                        color[p] = coolors[j + increment + 1]; p++;

                        vertices[p] = coords[j + increment + 2];
                        normals[p] = noormals[j + increment + 2];
                        color[p] = coolors[j + increment + 2]; p++;

                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment + 2];
                        normals[p] = noormals[j + increment + 2];
                        color[p] = coolors[j + increment + 2]; p++;

                        vertices[p] = coords[j + 1];
                        normals[p] = noormals[j + 1];
                        color[p] = coolors[j + 1]; p++;
                    }
                    if (j > startIndex + 2 * i && j < startIndex + 3 * i)
                    {
                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment + 2];
                        normals[p] = noormals[j + increment + 2];
                        color[p] = coolors[j + 2]; p++;

                        vertices[p] = coords[j + increment + 3];
                        normals[p] = noormals[j + increment + 3];
                        color[p] = coolors[j + increment + 3]; p++;

                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment + 3];
                        normals[p] = noormals[j + increment + 3];
                        color[p] = coolors[j + increment + 3]; p++;

                        vertices[p] = coords[j + 1];
                        normals[p] = noormals[j + 1];
                        color[p] = coolors[j + 1]; p++;
                    }
                    if (j > startIndex + 3 * i && j <= endIndex)
                    {
                        if (j != endIndex)
                        {
                            vertices[p] = coords[j];
                            normals[p] = noormals[j];
                            color[p] = coolors[j]; p++;

                            vertices[p] = coords[j + increment + 3];
                            normals[p] = noormals[j + increment + 3];
                            color[p] = coolors[j + increment + 3]; p++;

                            vertices[p] = coords[j + increment + 4];
                            normals[p] = noormals[j + increment + 4];
                            color[p] = coolors[j + increment + 4]; p++;

                            vertices[p] = coords[j];
                            normals[p] = noormals[j];
                            color[p] = coolors[j]; p++;

                            vertices[p] = coords[j + increment + 4];
                            normals[p] = noormals[j + increment + 4];
                            color[p] = coolors[j + increment + 4]; p++;

                            vertices[p] = coords[j + 1];
                            normals[p] = noormals[j + 1];
                            color[p] = coolors[j + 1]; p++;
                        }
                        else
                        {
                            vertices[p] = coords[j];
                            normals[p] = noormals[j];
                            color[p] = coolors[j]; p++;

                            vertices[p] = coords[j + increment + 3];
                            normals[p] = noormals[j + increment + 3];
                            color[p] = coolors[j + increment + 3]; p++;

                            vertices[p] = coords[j + increment + 4];
                            normals[p] = noormals[j + increment + 4];
                            color[p] = coolors[j + increment + 4]; p++;

                            vertices[p] = coords[j];
                            normals[p] = noormals[j];
                            color[p] = coolors[j]; p++;

                            vertices[p] = coords[j + increment + 4];
                            normals[p] = noormals[j + increment + 4];
                            color[p] = coolors[j + increment + 4]; p++;

                            vertices[p] = coords[startIndex];
                            normals[p] = noormals[startIndex];
                            color[p] = coolors[startIndex]; p++;
                        }
                    }
                }
            }
            #endregion

            #region dolna polovica
            for (int i = (NumOfParallels - 1) / 2; i >= 1; i--)
            {
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
                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment];
                        normals[p] = noormals[j + increment];
                        color[p] = coolors[j + increment]; p++;

                        vertices[p] = coords[j + 1];
                        normals[p] = noormals[j + 1];
                        color[p] = coolors[j + 1]; p++;
                    }
                    if (j == startIndex + i)
                    {
                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment - 1];
                        normals[p] = noormals[j + increment - 1];
                        color[p] = coolors[j + increment - 1]; p++;

                        vertices[p] = coords[j + 1];
                        normals[p] = noormals[j + 1];
                        color[p] = coolors[j + 1]; p++;
                    }
                    if (j == startIndex + 2 * i)
                    {
                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment - 2];
                        normals[p] = noormals[j + increment - 2];
                        color[p] = coolors[j + increment - 2]; p++;

                        vertices[p] = coords[j + 1];
                        normals[p] = noormals[j + 1];
                        color[p] = coolors[j + 1]; p++;
                    }
                    if (j == startIndex + 3 * i)
                    {
                        if (j == endIndex)
                        {
                            vertices[p] = coords[j];
                            normals[p] = noormals[j];
                            color[p] = coolors[j]; p++;

                            vertices[p] = coords[j + 1];
                            normals[p] = noormals[j + 1];
                            color[p] = coolors[j + 1]; p++;

                            vertices[p] = coords[startIndex];
                            normals[p] = noormals[startIndex];
                            color[p] = coolors[startIndex]; p++;
                        }
                        else
                        {
                            vertices[p] = coords[j];
                            normals[p] = noormals[j];
                            color[p] = coolors[j]; p++;

                            vertices[p] = coords[j + increment - 3];
                            normals[p] = noormals[j + increment - 3];
                            color[p] = coolors[j + increment - 3]; p++;

                            vertices[p] = coords[j + 1];
                            normals[p] = noormals[j + 1];
                            color[p] = coolors[j + 1]; p++;
                        }
                    }
                    if (j > startIndex && j < startIndex + i)
                    {
                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment - 1];
                        normals[p] = noormals[j + increment - 1];
                        color[p] = coolors[j + increment - 1]; p++;

                        vertices[p] = coords[j + increment];
                        normals[p] = noormals[j + increment];
                        color[p] = coolors[j + increment]; p++;

                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment];
                        normals[p] = noormals[j + increment];
                        color[p] = coolors[j + increment]; p++;

                        vertices[p] = coords[j + 1];
                        normals[p] = noormals[j + 1];
                        color[p] = coolors[j + 1]; p++;
                    }
                    if (j > startIndex + i && j < startIndex + 2 * i)
                    {
                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment - 2];
                        normals[p] = noormals[j + increment - 2];
                        color[p] = coolors[j + increment - 2]; p++;

                        vertices[p] = coords[j + increment - 1];
                        normals[p] = noormals[j + increment - 1];
                        color[p] = coolors[j + increment - 1]; p++;

                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment - 1];
                        normals[p] = noormals[j + increment - 1];
                        color[p] = coolors[j + increment - 1]; p++;

                        vertices[p] = coords[j + 1];
                        normals[p] = noormals[j + 1];
                        color[p] = coolors[j + 1]; p++;
                    }
                    if (j > startIndex + 2 * i && j < startIndex + 3 * i)
                    {
                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment - 3];
                        normals[p] = noormals[j + increment - 3];
                        color[p] = coolors[j + increment - 3]; p++;

                        vertices[p] = coords[j + increment - 2];
                        normals[p] = noormals[j + increment - 2];
                        color[p] = coolors[j + increment - 2]; p++;

                        vertices[p] = coords[j];
                        normals[p] = noormals[j];
                        color[p] = coolors[j]; p++;

                        vertices[p] = coords[j + increment - 2];
                        normals[p] = noormals[j + increment - 2];
                        color[p] = coolors[j + increment - 2]; p++;

                        vertices[p] = coords[j + 1];
                        normals[p] = noormals[j + 1];
                        color[p] = coolors[j + 1]; p++;
                    }
                    if (j > startIndex + 3 * i && j <= endIndex)
                    {
                        if (j != endIndex)
                        {
                            vertices[p] = coords[j];
                            normals[p] = noormals[j];
                            color[p] = coolors[j]; p++;

                            vertices[p] = coords[j + increment - 4];
                            normals[p] = noormals[j + increment - 4];
                            color[p] = coolors[j + increment - 4]; p++;

                            vertices[p] = coords[j + increment - 3];
                            normals[p] = noormals[j + increment - 3];
                            color[p] = coolors[j + increment - 3]; p++;

                            vertices[p] = coords[j];
                            normals[p] = noormals[j];
                            color[p] = coolors[j]; p++;

                            vertices[p] = coords[j + increment - 3];
                            normals[p] = noormals[j + increment - 3];
                            color[p] = coolors[j + increment - 3]; p++;

                            vertices[p] = coords[j + 1];
                            normals[p] = noormals[j + 1];
                            color[p] = coolors[j + 1]; p++;
                        }
                        else
                        {
                            vertices[p] = coords[j];
                            normals[p] = noormals[j];
                            color[p] = coolors[j]; p++;

                            vertices[p] = coords[j + increment - 4];
                            normals[p] = noormals[j + increment - 4];
                            color[p] = coolors[j + increment - 4]; p++;

                            vertices[p] = coords[j + 1];
                            normals[p] = noormals[j + 1];
                            color[p] = coolors[j + 1]; p++;

                            vertices[p] = coords[j];
                            normals[p] = noormals[j];
                            color[p] = coolors[j]; p++;

                            vertices[p] = coords[j + 1];
                            normals[p] = noormals[j + 1];
                            color[p] = coolors[j + 1]; p++;

                            vertices[p] = coords[startIndex];
                            normals[p] = noormals[startIndex];
                            color[p] = coolors[startIndex]; p++;
                        }
                    }
                }
            }
            #endregion

            System.Diagnostics.Debug.WriteLine("NumOfVertices = {0}", NumOfVertices);
        }

        private void SetColorList()
        {
            for (int i = 0; i < coords.Count; i++)
                coolors.Add(CalculateColor(coords[i].Z));
        }

        private Vector3 CalculateColor(float height)
        {
            Vector3 col = new Vector3(1.0f, 1.0f, 1.0f);
            float dv;
            dv = max - min;
            #region funkcie
            LinearFunction R1 = new LinearFunction(min + 0.35f * dv, min + 0.65f * dv, 0.0f, 1.0f);
            LinearFunction R2 = new LinearFunction(min + 0.9f * dv, max, 1.0f, 0.5f);
            LinearFunction G1 = new LinearFunction(min + 0.1f * dv, min + 0.35f * dv, 0.0f, 1.0f);
            LinearFunction G2 = new LinearFunction(min + 0.65f * dv, min + 0.9f * dv, 1.0f, 0.0f);
            LinearFunction B1 = new LinearFunction(0.0f, min + 0.1f * dv, 0.5f, 1.0f);
            LinearFunction B2 = new LinearFunction(min + 0.35f * dv, min + 0.65f * dv, 1.0f, 0.0f);
            #endregion

            if (height < min + 0.1f * dv)
            {
                col.X = col.Y = 0.0f;
                col.Z = B1.Value(height);
            }
            else if (height < min + 0.35f * dv)
            {
                col.X = 0.0f;
                col.Y = G1.Value(height);
            }
            else if (height < min + 0.65f * dv)
            {
                col.X = R1.Value(height);
                col.Z = B2.Value(height);
            }
            else if (height < min + 0.9f * dv)
            {
                col.Y = G2.Value(height);
                col.Z = 0.0f;
            }
            else
            {
                col.X = R2.Value(height);
                col.Y = col.Z = 0.0f;
            }
            return (col);
        }

        private void InitScene(bool b)
        {
            SetMatrices(b);
            GL.GenBuffers(3, VBO);
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

            //if (!VertexShader.LoadShader("..\\..\\Properties\\data\\shaders\\shader.vert", ShaderType.VertexShader))
            //    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");
            //if (!FragmentShader.LoadShader("..\\..\\Properties\\data\\shaders\\shader.frag", ShaderType.FragmentShader))
            //    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");

            //per pixel
            string vspath = string.Format("..{0}..{0}Properties{0}data{0}shaders{0}dirPerPixelShader.vert", Path.DirectorySeparatorChar);
            string fspath = string.Format("..{0}..{0}Properties{0}data{0}shaders{0}dirPerPixelShader.frag", Path.DirectorySeparatorChar);
            if (!VertexShader.LoadShader(vspath, ShaderType.VertexShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");
            if (!FragmentShader.LoadShader(fspath, ShaderType.FragmentShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");

            ////per fragment
            //string vspath = string.Format("..{0}..{0}Properties{0}data{0}shaders{0}dirShader.vert", Path.DirectorySeparatorChar);
            //string fspath = string.Format("..{0}..{0}Properties{0}data{0}shaders{0}dirShader.frag", Path.DirectorySeparatorChar);
            //if (!VertexShader.LoadShader(vspath, ShaderType.VertexShader))
            //    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");
            //if (!FragmentShader.LoadShader(fspath, ShaderType.FragmentShader))
            //    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");

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
            System.Diagnostics.Debug.WriteLine("min = {0}", min);
            System.Diagnostics.Debug.WriteLine("max = {0}", max);

            //float L = Math.Abs(max - min) * 5.0f;
            for (int i = 0; i < zaloha.Length; i++)
            {
                coords[i] = new Vector3(zaloha[i].X, zaloha[i].Y, zaloha[i].Z / (value*this.value));
                //coords[i] = new Vector3(coords[i].X, coords[i].Y, (coords[i].Z - min) / (float)L);
            }
        }

        private void GeoToSpatialCoords()
        {
            float rH, cosBxRAD, cosLxRAD, sinLxRAD, sinBxRAD;
            for (int i = 0; i < coords.Count; i++)
            {
                rH = R + coords[i].Z;
                cosBxRAD = (float)Math.Cos(coords[i].X * RAD);
                sinBxRAD = (float)Math.Sin(coords[i].X * RAD);
                cosLxRAD = (float)Math.Cos(coords[i].Y * RAD);
                sinLxRAD = (float)Math.Sin(coords[i].Y * RAD);
                coords[i] = new Vector3(rH * cosBxRAD * cosLxRAD, rH * cosBxRAD * sinLxRAD, rH * sinBxRAD);
            }
        }

        private void PustiToRychlejsie(float s)
        {
            Shaders.HeightMapGenerator hmg = new Shaders.HeightMapGenerator();
            for (int i = 0; i < coords.Count; i++)
                coords[i] = new Vector3(coords[i].X, coords[i].Y, hmg.GetNormalDistributedValue(coords[i].Z, s));
        }

        private void GetNumberOfTriangles()
        {
            for (int i = (NumOfParallels - 1) / 2; i > 0; i--)
                NumOfTriangles += i;

            NumOfTriangles = -2 * (NumOfParallels - 1) + 8 * NumOfTriangles;
            NumOfTriangles *= 2;//dve polgule
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
            width = w; height = h;
            SetMatrices(true);
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
            VBO = new int[3];
            VAO = new int[1];
            vertices=new Vector3[NumOfVertices];
            normals = new Vector3[NumOfVertices];
            color = new Vector3[NumOfVertices];
            noormals.Clear();
            noormals = new List<Vector3>();
            ScaleHeights(value);
            GeoToSpatialCoords();
            CalculateNormals();
            InitSphere();
            InitScene(true);
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

        public void DrawSphere()
        {
            GL.BindVertexArray(VAO[0]);
            //GL.DrawArrays(PrimitiveType.LineStrip, 0, NumOfVertices);
            GL.DrawArrays(PrimitiveType.Triangles, 0, NumOfVertices);
            //GL.DrawArrays(PrimitiveType.LineLoop, 0, NumOfVertices);
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

        public void Delete(bool b)
        {
            //zrejme by tu tiez mohla prist kontrola, ci znicenie programov prebehlo v poriadku, resp. ju zakomponovat do Shaders.dll
            if (b)
            {
                GL.DeleteBuffers(3, VBO);
                GL.DeleteVertexArrays(1, VAO);
                coords.Clear();
                noormals.Clear();
                coolors.Clear();
                spMain.DeleteProgram();
                VertexShader.DeleteShader();
                FragmentShader.DeleteShader();
            }
            else
            {
                GL.DeleteBuffers(3, VBO);
                GL.DeleteVertexArrays(1, VAO);
            }
        }
    }
}
