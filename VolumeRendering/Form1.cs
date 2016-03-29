using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace VolumeRendering
{
    public partial class Form1 : Form
    {
        private bool loaded;
        float scale;
        float dx, dy;
        float wPol, hPol;
        private Volume volume;

        public Form1()
        {
            InitializeComponent();
            scale = 1.0f;
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            wPol = glControl1.Width / 2.0f;
            hPol = glControl1.Height / 2.0f;
            GL.ClearColor(Color.Black);

            //volume = new Volume("BostonTeapot.raw", glControl1.Width, glControl1.Height);
            volume = new Volume("head256.raw",glControl1.Width, glControl1.Height);
            //volume = new Volume("Skull.raw", glControl1.Width, glControl1.Height);
            //volume = new Volume("foot.raw", glControl1.Width, glControl1.Height);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded)
                return;

            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            volume.Display();

            glControl1.SwapBuffers();
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            //posuvanie//skalovanie//rotacia
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle || e.Button == MouseButtons.Right)
            {
                dx = e.Location.X;
                dy = e.Location.Y;
            }
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            //posuvanie
            if (e.Button == MouseButtons.Middle)
            {
                float tmpx = e.Location.X - dx;
                float tmpy = dy - e.Location.Y;
                //if (sfera)
                //{
                //    sdat.Scale(scale);
                //    sdat.Transalte(wLomeno2 * tmpx, hLomeno2 * tmpy);
                //}
                glControl1.Invalidate();
            }

            //rotacia
            if (e.Button == MouseButtons.Left)
            {
                float tmpx = (e.Location.X - dx) / wPol;
                float tmpy = (e.Location.Y - dy) / hPol;
                float angle = (float)Math.Sqrt(tmpx * tmpx + tmpy * tmpy);
                volume.Scale(scale);
                volume.Rotate(tmpx, tmpy, angle);
                glControl1.Invalidate();
            }
            //skalovanie
            if (e.Button == MouseButtons.Right)
            {
                if (scale > 1.0f)
                    scale += ((float)dy - e.Location.Y) * scale / 500f;
                else
                    scale += ((float)dy - e.Location.Y) / 500f;

                dy = e.Location.Y;

                //if (sfera)
                //    sdat.Scale(scale);

                glControl1.Invalidate();
            }
        }
        
        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            //posuvanie//skalovanie//rotacia
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle || e.Button == MouseButtons.Right)
            {
                volume.Ende();
            }
            glControl1.Invalidate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            volume.Delete();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            if (loaded)
            {
                volume.Delete();
                volume = new Volume("head256.raw",glControl1.Width, glControl1.Height);
            }
        }
    }
}
