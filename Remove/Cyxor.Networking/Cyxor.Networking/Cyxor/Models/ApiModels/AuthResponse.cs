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

    public sealed class AuthResponse : ISerializable
    {
        public string s { get; set; }
        public string B { get; set; }
        public int? Tag { get; set; }
        public string Token { get; set; }

        public Result Result { get; set; }

        public void Serialize(Serializer serializer)
        {
            serializer.Serialize(s);
            serializer.Serialize(B);
            serializer.Serialize(Tag);
            serializer.Serialize(Token);
            serializer.Serialize((ISerializable)Result);
        }

        public void Deserialize(Serializer serializer)
        {
            s = serializer.DeserializeString();
            B = serializer.DeserializeString();
            Tag = serializer.DeserializeNullableInt32();
            Token = serializer.DeserializeString();
            Result = serializer.DeserializeObject<Result>();
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
