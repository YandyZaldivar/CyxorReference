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

namespace Cyxor.Networking.Config
{
    public class FileConfig : ConfigProperty
    {
        public FileConfig()
        {

        }

        public const string DefaultName = "Cyxor.Config.json";
        string name = DefaultName;
        [DefaultValue(DefaultName)]
        [Description("TODO:")]
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public const FileConfigMode DefaultMode = FileConfigMode.None;
        FileConfigMode mode = DefaultMode;
        [Description("TODO:")]
        public FileConfigMode Mode
        {
            get => mode;
            set => SetProperty(ref mode, value);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
