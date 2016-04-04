using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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
        private bool loaded, volumeLoaded;
        float scale;
        float dx, dy;
        float wPol, hPol;
        float wLomeno2, hLomeno2;
        string file;
        private Volume volume;

        public Form1()
        {
            InitializeComponent();
            index = indexValCol = 0;
            scale = 1.0f;
            volumeLoaded = false;
            panel3.Visible = false;
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            wLomeno2 = 1.0f / (float)glControl1.Width;
            hLomeno2 = 1.0f / (float)glControl1.Height;
            wPol = glControl1.Width / 2.0f;
            hPol = glControl1.Height / 2.0f;
            GL.ClearColor(Color.Black);
            SetListViewItems();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded)
                return;

            if (volumeLoaded)
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
            if (e.Button == MouseButtons.Middle && volumeLoaded)
            {
                float tmpx = e.Location.X - dx;
                float tmpy = dy - e.Location.Y;
                volume.Scale(scale);
                volume.Transalte(wLomeno2 * tmpx, hLomeno2 * tmpy);
                glControl1.Invalidate();
            }

            //rotacia
            if (e.Button == MouseButtons.Left && volumeLoaded)
            {
                float tmpx = (e.Location.X - dx) / wPol;
                float tmpy = (e.Location.Y - dy) / hPol;
                float angle = (float)Math.Sqrt(tmpx * tmpx + tmpy * tmpy);
                volume.Scale(scale);
                volume.Rotate(tmpx, tmpy, angle);
                glControl1.Invalidate();
            }
            //skalovanie
            if (e.Button == MouseButtons.Right && volumeLoaded)
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
                if (volumeLoaded)
                    volume.Ende();
            }
            glControl1.Invalidate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (volumeLoaded)
                volume.Delete();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            if (loaded && volumeLoaded)
            {
                wLomeno2 = 1.0f / (float)glControl1.Width;
                hLomeno2 = 1.0f / (float)glControl1.Height;
                volume.Resize(glControl1.Width, glControl1.Height);
            }
        }

        #region zmeny vzhladu a veci okolo toho

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

        #region zmena farba-isovalue

        //vyber v listu iso hodnot
        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
            {
                ValColDel_button.Enabled = true;
                indexValCol = listView3.Items.IndexOf(listView3.SelectedItems[0]);
                valCol_numericUpDown.Value = decimal.Parse(listView3.SelectedItems[0].Text);
                color_label.BackColor = listView2.Items[indexValCol].BackColor;
            }
        }

        //vyber z listu farieb,  hned ovplyvni vzhlad
        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                ValColDel_button.Enabled = false;
                indexValCol = listView2.Items.IndexOf(listView2.SelectedItems[0]);
                //valCol_numericUpDown.Value = decimal.Parse(listView3.Items[indexValCol].Text);
                color_label.BackColor = listView2.Items[indexValCol].BackColor;
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                    listView2.Items[indexValCol].BackColor = color_label.BackColor = colorDialog1.Color;
            }
            if (volumeLoaded)
                volume.ChangeColors(GetListFromViewList4());
            glControl1.Invalidate();
        }

        //zmena iso hodnoty, hned ovplyvni vzhlad
        private void valCol_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
                listView3.Items[indexValCol].Text = valCol_numericUpDown.Value.ToString();
            if (volumeLoaded)
                volume.ChangeColors(GetListFromViewList4());
            glControl1.Invalidate();
        }

        //volba farby
        private void color_label_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            color_label.BackColor = colorDialog1.Color;
        }

        //pridanie dvojice iso hodnota - farba
        private void ValCol_buttonAdd_Click(object sender, EventArgs e)
        {
            ListViewItem lvi = new ListViewItem(valCol_numericUpDown.Value.ToString());
            listView3.Items.Add(lvi);
            ListViewItem lvicol = new ListViewItem("");
            lvicol.BackColor = color_label.BackColor;
            listView2.Items.Add(lvicol);
            if (volumeLoaded)
                volume.ChangeColors(GetListFromViewList4());
            glControl1.Invalidate();
        }

        //delete vybranej zlozky
        private void ValColDel_button_Click(object sender, EventArgs e)
        {
            listView2.Items[indexValCol].Remove();
            listView3.Items[indexValCol].Remove();
            ValColDel_button.Enabled = false;
            if (volumeLoaded)
                volume.ChangeColors(GetListFromViewList4());
            glControl1.Invalidate();
        }

        #endregion

        #region zmena priehladnost - isovalue

        //pridanie dvojice
        private void button1_Click(object sender, EventArgs e)
        {
            ListViewItem lvi = new ListViewItem(value_numericUpDown.Value.ToString());
            lvi.SubItems.Add(opacity_numericUpDown.Value.ToString());
            ValOp_ListView.Items.Add(lvi);
            ValOp_ListView.Sort();
            if (volumeLoaded)
                volume.ChangeOpacity(GetListFromViewList2());
            glControl1.Invalidate();
        }

        //vyber urcitej polozky
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ValOp_ListView.SelectedItems.Count > 0)
            {
                index = ValOp_ListView.Items.IndexOf(ValOp_ListView.SelectedItems[0]);
                value_numericUpDown.Value = decimal.Parse(ValOp_ListView.SelectedItems[0].Text);
                opacity_numericUpDown.Value = decimal.Parse(ValOp_ListView.SelectedItems[0].SubItems[1].Text);
                ValOpDel_button.Enabled = true;
            }
        }

        //zmena iso hodnoty
        private void value_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (ValOp_ListView.SelectedItems.Count > 0)
                ValOp_ListView.Items[index].Text = value_numericUpDown.Value.ToString();
            if (volumeLoaded)
                volume.ChangeOpacity(GetListFromViewList2());
            glControl1.Invalidate();
        }

        //zmena priehladnosti
        private void opacity_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (ValOp_ListView.SelectedItems.Count > 0)
                ValOp_ListView.Items[index].SubItems[1].Text = opacity_numericUpDown.Value.ToString();
            if (volumeLoaded)
                volume.ChangeOpacity(GetListFromViewList2());
            glControl1.Invalidate();
        }

        //editovanie
        private void button2_Click(object sender, EventArgs e)
        {
            if (ValOp_ListView.Items.Count > 0)
            {
                ValOp_ListView.Items[index].Text = value_numericUpDown.Value.ToString();
                ValOp_ListView.Items[index].SubItems[1].Text = opacity_numericUpDown.Value.ToString();
                ValOpDel_button.Enabled = false;
            }
        }

        //delete itemu
        private void ValOpDel_button_Click(object sender, EventArgs e)
        {
            ValOp_ListView.Items[index].Remove();
            ValOpDel_button.Enabled = false;
            if (volumeLoaded)
                volume.ChangeOpacity(GetListFromViewList2());
            glControl1.Invalidate();
        }

        #endregion

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (volumeLoaded)
            {
                if (panel2.Visible)
                    volume.SetAlphaReduce((float)alphaReduce_numericUpDown.Value);
                if (panel3.Visible)
                    volume.SetAlphaReduce((float)alphaReduce_numericUpDown2.Value);
            }
            glControl1.Invalidate();
        }

        private void stepSize_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (volumeLoaded)
            {
                if (panel2.Visible)
                    volume.SetStepSize((float)stepSize_numericUpDown.Value);
                if (panel3.Visible)
                    volume.SetStepSize((float)stepSize_numericUpDown2.Value);
            }

            glControl1.Invalidate();
        }

        //zviditelni panel na tvorbu transferovej funkcie
        private void button2_Click_1(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel2.Visible = true;
            alphaReduce_numericUpDown.Value = alphaReduce_numericUpDown2.Value;
            stepSize_numericUpDown.Value = stepSize_numericUpDown2.Value;
            if (volumeLoaded)
            {
                volume.ChangeOpacity(GetListFromViewList2());
                volume.ChangeColors(GetListFromViewList4());
            }
            glControl1.Invalidate();
        }

        private List<Vector2> GetListFromViewList2()
        {
            List<Vector2> tmp = new List<Vector2>();
            int tmp_count = ValOp_ListView.Items.Count;
            if (tmp_count > 0)
                for (int i = 0; i < tmp_count; i++)
                    tmp.Add(new Vector2(float.Parse(ValOp_ListView.Items[i].SubItems[1].Text), float.Parse(ValOp_ListView.Items[i].Text)));
            return tmp;
        }

        private List<Vector4> GetListFromViewList4()
        {
            List<Vector4> tmp = new List<Vector4>();
            int tmp_count = listView2.Items.Count;
            if (tmp_count > 0)
                for (int i = 0; i < tmp_count; i++)
                {
                    float r = listView2.Items[i].BackColor.R / (float)255;
                    float g = listView2.Items[i].BackColor.G / (float)255;
                    float b = listView2.Items[i].BackColor.B / (float)255;
                    tmp.Add(new Vector4(r, g, b, float.Parse(listView3.Items[i].Text)));
                }
            return tmp;
        }

        #endregion

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "TSF files (*.tsf)|*.tsf|VTK files (*.vtk)|*.vtk";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                file = openFileDialog1.FileName;

                ValOp_ListView.Items.Clear();
                listView2.Items.Clear();
                listView3.Items.Clear();
                scale = 1.0f;
                stepSize_numericUpDown.Value = (decimal)0.001;
                alphaReduce_numericUpDown.Value = (decimal)0.5;
                SetListViewItems();

                if (volumeLoaded)
                    volume.Delete();
                volume = new Volume(file, glControl1.Width, glControl1.Height, volbaShadingu_checkBox.Checked);
                volumeLoaded = true;
                glControl1.Invalidate();
            }
        }

        private void saveIamgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            glControl1.Invalidate();
            saveFileDialog1.FileName = "obrazok.png";
            saveFileDialog1.Filter = "PNG files (*.png)|*.png";
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

        private void saveTransferFunctionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "nazovVystihujuciFunkciu.dat";
            saveFileDialog1.Filter = "DAT files (*.dat)|*.dat";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK && volumeLoaded)
                volume.SaveTransferFunction(saveFileDialog1.FileName);
        }

        private void loadTransferFunctionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "DAT files (*.dat)|*.dat";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                file = openFileDialog1.FileName;
                if (volumeLoaded)
                {
                    volume.LoadTransferFunction(file);
                    panel3.Visible = true;
                    panel2.Visible = false;
                    alphaReduce_numericUpDown2.Value = alphaReduce_numericUpDown.Value;
                    stepSize_numericUpDown2.Value = stepSize_numericUpDown.Value;
                }
                glControl1.Invalidate();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void shadingInfo_label_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Changing state of check button after volume data was loaded" +
                "\n don't take affect on the data." +
                "\n Before you load volume data, choose the type of fragment shader you want to use." +
                "\n Checked = shaded " +
                "\n Unchecked = without shading", "Shading info",
                MessageBoxButtons.OK, MessageBoxIcon.Information
                );
        }
    }
}
