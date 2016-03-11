using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Kocka
{
    public partial class Form1 : Form
    {
        private bool loaded, resize,sfera,shader; 
        //private Stvorec3D kocka;
        private SphereDAT sdat;
        float scale;
        float dx, dy;
        float wPol, hPol;
        float wLomeno2, hLomeno2;

        public Form1()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

            loaded = false;
            resize = false;
            sfera = false;
            shader = PerFragment.Checked;
            scale = 1.0f;
            //sdat = new SphereDAT("..\\..\\Properties\\data\\datFiles\\data_const.dat");
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            wPol = glControl1.Width / 2.0f;
            hPol = glControl1.Height / 2.0f;
            wLomeno2 = 2.0f / (float)glControl1.Width;
            hLomeno2 = 2.0f / (float)glControl1.Height;
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.ClearDepth(1.0);
            //GL.Enable(EnableCap.CullFace);
            //GL.CullFace(CullFaceMode.Back);
            //GL.FrontFace(FrontFaceDirection.Ccw);

            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            if(sfera)
            {
                //sdat = new SphereDAT(glControl1.Width, glControl1.Height, "..\\..\\Properties\\data\\datFiles\\data01.dat");
                //sdat.DrawSphere();
                //sphere = new Sphere(glControl1.Width, glControl1.Height, scale,(int)Pi.Value,(int)DvaPi.Value, flat,shader);
                //sphere = new Sphere(glControl1.Width, glControl1.Height, scale, "sfera.txt");
                //sphere.DrawSphere();
            }
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded)
                return;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (sfera)
                sdat.DrawSphere();
            //sphere.DrawSphere();

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
                    if (sfera)
                        sdat.MoveCamera(0.1f);
                    glControl1.Invalidate();
                    break;
                case Keys.S:
                    if (sfera)
                        sdat.MoveCamera(-0.1f);
                    glControl1.Invalidate();
                    break;
                case Keys.A:
                    if (sfera)
                        //sdat.Natoc(-1.3f);
                    glControl1.Invalidate();
                    break;
                case Keys.D:
                    if (sfera)
                        //sdat.Natoc(1.3f);
                    glControl1.Invalidate();
                    break;
                //
                default:
                    break;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sfera)
                sdat.Delete();
            //sphere.Delete();
           
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
                    //sphere.Scale(scale);
                    //sphere.Transalte(wLomeno2 * tmpx, hLomeno2 * tmpy);
                    sdat.Scale(scale);
                    sdat.Transalte(wLomeno2 * tmpx, hLomeno2 * tmpy);
                }
                glControl1.Invalidate();
            }
            //rotacia
            if (e.Button == MouseButtons.Left)
            {
                float tmpx = (e.Location.X - dx) / wPol;
                float tmpy = (e.Location.Y - dy) / hPol;
                float angle = (float)Math.Sqrt(tmpx * tmpx + tmpy * tmpy);
                if (sfera)
                {
                    //sphere.Scale(scale);
                    //sphere.Rotate(tmpx, tmpy, angle);
                    sdat.Scale(scale);
                    sdat.Rotate(tmpx, tmpy, angle);
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
                    sdat.Scale(scale);
                    //sphere.Scale(scale);
               
                glControl1.Invalidate();
            }
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            //posuvanie//skalovanie//rotacia
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle || e.Button == MouseButtons.Right)
            {
                if (sfera)
                    sdat.Ende();
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
                    sdat.Resize(glControl1.Width, glControl1.Height);
                    //sphere.Resize(glControl1.Width, glControl1.Height);
                    glControl1.Invalidate();
                }
            }
        }

        private void Reset()
        {
            wPol = glControl1.Width / 2.0f;
            hPol = glControl1.Height / 2.0f;
            wLomeno2 = 2.0f / (float)glControl1.Width;
            hLomeno2 = 2.0f / (float)glControl1.Height;
            scale = 1.0f;
        }

        private void SpecLx_ValueChanged(object sender, EventArgs e)
        {
            if(sfera)
            {
                Vector3 specular = new Vector3((float)SpecLx.Value, (float)SpecLy.Value,(float)SpecLz.Value);
                Vector3 ambient = new Vector3((float)AmbLx.Value, (float)AmbLy.Value, (float)AmbLz.Value);
                Vector3 diffuse = new Vector3((float)DiffLx.Value, (float)DiffLy.Value, (float)DiffLz.Value);
                Vector3 direction = new Vector3((float)DirLx.Value, (float)DirLy.Value, (float)DirLz.Value);
                //sphere.SetLight(specular,ambient,diffuse,direction);
            }
            glControl1.Invalidate();
        }

        private void SpecMx_ValueChanged(object sender, EventArgs e)
        {
            if (sfera)
            {
                float specCoeff = (float)this.specCoeff.Value;
                float ambCoeff = (float)this.ambCoeff.Value;
                float diffCoef = (float)this.diffCoef.Value;
                int shininess = (int)this.shininess.Value;
                //sphere.SetMaterial(specCoeff,ambCoeff,diffCoef,shininess);
            }
            glControl1.Invalidate();
        }

        private void PerFragment_CheckedChanged(object sender, EventArgs e)
        {
            shader = PerFragment.Checked;
            if(sfera)
            {
                if(PerFragment.Checked)
                {
                    //sphere.Delete();
                    Reset();
                    //sphere = new Sphere(glControl1.Width, glControl1.Height, scale, (int)Pi.Value, (int)DvaPi.Value, flat, shader);
                }
                else
                {
                    //sphere.Delete();
                    Reset();
                    //sphere = new Sphere(glControl1.Width, glControl1.Height, scale, (int)Pi.Value, (int)DvaPi.Value, flat, shader);
                }
                glControl1.Invalidate();
            }
        }

        private void Pi_ValueChanged(object sender, EventArgs e)
        {
            if(sfera)
            {
                //sphere.Delete();
                Reset();
                //sphere = new Sphere(glControl1.Width, glControl1.Height, scale, (int)Pi.Value, (int)DvaPi.Value, flat, shader);
                glControl1.Invalidate();
            }
        }

        private void otvorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //pridat zrusenie geoidu ak uz bol dajaky zobrazeny a ma sa kreslit novy
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                sfera = true;
                sdat = new SphereDAT(glControl1.Width, glControl1.Height, openFileDialog1.FileName.ToString());
                sdat.DrawSphere();
                glControl1.Invalidate();
            }
        }
    }
}
