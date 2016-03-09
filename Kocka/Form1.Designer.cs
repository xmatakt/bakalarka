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
            this.volacoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.otvorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.shininess = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.diffCoef = new System.Windows.Forms.NumericUpDown();
            this.ambCoeff = new System.Windows.Forms.NumericUpDown();
            this.specCoeff = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.PerFragment = new System.Windows.Forms.RadioButton();
            this.PerPixel = new System.Windows.Forms.RadioButton();
            this.ShadersGroupBox = new System.Windows.Forms.GroupBox();
            this.parametreGroupBox = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DvaPi = new System.Windows.Forms.NumericUpDown();
            this.Pi = new System.Windows.Forms.NumericUpDown();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
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
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shininess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.diffCoef)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ambCoeff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.specCoeff)).BeginInit();
            this.ShadersGroupBox.SuspendLayout();
            this.parametreGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DvaPi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pi)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl1.Location = new System.Drawing.Point(0, 0);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(696, 571);
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
            this.volacoToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(944, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // volacoToolStripMenuItem1
            // 
            this.volacoToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.otvorToolStripMenuItem});
            this.volacoToolStripMenuItem1.Name = "volacoToolStripMenuItem1";
            this.volacoToolStripMenuItem1.Size = new System.Drawing.Size(54, 20);
            this.volacoToolStripMenuItem1.Text = "Volaco";
            // 
            // otvorToolStripMenuItem
            // 
            this.otvorToolStripMenuItem.Name = "otvorToolStripMenuItem";
            this.otvorToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.otvorToolStripMenuItem.Text = "Otvor...";
            this.otvorToolStripMenuItem.Click += new System.EventHandler(this.otvorToolStripMenuItem_Click);
            // 
            // Specular
            // 
            this.Specular.AutoSize = true;
            this.Specular.Location = new System.Drawing.Point(0, 23);
            this.Specular.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Specular.Name = "Specular";
            this.Specular.Size = new System.Drawing.Size(49, 13);
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
            this.SpecLx.Location = new System.Drawing.Point(52, 21);
            this.SpecLx.Margin = new System.Windows.Forms.Padding(2);
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
            this.SpecLx.Size = new System.Drawing.Size(51, 20);
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
            this.Ambient.Location = new System.Drawing.Point(0, 51);
            this.Ambient.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Ambient.Name = "Ambient";
            this.Ambient.Size = new System.Drawing.Size(45, 13);
            this.Ambient.TabIndex = 4;
            this.Ambient.Text = "Ambient";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 79);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
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
            this.groupBox1.Location = new System.Drawing.Point(715, 26);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(220, 141);
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
            this.DirLz.Location = new System.Drawing.Point(163, 106);
            this.DirLz.Margin = new System.Windows.Forms.Padding(2);
            this.DirLz.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.DirLz.Name = "DirLz";
            this.DirLz.Size = new System.Drawing.Size(51, 20);
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
            this.DirLy.Location = new System.Drawing.Point(108, 106);
            this.DirLy.Margin = new System.Windows.Forms.Padding(2);
            this.DirLy.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.DirLy.Name = "DirLy";
            this.DirLy.Size = new System.Drawing.Size(51, 20);
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
            this.DirLx.Location = new System.Drawing.Point(52, 106);
            this.DirLx.Margin = new System.Windows.Forms.Padding(2);
            this.DirLx.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.DirLx.Name = "DirLx";
            this.DirLx.Size = new System.Drawing.Size(51, 20);
            this.DirLx.TabIndex = 18;
            this.DirLx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.DirLx.ValueChanged += new System.EventHandler(this.SpecLx_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 107);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
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
            this.DiffLz.Location = new System.Drawing.Point(163, 77);
            this.DiffLz.Margin = new System.Windows.Forms.Padding(2);
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
            this.DiffLz.Size = new System.Drawing.Size(51, 20);
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
            this.DiffLy.Location = new System.Drawing.Point(108, 77);
            this.DiffLy.Margin = new System.Windows.Forms.Padding(2);
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
            this.DiffLy.Size = new System.Drawing.Size(51, 20);
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
            this.DiffLx.Location = new System.Drawing.Point(52, 77);
            this.DiffLx.Margin = new System.Windows.Forms.Padding(2);
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
            this.DiffLx.Size = new System.Drawing.Size(51, 20);
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
            this.AmbLz.Location = new System.Drawing.Point(163, 50);
            this.AmbLz.Margin = new System.Windows.Forms.Padding(2);
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
            this.AmbLz.Size = new System.Drawing.Size(51, 20);
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
            this.AmbLy.Location = new System.Drawing.Point(108, 50);
            this.AmbLy.Margin = new System.Windows.Forms.Padding(2);
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
            this.AmbLy.Size = new System.Drawing.Size(51, 20);
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
            this.AmbLx.Location = new System.Drawing.Point(52, 50);
            this.AmbLx.Margin = new System.Windows.Forms.Padding(2);
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
            this.AmbLx.Size = new System.Drawing.Size(51, 20);
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
            this.SpecLz.Location = new System.Drawing.Point(163, 21);
            this.SpecLz.Margin = new System.Windows.Forms.Padding(2);
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
            this.SpecLz.Size = new System.Drawing.Size(51, 20);
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
            this.SpecLy.Location = new System.Drawing.Point(108, 21);
            this.SpecLy.Margin = new System.Windows.Forms.Padding(2);
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
            this.SpecLy.Size = new System.Drawing.Size(51, 20);
            this.SpecLy.TabIndex = 9;
            this.SpecLy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SpecLy.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpecLy.ValueChanged += new System.EventHandler(this.SpecLx_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.shininess);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.diffCoef);
            this.groupBox2.Controls.Add(this.ambCoeff);
            this.groupBox2.Controls.Add(this.specCoeff);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(717, 172);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(220, 98);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Material";
            // 
            // shininess
            // 
            this.shininess.Location = new System.Drawing.Point(160, 64);
            this.shininess.Margin = new System.Windows.Forms.Padding(2);
            this.shininess.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.shininess.Name = "shininess";
            this.shininess.Size = new System.Drawing.Size(51, 20);
            this.shininess.TabIndex = 25;
            this.shininess.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.shininess.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.shininess.ValueChanged += new System.EventHandler(this.SpecMx_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(88, 66);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 13);
            this.label10.TabIndex = 24;
            this.label10.Text = "Shininess";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(165, 20);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "Diffuse";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(51, 20);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Specular";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(107, 20);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Ambient";
            // 
            // diffCoef
            // 
            this.diffCoef.DecimalPlaces = 2;
            this.diffCoef.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.diffCoef.Location = new System.Drawing.Point(160, 37);
            this.diffCoef.Margin = new System.Windows.Forms.Padding(2);
            this.diffCoef.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.diffCoef.Name = "diffCoef";
            this.diffCoef.Size = new System.Drawing.Size(51, 20);
            this.diffCoef.TabIndex = 20;
            this.diffCoef.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.diffCoef.Value = new decimal(new int[] {
            44,
            0,
            0,
            131072});
            this.diffCoef.ValueChanged += new System.EventHandler(this.SpecMx_ValueChanged);
            // 
            // ambCoeff
            // 
            this.ambCoeff.DecimalPlaces = 2;
            this.ambCoeff.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.ambCoeff.Location = new System.Drawing.Point(106, 37);
            this.ambCoeff.Margin = new System.Windows.Forms.Padding(2);
            this.ambCoeff.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ambCoeff.Name = "ambCoeff";
            this.ambCoeff.Size = new System.Drawing.Size(51, 20);
            this.ambCoeff.TabIndex = 19;
            this.ambCoeff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ambCoeff.Value = new decimal(new int[] {
            23,
            0,
            0,
            131072});
            this.ambCoeff.ValueChanged += new System.EventHandler(this.SpecMx_ValueChanged);
            // 
            // specCoeff
            // 
            this.specCoeff.DecimalPlaces = 2;
            this.specCoeff.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.specCoeff.Location = new System.Drawing.Point(50, 37);
            this.specCoeff.Margin = new System.Windows.Forms.Padding(2);
            this.specCoeff.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.specCoeff.Name = "specCoeff";
            this.specCoeff.Size = new System.Drawing.Size(51, 20);
            this.specCoeff.TabIndex = 18;
            this.specCoeff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.specCoeff.Value = new decimal(new int[] {
            64,
            0,
            0,
            131072});
            this.specCoeff.ValueChanged += new System.EventHandler(this.SpecMx_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-1, 38);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Coeffs";
            // 
            // PerFragment
            // 
            this.PerFragment.AutoSize = true;
            this.PerFragment.Checked = true;
            this.PerFragment.Location = new System.Drawing.Point(12, 25);
            this.PerFragment.Margin = new System.Windows.Forms.Padding(2);
            this.PerFragment.Name = "PerFragment";
            this.PerFragment.Size = new System.Drawing.Size(85, 17);
            this.PerFragment.TabIndex = 26;
            this.PerFragment.TabStop = true;
            this.PerFragment.Text = "Per fragment";
            this.PerFragment.UseVisualStyleBackColor = true;
            this.PerFragment.CheckedChanged += new System.EventHandler(this.PerFragment_CheckedChanged);
            // 
            // PerPixel
            // 
            this.PerPixel.AutoSize = true;
            this.PerPixel.Location = new System.Drawing.Point(12, 47);
            this.PerPixel.Margin = new System.Windows.Forms.Padding(2);
            this.PerPixel.Name = "PerPixel";
            this.PerPixel.Size = new System.Drawing.Size(65, 17);
            this.PerPixel.TabIndex = 27;
            this.PerPixel.Text = "Per pixel";
            this.PerPixel.UseVisualStyleBackColor = true;
            // 
            // ShadersGroupBox
            // 
            this.ShadersGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ShadersGroupBox.Controls.Add(this.PerPixel);
            this.ShadersGroupBox.Controls.Add(this.PerFragment);
            this.ShadersGroupBox.Location = new System.Drawing.Point(715, 286);
            this.ShadersGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.ShadersGroupBox.Name = "ShadersGroupBox";
            this.ShadersGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.ShadersGroupBox.Size = new System.Drawing.Size(112, 81);
            this.ShadersGroupBox.TabIndex = 28;
            this.ShadersGroupBox.TabStop = false;
            this.ShadersGroupBox.Text = "Shaders";
            // 
            // parametreGroupBox
            // 
            this.parametreGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.parametreGroupBox.Controls.Add(this.label5);
            this.parametreGroupBox.Controls.Add(this.label4);
            this.parametreGroupBox.Controls.Add(this.DvaPi);
            this.parametreGroupBox.Controls.Add(this.Pi);
            this.parametreGroupBox.Location = new System.Drawing.Point(832, 286);
            this.parametreGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.parametreGroupBox.Name = "parametreGroupBox";
            this.parametreGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.parametreGroupBox.Size = new System.Drawing.Size(105, 81);
            this.parametreGroupBox.TabIndex = 29;
            this.parametreGroupBox.TabStop = false;
            this.parametreGroupBox.Text = "Parameters";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 50);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "2Pi";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 29);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Pi";
            // 
            // DvaPi
            // 
            this.DvaPi.Location = new System.Drawing.Point(38, 48);
            this.DvaPi.Margin = new System.Windows.Forms.Padding(2);
            this.DvaPi.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.DvaPi.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.DvaPi.Name = "DvaPi";
            this.DvaPi.Size = new System.Drawing.Size(59, 20);
            this.DvaPi.TabIndex = 30;
            this.DvaPi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.DvaPi.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.DvaPi.ValueChanged += new System.EventHandler(this.Pi_ValueChanged);
            // 
            // Pi
            // 
            this.Pi.Location = new System.Drawing.Point(38, 25);
            this.Pi.Margin = new System.Windows.Forms.Padding(2);
            this.Pi.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.Pi.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.Pi.Name = "Pi";
            this.Pi.Size = new System.Drawing.Size(59, 20);
            this.Pi.TabIndex = 0;
            this.Pi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Pi.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.Pi.ValueChanged += new System.EventHandler(this.Pi_ValueChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Text files |*.txt|Dat files |*.dat";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.glControl1);
            this.panel1.Location = new System.Drawing.Point(12, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(700, 575);
            this.panel1.TabIndex = 30;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 614);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.parametreGroupBox);
            this.Controls.Add(this.ShadersGroupBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
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
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shininess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.diffCoef)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ambCoeff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.specCoeff)).EndInit();
            this.ShadersGroupBox.ResumeLayout(false);
            this.ShadersGroupBox.PerformLayout();
            this.parametreGroupBox.ResumeLayout(false);
            this.parametreGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DvaPi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pi)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
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
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown diffCoef;
        private System.Windows.Forms.NumericUpDown ambCoeff;
        private System.Windows.Forms.NumericUpDown specCoeff;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown shininess;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RadioButton PerFragment;
        private System.Windows.Forms.RadioButton PerPixel;
        private System.Windows.Forms.GroupBox ShadersGroupBox;
        private System.Windows.Forms.GroupBox parametreGroupBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown DvaPi;
        private System.Windows.Forms.NumericUpDown Pi;
        private System.Windows.Forms.ToolStripMenuItem volacoToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem otvorToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel1;
    }
}

