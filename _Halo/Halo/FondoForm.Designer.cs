namespace Halo
{
    partial class FondoForm
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
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.SPLogoPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SPLogoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureBox
            // 
            this.PictureBox.Image = global::Halo.Properties.Resources.HaloLogo;
            this.PictureBox.Location = new System.Drawing.Point(32, 32);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(101, 68);
            this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PictureBox.TabIndex = 0;
            this.PictureBox.TabStop = false;
            this.PictureBox.Visible = false;
            // 
            // SPLogoPictureBox
            // 
            this.SPLogoPictureBox.Image = global::Halo.Properties.Resources.SaludPublica;
            this.SPLogoPictureBox.Location = new System.Drawing.Point(144, 32);
            this.SPLogoPictureBox.Name = "SPLogoPictureBox";
            this.SPLogoPictureBox.Size = new System.Drawing.Size(201, 199);
            this.SPLogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.SPLogoPictureBox.TabIndex = 8;
            this.SPLogoPictureBox.TabStop = false;
            this.SPLogoPictureBox.Visible = false;
            // 
            // FondoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(409, 280);
            this.Controls.Add(this.SPLogoPictureBox);
            this.Controls.Add(this.PictureBox);
            this.Enabled = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FondoForm";
            this.Opacity = 0.9D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "FondoForm";
            this.Shown += new System.EventHandler(this.FondoForm_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FondoForm_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SPLogoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PictureBox;
        private System.Windows.Forms.PictureBox SPLogoPictureBox;
    }
}
