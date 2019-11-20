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

namespace Cyxor.Models
{
    using Networking;
    using Serialization;

    //[PacketConfig(InternalCoreApiId.Login, @internal: true)]
    public sealed class LoginResponse : ISerializable
    {
        internal string Name;
        internal string SrpM;
        internal Guid UdpKey;
        internal Result Result;
        internal bool ResetFlag;
        internal bool FirstLogin;
        internal bool UdpAllowed;
        internal bool UpdateRequired;
        internal Serializer CustomData;

        public void Serialize(Serializer serializer)
        {
            serializer.Serialize(Name);
            serializer.Serialize(SrpM);
            serializer.Serialize(UdpKey);
            serializer.Serialize(ResetFlag);
            serializer.Serialize(CustomData);
            serializer.Serialize(FirstLogin);
            serializer.Serialize(UdpAllowed);
            serializer.Serialize(UpdateRequired);
            serializer.Serialize((ISerializable)Result);
        }

        public void Deserialize(Serializer serializer)
        {
            Name = serializer.DeserializeString();
            SrpM = serializer.DeserializeString();
            UdpKey = serializer.DeserializeGuid();
            ResetFlag = serializer.DeserializeBoolean();
            CustomData = serializer.DeserializeSerializer();
            FirstLogin = serializer.DeserializeBoolean();
            UdpAllowed = serializer.DeserializeBoolean();
            UpdateRequired = serializer.DeserializeBoolean();
            Result = serializer.DeserializeObject<Result>();
        }

        internal void Reset()
        {
            Name = null;
            SrpM = null;
            FirstLogin = false;
            UdpAllowed = false;
            UpdateRequired = false;
            Result = Result.Success;

            if (CustomData != null)
                CustomData.Reset();
        }

        public Result Validate(Node node)
        {
            return Result.Success;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
