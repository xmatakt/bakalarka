using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kocka
{
    public partial class MaterialControl : Form
    {
        private Form1 mainForm = null;
        private float spec, amb, diff;
        private int shin;

        public MaterialControl()
        {
            InitializeComponent();
        }

        public MaterialControl(Form callingForm, float amb, float spec, float diff, int shin)
        {
            mainForm = callingForm as Form1;
            InitializeComponent();

            this.spec = spec;
            this.diff = diff;
            this.amb = amb;
            this.shin = shin;

            SpecLabel.Text = spec.ToString();
            DiffLabel.Text = diff.ToString();
            AmbLabel.Text = amb.ToString();
            Shinlabel.Text = shin.ToString();

            SpecularTrackBar.Value = (int)(spec * 100);
            AmbientTrackBar.Value = (int)(amb * 100);
            DiffuseTrackBar.Value = (int)(diff * 100);
            ShininessTrackBar.Value = shin;
        }

        private void SpecularTrackBar_ValueChanged(object sender, EventArgs e)
        {
            spec = SpecularTrackBar.Value / 100.0f;
            SpecLabel.Text = spec.ToString();
            mainForm.ChangeMaterialProperties(amb,spec,diff,shin);
        }

        private void DiffuseTrackBar_ValueChanged(object sender, EventArgs e)
        {
            diff = DiffuseTrackBar.Value / 100.0f;
            DiffLabel.Text = diff.ToString();
            mainForm.ChangeMaterialProperties(amb, spec, diff, shin);
        }

        private void AmbientTrackBar_ValueChanged(object sender, EventArgs e)
        {
            amb = AmbientTrackBar.Value / 100.0f;
            AmbLabel.Text = amb.ToString();
            mainForm.ChangeMaterialProperties(amb, spec, diff, shin);
        }

        private void ShininessTrackBar_ValueChanged(object sender, EventArgs e)
        {
            shin = ShininessTrackBar.Value;
            Shinlabel.Text = shin.ToString();
            mainForm.ChangeMaterialProperties(amb, spec, diff, shin);
        }

        public float ReturnSpec()
        {
            return spec;
        }
        public float ReturnSDiff()
        {
            return diff;
        }
        public float ReturnAmb()
        {
            return amb;
        }
        public int ReturnShin()
        {
            return shin;
        }

        private void MaterialControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainForm.CloseMaterialWindow(this);
        }
    }
}
