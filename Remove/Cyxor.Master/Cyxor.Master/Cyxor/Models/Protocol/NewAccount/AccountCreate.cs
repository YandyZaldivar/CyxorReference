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

namespace Cyxor.Networking.Protocol
{
    using Serialization;

    using Utilities = Utilities;

    sealed class AccountCreate : Serializable, ISerializable
    {
        internal string Name;
        internal string Email;
        internal Serializer CustomData;

        public void Serialize(Serializer serializer)
        {
            serializer.Serialize(Name);
            serializer.Serialize(Email);
            serializer.Serialize(CustomData);
        }

        public void Deserialize(Serializer serializer)
        {
            Name = serializer.DeserializeString();
            Email = serializer.DeserializeString();
            CustomData = serializer.DeserializeSerializer();
        }

        internal Result Validate(Node node = null)
        {
            if (string.IsNullOrEmpty(Name))
                return new Result(ResultCode.NameNullOrEmpty);

            if (string.IsNullOrEmpty(Email))
                return new Result(ResultCode.EmailNullOrEmpty);

            if (node != null)
            {
                string name = Name;

                var result = node.Config.Names.Validate(ref name);

                if (!result)
                    return result;

                Name = name;
            }

            if (!Utilities.IsValidEmailFormat(Email))
                return new Result(ResultCode.EmailInvalidFormat);

            return Result.Success;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
