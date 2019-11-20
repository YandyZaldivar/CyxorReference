namespace Halo
{
    partial class ConexionControl
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
            this.ConfirmarPanel = new System.Windows.Forms.Panel();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.ErrorPanel = new System.Windows.Forms.Panel();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.ConfirmarPanel.SuspendLayout();
            this.ErrorPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConfirmarPanel
            // 
            this.ConfirmarPanel.Controls.Add(this.button7);
            this.ConfirmarPanel.Controls.Add(this.button8);
            this.ConfirmarPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConfirmarPanel.Location = new System.Drawing.Point(0, 0);
            this.ConfirmarPanel.Name = "ConfirmarPanel";
            this.ConfirmarPanel.Padding = new System.Windows.Forms.Padding(29, 0, 29, 0);
            this.ConfirmarPanel.Size = new System.Drawing.Size(329, 377);
            this.ConfirmarPanel.TabIndex = 15;
            // 
            // button7
            // 
            this.button7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.ForeColor = System.Drawing.Color.Black;
            this.button7.Location = new System.Drawing.Point(80, 328);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(170, 45);
            this.button7.TabIndex = 32;
            this.button7.Text = "Cancelar";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.FlatAppearance.BorderColor = System.Drawing.Color.Green;
            this.button8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.ForeColor = System.Drawing.Color.Green;
            this.button8.Location = new System.Drawing.Point(80, 264);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(170, 45);
            this.button8.TabIndex = 31;
            this.button8.Text = "Aceptar";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // ErrorPanel
            // 
            this.ErrorPanel.Controls.Add(this.ErrorLabel);
            this.ErrorPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ErrorPanel.Location = new System.Drawing.Point(0, 377);
            this.ErrorPanel.Name = "ErrorPanel";
            this.ErrorPanel.Size = new System.Drawing.Size(329, 77);
            this.ErrorPanel.TabIndex = 13;
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.BackColor = System.Drawing.Color.Transparent;
            this.ErrorLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.ErrorLabel.Location = new System.Drawing.Point(0, 0);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(329, 77);
            this.ErrorLabel.TabIndex = 2;
            this.ErrorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ConexionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.ConfirmarPanel);
            this.Controls.Add(this.ErrorPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ConexionControl";
            this.Size = new System.Drawing.Size(329, 454);
            this.ConfirmarPanel.ResumeLayout(false);
            this.ErrorPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Panel ConfirmarPanel;
        protected System.Windows.Forms.Panel ErrorPanel;
        protected System.Windows.Forms.Button button7;
        protected System.Windows.Forms.Button button8;
        protected System.Windows.Forms.Label ErrorLabel;
    }
}
