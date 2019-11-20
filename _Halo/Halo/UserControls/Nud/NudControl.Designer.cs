namespace Halo
{
    partial class NudControl
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
            this.Panel = new System.Windows.Forms.Panel();
            this.Label = new System.Windows.Forms.Label();
            this.NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // Panel
            // 
            this.Panel.BackColor = System.Drawing.Color.White;
            this.Panel.Controls.Add(this.Label);
            this.Panel.Location = new System.Drawing.Point(1, 2);
            this.Panel.Name = "Panel";
            this.Panel.Size = new System.Drawing.Size(19, 19);
            this.Panel.TabIndex = 0;
            // 
            // Label
            // 
            this.Label.BackColor = System.Drawing.Color.White;
            this.Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label.Location = new System.Drawing.Point(0, 0);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(19, 19);
            this.Label.TabIndex = 0;
            this.Label.Text = "97";
            this.Label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Label.Visible = false;
            // 
            // NumericUpDown
            // 
            this.NumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NumericUpDown.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NumericUpDown.Location = new System.Drawing.Point(0, 0);
            this.NumericUpDown.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.NumericUpDown.Name = "NumericUpDown";
            this.NumericUpDown.Size = new System.Drawing.Size(36, 23);
            this.NumericUpDown.TabIndex = 1;
            this.NumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // NudControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.Panel);
            this.Controls.Add(this.NumericUpDown);
            this.Name = "NudControl";
            this.Size = new System.Drawing.Size(36, 23);
            this.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel;
        private System.Windows.Forms.NumericUpDown NumericUpDown;
        private System.Windows.Forms.Label Label;
    }
}
