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
using System.ComponentModel;
using System.Collections.Generic;

namespace Cyxor.Networking.Config
{
#if NETSTANDARD1_3 || UAP10_0
    public class ExpandableObjectConverter : TypeConverter { }
#endif

    /// <summary>
    /// Provides a type converter to add the <see cref="ReadOnlyAttribute"/> attribute to properties of network
    /// configuration classes when the property is not marked with the <see cref="ConnectedModifiableAttribute"/>
    /// attribute and the network <see cref="Node"/> is connected.
    /// 
    /// You should use this type converter in network configuration classes that can be used as expandable objects
    /// in controls like a <see cref="System.Windows.Forms.PropertyGrid"/>.
    /// </summary>
    public sealed class ConfigExpandableObjectConverter : ExpandableObjectConverter
    {
        //public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        //{
        //    if (destinationType == typeof(string))
        //        return false;

        //    return base.CanConvertTo(context, destinationType);
        //}

#if !(NETSTANDARD1_3) && !(UAP10_0)
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            var properties = TypeDescriptor.GetProperties(value);
            var propertyList = new List<PropertyDescriptor>(properties.Count);

            var connected = ((value as NodeConfig)?.Node ?? (value as ConfigProperty)?.Node)?.IsConnected ?? false;

            foreach (PropertyDescriptor property in properties)
                if (property.IsBrowsable)
                {
                    if (connected && property.Attributes.Contains(ConnectedModifiableAttribute.Default))
                        propertyList.Add(TypeDescriptor.CreateProperty(value.GetType(), property, ReadOnlyAttribute.Yes));
                    else
                        propertyList.Add(property);
                }

            return new PropertyDescriptorCollection(propertyList.ToArray());
        }
#endif
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
