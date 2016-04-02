﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace VolumeRendering
{
    class TransferFunction
    {
        private List<Vector2> OpacityList;//o.Y [0,256] --> isovalue pre  o.X --> priehladnost [0,1]
        private List<Vector4> ColorList;//c.r,c.g,c.b --> aka ma byt farba pre c.w --> isovalue
        private byte[] Opacity;
        private byte[] Red;
        private byte[] Green;
        private byte[] Blue;
        private byte[] transferFunction;

        public TransferFunction(List<Vector2> opacity,List<Vector4> colors)
        {
            transferFunction = new byte[1024];
            OpacityList = new List<Vector2>(opacity);
            ColorList = new List<Vector4>(colors);

            Sort2();
            Sort4();

            SetTransferFunction();
        }

        private void SetTransferFunction()
        {
            SetOpacityArray();
            SetColorArrays();
            SetTransferFunctionArray();
        }

        private void SetOpacityArray()
        {
            Opacity = new byte[256];
            for (int i = 0; i < OpacityList.Count-1; i++)
            {
                LinearFunction lf = new LinearFunction(OpacityList[i].Y, OpacityList[i + 1].Y, OpacityList[i].X, OpacityList[i+1].X);
                for (int j = (int)OpacityList[i].Y; j < (int)OpacityList[i+1].Y; j++)
                {
                    Opacity[j] = (byte)(255 * lf.Value(j));
                }
            }
        }

        private void SetColorArrays()
        {
            Red = new byte[256];
            Green = new byte[256];
            Blue = new byte[256];

            for (int i = 0; i < ColorList.Count - 1; i++)
            {
                LinearFunction lf_r = new LinearFunction(ColorList[i].W, ColorList[i + 1].W, ColorList[i].X, ColorList[i + 1].X);
                LinearFunction lf_g = new LinearFunction(ColorList[i].W, ColorList[i + 1].W, ColorList[i].Y, ColorList[i + 1].Y);
                LinearFunction lf_b = new LinearFunction(ColorList[i].W, ColorList[i + 1].W, ColorList[i].Z, ColorList[i + 1].Z);
                for (int j = (int)ColorList[i].W; j < (int)ColorList[i + 1].W; j++)
                {
                    Red[j] = (byte)(255 * lf_r.Value(j));
                    Green[j] = (byte)(255 * lf_g.Value(j));
                    Blue[j] = (byte)(255 * lf_b.Value(j));
                    //System.Diagnostics.Debug.WriteLine("Red[" + j + "] = " + Blue[j]);
                }
            }
        }

        private void SetTransferFunctionArray()
        {
            int counterR = 0;
            int counterG = 0;
            int counterB = 0;
            int counterO = 0;
            for (int i = 0; i < 1024; i++)
            {
                if (i % 4 == 0)//r
                    transferFunction[i] = Red[counterR++];
                if (i % 4 == 1)//g
                    transferFunction[i] = Green[counterG++];
                if (i % 4 == 2)//b
                    transferFunction[i] = Blue[counterB++];
                if (i % 4 == 3)//w
                    transferFunction[i] = Opacity[counterO++];
            }
        }

        public byte[] GetTransferFunction() { return transferFunction; }

        private void Sort2() { OpacityList.Sort((x, y) => x.X.CompareTo(y.X)); }
        private void Sort4() { ColorList.Sort((x, y) => x.W.CompareTo(y.W)); }

    }
}