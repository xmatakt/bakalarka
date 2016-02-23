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

// daco na vyber ci chce clovek flat alebo gourand shading - ale to by som musel najprv vyriesit problem s deravou sferou
//      - najprv musim vyriesit deravost sfery
//      - uz mozem
// snazil som sa vytvorit per pixel shadere, zatial sa mi nepodarilo skompilovat vertex shader, takze sa nic nevykresluje
//      - tak jedina chyba bola, ze som posielal medzi shaderami inty, na co je zrejme alergicky, floaty mu nevadia...
// pridana moznost nacitat data pre sferu zo subor
//      - robil som to koli snahe odhalit problem s deravou sferou, jedna moznost co ma napadala bola, ze data sa po druhy, treti az nekonecny krat
//        z nejakych zahadnych dovodov vygeneruju zle -> nie je problem pri opatovnom generovani dat ale dade inde
// takze problem sposobuje kocka, zatial netusim preco
//      - problem sposobovalo GL.Enable(EnableCap.PrimitiveRestart);, po pridani GL.Disable.... do Delete() kocky bol problem odstraneny
//      - poucenie: co si kto zapne nech si aj vypne, zatial vsak nie uplne aplikovane

namespace Kocka
{
    public partial class Form1 : Form
    {
        private bool loaded, resize,sfera,flat; 
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
            flat = flatShadingToolStripMenuItem.Checked;
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
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.ClearDepth(1.0);
            //GL.Enable(EnableCap.CullFace);
            //GL.CullFace(CullFaceMode.Back);
            //GL.FrontFace(FrontFaceDirection.Ccw);

            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            if(sfera)
            {
                sphere = new Sphere(glControl1.Width, glControl1.Height, scale, flat);
                //sphere = new Sphere(glControl1.Width, glControl1.Height, scale, "sfera.txt");
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
                sphere.SetLight(specular,ambient,diffuse,direction);
            }
            else
            {
                Vector3 specular = new Vector3((float)SpecLx.Value, (float)SpecLy.Value, (float)SpecLz.Value);
                Vector3 ambient = new Vector3((float)AmbLx.Value, (float)AmbLy.Value, (float)AmbLz.Value);
                Vector3 diffuse = new Vector3((float)DiffLx.Value, (float)DiffLy.Value, (float)DiffLz.Value);
                Vector3 direction = new Vector3((float)DirLx.Value, (float)DirLy.Value, (float)DirLz.Value);
                kocka.SetLight(specular, ambient, diffuse, direction);
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
                sphere.SetMaterial(specCoeff,ambCoeff,diffCoef,shininess);
            }
            else
            {
                float specCoeff = (float)this.specCoeff.Value;
                float ambCoeff = (float)this.ambCoeff.Value;
                float diffCoef = (float)this.diffCoef.Value;
                int shininess = (int)this.shininess.Value;
                kocka.SetMaterial(specCoeff, ambCoeff, diffCoef, shininess);
            }
            glControl1.Invalidate();
        }

        private void flatShadingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("flat = {0}", flat);
            if(!flatShadingToolStripMenuItem.Checked)
            {
                flatShadingToolStripMenuItem.Checked = true;
                gourandShadingToolStripMenuItem.Checked = false;
            }
            System.Diagnostics.Debug.WriteLine("flat = {0}", flat);
        }

        private void gourandShadingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("flat = {0}",flat);
            if(!gourandShadingToolStripMenuItem.Checked)
            {
                gourandShadingToolStripMenuItem.Checked = true;
                flatShadingToolStripMenuItem.Checked = false;
            }
            System.Diagnostics.Debug.WriteLine("flat = {0}", flat);
        }

        //zrejme nie uplne optimalne
        private void flatShadingToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            sfera = true;
            kockaToolStripMenuItem.Checked = false;
            volacoToolStripMenuItem.Checked = true;
            if (sfera)
            {
                flat = flatShadingToolStripMenuItem.Checked;
                sphere.Delete();
                Reset();
                sphere = new Sphere(glControl1.Width, glControl1.Height, scale,flat);
            }
            glControl1.Invalidate();
        }

        private void volacoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!volacoToolStripMenuItem.Checked)
            {
                volacoToolStripMenuItem.Checked = true;
                kockaToolStripMenuItem.Checked = false;
            }
        }

        private void kockaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!kockaToolStripMenuItem.Checked)
            {
                kockaToolStripMenuItem.Checked = true;
                volacoToolStripMenuItem.Checked = false;
            }
        }

        private void volacoToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            sfera = volacoToolStripMenuItem.Checked;
            if (sfera)
            {
                flat = flatShadingToolStripMenuItem.Checked;
                kocka.Delete();
                Reset();
                sphere = new Sphere(glControl1.Width, glControl1.Height, scale, flat);
            }
            else
            {
                sphere.Delete();
                Reset();
                kocka = new KockaInak(glControl1.Width, glControl1.Height, scale);
            }
            glControl1.Invalidate();
        }
    }
}
