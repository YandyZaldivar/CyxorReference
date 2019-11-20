using System;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;

using Touchless.Vision.Camera;

namespace Frameview
{
    using Cyxor.Models;
    using Cyxor.Networking;
    using Cyxor.Networking.Events.Server;


    [Model("camera set")]
    public class CameraParams
    {
        public int? Fps { get; set; }
        public int? Bpp { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string Name { get; set; }
    }

    public class FrameBox : IDisposable
    {
        Timer Timer;
        int FontSize = 1;
        Size ControlSize;
        bool NeedSetBlack;
        string FramesrcPath;
        Connection Connection;

        public string Id { get; set; }
        public string Nombre { get; set; }
        public Bitmap Bitmap { get; set; }
        public PictureBox Picture { get; set; }
        public Camera Camera { get; private set; }
        public TableLayoutPanel Table { get; set; }
        public Process Process { get; private set; }
        public ToolStripComboBox ComboBox { get; private set; }

        public FrameBox(string name)
        {
            Id = name;
            Nombre = name;
            FramesrcPath = "Framesrc.exe";
            Picture = new PictureBox { Dock = DockStyle.Fill };

            Picture.Paint += Picture_Paint;

            Timer = new Timer { Interval = 100 };

            Timer.Tick += async (s, e) =>
            {
                using (var packet = new Packet(Connection, "camera get", new EmptyApiModel()))
                {
                    Timer.Enabled = false;
                    await packet.QueryAsync();

                    var bytes = packet.Response.GetModel<byte[]>();

                    if (bytes != null)
                    {
                        Bitmap?.Dispose();
                        Bitmap = Image.FromStream(new MemoryStream(bytes)) as Bitmap;
                        Picture.Invalidate();
                    }

                    Timer.Enabled = true;
                }
            };
        }

        void Picture_Paint(object sender, PaintEventArgs e)
        {
            var bitmap = Bitmap;

            if (bitmap != null)
                e.Graphics.DrawImage(bitmap, e.ClipRectangle);
            else if (NeedSetBlack)
            {
                NeedSetBlack = false;

                bitmap = new Bitmap(e.ClipRectangle.Width, e.ClipRectangle.Height);

                for (var x = 0; x < bitmap.Width; x++)
                    for (var y = 0; y < bitmap.Height; y++)
                        bitmap.SetPixel(x, y, Color.Black);

                e.Graphics.DrawImage(bitmap, e.ClipRectangle);

                bitmap.Dispose();
            }

            var text = Network.Instance.IsConnected ? Nombre : "Sin conexión";
            var brush = Network.Instance.IsConnected ? Brushes.NavajoWhite : Brushes.OrangeRed;
            var font = new Font(FontFamily.GenericSansSerif, FontSize, FontStyle.Bold);
            var textSize = e.Graphics.MeasureString(text, font);
            var controlSize = Picture.Size;
            var w = controlSize.Width - (textSize.Width + 8);
            var h = controlSize.Height - (textSize.Height + 5);
            var factor = w < controlSize.Width / 2 ? -1 : w > controlSize.Width / 2 ? 1 : 0;

            if (factor != 0 && controlSize != ControlSize)
                while ((factor == -1 && w < controlSize.Width / 2) || (factor == 1 && w > controlSize.Width / 2))
                {
                    if (FontSize + factor == 0)
                        break;

                    font?.Dispose();
                    FontSize += factor;
                    font = new Font(FontFamily.GenericSansSerif, FontSize, FontStyle.Bold);
                    textSize = e.Graphics.MeasureString(text, font);

                    w = controlSize.Width - (textSize.Width + 8);
                    h = controlSize.Height - (textSize.Height + 5);
                }

            ControlSize = controlSize;
            e.Graphics.DrawString(text, font, brush, w, h);
        }

        public async Task InitAsync(ToolStripComboBox comboBox)
        {
            try
            {
                Release();

                ComboBox = comboBox;
                Camera = comboBox.SelectedItem as Camera;
                Nombre = Camera.Name;

                var awaitable = new Utilities.Threading.Awaitable();

                Server.Instance.Events.ClientConnected += ClientConnected;

                async void ClientConnected(object sender, ClientConnectedEventArgs e)
                {
                    Connection = e.Connection;

                    var cameraParams = new CameraParams { Name = Camera.Name };

                    using (var packet = new Packet(Connection) { Model = cameraParams })
                        await packet.QueryAsync();

                    awaitable.TrySetResult(Result.Success);
                }

                var pi = new ProcessStartInfo(FramesrcPath)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                };

                Process = Process.Start(pi);

                await awaitable;

                Server.Instance.Events.ClientConnected -= ClientConnected;

                if (!Picture.InvokeRequired)
                    Timer.Enabled = true;
                else
                    Picture.Invoke(new EventHandler((s, e) => Timer.Enabled = true));
            }
            finally
            {
                if (Connection == null)
                {
                    var message = $"The camera process 'Framesrc.exe' can't be started, solve the problem and try again";
                    MessageBox.Show(message, nameof(Frameview), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Environment.Exit(0);
                }
            }
        }

        public void Release()
        {
            ComboBox = null;
            Camera = null;
            Timer.Enabled = false;

            Bitmap?.Dispose();
            Bitmap = null;

            Connection?.DisconnectAsync();
            Connection = null;

            NeedSetBlack = true;
            Picture.Invalidate();
            NeedSetBlack = false;
        }

        public void Dispose() => Release();
    }
}
