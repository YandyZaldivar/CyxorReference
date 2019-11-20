using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Touchless.Vision.Camera;

namespace Frameview
{
    using Controllers;
    using Cyxor.Networking;

    public partial class MainForm : Form
    {
        static Network Network => Network.Instance;

        public static MainForm Instance { get; private set; }

        public static Camera NullCamera { get; set; }

        public List<CameraFrameSource> FrameSources { get; set; }

        public List<FrameBox> LeftFrameBoxes { get; set; }
        public List<FrameBox> RightFrameBoxes { get; set; }
        public Panel FramePanel = new Panel { Dock = DockStyle.Fill };
        public ConcurrentDictionary<string, FrameBox> ClientFrameBoxes { get; set; }

        public List<ToolStripMenuItem> LeftChannelToolStripMenuItems { get; set; }
        public List<ToolStripComboBox> LeftChannelToolStripComboBoxes { get; set; }
        public List<ToolStripMenuItem> LeftActiveChannelToolStripMenuItems { get; set; }

        public bool SelectionDisable { get; set; }

        Bitmap LBitmap;
        Timer Timer;

        //public Size ImageSize { get; set; } = new Size(240, 160);

        public string ClientName { get; set; } = "Master";

        static MainForm()
        {
            NullCamera = new Camera(new WebCamLib.CameraMethods(), "", -1);
        }

        public MainForm()
        {
            InitializeComponent();

            Instance = this;
            CameraController.Send();

            var initForm = new InitForm();
            if (initForm.ShowDialog() != DialogResult.OK)
            {

            }

            BottomTableLayoutPanel.ColumnCount = 0;

            ClientFrameBoxes = new ConcurrentDictionary<string, FrameBox>();

            LeftFrameBoxes = new List<FrameBox>
            {
                new FrameBox("1"),
                new FrameBox("2"),
                new FrameBox("3"),
                new FrameBox("4"),
            };

            RightFrameBoxes = new List<FrameBox>
            {
                new FrameBox("1"),
                new FrameBox("2"),
                new FrameBox("3"),
                new FrameBox("4"),
            };

            LeftChannelToolStripMenuItems = new List<ToolStripMenuItem>
            {
                LeftChannelOneToolStripMenuItem,
                LeftChannelTwoToolStripMenuItem,
                LeftChannelThreeToolStripMenuItem,
                LeftChannelFourToolStripMenuItem,
            };

            LeftChannelToolStripComboBoxes = new List<ToolStripComboBox>
            {
                LeftChannelOneDeviceToolStripComboBox,
                LeftChannelTwoDeviceToolStripComboBox,
                LeftChannelThreeDeviceToolStripComboBox,
                LeftChannelFourDeviceToolStripComboBox,
            };

            LeftChannelOneDeviceToolStripComboBox.Tag = 1;
            LeftChannelTwoDeviceToolStripComboBox.Tag = 2;
            LeftChannelThreeDeviceToolStripComboBox.Tag = 3;
            LeftChannelFourDeviceToolStripComboBox.Tag = 4;

            foreach (var item in LeftChannelToolStripComboBoxes)
            {
                item.DropDown += LeftChannelDeviceToolStripComboBox_DropDown;
                item.SelectedIndexChanged += LeftChannelDeviceToolStripComboBox_SelectedIndexChanged;
            }

            LeftActiveChannelToolStripMenuItems = new List<ToolStripMenuItem>
            {
                LeftActivateChannelOneToolStripMenuItem,
                LeftActivateChannelTwoVerticalToolStripMenuItem,
                LeftActivateChannelTwoHorizontalToolStripMenuItem,
                LeftActivateChannelThreeTopToolStripMenuItem,
                LeftActivateChannelThreeLeftToolStripMenuItem,
                LeftActivateChannelThreeRightToolStripMenuItem,
                LeftActivateChannelThreeBottomToolStripMenuItem,
                LeftActivateChannelFourToolStripMenuItem,
            };

            foreach (var item in LeftActiveChannelToolStripMenuItems)
                item.CheckedChanged += LeftActivateChannelToolStripMenuItem_CheckedChanged;

            FrameSize100ToolStripMenuItem.Click += BottomTableColumnSizeToolStripItem_Click;
            FrameSize150ToolStripMenuItem.Click += BottomTableColumnSizeToolStripItem_Click;
            FrameSize200ToolStripMenuItem.Click += BottomTableColumnSizeToolStripItem_Click;
            FrameSize250ToolStripMenuItem.Click += BottomTableColumnSizeToolStripItem_Click;
            FrameSize300ToolStripMenuItem.Click += BottomTableColumnSizeToolStripItem_Click;
            FrameSize350ToolStripMenuItem.Click += BottomTableColumnSizeToolStripItem_Click;
            FrameSize400ToolStripMenuItem.Click += BottomTableColumnSizeToolStripItem_Click;
            FrameSize450ToolStripMenuItem.Click += BottomTableColumnSizeToolStripItem_Click;
            FrameSize500ToolStripMenuItem.Click += BottomTableColumnSizeToolStripItem_Click;

            LeftTableLayoutPanel.CellPaint += LeftTableLayoutPanel_CellPaint;

            SimulationActiveToolStripMenuItem.CheckedChanged += (s, e) =>
            {
                if (!SimulationActiveToolStripMenuItem.Checked)
                    SimulationActiveToolStripMenuItem.Text = "Inactivo";
                else
                {
                    SimulationActiveToolStripMenuItem.Text = "Activo";


                }
            };

            LeftActivateChannelToolStripMenuItem_CheckedChanged(LeftActivateChannelOneToolStripMenuItem, EventArgs.Empty);

            Timer = new Timer { Interval = 500 };
            LBitmap = new Bitmap(200, 200);

            Timer.Tick += (s, e) =>
            {
                LBitmap = new Bitmap(LeftTableLayoutPanel.Width, LeftTableLayoutPanel.Height);
                LeftTableLayoutPanel.DrawToBitmap(LBitmap, LeftTableLayoutPanel.Bounds);
                pictureBox1.Invalidate();
            };

            Timer.Start();
            pictureBox1.Paint += (s, e) =>
            {
                var thumbnail = LBitmap.GetThumbnailImage(Network.Config.FrameWidth, Network.Config.FrameHeight, delegate { return false; }, IntPtr.Zero);

                var st = new MemoryStream();
                thumbnail.Save(st, ImageFormat.Jpeg);
                //Text = (st.Length / 1024.0).ToString() + "KB";

                e.Graphics.DrawImage(thumbnail, e.ClipRectangle);
            };
        }

        void ConectarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                ClientName = loginForm.ClientName;
            }
        }

        async void DesconectarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DesconectarToolStripMenuItem.Enabled = false;

            if (Network.IsConnected)
            {
                Network.Active = false;
                await Network.DisconnectAsync();
            }
        }

        void LeftTableContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (Network.IsConnected)
            {
                ConectarToolStripMenuItem.Enabled = false;
                DesconectarToolStripMenuItem.Enabled = true;
            }
            else
            {
                ConectarToolStripMenuItem.Enabled = true;
                DesconectarToolStripMenuItem.Enabled = false;
            }
        }

        private void LeftTableLayoutPanel_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {

        }

        private async void LeftChannelDeviceToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectionDisable)
                return;

            var ts = sender as ToolStripComboBox;
            var frameBox = LeftFrameBoxes.Single(p => p.Id == ts.Tag.ToString());

            if (ts.SelectedItem == NullCamera)
                frameBox.Release();
            else if (ts.SelectedItem != null)
                await frameBox.InitAsync(ts);
        }

        void LeftChannelDeviceToolStripComboBox_DropDown(object sender, EventArgs e)
        {
            SelectionDisable = true;

            var availableCameras = new List<Camera>(CameraService.AvailableCameras.Count);

            foreach (var camera in CameraService.AvailableCameras)
            {
                var available = true;

                foreach (var cb in LeftChannelToolStripComboBoxes)
                    if (sender != cb)
                        if (camera == cb.SelectedItem)
                            available = false;

                if (available)
                    availableCameras.Add(camera);
            }

            var ts = sender as ToolStripComboBox;
            var selectedCamera = ts.SelectedItem as Camera;

            ts.Items.Clear();

            if (availableCameras.Count > 0)
            {
                ts.Items.Add(NullCamera);

                foreach (var camera in availableCameras)
                    ts.Items.Add(camera);
            }

            if (selectedCamera != null)
                ts.SelectedItem = selectedCamera;

            SelectionDisable = false;
        }

        void BottomTableColumnSizeToolStripItem_Click(object sender, EventArgs e)
        {
            foreach (var columnStyle in BottomTableLayoutPanel.ColumnStyles)
            {
                var column = columnStyle as ColumnStyle;
                column.SizeType = SizeType.Absolute;
                column.Width = int.Parse((sender as ToolStripItem).Tag.ToString());
            }
        }

        void LeftActivateChannelToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            var ts = sender as ToolStripMenuItem;

            if (ts.Checked)
            {
                foreach (var item in LeftChannelToolStripMenuItems)
                    if (item != LeftChannelOneToolStripMenuItem)
                        item.Enabled = false;

                foreach (var frameBox in LeftFrameBoxes)
                {
                    var control = frameBox.Picture;

                    if (LeftTableLayoutPanel.GetRowSpan(control) > 1)
                    {
                        var row = LeftTableLayoutPanel.GetRow(control);
                        LeftTableLayoutPanel.SetRowSpan(control, 1);
                    }

                    if (LeftTableLayoutPanel.GetColumnSpan(control) > 1)
                    {
                        var column = LeftTableLayoutPanel.GetColumn(control);
                        LeftTableLayoutPanel.SetColumnSpan(control, 1);
                    }

                    LeftTableLayoutPanel.Controls.Remove(control);
                }

                if (ts == LeftActivateChannelOneToolStripMenuItem)
                {
                    LeftTableLayoutPanel.RowCount = 1;
                    LeftTableLayoutPanel.ColumnCount = 1;

                    foreach (var cb in LeftChannelToolStripComboBoxes)
                        if (cb != LeftChannelOneDeviceToolStripComboBox)
                            cb.Items.Clear();

                    LeftTableLayoutPanel.Controls.Add(LeftFrameBoxes.Single(p => p.Id == "1").Picture, 0, 0);
                }
                else if (ts == LeftActivateChannelTwoVerticalToolStripMenuItem)
                {
                    LeftTableLayoutPanel.RowCount = 1;
                    LeftTableLayoutPanel.ColumnCount = 2;
                    LeftChannelTwoToolStripMenuItem.Enabled = true;

                    LeftChannelThreeDeviceToolStripComboBox.Items.Clear();
                    LeftChannelFourDeviceToolStripComboBox.Items.Clear();

                    LeftTableLayoutPanel.Controls.Add(LeftFrameBoxes.Single(p => p.Id == "1").Picture, 0, 0);
                    LeftTableLayoutPanel.Controls.Add(LeftFrameBoxes.Single(p => p.Id == "2").Picture, 1, 0);
                }
                else if (ts == LeftActivateChannelTwoHorizontalToolStripMenuItem)
                {
                    LeftTableLayoutPanel.RowCount = 2;
                    LeftTableLayoutPanel.ColumnCount = 1;
                    LeftChannelTwoToolStripMenuItem.Enabled = true;

                    LeftChannelThreeDeviceToolStripComboBox.Items.Clear();
                    LeftChannelFourDeviceToolStripComboBox.Items.Clear();

                    LeftTableLayoutPanel.Controls.Add(LeftFrameBoxes.Single(p => p.Id == "1").Picture, 0, 0);
                    LeftTableLayoutPanel.Controls.Add(LeftFrameBoxes.Single(p => p.Id == "2").Picture, 0, 1);
                }
                else if (ts == LeftActivateChannelThreeTopToolStripMenuItem)
                {
                    LeftTableLayoutPanel.RowCount = 2;
                    LeftTableLayoutPanel.ColumnCount = 2;
                    LeftChannelTwoToolStripMenuItem.Enabled = true;
                    LeftChannelThreeToolStripMenuItem.Enabled = true;

                    LeftChannelFourDeviceToolStripComboBox.Items.Clear();

                    var spanControl = LeftFrameBoxes.Single(p => p.Id == "3").Picture;

                    LeftTableLayoutPanel.Controls.Add(spanControl, 0, 0);
                    LeftTableLayoutPanel.Controls.Add(LeftFrameBoxes.Single(p => p.Id == "1").Picture, 0, 1);
                    LeftTableLayoutPanel.Controls.Add(LeftFrameBoxes.Single(p => p.Id == "2").Picture, 1, 1);

                    LeftTableLayoutPanel.SetColumnSpan(spanControl, 2);
                }
                else if (ts == LeftActivateChannelThreeLeftToolStripMenuItem)
                {
                    LeftTableLayoutPanel.RowCount = 2;
                    LeftTableLayoutPanel.ColumnCount = 2;
                    LeftChannelTwoToolStripMenuItem.Enabled = true;
                    LeftChannelThreeToolStripMenuItem.Enabled = true;

                    LeftChannelFourDeviceToolStripComboBox.Items.Clear();

                    var spanControl = LeftFrameBoxes.Single(p => p.Id == "3").Picture;

                    LeftTableLayoutPanel.Controls.Add(spanControl, 1, 0);
                    LeftTableLayoutPanel.Controls.Add(LeftFrameBoxes.Single(p => p.Id == "1").Picture, 0, 0);
                    LeftTableLayoutPanel.Controls.Add(LeftFrameBoxes.Single(p => p.Id == "2").Picture, 0, 1);

                    LeftTableLayoutPanel.SetRowSpan(spanControl, 2);
                }
                else if (ts == LeftActivateChannelThreeRightToolStripMenuItem)
                {
                    LeftTableLayoutPanel.RowCount = 2;
                    LeftTableLayoutPanel.ColumnCount = 2;
                    LeftChannelTwoToolStripMenuItem.Enabled = true;
                    LeftChannelThreeToolStripMenuItem.Enabled = true;

                    LeftChannelFourDeviceToolStripComboBox.Items.Clear();

                    var spanControl = LeftFrameBoxes.Single(p => p.Id == "3").Picture;

                    LeftTableLayoutPanel.Controls.Add(spanControl, 0, 0);
                    LeftTableLayoutPanel.Controls.Add(LeftFrameBoxes.Single(p => p.Id == "1").Picture, 1, 0);
                    LeftTableLayoutPanel.Controls.Add(LeftFrameBoxes.Single(p => p.Id == "2").Picture, 1, 1);

                    LeftTableLayoutPanel.SetRowSpan(spanControl, 2);
                }
                else if (ts == LeftActivateChannelThreeBottomToolStripMenuItem)
                {
                    LeftTableLayoutPanel.RowCount = 2;
                    LeftTableLayoutPanel.ColumnCount = 2;
                    LeftChannelTwoToolStripMenuItem.Enabled = true;
                    LeftChannelThreeToolStripMenuItem.Enabled = true;

                    LeftChannelFourDeviceToolStripComboBox.Items.Clear();

                    var spanControl = LeftFrameBoxes.Single(p => p.Id == "3").Picture;

                    LeftTableLayoutPanel.Controls.Add(spanControl, 0, 1);
                    LeftTableLayoutPanel.Controls.Add(LeftFrameBoxes.Single(p => p.Id == "1").Picture, 0, 0);
                    LeftTableLayoutPanel.Controls.Add(LeftFrameBoxes.Single(p => p.Id == "2").Picture, 1, 0);

                    LeftTableLayoutPanel.SetColumnSpan(spanControl, 2);
                }
                else if (ts == LeftActivateChannelFourToolStripMenuItem)
                {
                    LeftTableLayoutPanel.RowCount = 2;
                    LeftTableLayoutPanel.ColumnCount = 2;

                    foreach (var item in LeftChannelToolStripMenuItems)
                        item.Enabled = true;

                    LeftTableLayoutPanel.Controls.Add(LeftFrameBoxes.Single(p => p.Id == "1").Picture, 0, 0);
                    LeftTableLayoutPanel.Controls.Add(LeftFrameBoxes.Single(p => p.Id == "2").Picture, 1, 0);
                    LeftTableLayoutPanel.Controls.Add(LeftFrameBoxes.Single(p => p.Id == "3").Picture, 0, 1);
                    LeftTableLayoutPanel.Controls.Add(LeftFrameBoxes.Single(p => p.Id == "4").Picture, 1, 1);
                }

                foreach (var item in LeftActiveChannelToolStripMenuItems)
                    if (item != ts)
                        item.Checked = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void twoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            var random = new Random();
            var value = random.Next(MainForm.Instance.BottomTableLayoutPanel.Controls.Count - 1);

            var c = 0;
            var control = default(Control);

            foreach (var item in BottomTableLayoutPanel.Controls)
            {
                if (value == c)
                {
                    control = item as Control;
                    break;
                }

                c++;
            }

            MainForm.Instance.BottomTableLayoutPanel.Controls.Remove(control);

            if (BottomTableLayoutPanel.Controls.Count == 1)
            {
                MainForm.Instance.BottomTableLayoutPanel.Controls.Remove(Panel);
                return;
            }
            */
        }

        private void oneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            //var bitmap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            var bitmap = (Bitmap)Image.FromFile("D:/Pictures/Frameview.jpg");
            //DrawToBitmap(bitmap, ClientRectangle);
            var frameBox = new FrameBox(DateTime.Now.ToString()) { Bitmap = bitmap };


            ////frameBox.Picture.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

            //frameBox.Picture.Width = 300;
            //frameBox.Picture.Height = 200;
            //frameBox.Picture.Dock = DockStyle.None;


            MainForm.Instance.BottomTableLayoutPanel.Controls.Add(frameBox.Picture);

            if (MainForm.Instance.BottomTableLayoutPanel.Controls.Count > 0)
                MainForm.Instance.BottomTableLayoutPanel.Controls.Remove(Panel);

            MainForm.Instance.BottomTableLayoutPanel.Controls.Add(Panel);



            //MainForm.Instance.BottomTableLayoutPanel.Controls.Add(frameBox.Picture, MainForm.Instance.BottomTableLayoutPanel.ColumnCount - 1, 0);
            //MainForm.Instance.BottomTableLayoutPanel.ColumnCount++;

            foreach (var columnStyle in BottomTableLayoutPanel.ColumnStyles)
            {
                var column = columnStyle as ColumnStyle;
                column.SizeType = SizeType.Absolute;
                column.Width = 225;
            }

            //BottomTableLayoutPanel.ColumnCount = 10;
            //button1.Text = BottomTableLayoutPanel.ColumnStyles.Count.ToString();
            */
        }

        private void BottomTableLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Network.IsConnected)
            {
                e.Cancel = true;
                Text = "Closing...";
                Network.Active = false;
                await Network.DisconnectAsync();
                Close();
            }
        }

        void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
