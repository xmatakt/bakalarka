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
using System.Drawing.Imaging;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Kocka
{
    public partial class Form1 : Form
    {
        private bool loaded, resize, sfera,sur;
        private SphereDAT sdat;
        private Surface surf;
        private float Pi180;
        float scale;
        float dx, dy;
        float wPol, hPol;
        float wLomeno2, hLomeno2;
        float amb, spec, diff;
        int shin;
        //ColorScale cs;

        public Form1()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
            RotY_trackBar1.Enabled = RotX_trackBar2.Enabled = ZScale.Enabled = RekresliBtn.Enabled = false;

            amb = 0.29f; spec = 0.86f; diff = 0.57f; shin = 128;
            loaded = false;
            resize = false;
            sfera = sur = false; 
            Pi180 = (float)Math.PI / 180.0f;
            scale = 1.0f;
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            wPol = glControl1.Width / 2.0f;
            hPol = glControl1.Height / 2.0f;
            wLomeno2 = 10.0f / (float)glControl1.Width;
            hLomeno2 = 10.0f / (float)glControl1.Height;
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.ClearDepth(1.0);

            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);

            //cs = new ColorScale(0, 0, glControl1.Width, glControl1.Height);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded)
                return;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (sfera)
                sdat.DrawSphere();
            if (sur)
                surf.DrawSurface();

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
                sdat.Delete(true);
            if (sur)
                surf.Delete(true);
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
            if (e.Button == MouseButtons.Middle && sfera)
            {
                float tmpx = e.Location.X - dx;
                float tmpy = dy - e.Location.Y;
                if (sfera)
                {
                    sdat.Scale(scale);
                    sdat.Transalte(wLomeno2 * tmpx, hLomeno2 * tmpy);
                }
                glControl1.Invalidate();
            }
            if (e.Button == MouseButtons.Left && sur)
            {
                float tmpx = e.Location.X - dx;
                float tmpy = dy - e.Location.Y;
                if (sur)
                {
                    surf.Scale(scale);
                    surf.Transalte(wLomeno2 * tmpx, hLomeno2 * tmpy);
                }
                glControl1.Invalidate();
            }

            //rotacia
            if (e.Button == MouseButtons.Left && sfera)
            {
                float tmpx = (e.Location.X - dx) / wPol;
                float tmpy = (e.Location.Y - dy) / hPol;
                float angle = (float)Math.Sqrt(tmpx * tmpx + tmpy * tmpy);
                sdat.Scale(scale);
                sdat.Rotate(tmpx, tmpy, angle);
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
                if (sur)
                    surf.Scale(scale);

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
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                if (sur)
                    surf.Ende();
            }
        }

        private void ZScale_ValueChanged(object sender, EventArgs e)
        {
            ZScale_value.Text = ZScale.Value.ToString();
        }

        private void RekresliBtn_Click(object sender, EventArgs e)
        {
            if (sur)
            {
                surf.Rescale(ZScale.Value);
                surf.Scale(scale);
                glControl1.Invalidate();
            }
            if (sfera)
            {
                sdat.Rescale(ZScale.Value);
                sdat.Scale(scale);
                glControl1.Invalidate();
            }
        }

        private void RotX_trackBar2_ValueChanged(object sender, EventArgs e)
        {
            if (sur)
            {
                surf.Rotate(RotY_trackBar1.Value * Pi180, -RotX_trackBar2.Value * Pi180);
                surf.Scale(scale);
                glControl1.Invalidate();
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
                if (sfera)
                {
                    sdat.Resize(glControl1.Width, glControl1.Height);
                    glControl1.Invalidate();
                }
                if (sur)
                {
                    surf.Resize(glControl1.Width, glControl1.Height);
                    glControl1.Invalidate();
                }
            }
        }

        private void Reset()
        {
            amb = 0.29f; spec = 0.86f; diff = 0.57f; shin = 128;
            wPol = glControl1.Width / 2.0f;
            hPol = glControl1.Height / 2.0f;
            TrianglesRadioButton.Checked = true;
            if(sfera)
            {
                ZScale.Value = 10;
                wLomeno2 = 2.0f / (float)glControl1.Width;
                hLomeno2 = 2.0f / (float)glControl1.Height;
            }
            if(sur)
            {
                ZScale.Value = 50;
                wLomeno2 = 10.0f / (float)glControl1.Width;
                hLomeno2 = 10.0f / (float)glControl1.Height;
            }
            scale = 1.0f;
        }

        //FilterIndex=1-->sfericke
        //FilterIndex=2-->obdlznikove
        private void otvorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if(sfera)
                    sdat.Delete(true);
                if(sur)
                    surf.Delete(true);

                if (openFileDialog1.FilterIndex == 1)
                {
                    sur = false;
                    sdat = new SphereDAT(glControl1.Width, glControl1.Height, openFileDialog1.FileName.ToString());
                    sfera = sdat.Loaded();
                    ZScale.Enabled = RekresliBtn.Enabled = sfera;
                    Reset();
                    RotY_trackBar1.Enabled = RotX_trackBar2.Enabled = false;
                    RotX_trackBar2.Value = RotY_trackBar1.Value = 0;
                }
                if (openFileDialog1.FilterIndex == 2)
                {
                    sfera = false;
                    surf = new Surface(glControl1.Width, glControl1.Height, openFileDialog1.FileName.ToString());
                    sur = surf.Loaded();
                    ZScale.Enabled = RekresliBtn.Enabled = sur;
                    Reset();
                    RotY_trackBar1.Enabled = RotX_trackBar2.Enabled = true;
                }
                glControl1.Invalidate();
            }
        }

        public void ChangeMaterialProperties(float amb,float spec,float diff,int shin)
        {
            if(sur)
                surf.ChangeMaterialProperties(amb, spec, diff, shin);
            if (sfera)
                sdat.ChangeMaterialProperties(amb, spec, diff, shin);
            glControl1.Invalidate();
        }

        public void CloseMaterialWindow(MaterialControl mc)
        {
            spec = mc.ReturnSpec();
            diff = mc.ReturnSDiff();
            amb = mc.ReturnAmb();
            shin = mc.ReturnShin();
        }

        private void materiálToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MaterialControl lc = new MaterialControl(this, amb, spec, diff, shin); 
            lc.Show();
        }

        private void bielePozadieToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (bielePozadieToolStripMenuItem.Checked)
                GL.ClearColor(Color.White);
            else
                GL.ClearColor(Color.Black);
            glControl1.Invalidate();
        }

        private void bielePozadieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bielePozadieToolStripMenuItem.Checked = !bielePozadieToolStripMenuItem.Checked;
        }

        private void ulozObrazokToolStripMenuItem_Click(object sender, EventArgs e)
        {
            glControl1.Invalidate();
            saveFileDialog1.FileName = "obrazok.png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(this.glControl1.Width, this.glControl1.Height);
                System.Drawing.Imaging.BitmapData data =
                    bmp.LockBits(glControl1.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                GL.ReadPixels(0, 0, this.glControl1.Width, this.glControl1.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
                bmp.UnlockBits(data);
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                bmp.Save(saveFileDialog1.FileName, ImageFormat.Png);
                bmp.Dispose();
            }
        }

        private void farebnaSkalaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            farebnaSkalaToolStripMenuItem.Checked = !farebnaSkalaToolStripMenuItem.Checked;
            if (sfera)
                sdat.SetColorScaleOption(farebnaSkalaToolStripMenuItem.Checked);
            if(sur)
                surf.SetColorScaleOption(farebnaSkalaToolStripMenuItem.Checked);
            glControl1.Invalidate();
        }

        private void resetujPohladToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sfera)
                sdat.ResetViewport();
            if (sur)
                surf.ResetViewport();
            glControl1.Invalidate();
        }

        private void TrianglesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if(TrianglesRadioButton.Checked)
            {
                if (sfera)
                    sdat.SetWhatToDraw(1);
                if (sur)
                    surf.SetWhatToDraw(1);
            }
            else if (WireframeRadioButton.Checked)
            {
                if (sfera)
                    sdat.SetWhatToDraw(2);
                if (sur)
                    surf.SetWhatToDraw(2);
            }
            else
            {
                if (sfera)
                    sdat.SetWhatToDraw(3);
                if (sur)
                    surf.SetWhatToDraw(3);
            }
            glControl1.Invalidate();
        }
    }
}
