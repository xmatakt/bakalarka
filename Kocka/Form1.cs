using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Kocka
{
    public partial class Form1 : Form
    {
        private bool loaded, resize,sfera; 
        //private Stvorec3D kocka;
        private KockaInak kocka;
        private Sphere sphere;
        float scale;
        float dx, dy;
        float wPol, hPol;
        float wLomeno2, hLomeno2;

        public Form1()
        {
            InitializeComponent();
            loaded = false;
            resize = false;
            sfera = volacoToolStripMenuItem.Checked;
            scale = 1.0f;
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            wPol = glControl1.Width / 2.0f;
            hPol = glControl1.Height / 2.0f;
            wLomeno2 = 2.0f / (float)glControl1.Width;
            hLomeno2 = 2.0f / (float)glControl1.Height;
            GL.ClearColor(Color.Black);
            //GL.Enable(EnableCap.CullFace);
            //GL.CullFace(CullFaceMode.Back);
            //GL.FrontFace(FrontFaceDirection.Ccw);

            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            if(sfera)
            {
                sphere = new Sphere(glControl1.Width, glControl1.Height, scale);
                sphere.DrawSphere();
            }
            else
            {
                kocka = new KockaInak(glControl1.Width, glControl1.Height, scale);
            }
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded)
                return;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if(sfera)
                sphere.DrawSphere();
            else
                kocka.PrekresliKocku();
            

            glControl1.SwapBuffers();
        }

        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                //
                case Keys.Escape:
                    this.Close();
                    break;
                //
                case Keys.W:
                    if(sfera)
                        sphere.Pohyb(0.1f);
                    else
                        kocka.Pohyb(0.1f);
                    glControl1.Invalidate();
                    break;
                case Keys.S:
                    if (sfera)
                        sphere.Pohyb(-0.1f);
                    else
                        kocka.Pohyb(-0.1f);
                    glControl1.Invalidate();
                    break;
                case Keys.A:
                    if (sfera)
                        sphere.Natoc(-1.3f);
                    else
                        kocka.Natoc(-1.3f);
                    glControl1.Invalidate();
                    break;
                case Keys.D:
                    if (sfera)
                        sphere.Natoc(1.3f);
                    else
                        kocka.Natoc(1.3f);
                    glControl1.Invalidate();
                    break;
                //
                default:
                    break;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(sfera)
                sphere.Delete();
            else
                kocka.Delete();
           
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
                if(sfera)
                {
                    sphere.Scale(scale);
                    sphere.Transalte(wLomeno2 * tmpx, hLomeno2 * tmpy);
                }
                else
                {
                    kocka.Scale(scale);
                    kocka.Translate(wLomeno2 * tmpx, hLomeno2 * tmpy);
                }
                glControl1.Invalidate();
            }
            //rotacia
            if (e.Button == MouseButtons.Left)
            {
                float tmpx = (e.Location.X - dx) / wPol;
                float tmpy = (e.Location.Y - dy) / hPol;
                float angle = (float)Math.Sqrt(tmpx * tmpx + tmpy * tmpy);
                if(sfera)
                {
                    sphere.Scale(scale);
                    sphere.Rotate(tmpx, tmpy, angle);
                }
                else
                {
                    kocka.Scale(scale);
                    kocka.Rotate(tmpx, tmpy, angle);
                }
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

                if (sfera)
                    sphere.Scale(scale);
                else
                    kocka.Scale(scale);
               
                glControl1.Invalidate();
            }
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            //posuvanie//skalovanie//rotacia
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle || e.Button == MouseButtons.Right)
            {
                if(sfera)
                    sphere.Ende();
                else
                    kocka.Ende();
            }
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            if (!resize)
            {
                resize = true;
            }
            else
            {
                GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
                if(sfera)
                {
                    sphere.Resize(glControl1.Width, glControl1.Height);
                    glControl1.Invalidate();
                }
                else
                {
                    kocka.Resize(glControl1.Width, glControl1.Height);
                    glControl1.Invalidate();
                }
            }
        }

        private void volacoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sfera = volacoToolStripMenuItem.Checked = !volacoToolStripMenuItem.Checked;
        }

        private void volacoToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if(sfera)
            {
                sphere.Delete();
                Reset();
                kocka = new KockaInak(glControl1.Width, glControl1.Height, scale);
            }
            else
            {
                kocka.Delete();
                Reset();
                sphere = new Sphere(glControl1.Width, glControl1.Height, scale);
            }
            glControl1.Invalidate();
            //MessageBox.Show("calamada z tristohrnc ov");
        }

        private void Reset()
        {
            wPol = glControl1.Width / 2.0f;
            hPol = glControl1.Height / 2.0f;
            wLomeno2 = 2.0f / (float)glControl1.Width;
            hLomeno2 = 2.0f / (float)glControl1.Height;
            scale = 1.0f;
        }
    }
}
