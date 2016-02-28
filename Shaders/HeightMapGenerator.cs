using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaders
{
    public class HeightMapGenerator
    {
        private int width, NumberOfVertices,NumberOfNodes;//udava, na kolko casti je rozdelena os X,resp. os Y
        private float min, max;
        private float[] map;
        private int[] Indices;
        Random X;
        
        public HeightMapGenerator(int power)
        {
            
            int tmp = (int)Math.Pow(2, power - 1);
            NumberOfVertices = (tmp + 1) * (tmp + 1);
            NumberOfNodes = tmp + 1;

            width = (int)Math.Pow(2, power);
            map = new float[NumberOfVertices];
            min = -10.0f; max = 1.0f;
            X = new Random();//asi treba dat dajaky seed;

            //kontrolny vypis
            System.Diagnostics.Debug.WriteLine("power = {0}",power);
            System.Diagnostics.Debug.WriteLine("NumberOfVertices = {0}", NumberOfVertices);
            System.Diagnostics.Debug.WriteLine("NumberOfNodes = {0}", NumberOfNodes);
            //

            InitMap();
            GenerateMap(0,0,width,width);
            GenerateArrayOfIndices();
        }

        float Trim(float c)
        {
            if (c < min)
                return min;
            if (c > max)
                return max;
            return c;
        }

        //toto asi lepsie nepouzivat
        private float GetNormalDistributedValue(float mu,float sigma)
        {
            float u = 2.0f * ((float)X.NextDouble()) - 1.0f;
            float v = 2.0f * ((float)X.NextDouble()) - 1.0f;
            float s = u * u + v * v;

            while (s == 0 || s >= 1)
            {
                u = 2.0f * ((float)X.NextDouble()) - 1.0f;
                v = 2.0f * ((float)X.NextDouble()) - 1.0f;
                s = u * u + v * v;
            }

            float z0 = u * (float)Math.Sqrt((-2 * Math.Log(s)) / s);
            float z1 = v * (float)Math.Sqrt((-2 * Math.Log(s)) / s);
            if (X.Next(0,200) % 2 == 0)
                return z0 * sigma + mu;
            else
                return z1 * sigma + mu;
        }

        //nastavenie rohovych vrcholov/vysok
        private void InitMap()
        {
        //    map[0 / 2 + NumberOfNodes * 0 / 2] = GetNormalDistributedValue(0, 10);
        //    map[width / 2 + NumberOfNodes * 0 / 2] = GetNormalDistributedValue(0, 10);
        //    map[width / 2 + NumberOfNodes * width / 2] = GetNormalDistributedValue(0, 10);
        //    map[0 / 2 + NumberOfNodes * width / 2] = GetNormalDistributedValue(0, 10);
            map[0 / 2 + NumberOfNodes * 0 / 2] = 0.0f;
            map[width / 2 + NumberOfNodes * 0 / 2] = 0.0f;
            map[width / 2 + NumberOfNodes * width / 2] = 0.0f;
            map[0 / 2 + NumberOfNodes * width / 2] = 0.0f;
        }

        private void GenerateMap(int x1, int y1, int x2, int y2)
        {
            if ((int)Math.Abs(x2 - x1) <= 2)
                return;
            else
            {
                int sPolX = (x2 - x1) / 2; //stred strany
                int sPolY = (y2 - y1) / 2; //stred strany
                //        suradnice novych vrcholov
                int xN1 = x1 + sPolX;
                int yN1 = y1;
                int xN2 = x2;
                int yN2 = y1 + sPolY;
                int xN3 = xN1;
                int yN3 = y2;
                int xN4 = x1;
                int yN4 = y1 + sPolY;
                int xN5 = x1 + sPolX;
                int yN5 = y1 + sPolY;
                //        vysky vo vstupnych vrcholoch
                float v1 = map[x1 / 2 + NumberOfNodes * y1 / 2];
                float v2 = map[x2 / 2 + NumberOfNodes * y1 / 2];
                float v3 = map[x2 / 2 + NumberOfNodes * y2 / 2];
                float v4 = map[x1 / 2 + NumberOfNodes * y2 / 2];
                //        vypocet vysky v stredoch stran a strede stvorca
                float v1N = (v1 + v2) / 2.0f;//+fabs(Trim(GetNormalDistributedValue(0,xN2-xN1)));
                float v2N = (v2 + v3) / 2.0f;//+fabs(Trim(GetNormalDistributedValue(0,yN2-yN1)));
                float v3N = (v3 + v4) / 2.0f;//+fabs(Trim(GetNormalDistributedValue(0,xN2-xN1)));
                float v4N = (v1 + v4) / 2.0f;//+fabs(Trim(GetNormalDistributedValue(0,yN2-yN1)));
                //        double v5N=(v1+v2+v3+v4)/4.0+ Orez( GetNormalDistributedValue(0,(sqrt(2*(xN2-xN1)*(xN2-xN1))/(double)2) ) ) ;
                float v5N = (v1 + v2 + v3 + v4) / 4.0f + Trim(GetNormalDistributedValue(0, (xN2 - xN1) / 2.0f));
                //        nastavenie vysky pre nove vrcholy
                map[xN1 / 2 + NumberOfNodes * yN1 / 2] = v1N;
                map[xN2 / 2 + NumberOfNodes * yN2 / 2] = v2N;
                map[xN3 / 2 + NumberOfNodes * yN3 / 2] = v3N;
                map[xN4 / 2 + NumberOfNodes * yN4 / 2] = v4N;
                map[xN5 / 2 + NumberOfNodes * yN5 / 2] = v5N;

                //        docasny kontrolny vypis
                //        printf("++++++++++++++++++++\n");
                //        printf("v(%d,%d)=%lf\n",x1,y1,v1);
                //        printf("v(%d,%d)=%lf\n",x2,y1,v2);
                //        printf("v(%d,%d)=%lf\n",x2,y2,v3);
                //        printf("v(%d,%d)=%lf\n",x1,y2,v4);
                //        printf("++++++++++++++++++++\n");

                //      rekurzia=zavolanie funkcie na novovzniknute stvorce
                GenerateMap(x1, y1, xN5, yN5);  //1. stverec
                GenerateMap(xN1, yN1, xN2, yN2);//2. stverec
                GenerateMap(xN5, yN5, x2, y2);  //3. stverec
                GenerateMap(xN4, yN4, xN3, yN3);//4. stverec
            }
        }

        private void GenerateArrayOfIndices()
        {
            int k = 0;
            int tmp = NumberOfNodes * NumberOfNodes;
            Indices = new int[NumberOfNodes * (NumberOfNodes - 1) * 2 + NumberOfNodes - 1];

            for (int i = NumberOfNodes; i < tmp; i++)
            {
                Indices[k] = i;
                k++;
                Indices[k] = i - NumberOfNodes;
                k++;
                if((i%NumberOfNodes==NumberOfNodes-1) && i!=tmp-1)
                {
                    Indices[k] = tmp;
                    k++;
                }
            }

            //Kontrolny vypis
            System.Diagnostics.Debug.WriteLine("{0} ?=? {1}", NumberOfNodes * (NumberOfNodes - 1) * 2 + NumberOfNodes - 1,k+1);
            //
        }

        public float[] ReturnHeightMap()
        {
            return map;
        }

        public int[] ReturnIndices()
        {
            return Indices;
        }

        public int ReturnNumbreOfNodes()
        {
            return NumberOfNodes;
        }
    }
}
