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

namespace VolumeRendering
{
    public partial class IsoHunter : Form
    {
        private Volume volume;
        private GLControl control;

        public IsoHunter()
        {
            InitializeComponent();
        }

        public IsoHunter(Volume volume,GLControl control)
        {
            InitializeComponent();
            this.volume = volume;
            this.control = control;
        }

        private List<Vector2> GetListFromViewList2()
        {
            List<Vector2> tmp = new List<Vector2>();
            tmp.Add(new Vector2(0, 0));
            tmp.Add(new Vector2(0, Math.Max((float)(value_numericUpDown.Value - minusValue_numericUpDown.Value), 0)));
            tmp.Add(new Vector2(1,(float)(value_numericUpDown.Value)));
            tmp.Add(new Vector2(0, Math.Min((float)(value_numericUpDown.Value + plusValue_numericUpDown.Value), 256)));
            tmp.Add(new Vector2(0, 256));
            return tmp;
        }

        private void IsoHunter_Load(object sender, EventArgs e)
        {

        }

        private void value_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            volume.ChangeOpacity(GetListFromViewList2());
            control.Invalidate();
        }
    }
}
