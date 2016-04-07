using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VolumeRendering
{
    public partial class MsgBox : Form
    {
        bool btn;
        public MsgBox()
        {
            InitializeComponent();
        }

        public MsgBox(string message_text, string caption)
        {
            InitializeComponent();
            this.Text = caption;
            this.label1.Text = message_text;
            btn = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(btn)
                this.Height += richTextBox1.Height + 10;
            else
                this.Height -= richTextBox1.Height + 10;
            btn = !btn;
        }
    }
}
