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
using System.Linq;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Cyxor.Networking.Config.Server
{
    // TODO: Review, SortedSet was replaced by List
    public class CommandConfig : ConfigProperty
    {
        public CommandConfig()
        {
            //allowedIPAddresses = new SortedSet<string>(StringComparer.CurrentCultureIgnoreCase);
            allowedIPAddresses = new List<string>();
            {
                allowedIPAddresses.Add("127.0.0.1");
            }
        }

        bool allowUnrestrictedAccountCreations = false;
        [DefaultValue(false), Description("TODO:")]
        public bool AllowUnrestrictedAccountCreations
        {
            get => allowUnrestrictedAccountCreations;
            set => SetProperty(ref allowUnrestrictedAccountCreations, value);
        }

        bool restrictToSpecifiedAddresses = false;
        [DefaultValue(false)]
        [Description("TODO:")]
        public bool RestrictToSpecifiedAddresses
        {
            get => restrictToSpecifiedAddresses;
            set => SetProperty(ref restrictToSpecifiedAddresses, value);
        }

        List<string> allowedIPAddresses;
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public List<string> AllowedIPAddressesSet
        {
            get => allowedIPAddresses;
            set => SetProperty(ref allowedIPAddresses, value);
        }

        public string[] AllowedIPAddresses
        {
            get => AllowedIPAddressesSet?.ToArray();
            set => AllowedIPAddressesSet = value != null ? new List<string>(value) : null;
        }

        int minimumRequiredSecurityLevel = 0;
        [DefaultValue(0)]
        [Description("TODO:")]
        public int MinimumRequiredSecurityLevel
        {
            get => minimumRequiredSecurityLevel;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(
                       "RequiredSecurityLevelForExpressCommands",
                       minimumRequiredSecurityLevel,
                       "The required security level must be a positive value");

                SetProperty(ref minimumRequiredSecurityLevel, value);
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
