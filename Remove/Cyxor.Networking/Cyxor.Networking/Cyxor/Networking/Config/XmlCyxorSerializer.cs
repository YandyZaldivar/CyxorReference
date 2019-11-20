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
using System.Xml.Serialization;

namespace Cyxor.Serialization
{
    public sealed class XmlCyxorSerializer : IBackingSerializer
    {
        public static XmlCyxorSerializer Instance => LazyInstance.Value;
        static Lazy<XmlCyxorSerializer> LazyInstance = new Lazy<XmlCyxorSerializer>(() => new XmlCyxorSerializer());

        Type CurrentType { get; set; }
        XmlSerializer XmlSerializer { get; set; }

        XmlSerializer GetSerializer(Type type)
        {
            if (XmlSerializer == null || CurrentType != type)
                XmlSerializer = new XmlSerializer(CurrentType = type);

            return XmlSerializer;
        }

        public void Serialize(object value, Serializer serializer, bool rawValue = false)
        {
            var stream = new MemoryStream();
            GetSerializer(value.GetType()).Serialize(stream, value);

            if (!rawValue)
                serializer.Serialize(stream);
            else
                serializer.SerializeRaw(stream);
                //serializer.SetData(Utilities.MemoryStream.GetBuffer(stream));
        }

        public T Deserialize<T>(Serializer serializer, Type type = null, bool rawValue = false)
        {
            var position = 0;
            var count = serializer.Length;

            if (!rawValue)
            {
                position = serializer.Position;
                count = serializer.DeserializeInt32();
            }

            var stream = new MemoryStream(serializer.Buffer, position, count, writable: false);
            return (T)GetSerializer(type ?? typeof(T)).Deserialize(stream);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
