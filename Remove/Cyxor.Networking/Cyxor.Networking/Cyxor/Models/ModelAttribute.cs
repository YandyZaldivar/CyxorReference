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

    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class ModelAttribute : Attribute
    {
        public int Id { get; }
        public string Route { get; }
        public int MaximumBytes { get; set; }
        public string Description { get; set; }
        public PacketProtocol Protocol { get; set; }
        public PacketSerializer Serializer { get; set; }

        public ModelAttribute(object api)
        {
            MaximumBytes = Serialization.Utilities.EncodedInteger.OneByteCap;
            Id = api is string route ? Utilities.HashCode.GetFrom(Route = route.ToLowerInvariant()) : api.GetHashCode();
        }

        public ModelAttribute(object api, string description) : this(api)
            => Description = description;
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
