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
using System.Xml.Linq;

using Newtonsoft.Json;

namespace Cyxor.Networking
{
    using Serialization;

    public class XmlBackingSerializer : IBackingSerializer
    {
        public static XmlBackingSerializer Instance = new XmlBackingSerializer();

        public T Deserialize<T>(Serializer serializer, Type type = null, bool rawValue = false)
        {
            var xmlString = rawValue ? serializer.ToString() : serializer.DeserializeString();

            var jsonString = JsonConvert.SerializeXNode(XDocument.Parse(xmlString), Formatting.None, omitRootObject: true);

            return (T)JsonConvert.DeserializeObject(jsonString, type);
        }

        public void Serialize(object value, Serializer serializer, bool rawValue = false)
        {
            var jsonString = JsonConvert.SerializeObject(value);

            var xDocument = JsonConvert.DeserializeXNode(jsonString, value.GetType().Name, writeArrayAttribute: true);

            if (!rawValue)
                serializer.Serialize(xDocument.ToString());
            else
                serializer.SerializeRaw(xDocument.ToString());
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
