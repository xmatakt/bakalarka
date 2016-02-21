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
            this.glControl1 = new OpenTK.GLControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dacoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.volacoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Specular = new System.Windows.Forms.Label();
            this.SpecLx = new System.Windows.Forms.NumericUpDown();
            this.Ambient = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DirLz = new System.Windows.Forms.NumericUpDown();
            this.DirLy = new System.Windows.Forms.NumericUpDown();
            this.DirLx = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.DiffLz = new System.Windows.Forms.NumericUpDown();
            this.DiffLy = new System.Windows.Forms.NumericUpDown();
            this.DiffLx = new System.Windows.Forms.NumericUpDown();
            this.AmbLz = new System.Windows.Forms.NumericUpDown();
            this.AmbLy = new System.Windows.Forms.NumericUpDown();
            this.AmbLx = new System.Windows.Forms.NumericUpDown();
            this.SpecLz = new System.Windows.Forms.NumericUpDown();
            this.SpecLy = new System.Windows.Forms.NumericUpDown();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpecLx)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DirLz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DirLy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DirLx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiffLz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiffLy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiffLx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmbLz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmbLy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmbLx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpecLz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpecLy)).BeginInit();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(0, 32);
            this.glControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(946, 719);
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
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dacoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1258, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dacoToolStripMenuItem
            // 
            this.dacoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.volacoToolStripMenuItem});
            this.dacoToolStripMenuItem.Name = "dacoToolStripMenuItem";
            this.dacoToolStripMenuItem.Size = new System.Drawing.Size(56, 24);
            this.dacoToolStripMenuItem.Text = "Daco";
            // 
            // volacoToolStripMenuItem
            // 
            this.volacoToolStripMenuItem.Checked = true;
            this.volacoToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.volacoToolStripMenuItem.Name = "volacoToolStripMenuItem";
            this.volacoToolStripMenuItem.Size = new System.Drawing.Size(112, 26);
            this.volacoToolStripMenuItem.Text = "Sfera";
            this.volacoToolStripMenuItem.CheckedChanged += new System.EventHandler(this.volacoToolStripMenuItem_CheckedChanged);
            this.volacoToolStripMenuItem.Click += new System.EventHandler(this.volacoToolStripMenuItem_Click);
            // 
            // Specular
            // 
            this.Specular.AutoSize = true;
            this.Specular.Location = new System.Drawing.Point(0, 28);
            this.Specular.Name = "Specular";
            this.Specular.Size = new System.Drawing.Size(64, 17);
            this.Specular.TabIndex = 0;
            this.Specular.Text = "Specular";
            // 
            // SpecLx
            // 
            this.SpecLx.DecimalPlaces = 2;
            this.SpecLx.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.SpecLx.Location = new System.Drawing.Point(70, 26);
            this.SpecLx.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpecLx.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.SpecLx.Name = "SpecLx";
            this.SpecLx.Size = new System.Drawing.Size(68, 22);
            this.SpecLx.TabIndex = 1;
            this.SpecLx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SpecLx.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpecLx.ValueChanged += new System.EventHandler(this.SpecLx_ValueChanged);
            // 
            // Ambient
            // 
            this.Ambient.AutoSize = true;
            this.Ambient.Location = new System.Drawing.Point(0, 63);
            this.Ambient.Name = "Ambient";
            this.Ambient.Size = new System.Drawing.Size(59, 17);
            this.Ambient.TabIndex = 4;
            this.Ambient.Text = "Ambient";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Diffuse";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.DirLz);
            this.groupBox1.Controls.Add(this.DirLy);
            this.groupBox1.Controls.Add(this.DirLx);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.DiffLz);
            this.groupBox1.Controls.Add(this.DiffLy);
            this.groupBox1.Controls.Add(this.DiffLx);
            this.groupBox1.Controls.Add(this.AmbLz);
            this.groupBox1.Controls.Add(this.AmbLy);
            this.groupBox1.Controls.Add(this.AmbLx);
            this.groupBox1.Controls.Add(this.SpecLz);
            this.groupBox1.Controls.Add(this.SpecLy);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.Ambient);
            this.groupBox1.Controls.Add(this.SpecLx);
            this.groupBox1.Controls.Add(this.Specular);
            this.groupBox1.Location = new System.Drawing.Point(953, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(293, 174);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Light";
            // 
            // DirLz
            // 
            this.DirLz.DecimalPlaces = 2;
            this.DirLz.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.DirLz.Location = new System.Drawing.Point(217, 130);
            this.DirLz.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DirLz.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.DirLz.Name = "DirLz";
            this.DirLz.Size = new System.Drawing.Size(68, 22);
            this.DirLz.TabIndex = 20;
            this.DirLz.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.DirLz.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.DirLz.ValueChanged += new System.EventHandler(this.SpecLx_ValueChanged);
            // 
            // DirLy
            // 
            this.DirLy.DecimalPlaces = 2;
            this.DirLy.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.DirLy.Location = new System.Drawing.Point(144, 130);
            this.DirLy.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DirLy.Name = "DirLy";
            this.DirLy.Size = new System.Drawing.Size(68, 22);
            this.DirLy.TabIndex = 19;
            this.DirLy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.DirLy.ValueChanged += new System.EventHandler(this.SpecLx_ValueChanged);
            // 
            // DirLx
            // 
            this.DirLx.DecimalPlaces = 2;
            this.DirLx.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.DirLx.Location = new System.Drawing.Point(70, 130);
            this.DirLx.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DirLx.Name = "DirLx";
            this.DirLx.Size = new System.Drawing.Size(68, 22);
            this.DirLx.TabIndex = 18;
            this.DirLx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.DirLx.ValueChanged += new System.EventHandler(this.SpecLx_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 17);
            this.label1.TabIndex = 17;
            this.label1.Text = "Direction";
            // 
            // DiffLz
            // 
            this.DiffLz.DecimalPlaces = 2;
            this.DiffLz.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.DiffLz.Location = new System.Drawing.Point(217, 95);
            this.DiffLz.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DiffLz.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.DiffLz.Name = "DiffLz";
            this.DiffLz.Size = new System.Drawing.Size(68, 22);
            this.DiffLz.TabIndex = 16;
            this.DiffLz.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.DiffLz.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DiffLz.ValueChanged += new System.EventHandler(this.SpecLx_ValueChanged);
            // 
            // DiffLy
            // 
            this.DiffLy.DecimalPlaces = 2;
            this.DiffLy.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.DiffLy.Location = new System.Drawing.Point(144, 95);
            this.DiffLy.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DiffLy.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.DiffLy.Name = "DiffLy";
            this.DiffLy.Size = new System.Drawing.Size(68, 22);
            this.DiffLy.TabIndex = 15;
            this.DiffLy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.DiffLy.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DiffLy.ValueChanged += new System.EventHandler(this.SpecLx_ValueChanged);
            // 
            // DiffLx
            // 
            this.DiffLx.DecimalPlaces = 2;
            this.DiffLx.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.DiffLx.Location = new System.Drawing.Point(70, 95);
            this.DiffLx.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DiffLx.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.DiffLx.Name = "DiffLx";
            this.DiffLx.Size = new System.Drawing.Size(68, 22);
            this.DiffLx.TabIndex = 14;
            this.DiffLx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.DiffLx.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DiffLx.ValueChanged += new System.EventHandler(this.SpecLx_ValueChanged);
            // 
            // AmbLz
            // 
            this.AmbLz.DecimalPlaces = 2;
            this.AmbLz.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.AmbLz.Location = new System.Drawing.Point(217, 61);
            this.AmbLz.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AmbLz.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.AmbLz.Name = "AmbLz";
            this.AmbLz.Size = new System.Drawing.Size(68, 22);
            this.AmbLz.TabIndex = 13;
            this.AmbLz.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.AmbLz.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AmbLz.ValueChanged += new System.EventHandler(this.SpecLx_ValueChanged);
            // 
            // AmbLy
            // 
            this.AmbLy.DecimalPlaces = 2;
            this.AmbLy.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.AmbLy.Location = new System.Drawing.Point(144, 61);
            this.AmbLy.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AmbLy.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.AmbLy.Name = "AmbLy";
            this.AmbLy.Size = new System.Drawing.Size(68, 22);
            this.AmbLy.TabIndex = 12;
            this.AmbLy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.AmbLy.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AmbLy.ValueChanged += new System.EventHandler(this.SpecLx_ValueChanged);
            // 
            // AmbLx
            // 
            this.AmbLx.DecimalPlaces = 2;
            this.AmbLx.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.AmbLx.Location = new System.Drawing.Point(70, 61);
            this.AmbLx.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AmbLx.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.AmbLx.Name = "AmbLx";
            this.AmbLx.Size = new System.Drawing.Size(68, 22);
            this.AmbLx.TabIndex = 11;
            this.AmbLx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.AmbLx.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AmbLx.ValueChanged += new System.EventHandler(this.SpecLx_ValueChanged);
            // 
            // SpecLz
            // 
            this.SpecLz.DecimalPlaces = 2;
            this.SpecLz.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.SpecLz.Location = new System.Drawing.Point(217, 26);
            this.SpecLz.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpecLz.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.SpecLz.Name = "SpecLz";
            this.SpecLz.Size = new System.Drawing.Size(68, 22);
            this.SpecLz.TabIndex = 10;
            this.SpecLz.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SpecLz.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpecLz.ValueChanged += new System.EventHandler(this.SpecLx_ValueChanged);
            // 
            // SpecLy
            // 
            this.SpecLy.DecimalPlaces = 2;
            this.SpecLy.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.SpecLy.Location = new System.Drawing.Point(144, 26);
            this.SpecLy.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpecLy.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.SpecLy.Name = "SpecLy";
            this.SpecLy.Size = new System.Drawing.Size(68, 22);
            this.SpecLy.TabIndex = 9;
            this.SpecLy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SpecLy.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpecLy.ValueChanged += new System.EventHandler(this.SpecLx_ValueChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1258, 756);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpecLx)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DirLz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DirLy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DirLx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiffLz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiffLy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiffLx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmbLz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmbLy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmbLx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpecLz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpecLy)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dacoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem volacoToolStripMenuItem;
        private System.Windows.Forms.Label Specular;
        private System.Windows.Forms.NumericUpDown SpecLx;
        private System.Windows.Forms.Label Ambient;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown DirLz;
        private System.Windows.Forms.NumericUpDown DirLy;
        private System.Windows.Forms.NumericUpDown DirLx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown DiffLz;
        private System.Windows.Forms.NumericUpDown DiffLy;
        private System.Windows.Forms.NumericUpDown DiffLx;
        private System.Windows.Forms.NumericUpDown AmbLz;
        private System.Windows.Forms.NumericUpDown AmbLy;
        private System.Windows.Forms.NumericUpDown AmbLx;
        private System.Windows.Forms.NumericUpDown SpecLz;
        private System.Windows.Forms.NumericUpDown SpecLy;
    }
}

