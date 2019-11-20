namespace Configuracion
{
    partial class frmPrincipal
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrincipal));
            this.frConectarse = new System.Windows.Forms.GroupBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.optSeguridad1 = new System.Windows.Forms.RadioButton();
            this.optSeguridad0 = new System.Windows.Forms.RadioButton();
            this.btnProbar = new System.Windows.Forms.Button();
            this.txtContrasenna = new System.Windows.Forms.TextBox();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.cbxCatalogo = new System.Windows.Forms.ComboBox();
            this.cbxServidor = new System.Windows.Forms.ComboBox();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.lblContrasenna = new System.Windows.Forms.Label();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.lblSeguridad = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.lblServidor = new System.Windows.Forms.Label();
            this.frParametros = new System.Windows.Forms.GroupBox();
            this.btnReportes = new System.Windows.Forms.Button();
            this.txtCaminoReportes = new System.Windows.Forms.TextBox();
            this.txtCaminoSalva = new System.Windows.Forms.TextBox();
            this.chkMulticompannia = new System.Windows.Forms.CheckBox();
            this.btnSalvas = new System.Windows.Forms.Button();
            this.lblCaminoSalva = new System.Windows.Forms.Label();
            this.lblCaminoReportes = new System.Windows.Forms.Label();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.ttMensaje = new System.Windows.Forms.ToolTip(this.components);
            this.errIconoError = new System.Windows.Forms.ErrorProvider(this.components);
            this.errIconoInfo = new System.Windows.Forms.ErrorProvider(this.components);
            this.frConectarse.SuspendLayout();
            this.frParametros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errIconoError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errIconoInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // frConectarse
            // 
            this.frConectarse.AllowDrop = true;
            this.frConectarse.BackColor = System.Drawing.SystemColors.Control;
            this.frConectarse.Controls.Add(this.lblVersion);
            this.frConectarse.Controls.Add(this.optSeguridad1);
            this.frConectarse.Controls.Add(this.optSeguridad0);
            this.frConectarse.Controls.Add(this.btnProbar);
            this.frConectarse.Controls.Add(this.txtContrasenna);
            this.frConectarse.Controls.Add(this.txtUsuario);
            this.frConectarse.Controls.Add(this.cbxCatalogo);
            this.frConectarse.Controls.Add(this.cbxServidor);
            this.frConectarse.Controls.Add(this.btnActualizar);
            this.frConectarse.Controls.Add(this.lblContrasenna);
            this.frConectarse.Controls.Add(this.lblDatabase);
            this.frConectarse.Controls.Add(this.lblSeguridad);
            this.frConectarse.Controls.Add(this.lblUsuario);
            this.frConectarse.Controls.Add(this.lblServidor);
            this.frConectarse.ForeColor = System.Drawing.SystemColors.ControlText;
            this.frConectarse.Location = new System.Drawing.Point(10, 10);
            this.frConectarse.Name = "frConectarse";
            this.frConectarse.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.frConectarse.Size = new System.Drawing.Size(480, 217);
            this.frConectarse.TabIndex = 0;
            this.frConectarse.TabStop = false;
            this.frConectarse.Text = "   Información para conectarse al Servidor SQL";
            // 
            // lblVersion
            // 
            this.lblVersion.AllowDrop = true;
            this.lblVersion.AutoSize = true;
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblVersion.Location = new System.Drawing.Point(370, 16);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblVersion.Size = new System.Drawing.Size(42, 13);
            this.lblVersion.TabIndex = 14;
            this.lblVersion.Text = "Version";
            this.lblVersion.Visible = false;
            // 
            // optSeguridad1
            // 
            this.optSeguridad1.AllowDrop = true;
            this.optSeguridad1.BackColor = System.Drawing.SystemColors.Control;
            this.optSeguridad1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.optSeguridad1.Location = new System.Drawing.Point(32, 128);
            this.optSeguridad1.Name = "optSeguridad1";
            this.optSeguridad1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.optSeguridad1.Size = new System.Drawing.Size(247, 17);
            this.optSeguridad1.TabIndex = 5;
            this.optSeguridad1.Text = "Usar un nombre de usuario y contraseña";
            this.optSeguridad1.UseVisualStyleBackColor = false;
            // 
            // optSeguridad0
            // 
            this.optSeguridad0.AllowDrop = true;
            this.optSeguridad0.BackColor = System.Drawing.SystemColors.Control;
            this.optSeguridad0.Checked = true;
            this.optSeguridad0.ForeColor = System.Drawing.SystemColors.ControlText;
            this.optSeguridad0.Location = new System.Drawing.Point(32, 104);
            this.optSeguridad0.Name = "optSeguridad0";
            this.optSeguridad0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.optSeguridad0.Size = new System.Drawing.Size(247, 17);
            this.optSeguridad0.TabIndex = 4;
            this.optSeguridad0.TabStop = true;
            this.optSeguridad0.Text = "Usar la seguridad integrada de Windows NT";
            this.optSeguridad0.UseVisualStyleBackColor = false;
            this.optSeguridad0.CheckedChanged += new System.EventHandler(this.optSeguridad0_CheckedChanged);
            // 
            // btnProbar
            // 
            this.btnProbar.AllowDrop = true;
            this.btnProbar.BackColor = System.Drawing.SystemColors.Control;
            this.btnProbar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnProbar.Image = global::Configuracion.Properties.Resources.test;
            this.btnProbar.Location = new System.Drawing.Point(264, 170);
            this.btnProbar.Name = "btnProbar";
            this.btnProbar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnProbar.Size = new System.Drawing.Size(100, 35);
            this.btnProbar.TabIndex = 13;
            this.btnProbar.Text = "&Probar";
            this.btnProbar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnProbar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnProbar.UseVisualStyleBackColor = false;
            this.btnProbar.Click += new System.EventHandler(this.btnProbar_Click);
            // 
            // txtContrasenna
            // 
            this.txtContrasenna.AllowDrop = true;
            this.txtContrasenna.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtContrasenna.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtContrasenna.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtContrasenna.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtContrasenna.Location = new System.Drawing.Point(365, 128);
            this.txtContrasenna.MaxLength = 0;
            this.txtContrasenna.Name = "txtContrasenna";
            this.txtContrasenna.PasswordChar = '*';
            this.txtContrasenna.ReadOnly = true;
            this.txtContrasenna.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtContrasenna.Size = new System.Drawing.Size(97, 20);
            this.txtContrasenna.TabIndex = 9;
            this.txtContrasenna.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.txtContrasenna.Validating += new System.ComponentModel.CancelEventHandler(this.Control_Validating);
            // 
            // txtUsuario
            // 
            this.txtUsuario.AllowDrop = true;
            this.txtUsuario.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtUsuario.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtUsuario.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtUsuario.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtUsuario.Location = new System.Drawing.Point(365, 104);
            this.txtUsuario.MaxLength = 0;
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.ReadOnly = true;
            this.txtUsuario.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtUsuario.Size = new System.Drawing.Size(97, 20);
            this.txtUsuario.TabIndex = 7;
            this.txtUsuario.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.txtUsuario.Validating += new System.ComponentModel.CancelEventHandler(this.Control_Validating);
            // 
            // cbxCatalogo
            // 
            this.cbxCatalogo.AllowDrop = true;
            this.cbxCatalogo.BackColor = System.Drawing.SystemColors.Window;
            this.cbxCatalogo.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cbxCatalogo.Location = new System.Drawing.Point(32, 184);
            this.cbxCatalogo.Name = "cbxCatalogo";
            this.cbxCatalogo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbxCatalogo.Size = new System.Drawing.Size(185, 21);
            this.cbxCatalogo.TabIndex = 11;
            this.cbxCatalogo.DropDown += new System.EventHandler(this.cbxCatalogo_DropDown);
            this.cbxCatalogo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            // 
            // cbxServidor
            // 
            this.cbxServidor.AllowDrop = true;
            this.cbxServidor.BackColor = System.Drawing.SystemColors.Window;
            this.cbxServidor.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cbxServidor.Items.AddRange(new object[] {
            "(local)"});
            this.cbxServidor.Location = new System.Drawing.Point(32, 43);
            this.cbxServidor.Name = "cbxServidor";
            this.cbxServidor.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbxServidor.Size = new System.Drawing.Size(185, 21);
            this.cbxServidor.TabIndex = 2;
            this.cbxServidor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            // 
            // btnActualizar
            // 
            this.btnActualizar.AllowDrop = true;
            this.btnActualizar.BackColor = System.Drawing.SystemColors.Control;
            this.btnActualizar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnActualizar.Image = global::Configuracion.Properties.Resources.refresh;
            this.btnActualizar.Location = new System.Drawing.Point(264, 35);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnActualizar.Size = new System.Drawing.Size(100, 35);
            this.btnActualizar.TabIndex = 12;
            this.btnActualizar.Text = "&Actualizar";
            this.btnActualizar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnActualizar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnActualizar.UseVisualStyleBackColor = false;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // lblContrasenna
            // 
            this.lblContrasenna.AllowDrop = true;
            this.lblContrasenna.AutoSize = true;
            this.lblContrasenna.BackColor = System.Drawing.Color.Transparent;
            this.lblContrasenna.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblContrasenna.Location = new System.Drawing.Point(291, 131);
            this.lblContrasenna.Name = "lblContrasenna";
            this.lblContrasenna.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblContrasenna.Size = new System.Drawing.Size(64, 13);
            this.lblContrasenna.TabIndex = 8;
            this.lblContrasenna.Text = "Contraseña:";
            // 
            // lblDatabase
            // 
            this.lblDatabase.AllowDrop = true;
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.BackColor = System.Drawing.Color.Transparent;
            this.lblDatabase.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDatabase.Location = new System.Drawing.Point(16, 160);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblDatabase.Size = new System.Drawing.Size(210, 13);
            this.lblDatabase.TabIndex = 10;
            this.lblDatabase.Text = "3. Seleccione la base de datos del servidor";
            // 
            // lblSeguridad
            // 
            this.lblSeguridad.AllowDrop = true;
            this.lblSeguridad.AutoSize = true;
            this.lblSeguridad.BackColor = System.Drawing.Color.Transparent;
            this.lblSeguridad.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblSeguridad.Location = new System.Drawing.Point(16, 80);
            this.lblSeguridad.Name = "lblSeguridad";
            this.lblSeguridad.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblSeguridad.Size = new System.Drawing.Size(293, 13);
            this.lblSeguridad.TabIndex = 3;
            this.lblSeguridad.Text = "2. Seleccione el tipo de seguridad de su conexión al servidor";
            // 
            // lblUsuario
            // 
            this.lblUsuario.AllowDrop = true;
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.BackColor = System.Drawing.Color.Transparent;
            this.lblUsuario.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblUsuario.Location = new System.Drawing.Point(309, 106);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblUsuario.TabIndex = 6;
            this.lblUsuario.Text = "Usuario:";
            // 
            // lblServidor
            // 
            this.lblServidor.AllowDrop = true;
            this.lblServidor.AutoSize = true;
            this.lblServidor.BackColor = System.Drawing.Color.Transparent;
            this.lblServidor.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblServidor.Location = new System.Drawing.Point(16, 24);
            this.lblServidor.Name = "lblServidor";
            this.lblServidor.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblServidor.Size = new System.Drawing.Size(226, 13);
            this.lblServidor.TabIndex = 1;
            this.lblServidor.Text = "1. Seleccione o escriba un nombre de servidor";
            // 
            // frParametros
            // 
            this.frParametros.AllowDrop = true;
            this.frParametros.BackColor = System.Drawing.SystemColors.Control;
            this.frParametros.Controls.Add(this.btnReportes);
            this.frParametros.Controls.Add(this.txtCaminoReportes);
            this.frParametros.Controls.Add(this.txtCaminoSalva);
            this.frParametros.Controls.Add(this.chkMulticompannia);
            this.frParametros.Controls.Add(this.btnSalvas);
            this.frParametros.Controls.Add(this.lblCaminoSalva);
            this.frParametros.Controls.Add(this.lblCaminoReportes);
            this.frParametros.ForeColor = System.Drawing.SystemColors.ControlText;
            this.frParametros.Location = new System.Drawing.Point(10, 233);
            this.frParametros.Name = "frParametros";
            this.frParametros.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.frParametros.Size = new System.Drawing.Size(480, 159);
            this.frParametros.TabIndex = 14;
            this.frParametros.TabStop = false;
            this.frParametros.Text = "   Parámetros de configuración del sistema";
            // 
            // btnReportes
            // 
            this.btnReportes.AllowDrop = true;
            this.btnReportes.BackColor = System.Drawing.SystemColors.Control;
            this.btnReportes.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnReportes.Image = global::Configuracion.Properties.Resources.search;
            this.btnReportes.Location = new System.Drawing.Point(362, 113);
            this.btnReportes.Name = "btnReportes";
            this.btnReportes.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnReportes.Size = new System.Drawing.Size(100, 35);
            this.btnReportes.TabIndex = 21;
            this.btnReportes.Text = "&Reportes";
            this.btnReportes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReportes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReportes.UseVisualStyleBackColor = false;
            this.btnReportes.Click += new System.EventHandler(this.btnReportes_Click);
            // 
            // txtCaminoReportes
            // 
            this.txtCaminoReportes.AcceptsReturn = true;
            this.txtCaminoReportes.AllowDrop = true;
            this.txtCaminoReportes.BackColor = System.Drawing.SystemColors.Window;
            this.txtCaminoReportes.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCaminoReportes.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtCaminoReportes.Location = new System.Drawing.Point(31, 121);
            this.txtCaminoReportes.MaxLength = 0;
            this.txtCaminoReportes.Name = "txtCaminoReportes";
            this.txtCaminoReportes.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtCaminoReportes.Size = new System.Drawing.Size(313, 20);
            this.txtCaminoReportes.TabIndex = 19;
            this.txtCaminoReportes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            // 
            // txtCaminoSalva
            // 
            this.txtCaminoSalva.AcceptsReturn = true;
            this.txtCaminoSalva.AllowDrop = true;
            this.txtCaminoSalva.BackColor = System.Drawing.SystemColors.Window;
            this.txtCaminoSalva.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCaminoSalva.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtCaminoSalva.Location = new System.Drawing.Point(32, 73);
            this.txtCaminoSalva.MaxLength = 0;
            this.txtCaminoSalva.Name = "txtCaminoSalva";
            this.txtCaminoSalva.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtCaminoSalva.Size = new System.Drawing.Size(313, 20);
            this.txtCaminoSalva.TabIndex = 17;
            this.txtCaminoSalva.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            // 
            // chkMulticompannia
            // 
            this.chkMulticompannia.AllowDrop = true;
            this.chkMulticompannia.BackColor = System.Drawing.SystemColors.Control;
            this.chkMulticompannia.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkMulticompannia.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkMulticompannia.Location = new System.Drawing.Point(16, 19);
            this.chkMulticompannia.Name = "chkMulticompannia";
            this.chkMulticompannia.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkMulticompannia.Size = new System.Drawing.Size(136, 25);
            this.chkMulticompannia.TabIndex = 15;
            this.chkMulticompannia.Text = "4. ¿Es Multicompañia?";
            this.chkMulticompannia.UseVisualStyleBackColor = false;
            // 
            // btnSalvas
            // 
            this.btnSalvas.AllowDrop = true;
            this.btnSalvas.BackColor = System.Drawing.SystemColors.Control;
            this.btnSalvas.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSalvas.Image = global::Configuracion.Properties.Resources.search;
            this.btnSalvas.Location = new System.Drawing.Point(362, 66);
            this.btnSalvas.Name = "btnSalvas";
            this.btnSalvas.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSalvas.Size = new System.Drawing.Size(100, 35);
            this.btnSalvas.TabIndex = 20;
            this.btnSalvas.Text = "Sal&vas";
            this.btnSalvas.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSalvas.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSalvas.UseVisualStyleBackColor = false;
            this.btnSalvas.Click += new System.EventHandler(this.btnSalvas_Click);
            // 
            // lblCaminoSalva
            // 
            this.lblCaminoSalva.AllowDrop = true;
            this.lblCaminoSalva.AutoSize = true;
            this.lblCaminoSalva.BackColor = System.Drawing.Color.Transparent;
            this.lblCaminoSalva.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCaminoSalva.Location = new System.Drawing.Point(16, 53);
            this.lblCaminoSalva.Name = "lblCaminoSalva";
            this.lblCaminoSalva.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblCaminoSalva.Size = new System.Drawing.Size(220, 13);
            this.lblCaminoSalva.TabIndex = 16;
            this.lblCaminoSalva.Text = "5. Seleccione o escriba el camino de la salva";
            // 
            // lblCaminoReportes
            // 
            this.lblCaminoReportes.AllowDrop = true;
            this.lblCaminoReportes.AutoSize = true;
            this.lblCaminoReportes.BackColor = System.Drawing.Color.Transparent;
            this.lblCaminoReportes.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCaminoReportes.Location = new System.Drawing.Point(16, 101);
            this.lblCaminoReportes.Name = "lblCaminoReportes";
            this.lblCaminoReportes.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblCaminoReportes.Size = new System.Drawing.Size(238, 13);
            this.lblCaminoReportes.TabIndex = 18;
            this.lblCaminoReportes.Text = "6. Seleccione o escriba el camino de los reportes";
            // 
            // btnSalir
            // 
            this.btnSalir.AllowDrop = true;
            this.btnSalir.BackColor = System.Drawing.SystemColors.Control;
            this.btnSalir.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSalir.Image = global::Configuracion.Properties.Resources.exit;
            this.btnSalir.Location = new System.Drawing.Point(296, 398);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSalir.Size = new System.Drawing.Size(100, 35);
            this.btnSalir.TabIndex = 23;
            this.btnSalir.Text = "&Salir";
            this.btnSalir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSalir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.AllowDrop = true;
            this.btnGuardar.BackColor = System.Drawing.SystemColors.Control;
            this.btnGuardar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnGuardar.Image = global::Configuracion.Properties.Resources.save;
            this.btnGuardar.Location = new System.Drawing.Point(104, 398);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnGuardar.Size = new System.Drawing.Size(100, 35);
            this.btnGuardar.TabIndex = 22;
            this.btnGuardar.Text = "&Guardar";
            this.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGuardar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Seleccione el directorio";
            // 
            // ttMensaje
            // 
            this.ttMensaje.IsBalloon = true;
            // 
            // errIconoError
            // 
            this.errIconoError.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errIconoError.ContainerControl = this;
            // 
            // errIconoInfo
            // 
            this.errIconoInfo.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errIconoInfo.ContainerControl = this;
            this.errIconoInfo.Icon = ((System.Drawing.Icon)(resources.GetObject("errIconoInfo.Icon")));
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 442);
            this.Controls.Add(this.frParametros);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.frConectarse);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuración de Sicema SQL";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmPrincipal_FormClosed);
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.frConectarse.ResumeLayout(false);
            this.frConectarse.PerformLayout();
            this.frParametros.ResumeLayout(false);
            this.frParametros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errIconoError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errIconoInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.GroupBox frConectarse;
        private System.Windows.Forms.RadioButton optSeguridad1;
        private System.Windows.Forms.RadioButton optSeguridad0;
        public System.Windows.Forms.Button btnProbar;
        public System.Windows.Forms.TextBox txtContrasenna;
        public System.Windows.Forms.TextBox txtUsuario;
        public System.Windows.Forms.ComboBox cbxCatalogo;
        public System.Windows.Forms.ComboBox cbxServidor;
        public System.Windows.Forms.Button btnActualizar;
        public System.Windows.Forms.Label lblContrasenna;
        public System.Windows.Forms.Label lblDatabase;
        public System.Windows.Forms.Label lblSeguridad;
        public System.Windows.Forms.Label lblUsuario;
        public System.Windows.Forms.Label lblServidor;
        public System.Windows.Forms.Button btnSalir;
        public System.Windows.Forms.Button btnGuardar;
        public System.Windows.Forms.GroupBox frParametros;
        private System.Windows.Forms.Button btnReportes;
        public System.Windows.Forms.TextBox txtCaminoReportes;
        public System.Windows.Forms.TextBox txtCaminoSalva;
        public System.Windows.Forms.CheckBox chkMulticompannia;
        private System.Windows.Forms.Button btnSalvas;
        public System.Windows.Forms.Label lblCaminoSalva;
        public System.Windows.Forms.Label lblCaminoReportes;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolTip ttMensaje;
        public System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.ErrorProvider errIconoError;
        private System.Windows.Forms.ErrorProvider errIconoInfo;
    }
}