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
        int index, indexValCol;
        private bool loaded;
        float scale;
        float dx, dy;
        float wPol, hPol;
        private Volume volume;

        public Form1()
        {
            InitializeComponent();
            index = indexValCol = 0;
            scale = 1.0f;
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            wPol = glControl1.Width / 2.0f;
            hPol = glControl1.Height / 2.0f;
            GL.ClearColor(Color.Black);
            SetListViewItems();

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

                volume.Scale(scale);

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
                //prerobit, aby sa neresetovalo vsetko
                volume.Delete();
                volume = new Volume("head256.raw",glControl1.Width, glControl1.Height);
            }
        }

        private void SetListViewItems()
        {
            List<Vector2> tmp2 = new List<Vector2>();
            tmp2.Add(new Vector2(0.0f, 0));
            tmp2.Add(new Vector2(0.0f, 40));
            tmp2.Add(new Vector2(0.2f, 60));
            tmp2.Add(new Vector2(0.05f, 63));
            tmp2.Add(new Vector2(0.0f, 80));
            tmp2.Add(new Vector2(0.9f, 82));
            tmp2.Add(new Vector2(1f, 256));

            List<Vector4> tmp4 = new List<Vector4>();
            tmp4.Add(new Vector4(.91f, .7f, .61f, 0));
            tmp4.Add(new Vector4(.91f, .7f, .61f, 80));
            tmp4.Add(new Vector4(1.0f, 1.0f, .85f, 82));
            tmp4.Add(new Vector4(1.0f, 1.0f, .85f, 256));

            for (int i = 0; i < tmp2.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(tmp2[i].Y.ToString());
                lvi.SubItems.Add(tmp2[i].X.ToString());
                ValOp_ListView.Items.Add(lvi);
            }

            for (int i = 0; i < tmp4.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(tmp4[i].W.ToString());
                ListViewItem lvicol = new ListViewItem("");
                lvicol.BackColor = Color.FromArgb((int)(255 * tmp4[i].X), (int)(255 * tmp4[i].Y), (int)(255 * tmp4[i].Z));
                listView2.Items.Add(lvicol);
                lvi.SubItems.Add(tmp2[i].X.ToString());
                listView3.Items.Add(lvi);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListViewItem lvi = new ListViewItem(value_numericUpDown.Value.ToString());
            lvi.SubItems.Add(opacity_numericUpDown.Value.ToString());
            ValOp_ListView.Items.Add(lvi);
            ValOp_ListView.Sort();
        }

        private void color_label_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            color_label.BackColor = colorDialog1.Color;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ValOp_ListView.SelectedItems.Count > 0)
            {
                index = ValOp_ListView.Items.IndexOf(ValOp_ListView.SelectedItems[0]);
                value_numericUpDown.Value = decimal.Parse(ValOp_ListView.SelectedItems[0].Text);
                opacity_numericUpDown.Value = decimal.Parse(ValOp_ListView.SelectedItems[0].SubItems[1].Text);
                ValOpDel_button.Enabled = true;
                ValOpEdit_button.Enabled = true;
               
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(ValOp_ListView.Items.Count > 0)
            {
                ValOp_ListView.Items[index].Text = value_numericUpDown.Value.ToString();
                ValOp_ListView.Items[index].SubItems[1].Text = opacity_numericUpDown.Value.ToString();
                ValOpEdit_button.Enabled = false;
                ValOpDel_button.Enabled = false;
            }
        }

        private void ValCol_buttonAdd_Click(object sender, EventArgs e)
        {
            ListViewItem lvi = new ListViewItem(valCol_numericUpDown.Value.ToString());
            listView3.Items.Add(lvi);
            ListViewItem lvicol = new ListViewItem("");
            lvicol.BackColor = color_label.BackColor;
            listView2.Items.Add(lvicol);
        }

        private void ValCol_buttonEdit_Click(object sender, EventArgs e)
        {
            if (listView3.Items.Count > 0)
            {
                listView3.Items[indexValCol].Text = valCol_numericUpDown.Value.ToString();
                listView2.Items[indexValCol].BackColor = color_label.BackColor;
                ValCol_buttonEdit.Enabled = false;
                ValColDel_button.Enabled = false;
            }
        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
            {
                ValColDel_button.Enabled = true;
                ValCol_buttonEdit.Enabled = true;
                indexValCol = listView3.Items.IndexOf(listView3.SelectedItems[0]);
                valCol_numericUpDown.Value = decimal.Parse(listView3.SelectedItems[0].Text);
                color_label.BackColor = listView2.Items[indexValCol].BackColor;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            volume.SetAlphaReduce((float)alphaReduce_numericUpDown.Value);
            glControl1.Invalidate();
        }

        private void stepSize_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            volume.SetStepSize((float)stepSize_numericUpDown.Value);
            glControl1.Invalidate();
        }

        private void ValOpDel_button_Click(object sender, EventArgs e)
        {
            ValOp_ListView.Items[index].Remove();
            ValOpDel_button.Enabled = false;
            ValOpEdit_button.Enabled = false;
        }

        private void ValColDel_button_Click(object sender, EventArgs e)
        {
            listView3.Items[indexValCol].Remove();
            listView2.Items[indexValCol].Remove();
            ValColDel_button.Enabled = false;
            ValCol_buttonEdit.Enabled = false;
        }

        private void ValOpApply_button_Click(object sender, EventArgs e)
        {
            volume.ChangeOpacity(GetListFromViewList2());
            glControl1.Invalidate();
        }

        private List<Vector2> GetListFromViewList2()
        {
            List<Vector2> tmp = new List<Vector2>();
            int tmp_count = ValOp_ListView.Items.Count;
            if (tmp_count > 0)
                for (int i = 0; i < tmp_count; i++)
                    tmp.Add(new Vector2(float.Parse(ValOp_ListView.Items[i].SubItems[1].Text), float.Parse(ValOp_ListView.Items[i].Text)));

            foreach (var item in tmp)
            {
                System.Diagnostics.Debug.WriteLine(item);
            }

            return tmp;
        }


        private void ValColApply_button_Click(object sender, EventArgs e)
        {
            volume.ChangeColors(GetListFromViewList4());
            glControl1.Invalidate();
        }

        private List<Vector4> GetListFromViewList4()
        {
            List<Vector4> tmp = new List<Vector4>();
            int tmp_count = listView2.Items.Count;
            if (tmp_count > 0)
                for (int i = 0; i < tmp_count; i++)
                {
                    float r = listView2.Items[i].BackColor.R/(float)255;
                    float g = listView2.Items[i].BackColor.G / (float)255;
                    float b = listView2.Items[i].BackColor.B / (float)255;
                    tmp.Add(new Vector4(r, g, b, float.Parse(listView3.Items[i].Text)));
                }

            foreach (var item in tmp)
            {
                System.Diagnostics.Debug.WriteLine(item);
            }

            return tmp;
        }
    }
}
