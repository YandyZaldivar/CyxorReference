namespace Halo.ConexionControls
{
    partial class ServidorConexionControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ConfirmarPanel.SuspendLayout();
            this.ErrorPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConfirmarPanel
            // 
            this.ConfirmarPanel.Size = new System.Drawing.Size(327, 375);
            // 
            // ErrorPanel
            // 
            this.ErrorPanel.Location = new System.Drawing.Point(0, 375);
            this.ErrorPanel.Size = new System.Drawing.Size(327, 77);
            // 
            // button7
            // 
            this.button7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            // 
            // button8
            // 
            this.button8.FlatAppearance.BorderColor = System.Drawing.Color.Green;
            this.button8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.Size = new System.Drawing.Size(327, 77);
            // 
            // ServidorConexionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ServidorConexionControl";
            this.ConfirmarPanel.ResumeLayout(false);
            this.ErrorPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
