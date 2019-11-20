﻿/*
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

    sealed class AccountReset : Serializable, ISerializable
    {
        internal string Email;

        public void Serialize(Serializer serializer)
        {
            serializer.Serialize(Email);
        }

        public void Deserialize(Serializer serializer)
        {
            Email = serializer.DeserializeString();
        }

        internal Result Validate(Node node = null)
        {
            if (string.IsNullOrEmpty(Email))
                return new Result(ResultCode.EmailNullOrEmpty);

            if (!Utilities.IsValidEmailFormat(Email))
                return new Result(ResultCode.EmailInvalidFormat);

            return Result.Success;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
