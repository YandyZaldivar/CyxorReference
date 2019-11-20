namespace Frameview
{
    partial class InitForm
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
            this.TopPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.InitLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.TopPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.TopPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TopPanel.Controls.Add(this.label1);
            this.TopPanel.Controls.Add(this.pictureBox1);
            this.TopPanel.Controls.Add(this.PictureBox);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(310, 110);
            this.TopPanel.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(118, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 32);
            this.label1.TabIndex = 5;
            this.label1.Text = "Frameview";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.InitLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 110);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(310, 282);
            this.panel1.TabIndex = 3;
            // 
            // InitLabel
            // 
            this.InitLabel.AutoSize = true;
            this.InitLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InitLabel.ForeColor = System.Drawing.Color.White;
            this.InitLabel.Location = new System.Drawing.Point(73, 235);
            this.InitLabel.Name = "InitLabel";
            this.InitLabel.Size = new System.Drawing.Size(164, 17);
            this.InitLabel.TabIndex = 4;
            this.InitLabel.Text = "Copyright (c) 2017 Alimatic";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Frameview.Properties.Resources.Frameview64;
            this.pictureBox1.Location = new System.Drawing.Point(34, 23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // PictureBox
            // 
            this.PictureBox.BackColor = System.Drawing.Color.White;
            this.PictureBox.Image = global::Frameview.Properties.Resources.AlimaticLogoVS;
            this.PictureBox.Location = new System.Drawing.Point(12, 13);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(287, 84);
            this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PictureBox.TabIndex = 0;
            this.PictureBox.TabStop = false;
            this.PictureBox.Visible = false;
            // 
            // InitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.ClientSize = new System.Drawing.Size(310, 392);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.TopPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "InitForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "InitForm";
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.PictureBox PictureBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label InitLabel;
    }
}