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
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO.Compression;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Cyxor.Networking
{
    using Models;
    using Extensions;
    using Serialization;

    // TODO: Make private
    public sealed class Box : IBox//, ISerializable
    {
        // TODO: Switch the RelayConnection with the RootConnection or SourceConnection
        internal Node Node { get; set; }
        internal Connection Connection { get; set; }
        internal Connection RootConnection { get; set; }
        internal Connection RelayConnection { get; set; }
        internal IEnumerable<Connection> Connections { get; set; }

        internal bool IsHttpMessage { get; set; }
        internal HttpRequest HttpRequest { get; set; }
        internal ControllerAction ControllerAction { get; set; }

        public Box()
        {

        }

        bool Serialized = false;
        Serializer BaseBuffer = new Serializer();

        internal Serializer SerialBuffer
        {
            get
            {
                Serialize();
                return BaseBuffer;
            }
            //set
            //{
            //    if (TryLoad(value, false) != 0)
            //        throw new InvalidDataException("Mal formed packet.");
            //}
        }

        public void Serialize(Serializer serializer)
        {
            Serialize();
            serializer.Serialize(BaseBuffer);
        }

        //public void Deserialize(Serializer serializer)
        //{
        //    if (TryLoad(serializer, false) != 0)
        //        throw new InvalidDataException("Mal formed packet.");
        //}

        /*
        static class Properties
        {
            internal const byte

                Internal = 0,
                Type = 1,
                Broadcast = 2,
                Address = 3,
                QueryMode = 4,
                //QueryMode = 5,
                Message = 6,
                CfgByte2 = 7,

                Anonymous = 0,
                Sender = 1,
                Progress = 2,
                Compress = 3,
                Reserved = 4,
                Priority = 5,
                //Priority = 6;
                CfgByte3 = 7,

                Reserved30 = 0,
                Reserved31 = 1,
                Reserved32 = 2,
                Reserved33 = 3,
                Reserved34 = 4,
                Reserved35 = 5,
                Reserved36 = 6,
                CfgByte4 = 7,

                Reserved40 = 0,
                Reserved41 = 1,
                Reserved42 = 2,
                Reserved43 = 3,
                Reserved44 = 4,
                Reserved45 = 5,
                Reserved46 = 6,
                CfgByte5 = 7;
        }
        */

        internal bool NodeFree { get; set; }
        internal bool Received;
        //internal InterlockedInt References;

        bool @internal;
        bool UseDefaultEncrypt = true;
        internal bool Internal
        {
            get => @internal;
            set
            {
                if (@internal != value)
                {
                    if (UseDefaultEncrypt)
                    {
                        switch (value)
                        {
                            case false: Encrypt = (bool)Node?.Config.Packets.Encrypt; break;
                            case true: Encrypt = (bool)Node?.Config.Packets.InternalEncrypt; break;
                        }

                        UseDefaultEncrypt = true;
                    }

                    @internal = value;
                }
            }
        }

        internal void TrySet<T>(ref T property, T value)
        {
            //if (ReadOnly)
            //    throw new InvalidOperationException("This packet instance is readonly.");

            if (property == null && value == null)
                return;

            if (property == null || !property.Equals(value))
            {
                property = value;
                Serialized = false;
            }
        }

        #region Public Properties

        public bool ReadOnly { get; internal set; }

        //bool trackSending;
        //public bool TrackSending
        //{
        //   get
        //   {
        //      return trackSending;
        //   }
        //   set
        //   {
        //      if (!ReadOnly)
        //      {
        //         if (trackSending != value)
        //         {
        //            trackSending = value;
        //            Serialized = false;
        //         }
        //      }
        //      else
        //         throw new InvalidOperationException(String.Format(ReadOnlyExceptionMsgFormat, "TrackSending", value));
        //   }
        //}

        bool trackSendProgress;
        public bool TrackSendProgress
        {
            get => trackSendProgress;
            set => TrySet(ref trackSendProgress, value);
        }

        string transmitFilesSearchPattern = "*";
        public string TransmitFilesSearchPattern
        {
            get => transmitFilesSearchPattern;
            set => TrySet(ref transmitFilesSearchPattern, value);
        }

        SearchOption transmitFilesSearchOption = SearchOption.AllDirectories;
        public SearchOption TransmitFilesSearchOption
        {
            get => transmitFilesSearchOption;
            set => TrySet(ref transmitFilesSearchOption, value);
        }

        PacketTransmitFilesThreadOptions packetTransmitFilesThreadOptions = PacketTransmitFilesThreadOptions.UseKernelApc;
        public PacketTransmitFilesThreadOptions PacketTransmitFilesThreadOptions
        {
            get => packetTransmitFilesThreadOptions;
            set => TrySet(ref packetTransmitFilesThreadOptions, value);
        }

        bool error;
        public bool Error
        {
            get => error;
            set => TrySet(ref error, value);
        }

        bool broadcast;
        public bool Broadcast
        {
            get => broadcast;
            set => TrySet(ref broadcast, value);
        }

        bool anonymous;
        public bool Anonymous
        {
            get => anonymous;
            set => TrySet(ref anonymous, value);
        }

        int id;
        public int Id
        {
            get => id;
            set => TrySet(ref id, value);
        }

        bool isCommand;
        internal bool IsCommand
        {
            get => isCommand;
            set => TrySet(ref isCommand, value);
        }

        PacketSending sendMode;
        public PacketSending Sending
        {
            get => sendMode;
            set
            {
                if (Node.Config.Ssl.Enabled && value == PacketSending.Overlapped)
                    value = PacketSending.Queued;

                TrySet(ref sendMode, value);
            }
        }

        PacketPriority priority;
        public PacketPriority Priority
        {
            get => priority;
            set => TrySet(ref priority, value);
        }

        PacketProtocol protocol;
        public PacketProtocol Protocol
        {
            get => protocol;
            set => TrySet(ref protocol, value);
        }

        PacketQueryMode queryMode;
        public PacketQueryMode QueryMode
        {
            get => queryMode;
            internal set => TrySet(ref queryMode, value);
        }

        int queryId;
        internal int QueryId
        {
            get => queryId;
            set => TrySet(ref queryId, value);
        }

        int replyId;
        internal int ReplyId
        {
            get => replyId;
            set => TrySet(ref replyId, value);
        }

        public bool IsQueryAndReply => QueryMode == PacketQueryMode.QueryAndReply;
        public bool IsQuery => IsQueryAndReply || QueryMode == PacketQueryMode.Query;
        public bool IsReply => IsQueryAndReply || QueryMode == PacketQueryMode.Reply;

        bool encrypt;
        public bool Encrypt
        {
            get => encrypt;
            set
            {
                UseDefaultEncrypt = false;
                TrySet(ref encrypt, value);
            }
        }

        bool compress;
        public bool Compress
        {
            get => compress;
            set => TrySet(ref compress, value);
        }

        Serializer message;
        public Serializer Serializer
        {
            get
            {
                if (message == null)
                    if (Node != null)
                        message = Node.Pools.PopBuffer();
                    else
                        message = new Serializer();

                Serialized = false;
                return message;
            }
        }

        string address;
        public string Address
        {
            get => address;
            set => TrySet(ref address, value);
        }

        string sender;
        public string Sender
        {
            get => sender;
            internal set => TrySet(ref sender, value);
        }

        bool isRelay;
        public bool IsRelay
        {
            get => isRelay;
            set => TrySet(ref isRelay, value);
        }

        public bool IsFromNode => sender == null && !anonymous;
        public bool IsToNode => !IsRelay && !broadcast && string.IsNullOrEmpty(Address);
        public bool IsGroupAddress => broadcast && !string.IsNullOrEmpty(Address);

        #endregion Public Properties

        /*
        public Box Clonar()
        {
            Box box = Node.Pools.PopBox();
            {
                box.Node = Node;
                box.Link = Link;

                box.Received = Received;
                box.Sending = Sending;

                box.ReadOnly = ReadOnly;
                box.encrypt = encrypt;

                box.Internal = Internal;
                box.broadcast = broadcast;
                box.protocol = protocol;
                box.id = id;
                box.address = address;
                box.sender = sender;

                box.Serializer.WriteRaw(message);
                box.Serializer.SetPosition(0);

                box.priority = priority;
                box.queryMode = queryMode;
                box.trackSendProgress = trackSendProgress;
                box.anonymous = anonymous;
                box.compress = compress;

                box.Serialized = false;
            }

            return box;
        }
        */

        // TODO: Update correctly using the Connection/s object

        internal void Reset(bool fullReset = false)
        {
            Received = false;
            trackSendProgress = false;

            IsHttpMessage = false;
            HttpRequest = null;
            ControllerAction = null;

            sendMode = Node.Config.Packets.PacketSending;

            ReadOnly = false;
            encrypt = false;

            UseDefaultEncrypt = true;
            Internal = false;
            broadcast = false;
            id = 0;
            isCommand = false;
            address = null;
            sender = null;

            if (message != null)
                if (NodeFree)
                    message.Reset(8192);
                else
                    message.Reset(Node.Config.IOBufferSize);

            BaseBuffer.Reset();

            priority = PacketPriority.Exclusive;
            protocol = PacketProtocol.Tcp;
            queryMode = PacketQueryMode.Send;
            anonymous = false;
            compress = false;

            Serialized = false;

            if (fullReset)
            {
                //if (Node is Server)
                //    Link = null;

                RelayConnection = null;
                NodeFree = false;
            }
        }

        #region Private Functions

        bool IsSubprotocolConfigByte1 => true;
        bool IsConfigByte2 => Error || Anonymous || trackSendProgress || Compress || Priority != PacketPriority.Exclusive || IsConfigByte3;
        bool IsConfigByte3 => false;

        byte GetConfigByte()
        {
            var bits = (BitSerializer)0;

            bits[0] = Internal;
            bits[1] = IsCommand;
            bits.Serialize((int)QueryMode, FrameProtocolMap.QueryMode);
            bits[4] = Broadcast;
            bits[5] = !string.IsNullOrEmpty(Address);
            bits[6] = RelayConnection != null && !Anonymous;
            bits[7] = IsConfigByte2;

            return bits;
        }

        byte GetConfigByte2()
        {
            if (!IsConfigByte2)
                return 0;

            var bits = (BitSerializer)0;

            bits[0] = Error;
            bits[1] = trackSendProgress;
            bits[2] = Compress;
            bits[3] = Anonymous;
            bits.Serialize((int)Priority, FrameProtocolMap.Priority);
            bits[7] = IsConfigByte3;

            return bits;
        }

        internal static bool TryParseHeader(Serializer serializer, ref ReceiveProgress progress)
        {
            if (serializer.Int32Length == 0)
                return false;

            try
            {
                serializer.Int32Position = 0;

                var cfg = (BitSerializer)serializer.DeserializeByte();

                if (cfg[FrameProtocolMap.CfgByte2])
                {
                    var cfg2 = (BitSerializer)serializer.DeserializeByte();

                    progress.Tracking = cfg2[FrameProtocolMap.Progress];

                    if (cfg[FrameProtocolMap.CfgByte3])
                        throw new NotImplementedException();
                }

                if (cfg[FrameProtocolMap.Encrypted])
                    return false;

                progress.Type = serializer.DeserializeInt32();

                if (!cfg[FrameProtocolMap.Broadcast])
                    if (!cfg[FrameProtocolMap.Address])
                        progress.IsToServer = true;

                return true;
            }
            catch
            {
                return false;
            }
        }

        //int? CheckHttpRequest(Connection connection, Serializer serializer, bool pop = true)
        //{
        //    try
        //    {
        //        var httpRequest = new HttpRequest(serializer.ToString());

        //        return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.ToString().Length;
        //    }
        //}


        // TODO: Eliminar lo del pop y usar un segundo buffer para cargar el resto del paquete, luego regresar al principio
        // del buffer 1. Además de no establecer la posición en 0, sino dejar que se empiece a leer por donde vaya el buffer.
        internal int TryLoad(Connection connection, Serializer serializer, bool pop = true)
        {
            if (serializer.Int32Length == 0)
                return -1;

            RootConnection = connection;

            var result = 0;

            Reset();

            Serialized = false;

            serializer.Int32Position = 0;

            var httpRequest = default(HttpRequest);

            try
            {
                if (connection.IsHttp ?? true)
                    httpRequest = new HttpRequest(connection, serializer, pop);
            }
            catch (DecoderFallbackException)
            {

            }
            catch
            {
                if (connection.IsHttp ?? false)
                    return -1;
            }

            if (httpRequest?.Result == null)
            {
                connection.IsHttp = false;
                serializer.Int32Position = 0;
            }
            else
            {
                connection.IsHttp = true;

                if (httpRequest.Result != 0)
                    return httpRequest.Result.Value;

                IsHttpMessage = true;
                HttpRequest = httpRequest;

                isCommand = true;
                queryMode = PacketQueryMode.Query;

                if (message != null)
                    message.Int32Position = 0;
                else
                    message = Node.Pools.PopBuffer();

                message.SerializeRaw(new Result(model: httpRequest.Api));
                message.Int32Position = 0;

                return httpRequest.Result.Value;
            }

            var rb = (byte)0;
            if (!serializer.TryDeserializeByte(out rb))
                return -1;

            var cfg = (BitSerializer)rb;

            if (cfg[FrameProtocolMap.CfgByte2])
            {
                if (!serializer.TryDeserializeByte(out rb))
                    return -1;

                var cfg2 = (BitSerializer)rb;

                priority = (PacketPriority)cfg2.Deserialize(FrameProtocolMap.Priority, 2);
                trackSendProgress = cfg2[FrameProtocolMap.Progress];
                anonymous = cfg2[FrameProtocolMap.Anonymous];
                compress = cfg2[FrameProtocolMap.Compress];
                error = cfg2[FrameProtocolMap.Error];

                if (cfg2[FrameProtocolMap.CfgByte3])
                    throw new NotImplementedException();
            }

            isCommand = cfg[FrameProtocolMap.Command];
            Internal = cfg[FrameProtocolMap.Encrypted];
            broadcast = cfg[FrameProtocolMap.Broadcast];
            queryMode = (PacketQueryMode)cfg.Deserialize(FrameProtocolMap.QueryMode, 2);
            //protocol = cfg[ProtocolMap.Protocol] ? PacketProtocol.Udp : PacketProtocol.Tcp;

            if (!serializer.TryDeserializeInt32(out id))
                return -1;

            if (IsQuery)
                if (!serializer.TryDeserializeInt32(out queryId))
                    return -1;

            if (IsReply)
                if (!serializer.TryDeserializeInt32(out replyId))
                    return -1;

            var server = Node as Server;

            if (server != null)
                if (cfg[FrameProtocolMap.Sender])
                    throw new Exception("Protocol Error.");

            if (!cfg[FrameProtocolMap.Address] && broadcast)
            {
                if (server == null)
                    throw new Exception("Protocol Error.");

                Connections = from cx in server.Connections.List
                              where cx != Connection
                              select cx;

                RelayConnection = connection;
            }
            else if (cfg[FrameProtocolMap.Address])
            {
                if (!serializer.TryDeserializeUInt32(out var addressHashCode))
                    return -1;

                // TODO: Handle the case when address be null

                if (server == null)
                {
                    if (!broadcast || (broadcast && (!anonymous && !cfg[FrameProtocolMap.Sender])))
                        throw new Exception("Protocol Error.");

                    // TODO:
                    //address = Link.FindGroup((int)receiverHashCode);
                }
                else
                {
                    IsRelay = true;
                    RelayConnection = connection;

                    if (!broadcast)
                        Connection = server.Connections.Find((int)addressHashCode);
                    else
                    {
                        // TODO: Select correct group connections
                        //var group = server.GetGroup((int)addressHashCode);

                        Connections = from cx in server.Connections.List
                                      where cx != Connection
                                      select cx;
                    }

                    if (!anonymous)
                        sender = RootConnection?.Name;
                }
            }

            if (cfg[FrameProtocolMap.Sender])
                if (!serializer.TryDeserializeString(out sender, throwOnEncodingException: true))
                    return -1;

            //if (cfg[ProtocolMap.Message])
            {
                if (!serializer.TryDeserializeInt32(out var size))
                    return -1;

                if (size > serializer.Int32Length - serializer.Int32Position)
                    return serializer.Int32Position + size;

                if (message != null)
                    message.Int32Position = 0;
                else
                    message = Node.Pools.PopBuffer();

                var outMs = (MemoryStream)null;

                if (compress)
                {
                    outMs = new MemoryStream();

                    using (var inMs = new MemoryStream(serializer.Buffer, serializer.Int32Position, size))
                        using (var gzip = new GZipStream(inMs, CompressionMode.Decompress))
                            gzip.CopyTo(outMs);
                }

                if (Internal)// TODO: Change Internal for Encrypted
                {
                    if (connection?.Link?.Crypto == null)
                        throw new CryptographicException("Cyxor cryptographic exception: Null cryptographic algorithm.");

                    var tmpBuffer = Node.Pools.PopBuffer();

                    //var decryptor = connection.Link.Crypto.Decryptor;

                    //if (compress)
                    //    tmpBuffer.WriteRaw(decryptor.TransformFinalBlock(Utilities.MemoryStream.GetBuffer(outMs), 0, (int)outMs.Length));
                    //else
                    //    tmpBuffer.WriteRaw(decryptor.TransformFinalBlock(serializer.Buffer, serializer.Position, size));

                    //result = TryLoad(connection, tmpBuffer, false);

                    encrypt = true;

                    Node.Pools.PushBuffer(tmpBuffer);
                }
                else
                {
                    if (compress)
                        message.SerializeRaw(outMs.GetBuffer(), 0, (int)outMs.Length);
                    else
                        message.SerializeRaw(serializer.Buffer, serializer.Int32Position, size);

                    message.Int32Position = 0;
                }

                serializer.Int32Position = serializer.Int32Position + size;
            }

            if (pop)
                if (serializer.Int32Position == serializer.Int32Length)
                    serializer.Reset();
                else
                    serializer.Pop(serializer.Int32Position);

            return result;
        }

        #endregion

        #region Serialization

        string DefaultHttpResponse()
        {
            var statusCode = 200;
            var contentType = "application/json";
            var contentLength = 0;

            var corsOrigin = "*";
            var corsCredentials = "true";
            var corsMethods = "GET, PUT, POST, DELETE, PATCH, OPTIONS";
            var corsHeaders = "Origin, Content-Type, X-Requested-With, Accept, Authorization";

            var newLine = Utilities.Http.NewLine;

            return

                $"HTTP/1.1 {statusCode}{newLine}" +
                $"Content-Type: {contentType}{newLine}" +
                $"Content-Length: {contentLength}{newLine}" +

                $"Access-Control-Allow-Origin: {corsOrigin}{newLine}" +
                $"Access-Control-Allow-Credentials: {corsCredentials}{newLine}" +
                $"Access-Control-Allow-Headers: {corsHeaders}{newLine}" +
                $"Access-Control-Allow-Methods: {corsMethods}{newLine}" +

                $"Connection: keep-alive{newLine}{newLine}";
        }

        void Serialize()
        {
            if (Serialized)
                return;

            var tmpBuffer = BaseBuffer;

            tmpBuffer.Reset();

            if (IsHttpMessage)
            {
                if (message.Int32Length > 0)
                    tmpBuffer.SerializeRaw(message);
                else
                    tmpBuffer.SerializeRaw(DefaultHttpResponse());

                return;
            }

            if (encrypt)
            {
                //if (Link.Crypto == null)
                //    throw new CryptographicException("Cyxor cryptographic exception: Null cryptographic algorithm.");

                //tmpBuffer = Node.Pools.PopBuffer();
            }

            tmpBuffer.Serialize(GetConfigByte());

            if (IsConfigByte2)
            {
                tmpBuffer.Serialize(GetConfigByte2());

                if (IsConfigByte3)
                    throw new NotImplementedException();
            }

            tmpBuffer.Serialize(id);

            if (IsQuery)
                tmpBuffer.Serialize(queryId);

            if (IsReply)
                tmpBuffer.Serialize(replyId);

            if (address != null)
                tmpBuffer.Serialize((uint)Utilities.HashCode.GetFrom(address));

            if (RelayConnection != null && !Anonymous)
                tmpBuffer.Serialize(RelayConnection.Name);

            // TODO: We are always serializing a message length, remove this when upgrade to websockets.
            if (!(message != null ? message.Int32Length != 0 ? true : false : false))
                tmpBuffer.Serialize(0);
            else
            {
                if (!compress)
                    tmpBuffer.Serialize(message);
                else
                    using (var gzip = new GZipStream(new MemoryStream(), CompressionMode.Compress))
                    {
                        var stream = gzip.BaseStream as MemoryStream;
                        gzip.Write(message.Buffer, 0, message.Int32Length);
                        gzip.Flush();

                        if (stream.Length < message.Int32Length)
                            tmpBuffer.Serialize(stream.GetBuffer(), 0, (int)stream.Length);
                        else
                        {
                            compress = false;
                            tmpBuffer.Serialize(message);
                        }
                    }
            }

            if (encrypt)
            {
                //var bits = (BitBuffer)0;

                //bits[Properties.Internal] = true;
                //bits[Properties.Message] = true;

                //BaseBuffer.Write((byte)bits);
                //BaseBuffer.Write(Link.Crypto.Encryptor.TransformFinalBlock(tmpBuffer.Buffer, 0, tmpBuffer.Length));

                //Node.Pools.PushBuffer(tmpBuffer);
            }

            Serialized = true;
        }

        //internal void LoadFromFile(string fileName = null)
        //{
        //    Serializer serializer = new Serializer();
        //    serializer.LoadFromFile(fileName);

        //    if (TryLoad(serializer, false) != 0)
        //        throw new InvalidDataException("Mal formed packet.");
        //}

        #endregion

        public override string ToString()
        {
            return "";

            /*
            ValidateNodes();

            int cHeaderLength = 1;
            int cMessageLength = -1;

            int headerLength = -1;
            int messageLength = message == null ? 0 : message.Length;

            Action<bool> CShift = serialized =>
            {
               if (serialized)
                  cMessageLength = baseBuffer.Length - cHeaderLength;

               encrypt = false;
               compress = false;

               Serialize();

               headerLength = baseBuffer.Length - messageLength;

               encrypt = true;
               compress = true;

               Serialized = false;

               if (!serialized)
               {
                  Serialize();
                  cMessageLength = baseBuffer.Length - cHeaderLength;
               }
            };

            if (compress || encrypt)
               CShift(Serialized);

            if (headerLength == -1)
            {
               Serialize();
               headerLength = baseBuffer.Length - messageLength;
            }

            string visibility = IsGroupAddress ? "Group" : broadcast ? "Broadcast" : "Private";

            string target = !Received ? "to " + (!IsGroupAddress ?
               !broadcast ? Node is Server ? "'" + receiver + "'" : "'Server'" : "'All'" : "'" + receiver + "'") :
               "from " + (IsFromServer ? "'Server'" : anonymous ? "Anonymous" : "'" + sender + "'") +
               "to " + (receiver == null ? "Server" : "'" + receiver + "'");

            string format = null;

            if (compress || encrypt)
               format = "{0} Box<{1}> {2} ({3} + {4}) bytes [{5}-{6}] [{7}] [{8}] ({9} + {10}) bytes";
            else
               format = "{0} Box<{1}> {2} ({3} + {4}) bytes [{5}-{6}] [{7}] [{8}]";

            string value = string.Format
            (
               format,
               visibility,
               id,
               target,
               headerLength,
               messageLength,
               protocol,
               priority,
               encrypt == true ? "Encrypt=true" : "Encrypt=false",
               compress == true ? "Compress=true" : "Compress=false",
               cHeaderLength,
               cMessageLength
            );

            return value;
            */
        }

        // TODO: This method could not throw on not connected and instead try to connect
        internal Result Validate()
        {
            if (Address != null)
            {
                var receiverName = Address;
                Node.Config.Names.Validate(ref receiverName);
            }

            //if (Node is Server)
            //    if (!string.IsNullOrEmpty(Address))
            //        if (string.Compare(Link.Connection.Name, Address, ignoreCase: true) != 0)
            //            throw new InvalidOperationException("The Receiver name doesn't match with the Link");

            //if (Query == PacketQuery.On)
            //    if (Connections != null)
            //        return new Result(ResultCode.PacketQueryNotSupported);

            return Result.Success;
        }

        /*
        internal async Task<bool> TweakBox(Box boxer, ref Link receiver)
        {
           if (box == null)
              throw new ArgumentNullException();

           if (box.IsReadOnly)
              throw new InvalidOperationException("The argument box is read only");

           if (receiver == null)
              receiver = box.Link;

           if (receiver != null)
              if (box.Receiver != null)
                 if (receiver.Connection.Name != box.Receiver)
                    throw new ArgumentException();

           if (receiver == null)
              if (box.Receiver == null)
                 if (box.Broadcast == false)
                    throw new ArgumentException("The argument box has not a valid address");

           if (receiver != null)
              box.Receiver = null;
           else if (box.Receiver != null)
           {
              if ((receiver = ConnectionManager.GetLink(box.Receiver)) != null)
                 box.Receiver = null;
              else
              {
                 // TODO: Message to group
              }
           }
           else // Broadcast
           {
              SendTargets targets = null;

              box.Serialize();

              box.References.Increment();
              {
                 var linkValues = ConnectionManager.ConnectedLinks.Values;

                 if (Config.MulticastBoxes == MulticastBoxes.Sequential)
                 {
                    var fails = new List<string>(linkValues.Count);
                    var succeeds = new List<string>(linkValues.Count);

                    foreach (Link linker in linkValues)
                       if (Config.UpdateSendTargets)
                          if (await SendAsync(box, link).ConfigureAwait(false))
                             succeeds.Add(link.Connection.Name);
                          else
                             fails.Add(link.Connection.Name);
                       else
                          await SendAsync(box, link).ConfigureAwait(false);

                    if (Config.UpdateSendTargets)
                       targets = new SendTargets(fails, succeeds);
                 }
                 else
                 {
                    var fails = new ConcurrentStack<string>();
                    var succeeds = new ConcurrentStack<string>();

                    // TODO: Parallels for with async action needs to be awaited
                    Parallel.ForEach(linkValues, async (link) =>
                    {
                       if (Config.UpdateSendTargets)
                          if (await SendAsync(box, link).ConfigureAwait(false))
                             succeeds.Push(link.Connection.Name);
                          else
                             fails.Push(link.Connection.Name);
                       else
                          await SendAsync(box, link).ConfigureAwait(false);
                    });

                    if (Config.UpdateSendTargets)
                       targets = new SendTargets(fails, succeeds);
                 }
              }
              if (box.References.Decrement() == 0)
                 Factory.PushBox(box);

              if (Config.UpdateSendTargets)
                 sendTargets.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, targets, (key, value) => value = targets);

              return false;
           }

           return true;
        }

        */
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
