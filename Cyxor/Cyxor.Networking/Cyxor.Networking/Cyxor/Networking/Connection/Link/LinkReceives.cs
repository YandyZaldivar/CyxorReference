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
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace Cyxor.Networking
{
    using Extensions;
    using Serialization;

    using static Utilities.Threading;

    sealed partial class Link
    {
        internal sealed class LinkReceives : LinkProperty, IDisposable
        {
            bool Disposed;
            int PacketSize;
            ReceiveProgress Progress;

            InterlockedCountdown Countdown;
            InterlockedInt UdpReferences;
            InterlockedInt TcpReferences;

            Box TcpBox;
            Serializer TcpBuffer;
            SocketAsyncEventArgs TcpSaea;

            Box UdpBox;
            Serializer UdpBuffer;
            SocketAsyncEventArgs UdpSaea;

            public InterlockedInt PacketQueueReferences;
            public PacketDeliveryQueue<Packet> PacketQueue;

            internal Dictionary<int, Frame> Frames = new Dictionary<int, Frame>();

            internal LinkReceives(Link link) : base(link)
            {
                Progress = new ReceiveProgress();

                Countdown = new InterlockedCountdown(initialCount: 0);

                PacketQueue = new PacketDeliveryQueue<Packet>();
                PacketQueueReferences = new InterlockedInt();

                TcpBuffer = Node.Pools.PopBuffer();
                TcpSaea = new SocketAsyncEventArgs { UserToken = Link };
                TcpSaea.SetBuffer(TcpBuffer.Buffer, 0, TcpBuffer.Capacity);
                TcpSaea.Completed += ProcessReceive;
                TcpBox = Node.Pools.PopBox();

                UdpBuffer = Node.Pools.PopBuffer();
                UdpSaea = new SocketAsyncEventArgs { UserToken = Link };
                UdpSaea.SetBuffer(UdpBuffer.Buffer, 0, UdpBuffer.Capacity);
                UdpSaea.Completed += ProcessReceive;
                UdpBox = Node.Pools.PopBox();
            }

            internal void Initialize()
                => Countdown.Reset(initialCount: 1);

            internal bool Acquire(Packet packet, [CallerMemberName] string callerMemberName = null)
                => Acquire(Link.GetPacketSocket(packet), callerMemberName);

            internal bool Acquire(Socket socket, [CallerMemberName] string callerMemberName = null)
            {
                if (socket != null)
                {
                    if (socket == Link.TcpSocket)
                        TcpReferences.Increment();
                    else if (socket == Link.Node.UdpSocket)
                        UdpReferences.Increment();
                }

                var references = Countdown.Acquire();

                //Node.Log(LogCategory.Information, $"Acquire: {Link.AddressHash} / {references} / {TcpReferences.Value} :: {callerMemberName}");

                if (references <= 0)
                    return false;
                else if (references == int.MaxValue)
                    Node.Log(LogCategory.Fatal, $"{nameof(Link)} receive references max value reached.");

                return true;
            }

            internal void Release(Packet packet, [CallerMemberName] string callerMemberName = null)
                => Release(Link.GetPacketSocket(packet), callerMemberName);

            internal void Release(Socket socket, [CallerMemberName] string callerMemberName = null)
            {
                var socketReferences = int.MinValue;

                if (socket != null)
                {
                    if (socket == Link.TcpSocket)
                        socketReferences = TcpReferences.Decrement();
                    else if (socket == Link.Node.UdpSocket)
                        socketReferences = UdpReferences.Decrement();

                    if (socketReferences < 0)
                        Node.Log(LogCategory.Fatal, $"{nameof(Link)} receive socket countdown decrement below zero.");
                }

                var references = Countdown.Release();

                if (references == 0)
                    Link.DisconnectReferencesIncrement();
                else if (references < 0)
                    Node.Log(LogCategory.Fatal, $"{nameof(Link)} receive countdown decrement below zero.");
                else if (socketReferences == 0)
                    SocketReceiveAsync(socket);

                //Node.Log(LogCategory.Information, $"Release: {Link.AddressHash} / {references} / {TcpReferences.Value} :: {callerMemberName}");
            }

            // TODO: Review and update this method. Mix with SocketReceiveAsync
            void CheckBuffersAndBoxs22(Socket socket)
            {
                var serializer = socket == Link.TcpSocket ? TcpBuffer : UdpBuffer;

                if (PacketSize > 0 && serializer.Capacity < PacketSize)
                {
                    var tmp = serializer;

                    if (serializer == TcpBuffer)
                        serializer = TcpBuffer = new Serializer();
                    else
                        serializer = UdpBuffer = new Serializer();

                    serializer.SetCapacity(PacketSize);

                    serializer.SerializeRaw(tmp);

                    Node.Pools.PushBuffer(tmp);
                }

                // CheckBuffersAndBoxs
                {
                    if (TcpBuffer.Int32Length == 0 || PacketSize == 0)
                        TcpBuffer.Reset(Node.Config.IOBufferSize);

                    TcpBox.Reset();

                    if (Node.Config.UdpEnabled)
                    {
                        if (UdpBuffer.Int32Length == 0)
                            UdpBuffer.Reset(Node.Config.IOBufferSize);

                        UdpBox.Reset();
                    }
                }

                var bufferCount = serializer.Capacity - serializer.Int32Length < Node.Config.MaxTCPReceiveBufferSize ?
                    serializer.Capacity - serializer.Int32Length : Node.Config.MaxTCPReceiveBufferSize;

                var saea = socket == Link.TcpSocket ? TcpSaea : UdpSaea;

                //if (!Node.Config.Ssl.Enabled)
                    saea.SetBuffer(serializer.Buffer, serializer.Int32Length, bufferCount);
                //else
                //    Link.Ssl.PrepareReadBuffer(saea);
            }

            //internal async void SocketReceiveAsync(Socket socket)
            //{
            //    if (!Node.Config.Ssl.Enabled || socket == Node.UdpSocket)
            //        InternalSocketReceiveAsync(socket);
            //    else
            //    {
            //        var readBytes = await Link.Ssl.ReadAsync(TcpBuffer);

            //        if (readBytes == 0)
            //            Release(socket: null);
            //        else
            //        {
            //            TcpBuffer.SetLength(TcpBuffer.Length + readBytes);
            //            ParsePacket(socket, TcpBuffer);
            //        }
            //    }
            //}

            void CheckBuffersAndBoxs(Socket socket)
            {


                // CheckBuffersAndBoxs
                {
                    if (TcpBuffer.Int32Length == 0 || PacketSize == 0)
                        TcpBuffer.Reset(Node.Config.IOBufferSize);

                    TcpBox.Reset();

                    if (Node.Config.UdpEnabled)
                    {
                        if (UdpBuffer.Int32Length == 0)
                            UdpBuffer.Reset(Node.Config.IOBufferSize);

                        UdpBox.Reset();
                    }
                }

                //// TODO: Error, when the buffer is reset with TcpBuffer.Reset(Node.Config.IOBufferSize);
                //// the buffer.Capacity - buffer.Length is set to 0 passing a 0 length buffer to SAEA
                //var count = buffer.Capacity - buffer.Length < Node.Config.MaxTCPReceiveBufferSize ?
                //    buffer.Capacity - buffer.Length : Node.Config.MaxTCPReceiveBufferSize;


            }

            //internal void InternalSocketReceiveAsync(Socket socket)
            internal void SocketReceiveAsync(Socket socket)
            {
                var buffer = socket == Link.TcpSocket ? TcpBuffer : UdpBuffer;

                if (PacketSize > 0 && buffer.Capacity < PacketSize)
                {
                    var tmp = buffer;

                    buffer = buffer == TcpBuffer ? TcpBuffer = new Serializer() : UdpBuffer = new Serializer();

                    buffer.SetCapacity(PacketSize);
                    buffer.SerializeRaw(tmp);

                    if (tmp.Capacity == Node.Config.IOBufferSize)
                        Node.Pools.PushBuffer(tmp);
                }

                var saea = socket == Link.TcpSocket ? TcpSaea : UdpSaea;
                saea.SetBuffer(buffer.Buffer, buffer.Int32Length, buffer.Capacity - buffer.Int32Length);

                var result = false;

                try
                {
                    result = socket.ReceiveAsync(socket == Link.TcpSocket ? TcpSaea : UdpSaea);
                }
                catch (Exception ex)
                {
                    Node.Log(LogCategory.Error, ex);
                    result = false;
                }

                if (!result)
                    // NOTE: Run in a task to avoid deep recursion which may leads to stack overflow
                    Utilities.Task.Run(() => ProcessReceive(socket, socket == Link.TcpSocket ? TcpSaea : UdpSaea)).ConfigureAwait(false);
            }

            void ProcessReceive(object sender, SocketAsyncEventArgs e)
            {
                Node.Statistics.LastOperationDate = DateTime.Now;
                Connection.Statistics.LastOperationDate = DateTime.Now;

                if (e.SocketError != SocketError.Success || e.BytesTransferred == 0)
                {
                    Link.Queries.Reset();

                    if (Node.Config.Ssl.Enabled && e == TcpSaea)
                    {
                        //if (Link.Ssl.IsAuthenticated)
                        //    Acquire(socket: null);

                        //Link.Ssl.SignalReader(bytesTransferred: 0);

                        //if (!Link.Ssl.IsAuthenticated)
                        //    Link.Ssl.SignalReader();
                        //else
                        //    Link.Ssl.Read(TcpSaea);

                        Link.Ssl.Read(TcpSaea);
                    }

                    if (e.SocketError != SocketError.Success)
                    {
                        var socketErrorResult = new Result(ResultCode.SocketError, e.SocketError.ToString());

                        if (Connection.DisconnectionSource == DisconnectionSource.Local)
                        {
                            // TODO: Decide if use the result already set in Connection.Result when Link.DisconnectAsync was called.
                            Connection.Result = socketErrorResult;
                        }

                        if (Connection.DisconnectionSource == DisconnectionSource.None)
                            Connection.DisconnectAsync(socketErrorResult, ShutdownSequence.Abortive, DisconnectionSource.Error);
                    }
                    else if (Connection.DisconnectionSource == DisconnectionSource.Local)
                        Link.TcpSocket.Shutdown(SocketShutdown.Both);

                    Release(socket: null);

                    if (Connection.DisconnectionSource == DisconnectionSource.None)
                        Connection.DisconnectAsync(Connection.Result, ShutdownSequence.Graceful, DisconnectionSource.Remote);

                    return;
                }

                Node.Statistics.AddReceivedBytes(e.BytesTransferred);
                Connection.Statistics.AddReceivedBytes(e.BytesTransferred);

                //if (Node.Config.Ssl.Enabled && e == TcpSaea)
                //    Link.Ssl.SignalReader(e.BytesTransferred);
                //else
                //{
                //    var buffer = e == UdpSaea ? UdpBuffer : TcpBuffer;
                //    buffer.SetLength(buffer.Length + e.BytesTransferred);

                //    ParsePacket(sender as Socket, buffer);
                //}

                var count = 0;
                var buffer = e == UdpSaea ? UdpBuffer : TcpBuffer;
                buffer.SetLength(buffer.Int32Length + e.BytesTransferred);

                if (Node.Config.Ssl.Enabled && e == TcpSaea)
                {
                    do
                    {
                        count += Link.Ssl.Read(TcpSaea);
                    }
                    while (Link.Ssl.IsAuthenticated && count < e.BytesTransferred);

                    buffer = Link.Ssl.ReadBuffer;
                }

                if (count >= 0)
                    ParsePacket(sender as Socket, buffer);
            }

            void ParsePacketFrame(Socket socket, Serializer buffer)
            {
                Acquire(socket);

                try
                {
                    var box = socket == Link.TcpSocket ? TcpBox : UdpBox;

                    while (true)
                    {
                        var frameResult = Frame.TryRead(Connection, buffer, out var frame);

                        switch (frameResult)
                        {
                            case FrameResult.Error:
                            {
                                break;
                            }

                            case FrameResult.Header:
                            {
                                break;
                            }

                            case FrameResult.Partial:
                            {
                                break;
                            }

                            case FrameResult.Ok:
                            {
                                Link.Ssl.BytesProcessed = 0;

                                box.Received = true;

                                var packetBox = box;
                                box = Node.Pools.PopBox();
                                box = packetBox == TcpBox ? TcpBox = box : UdpBox = box;
                                var packet = new Packet(packetBox);

                                if (Progress.Active)
                                {
                                    Progress.BytesTransferred = PacketSize;
                                    Node.Events.Post(new Events.PacketReceiveProgressChangedEventArgs(Link, Progress));
                                    Progress.Reset();
                                }

                                if (Delivery.TryProcessReply(Connection, packet))
                                    continue;

                                Node.Middleware.OnPacketReceived(Connection, packet);

                                if (buffer.Count == 0)
                                    break;

                                break;
                            }

                            default: // Contains bytes needed to complete a frame.
                            {
                                break;
                            }
                        }
                    }
                }
                //catch (Exception ex)
                catch
                {

                }
                finally
                {
                    Release(socket);
                }
            }

            void ParsePacket(Socket socket, Serializer buffer)
            {
                Acquire(socket);

                try
                {
                    if (PacketSize <= buffer.Int32Length)
                    {
                        var box = socket == Link.TcpSocket ? TcpBox : UdpBox;
                        //PacketSize = box.TryLoad(Connection, buffer, pop: true);

                        //while (PacketSize == 0)
                        while ((PacketSize = box.TryLoad(Connection, buffer, pop: true)) == 0)
                        {
                            Link.Ssl.BytesProcessed = 0;

                            box.Received = true;

                            var packetBox = box;
                            box = Node.Pools.PopBox();
                            box = packetBox == TcpBox ? TcpBox = box : UdpBox = box;
                            var packet = new Packet(packetBox);

                            if (Progress.Active)
                            {
                                Progress.BytesTransferred = PacketSize;
                                Node.Events.Post(new Events.PacketReceiveProgressChangedEventArgs(Link, Progress));
                                Progress.Reset();
                            }

                            if (Delivery.TryProcessReply(Connection, packet))
                                continue;

                            Node.Middleware.OnPacketReceived(Connection, packet);

                            if (buffer.Count == 0)
                                break;
                        }
                    }
                    else
                    {
                        if (!Progress.Active)
                        {
                            if (!Box.TryParseHeader(buffer, ref Progress) || (Connection.IsHttp ?? false))
                                goto Invalid;

                            Progress.Active = true;
                            Progress.TotalSize = PacketSize;
                        }

                        var progress = (int)((ulong)buffer.Int32Length * 100 / (ulong)PacketSize);

                        var needReport = progress >= Progress.ProgressPercent + Node.Config.TransferReportPercentThreshold;
                        needReport &= DateTime.Now >= Progress.LastUpdateTime.AddMilliseconds(Node.Config.TransferReportMillisecondsThreshold);

                        if (needReport)
                        {
                            Progress.ProgressPercent = progress;
                            Progress.LastUpdateTime = DateTime.Now;
                            Progress.BytesTransferred = buffer.Int32Length;

                            if (Progress.Tracking) { /* TODO: ?!?! */ }

                            Node.Events.Post(new Events.PacketReceiveProgressChangedEventArgs(Link, Progress));
                        }

                        Invalid:;
                    }
                }
                catch (InvalidOperationException ex) when (ex.Message == Utilities.ResourceStrings.CyxorInternalException)
                {
                    var result = new Result(ResultCode.Exception, exception: ex);
                    Connection.DisconnectAsync(result, ShutdownSequence.Graceful, DisconnectionSource.Local);
                    Node.Log(LogCategory.Fatal, ex);
                }
                catch (CryptographicException ex)
                {
                    var result = new Result(ResultCode.ProtocolErrorCrypto, exception: ex);
                    Connection.DisconnectAsync(result, ShutdownSequence.Graceful, DisconnectionSource.Local);
                    Node.Log(LogCategory.Error, $"[{Connection.Name}]: {Utilities.ResourceStrings.CryptographicError}");
                }
                catch (Exception ex)
                {
                    var result = new Result(ResultCode.ProtocolError, exception: ex);
                    Connection.DisconnectAsync(result, ShutdownSequence.Graceful, DisconnectionSource.Local);
                    Node.Log(LogCategory.Error, $"[{Connection.Name}]: {Utilities.ResourceStrings.ProtocolError}");
                }
                finally
                {
                    //if (link.ShutdownState == Enums.ShutdownMode.None)
                    //   if (buffer.Length > Config.IOBuffersSize)
                    //      ProtocolError(link, "Too large box");

                    Release(socket);
                }
            }

            internal void Reset()
            {
                var referenceCountCheck = 0;

                referenceCountCheck |= Countdown.Count;
                referenceCountCheck |= TcpReferences.Value;
                referenceCountCheck |= UdpReferences.Value;

                if (referenceCountCheck != 0)
                    Link.Node.Log(LogCategory.Fatal, $"{nameof(Link)} reset with invalid receive references count.");

                Countdown.Reset();
            }

            void IDisposable.Dispose()
            {
                if (!Disposed)
                {
                    Reset();

                    if (TcpSaea != null)
                    {
                        TcpSaea.Completed -= ProcessReceive;
                        TcpSaea.Dispose();
                    }

                    if (UdpSaea != null)
                    {
                        UdpSaea.Completed -= ProcessReceive;
                        UdpSaea.Dispose();
                    }

                    Disposed = true;
                }
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
