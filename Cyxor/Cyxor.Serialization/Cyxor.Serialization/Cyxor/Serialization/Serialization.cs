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

namespace Cyxor.Serialization
{
    public enum ByteOrder
    {
        LittleEndian,
        BigEndian,
    }

    public interface ISerializable
    {
        void Serialize(Serializer serializer);
        void Deserialize(Serializer serializer);
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class CyxorIgnoreAttribute : Attribute { }

    public class Serializable
    {
        [CyxorIgnore]
        Serializer serializer;
        public virtual Serializer Serializer
        {
            get
            {
                if (serializer == null)
                    serializer = new Serializer();

                serializer.Int32Position = 0;
                serializer.SerializeRaw(this);
                return serializer;
            }
            set
            {
                serializer = value;
                serializer.Int32Position = 0;
                serializer.DeserializeRawObject(this);
            }
        }
    }

    public interface IBackingSerializer
    {
        void Serialize(object value, Serializer serializer, bool rawValue = false);
        TObject Deserialize<TObject>(Serializer serializer, bool rawValue = false);
        object Deserialize(Serializer serializer, Type type, bool rawValue = false);
    }

    public class NullSerializer : IBackingSerializer
    {
        public void Serialize(object value, Serializer serializer, bool rawValue = false)
        {
            if (rawValue)
                serializer.SerializeRaw(default(object));
            else
                serializer.Serialize(default(object));
        }

        public TObject Deserialize<TObject>(Serializer serializer, bool rawValue = false)
            => rawValue ? serializer.DeserializeRawObject<TObject>() : serializer.DeserializeObject<TObject>();

        public object Deserialize(Serializer serializer, Type type, bool rawValue = false)
            => rawValue ? serializer.DeserializeRawObject(type) : serializer.DeserializeObject(type);
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class BackingSerializerAttribute : Attribute
    {
        public static BackingSerializerAttribute Default { get; } = new BackingSerializerAttribute(null);

        public IBackingSerializer BackingSerializer { get; private set; }

        public BackingSerializerAttribute(IBackingSerializer backingSerializer)
        {
            BackingSerializer = backingSerializer;
        }

        public override int GetHashCode() => BackingSerializer.GetHashCode();

        public override bool Equals(object value)
        {
            if (value == this)
                return true;

            var attribute = (value as BackingSerializerAttribute);

            if (attribute != null)
                return BackingSerializer == attribute.BackingSerializer;

            return false;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
