namespace Halo
{
    partial class GestionarPacienteControl
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
            this.MainPanel = new System.Windows.Forms.Panel();
            this.ConfirmarPanel = new System.Windows.Forms.Panel();
            this.ConfirmarGroupBox = new System.Windows.Forms.GroupBox();
            this.CancelarButton = new System.Windows.Forms.Button();
            this.AceptarButton = new System.Windows.Forms.Button();
            this.ErrorPanel = new System.Windows.Forms.Panel();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.TitlePanel = new System.Windows.Forms.Panel();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.MainPanel.SuspendLayout();
            this.ConfirmarPanel.SuspendLayout();
            this.ConfirmarGroupBox.SuspendLayout();
            this.ErrorPanel.SuspendLayout();
            this.TitlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MainPanel.Controls.Add(this.ConfirmarPanel);
            this.MainPanel.Controls.Add(this.ErrorPanel);
            this.MainPanel.Controls.Add(this.TitlePanel);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(331, 454);
            this.MainPanel.TabIndex = 2;
            // 
            // ConfirmarPanel
            // 
            this.ConfirmarPanel.Controls.Add(this.ConfirmarGroupBox);
            this.ConfirmarPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConfirmarPanel.Location = new System.Drawing.Point(0, 127);
            this.ConfirmarPanel.Name = "ConfirmarPanel";
            this.ConfirmarPanel.Padding = new System.Windows.Forms.Padding(29, 0, 29, 0);
            this.ConfirmarPanel.Size = new System.Drawing.Size(329, 198);
            this.ConfirmarPanel.TabIndex = 13;
            // 
            // ConfirmarGroupBox
            // 
            this.ConfirmarGroupBox.Controls.Add(this.CancelarButton);
            this.ConfirmarGroupBox.Controls.Add(this.AceptarButton);
            this.ConfirmarGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConfirmarGroupBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConfirmarGroupBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ConfirmarGroupBox.Location = new System.Drawing.Point(29, 0);
            this.ConfirmarGroupBox.Name = "ConfirmarGroupBox";
            this.ConfirmarGroupBox.Size = new System.Drawing.Size(271, 198);
            this.ConfirmarGroupBox.TabIndex = 7;
            this.ConfirmarGroupBox.TabStop = false;
            this.ConfirmarGroupBox.Text = "Confirmar operación";
            // 
            // CancelarButton
            // 
            this.CancelarButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.CancelarButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CancelarButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelarButton.ForeColor = System.Drawing.Color.Black;
            this.CancelarButton.Location = new System.Drawing.Point(50, 115);
            this.CancelarButton.Name = "CancelarButton";
            this.CancelarButton.Size = new System.Drawing.Size(170, 45);
            this.CancelarButton.TabIndex = 12;
            this.CancelarButton.Text = "Cancelar";
            this.CancelarButton.UseVisualStyleBackColor = true;
            // 
            // AceptarButton
            // 
            this.AceptarButton.FlatAppearance.BorderColor = System.Drawing.Color.Green;
            this.AceptarButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.AceptarButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AceptarButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AceptarButton.ForeColor = System.Drawing.Color.Green;
            this.AceptarButton.Location = new System.Drawing.Point(50, 46);
            this.AceptarButton.Name = "AceptarButton";
            this.AceptarButton.Size = new System.Drawing.Size(170, 45);
            this.AceptarButton.TabIndex = 11;
            this.AceptarButton.Text = "Aceptar";
            this.AceptarButton.UseVisualStyleBackColor = true;
            // 
            // ErrorPanel
            // 
            this.ErrorPanel.Controls.Add(this.ErrorLabel);
            this.ErrorPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ErrorPanel.Location = new System.Drawing.Point(0, 325);
            this.ErrorPanel.Name = "ErrorPanel";
            this.ErrorPanel.Size = new System.Drawing.Size(329, 127);
            this.ErrorPanel.TabIndex = 12;
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.BackColor = System.Drawing.Color.Transparent;
            this.ErrorLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.ErrorLabel.Location = new System.Drawing.Point(0, 0);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(329, 127);
            this.ErrorLabel.TabIndex = 1;
            this.ErrorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TitlePanel
            // 
            this.TitlePanel.Controls.Add(this.TitleLabel);
            this.TitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitlePanel.Location = new System.Drawing.Point(0, 0);
            this.TitlePanel.Name = "TitlePanel";
            this.TitlePanel.Size = new System.Drawing.Size(329, 127);
            this.TitlePanel.TabIndex = 11;
            // 
            // TitleLabel
            // 
            this.TitleLabel.BackColor = System.Drawing.Color.Transparent;
            this.TitleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TitleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.ForeColor = System.Drawing.Color.Green;
            this.TitleLabel.Location = new System.Drawing.Point(0, 0);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(329, 127);
            this.TitleLabel.TabIndex = 1;
            this.TitleLabel.Text = "Title";
            this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GestionarPacienteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "GestionarPacienteControl";
            this.Size = new System.Drawing.Size(331, 454);
            this.MainPanel.ResumeLayout(false);
            this.ConfirmarPanel.ResumeLayout(false);
            this.ConfirmarGroupBox.ResumeLayout(false);
            this.ErrorPanel.ResumeLayout(false);
            this.TitlePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Panel ConfirmarPanel;
        private System.Windows.Forms.GroupBox ConfirmarGroupBox;
        private System.Windows.Forms.Panel ErrorPanel;
        private System.Windows.Forms.Label ErrorLabel;
        private System.Windows.Forms.Panel TitlePanel;
        private System.Windows.Forms.Label TitleLabel;
        internal System.Windows.Forms.Button CancelarButton;
        internal System.Windows.Forms.Button AceptarButton;
    }
}
