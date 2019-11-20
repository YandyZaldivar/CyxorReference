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

namespace Cyxor.Models
{
    using Networking;
    using Serialization;

    //[PacketConfig(InternalCoreApiId.Login, @internal: true)]
    [Model("login", Description = "TODO:")]
    public sealed class LoginRequest : ISerializable
    {
        internal string Name;
        internal string Srp_M;
        internal bool Reset;
        internal bool NoDelay;
        internal bool UdpEnabled;
        internal Serializer CustomData;
        internal ClientServices Services;
        internal string UserVersion = "1.0.0"; // TODO: Retrieve from configuration
        internal string CyxorVersion = Networking.Version.Value;

        public void Serialize(Serializer serializer)
        {
            serializer.Serialize(Name);
            serializer.Serialize(Srp_M);
            serializer.Serialize(Reset);
            serializer.Serialize(NoDelay);
            serializer.Serialize(Services);
            serializer.Serialize(UdpEnabled);
            serializer.Serialize(UserVersion);
            serializer.Serialize(CyxorVersion);
            serializer.Serialize(CustomData);
        }

        public void Deserialize(Serializer serializer)
        {
            Name = serializer.DeserializeString();
            Srp_M = serializer.DeserializeString();
            Reset = serializer.DeserializeBoolean();
            NoDelay = serializer.DeserializeBoolean();
            Services = serializer.DeserializeEnum<ClientServices>();
            UdpEnabled = serializer.DeserializeBoolean();
            UserVersion = serializer.DeserializeString();
            CyxorVersion = serializer.DeserializeString();
            CustomData = serializer.DeserializeSerializer();
        }

        public Result Validate(Node node)
        {
            return Result.Success;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
