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
using System.Net.Sockets;
using System.Net.Security;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace Cyxor.Networking
{
    using Extensions;
    using Serialization;

    using static Utilities.Threading;

    sealed partial class Link
    {
        internal sealed partial class LinkSsl : LinkProperty
        {
            SslStream Stream;
            readonly SslBackendStream BackendStream;

            //ManualResetEventSlim ReaderEvent;

            //int ReadBytesTransferred;

            //ArraySegment<byte> ReadBuffer;
            internal Serializer ReadBuffer;

            SocketAsyncEventArgs SendAsyncSaea;
            SocketAsyncEventArgs ReceiveAsyncSaea;

            //int BytesConsumed;

            internal int BytesBuffered;
            internal int BytesProcessed;

            internal bool IsAuthenticated => Stream?.IsAuthenticated ?? false;

//#if !NET35 && !NET40
            Awaitable ReaderAwaitable = new Awaitable();
//#else
//            ManualResetEventSlim ReaderEvent = new ManualResetEventSlim();
//#endif

            internal LinkSsl(Link link) : base(link)
            {
                ReadBuffer = link.Node.Pools.PopBuffer();
                //ReaderEvent = new ManualResetEventSlim();
                BackendStream = new SslBackendStream(this);
            }

            /*
            //internal void SignalReader(int bytesTransferred)
            internal void SignalReader(SocketAsyncEventArgs saea)
            {
                //ReadBytesTransferred = bytesTransferred;

                //#if NET35 || NET40
                //                ReaderEvent.Set();
                //#else
                //                ReaderAwaitable.TrySetResult(Result.Success);
                //#endif

                ReceiveAsyncSaea = saea;
                ReaderEvent.Set();
            }
            */

            //internal void PrepareReadBuffer(SocketAsyncEventArgs saea) =>
            //    saea.SetBuffer(ReadBuffer.Array, ReadBuffer.Offset, ReadBuffer.Count);

            internal void Write(SocketAsyncEventArgs saea)
            {
                SendAsyncSaea = saea;
                Stream.Write(SendAsyncSaea.Buffer, SendAsyncSaea.Offset, SendAsyncSaea.Count);
            }

            //internal int Read(SocketAsyncEventArgs saea, Serializer serializer)
            internal int Read(SocketAsyncEventArgs saea)
            {
                ReceiveAsyncSaea = saea;

                if (!IsAuthenticated)
                {
                    if (ReceiveAsyncSaea.BytesTransferred > 0)
                        //ReaderEvent.Set();
                        ReaderAwaitable.TrySetResult(Result.Success);
                    else if (ReceiveAsyncSaea.BytesTransferred == 0)
                        Reset();

                    return -1;
                }

                ReadBuffer.EnsureCapacity(saea.BytesTransferred);

                var count = Stream.Read(ReadBuffer.Buffer, ReadBuffer.Int32Position, ReadBuffer.Capacity - ReadBuffer.Int32Position);

                ReadBuffer.SetLength(ReadBuffer.Int32Position + count);
                ReadBuffer.Int32Position = ReadBuffer.Int32Length;

                return BytesProcessed;
            }

            /*
            internal Task<int> ReadAsync(Serializer serializer)
            {
                try
                {
#if !NET35
                    return Stream.ReadAsync(serializer.Buffer, serializer.Position, serializer.Capacity - serializer.Position);
#else
                    return Task.Factory.FromAsync(Stream.BeginRead, Stream.EndRead, serializer.Buffer, serializer.Position, serializer.Capacity - serializer.Position, state: null);
#endif
                }
                catch (Exception ex)
                {
                    var res = 0;

#if !NET35
                    var dt = ex.ToString();
                    if (dt.Length > 0)
                        res = 0;
#else
                    Console.WriteLine(ex.ToString());
#endif

                    return Utilities.Task.FromResult(res);
                }
            }
            */

            bool CertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                var logErrors = sslPolicyErrors != SslPolicyErrors.None;

                if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateNotAvailable)
                {
                    //var server = Node as Server;

                    //if (server != null)
                    //    if (!server.Config.Ssl.ClientCertificateRequired)
                    //        logErrors = false;

                    if (!(Node.AsServer?.Config.Ssl.ClientCertificateRequired ?? false))
                        logErrors = false;
                }

                if (logErrors)
                {
                    var endpoint = Connection.NetworkInformation.RemoteEndPoint?.ToString();
                    Node.Log(LogCategory.Error, "There are SSL policy errors on the remote certificate.");
                    Node.Log(LogCategory.Warning, $"The identity of the remote endpoint [{endpoint}] is not trusted.");
                }

                var validation = Node.Events.Post(new Events.SslCertificateValidatingEventArgs(Connection, certificate, chain, sslPolicyErrors));

                if (!validation.IsCompleted)
                {
                    var validationEvent = new ManualResetEventSlim();

                    new Action(async () =>
                    {
                        await validation.ConfigureAwait(false);
                        validationEvent.Set();
                    }).Invoke();

                    validationEvent.Wait();
                    validationEvent.Dispose();
                }

                return validation.Result ?? true;
            }

            X509Certificate CertificateSelectionCallback(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
            {
                var selection = new Events.SslCertificateSelectingEventArgs(Connection, targetHost, localCertificates, remoteCertificate, acceptableIssuers);

                if (localCertificates.Count > 0)
                    selection.Certificate = localCertificates[0];

                Node.Events.Post(selection);

                if (!selection.IsCompleted)
                {
                    var selectionEvent = new ManualResetEventSlim();

                    new Action(async () =>
                    {
                        await selection.ConfigureAwait(false);
                        selectionEvent.Set();
                    }).Invoke();

                    selectionEvent.Wait();
                    selectionEvent.Dispose();
                }

                return selection.Certificate;
            }

            //internal async Task<Result> ConnectAsync()
            //{
            //    var result = Result.Success;

            //    try
            //    {
            //        Stream = Utilities.SslStream.Create(BackendStream, true, CertificateValidationCallback, CertificateSelectionCallback, Node.Config.Ssl.EncryptionPolicy);

            //        switch (Node.IsClient)
            //        {
            //            case true: await Stream.AuthenticateAsClientAsync(Node.AsClient.Config.Ssl.TargetHost, Node.AsClient.Config.Ssl.CertificateCollection, Node.Config.Ssl.Protocols, Node.Config.Ssl.CheckCertificateRevocation); break;
            //            case false: await Stream.AuthenticateAsServerAsync(Node.AsServer.Config.Ssl.Certificate, Node.AsServer.Config.Ssl.ClientCertificateRequired, Node.Config.Ssl.Protocols, Node.Config.Ssl.CheckCertificateRevocation); break;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        result = new Result(ResultCode.Exception, exception: ex);
            //    }

            //    return result;
            //}

            internal async Task<Result> ConnectAsync()
            {
                var result = Result.Success;

                //await Utilities.Task.Run(() =>
                //{
                    try
                    {
                        Stream = Utilities.SslStream.Create(BackendStream, true, CertificateValidationCallback, CertificateSelectionCallback, Node.Config.Ssl.EncryptionPolicy);

                        switch (Node.IsClient)
                        {
                            case true: await Stream.AuthenticateAsClientAsync(Node.AsClient.Config.Ssl.TargetHost, Node.AsClient.Config.Ssl.CertificateCollection, Node.Config.Ssl.Protocols, Node.Config.Ssl.CheckCertificateRevocation); break;
                            case false: await Stream.AuthenticateAsServerAsync(Node.AsServer.Config.Ssl.Certificate, Node.AsServer.Config.Ssl.ClientCertificateRequired, Node.Config.Ssl.Protocols, Node.Config.Ssl.CheckCertificateRevocation); break;
                        }
                    }
                    catch (Exception ex)
                    {
                        result = new Result(ResultCode.Exception, exception: ex);
                    }
                //});

                return result;
            }

            internal void Reset()
            {
                Stream?.Dispose();
                Stream = null;
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
