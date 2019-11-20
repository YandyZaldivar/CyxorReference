using System;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;

using PdfFileWriter;

using OfficeOpenXml;

namespace Halo
{
    using Models;

    using Cyxor.Networking;
    using Cyxor.Serialization;
    using Cyxor.Networking.Config;

    //using Utilities = Cyxor.Networking.Utilities;

    public partial class MainForm : Form
    {
        public enum PlanillaMode
        {
            Edicion,
            Busqueda,
        }

        public static string VersionString = Assembly.GetExecutingAssembly().GetName().Version.ToString(fieldCount: 3);

        //public static MainForm Instance { get; private set; }

        public FondoForm FondoForm { get; set; }

        ListView ListView;

        bool Loading;
        bool Minimized;
        int LastColumn;
        bool Initialized;
        SortOrder SortOrder;
        int PlanillaTopOffset;

        public bool PlanillaScrolling { get; private set; }

        const string DataFileName = "HaloDB";
        const string AyudaFileName = "Manual de usuario.pdf";

        ToolStripMenuItem CheckedToolStripMenuItem;
        ToolStripMenuItem[] PropiedadtoolStripMenuItems;

        ListViewItem SelectedItem { get; set; }

        Operation Operation { get; set; }

        Color LeftColor { get; set; }

        TabPage[] TabPages { get; set; }

        //Utilities.Threading.InterlockedInt ItemCounter = new Utilities.Threading.InterlockedInt();

        bool editMode;
        bool EditMode
        {
            get => editMode;
            set
            {
                if (Initialized && editMode == value)
                    return;

                PlanillaMenuEnable(!(editMode = value));

                if (editMode)
                    PlanillaControl.Top = ImagenPanel.Top;
                else
                    PictureBox.Image = PlanillaBitmap;

                if (PlanillaControl.Visible = editMode)
                    PlanillaControl.Focus();

                PlanillaProgressBarPanel.BackColor = editMode ? Color.Green : Color.Red;
            }
        }

        void PlanillaMenuEnable(bool value)
        {
            ImprimirToolStripSplitButton.Enabled = value;
            PaginaToolStripMenuItem.Enabled = value;
            GuardarToolStripButton.Enabled = value;
            CopiarToolStripButton.Enabled = value;
        }

        Bitmap PlanillaBitmap
        {
            get
            {
                PlanillaControl.Top = ImagenPanel.Top;

                PlanillaControl.Visible = true;
                var bitmap = new Bitmap(PlanillaControl.Width, PlanillaControl.Height);
                PlanillaControl.DrawToBitmap(bitmap, new Rectangle(0, 0, PlanillaControl.Width, PlanillaControl.Height));
                PlanillaControl.Visible = EditMode;

                return bitmap;
            }
        }

        bool PlanillaImagenMouseMoving;
        int PlanillaImagenMouseY;

        public MainForm()
        {
            InitializeComponent();

            //Instance = this;

            Operation = Operation.Paciente;
            NombrePacienteToolStripStatusLabel.Text = default;

            ListView = ListViewPaciente;

            PropiedadtoolStripMenuItems = new ToolStripMenuItem[5];
            PropiedadtoolStripMenuItems[0] = IdentificadorToolStripMenuItem;
            PropiedadtoolStripMenuItems[1] = HospitalToolStripMenuItem;
            PropiedadtoolStripMenuItems[2] = ProvinciaToolStripMenuItem;
            PropiedadtoolStripMenuItems[3] = HospitalProvinciaToolStripMenuItem;
            PropiedadtoolStripMenuItems[4] = EstadiaHospitalariaToolStripMenuItem;

            //FondoForm = new FondoForm() { MainForm = this };

            //var server = new Server();
            //server.Config.EventDispatching = Cyxor.Networking.Config.EventDispatching.Synchronized;

            PictureBox.MouseWheel += (s, e) =>
                PlanillaScroll(VScrollBar.SmallChange * (e.Delta > 0 ? 1 : -1));

            PictureBox.MouseDown += (s, e) =>
            {
                PlanillaImagenMouseY = e.Y;
                PlanillaImagenMouseMoving = true;
                PictureBox.Cursor = Cursors.NoMoveVert;
            };

            PictureBox.MouseUp += (s, e) =>
            {
                PlanillaImagenMouseMoving = false;
                PictureBox.Cursor = Cursors.Default;
            };

            PictureBox.MouseMove += (s, e) =>
            {
                if (PlanillaImagenMouseMoving)
                    PlanillaScroll(e.Y - PlanillaImagenMouseY);
            };

            void PlanillaScroll(int value)
            {
                var delta = Math.Abs(value);

                if (value > 0)
                    VScrollBar.Value = VScrollBar.Value - delta < VScrollBar.Minimum ?
                    VScrollBar.Minimum : VScrollBar.Value - delta;
                else
                    VScrollBar.Value = VScrollBar.Value + delta > VScrollBar.Maximum - VScrollBar.LargeChange ?
                    VScrollBar.Maximum - VScrollBar.LargeChange + 1 : VScrollBar.Value + delta;
            }

            PlanillaTopOffset = PlanillaControl.Location.Y;

            var visibleRegionSize = splitContainer1.Panel2.Height - PlanillaTopOffset;

            VScrollBar.LargeChange = visibleRegionSize / 10;
            VScrollBar.SmallChange = visibleRegionSize / 20;

            VScrollBar.Maximum = PlanillaControl.Height - visibleRegionSize + VScrollBar.LargeChange;

            PropertyGrid.SelectedObject = Network.Instance.Config;

            //var dt = await Server.Instance.ConnectAsync();

            //if (await Network.Instance.ConnectAsync())
            //{
            //    PropertyGrid.SelectedObject = null;
            //    PropertyGrid.Refresh();
            //    await Task.Delay(2000);
            //    PropertyGrid.SelectedObject = Network.Instance.Config;
            //}

            Network.Instance.Config.Proxy.Enabled = true;

            Network.Instance.Config.PreferIPv4Addresses = true;
            Network.Instance.Config.Proxy.PreAuthenticate = true;
            Network.Instance.Config.AuthenticationMode = AuthenticationSchema.Basic;

            //Network.Instance.Config.Proxy.Port = 808;
            //Network.Instance.Config.Proxy.Address = "localhost";
            //Network.Instance.Config.Proxy.UserName = "pepe";
            //Network.Instance.Config.Proxy.Password = "pépe";
            Network.Instance.Config.Proxy.Port = 3128;
            Network.Instance.Config.Proxy.Address = "10.10.2.89";
            Network.Instance.Config.Proxy.UserName = "yandy@alimatic.alinet.cu";
            Network.Instance.Config.Proxy.Password = "yandy16*";

            //Network.Instance.Config.Address = "google.com";
            //Network.Instance.Config.Port = 80;

            //var dt = await Network.Instance.ConnectAsync();

            //while (true)
            //{
            //    using (var packet = new Packet(Network.Instance) { Message = "Papa has algo" })
            //        await packet.SendAsync();

            //    await Task.Delay(10000);
            //}

            TabPages = new TabPage[TabControl.TabPages.Count];

            var i = 0;
            foreach (var control in TabControl.TabPages)
            {
                var tabPage = control as TabPage;

                TabPages[i++] = tabPage;

                //if (tabPage.Name != nameof(ConexionTabPage) && tabPage.Name != nameof(PacientesTabPage))
                if (tabPage.Name != nameof(PacientesTabPage) && tabPage.Name != nameof(BusquedaTabPage))
                //if (tabPage.Name != nameof(PacientesTabPage))
                    TabControl.TabPages.Remove(tabPage);
            }

            PrintDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Halo", 850, 1100);

            //var random = new Random();

            //for (var j = 0; j < 100; j++)
            //{
            //    var item = ListView.Items.Add("");
            //    item.UseItemStyleForSubItems = false;
            //    //var rr1 = item.SubItems.Add(886689.ToString());
            //    var rr1 = item.SubItems.Add("DDDDDD");
            //    rr1.Font = new Font(rr1.Font, FontStyle.Regular);
            //    //var rr2 = item.SubItems.Add(DateTime.Now.ToShortDateString());
            //    var rr2 = item.SubItems.Add(DateTime.Now.ToString("dd/MM/yy"));
            //    rr2.Font = new Font(rr2.Font, FontStyle.Regular);
            //    //item.SubItems.Add(j.ToString().GetHashCode().ToString());
            //    var rr3 = item.SubItems.Add(j.ToString());
            //    rr3.Font = new Font(rr3.Font, FontStyle.Regular);
            //    var rr4 = item.SubItems.Add("Julia de la Caridad Fernández Martínez del Valle");
            //    rr4.Font = new Font(rr4.Font, FontStyle.Regular);
            //}

            //LeftColor = Color.FromArgb(196, 196, 224);
            //LeftColor = Color.FromKnownColor(KnownColor.Lavender);
            LeftColor = Color.FromKnownColor(KnownColor.White);

            ConexionPanel.BackColor = LeftColor;

            DireccionServidorTextBox.Text = Network.Instance.Config.Address;
            PuertoServidorTextBox.Text = Network.Instance.Config.Port.ToString();

            SortOrder = SortOrder.Descending;
            LastColumn = FechaColumnHeader.Index;
            ListView.ListViewItemSorter = new ListViewItemComparer(SortOrder, LastColumn, CheckedToolStripMenuItem);

            IdentificadorToolStripMenuItem.PerformClick();

            LoadData();
        }

        void MainForm_Shown(object sender, EventArgs e)
        {
            EditMode = false;
            Initialized = true;
        }

        void MainForm_Activated(object sender, EventArgs e)
        {
            if (Minimized)
            {
                Minimized = false;
                FondoForm.Show();
            }
        }

        void MainForm_Deactivate(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Minimized = true;
                FondoForm.Hide();
            }
        }

        private void VScrollBar_Scroll(object sender, ScrollEventArgs e)
            => PlanillaScroll(e);

        private void VScrollBar_ValueChanged(object sender, EventArgs e)
            => PlanillaScroll();

        void PlanillaScroll(ScrollEventArgs e = null)
        {
            // TODO: Set the active panel
            if (e?.Type == ScrollEventType.ThumbTrack && !PlanillaScrolling && EditMode)
            {
                PictureBox.Image = PlanillaBitmap;
                PlanillaControl.Visible = false;
                PlanillaScrolling = true;
            }

            ImagenPanel.Top = PlanillaTopOffset - VScrollBar.Value;

            if (e?.Type == ScrollEventType.EndScroll && EditMode)
            {
                PlanillaControl.Top = ImagenPanel.Top;
                PlanillaControl.Visible = true;
                PlanillaControl.SelectFocusedControl();
                PlanillaScrolling = false;
            }

            //PlanillaControl.Top = PlanillaTopOffset - VScrollBar.Value;
        }

        #region Printing

        void ImprimirToolStripButton_Click(object sender, EventArgs e)
        {
            if (PrintDialog.ShowDialog() == DialogResult.OK)
                PrintDialog.Document.Print();
        }

        void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
            => e.Graphics.DrawImage(PlanillaBitmap, e.MarginBounds);

        void PaginaToolStripButton_Click(object sender, EventArgs e)
            => PageSetupDialog.ShowDialog();

        public int LastSaveFileDialogFilterIndex { get; set; } = 1;

        void AbrirToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog.InitialDirectory = $"{Directory.GetCurrentDirectory()}\\HC";

                if (!Directory.Exists(OpenFileDialog.InitialDirectory))
                    Directory.CreateDirectory(OpenFileDialog.InitialDirectory);

                if (OpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var fileName = OpenFileDialog.FileName;

                    var bytes = File.ReadAllBytes(fileName);
                    var paciente = Serializer.XDeserialize<PacienteApiModel>(bytes);

                    //if (paciente.Version != VersionString)
                    //{
                    //    MessageBox.Show("El archivo de historia clínica fue creado con una versión diferente de Halo.", "Halo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    return;
                    //}

                    var messageHeader = $"La historia clínica [{paciente.HistoriaClinica}: {paciente.Nombre}] ";

                    if (!EditMode)
                    {
                        foreach (ListViewItem item in ListView.Items)
                            if ((item.Tag as PacienteApiModel).Id == paciente.Id)
                            {
                                MessageBox.Show(messageHeader + "ya existe en la base de datos, " +
                                    "modifique los datos para actualizar.", "Halo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                        MessageBox.Show(messageHeader + "no existe en la base de datos, " +
                            "inserte una nueva para actualizar.", "Halo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (string.IsNullOrEmpty(PlanillaControl.Paciente.Id))
                    {
                        foreach (ListViewItem item in ListView.Items)
                            if ((item.Tag as PacienteApiModel).Id == paciente.Id)
                            {
                                MessageBox.Show(messageHeader + "que intenta insertar ya se encuentra en la base de datos.", "Halo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                        PlanillaControl.Paciente = paciente;
                    }
                    else if (PlanillaControl.Paciente.Id == paciente.Id)
                        PlanillaControl.Paciente = paciente;
                    else
                    {
                        MessageBox.Show(messageHeader + "que intenta abrir no coincide con la actual " +
                            $"[{PlanillaControl.Paciente.HistoriaClinica}: {PlanillaControl.Paciente.Nombre}].", "Halo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("El archivo de historia clínica está dañado o fue creado con una versión diferente de Halo. " +
                    $"La versión actual es {VersionString}", "Halo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void GuardarToolStripButton_Click(object sender, EventArgs e)
        {
            var filter = "PDF (*.pdf)|*.pdf|PNG (*.png)|*.png|All files (*.*)|*.*";

            var paciente = SelectedItem?.Tag as PacienteApiModel;

            if (paciente != null)
                filter = filter.Insert(startIndex: 0, "Halo (*.halo)|*.halo|");

            SaveFileDialog.Filter = filter;
            //SaveFileDialog.FilterIndex = LastSaveFileDialogFilterIndex;
            SaveFileDialog.FilterIndex = 0;
            SaveFileDialog.FileName = paciente != null ? $"{paciente.Nombre}" : "Planilla";

            SaveFileDialog.InitialDirectory = $"{Directory.GetCurrentDirectory()}\\HC";

            if (!Directory.Exists(SaveFileDialog.InitialDirectory))
                Directory.CreateDirectory(SaveFileDialog.InitialDirectory);

            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileName = SaveFileDialog.FileName;
                LastSaveFileDialogFilterIndex = SaveFileDialog.FilterIndex;

                switch (LastSaveFileDialogFilterIndex)
                {
                    case 1:
                    {
                        //if (paciente == null)
                        //{
                        //    MessageBox.Show("Para guardar en formato *.halo debe seleccionar una paciente",
                        //        "Halo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //    return;
                        //}

                        var bytes = Serializer.XSerialize(paciente).ToByteArray();
                        File.WriteAllBytes(fileName, bytes);

                        break;
                    }
                    case 2:
                    {
                        var pdfDocument = new PdfDocument(PaperType.Letter, false, UnitOfMeasure.Inch, fileName);

                        var pdfImageControl = new PdfImageControl
                        {
                            Resolution = 96.0,
                            ImageQuality = 100,
                            SaveAs = SaveImageAs.GrayImage,
                        };

                        var pdfPage = new PdfPage(pdfDocument);
                        var pdfContents = new PdfContents(pdfPage);
                        var pdfImage = new PdfImage(pdfDocument, PlanillaBitmap, pdfImageControl);
                        pdfContents.DrawImage(pdfImage, 1.0, 1.0, 6.5, 9.0);
                        pdfDocument.CreateFile();

                        break;
                    }

                    case 3: PlanillaBitmap.Save(fileName, ImageFormat.Png); break;
                }
            }
        }

        private void CopiarToolStripButton_Click(object sender, EventArgs e)
            => Clipboard.SetImage(PlanillaBitmap);

        #endregion Printing

        #region RadioButtonAdjust

        void ProcessRadioButtonCheckedChanged(RadioButton radioButton, ref RadioButton lastCheckedRadioButton, ref RadioButton lastClickedRadioButton)
        {
            if (!radioButton.Checked)
                return;

            lastClickedRadioButton = null;
            lastCheckedRadioButton = radioButton;
        }

        void ProcessRadioButtonClicked(RadioButton radioButton, RadioButton lastCheckedRadioButton, ref RadioButton lastClickedRadioButton)
        {
            if (lastClickedRadioButton == null)
                lastClickedRadioButton = radioButton;
            else if (lastClickedRadioButton == lastCheckedRadioButton)
                lastCheckedRadioButton.Checked = false;
        }

        RadioButton LastClickedAreaRadioButton;
        RadioButton LastCheckedAreaRadioButton;

        void AreaRadioButton_CheckedChanged(object sender, EventArgs e) =>
            ProcessRadioButtonCheckedChanged(sender as RadioButton, ref LastCheckedAreaRadioButton, ref LastClickedAreaRadioButton);

        void AreaRadioButton_Click(object sender, EventArgs e) =>
            ProcessRadioButtonClicked(sender as RadioButton, LastCheckedAreaRadioButton, ref LastClickedAreaRadioButton);

        RadioButton LastClickedEscolaridadRadioButton;
        RadioButton LastCheckedEscolaridadRadioButton;

        void EscolaridadRadioButton_CheckedChanged(object sender, EventArgs e) =>
            ProcessRadioButtonCheckedChanged(sender as RadioButton, ref LastCheckedEscolaridadRadioButton, ref LastClickedEscolaridadRadioButton);

        void EscolaridadRadioButton_Click(object sender, EventArgs e)
            => ProcessRadioButtonClicked(sender as RadioButton, LastCheckedEscolaridadRadioButton, ref LastClickedEscolaridadRadioButton);

        #endregion RadioButtonAdjust

        #region Pacientes Tab

        void SwitchPacienteButtonsAvailability(bool enable)
        {
            InsertarPacienteToolStripButton.Enabled = enable;
            var paciente = SelectedItem?.Tag as PacienteApiModel;

            ModificarPacienteToolStripButton.Enabled = SelectedItem != null ? enable && !paciente.SetAsDeleted : false;
            EliminarPacienteToolStripButton.Enabled = SelectedItem != null ? enable : false;
        }

        void SwitchPacienteTabsVisibility(Control control)
        {
            InsertarGestionarPaciente.Visible = false;
            ModificarGestionarPaciente.Visible = false;
            EliminarGestionarPaciente.Visible = false;
            ListView.Visible = false;

            if (control is GestionarPacienteControl gestionarPacienteControl)
                gestionarPacienteControl.Error = null;

            switch (control)
            {
                case ListView listView:
                SwitchPacienteButtonsAvailability(enable: true);
                break;

                case GestionarPacienteControl gp when (gp == InsertarGestionarPaciente):
                SwitchPacienteButtonsAvailability(enable: false);
                break;

                case GestionarPacienteControl gp when (gp == ModificarGestionarPaciente):
                SwitchPacienteButtonsAvailability(enable: false);
                break;

                case GestionarPacienteControl gp when (gp == EliminarGestionarPaciente):
                SwitchPacienteButtonsAvailability(enable: false);
                break;
            }

            control.BackColor = LeftColor;
            control.Visible = true;
        }

        void GestionarPacienteToolStripButton_Click(object sender, EventArgs e)
        {
            switch (sender)
            {
                case ToolStripButton button when (button == InsertarPacienteToolStripButton):
                {
                    NombrePacienteToolStripStatusLabel.Text = default;
                    SwitchPacienteTabsVisibility(InsertarGestionarPaciente);
                    PlanillaControl.Paciente = default;
                    EditMode = true;
                    break;
                }

                case ToolStripButton button when (button == ModificarPacienteToolStripButton):
                {
                    SwitchPacienteTabsVisibility(ModificarGestionarPaciente);
                    EditMode = true;
                    break;
                }

                case ToolStripButton button when (button == EliminarPacienteToolStripButton):
                SwitchPacienteTabsVisibility(EliminarGestionarPaciente);
                break;
            }
        }

        void GestionarPacienteButton_MouseUp(object sender, MouseEventArgs e)
            => (sender as Button).Parent.Focus();

        void CancelarGestionarPacienteButton_Click(object sender, EventArgs e)
        {
            var paciente = SelectedItem?.Tag as PacienteApiModel;

            if (sender != EliminarGestionarPaciente.CancelarButton)
                PlanillaControl.Paciente = paciente;

            EditMode = false;
            SwitchPacienteTabsVisibility(ListView);
        }

        void AceptarGestionarPacienteButton_Click(object sender, EventArgs e)
        {
            var modified = false;

            if (sender == InsertarGestionarPaciente.AceptarButton)
            {
                var paciente = PlanillaControl.Paciente;
                //paciente.Id = ItemCounter.Increment();
                if (string.IsNullOrEmpty(paciente.Id))
                    paciente.Id = Guid.NewGuid().ToString();

                if (paciente.Account.Error != null)
                    InsertarGestionarPaciente.Error = paciente.Account.Error;
                else
                {
                    modified = true;
                    ListViewUpdate(paciente, addNew: true);

                    EditMode = false;
                    SwitchPacienteTabsVisibility(ListView);
                }
            }
            else if (sender == ModificarGestionarPaciente.AceptarButton)
            {
                var paciente = PlanillaControl.Paciente;

                if (paciente.Account.Error != null)
                    ModificarGestionarPaciente.Error = paciente.Account.Error;
                else
                {
                    modified = true;
                    ListViewUpdate(paciente, addNew: false);

                    EditMode = false;
                    SwitchPacienteTabsVisibility(ListView);
                }
            }
            else if (sender == EliminarGestionarPaciente.AceptarButton)
            {
                var paciente = SelectedItem.Tag as PacienteApiModel;

                if (paciente.SetAsDeleted)
                    ListView.Items.Remove(SelectedItem);
                else
                {
                    paciente.SetAsDeleted = true;
                    SelectedItem.Font = new Font(SelectedItem.Font, SelectedItem.Font.Style | FontStyle.Strikeout);
                }

                modified = true;
                SwitchPacienteTabsVisibility(ListView);
            }

            if (modified)
            {
                PacienteColumnHeader.Text = $"Paciente ({ListView.Items.Count})";
                SaveData();
            }
        }

        #endregion Pacientes Tab

        #region ListView

        void AbrirUbicacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fullPath = Path.GetFullPath("DB");
            Process.Start(fullPath);
        }

        void ExcelToolStripMenuItem_Click(object sender, EventArgs e)
            => SaveToExcel(sender == ExportarYAbrirToolStripMenuItem ? true : false);

        public void SaveToExcel(bool open)
        {
            using (var excel = new ExcelPackage())
            {
                var columnNames = new List<KeyValuePair<string, string>>
                {
                    #region Datos Generales
                    new KeyValuePair<string, string>("A", $"Pacientes({ListView.Items.Count})"),
                    new KeyValuePair<string, string>("B", "Edad"),
                    new KeyValuePair<string, string>("C", "Identificador"),
                    new KeyValuePair<string, string>("D", "FechaIngreso"),
                    new KeyValuePair<string, string>("E", "Provincia"),
                    new KeyValuePair<string, string>("F", "Municipio"),
                    new KeyValuePair<string, string>("G", "Área"),
                    new KeyValuePair<string, string>("H", "Ocupación"),
                    new KeyValuePair<string, string>("I", "Escolaridad"),
                    new KeyValuePair<string, string>("J", "Hospital"),
                    new KeyValuePair<string, string>("K", "HospitalProv."),
                    new KeyValuePair<string, string>("L", "Remitida"),
                    new KeyValuePair<string, string>("M", "Institución1"),
                    new KeyValuePair<string, string>("N", "Institución2"),
                    #endregion

                    #region Antecedentes Gineco-Obstétricos
                    new KeyValuePair<string, string>("O", "Gestaciones"),
                    new KeyValuePair<string, string>("P", "PartosVaginales"),
                    new KeyValuePair<string, string>("Q", "Cesáreas"),
                    new KeyValuePair<string, string>("R", "Vivos"),
                    new KeyValuePair<string, string>("S", "Muertos"),
                    new KeyValuePair<string, string>("T", "Abortos"),
                    new KeyValuePair<string, string>("U", "Ectópicos"),
                    new KeyValuePair<string, string>("V", "Molas"),
                    new KeyValuePair<string, string>("W", "PartoAnterior"),
                    #endregion

                    #region Atención prenatal
                    new KeyValuePair<string, string>("X", "Captación"),
                    new KeyValuePair<string, string>("Y", "Controles"),
                    new KeyValuePair<string, string>("Z", "Riesgo"),
                    new KeyValuePair<string, string>("AA", "Doppler"),
                    new KeyValuePair<string, string>("AB", "Re-evaluación"),
                    new KeyValuePair<string, string>("AC", "Edad extrema"),
                    new KeyValuePair<string, string>("AD", "Asma"),
                    new KeyValuePair<string, string>("AE", "DiabetesMellitus"),

                    new KeyValuePair<string, string>("AF", "Anemia"),
                    new KeyValuePair<string, string>("AG", "Malnutrición"),
                    new KeyValuePair<string, string>("AH", "HTA"),
                    new KeyValuePair<string, string>("AI", "Pre-eclampsia"),
                    new KeyValuePair<string, string>("AJ", "Prematuridad"),
                    new KeyValuePair<string, string>("AK", "Gemelaridad"),
                    new KeyValuePair<string, string>("AL", "InfecciónUrinaria"),
                    new KeyValuePair<string, string>("AM", "InfecciónVaginal"),
                    new KeyValuePair<string, string>("AN", "ITS"),
                    new KeyValuePair<string, string>("AO", "HábitosTóxicos"),
                    new KeyValuePair<string, string>("AP", "OtrasCondiciones"),

                    new KeyValuePair<string, string>("AQ", "IMC"),

                    new KeyValuePair<string, string>("AR", "Hemoglobina1T"),
                    new KeyValuePair<string, string>("AS", "Hemoglobina2T"),
                    new KeyValuePair<string, string>("AT", "Hemoglobina3T"),
                    new KeyValuePair<string, string>("AU", "Urocultivo1T"),
                    new KeyValuePair<string, string>("AV", "Urocultivo2T"),
                    new KeyValuePair<string, string>("AW", "Urocultivo3T"),
                    #endregion

                    #region Atención hospitalaria
                    new KeyValuePair<string, string>("AX", "CuidadosPerinatales"),
                    new KeyValuePair<string, string>("AY", "CuidadosIntensivos"),
                    new KeyValuePair<string, string>("AZ", "TipoParto"),
                    new KeyValuePair<string, string>("BA", "DiagnosticoMMEG"),

                    new KeyValuePair<string, string>("BB", "HtaPee"),
                    new KeyValuePair<string, string>("BC", "HtaCrónica"),
                    new KeyValuePair<string, string>("BD", "ComplicacionesHemorrágicas"),
                    new KeyValuePair<string, string>("BE", "Complicaciones Aborto"),
                    new KeyValuePair<string, string>("BF", "SepsisObstétrico"),
                    new KeyValuePair<string, string>("BG", "SepsisNoObstétrico"),
                    new KeyValuePair<string, string>("BH", "SepsisPulmonar"),
                    new KeyValuePair<string, string>("BI", "EnfermedadExistente"),
                    new KeyValuePair<string, string>("BJ", "OtraCausa"),

                    new KeyValuePair<string, string>("BK", "ShockSéptico"),
                    new KeyValuePair<string, string>("BL", "ShockHipovolémico"),
                    new KeyValuePair<string, string>("BM", "Eclampsia"),
                
                    new KeyValuePair<string, string>("BN", "FallaCerebral"),
                    new KeyValuePair<string, string>("BO", "FallaCardiaca"),
                    new KeyValuePair<string, string>("BP", "FallaHepática"),
                    new KeyValuePair<string, string>("BQ", "FallaVascular"),
                    new KeyValuePair<string, string>("BR", "FallaRenal"),
                    new KeyValuePair<string, string>("BS", "FallaSangre"),
                    new KeyValuePair<string, string>("BT", "FallaMetabólica"),
                    new KeyValuePair<string, string>("BU", "FallaRespiratoria"),

                    new KeyValuePair<string, string>("BV", "Cirugía"),
                    new KeyValuePair<string, string>("BW", "Transfusión"),

                    new KeyValuePair<string, string>("BX", "HisterectomíaTotal"),
                    new KeyValuePair<string, string>("BY", "HisterectomíaSubTotal"),
                    new KeyValuePair<string, string>("BZ", "SalpingectomíaTotal"),
                    new KeyValuePair<string, string>("CA", "SuturasCompresivas"),
                    new KeyValuePair<string, string>("CB", "LigadurasSelectivas"),
                    new KeyValuePair<string, string>("CC", "LigadurasHipogástricas"),
                    new KeyValuePair<string, string>("CD", "OtraIntervención"),

                    new KeyValuePair<string, string>("CE", "CausaHemorragia"),
                    new KeyValuePair<string, string>("CF", "PeriodoHemorragia"),
                    new KeyValuePair<string, string>("CG", "ÁcidoTranexámico"),
                    new KeyValuePair<string, string>("CH", "Ocitocina"),
                    new KeyValuePair<string, string>("CI", "Ergonovina"),
                    new KeyValuePair<string, string>("CJ", "Misoprostol"),
                    new KeyValuePair<string, string>("CK", "SulfatoMagnesio"),
                    #endregion

                    #region Egreso
                    new KeyValuePair<string, string>("CL", "Fallecida"),
                    new KeyValuePair<string, string>("CM", "FechaEgreso"),
                    new KeyValuePair<string, string>("CN", "EstadíaHosp"),
                    new KeyValuePair<string, string>("CO", "ReciénNacido"),
                    new KeyValuePair<string, string>("CP", "EdadGestacional"),
                    new KeyValuePair<string, string>("CQ", "Peso"),
                    new KeyValuePair<string, string>("CR", "Apgar1'"),
                    new KeyValuePair<string, string>("CS", "Apgar5'"),
                    new KeyValuePair<string, string>("CT", "CausaMuerteDirecta"),
                    new KeyValuePair<string, string>("CU", "CausaMuerteIndirecta"),
                    #endregion
                };

                var rowOffset = 3;

                var date = DateTime.Now;

                var sheet = excel.Workbook.Worksheets.Add("Base de datos");

                var columnCount = columnNames.Count;

                var firstColumn = columnNames[0].Key;
                var lastColumn = columnNames[columnNames.Count - 1].Key;

                var titleRange = $"{firstColumn}1:{lastColumn}1";

                sheet.Cells[titleRange].Merge = true;
                sheet.Cells[titleRange].Value = $"Halo v{VersionString} - {DateTime.Now.ToString("dd/MM/yyyy")} - {DateTime.Now.ToString("hh:mm:ss")}";
                sheet.Cells[titleRange].Style.Font.SetFromFont(new Font(ListView.Font.FontFamily, emSize: 10, FontStyle.Bold));

                sheet.Cells[titleRange].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Double);

                #region Headers
                var rowCount = rowOffset + ListView.Items.Count + 2;

                #region Datos Generales
                var headerRange = $"A{rowOffset}:N{rowOffset}";
                var bodyRange = $"N{rowOffset}:N{rowCount}";

                sheet.Cells[headerRange].Merge = true;
                sheet.Cells[headerRange].Value = "               Datos Generales               ";
                sheet.Cells[headerRange].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.MediumDashDotDot);
                sheet.Cells[headerRange].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Fill;
                sheet.Cells[headerRange].Style.Font.SetFromFont(new Font(ListView.Font.FontFamily, emSize: 8, FontStyle.Bold));
                sheet.Cells[bodyRange].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.MediumDashDotDot;
                #endregion


                #region Antecedentes Gineco-Obstétricos
                headerRange = $"O{rowOffset}:W{rowOffset}";
                bodyRange = $"W{rowOffset}:W{rowCount}";

                sheet.Cells[headerRange].Merge = true;
                sheet.Cells[headerRange].Value = "                               Antecedentes Gineco-Obstétricos                               ";
                sheet.Cells[headerRange].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.MediumDashDotDot);
                sheet.Cells[headerRange].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Fill;
                sheet.Cells[headerRange].Style.Font.SetFromFont(new Font(ListView.Font.FontFamily, emSize: 8, FontStyle.Bold));
                sheet.Cells[bodyRange].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.MediumDashDotDot;
                #endregion

                #region Atención Prenatal
                headerRange = $"X{rowOffset}:AW{rowOffset}";
                bodyRange = $"AW{rowOffset}:AW{rowCount}";

                sheet.Cells[headerRange].Merge = true;
                sheet.Cells[headerRange].Value = "                 Atención Prenatal                 ";
                sheet.Cells[headerRange].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.MediumDashDotDot);
                sheet.Cells[headerRange].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Fill;
                sheet.Cells[headerRange].Style.Font.SetFromFont(new Font(ListView.Font.FontFamily, emSize: 8, FontStyle.Bold));
                sheet.Cells[bodyRange].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.MediumDashDotDot;
                #endregion

                #region Atención Hospitalaria
                headerRange = $"AX{rowOffset}:CK{rowOffset}";
                bodyRange = $"CK{rowOffset}:CK{rowCount}";

                sheet.Cells[headerRange].Merge = true;
                sheet.Cells[headerRange].Value = "                     Atención Hospitalaria                     ";
                sheet.Cells[headerRange].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.MediumDashDotDot);
                sheet.Cells[headerRange].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Fill;
                sheet.Cells[headerRange].Style.Font.SetFromFont(new Font(ListView.Font.FontFamily, emSize: 8, FontStyle.Bold));
                sheet.Cells[bodyRange].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.MediumDashDotDot;
                #endregion

                #region Egreso
                headerRange = $"CL{rowOffset}:CU{rowOffset}";
                bodyRange = $"CU{rowOffset}:CU{rowCount}";

                sheet.Cells[headerRange].Merge = true;
                sheet.Cells[headerRange].Value = "      Egreso      ";
                sheet.Cells[headerRange].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.MediumDashDotDot);
                sheet.Cells[headerRange].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Fill;
                sheet.Cells[headerRange].Style.Font.SetFromFont(new Font(ListView.Font.FontFamily, emSize: 8, FontStyle.Bold));
                sheet.Cells[bodyRange].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.MediumDashDotDot;
                #endregion

                rowOffset++;
                #endregion

                var tableRange = $"{columnNames[0].Key}{rowOffset}:{columnNames[columnCount - 1].Key}{ListView.Items.Count + rowOffset}";

                var table = sheet.Tables.Add(new ExcelAddressBase(tableRange), nameof(Halo));
                {
                    table.ShowTotal = true;
                    table.ShowHeader = true;
                    table.ShowRowStripes = false;
                    table.TableStyle = OfficeOpenXml.Table.TableStyles.Light9;
                }

                foreach (var columnName in columnNames)
                    sheet.Cells[$"{columnName.Key}{rowOffset}"].Value = columnName.Value;

                foreach (ListViewItem item in ListView.Items)
                {
                    rowOffset++;

                    var j = 0;

                    var paciente = item.Tag as PacienteApiModel;

                    #region Datos Generales
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.Nombre;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.Edad;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistoriaClinica;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.FechaIngreso?.ToString("dd/MM/yyyy");
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.ProvinciaNombreCorto;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = MunicipioApiModel.Municipios.SingleOrDefault(p => p.Id == paciente.MunicipioId);
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = (TipoArea?)paciente.AreaId;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = (TipoOcupacion?)paciente.OcupacionId;

                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = (TipoEscolaridad?)paciente.EscolaridadId;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HospitalNombre;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HospitalProvinciaNombreCorto;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.Traslado1HospitalId == null ? "No" : "Sí";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = HospitalApiModel.Hospitales.SingleOrDefault(p => p.Id == paciente.Traslado1HospitalId);
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = HospitalApiModel.Hospitales.SingleOrDefault(p => p.Id == paciente.Traslado2HospitalId);
                    #endregion

                    #region Antecedentes Gineco-Obstétricos
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.AntecedenteGinecoObstetrico?.Gestaciones;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.AntecedenteGinecoObstetrico?.PartosVaginales;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.AntecedenteGinecoObstetrico?.Cesareas;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.AntecedenteGinecoObstetrico?.Vivos;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.AntecedenteGinecoObstetrico?.Muertos;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.AntecedenteGinecoObstetrico?.Abortos;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.AntecedenteGinecoObstetrico?.Ectopicos;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.AntecedenteGinecoObstetrico?.Molas;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.AntecedenteGinecoObstetrico?.UltimaGestacion?.ToString("dd/MM/yyyy");
                    #endregion

                    #region Atención prenatal
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.AtencionPrenatal?.SemanasCaptacion;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.AtencionPrenatal?.ControlesPrenatales;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.AtencionPrenatal?.EvaluadoComoRiesgo == null ? null : paciente.HistorialMedico?.AtencionPrenatal?.EvaluadoComoRiesgo.Value == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = (DopplerArteriaUterina?)paciente.HistorialMedico?.AtencionPrenatal?.DopplerArteriaUterina;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.AtencionPrenatal?.Reevaluacion == null ? null : paciente.HistorialMedico?.AtencionPrenatal?.Reevaluacion.Value == true ? "Sí" : "No";

                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CondicionesIdentificadas?)paciente.HistorialMedico?.AtencionPrenatal?.CondicionesIdentificadasFlags)?.HasFlag(CondicionesIdentificadas.EdadExtrema) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CondicionesIdentificadas?)paciente.HistorialMedico?.AtencionPrenatal?.CondicionesIdentificadasFlags)?.HasFlag(CondicionesIdentificadas.Asma) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CondicionesIdentificadas?)paciente.HistorialMedico?.AtencionPrenatal?.CondicionesIdentificadasFlags)?.HasFlag(CondicionesIdentificadas.DiabetesMellitus) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CondicionesIdentificadas?)paciente.HistorialMedico?.AtencionPrenatal?.CondicionesIdentificadasFlags)?.HasFlag(CondicionesIdentificadas.Anemia) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CondicionesIdentificadas?)paciente.HistorialMedico?.AtencionPrenatal?.CondicionesIdentificadasFlags)?.HasFlag(CondicionesIdentificadas.Malnutricion) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CondicionesIdentificadas?)paciente.HistorialMedico?.AtencionPrenatal?.CondicionesIdentificadasFlags)?.HasFlag(CondicionesIdentificadas.HTA) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CondicionesIdentificadas?)paciente.HistorialMedico?.AtencionPrenatal?.CondicionesIdentificadasFlags)?.HasFlag(CondicionesIdentificadas.Preeclampsia) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CondicionesIdentificadas?)paciente.HistorialMedico?.AtencionPrenatal?.CondicionesIdentificadasFlags)?.HasFlag(CondicionesIdentificadas.Prematuridad) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CondicionesIdentificadas?)paciente.HistorialMedico?.AtencionPrenatal?.CondicionesIdentificadasFlags)?.HasFlag(CondicionesIdentificadas.Gemelaridad) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CondicionesIdentificadas?)paciente.HistorialMedico?.AtencionPrenatal?.CondicionesIdentificadasFlags)?.HasFlag(CondicionesIdentificadas.InfeccionUrinaria) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CondicionesIdentificadas?)paciente.HistorialMedico?.AtencionPrenatal?.CondicionesIdentificadasFlags)?.HasFlag(CondicionesIdentificadas.InfeccionVaginal) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CondicionesIdentificadas?)paciente.HistorialMedico?.AtencionPrenatal?.CondicionesIdentificadasFlags)?.HasFlag(CondicionesIdentificadas.ITS) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CondicionesIdentificadas?)paciente.HistorialMedico?.AtencionPrenatal?.CondicionesIdentificadasFlags)?.HasFlag(CondicionesIdentificadas.HabitosToxicos) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CondicionesIdentificadas?)paciente.HistorialMedico?.AtencionPrenatal?.CondicionesIdentificadasFlags)?.HasFlag(CondicionesIdentificadas.Otras) == true ? "Sí" : "No";

                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = (TipoIndiceMasaCorporal?)paciente.HistorialMedico?.AtencionPrenatal?.IndiceMasaCorporalId;

                    if (paciente.HistorialMedico?.AtencionPrenatal?.Hemoglobina?.PrimerTrimestre is int hemoglobinaPrimerTrimestre)
                        if (hemoglobinaPrimerTrimestre == PlanillaControl.HemoglobinaNRCheckedConstant)
                            sheet.Cells[$"{columnNames[j].Key}{rowOffset}"].Value = "No realizado";
                        else
                            sheet.Cells[$"{columnNames[j].Key}{rowOffset}"].Value = hemoglobinaPrimerTrimestre;
                    j++;

                    if (paciente.HistorialMedico?.AtencionPrenatal?.Hemoglobina?.SegundoTrimestre is int hemoglobinaSegundoTrimestre)
                        if (hemoglobinaSegundoTrimestre == PlanillaControl.HemoglobinaNRCheckedConstant)
                            sheet.Cells[$"{columnNames[j].Key}{rowOffset}"].Value = "No realizado";
                        else
                            sheet.Cells[$"{columnNames[j].Key}{rowOffset}"].Value = hemoglobinaSegundoTrimestre;
                    j++;

                    if (paciente.HistorialMedico?.AtencionPrenatal?.Hemoglobina?.TercerTrimestre is int hemoglobinaTercerTrimestre)
                        if (hemoglobinaTercerTrimestre == PlanillaControl.HemoglobinaNRCheckedConstant)
                            sheet.Cells[$"{columnNames[j].Key}{rowOffset}"].Value = "No realizado";
                        else
                            sheet.Cells[$"{columnNames[j].Key}{rowOffset}"].Value = hemoglobinaTercerTrimestre;
                    j++;

                    switch (paciente.HistorialMedico?.AtencionPrenatal?.Urocultivo?.PrimerTrimestre)
                    {
                        case 0: sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = "Negativo"; break;
                        case 1: sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = "Positivo"; break;
                        case 2: sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = "No realizado"; break;
                        default: j++; break;
                    }

                    switch (paciente.HistorialMedico?.AtencionPrenatal?.Urocultivo?.SegundoTrimestre)
                    {
                        case 0: sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = "Negativo"; break;
                        case 1: sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = "Positivo"; break;
                        case 2: sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = "No realizado"; break;
                        default: j++; break;
                    }

                    switch (paciente.HistorialMedico?.AtencionPrenatal?.Urocultivo?.TercerTrimestre)
                    {
                        case 0: sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = "Negativo"; break;
                        case 1: sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = "Positivo"; break;
                        case 2: sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = "No realizado"; break;
                        default: j++; break;
                    }
                    #endregion

                    #region Atención hospitalaria
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((LugarIngreso?)paciente.HistorialMedico?.AtencionHospitalaria?.LugarIngresoFlags)?.HasFlag(LugarIngreso.CuidadosPerinatales) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((LugarIngreso?)paciente.HistorialMedico?.AtencionHospitalaria?.LugarIngresoFlags)?.HasFlag(LugarIngreso.UnidadCuidadosIntensivos) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = (TipoParto?)paciente.HistorialMedico?.AtencionHospitalaria?.PartoId;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = (TipoMorbilidadParto?)paciente.HistorialMedico?.AtencionHospitalaria?.MorbilidadPartoId;

                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CausasMorbilidad?)paciente.HistorialMedico?.AtencionHospitalaria?.CausasMorbilidadFlags)?.HasFlag(CausasMorbilidad.HtaPee) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CausasMorbilidad?)paciente.HistorialMedico?.AtencionHospitalaria?.CausasMorbilidadFlags)?.HasFlag(CausasMorbilidad.HtaCronica) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CausasMorbilidad?)paciente.HistorialMedico?.AtencionHospitalaria?.CausasMorbilidadFlags)?.HasFlag(CausasMorbilidad.ComplicacionesHemorragicas) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CausasMorbilidad?)paciente.HistorialMedico?.AtencionHospitalaria?.CausasMorbilidadFlags)?.HasFlag(CausasMorbilidad.ComplicacionesAborto) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CausasMorbilidad?)paciente.HistorialMedico?.AtencionHospitalaria?.CausasMorbilidadFlags)?.HasFlag(CausasMorbilidad.SepsisOrigenObstetrico) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CausasMorbilidad?)paciente.HistorialMedico?.AtencionHospitalaria?.CausasMorbilidadFlags)?.HasFlag(CausasMorbilidad.SepsisOrigenNoObstetrico) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CausasMorbilidad?)paciente.HistorialMedico?.AtencionHospitalaria?.CausasMorbilidadFlags)?.HasFlag(CausasMorbilidad.SepsisOrigenPulmonar) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((CausasMorbilidad?)paciente.HistorialMedico?.AtencionHospitalaria?.CausasMorbilidadFlags)?.HasFlag(CausasMorbilidad.ComplicacionEnfermedadExistente) == true ? "Sí" : "No";

                    if (((CausasMorbilidad?)paciente.HistorialMedico?.AtencionHospitalaria?.CausasMorbilidadFlags)?.HasFlag(CausasMorbilidad.Otra) == true)
                        sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.AtencionHospitalaria?.OtraCausaMorbilidad;
                    else
                        j++;

                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((EnfermedadEspecifica?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(EnfermedadEspecifica.ShockSeptico) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((EnfermedadEspecifica?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(EnfermedadEspecifica.ShockHipovolemico) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((EnfermedadEspecifica?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(EnfermedadEspecifica.Eclampsia) == true ? "Sí" : "No";

                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((FallaOrganica?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(FallaOrganica.Cerebral) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((FallaOrganica?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(FallaOrganica.Cardiaca) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((FallaOrganica?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(FallaOrganica.Hepatica) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((FallaOrganica?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(FallaOrganica.Vascular) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((FallaOrganica?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(FallaOrganica.Renal) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((FallaOrganica?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(FallaOrganica.Coagulacion) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((FallaOrganica?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(FallaOrganica.Metabolica) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((FallaOrganica?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(FallaOrganica.Respiratoria) == true ? "Sí" : "No";

                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((TipoManejo?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(TipoManejo.Cirugia) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((TipoManejo?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(TipoManejo.Transfusion) == true ? "Sí" : "No";

                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((IntervencionQuirurgica?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(IntervencionQuirurgica.HisterectomiaTotal) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((IntervencionQuirurgica?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(IntervencionQuirurgica.HisterectomiaSubTotal) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((IntervencionQuirurgica?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(IntervencionQuirurgica.SalpingectomiaTotal) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((IntervencionQuirurgica?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(IntervencionQuirurgica.SuturasCompresivas) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((IntervencionQuirurgica?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(IntervencionQuirurgica.LigadurasArterialesSelectivas) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((IntervencionQuirurgica?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(IntervencionQuirurgica.LigadurasArteriasHipogastricas) == true ? "Sí" : "No";

                    if (((IntervencionQuirurgica?)paciente.HistorialMedico?.AtencionHospitalaria?.CausasMorbilidadFlags)?.HasFlag(IntervencionQuirurgica.Otra) == true)
                        sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.AtencionHospitalaria?.OtraIntervencionQuirurgica;
                    else
                        j++;

                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = (CausaHemorragia?)paciente.HistorialMedico?.AtencionHospitalaria?.CausaHemorragiaId;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = (PeriodoHemorragia?)paciente.HistorialMedico?.AtencionHospitalaria?.HemorragiaId;

                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((UsoOcitocicos?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(UsoOcitocicos.AcidoTranexamico) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((UsoOcitocicos?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(UsoOcitocicos.Ocitocina) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((UsoOcitocicos?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(UsoOcitocicos.Ergonovina) == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = ((UsoOcitocicos?)paciente.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad.EnfermedadEspecificaFlags)?.HasFlag(UsoOcitocicos.Misoprostol) == true ? "Sí" : "No";

                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.AtencionHospitalaria?.UsoSulfatoMagnesio == null ? null : paciente.HistorialMedico?.AtencionHospitalaria?.UsoSulfatoMagnesio.Value == true ? "Sí" : "No";
                    #endregion

                    #region Egreso
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.Egreso?.Fallecida == null ? null : paciente.HistorialMedico?.Egreso?.Fallecida.Value == true ? "Sí" : "No";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.Egreso?.Fecha?.ToString("dd/MM/yyyy");
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.EstadiaHospitalaria;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.Egreso?.RecienNacido?.Fallecido == null ? null : paciente.HistorialMedico?.Egreso?.RecienNacido?.Fallecido.Value == true ? "Muerto" : "Vivo";
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.Egreso?.RecienNacido?.EdadGestacional;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.Egreso?.RecienNacido?.Peso;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.Egreso?.RecienNacido?.Apgar1;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = paciente.HistorialMedico?.Egreso?.RecienNacido?.Apgar2;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = (TipoCausaMuerteDirecta?)paciente.HistorialMedico?.Egreso?.CausaMuerteDirectaId;
                    sheet.Cells[$"{columnNames[j++].Key}{rowOffset}"].Value = (TipoCausaMuerteIndirecta?)paciente.HistorialMedico?.Egreso?.CausaMuerteIndirectaId;
                    #endregion

                    sheet.Cells[$"{firstColumn}{rowOffset}:{lastColumn}{rowOffset}"].Style.Font.SetFromFont(item.Font);
                    sheet.Cells[$"{firstColumn}{rowOffset}:{LastColumn}{rowOffset}"].Style.Font.Color.SetColor(item.ForeColor);
                }

                sheet.Cells[tableRange].AutoFitColumns();

                if (!Directory.Exists("DB"))
                    Directory.CreateDirectory("DB");

                var fileName = $"DB/{DataFileName}_(v{VersionString})_{date.ToString("dd-MM-yyyy")}_{date.ToString("hh-mm-ss")}.xlsx";
                var fullPath = Path.GetFullPath(fileName);

                try
                {
                    excel.SaveAs(new FileInfo(fullPath));
                }
                catch
                {
                    MessageBox.Show($"Error exportando a excel, verifique el espacio en disco y los permisos de escritura. ", "Halo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }

                try
                {
                    if (open)
                        Process.Start(fullPath);
                }
                catch
                {
                    MessageBox.Show($"Error abriendo excel. ", "Halo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
            }
        }

        void SaveData()
        {
            if (Operation != Operation.Paciente)
                return;

            var pacientes = new Queue<PacienteApiModel>(ListView.Items.Count);

            foreach (ListViewItem item in ListView.Items)
                pacientes.Enqueue(item.Tag as PacienteApiModel);

            //var data = JsonConvert.SerializeObject(pacientes, Formatting.Indented);
            //File.WriteAllText($"{DataFileName}.json", data);

            var serializer = new Serializer();
            serializer.Serialize(nameof(Halo));
            serializer.Serialize(VersionString);
            serializer.Serialize(pacientes);

            File.WriteAllBytes($"{DataFileName}.cyxor", serializer.ToByteArray());
        }

        void LoadData()
        {
            if (Operation != Operation.Paciente)
                return;

            if (!File.Exists($"{DataFileName}.cyxor"))
                return;

            //var data = File.ReadAllText($"{DataFileName}.json");
            //var pacientes = JsonConvert.DeserializeObject<IEnumerable<PacienteApiModel>>(data);

            var data = File.ReadAllBytes($"{DataFileName}.cyxor");

            var pacientes = default(IEnumerable<PacienteApiModel>);

            //var tryOld

            var updated = false;

            try
            {
                var serializer = new Serializer(data);

                var fileType = serializer.DeserializeString();

                if (fileType != nameof(Halo))
                {

                }

                var fileVersion = serializer.DeserializeString();

                if (fileVersion != VersionString)
                {

                }

                pacientes = serializer.DeserializeIEnumerable<PacienteApiModel>();
            }
            catch
            {
                if (MessageBox.Show($"Se ha detectado una base de datos incompatible. " +
                    $"El programa intentará actualizarla a la versión {VersionString}. ", "Halo",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                {
                    Application.Exit();
                    return;
                }

                try
                {
                    var pacientesV090 = Serializer.XDeserialize<IEnumerable<Models.Migrations.V0900.PacienteApiModel>>(data);

                    pacientes = new List<PacienteApiModel>();

                    foreach (var pacienteV090 in pacientesV090)
                    {
                        var causasMorbilidadV090 = (Models.Migrations.V0900.CausasMorbilidad?)pacienteV090.HistorialMedico?.AtencionHospitalaria?.CausasMorbilidadFlags;

                        var causaMorbilidad = causasMorbilidadV090.HasValue ? default(CausasMorbilidad) : default(CausasMorbilidad?);

                        if (causaMorbilidad != null)
                        {
                            var cm = causasMorbilidadV090.Value;

                            causaMorbilidad |= cm.HasFlag(Models.Migrations.V0900.CausasMorbilidad.TrastornosHipertensivos) ? CausasMorbilidad.HtaPee : 0;
                            causaMorbilidad |= cm.HasFlag(Models.Migrations.V0900.CausasMorbilidad.ComplicacionesHemorragicas) ? CausasMorbilidad.ComplicacionesHemorragicas : 0;
                            causaMorbilidad |= cm.HasFlag(Models.Migrations.V0900.CausasMorbilidad.ComplicacionesAborto) ? CausasMorbilidad.ComplicacionesAborto : 0;
                            causaMorbilidad |= cm.HasFlag(Models.Migrations.V0900.CausasMorbilidad.SepsisOrigenPulmonar) ? CausasMorbilidad.SepsisOrigenPulmonar : 0;
                            causaMorbilidad |= cm.HasFlag(Models.Migrations.V0900.CausasMorbilidad.SepsisOrigenObstetrico) ? CausasMorbilidad.SepsisOrigenObstetrico : 0;
                            causaMorbilidad |= cm.HasFlag(Models.Migrations.V0900.CausasMorbilidad.SepsisOrigenNoObstetrico) ? CausasMorbilidad.SepsisOrigenNoObstetrico : 0;
                            causaMorbilidad |= cm.HasFlag(Models.Migrations.V0900.CausasMorbilidad.ComplicacionEnfermedadExistente) ? CausasMorbilidad.ComplicacionEnfermedadExistente : 0;
                            causaMorbilidad |= cm.HasFlag(Models.Migrations.V0900.CausasMorbilidad.OtraCausa) ? CausasMorbilidad.Otra : 0;
                        }

                        var paciente = new PacienteApiModel
                        {
                            //Version = VersionString,
                            Version = default,
                            AreaId = pacienteV090.AreaId,
                            Edad = pacienteV090.Edad,
                            EscolaridadId = pacienteV090.EscolaridadId,
                            FechaIngreso = pacienteV090.FechaIngreso,
                            HospitalId = pacienteV090.HospitalId,
                            Id = pacienteV090.Id,
                            MunicipioId = pacienteV090.MunicipioId,
                            Nombre = pacienteV090.Nombre,
                            OcupacionId = pacienteV090.OcupacionId,
                            SetAsDeleted = pacienteV090.SetAsDeleted,
                            Traslado1HospitalId = pacienteV090.Traslado1HospitalId,
                            Traslado2HospitalId = pacienteV090.Traslado2HospitalId,

                            Account = new AccountPacienteApiModel
                            {
                                Error = pacienteV090.Account?.Error,
                                Seguimiento = pacienteV090.Account?.Seguimiento ?? false,
                                Visto = pacienteV090.Account?.Visto ?? false,
                            },

                            HistorialMedico = new HistorialMedicoApiModel
                            {
                                AntecedenteGinecoObstetrico = new AntecedenteGinecoObstetricoApiModel
                                {
                                    Abortos = pacienteV090.HistorialMedico?.AntecedenteGinecoObstetrico?.Abortos,
                                    Cesareas = pacienteV090.HistorialMedico?.AntecedenteGinecoObstetrico?.Cesareas,
                                    Ectopicos = pacienteV090.HistorialMedico?.AntecedenteGinecoObstetrico?.Ectopicos,
                                    Gestaciones = pacienteV090.HistorialMedico?.AntecedenteGinecoObstetrico?.Gestaciones,
                                    Molas = pacienteV090.HistorialMedico?.AntecedenteGinecoObstetrico?.Molas,
                                    Muertos = pacienteV090.HistorialMedico?.AntecedenteGinecoObstetrico?.Muertos,
                                    PartosVaginales = pacienteV090.HistorialMedico?.AntecedenteGinecoObstetrico?.PartosVaginales,
                                    UltimaGestacion = pacienteV090.HistorialMedico?.AntecedenteGinecoObstetrico?.UltimaGestacion,
                                    Vivos = pacienteV090.HistorialMedico?.AntecedenteGinecoObstetrico?.Vivos,
                                },
                                AtencionHospitalaria = new AtencionHospitalariaApiModel
                                {
                                    //CausaHemorragiaId = causaMorbilidad?.HasFlag(CausasMorbilidad.ComplicacionesHemorragicas) ?? false ? (int)CausaHemorragia.Atonia : default(int?),

                                    CausasMorbilidadFlags = (int?)causaMorbilidad,
                                    HemorragiaId = pacienteV090.HistorialMedico?.AtencionHospitalaria?.HemorragiaId,
                                    IntervencionQuirurgicaFlags = pacienteV090.HistorialMedico?.AtencionHospitalaria?.IntervencionQuirurgicaFlags,
                                    LugarIngresoFlags = pacienteV090.HistorialMedico?.AtencionHospitalaria?.LugarIngresoFlags,
                                    MorbilidadPartoId = pacienteV090.HistorialMedico?.AtencionHospitalaria?.MorbilidadPartoId,
                                    OtraCausaMorbilidad = null,
                                    OtraIntervencionQuirurgica = pacienteV090.HistorialMedico?.AtencionHospitalaria?.OtraIntervencionQuirurgica,
                                    PartoId = pacienteV090.HistorialMedico?.AtencionHospitalaria?.PartoId,
                                    UsoOcitocicosFlags = pacienteV090.HistorialMedico?.AtencionHospitalaria?.UsoOcitocicosFlags,
                                    UsoSulfatoMagnesio = pacienteV090.HistorialMedico?.AtencionHospitalaria?.UsoSulfatoMagnesio,
                                    CriterioMorbilidad = new CriterioMorbilidadApiModel
                                    {
                                        ManejoFlags = pacienteV090.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad?.ManejoFlags,
                                        FallaOrganicaFlags = pacienteV090.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad?.FallaOrganicaFlags,
                                        EnfermedadEspecificaFlags = pacienteV090.HistorialMedico?.AtencionHospitalaria?.CriterioMorbilidad?.EnfermedadEspecificaFlags,
                                    },
                                },
                                AtencionPrenatal = new AtencionPrenatalApiModel
                                {
                                    CondicionesIdentificadasFlags = pacienteV090.HistorialMedico?.AtencionPrenatal?.CondicionesIdentificadasFlags,
                                    ControlesPrenatales = pacienteV090.HistorialMedico?.AtencionPrenatal?.ControlesPrenatales,
                                    DopplerArteriaUterina = pacienteV090.HistorialMedico?.AtencionPrenatal?.DopplerArteriaUterina,
                                    EvaluadoComoRiesgo = pacienteV090.HistorialMedico?.AtencionPrenatal?.EvaluadoComoRiesgo,
                                    IndiceMasaCorporalId = pacienteV090.HistorialMedico?.AtencionPrenatal?.IndiceMasaCorporalId,
                                    Reevaluacion = pacienteV090.HistorialMedico?.AtencionPrenatal?.Reevaluacion,
                                    SemanasCaptacion = pacienteV090.HistorialMedico?.AtencionPrenatal?.SemanasCaptacion,
                                    Hemoglobina = new HemoglobinaApiModel
                                    {
                                        PrimerTrimestre = pacienteV090.HistorialMedico?.AtencionPrenatal?.Hemoglobina?.PrimerTrimestre,
                                        SegundoTrimestre = pacienteV090.HistorialMedico?.AtencionPrenatal?.Hemoglobina?.SegundoTrimestre,
                                        TercerTrimestre = pacienteV090.HistorialMedico?.AtencionPrenatal?.Hemoglobina?.TercerTrimestre,
                                    },
                                    Urocultivo = new UrocultivoApiModel
                                    {
                                        PrimerTrimestre = pacienteV090.HistorialMedico?.AtencionPrenatal?.Urocultivo?.PrimerTrimestre,
                                        SegundoTrimestre = pacienteV090.HistorialMedico?.AtencionPrenatal?.Urocultivo?.SegundoTrimestre,
                                        TercerTrimestre = pacienteV090.HistorialMedico?.AtencionPrenatal?.Urocultivo?.TercerTrimestre,
                                    },
                                },
                                Egreso = new EgresoApiModel
                                {
                                    CausaMuerteDirectaId = pacienteV090.HistorialMedico?.Egreso?.CausaMuerteDirectaId,
                                    CausaMuerteIndirectaId = pacienteV090.HistorialMedico?.Egreso?.CausaMuerteIndirectaId,
                                    Fallecida = pacienteV090.HistorialMedico?.Egreso?.Fallecida,
                                    Fecha = pacienteV090.HistorialMedico?.Egreso?.Fecha,

                                    RecienNacido = new RecienNacidoApiModel
                                    {
                                        Apgar1 = pacienteV090.HistorialMedico?.Egreso?.RecienNacido?.Apgar1,
                                        Apgar2 = pacienteV090.HistorialMedico?.Egreso?.RecienNacido?.Apgar2,
                                        EdadGestacional = pacienteV090.HistorialMedico?.Egreso?.RecienNacido?.EdadGestacional,
                                        Fallecido = pacienteV090.HistorialMedico?.Egreso?.RecienNacido?.Fallecido,
                                        Peso = pacienteV090.HistorialMedico?.Egreso?.RecienNacido?.Peso,
                                    },
                                },
                            },
                        };

                        (pacientes as List<PacienteApiModel>).Add(paciente);
                    }

                    updated = true;
                }
                catch
                {
                    MessageBox.Show("La base de datos está dañada. Restaure la salva más reciente o póngase en contacto con los administradores.", "Halo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
            }

            ListView.BeginUpdate();
            Loading = true;

            //ItemCounter.Value = pacientes?.Max(p => p.Id) ?? 0;

            if (pacientes != null)
                foreach (var paciente in pacientes)
                    ListViewUpdate(paciente, addNew: true);

            Loading = false;
            ListView.EndUpdate();

            if (updated)
                SaveData();
        }

        void ListViewUpdate(PacienteApiModel paciente, bool addNew)
        {
            var item = SelectedItem;
            var fontStyle = FontStyle.Regular;
            NombrePacienteToolStripStatusLabel.Text = paciente.Nombre;

            if (!addNew)
            {
                item.SubItems[2].Text = paciente.FechaIngreso?.ToString("dd/MM/yy");
                item.SubItems[3].Text = paciente.Edad.ToString();
                item.SubItems[4].Text = paciente.Nombre;

                item.Tag = paciente;
            }
            else
            {
                item = new ListViewItem("") { Tag = paciente };

                item.SubItems.Add(
                    CheckedToolStripMenuItem == IdentificadorToolStripMenuItem ? paciente.HistoriaClinica :
                    CheckedToolStripMenuItem == HospitalToolStripMenuItem ? paciente.HospitalNombre :
                    CheckedToolStripMenuItem == ProvinciaToolStripMenuItem ? paciente.ProvinciaNombreCorto :
                    CheckedToolStripMenuItem == HospitalProvinciaToolStripMenuItem ? paciente.HospitalProvinciaNombreCorto :
                    paciente.EstadiaHospitalaria.ToString());

                item.SubItems.Add(paciente.FechaIngreso?.ToString("dd/MM/yy"));
                item.SubItems.Add(paciente.Edad.ToString());
                item.SubItems.Add(paciente.Nombre);

                ListView.Items.Add(item);
            }

            SelectedItem = item;

            if (!Loading)
                SelectedItem.Selected = true;

            if (paciente.HistorialMedico?.Egreso?.Fecha == null)
                item.ForeColor = Color.Black;
            else if (paciente.HistorialMedico?.Egreso?.Fallecida ?? false)
                item.ForeColor = Color.Red;
            else
                item.ForeColor = Color.Green;

            fontStyle |= paciente.Account.Visto ? FontStyle.Regular : FontStyle.Bold;
            fontStyle |= paciente.SetAsDeleted ? FontStyle.Strikeout : FontStyle.Regular;
            fontStyle |= paciente.Account.Seguimiento ? FontStyle.Underline : FontStyle.Regular;

            item.Font = new Font(item.Font, fontStyle);
        }

        void ListViewRemove()
        {
            if (SelectedItem.Tag is PacienteApiModel paciente)
                if (paciente.SetAsDeleted)
                    ListView.Items.Remove(SelectedItem);
                else
                {
                    paciente.SetAsDeleted = true;
                    SelectedItem.Font = new Font(SelectedItem.Font, SelectedItem.Font.Style | FontStyle.Strikeout);
                }
        }

        void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (LastColumn != e.Column)
                ListView.ListViewItemSorter = new ListViewItemComparer(SortOrder, e.Column, CheckedToolStripMenuItem);
            else
            {
                SortOrder = SortOrder == SortOrder.Ascending ? SortOrder = SortOrder.Descending : SortOrder.Ascending;
                ListView.ListViewItemSorter = new ListViewItemComparer(SortOrder, e.Column, CheckedToolStripMenuItem);
            }

            LastColumn = e.Column;
        }

        void ListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            var currentCursor = Cursor;
            Cursor = Cursors.WaitCursor;

            SelectedItem = e.IsSelected ? e.Item : null;
            var paciente = SelectedItem?.Tag as PacienteApiModel;

            SwitchPacienteButtonsAvailability(enable: true);

            PlanillaControl.Paciente = paciente;
            PictureBox.Image = PlanillaBitmap;
            NombrePacienteToolStripStatusLabel.Text = paciente?.Nombre;

            Cursor = currentCursor;
        }

        #endregion

        #region ListView Menu

        void InsertarToolStripMenuItem_Click(object sender, EventArgs e)
            => GestionarPacienteToolStripButton_Click(InsertarPacienteToolStripButton, e);

        void ModificarToolStripMenuItem_Click(object sender, EventArgs e)
            => GestionarPacienteToolStripButton_Click(ModificarPacienteToolStripButton, e);

        void EliminarToolStripMenuItem_Click(object sender, EventArgs e)
            => GestionarPacienteToolStripButton_Click(EliminarPacienteToolStripButton, e);

        void RestaurarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (SelectedItem.Tag as PacienteApiModel).SetAsDeleted = false;
            SwitchPacienteButtonsAvailability(enable: true);

            var fontStyle = SelectedItem.Font.Style ^ FontStyle.Strikeout;
            SelectedItem.Font = new Font(SelectedItem.Font, fontStyle);

            SaveData();
        }

        void ListViewContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (!(SelectedItem?.Tag is PacienteApiModel paciente))
            {
                e.Cancel = true;
                return;
            }

            RestaurarToolStripMenuItem.Enabled = paciente.SetAsDeleted;
            ModificarToolStripMenuItem.Enabled = !paciente.SetAsDeleted;

            VistoToolStripMenuItem.CheckState = paciente.Account.Visto ?
                CheckState.Checked : CheckState.Unchecked;

            SeguimientoToolStripMenuItem.CheckState = paciente.Account.Seguimiento ?
                CheckState.Checked : CheckState.Unchecked;
        }

        void VistoToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            var paciente = SelectedItem.Tag as PacienteApiModel;

            if (paciente.Account.Visto == VistoToolStripMenuItem.Checked)
                return;

            paciente.Account.Visto = VistoToolStripMenuItem.Checked;

            var fontStyle = VistoToolStripMenuItem.Checked ?
                SelectedItem.Font.Style ^ FontStyle.Bold :
                SelectedItem.Font.Style | FontStyle.Bold;

            SelectedItem.Font = new Font(SelectedItem.Font, fontStyle);

            SaveData();
        }

        void SeguimientoToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            var paciente = SelectedItem.Tag as PacienteApiModel;

            if (paciente.Account.Seguimiento == SeguimientoToolStripMenuItem.Checked)
                return;

            paciente.Account.Seguimiento = SeguimientoToolStripMenuItem.Checked;

            var fontStyle = SeguimientoToolStripMenuItem.Checked ?
                SelectedItem.Font.Style | FontStyle.Underline :
                SelectedItem.Font.Style ^ FontStyle.Underline;

            SelectedItem.Font = new Font(SelectedItem.Font, fontStyle);

            SaveData();
        }

        void PropiedadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckedToolStripMenuItem = sender as ToolStripMenuItem;

            if (CheckedToolStripMenuItem.Checked)
                return;

            foreach (var propiedadtoolStripMenuItem in PropiedadtoolStripMenuItems)
                propiedadtoolStripMenuItem.Checked = false;

            CheckedToolStripMenuItem.Checked = true;            

            foreach (var item in ListView.Items)
            {
                var listViewItem = item as ListViewItem;
                var paciente = listViewItem.Tag as PacienteApiModel;
                listViewItem.SubItems[1].Text =
                    CheckedToolStripMenuItem == IdentificadorToolStripMenuItem ? paciente.HistoriaClinica :
                    CheckedToolStripMenuItem == HospitalToolStripMenuItem ? paciente.HospitalNombre :
                    CheckedToolStripMenuItem == ProvinciaToolStripMenuItem ? paciente.ProvinciaNombreCorto :
                    CheckedToolStripMenuItem == HospitalProvinciaToolStripMenuItem ? paciente.HospitalProvinciaNombreCorto :
                    paciente.EstadiaHospitalaria.ToString();
            }

            ListView.Columns[1].Text =
                CheckedToolStripMenuItem == IdentificadorToolStripMenuItem ? "Id" :
                CheckedToolStripMenuItem == HospitalToolStripMenuItem ? "Hospital" :
                CheckedToolStripMenuItem == ProvinciaToolStripMenuItem ? "Provincia" :
                CheckedToolStripMenuItem == HospitalProvinciaToolStripMenuItem ? "Hosp P." :
                "Estadía";

            //SaveData();
        }

        #endregion


        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
            => Application.Exit();

        private void TabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //if (Estado != Estado.None)
            //{
            //    e.Cancel = true;
            //    System.Media.SystemSounds.Hand.Play();
            //}
        }

        private void TabControl_Selected(object sender, TabControlEventArgs e)
        {
            switch (e.TabPage.Name)
            {
                case nameof(PacientesTabPage): Operation = Operation.Paciente; break;
                case nameof(BusquedaTabPage): Operation = Operation.Busqueda; break;
            }
                //!= nameof(PacientesTabPage) && tabPage.Name != nameof(BusquedaTabPage))
                //TabControl.TabPages.Remove(tabPage);
        }

        private void OpcionesConexionCheckBox_CheckedChanged(object sender, EventArgs e)
            => OpcionesConexionGroupBox.Enabled = OpcionesConexionCheckBox.Checked;

        private void ProxyCheckBox_CheckedChanged(object sender, EventArgs e)
            => ProxyGroupBox.Enabled = ProxyCheckBox.Checked;

        private void ServidorCheckBox_CheckedChanged(object sender, EventArgs e)
            => ServidorGroupBox.Enabled = ServidorCheckBox.Checked;

        private void ContrasenaConexionToolStripButton_Click(object sender, EventArgs e)
        {
            if (ConexionProgressBar.Style == ProgressBarStyle.Marquee)
                ConexionProgressBar.Style = ProgressBarStyle.Continuous;
            else
                ConexionProgressBar.Style = ProgressBarStyle.Marquee;
        }

        void ProxyConexionToolStripButton_Click(object sender, EventArgs e)
            => ConexionProgressBar.Visible = !ConexionProgressBar.Visible;

        void AcercaDeToolStripButton_Click(object sender, EventArgs e)
            => new AboutBox().ShowDialog();

        void AyudaToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                var path = Path.Combine(Environment.CurrentDirectory, AyudaFileName);

                Process.Start(new ProcessStartInfo(path)
                {
                    Verb = "open",
                    UseShellExecute = true
                });
            }
            catch
            {
                MessageBox.Show("El manual de usuario no se encuentra o está dañado.", "Halo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
