using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;
using System.IO;
using System.Security.Cryptography;

namespace Configuracion
{
    using static Sicema.Utiles;

    public partial class frmPrincipal : Form
    {
        private const string ARCHIVO_INI = "\\config.ini";
        private const string SECCION = "SicemaSQL";

        private string strGenKey = "ATKVV2WMJM4R35499X5BZHP53F00HOHG1PKT9CX1XLNM1RWZ1UNU88X3IHZRDMWBXDI6MWDEPO6SBM96NGGSFE59I1QC1FZFEL1ZV0F8AMCAUYDCKH58H9J5ITZREDXB";

        SymmetricAlgorithm SymmetricAlgorithm;

        private string _database = "";

        public frmPrincipal()
        {
            InitializeComponent();

            errIconoInfo.SetIconAlignment(cbxServidor, ErrorIconAlignment.MiddleLeft);
            errIconoInfo.SetIconPadding(cbxServidor, 3);
            errIconoInfo.SetError(cbxServidor, "Campo requerido");
            errIconoInfo.SetIconAlignment(cbxCatalogo, ErrorIconAlignment.MiddleLeft);
            errIconoInfo.SetIconPadding(cbxCatalogo, 3);
            errIconoInfo.SetError(cbxCatalogo, "Campo requerido");
            errIconoInfo.SetIconAlignment(txtCaminoSalva, ErrorIconAlignment.MiddleLeft);
            errIconoInfo.SetIconPadding(txtCaminoSalva, 3);
            errIconoInfo.SetError(txtCaminoSalva, "Campo requerido");
            errIconoInfo.SetIconAlignment(txtCaminoReportes, ErrorIconAlignment.MiddleLeft);
            errIconoInfo.SetIconPadding(txtCaminoReportes, 3);
            errIconoInfo.SetError(txtCaminoReportes, "Campo requerido");

            SymmetricAlgorithm = Crypto.CreateSymmetricAlgorithm(strGenKey);
        }

        private string PadL(string Cadena, int Cantidad, char Caracter = ' ')
        { 
            string tmpCad = Cadena.Trim();
            int tmpLong = tmpCad.Length;
            if (tmpLong >= Cantidad) { return tmpCad; }
            else { return new String(Caracter, Cantidad - tmpLong) + tmpCad;}
        }

        private void LimpiarConfiguracion()
        {
            cbxServidor.DataSource = null;
            cbxCatalogo.DataSource = null;
            cbxServidor.Text = "";
            cbxCatalogo.Text = "";

            optSeguridad0.Checked = true;
            txtUsuario.Clear();
            txtContrasenna.Clear();
            txtUsuario.ReadOnly = true;
            txtUsuario.BackColor = SystemColors.InactiveBorder;
            txtContrasenna.ReadOnly = true;
            txtContrasenna.BackColor = SystemColors.InactiveBorder;
        }

        private void CargarConfiguracion()
        {
            string _servidor;
            string _catalogo;
            bool _seguridad_integrada;
            string _usuario;
            string _contrasenna;
            string _salvas;
            string _reportes;
            bool _multicomp;

            //AppSettingsReader Reader = new AppSettingsReader();
            //string _servidor = Reader.GetValue("server", typeof(string)).ToString();
            //string _database = Reader.GetValue("database", typeof(string)).ToString();
            //bool _seguridad_integrada = Convert.ToBoolean(Reader.GetValue("security", typeof(bool)).ToString());
            //string _usuario = Reader.GetValue("user", typeof(string)).ToString();
            //string _contrasenna = Reader.GetValue("password", typeof(string)).ToString();

            // Get the configuration file.
            System.Configuration.Configuration config =
              ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // Get the appSettings section.
            System.Configuration.AppSettingsSection appSettings =
            (System.Configuration.AppSettingsSection)config.GetSection("appSettings");

            if (appSettings.Settings.Count == 0)
            {
                _servidor = "(local)";
                _catalogo = "";
                _seguridad_integrada = true;
                _usuario = "";
                _contrasenna = "";
                this._database = "";
                _salvas = "";
                _reportes = "";
                _multicomp = false;
            }
            else
            {
                _servidor = config.AppSettings.Settings["Servidor"].Value;
                _catalogo = config.AppSettings.Settings["Catalogo"].Value;
                _seguridad_integrada = Convert.ToBoolean(config.AppSettings.Settings["Seguridad"].Value);
                if (_seguridad_integrada)
                {
                    _usuario = "";
                    _contrasenna = "";
                }
                else
                {
                    _usuario = config.AppSettings.Settings["Usuario"].Value;
                    _contrasenna = config.AppSettings.Settings["Contrasenna"].Value;
                }
                this._database = config.AppSettings.Settings["Database"].Value;
                _salvas = config.AppSettings.Settings["Salva"].Value;
                _reportes = config.AppSettings.Settings["Reportes"].Value;
                _multicomp = Convert.ToBoolean(config.AppSettings.Settings["Multicompañia"].Value);
            }

            cbxServidor.DataSource = null;
            cbxCatalogo.DataSource = null;
            cbxServidor.Text = _servidor;
            cbxCatalogo.Text = _catalogo;

            if (_seguridad_integrada)
            {
                optSeguridad0.Checked = true;
                txtUsuario.Clear();
                txtContrasenna.Clear();
                txtUsuario.ReadOnly = true;
                txtUsuario.BackColor = SystemColors.InactiveBorder;
                txtContrasenna.ReadOnly = true;
                txtContrasenna.BackColor = SystemColors.InactiveBorder;
            }
            else
            {
                optSeguridad1.Checked = true;
                txtUsuario.Text = _usuario;
                txtContrasenna.Text = _contrasenna;
                txtUsuario.ReadOnly = false;
                txtUsuario.BackColor = SystemColors.Window;
                txtContrasenna.ReadOnly = false;
                txtContrasenna.BackColor = SystemColors.Window;
            }
            chkMulticompannia.Checked = _multicomp;
            txtCaminoSalva.Text = _salvas;
            txtCaminoReportes.Text = _reportes;
        }

        private void GuardarConfiguracion()
        {
            try
            {
                // Get the configuration file.
                System.Configuration.Configuration config =
                  ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // Get the appSettings section.
                System.Configuration.AppSettingsSection appSettings =
                (System.Configuration.AppSettingsSection)config.GetSection("appSettings");

                if (appSettings.Settings.Count == 0)
                {
                    // Add Application settings.
                    config.AppSettings.Settings.Add("Servidor", cbxServidor.Text);
                    config.AppSettings.Settings.Add("Catalogo", cbxCatalogo.Text);
                    if (optSeguridad0.Checked)
                    {
                        config.AppSettings.Settings.Add("Seguridad", "true");
                        config.AppSettings.Settings.Add("Usuario", "");
                        config.AppSettings.Settings.Add("Contrasenna", "");
                    }
                    else
                    {
                        config.AppSettings.Settings.Add("Seguridad", "false");
                        config.AppSettings.Settings.Add("Usuario", txtUsuario.Text);
                        config.AppSettings.Settings.Add("Contrasenna", txtContrasenna.Text);
                    }
                    config.AppSettings.Settings.Add("Database", this._database);
                    config.AppSettings.Settings.Add("Salva", txtCaminoSalva.Text);
                    config.AppSettings.Settings.Add("Reportes", txtCaminoReportes.Text);
                    if (chkMulticompannia.Checked) { config.AppSettings.Settings.Add("Multicompañia", "true"); }
                    else { config.AppSettings.Settings.Add("Multicompañia", "false"); }

                    // Save the configuration file.
                    config.Save(ConfigurationSaveMode.Full);
                }
                else
                {
                    config.AppSettings.Settings["Servidor"].Value = cbxServidor.Text;
                    config.AppSettings.Settings["Catalogo"].Value = cbxCatalogo.Text;
                    if (optSeguridad0.Checked)
                    {
                        config.AppSettings.Settings["Seguridad"].Value = "true";
                        config.AppSettings.Settings["Usuario"].Value = "";
                        config.AppSettings.Settings["Contrasenna"].Value = "";
                    }
                    else
                    {
                        config.AppSettings.Settings["Seguridad"].Value = "false";
                        config.AppSettings.Settings["Usuario"].Value = txtUsuario.Text;
                        config.AppSettings.Settings["Contrasenna"].Value = txtContrasenna.Text;
                    }
                    config.AppSettings.Settings["Database"].Value = this._database;
                    config.AppSettings.Settings["Salva"].Value = txtCaminoSalva.Text;
                    config.AppSettings.Settings["Reportes"].Value = txtCaminoReportes.Text;
                    if (chkMulticompannia.Checked) { config.AppSettings.Settings["Multicompañia"].Value = "true"; }
                    else { config.AppSettings.Settings["Multicompañia"].Value = "false"; }

                    // Save the configuration file.
                    config.Save(ConfigurationSaveMode.Modified);
                }

                // Force a reload of a changed section.
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (ConfigurationErrorsException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
        }

        private void CargarParamInicio()
        {
            try
            {
                //strCamino = Path.GetDirectoryName(Application.ExecutablePath) + ARCHIVO_INI;
                string strCamino = Application.StartupPath + ARCHIVO_INI;
                if (File.Exists(strCamino))
                {
                    clsIniFile ini = new clsIniFile(strCamino);
                    string _servidor = ini.IniReadValue(SECCION, "Servidor");
                    string _catalogo = ini.IniReadValue(SECCION, "Catalogo");
                    bool _seguridad = (ini.IniReadValue(SECCION, "Seguridad") == "1") ? true : false;
                    string _usuario = ini.IniReadValue(SECCION, "Usuario");
                    string _contrasenna = ini.IniReadValue(SECCION, "Contrasenna");
                    if (SymmetricAlgorithm == null) { _contrasenna = ""; }
                    else { _contrasenna = Crypto.DecryptText(SymmetricAlgorithm, _contrasenna); }
                    this._database = ini.IniReadValue(SECCION, "Database");
                    string _salva = ini.IniReadValue(SECCION, "Salva");
                    string _reportes = ini.IniReadValue(SECCION, "Reportes");
                    bool _multicomp = (ini.IniReadValue(SECCION, "Multicompañia") == "1") ? true : false;

                    cbxServidor.DataSource = null;
                    cbxCatalogo.DataSource = null;
                    cbxServidor.Text = _servidor;
                    cbxCatalogo.Text = _catalogo;
                    if (!_seguridad)
                    {
                        optSeguridad0.Checked = true;
                        txtUsuario.Clear();
                        txtContrasenna.Clear();
                        txtUsuario.ReadOnly = true;
                        txtUsuario.BackColor = SystemColors.InactiveBorder;
                        txtContrasenna.ReadOnly = true;
                        txtContrasenna.BackColor = SystemColors.InactiveBorder;
                    }
                    else
                    {
                        optSeguridad1.Checked = true;
                        txtUsuario.Text = _usuario;
                        txtContrasenna.Text = _contrasenna;
                        txtUsuario.ReadOnly = false;
                        txtUsuario.BackColor = SystemColors.Window;
                        txtContrasenna.ReadOnly = false;
                        txtContrasenna.BackColor = SystemColors.Window;
                    }
                    txtCaminoSalva.Text = _salva;
                    txtCaminoReportes.Text = _reportes;
                    chkMulticompannia.Checked = _multicomp;
                }
                else
                {
                    LimpiarConfiguracion();
                }
            }
            catch (Exception ex) { throw ex; }
        }

        private void GuardarParamInicio()
        {
            try
            {
                string strCamino = Application.StartupPath + ARCHIVO_INI;

                clsIniFile ini = new clsIniFile(strCamino);
                ini.IniWriteValue(SECCION, "Servidor", cbxServidor.Text);
                ini.IniWriteValue(SECCION, "Catalogo", cbxCatalogo.Text);
                if (optSeguridad0.Checked) { ini.IniWriteValue(SECCION, "Seguridad", "0"); }
                else { ini.IniWriteValue(SECCION, "Seguridad", "1"); }
                ini.IniWriteValue(SECCION, "Usuario", txtUsuario.Text);
                string _contrasenna = txtContrasenna.Text;
                if (SymmetricAlgorithm != null) { _contrasenna = Crypto.EncryptText(SymmetricAlgorithm, txtContrasenna.Text); }
                ini.IniWriteValue(SECCION, "Contrasenna", _contrasenna);
                ini.IniWriteValue(SECCION, "Database", this._database);
                ini.IniWriteValue(SECCION, "Salva", txtCaminoSalva.Text);
                ini.IniWriteValue(SECCION, "Reportes", txtCaminoReportes.Text);
                if (chkMulticompannia.Checked) { ini.IniWriteValue(SECCION, "Multicompañia", "1"); }
                else { ini.IniWriteValue(SECCION, "Multicompañia", "0"); }
            }
            catch (Exception ex) { throw ex; }
        }
        
        private void ObtenerServidores()
        {
            this.Cursor = Cursors.WaitCursor;
            cbxServidor.DataSource = null;
            try
            {
                //string[] arrServidores = clsSqlLocator.GetServers();
                string[] arrServidores = clsSqlLocator.GetSqlServers();
                cbxServidor.DataSource = arrServidores;
            }
            catch (ApplicationException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { this.Cursor = Cursors.Default; }
        }

        private void ObtenerBases() 
        {
            List<string> listaBases = new List<string>();

            this.Cursor = Cursors.WaitCursor;
            cbxCatalogo.DataSource = null;
            try
            {
                if (optSeguridad0.Checked) { listaBases = clsSqlLocator.GetDatabases(cbxServidor.Text); }
                else { listaBases = clsSqlLocator.GetDatabases(cbxServidor.Text, false, txtUsuario.Text, txtContrasenna.Text); }
                cbxCatalogo.DataSource = listaBases;
            }
            catch (ApplicationException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { this.Cursor = Cursors.Default; }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                ObtenerServidores();
                cbxServidor.Focus();
                SendKeys.Send("{F4}");
            }
            catch (ApplicationException ex)
            { MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            catch (Exception)
            { MessageBox.Show("No se ha podido recuperar la lista de servidores.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Desea cerrar la aplicación sin guardar los cambios?", "Cerrar aplicación", MessageBoxButtons.YesNo,
                               MessageBoxIcon.Question);
            if (result == DialogResult.Yes) { Application.Exit(); }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string msgError = ChequearDatos();
            if (msgError.Length != 0)
            {
                MessageBox.Show(msgError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                try
                {
                    GuardarParamInicio();
                    Application.Exit();
                }
                catch (Exception)
                { MessageBox.Show("No se ha podido guardar los parámetros definidos.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void optSeguridad0_CheckedChanged(object sender, EventArgs e)
        {
            if (optSeguridad0.Checked)
            {
                txtUsuario.Text = "";
                txtUsuario.ReadOnly = true;
                txtUsuario.BackColor = SystemColors.InactiveBorder;
                txtContrasenna.Text = "";
                txtContrasenna.ReadOnly = true;
                txtContrasenna.BackColor = SystemColors.InactiveBorder;
            }
            else
            {
                txtUsuario.ReadOnly = false;
                txtUsuario.BackColor = SystemColors.Window;
                txtContrasenna.ReadOnly = false;
                txtContrasenna.BackColor = SystemColors.Window;
                txtUsuario.Focus();
            }
        }

        private string ChequearDatos(bool bTodos = true)
        {
            string msgError = String.Empty;
            errIconoError.Clear();

            if (cbxServidor.Text.Length == 0)
            {
                errIconoError.SetError(cbxServidor, "Ingrese un valor");
                msgError = "Faltan algunos datos por ingresar, serán remarcados";
            }

            if (cbxCatalogo.Text.Length == 0)
            {
                errIconoError.SetError(cbxCatalogo, "Ingrese un valor");
                msgError = "Faltan algunos datos por ingresar, serán remarcados";
            }

            if ((optSeguridad1.Checked) && (txtUsuario.Text.Length == 0))
            {
                errIconoError.SetError(txtUsuario, "Ingrese un valor");
                msgError = "Faltan algunos datos por ingresar, serán remarcados";
            }

            if ((optSeguridad1.Checked) && (txtContrasenna.Text.Length == 0))
            {
                errIconoError.SetError(txtContrasenna, "Ingrese un valor");
                msgError = "Faltan algunos datos por ingresar, serán remarcados";
            }

            if (bTodos)
            {
                if (txtCaminoSalva.Text.Length == 0)
                {
                    errIconoError.SetError(txtCaminoSalva, "Ingrese un valor");
                    msgError = "Faltan algunos datos por ingresar, serán remarcados";
                }

                if (txtCaminoReportes.Text.Length == 0)
                {
                    errIconoError.SetError(txtCaminoReportes, "Ingrese un valor");
                    msgError = "Faltan algunos datos por ingresar, serán remarcados";
                }
            }

            return msgError;
        }

        private void btnProbar_Click(object sender, EventArgs e)
        {
            string msgError = ChequearDatos(false);
            if (msgError.Length != 0)
	        {
                MessageBox.Show(msgError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
	        }
            else
            {
                this.Cursor = Cursors.WaitCursor;

                // -- Conectar con la BD SQL Server --
                try
                {
                    if (clsSqlLocator.ConnectSQL(cbxServidor.Text, cbxCatalogo.Text, optSeguridad0.Checked, txtUsuario.Text, txtContrasenna.Text))
                    {
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("La conexión con la base de datos ha sido satisfactoria.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else { this.Cursor = Cursors.Default; }
                }
                catch (ApplicationException ex)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                catch (Exception)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("No se ha podido realizar la conexión con la de bases de datos.\n" + "Compruebe los parámetros de la conexión.",
                          Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSalvas_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Seleccione el camino de la salva";
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) { txtCaminoSalva.Text = folderBrowserDialog1.SelectedPath + @"\"; }
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Seleccione el camino de los reportes";
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) { txtCaminoReportes.Text = folderBrowserDialog1.SelectedPath + @"\"; }
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (processes.Length > 1 && Process.GetCurrentProcess().StartTime != processes[0].StartTime)
            {
                MessageBox.Show("Esta aplicacion solo puede estar cargada una sola vez.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            else
            {
                this.Text = String.Format(this.Text + "{0}v{1}.{2} Rev. {3}", new String(' ', 3), FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileMajorPart.ToString(), 
                    FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileMinorPart.ToString(), PadL(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileBuildPart.ToString(), 3, '0'));
                lblVersion.Text = "ver. " + Application.ProductVersion;

                try
                {
                    CargarParamInicio();

                    ttMensaje.SetToolTip(btnActualizar, "Actualizar lista de servidores SQL");
                    ttMensaje.SetToolTip(txtUsuario, "Login del usuario con acceso a la base de datos");
                    ttMensaje.SetToolTip(txtContrasenna, "Contraseña del usuario con acceso a la base de datos");
                    ttMensaje.SetToolTip(btnProbar, "Probar conexión a la base de datos");
                    ttMensaje.SetToolTip(chkMulticompannia, "¿El sistema utiliza multicompañia?");
                    ttMensaje.SetToolTip(btnSalvas, "Seleccionar el camino de las salvas del sistema");
                    ttMensaje.SetToolTip(btnReportes, "Seleccionar el camino de los reportes del sistema");
                    ttMensaje.SetToolTip(btnGuardar, "Guardar la configuración establecida");
                    ttMensaje.SetToolTip(btnSalir, "Salir sin guardar la configuración");
                }
                catch (Exception ex) 
                { MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void frmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            SymmetricAlgorithm?.Dispose();
        }

        private void cbxCatalogo_DropDown(object sender, EventArgs e)
        {
            try { ObtenerBases(); }
            catch (ApplicationException ex)
            { MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            catch (Exception)
            { MessageBox.Show("No se ha podido recuperar la lista de bases de datos.\n" + "Compruebe los parámetros de la conexión.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            Control ctrl = (Control)sender;
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Tab))
            {
                ctrl.Parent.SelectNextControl(ctrl, true, true, true, true);
                e.Handled = true;
            }
        }

        private void Control_Validating(object sender, CancelEventArgs e)
        {
            Control ctrl = (Control)sender;
            if ((ctrl.Name == "txtUsuario") || (ctrl.Name == "txtContrasenna"))
            {
                if (!optSeguridad0.Checked)
                {
                    if (ctrl.Text.Length == 0) { errIconoError.SetError(ctrl, "Ingrese un valor"); }
                    else { errIconoError.SetError(ctrl, ""); }
                }
                else { errIconoError.SetError(ctrl, ""); }
            }
            else
            {
                if (ctrl.Text.Length == 0) { errIconoError.SetError(ctrl, "Ingrese un valor"); }
                else { errIconoError.SetError(ctrl, ""); }
            }
        }
    }
}
