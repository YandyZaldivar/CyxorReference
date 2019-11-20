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
using System.Threading.Tasks;

namespace Cyxor.Networking
{
    using static Utilities.Threading;

    sealed partial class Link
    {
        internal sealed partial class LinkSsl
        {
            /// <summary>
            /// Provides the underlying stream of data for SSL.
            /// </summary>
            /// <seealso cref="System.IO.Stream" />
            internal class SslBackendStream : Stream
            {
                internal LinkSsl Ssl { get; }

                public SslBackendStream(LinkSsl ssl) => Ssl = ssl;

                public override bool CanRead => true;

                public override bool CanWrite => true;

                public override bool CanSeek => false;

                /// <summary>
                /// The length of data available on the stream. Always throws <see cref='NotSupportedException'/>.
                /// </summary>
                public override long Length => throw new NotSupportedException();

                /// <summary>
                /// Gets or sets the position in the stream. Always throws <see cref='NotSupportedException'/>.
                /// </summary>
                public override long Position
                {
                    get => throw new NotSupportedException();
                    set => throw new NotSupportedException();
                }

                /// <summary>
                /// Flushes data from the stream. This is meaningless for us, so it does nothing.
                /// </summary>
                public override void Flush() { }

                /// <summary>
                /// Seeks a specific position in the stream. This method is not supported by the <see cref='LinkStream'/> class.
                /// </summary>
                public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

                /// <summary>
                /// Sets the length of the stream. Always throws <see cref='NotSupportedException'/>
                /// </summary>
                public override void SetLength(long value) => throw new NotSupportedException();

                public override int Read(byte[] buffer, int offset, int count)
                    => ReadInternal(buffer, offset, count);

#if !NET35 && !NET40
                public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
                {
                    var bytesAvailable = ((Ssl.ReceiveAsyncSaea?.BytesTransferred ?? 0) + Ssl.BytesBuffered) - Ssl.BytesProcessed;

                    if (bytesAvailable == 0 || bytesAvailable < count)
                    {
                        Ssl.BytesBuffered += Ssl.ReceiveAsyncSaea?.BytesTransferred ?? 0;
                        Ssl.Link.Receives.SocketReceiveAsync(Ssl.Link.TcpSocket);

                        await Ssl.ReaderAwaitable;
                        Ssl.ReaderAwaitable.Reset();
                    }

                    return ReadInternal(buffer, offset, count);
                }
#endif

                int ReadInternal(byte[] buffer, int offset, int count)
                {
                    var bytesAvailable = (Ssl.ReceiveAsyncSaea.BytesTransferred + Ssl.BytesBuffered) - Ssl.BytesProcessed;
                    var bytesRead = count < bytesAvailable ? count : bytesAvailable;
                    Ssl.BytesProcessed += bytesRead;
                    Buffer.BlockCopy(Ssl.ReceiveAsyncSaea.Buffer, Ssl.ReceiveAsyncSaea.Offset, buffer, offset, bytesRead);
                    Ssl.ReceiveAsyncSaea.SetBuffer(Ssl.ReceiveAsyncSaea.Offset + bytesRead, Ssl.ReceiveAsyncSaea.Count - bytesRead);

                    return bytesRead;
                }



                /// <summary>
                /// Reads data from the stream.
                /// </summary>
                //public override int Read(byte[] buffer, int offset, int count)
                //{
                //    //var bytesTransferred = (Ssl.ReceiveAsyncSaea?.BytesTransferred ?? 0) - (Ssl.ReceiveAsyncSaea?.Offset ?? 0);
                //    var bytesAvailable = (Ssl.ReceiveAsyncSaea?.BytesTransferred ?? 0) - Ssl.BytesConsumed;

                //    if (!Ssl.IsAuthenticated)
                //    {
                //        if (bytesAvailable == 0 || bytesAvailable < count)
                //        {
                //            Ssl.BytesConsumed = 0;
                //            Ssl.Link.Receives.SocketReceiveAsync(Ssl.Link.TcpSocket);

                //            Ssl.ReaderEvent.Wait();
                //            Ssl.ReaderEvent.Reset();
                //        }
                //    }

                //    //bytesTransferred = Ssl.ReceiveAsyncSaea.BytesTransferred - Ssl.ReceiveAsyncSaea.Offset;
                //    bytesAvailable = Ssl.ReceiveAsyncSaea.BytesTransferred - Ssl.BytesConsumed;
                //    //bytesTransferred = Ssl.ReceiveAsyncSaea.BytesTransferred;
                //    var bytesRead = count < bytesAvailable ? count : bytesAvailable;
                //    Ssl.BytesConsumed += bytesRead;
                //    Buffer.BlockCopy(Ssl.ReceiveAsyncSaea.Buffer, Ssl.ReceiveAsyncSaea.Offset, buffer, offset, bytesRead);
                //    Ssl.ReceiveAsyncSaea.SetBuffer(Ssl.ReceiveAsyncSaea.Offset + bytesRead, Ssl.ReceiveAsyncSaea.Count - bytesRead);

                //    return bytesRead;
                //}

                /// <summary>
                /// Writes data to the stream.
                /// </summary>
                public override async void Write(byte[] buffer, int offset, int count)
                {
                    if (Ssl.Stream.IsAuthenticated)
                        Ssl.SendAsyncSaea.SetBuffer(buffer, offset, count);
                    else
                        using (var packet = new Packet(Ssl.Connection))
                        {
                            packet.Serializer.SerializeRaw(buffer, offset, count);
                            var result = await packet.SendAsync();

                            if (!result)
                            {
                                Dispose();
                                Ssl.Node.Log(LogCategory.Error, result.Comment);
                                await Ssl.Connection.DisconnectAsync(result, ShutdownSequence.Immediate);
                            }
                        }
                }

                /*
#if NET35 || NET40
                public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
                {
                    //Ssl.ReadBuffer = new ArraySegment<byte>(buffer, offset, count);

                    //Ssl.Link.Receives.InternalSocketReceiveAsync(Ssl.Link.TcpSocket);

                    return new Action(() =>
                    {
                        //Ssl.ReaderEvent.Wait();
                        //Ssl.ReaderEvent.Reset();
                    }).BeginInvoke(callback, state);
                }

                //public override int EndRead(IAsyncResult asyncResult) => Ssl.ReadBytesTransferred;
                public override int EndRead(IAsyncResult asyncResult) => Ssl.ReceiveAsyncSaea.BytesTransferred;
#else
                public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
                {
                    //Ssl.ReadBuffer = new ArraySegment<byte>(buffer, offset, count);

                    //Ssl.Link.Receives.InternalSocketReceiveAsync(Ssl.Link.TcpSocket);

                    //await Ssl.ReaderAwaitable.ConfigureAwait(false);
                    //Ssl.ReaderAwaitable.Reset();

                    await Task.Delay(2000);

                    return Ssl.ReceiveAsyncSaea.BytesTransferred;
                }
#endif
                */
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
