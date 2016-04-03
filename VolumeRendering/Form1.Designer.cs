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
            this.ValOpEdit_button = new System.Windows.Forms.Button();
            this.listView3 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.valCol_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.ValCol_buttonEdit = new System.Windows.Forms.Button();
            this.ValCol_buttonAdd = new System.Windows.Forms.Button();
            this.alphaReduce_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.stepSize_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.ValColDel_button = new System.Windows.Forms.Button();
            this.ValOpDel_button = new System.Windows.Forms.Button();
            this.ValOpApply_button = new System.Windows.Forms.Button();
            this.ValColApply_button = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.value_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.opacity_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valCol_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.alphaReduce_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepSize_numericUpDown)).BeginInit();
            this.panel2.SuspendLayout();
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
            this.glControl1.Size = new System.Drawing.Size(811, 685);
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
            this.panel1.Controls.Add(this.glControl1);
            this.panel1.Location = new System.Drawing.Point(194, 14);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(819, 692);
            this.panel1.TabIndex = 1;
            // 
            // ValOp_ListView
            // 
            this.ValOp_ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Value,
            this.Opacity});
            this.ValOp_ListView.FullRowSelect = true;
            this.ValOp_ListView.Location = new System.Drawing.Point(3, 3);
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
            this.listView2.Location = new System.Drawing.Point(65, 325);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(66, 216);
            this.listView2.TabIndex = 2;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // Farba
            // 
            this.Farba.Text = "Color";
            // 
            // value_numericUpDown
            // 
            this.value_numericUpDown.Location = new System.Drawing.Point(3, 225);
            this.value_numericUpDown.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.value_numericUpDown.Name = "value_numericUpDown";
            this.value_numericUpDown.Size = new System.Drawing.Size(79, 22);
            this.value_numericUpDown.TabIndex = 3;
            // 
            // opacity_numericUpDown
            // 
            this.opacity_numericUpDown.DecimalPlaces = 2;
            this.opacity_numericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.opacity_numericUpDown.Location = new System.Drawing.Point(88, 225);
            this.opacity_numericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.opacity_numericUpDown.Name = "opacity_numericUpDown";
            this.opacity_numericUpDown.Size = new System.Drawing.Size(71, 22);
            this.opacity_numericUpDown.TabIndex = 4;
            // 
            // color_label
            // 
            this.color_label.AutoSize = true;
            this.color_label.BackColor = System.Drawing.Color.Red;
            this.color_label.Location = new System.Drawing.Point(79, 549);
            this.color_label.Name = "color_label";
            this.color_label.Size = new System.Drawing.Size(44, 17);
            this.color_label.TabIndex = 5;
            this.color_label.Text = "         ";
            this.color_label.Click += new System.EventHandler(this.color_label_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 253);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 27);
            this.button1.TabIndex = 6;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ValOpEdit_button
            // 
            this.ValOpEdit_button.Location = new System.Drawing.Point(68, 253);
            this.ValOpEdit_button.Name = "ValOpEdit_button";
            this.ValOpEdit_button.Size = new System.Drawing.Size(59, 28);
            this.ValOpEdit_button.TabIndex = 7;
            this.ValOpEdit_button.Text = "Edit";
            this.ValOpEdit_button.UseVisualStyleBackColor = true;
            this.ValOpEdit_button.Click += new System.EventHandler(this.button2_Click);
            // 
            // listView3
            // 
            this.listView3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView3.FullRowSelect = true;
            this.listView3.Location = new System.Drawing.Point(3, 325);
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
            this.valCol_numericUpDown.Location = new System.Drawing.Point(3, 547);
            this.valCol_numericUpDown.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.valCol_numericUpDown.Name = "valCol_numericUpDown";
            this.valCol_numericUpDown.Size = new System.Drawing.Size(66, 22);
            this.valCol_numericUpDown.TabIndex = 9;
            // 
            // ValCol_buttonEdit
            // 
            this.ValCol_buttonEdit.Location = new System.Drawing.Point(68, 575);
            this.ValCol_buttonEdit.Name = "ValCol_buttonEdit";
            this.ValCol_buttonEdit.Size = new System.Drawing.Size(59, 28);
            this.ValCol_buttonEdit.TabIndex = 11;
            this.ValCol_buttonEdit.Text = "Edit";
            this.ValCol_buttonEdit.UseVisualStyleBackColor = true;
            this.ValCol_buttonEdit.Click += new System.EventHandler(this.ValCol_buttonEdit_Click);
            // 
            // ValCol_buttonAdd
            // 
            this.ValCol_buttonAdd.Location = new System.Drawing.Point(3, 575);
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
            this.alphaReduce_numericUpDown.Location = new System.Drawing.Point(3, 661);
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
            this.stepSize_numericUpDown.Location = new System.Drawing.Point(83, 662);
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
            // ValColDel_button
            // 
            this.ValColDel_button.Location = new System.Drawing.Point(68, 608);
            this.ValColDel_button.Name = "ValColDel_button";
            this.ValColDel_button.Size = new System.Drawing.Size(59, 28);
            this.ValColDel_button.TabIndex = 14;
            this.ValColDel_button.Text = "Del";
            this.ValColDel_button.UseVisualStyleBackColor = true;
            this.ValColDel_button.Click += new System.EventHandler(this.ValColDel_button_Click);
            // 
            // ValOpDel_button
            // 
            this.ValOpDel_button.Location = new System.Drawing.Point(68, 287);
            this.ValOpDel_button.Name = "ValOpDel_button";
            this.ValOpDel_button.Size = new System.Drawing.Size(59, 28);
            this.ValOpDel_button.TabIndex = 15;
            this.ValOpDel_button.Text = "Del";
            this.ValOpDel_button.UseVisualStyleBackColor = true;
            this.ValOpDel_button.Click += new System.EventHandler(this.ValOpDel_button_Click);
            // 
            // ValOpApply_button
            // 
            this.ValOpApply_button.Location = new System.Drawing.Point(3, 286);
            this.ValOpApply_button.Name = "ValOpApply_button";
            this.ValOpApply_button.Size = new System.Drawing.Size(59, 28);
            this.ValOpApply_button.TabIndex = 16;
            this.ValOpApply_button.Text = "Apply";
            this.ValOpApply_button.UseVisualStyleBackColor = true;
            this.ValOpApply_button.Click += new System.EventHandler(this.ValOpApply_button_Click);
            // 
            // ValColApply_button
            // 
            this.ValColApply_button.Location = new System.Drawing.Point(3, 608);
            this.ValColApply_button.Name = "ValColApply_button";
            this.ValColApply_button.Size = new System.Drawing.Size(59, 28);
            this.ValColApply_button.TabIndex = 17;
            this.ValColApply_button.Text = "Apply";
            this.ValColApply_button.UseVisualStyleBackColor = true;
            this.ValColApply_button.Click += new System.EventHandler(this.ValColApply_button_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.ValColApply_button);
            this.panel2.Controls.Add(this.stepSize_numericUpDown);
            this.panel2.Controls.Add(this.ValOp_ListView);
            this.panel2.Controls.Add(this.alphaReduce_numericUpDown);
            this.panel2.Controls.Add(this.ValColDel_button);
            this.panel2.Controls.Add(this.ValOpApply_button);
            this.panel2.Controls.Add(this.value_numericUpDown);
            this.panel2.Controls.Add(this.ValOpDel_button);
            this.panel2.Controls.Add(this.ValCol_buttonEdit);
            this.panel2.Controls.Add(this.opacity_numericUpDown);
            this.panel2.Controls.Add(this.ValCol_buttonAdd);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.valCol_numericUpDown);
            this.panel2.Controls.Add(this.ValOpEdit_button);
            this.panel2.Controls.Add(this.listView3);
            this.panel2.Controls.Add(this.color_label);
            this.panel2.Controls.Add(this.listView2);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(176, 690);
            this.panel2.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(83, 642);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 17);
            this.label2.TabIndex = 19;
            this.label2.Text = "Step size:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 640);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 17);
            this.label1.TabIndex = 18;
            this.label1.Text = "A reduce:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 715);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.value_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.opacity_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valCol_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.alphaReduce_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepSize_numericUpDown)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
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
        private System.Windows.Forms.Button ValOpEdit_button;
        private System.Windows.Forms.ListView listView3;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.NumericUpDown valCol_numericUpDown;
        private System.Windows.Forms.Button ValCol_buttonEdit;
        private System.Windows.Forms.Button ValCol_buttonAdd;
        private System.Windows.Forms.NumericUpDown alphaReduce_numericUpDown;
        private System.Windows.Forms.NumericUpDown stepSize_numericUpDown;
        private System.Windows.Forms.Button ValColDel_button;
        private System.Windows.Forms.Button ValOpDel_button;
        private System.Windows.Forms.Button ValOpApply_button;
        private System.Windows.Forms.Button ValColApply_button;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}

