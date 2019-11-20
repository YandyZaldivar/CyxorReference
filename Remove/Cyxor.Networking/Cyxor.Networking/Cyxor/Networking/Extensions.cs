/*
  { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/>
  Copyright (C) 2017  Yandy Zaldivar

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU Affero General Public License as
  published by the Free Software Foundation, either version 3 of the
  License, or (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU Affero General Public License for more details.

  You should have received a copy of the GNU Affero General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Net.Security;
using System.Threading.Tasks;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;


using System.Linq;
using System.Linq.Expressions;

namespace Cyxor.Extensions
{
    using Networking;
    using Serialization;

    public static class MutexExtensions
    {
#if NET35
        public static void Dispose(this Mutex mutex) { }
#endif
    }



    public static class SerializerExtensions
    {
        public static void LoadFromFile(this Serializer serializer, string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var fileInfo = new FileInfo(filePath);

                if (fileInfo.Length > Serializer.MaxCapacity)
                    throw new EndOfStreamException();

                var fileLength = (int)fileInfo.Length;

                serializer.Position = 0;
                serializer.EnsureCapacity(fileLength);

                var offset = 0;

                while (offset != fileLength)
                    offset += stream.Read(serializer.Buffer, offset, fileLength - offset);
            }
        }

        public static void SaveToFile(this Serializer serializer, string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                if (serializer.Buffer != null)
                {
                    stream.Write(serializer.Buffer, 0, serializer.Length);
                    stream.Flush();
                }
        }

#if NET35 || NET40

        public static Task LoadFromFileAsync(this Serializer serializer, string filePath) => Task.Factory.StartNew(() => serializer.LoadFromFile(filePath));
        public static Task SaveToFileAsync(this Serializer serializer, string filePath) => Task.Factory.StartNew(() => serializer.SaveToFile(filePath));

#else

        public static async Task LoadFromFileAsync(this Serializer serializer, string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var fileInfo = new FileInfo(filePath);

                if (fileInfo.Length > Serializer.MaxCapacity)
                    throw new EndOfStreamException();

                var fileLength = (int)fileInfo.Length;

                serializer.Position = 0;
                serializer.EnsureCapacity(fileLength);

                var offset = 0;

                while (offset != fileLength)
                    offset += await stream.ReadAsync(serializer.Buffer, 0, fileLength).ConfigureAwait(false);
            }
        }

        public static async Task SaveToFileAsync(this Serializer serializer, string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                if (serializer.Buffer != null)
                {
                    await stream.WriteAsync(serializer.Buffer, 0, serializer.Length).ConfigureAwait(false);
                    await stream.FlushAsync().ConfigureAwait(false);
                }
        }

#endif
    }

    public static class MiscExtensions
    {
        public static void Reset(this Serializer serializer, Node node)
        {
            serializer.Reset(node.Config.IOBufferSize);

            if (serializer.Buffer == null)
                serializer.SetCapacity(node.Config.IOBufferSize);
        }

#if NET35
        internal static void Dispose(this System.Net.Sockets.Socket socket)
            => socket.Close();

        internal static void Clear(this System.Text.StringBuilder stringBuilder)
        {
            if (stringBuilder.Length > 0)
                stringBuilder.Remove(0, stringBuilder.Length);
        }
#endif

#if NET35 || NET40
        internal static void CancelAfter(this CancellationTokenSource cancellationTokenSource, int millisecondsTimeout)
        {
            var timer = default(Timer);

            timer = new Timer(delegate
            {
                cancellationTokenSource.Cancel();
                timer.Dispose();
            }, state: null, dueTime: millisecondsTimeout, period: Timeout.Infinite);
        }

        internal static Task WriteLineAsync(this TextWriter stream, string value) => Networking.Utilities.Task.Run(() => stream.WriteLine(value));

        internal static Task<string> ReadLineAsync(this TextReader stream) => Networking.Utilities.Task.Run(() => stream.ReadLine());

        internal static Task ReadAsync(this Stream stream, byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object>();
            stream.BeginRead(buffer, offset, count, iar =>
            {
                try
                {
                    stream.EndRead(iar);
                    tcs.TrySetResult(result: null);
                }
                catch (OperationCanceledException) { tcs.TrySetCanceled(); }
                catch (Exception exc) { tcs.TrySetException(exc); }
            }, null);
            return tcs.Task;
        }


        internal static Task AuthenticateAsClientAsync(this SslStream sslStream, string targetHost, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
        {
            var tcs = new TaskCompletionSource<object>();
            sslStream.BeginAuthenticateAsClient(targetHost, clientCertificates, enabledSslProtocols, checkCertificateRevocation, iar =>
            {
                try
                {
                    sslStream.EndAuthenticateAsClient(iar);
                    tcs.TrySetResult(result: null);
                }
                catch (OperationCanceledException) { tcs.TrySetCanceled(); }
                catch (Exception exc) { tcs.TrySetException(exc); }
            }, null);
            return tcs.Task;
        }

        internal static Task AuthenticateAsServerAsync(this SslStream sslStream, X509Certificate serverCertificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
        {
            var tcs = new TaskCompletionSource<object>();
            sslStream.BeginAuthenticateAsServer(serverCertificate, clientCertificateRequired, enabledSslProtocols, checkCertificateRevocation, iar =>
            {
                try
                {
                    sslStream.EndAuthenticateAsClient(iar);
                    tcs.TrySetResult(result: null);
                }
                catch (OperationCanceledException) { tcs.TrySetCanceled(); }
                catch (Exception exc) { tcs.TrySetException(exc); }
            }, null);
            return tcs.Task;
        }
#endif

#if !NETSTANDARD1_3 && !UAP10_0
        internal static void Dispose(this X509Certificate certificate) => certificate.Dispose();
#endif
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
