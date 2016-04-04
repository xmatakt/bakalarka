namespace VolumeRendering
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.ValOp_ListView = new System.Windows.Forms.ListView();
            this.Value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Opacity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView2 = new System.Windows.Forms.ListView();
            this.Farba = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.value_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.opacity_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.color_label = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.listView3 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.valCol_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.ValCol_buttonAdd = new System.Windows.Forms.Button();
            this.alphaReduce_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.stepSize_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.ValOpDel_button = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ValColDel_button = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveIamgeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveTransferFunctionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTransferFunctionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.info_label = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.stepSize_numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.alphaReduce_numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.value_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.opacity_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valCol_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.alphaReduce_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepSize_numericUpDown)).BeginInit();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stepSize_numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.alphaReduce_numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(4, 3);
            this.glControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(807, 690);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDown);
            this.glControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseMove);
            this.glControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseUp);
            this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.glControl1);
            this.panel1.Location = new System.Drawing.Point(194, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(819, 701);
            this.panel1.TabIndex = 1;
            // 
            // ValOp_ListView
            // 
            this.ValOp_ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Value,
            this.Opacity});
            this.ValOp_ListView.FullRowSelect = true;
            this.ValOp_ListView.Location = new System.Drawing.Point(6, 3);
            this.ValOp_ListView.Name = "ValOp_ListView";
            this.ValOp_ListView.Size = new System.Drawing.Size(156, 216);
            this.ValOp_ListView.TabIndex = 1;
            this.ValOp_ListView.UseCompatibleStateImageBehavior = false;
            this.ValOp_ListView.View = System.Windows.Forms.View.Details;
            this.ValOp_ListView.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // Value
            // 
            this.Value.Text = "Value";
            this.Value.Width = 50;
            // 
            // Opacity
            // 
            this.Opacity.Text = "Opacity";
            this.Opacity.Width = 50;
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Farba});
            this.listView2.Location = new System.Drawing.Point(68, 287);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(66, 216);
            this.listView2.TabIndex = 2;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            this.listView2.SelectedIndexChanged += new System.EventHandler(this.listView2_SelectedIndexChanged);
            // 
            // Farba
            // 
            this.Farba.Text = "Color";
            // 
            // value_numericUpDown
            // 
            this.value_numericUpDown.Location = new System.Drawing.Point(6, 225);
            this.value_numericUpDown.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.value_numericUpDown.Name = "value_numericUpDown";
            this.value_numericUpDown.Size = new System.Drawing.Size(79, 22);
            this.value_numericUpDown.TabIndex = 3;
            this.value_numericUpDown.ValueChanged += new System.EventHandler(this.value_numericUpDown_ValueChanged);
            // 
            // opacity_numericUpDown
            // 
            this.opacity_numericUpDown.DecimalPlaces = 2;
            this.opacity_numericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.opacity_numericUpDown.Location = new System.Drawing.Point(91, 225);
            this.opacity_numericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.opacity_numericUpDown.Name = "opacity_numericUpDown";
            this.opacity_numericUpDown.Size = new System.Drawing.Size(71, 22);
            this.opacity_numericUpDown.TabIndex = 4;
            this.opacity_numericUpDown.ValueChanged += new System.EventHandler(this.opacity_numericUpDown_ValueChanged);
            // 
            // color_label
            // 
            this.color_label.AutoSize = true;
            this.color_label.BackColor = System.Drawing.Color.Red;
            this.color_label.Location = new System.Drawing.Point(82, 511);
            this.color_label.Name = "color_label";
            this.color_label.Size = new System.Drawing.Size(44, 17);
            this.color_label.TabIndex = 5;
            this.color_label.Text = "         ";
            this.color_label.Click += new System.EventHandler(this.color_label_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 253);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 27);
            this.button1.TabIndex = 6;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listView3
            // 
            this.listView3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView3.FullRowSelect = true;
            this.listView3.Location = new System.Drawing.Point(6, 287);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(66, 216);
            this.listView3.TabIndex = 8;
            this.listView3.UseCompatibleStateImageBehavior = false;
            this.listView3.View = System.Windows.Forms.View.Details;
            this.listView3.SelectedIndexChanged += new System.EventHandler(this.listView3_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Value";
            // 
            // valCol_numericUpDown
            // 
            this.valCol_numericUpDown.Location = new System.Drawing.Point(6, 509);
            this.valCol_numericUpDown.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.valCol_numericUpDown.Name = "valCol_numericUpDown";
            this.valCol_numericUpDown.Size = new System.Drawing.Size(66, 22);
            this.valCol_numericUpDown.TabIndex = 9;
            this.valCol_numericUpDown.ValueChanged += new System.EventHandler(this.valCol_numericUpDown_ValueChanged);
            // 
            // ValCol_buttonAdd
            // 
            this.ValCol_buttonAdd.Location = new System.Drawing.Point(6, 537);
            this.ValCol_buttonAdd.Name = "ValCol_buttonAdd";
            this.ValCol_buttonAdd.Size = new System.Drawing.Size(59, 27);
            this.ValCol_buttonAdd.TabIndex = 10;
            this.ValCol_buttonAdd.Text = "Add";
            this.ValCol_buttonAdd.UseVisualStyleBackColor = true;
            this.ValCol_buttonAdd.Click += new System.EventHandler(this.ValCol_buttonAdd_Click);
            // 
            // alphaReduce_numericUpDown
            // 
            this.alphaReduce_numericUpDown.DecimalPlaces = 2;
            this.alphaReduce_numericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.alphaReduce_numericUpDown.Location = new System.Drawing.Point(6, 597);
            this.alphaReduce_numericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.alphaReduce_numericUpDown.Name = "alphaReduce_numericUpDown";
            this.alphaReduce_numericUpDown.Size = new System.Drawing.Size(71, 22);
            this.alphaReduce_numericUpDown.TabIndex = 12;
            this.alphaReduce_numericUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.alphaReduce_numericUpDown.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // stepSize_numericUpDown
            // 
            this.stepSize_numericUpDown.DecimalPlaces = 4;
            this.stepSize_numericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.stepSize_numericUpDown.Location = new System.Drawing.Point(83, 597);
            this.stepSize_numericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.stepSize_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.stepSize_numericUpDown.Name = "stepSize_numericUpDown";
            this.stepSize_numericUpDown.Size = new System.Drawing.Size(71, 22);
            this.stepSize_numericUpDown.TabIndex = 13;
            this.stepSize_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.stepSize_numericUpDown.ValueChanged += new System.EventHandler(this.stepSize_numericUpDown_ValueChanged);
            // 
            // ValOpDel_button
            // 
            this.ValOpDel_button.Enabled = false;
            this.ValOpDel_button.Location = new System.Drawing.Point(71, 253);
            this.ValOpDel_button.Name = "ValOpDel_button";
            this.ValOpDel_button.Size = new System.Drawing.Size(59, 28);
            this.ValOpDel_button.TabIndex = 15;
            this.ValOpDel_button.Text = "Del";
            this.ValOpDel_button.UseVisualStyleBackColor = true;
            this.ValOpDel_button.Click += new System.EventHandler(this.ValOpDel_button_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.stepSize_numericUpDown);
            this.panel2.Controls.Add(this.ValOp_ListView);
            this.panel2.Controls.Add(this.alphaReduce_numericUpDown);
            this.panel2.Controls.Add(this.ValColDel_button);
            this.panel2.Controls.Add(this.value_numericUpDown);
            this.panel2.Controls.Add(this.ValOpDel_button);
            this.panel2.Controls.Add(this.opacity_numericUpDown);
            this.panel2.Controls.Add(this.ValCol_buttonAdd);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.valCol_numericUpDown);
            this.panel2.Controls.Add(this.listView3);
            this.panel2.Controls.Add(this.color_label);
            this.panel2.Controls.Add(this.listView2);
            this.panel2.Location = new System.Drawing.Point(12, 42);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(176, 697);
            this.panel2.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(83, 577);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 17);
            this.label2.TabIndex = 19;
            this.label2.Text = "Step size:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 577);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 17);
            this.label1.TabIndex = 18;
            this.label1.Text = "A reduce:";
            // 
            // ValColDel_button
            // 
            this.ValColDel_button.Enabled = false;
            this.ValColDel_button.Location = new System.Drawing.Point(71, 537);
            this.ValColDel_button.Name = "ValColDel_button";
            this.ValColDel_button.Size = new System.Drawing.Size(59, 28);
            this.ValColDel_button.TabIndex = 14;
            this.ValColDel_button.Text = "Del";
            this.ValColDel_button.UseVisualStyleBackColor = true;
            this.ValColDel_button.Click += new System.EventHandler(this.ValColDel_button_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "TSF files (*.tsf)|*.tsf|VTK files (*.vtk)|*.vtk";
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1028, 28);
            this.menuStrip1.TabIndex = 19;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveIamgeToolStripMenuItem,
            this.saveTransferFunctionToolStripMenuItem,
            this.loadTransferFunctionToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(44, 24);
            this.toolStripMenuItem1.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveIamgeToolStripMenuItem
            // 
            this.saveIamgeToolStripMenuItem.Name = "saveIamgeToolStripMenuItem";
            this.saveIamgeToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.saveIamgeToolStripMenuItem.Text = "Save image...";
            this.saveIamgeToolStripMenuItem.Click += new System.EventHandler(this.saveIamgeToolStripMenuItem_Click);
            // 
            // saveTransferFunctionToolStripMenuItem
            // 
            this.saveTransferFunctionToolStripMenuItem.Name = "saveTransferFunctionToolStripMenuItem";
            this.saveTransferFunctionToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.saveTransferFunctionToolStripMenuItem.Text = "Save transfer function...";
            this.saveTransferFunctionToolStripMenuItem.Click += new System.EventHandler(this.saveTransferFunctionToolStripMenuItem_Click);
            // 
            // loadTransferFunctionToolStripMenuItem
            // 
            this.loadTransferFunctionToolStripMenuItem.Name = "loadTransferFunctionToolStripMenuItem";
            this.loadTransferFunctionToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.loadTransferFunctionToolStripMenuItem.Text = "Load transfer function...";
            this.loadTransferFunctionToolStripMenuItem.Click += new System.EventHandler(this.loadTransferFunctionToolStripMenuItem_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "PNG files (*.png)|*.png";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(10, 192);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(79, 35);
            this.button2.TabIndex = 21;
            this.button2.Text = "OwnTff";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // info_label
            // 
            this.info_label.Location = new System.Drawing.Point(12, 12);
            this.info_label.Name = "info_label";
            this.info_label.Size = new System.Drawing.Size(163, 123);
            this.info_label.TabIndex = 20;
            this.info_label.Text = "Now you are using loaded transfer function which can not be edited.\r\nTo make your" +
    " own transfer function, click OwnTff button.\r\n";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.stepSize_numericUpDown2);
            this.panel3.Controls.Add(this.alphaReduce_numericUpDown2);
            this.panel3.Controls.Add(this.button2);
            this.panel3.Controls.Add(this.info_label);
            this.panel3.Location = new System.Drawing.Point(14, 42);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(179, 245);
            this.panel3.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(90, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 17);
            this.label3.TabIndex = 25;
            this.label3.Text = "Step size:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 17);
            this.label4.TabIndex = 24;
            this.label4.Text = "A reduce:";
            // 
            // stepSize_numericUpDown2
            // 
            this.stepSize_numericUpDown2.DecimalPlaces = 4;
            this.stepSize_numericUpDown2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.stepSize_numericUpDown2.Location = new System.Drawing.Point(90, 154);
            this.stepSize_numericUpDown2.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.stepSize_numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.stepSize_numericUpDown2.Name = "stepSize_numericUpDown2";
            this.stepSize_numericUpDown2.Size = new System.Drawing.Size(71, 22);
            this.stepSize_numericUpDown2.TabIndex = 23;
            this.stepSize_numericUpDown2.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.stepSize_numericUpDown2.ValueChanged += new System.EventHandler(this.stepSize_numericUpDown_ValueChanged);
            // 
            // alphaReduce_numericUpDown2
            // 
            this.alphaReduce_numericUpDown2.DecimalPlaces = 2;
            this.alphaReduce_numericUpDown2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.alphaReduce_numericUpDown2.Location = new System.Drawing.Point(13, 154);
            this.alphaReduce_numericUpDown2.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.alphaReduce_numericUpDown2.Name = "alphaReduce_numericUpDown2";
            this.alphaReduce_numericUpDown2.Size = new System.Drawing.Size(71, 22);
            this.alphaReduce_numericUpDown2.TabIndex = 22;
            this.alphaReduce_numericUpDown2.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.alphaReduce_numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 752);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.value_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.opacity_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valCol_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.alphaReduce_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepSize_numericUpDown)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stepSize_numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.alphaReduce_numericUpDown2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView ValOp_ListView;
        private System.Windows.Forms.ColumnHeader Value;
        private System.Windows.Forms.ColumnHeader Opacity;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader Farba;
        private System.Windows.Forms.NumericUpDown value_numericUpDown;
        private System.Windows.Forms.NumericUpDown opacity_numericUpDown;
        private System.Windows.Forms.Label color_label;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ListView listView3;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.NumericUpDown valCol_numericUpDown;
        private System.Windows.Forms.Button ValCol_buttonAdd;
        private System.Windows.Forms.NumericUpDown alphaReduce_numericUpDown;
        private System.Windows.Forms.NumericUpDown stepSize_numericUpDown;
        private System.Windows.Forms.Button ValOpDel_button;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveIamgeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveTransferFunctionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTransferFunctionToolStripMenuItem;
        private System.Windows.Forms.Button ValColDel_button;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label info_label;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown stepSize_numericUpDown2;
        private System.Windows.Forms.NumericUpDown alphaReduce_numericUpDown2;
    }
}

