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
    class CubicSpline
    {
        private List<Vector2> uzloveBody;
        private Vector2[] pravaStrana;
        private Vector2[] riesenie;
        private Vector2[] a, b, c, d;
        private float[] diag, lower, upper;
        private int pocetBodov;

        public CubicSpline(List<Vector2> uzloveBody)
        {
            this.uzloveBody = uzloveBody;
            pocetBodov=uzloveBody.Count;

            //najprv zosortovat list podla item.Y hodnot
            Sort();
            NastavMaticu();
            NastavPravuStranu();
            ThomasovAlgoritmus();
            NastavKoeficienty();
        }

        #region vypocet Splajnu

        private void NastavMaticu()
        {
            diag = new float[pocetBodov];
            lower = new float[pocetBodov];
            upper = new float[pocetBodov];

            for (int i = 0; i < pocetBodov - 1; i++)
                upper[i] = 1;
            upper[pocetBodov - 1] = 0;

            for (int i = 1; i < pocetBodov; i++)
                lower[i] = 1;
            lower[0] = 0;

            diag[0] = 2;
            for (int i = 1; i < pocetBodov - 1; i++)
                diag[i] = 4;
            diag[pocetBodov - 1] = 2;
        }

        private void NastavPravuStranu()
        {
            pravaStrana = new Vector2[pocetBodov];

            //prva pravaStrana
            //pravaStrana[0] = new Vector2(3 * (uzloveBody[1].X - uzloveBody[0].X), 3 * (uzloveBody[1].Y - uzloveBody[0].Y));
            pravaStrana[0] = 3 * (uzloveBody[1] - uzloveBody[0]);
            //2. az (n-1).
            for (int i = 1; i < pocetBodov - 1; i++)
            {
                //pravaStrana[i] = new Vector2(3 * (uzloveBody[i + 1].X - uzloveBody[i - 1].X), 3 * (uzloveBody[i + 1].Y - uzloveBody[i - 1].Y));
                pravaStrana[i] = 3 * (uzloveBody[i + 1] - uzloveBody[i - 1]);
            }
            //n-ta(posledna) pravaStrana
            //pravaStrana[pocetBodov - 1] = new Vector2(3 * (uzloveBody[pocetBodov - 1].X - uzloveBody[pocetBodov - 2].X), 3 * (uzloveBody[pocetBodov - 1].Y - uzloveBody[pocetBodov - 2].Y));
            pravaStrana[pocetBodov - 1] = 3 * (uzloveBody[pocetBodov - 1] - uzloveBody[pocetBodov - 2]);
        }

        private void ThomasovAlgoritmus()
        {
            riesenie = new Vector2[pocetBodov];
            //priprava
            for (int i = 1; i < pocetBodov; i++)
            {
                diag[i] = diag[i] - (lower[i] * upper[i - 1]) / (float)diag[i - 1];
                Vector2 tmp = new Vector2(pravaStrana[i]);
                pravaStrana[i] = tmp - (pravaStrana[i - 1] * lower[i]) / (float)diag[i - 1];
            }

            //posledne riesenie (D[last])
            riesenie[pocetBodov - 1] = pravaStrana[pocetBodov - 1] / (float)diag[pocetBodov - 1];
            //vypocet zvysnych rieseni
            for (int i = riesenie.Length - 2; i >= 0; i--)
                riesenie[i] = (pravaStrana[i] - (upper[i] * riesenie[i + 1])) / (float)diag[i];
        }

        private void NastavKoeficienty()
        {
            a = new Vector2[pocetBodov - 1];
            b = new Vector2[pocetBodov - 1];
            c = new Vector2[pocetBodov - 1];
            d = new Vector2[pocetBodov - 1];

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = 2 * (uzloveBody[i] - uzloveBody[i + 1]) + riesenie[i] + riesenie[i + 1];
                b[i] = 3 * (uzloveBody[i + 1] - uzloveBody[i]) - 2 * riesenie[i] - riesenie[i + 1];
                c[i] = riesenie[i];
                d[i] = uzloveBody[i];
            }
        }

        public Vector2 BodKrivky(int isovalue)
        {
            Vector2 tmp=new Vector2();

            if (isovalue < 0 || isovalue > 256)
                System.Windows.Forms.MessageBox.Show("Hodnota musi byt z intervalu [0,256]!");
            else
            {
                for (int i = 0; i < uzloveBody.Count - 1 ; i++)
                {
                    if (uzloveBody[i].Y <= isovalue && isovalue <= uzloveBody[i + 1].Y)
                    {
                        float tmp_isovalue = isovalue / (float)256.0f;
                        //tmp = (((d[i] * tmp_isovalue) + c[i]) * tmp_isovalue + b[i]) * tmp_isovalue + a[i];
                        tmp = a[i] * tmp_isovalue * tmp_isovalue * tmp_isovalue + b[i] * tmp_isovalue * tmp_isovalue + c[i] * tmp_isovalue + d[i];
                        break;
                    }
                }
            }
            return tmp;
        }

        public void  Krivka()
        {
            Vector2 tmp = new Vector2();
            for (int i = 0; i < uzloveBody.Count - 1; i++)
            {
                int steps = (int)(uzloveBody[i + 1].Y - uzloveBody[i].Y);

                for (int j = 0; j < steps; j++)
                {
                    float k = (float)j / (float)(steps - 1);
                    tmp = BodKrivky(k,i);
                    System.Diagnostics.Debug.Write("{" + tmp.X + "," + tmp.Y + "},");
                }
            }
        }

        private Vector2 BodKrivky(float k,int i)
        {
            //return new Vector2((((a[i] * k) + b[i]) * k + c[i]) * k + d[i]);
            Vector2 tmp = a[i] * k * k * k + b[i] * k * k + c[i] * k + d[i];
            if (tmp.X < 0.0f)
                tmp.X = 0.0f;
            if (tmp.X > 1.0f)
                tmp.X = 1.0f;
            return tmp;
        }

        public Vector2 BodKrivky(float isovalue)
        {
            StreamWriter sw = new StreamWriter("file.txt",true);

            Vector2 tmp = new Vector2();
            int val = (int)Math.Round(256 * isovalue);

            if (isovalue < 0.0f || isovalue > 1.0f)
                System.Windows.Forms.MessageBox.Show("Hodnota musi byt z intervalu [0,1]!");
            else
            {
                for (int i = 0; i < uzloveBody.Count - 1; i++)
                {
                    //if (uzloveBody[i].X <= isovalue && isovalue <= uzloveBody[i + 1].X)
                    //{
                        //tmp = (((d[i] * isovalue) + c[i]) * isovalue + b[i]) * isovalue + a[i];
                        tmp = (((a[i] * isovalue) + b[i]) * isovalue + c[i]) * isovalue + d[i];
                        sw.Write("{" + tmp.X + "," + tmp.Y + "},");
                    //    break;
                    //}
                }
                sw.Flush();
                sw.Close();
            }
            return tmp;
        }

        #endregion

        private void Sort() { uzloveBody.Sort((x, y) => x.Y.CompareTo(y.Y)); }
    }
}
