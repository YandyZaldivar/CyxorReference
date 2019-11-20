/*
  { Framesrc } - Sistema de videoconferencia por imágenes
  Copyright (C) 2018 Alimatic
  Authors:  Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;

using Touchless.Vision.Camera;
using Touchless.Vision.Contracts;

namespace Framesrc
{
    using Cyxor.Networking;
    using Cyxor.Controllers;
    using Cyxor.Networking.Config;

    class Program
    {
        static void Main(string[] args)
        {
            var network = Client.Instance;

            network.Events.DisconnectCompleted += (s, e) => Environment.Exit(0);
            network.Config.AuthenticationMode = AuthenticationSchema.Basic;
            network.Config.Port = 29540;

            Utilities.Task.Run(async () =>
            {
                var result = Result.Success;

                if (!(result = await network.ConnectAsync()))
                {
                    File.WriteAllText($"{nameof(Framesrc)}.txt", result.ToString());
                    Environment.Exit(0);
                }
            });

            Console.ReadKey(intercept: true);
        }

        public class CameraParams
        {
            public int? Fps { get; set; }
            public int? Bpp { get; set; }
            public int? Width { get; set; }
            public int? Height { get; set; }
            public string Name { get; set; }
        }

        class CameraController : Controller
        {
            Bitmap bitmap;
            Bitmap Bitmap;
            CameraFrameSource FrameSource;

            public IEnumerable<object> Sizes()
            {
                FrameSource?.StopFrameCapture();
                var sizes = FrameSource?.Camera?.CaptureSizes.Select(p => new { p.Width, p.Height, p.ColorDepth });
                FrameSource?.StartFrameCapture();
                return sizes;
            }

            public IEnumerable<string> List() => CameraService.AvailableCameras.Select(p => p.Name);

            public byte[] Get()
            {
                var currentBitmap = Bitmap;

                if (FrameSource is var fs && currentBitmap != null && currentBitmap != bitmap)
                {
                    bitmap?.Dispose();
                    bitmap = currentBitmap;

                    var stream = new MemoryStream();
                    bitmap.Save(stream, ImageFormat.Jpeg);
                    //bitmap.Save($"{fs.Camera.Name}.jpeg", ImageFormat.Jpeg);

                    return stream.ToArray();
                }

                return null;
            }

            public void Set(CameraParams cameraParams)
            {
                var camera = cameraParams.Name == null ? FrameSource?.Camera ?? CameraService.DefaultCamera :
                    CameraService.AvailableCameras.Single(p => p.Name == cameraParams.Name);

                var previousCamera = FrameSource?.Camera;

                FrameSource?.StopFrameCapture();

                if (previousCamera != camera)
                {
                    if (FrameSource != null)
                    {
                        FrameSource?.StopFrameCapture();
                        FrameSource.NewFrame -= FrameSourceNewFrame;
                        FrameSource?.Camera?.Dispose();
                    }

                    FrameSource = new CameraFrameSource(camera);
                }

                FrameSource.Camera.Fps = cameraParams.Fps ?? FrameSource.Camera.Fps;
                FrameSource.Camera.CaptureWidth = cameraParams.Width ?? FrameSource.Camera.CaptureWidth;
                FrameSource.Camera.CaptureHeight = cameraParams.Height ?? FrameSource.Camera.CaptureHeight;
                FrameSource.Camera.CaptureBitsPerPixel = cameraParams.Bpp ?? FrameSource.Camera.CaptureBitsPerPixel;

                FrameSource.NewFrame += FrameSourceNewFrame;

                FrameSource.StartFrameCapture();
            }

            private void FrameSourceNewFrame(IFrameSource arg1, Frame arg2, double arg3)
                => Bitmap = arg2.Image;
        }
    }
}
/* { Framesrc } - Sistema de videoconferencia por imágenes */
