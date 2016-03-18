namespace Kocka
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //this.glControl1 = new OpenTK.GLControl();
            this.glControl1 = new OpenTK.GLControl(new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(32), 24, 0, 8));

            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.RotY_trackBar1 = new System.Windows.Forms.TrackBar();
            this.RotX_trackBar2 = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.ZScale = new System.Windows.Forms.TrackBar();
            this.ZScale_value = new System.Windows.Forms.Label();
            this.RekresliBtn = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.volacoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otvorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ulozObrazokToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nastaveniaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bielePozadieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.farebnaSkalaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetujPohladToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.materiálToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.TrianglesRadioButton = new System.Windows.Forms.RadioButton();
            this.WireframeRadioButton = new System.Windows.Forms.RadioButton();
            this.PointsRadioButton = new System.Windows.Forms.RadioButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RotY_trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotX_trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZScale)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(0, 0);
            this.glControl1.Margin = new System.Windows.Forms.Padding(5);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(981, 683);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            this.glControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glControl1_KeyDown);
            this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDown);
            this.glControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseMove);
            this.glControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseUp);
            this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Spherical dat files |*.dat|Rectanguler dat files |*.dat";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.glControl1);
            this.panel1.Location = new System.Drawing.Point(48, 32);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(985, 687);
            this.panel1.TabIndex = 30;
            // 
            // RotY_trackBar1
            // 
            this.RotY_trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RotY_trackBar1.Location = new System.Drawing.Point(48, 717);
            this.RotY_trackBar1.Maximum = 90;
            this.RotY_trackBar1.Minimum = -90;
            this.RotY_trackBar1.Name = "RotY_trackBar1";
            this.RotY_trackBar1.Size = new System.Drawing.Size(983, 56);
            this.RotY_trackBar1.TabIndex = 40;
            this.RotY_trackBar1.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.RotY_trackBar1.ValueChanged += new System.EventHandler(this.RotX_trackBar2_ValueChanged);
            // 
            // RotX_trackBar2
            // 
            this.RotX_trackBar2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.RotX_trackBar2.Location = new System.Drawing.Point(12, 31);
            this.RotX_trackBar2.Maximum = 90;
            this.RotX_trackBar2.Minimum = -90;
            this.RotX_trackBar2.Name = "RotX_trackBar2";
            this.RotX_trackBar2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.RotX_trackBar2.Size = new System.Drawing.Size(56, 689);
            this.RotX_trackBar2.TabIndex = 39;
            this.RotX_trackBar2.ValueChanged += new System.EventHandler(this.RotX_trackBar2_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1042, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 17);
            this.label1.TabIndex = 41;
            this.label1.Text = "Z scale:";
            // 
            // ZScale
            // 
            this.ZScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZScale.Location = new System.Drawing.Point(1041, 62);
            this.ZScale.Maximum = 50;
            this.ZScale.Minimum = 1;
            this.ZScale.Name = "ZScale";
            this.ZScale.Size = new System.Drawing.Size(104, 56);
            this.ZScale.TabIndex = 42;
            this.ZScale.TickStyle = System.Windows.Forms.TickStyle.None;
            this.ZScale.Value = 50;
            this.ZScale.ValueChanged += new System.EventHandler(this.ZScale_ValueChanged);
            // 
            // ZScale_value
            // 
            this.ZScale_value.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZScale_value.AutoSize = true;
            this.ZScale_value.Location = new System.Drawing.Point(1089, 89);
            this.ZScale_value.Name = "ZScale_value";
            this.ZScale_value.Size = new System.Drawing.Size(24, 17);
            this.ZScale_value.TabIndex = 43;
            this.ZScale_value.Text = "50";
            // 
            // RekresliBtn
            // 
            this.RekresliBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RekresliBtn.Location = new System.Drawing.Point(1063, 109);
            this.RekresliBtn.Name = "RekresliBtn";
            this.RekresliBtn.Size = new System.Drawing.Size(75, 28);
            this.RekresliBtn.TabIndex = 44;
            this.RekresliBtn.Text = "Rekresli";
            this.RekresliBtn.UseVisualStyleBackColor = true;
            this.RekresliBtn.Click += new System.EventHandler(this.RekresliBtn_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.volacoToolStripMenuItem,
            this.nastaveniaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1150, 28);
            this.menuStrip1.TabIndex = 45;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // volacoToolStripMenuItem
            // 
            this.volacoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.otvorToolStripMenuItem,
            this.ulozObrazokToolStripMenuItem});
            this.volacoToolStripMenuItem.Name = "volacoToolStripMenuItem";
            this.volacoToolStripMenuItem.Size = new System.Drawing.Size(67, 24);
            this.volacoToolStripMenuItem.Text = "Volaco";
            // 
            // otvorToolStripMenuItem
            // 
            this.otvorToolStripMenuItem.Name = "otvorToolStripMenuItem";
            this.otvorToolStripMenuItem.Size = new System.Drawing.Size(166, 24);
            this.otvorToolStripMenuItem.Text = "Otvor...";
            this.otvorToolStripMenuItem.Click += new System.EventHandler(this.otvorToolStripMenuItem_Click);
            // 
            // ulozObrazokToolStripMenuItem
            // 
            this.ulozObrazokToolStripMenuItem.Name = "ulozObrazokToolStripMenuItem";
            this.ulozObrazokToolStripMenuItem.Size = new System.Drawing.Size(166, 24);
            this.ulozObrazokToolStripMenuItem.Text = "Uloz obrazok";
            this.ulozObrazokToolStripMenuItem.Click += new System.EventHandler(this.ulozObrazokToolStripMenuItem_Click);
            // 
            // nastaveniaToolStripMenuItem
            // 
            this.nastaveniaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bielePozadieToolStripMenuItem,
            this.farebnaSkalaToolStripMenuItem,
            this.resetujPohladToolStripMenuItem,
            this.materiálToolStripMenuItem});
            this.nastaveniaToolStripMenuItem.Name = "nastaveniaToolStripMenuItem";
            this.nastaveniaToolStripMenuItem.Size = new System.Drawing.Size(94, 24);
            this.nastaveniaToolStripMenuItem.Text = "Nastavenia";
            // 
            // bielePozadieToolStripMenuItem
            // 
            this.bielePozadieToolStripMenuItem.Name = "bielePozadieToolStripMenuItem";
            this.bielePozadieToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.bielePozadieToolStripMenuItem.Text = "Biele pozadie";
            this.bielePozadieToolStripMenuItem.CheckedChanged += new System.EventHandler(this.bielePozadieToolStripMenuItem_CheckedChanged);
            this.bielePozadieToolStripMenuItem.Click += new System.EventHandler(this.bielePozadieToolStripMenuItem_Click);
            // 
            // farebnaSkalaToolStripMenuItem
            // 
            this.farebnaSkalaToolStripMenuItem.Name = "farebnaSkalaToolStripMenuItem";
            this.farebnaSkalaToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.farebnaSkalaToolStripMenuItem.Text = "Farebna skala";
            this.farebnaSkalaToolStripMenuItem.Click += new System.EventHandler(this.farebnaSkalaToolStripMenuItem_Click);
            // 
            // resetujPohladToolStripMenuItem
            // 
            this.resetujPohladToolStripMenuItem.Name = "resetujPohladToolStripMenuItem";
            this.resetujPohladToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.resetujPohladToolStripMenuItem.Text = "Resetuj pohlad...";
            this.resetujPohladToolStripMenuItem.Click += new System.EventHandler(this.resetujPohladToolStripMenuItem_Click);
            // 
            // materiálToolStripMenuItem
            // 
            this.materiálToolStripMenuItem.Name = "materiálToolStripMenuItem";
            this.materiálToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.materiálToolStripMenuItem.Text = "Materiál";
            this.materiálToolStripMenuItem.Click += new System.EventHandler(this.materiálToolStripMenuItem_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = " \"PNG files(*.png)|*.png";
            // 
            // TrianglesRadioButton
            // 
            this.TrianglesRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TrianglesRadioButton.AutoSize = true;
            this.TrianglesRadioButton.Checked = true;
            this.TrianglesRadioButton.Location = new System.Drawing.Point(1063, 160);
            this.TrianglesRadioButton.Name = "TrianglesRadioButton";
            this.TrianglesRadioButton.Size = new System.Drawing.Size(72, 21);
            this.TrianglesRadioButton.TabIndex = 46;
            this.TrianglesRadioButton.TabStop = true;
            this.TrianglesRadioButton.Text = "povrch";
            this.TrianglesRadioButton.UseVisualStyleBackColor = true;
            this.TrianglesRadioButton.CheckedChanged += new System.EventHandler(this.TrianglesRadioButton_CheckedChanged);
            // 
            // WireframeRadioButton
            // 
            this.WireframeRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.WireframeRadioButton.AutoSize = true;
            this.WireframeRadioButton.Location = new System.Drawing.Point(1063, 188);
            this.WireframeRadioButton.Name = "WireframeRadioButton";
            this.WireframeRadioButton.Size = new System.Drawing.Size(59, 21);
            this.WireframeRadioButton.TabIndex = 47;
            this.WireframeRadioButton.Text = "#siet";
            this.WireframeRadioButton.UseVisualStyleBackColor = true;
            this.WireframeRadioButton.CheckedChanged += new System.EventHandler(this.TrianglesRadioButton_CheckedChanged);
            // 
            // PointsRadioButton
            // 
            this.PointsRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PointsRadioButton.AutoSize = true;
            this.PointsRadioButton.Location = new System.Drawing.Point(1063, 215);
            this.PointsRadioButton.Name = "PointsRadioButton";
            this.PointsRadioButton.Size = new System.Drawing.Size(60, 21);
            this.PointsRadioButton.TabIndex = 48;
            this.PointsRadioButton.Text = "body";
            this.PointsRadioButton.UseVisualStyleBackColor = true;
            this.PointsRadioButton.CheckedChanged += new System.EventHandler(this.TrianglesRadioButton_CheckedChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 763);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1150, 25);
            this.statusStrip1.TabIndex = 50;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Margin = new System.Windows.Forms.Padding(10, 3, 1, 3);
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 19);
            this.toolStripProgressBar1.Step = 1;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(151, 20);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.TextChanged += new System.EventHandler(this.toolStripStatusLabel1_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1150, 788);
            this.Controls.Add(this.PointsRadioButton);
            this.Controls.Add(this.WireframeRadioButton);
            this.Controls.Add(this.TrianglesRadioButton);
            this.Controls.Add(this.RekresliBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.RotX_trackBar2);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.ZScale_value);
            this.Controls.Add(this.ZScale);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.RotY_trackBar1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RotY_trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotX_trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZScale)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.TrackBar RotY_trackBar1;
        private System.Windows.Forms.TrackBar RotX_trackBar2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar ZScale;
        private System.Windows.Forms.Label ZScale_value;
        private System.Windows.Forms.Button RekresliBtn;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem volacoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otvorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nastaveniaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bielePozadieToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ulozObrazokToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem farebnaSkalaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetujPohladToolStripMenuItem;
        private System.Windows.Forms.RadioButton TrianglesRadioButton;
        private System.Windows.Forms.RadioButton WireframeRadioButton;
        private System.Windows.Forms.RadioButton PointsRadioButton;
        private System.Windows.Forms.ToolStripMenuItem materiálToolStripMenuItem;
        public System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

