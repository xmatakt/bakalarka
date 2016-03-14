namespace Kocka
{
    partial class MaterialControl
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
            this.Shinlabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ShininessTrackBar = new System.Windows.Forms.TrackBar();
            this.AmbLabel = new System.Windows.Forms.Label();
            this.DiffLabel = new System.Windows.Forms.Label();
            this.SpecLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.AmbientTrackBar = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.DiffuseTrackBar = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.SpecularTrackBar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.ShininessTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmbientTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiffuseTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpecularTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // Shinlabel
            // 
            this.Shinlabel.AutoSize = true;
            this.Shinlabel.Location = new System.Drawing.Point(208, 206);
            this.Shinlabel.Name = "Shinlabel";
            this.Shinlabel.Size = new System.Drawing.Size(46, 17);
            this.Shinlabel.TabIndex = 32;
            this.Shinlabel.Text = "label6";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 210);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 17);
            this.label5.TabIndex = 31;
            this.label5.Text = "Shininess";
            // 
            // ShininessTrackBar
            // 
            this.ShininessTrackBar.Location = new System.Drawing.Point(98, 201);
            this.ShininessTrackBar.Maximum = 128;
            this.ShininessTrackBar.Name = "ShininessTrackBar";
            this.ShininessTrackBar.Size = new System.Drawing.Size(104, 56);
            this.ShininessTrackBar.TabIndex = 30;
            this.ShininessTrackBar.Value = 128;
            this.ShininessTrackBar.ValueChanged += new System.EventHandler(this.ShininessTrackBar_ValueChanged);
            // 
            // AmbLabel
            // 
            this.AmbLabel.AutoSize = true;
            this.AmbLabel.Location = new System.Drawing.Point(208, 149);
            this.AmbLabel.Name = "AmbLabel";
            this.AmbLabel.Size = new System.Drawing.Size(46, 17);
            this.AmbLabel.TabIndex = 29;
            this.AmbLabel.Text = "label6";
            // 
            // DiffLabel
            // 
            this.DiffLabel.AutoSize = true;
            this.DiffLabel.Location = new System.Drawing.Point(208, 82);
            this.DiffLabel.Name = "DiffLabel";
            this.DiffLabel.Size = new System.Drawing.Size(46, 17);
            this.DiffLabel.TabIndex = 28;
            this.DiffLabel.Text = "label5";
            // 
            // SpecLabel
            // 
            this.SpecLabel.AutoSize = true;
            this.SpecLabel.Location = new System.Drawing.Point(208, 21);
            this.SpecLabel.Name = "SpecLabel";
            this.SpecLabel.Size = new System.Drawing.Size(46, 17);
            this.SpecLabel.TabIndex = 27;
            this.SpecLabel.Text = "label4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 17);
            this.label3.TabIndex = 26;
            this.label3.Text = "Ambient";
            // 
            // AmbientTrackBar
            // 
            this.AmbientTrackBar.Location = new System.Drawing.Point(98, 144);
            this.AmbientTrackBar.Maximum = 100;
            this.AmbientTrackBar.Name = "AmbientTrackBar";
            this.AmbientTrackBar.Size = new System.Drawing.Size(104, 56);
            this.AmbientTrackBar.TabIndex = 25;
            this.AmbientTrackBar.Value = 29;
            this.AmbientTrackBar.ValueChanged += new System.EventHandler(this.AmbientTrackBar_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 17);
            this.label2.TabIndex = 24;
            this.label2.Text = "Diffuse";
            // 
            // DiffuseTrackBar
            // 
            this.DiffuseTrackBar.Location = new System.Drawing.Point(98, 78);
            this.DiffuseTrackBar.Maximum = 100;
            this.DiffuseTrackBar.Name = "DiffuseTrackBar";
            this.DiffuseTrackBar.Size = new System.Drawing.Size(104, 56);
            this.DiffuseTrackBar.TabIndex = 23;
            this.DiffuseTrackBar.Value = 57;
            this.DiffuseTrackBar.ValueChanged += new System.EventHandler(this.DiffuseTrackBar_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 17);
            this.label1.TabIndex = 22;
            this.label1.Text = "Specular";
            // 
            // SpecularTrackBar
            // 
            this.SpecularTrackBar.Location = new System.Drawing.Point(98, 16);
            this.SpecularTrackBar.Maximum = 100;
            this.SpecularTrackBar.Name = "SpecularTrackBar";
            this.SpecularTrackBar.Size = new System.Drawing.Size(104, 56);
            this.SpecularTrackBar.TabIndex = 21;
            this.SpecularTrackBar.Value = 86;
            this.SpecularTrackBar.ValueChanged += new System.EventHandler(this.SpecularTrackBar_ValueChanged);
            // 
            // MaterialControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 300);
            this.Controls.Add(this.Shinlabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ShininessTrackBar);
            this.Controls.Add(this.AmbLabel);
            this.Controls.Add(this.DiffLabel);
            this.Controls.Add(this.SpecLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AmbientTrackBar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DiffuseTrackBar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SpecularTrackBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MaterialControl";
            this.Text = "MaterialControl";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MaterialControl_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.ShininessTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmbientTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiffuseTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpecularTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Shinlabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar ShininessTrackBar;
        private System.Windows.Forms.Label AmbLabel;
        private System.Windows.Forms.Label DiffLabel;
        private System.Windows.Forms.Label SpecLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar AmbientTrackBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar DiffuseTrackBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar SpecularTrackBar;
    }
}