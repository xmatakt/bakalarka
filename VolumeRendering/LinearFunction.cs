using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolumeRendering
{
    class LinearFunction
    {
        private float k;
        private float q;

        public LinearFunction(float x1, float x2, float h1, float h2)
        {
            k = (h2 - h1) / (x2 - x1);
            q = h1 - x1 * k;
        }

        public float Value(float x)
        {
            return k * x + q;
        }
    }
}
