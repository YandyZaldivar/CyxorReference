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
using System.ComponentModel;

namespace Cyxor.Networking.Config
{
    public sealed class LogConfig : ConfigProperty
    {
        public const bool DefaultEnabled = false;
        public const string DefaultFileName = null;
        public const LogCategory DefaultMinLogCategory = LogCategory.Message;

        //Stream FileStream;
        internal TextWriter TextWriter { get; private set; }

        public LogConfig() { }

        bool enabled = DefaultEnabled;
        [DefaultValue(DefaultEnabled)]
        [Description("TODO:")]
        public bool Enabled
        {
            get => enabled;
            set => SetProperty(ref enabled, value);
        }

        string fileName = DefaultFileName;
        [DefaultValue(DefaultFileName)]
        [Description("TODO:")]
        public string FileName
        {
            get => fileName;
            set => SetProperty(ref fileName, value);
        }

        LogCategory minLogCategory = DefaultMinLogCategory;
        [DefaultValue(DefaultMinLogCategory)]
        [Description("TODO:")]
        public LogCategory MinLogCategory
        {
            get => minLogCategory;
            set => SetProperty(ref minLogCategory, value);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
