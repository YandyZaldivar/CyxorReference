/*
  { Frameview } - Sistema de videoconferencia por imágenes
  Copyright (C) 2018 Alimatic
  Authors:  Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Frameview.Controllers
{
    using Cyxor.Models;
    using Cyxor.Networking;
    using Cyxor.Controllers;

    [Model("camera send")]
    class Frame
    {
        public string Name { get; set; }
        public byte[] Bytes { get; set; }
    }

    class CameraController : Controller
    {
        static Network Network => Network.Instance;
        static MainForm MainForm => MainForm.Instance;

        static int SendDelay = 80;

        public static async void Send()
        {
            while (!Network.IsMaster && !Network.Active)
                await Utilities.Task.Delay(SendDelay);

            var bitmap = new Bitmap(MainForm.LeftTableLayoutPanel.Width, MainForm.LeftTableLayoutPanel.Height);
            MainForm.LeftTableLayoutPanel.DrawToBitmap(bitmap, MainForm.LeftTableLayoutPanel.Bounds);
            var thumbnail = bitmap.GetThumbnailImage(Network.Config.FrameWidth, Network.Config.FrameHeight, delegate { return false; }, IntPtr.Zero);

            var stream = new MemoryStream();
            thumbnail.Save(stream, ImageFormat.Jpeg);

            thumbnail.Dispose();
            bitmap.Dispose();

            var frame = new Frame
            {
                Name = MainForm.ClientName,
                Bytes = stream.ToArray(),
            };

            using (var packet = new Packet(Network.Instance, "camera send", frame))
                if (!await packet.QueryAsync())
                {
                    // TODO: Handle the case when the server goes offline

                    if (Network.Active)
                        MessageBox.Show($"{packet.Response.Result}", nameof(Frameview), MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return;
                }

            await Utilities.Task.Delay(SendDelay);

            Send();
        }

        public void Send(Frame frame)
        {
            var newBitmap = (Bitmap)Image.FromStream(new MemoryStream(frame.Bytes));

            if (MainForm.ClientFrameBoxes.TryGetValue(frame.Name, out var frameBox))
            {
                var oldBitmap = frameBox.Bitmap;
                frameBox.Bitmap = newBitmap;
                frameBox.Picture.Invalidate();
                oldBitmap?.Dispose();
            }
            else
            {
                frameBox = new FrameBox(frame.Name) { Bitmap = newBitmap };

                MainForm.BottomTableLayoutPanel.Controls.Add(frameBox.Picture);

                if (MainForm.BottomTableLayoutPanel.Controls.Count > 0)
                    MainForm.BottomTableLayoutPanel.Controls.Remove(MainForm.FramePanel);

                MainForm.BottomTableLayoutPanel.Controls.Add(MainForm.FramePanel);

                MainForm.ClientFrameBoxes.TryAdd(frame.Name, frameBox);

                if (!Network.IsMaster && !Network.Active)
                    Network.Active = true;
            }

            if (!Network.IsMaster)
            {
                Network.Config.FrameWidth = frameBox.Bitmap.Width;
                Network.Config.FrameHeight = frameBox.Bitmap.Height;
            }
        }

        public void Leave(string name)
        {
            MainForm.ClientFrameBoxes.TryRemove(name, out var frameBox);

            MainForm.BottomTableLayoutPanel.Controls.Remove(frameBox.Picture);

            if (MainForm.BottomTableLayoutPanel.Controls.Count == 1)
                MainForm.BottomTableLayoutPanel.Controls.Remove(MainForm.FramePanel);

            frameBox.Dispose();

            if (!Network.IsMaster)
                Network.Active = false;
        }
    }
}
/* { Frameview } - Sistema de videoconferencia por imágenes */
