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

using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;

using Newtonsoft.Json;

namespace Cyxor.Networking.Config
{
    using Serialization;

    [JsonObject]
    [TypeConverter(typeof(ConfigExpandableObjectConverter))]
    public class ConfigProperty
    {
        [CyxorIgnore]
        NodeConfig rootConfig;
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public virtual NodeConfig RootConfig
        {
            get => rootConfig;
            internal set => rootConfig = value;
        }

        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public Node Node => RootConfig?.Node;

        public virtual void Initialize() { }

        public override string ToString() => null;

        public virtual Result Validate() => Result.Success;

        protected virtual void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
            => RootConfig.SetPropertyInternal(ref property, value, GetType(), propertyName);
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
