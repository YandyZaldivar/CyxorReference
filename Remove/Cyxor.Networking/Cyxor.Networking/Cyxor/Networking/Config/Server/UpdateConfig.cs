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

namespace Cyxor.Networking.Config.Server
{
    public class UpdateConfig : ConfigProperty
    {
        public UpdateConfig() { }

        string version;
        [DefaultValue(null)]
        [Description("Update files version. This can be used to compare against client version " +
           "for better selection of update files.")]
        public string Version
        {
            get => version;
            set => SetProperty(ref version, value);
        }

        string[] files;
        [DefaultValue(null)]
        [Description("Default needed update files by clients. This value can be customized for each client " +
           "in the server ClientConnecting event.")]
        public string[] Files
        {
            get => files;
            set => SetProperty(ref files, value);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
