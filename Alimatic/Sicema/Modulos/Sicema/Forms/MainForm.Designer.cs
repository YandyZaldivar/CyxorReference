namespace Sicema
{
    partial class MainForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.LeftSplitContainer = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.VariablesLabel = new System.Windows.Forms.Label();
            this.VariablesTextBox = new System.Windows.Forms.TextBox();
            this.VariablesEliminarButton = new System.Windows.Forms.Button();
            this.VariablesAdicionarButton = new System.Windows.Forms.Button();
            this.VariablesComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ColumnasTextBox = new System.Windows.Forms.TextBox();
            this.ColumnasEliminarButton = new System.Windows.Forms.Button();
            this.ColumnasAdicionarButton = new System.Windows.Forms.Button();
            this.ColumnasComboBox = new System.Windows.Forms.ComboBox();
            this.DataGridView = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.FormulaTextBox = new System.Windows.Forms.TextBox();
            this.A = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.B = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.D = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.E = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.F = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.LeftSplitContainer)).BeginInit();
            this.LeftSplitContainer.Panel1.SuspendLayout();
            this.LeftSplitContainer.Panel2.SuspendLayout();
            this.LeftSplitContainer.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LeftSplitContainer
            // 
            this.LeftSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LeftSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.LeftSplitContainer.IsSplitterFixed = true;
            this.LeftSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.LeftSplitContainer.Name = "LeftSplitContainer";
            // 
            // LeftSplitContainer.Panel1
            // 
            this.LeftSplitContainer.Panel1.Controls.Add(this.groupBox2);
            this.LeftSplitContainer.Panel1.Controls.Add(this.groupBox1);
            // 
            // LeftSplitContainer.Panel2
            // 
            this.LeftSplitContainer.Panel2.Controls.Add(this.DataGridView);
            this.LeftSplitContainer.Panel2.Controls.Add(this.panel1);
            this.LeftSplitContainer.Size = new System.Drawing.Size(793, 461);
            this.LeftSplitContainer.SplitterDistance = 200;
            this.LeftSplitContainer.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.VariablesLabel);
            this.groupBox2.Controls.Add(this.VariablesTextBox);
            this.groupBox2.Controls.Add(this.VariablesEliminarButton);
            this.groupBox2.Controls.Add(this.VariablesAdicionarButton);
            this.groupBox2.Controls.Add(this.VariablesComboBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 156);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(178, 252);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Variables:";
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(21, 134);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(122, 97);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "Ejemplo:\r\n\r\nx = 1\r\ny = rq + n\r\nn = (x + 2) * 3^2\r\nrq = z + x\r\nz = 8";
            // 
            // VariablesLabel
            // 
            this.VariablesLabel.Location = new System.Drawing.Point(19, 53);
            this.VariablesLabel.Name = "VariablesLabel";
            this.VariablesLabel.Size = new System.Drawing.Size(124, 13);
            this.VariablesLabel.TabIndex = 5;
            this.VariablesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VariablesTextBox
            // 
            this.VariablesTextBox.Location = new System.Drawing.Point(22, 69);
            this.VariablesTextBox.Name = "VariablesTextBox";
            this.VariablesTextBox.Size = new System.Drawing.Size(121, 20);
            this.VariablesTextBox.TabIndex = 4;
            // 
            // VariablesEliminarButton
            // 
            this.VariablesEliminarButton.Location = new System.Drawing.Point(83, 95);
            this.VariablesEliminarButton.Name = "VariablesEliminarButton";
            this.VariablesEliminarButton.Size = new System.Drawing.Size(60, 23);
            this.VariablesEliminarButton.TabIndex = 3;
            this.VariablesEliminarButton.Text = "Eliminar";
            this.VariablesEliminarButton.UseVisualStyleBackColor = true;
            this.VariablesEliminarButton.Click += new System.EventHandler(this.VariablesEliminarButton_Click);
            // 
            // VariablesAdicionarButton
            // 
            this.VariablesAdicionarButton.Location = new System.Drawing.Point(22, 95);
            this.VariablesAdicionarButton.Name = "VariablesAdicionarButton";
            this.VariablesAdicionarButton.Size = new System.Drawing.Size(60, 23);
            this.VariablesAdicionarButton.TabIndex = 2;
            this.VariablesAdicionarButton.Text = "Adicionar";
            this.VariablesAdicionarButton.UseVisualStyleBackColor = true;
            this.VariablesAdicionarButton.Click += new System.EventHandler(this.VariablesAdicionarButton_Click);
            // 
            // VariablesComboBox
            // 
            this.VariablesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VariablesComboBox.FormattingEnabled = true;
            this.VariablesComboBox.Location = new System.Drawing.Point(22, 28);
            this.VariablesComboBox.Name = "VariablesComboBox";
            this.VariablesComboBox.Size = new System.Drawing.Size(121, 21);
            this.VariablesComboBox.TabIndex = 1;
            this.VariablesComboBox.DropDown += new System.EventHandler(this.VariablesComboBox_DropDown);
            this.VariablesComboBox.SelectedIndexChanged += new System.EventHandler(this.VariablesComboBox_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ColumnasTextBox);
            this.groupBox1.Controls.Add(this.ColumnasEliminarButton);
            this.groupBox1.Controls.Add(this.ColumnasAdicionarButton);
            this.groupBox1.Controls.Add(this.ColumnasComboBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(178, 136);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Columnas:";
            // 
            // ColumnasTextBox
            // 
            this.ColumnasTextBox.Location = new System.Drawing.Point(22, 69);
            this.ColumnasTextBox.Name = "ColumnasTextBox";
            this.ColumnasTextBox.Size = new System.Drawing.Size(121, 20);
            this.ColumnasTextBox.TabIndex = 5;
            // 
            // ColumnasEliminarButton
            // 
            this.ColumnasEliminarButton.Location = new System.Drawing.Point(83, 95);
            this.ColumnasEliminarButton.Name = "ColumnasEliminarButton";
            this.ColumnasEliminarButton.Size = new System.Drawing.Size(60, 23);
            this.ColumnasEliminarButton.TabIndex = 3;
            this.ColumnasEliminarButton.Text = "Eliminar";
            this.ColumnasEliminarButton.UseVisualStyleBackColor = true;
            this.ColumnasEliminarButton.Click += new System.EventHandler(this.ColumnasEliminarButton_Click);
            // 
            // ColumnasAdicionarButton
            // 
            this.ColumnasAdicionarButton.Location = new System.Drawing.Point(22, 95);
            this.ColumnasAdicionarButton.Name = "ColumnasAdicionarButton";
            this.ColumnasAdicionarButton.Size = new System.Drawing.Size(60, 23);
            this.ColumnasAdicionarButton.TabIndex = 2;
            this.ColumnasAdicionarButton.Text = "Adicionar";
            this.ColumnasAdicionarButton.UseVisualStyleBackColor = true;
            this.ColumnasAdicionarButton.Click += new System.EventHandler(this.ColumnasAdicionarButton_Click);
            // 
            // ColumnasComboBox
            // 
            this.ColumnasComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ColumnasComboBox.FormattingEnabled = true;
            this.ColumnasComboBox.Location = new System.Drawing.Point(22, 28);
            this.ColumnasComboBox.Name = "ColumnasComboBox";
            this.ColumnasComboBox.Size = new System.Drawing.Size(121, 21);
            this.ColumnasComboBox.TabIndex = 1;
            this.ColumnasComboBox.DropDown += new System.EventHandler(this.ColumnasComboBox_DropDown);
            this.ColumnasComboBox.SelectedIndexChanged += new System.EventHandler(this.ColumnasComboBox_SelectedIndexChanged);
            // 
            // DataGridView
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DataGridView.BackgroundColor = System.Drawing.Color.White;
            this.DataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.A,
            this.B,
            this.C,
            this.D,
            this.E,
            this.F});
            this.DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView.Location = new System.Drawing.Point(0, 33);
            this.DataGridView.Name = "DataGridView";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.DataGridView.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.DataGridView.Size = new System.Drawing.Size(589, 428);
            this.DataGridView.TabIndex = 3;
            this.DataGridView.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.DataGridView_CellBeginEdit);
            this.DataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellContentClick);
            this.DataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellEndEdit);
            this.DataGridView.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellEnter);
            this.DataGridView.CellStateChanged += new System.Windows.Forms.DataGridViewCellStateChangedEventHandler(this.DataGridView_CellStateChanged);
            this.DataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellValueChanged);
            this.DataGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.DataGridView_RowsAdded);
            this.DataGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.DataGridView_RowsRemoved);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.FormulaTextBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(589, 33);
            this.panel1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Fórmula:";
            // 
            // FormulaTextBox
            // 
            this.FormulaTextBox.Enabled = false;
            this.FormulaTextBox.Location = new System.Drawing.Point(70, 7);
            this.FormulaTextBox.Name = "FormulaTextBox";
            this.FormulaTextBox.Size = new System.Drawing.Size(514, 20);
            this.FormulaTextBox.TabIndex = 0;
            // 
            // A
            // 
            this.A.HeaderText = "A";
            this.A.Name = "A";
            // 
            // B
            // 
            this.B.HeaderText = "B";
            this.B.Name = "B";
            // 
            // C
            // 
            this.C.HeaderText = "C";
            this.C.Name = "C";
            // 
            // D
            // 
            this.D.HeaderText = "D";
            this.D.Name = "D";
            // 
            // E
            // 
            this.E.HeaderText = "E";
            this.E.Name = "E";
            // 
            // F
            // 
            this.F.HeaderText = "F";
            this.F.Name = "F";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 461);
            this.Controls.Add(this.LeftSplitContainer);
            this.Name = "MainForm";
            this.Text = "Costo - Prueba de concepto - Alimatic";
            this.LeftSplitContainer.Panel1.ResumeLayout(false);
            this.LeftSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LeftSplitContainer)).EndInit();
            this.LeftSplitContainer.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer LeftSplitContainer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox VariablesTextBox;
        private System.Windows.Forms.Button VariablesEliminarButton;
        private System.Windows.Forms.Button VariablesAdicionarButton;
        private System.Windows.Forms.ComboBox VariablesComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ColumnasEliminarButton;
        private System.Windows.Forms.Button ColumnasAdicionarButton;
        private System.Windows.Forms.ComboBox ColumnasComboBox;
        private System.Windows.Forms.TextBox ColumnasTextBox;
        private System.Windows.Forms.Label VariablesLabel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView DataGridView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FormulaTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn A;
        private System.Windows.Forms.DataGridViewTextBoxColumn B;
        private System.Windows.Forms.DataGridViewTextBoxColumn C;
        private System.Windows.Forms.DataGridViewTextBoxColumn D;
        private System.Windows.Forms.DataGridViewTextBoxColumn E;
        private System.Windows.Forms.DataGridViewTextBoxColumn F;
    }
}

