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

using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Cyxor.Networking.Config
{
    using Serialization;

    public class PluginConfig : ConfigProperty
    {
        public PluginConfig() { }

        public const bool DefaultEnabled = false;
        bool enabled = DefaultEnabled;
        [Description("TODO:")]
        [DefaultValue(DefaultEnabled)]
        public bool Enabled
        {
            get => enabled;
            set => SetProperty(ref enabled, value);
        }

        //public const string DefaultPath = "Modules";
        public const string DefaultPath = nameof(NodeConfig.Plugins);
        string path = DefaultPath;
        [DefaultValue(DefaultPath)]
        [Description("TODO:")]
        public string Path
        {
            get => path;
            set => SetProperty(ref path, value);
        }

        [CyxorIgnore]
        HashSet<Assembly> assemblies;
        [Description("TODO:")]
        [JsonIgnore]
        [CyxorIgnore]
        public virtual HashSet<Assembly> Assemblies
        {
            get => assemblies ?? (assemblies = new HashSet<Assembly>());
            set => SetProperty(ref assemblies, value);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
