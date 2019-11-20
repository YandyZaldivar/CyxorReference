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

        Type CurrentType;
        XmlSerializer XmlSerializer;

        XmlSerializer GetXmlSerializer(Type type)
        {
            if (XmlSerializer == null || CurrentType != type)
                XmlSerializer = new XmlSerializer(CurrentType = type);

            return XmlSerializer;
        }

        public void Serialize(object value, Serializer serializer, bool rawValue = false)
        {
            serializer.StreamWriteRaw = rawValue;
            GetXmlSerializer(value.GetType()).Serialize(serializer, value);
            serializer.StreamWriteRaw = false;
        }

        public T Deserialize<T>(Serializer serializer, bool rawValue = false)
        {
            var count = rawValue ? serializer.Int32Length : serializer.DeserializeInt32();
            var stream = new MemoryStream(serializer.Buffer, serializer.Int32Position, count, writable: false);
            return (T)GetXmlSerializer(typeof(T)).Deserialize(stream);
        }

        public object Deserialize(Serializer serializer, Type type, bool rawValue = false)
        {
            var count = rawValue ? serializer.Int32Length : serializer.DeserializeInt32();
            var stream = new MemoryStream(serializer.Buffer, serializer.Int32Position, count, writable: false);
            return GetXmlSerializer(type).Deserialize(stream);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
