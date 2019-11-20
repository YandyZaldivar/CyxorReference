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
using System.ComponentModel;
using System.Xml.Serialization;

using Newtonsoft.Json;

namespace Cyxor.Networking.Config
{
    using Serialization;

    public sealed class PacketConfig : ConfigProperty
    {
        public const int DefaultType = 0;
        public const bool DefaultEncrypt = true;
        public const bool DefaultCompress = false;
        public const PacketProtocol DefaultPacketProtocol = PacketProtocol.Tcp;
        public const PacketSending DefaultPacketSending = PacketSending.Queued;
        public const PacketPriority DefaultPacketPriority = PacketPriority.Exclusive;

        [CyxorIgnore]
        Connection connection;
        [XmlIgnore]
        [JsonIgnore]
        public Connection Connection
        {
            get => connection;
            set => connection = value;
        }

        public int Type { get; set; }
        public bool Compress { get; set; }

        PacketProtocol packetProtocol = DefaultPacketProtocol;
        [Description("TODO:")]
        [DefaultValue(DefaultPacketProtocol)]
        public PacketProtocol PacketProtocol
        {
            get => packetProtocol;
            set => SetProperty(ref packetProtocol, value);
        }

        PacketPriority packetPriority = DefaultPacketPriority;
        [Description("TODO:")]
        [DefaultValue(DefaultPacketPriority)]
        public PacketPriority PacketPriority
        {
            get => packetPriority;
            set => SetProperty(ref packetPriority, value);
        }

        PacketSending packetSending = DefaultPacketSending;
        [Description("TODO:")]
        [DefaultValue(DefaultPacketSending)]
        public PacketSending PacketSending
        {
            get => packetSending;
            set => SetProperty(ref packetSending, value);
        }

        bool encrypt = true;
        [DefaultValue(DefaultEncrypt)]
        [Description("Specifies whether or not to encrypt your data for secure communications. " +
            "This value has no effect if you are not using the integrated SRP authentication protocol." +
            "Alternatively you can provide your own secure symmetric key and initialization vector.")]
        public bool Encrypt
        {
            get => encrypt && RootConfig.EncryptionEnabled;
            set => SetProperty(ref encrypt, value);
        }

        bool internalEncrypt = true;
        [DefaultValue(DefaultEncrypt)]
        [Description("Specifies whether or not to encrypt internal data in secure communications. " +
            "The internal data may represent messages sent in behalf of high level API calls like " +
            "'Client.ConnectAsync'. The same description of 'Encrypt' applies for 'InternalEncrypt'.")]
        public bool InternalEncrypt
        {
            get => internalEncrypt && RootConfig.EncryptionEnabled;
            set => SetProperty(ref internalEncrypt, value);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
