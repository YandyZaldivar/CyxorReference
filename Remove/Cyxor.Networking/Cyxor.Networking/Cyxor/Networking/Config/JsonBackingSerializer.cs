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
using System.Xml.Linq;

using Newtonsoft.Json;

namespace Cyxor.Networking
{
    using Serialization;
    //using Utilities = Cyxor.Networking.Utilities;

    public class JsonBackingSerializer : IBackingSerializer
    {
        public static JsonBackingSerializer Instance = new JsonBackingSerializer();

        public T Deserialize<T>(Serializer serializer, Type type = null, bool rawValue = false)
        {
            var jsonString = default(string);

            if (rawValue)
                jsonString = serializer.ToString();
            else
                jsonString = serializer.DeserializeString();

            return (T)Utilities.Json.Deserialize(jsonString, type);
        }

        public void Serialize(object value, Serializer serializer, bool rawValue = false)
        {
            var jsonString = Utilities.Json.Serialize(value, includeComments: true);

            if (!rawValue)
                serializer.Serialize(jsonString);
            else
                serializer.SerializeRaw(jsonString);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
