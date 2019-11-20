/*
  { Cyxor } - Core Asynchronous Networking <http://www.cyxor.com/>
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

namespace Cyxor.Server
{
    using Cyxor.Networking;
    using Cyxor.Networking.Config;
    using Cyxor.Networking.Config.Server;

    public class Program
    {
        public static void Main(string[] args)
        {
            Network.Instance.Config.AppAutoStart = true;
            Network.Instance.Config.Name = $"Cyxor.Server";
            Network.Instance.Config.Plugins.Enabled = true;
            Network.Instance.Config.ExclusiveProcess = true;
            Network.Instance.Config.Database.Enabled = true;
            Network.Instance.Config.UserVersion = Version.Value;
            Network.Instance.Config.Database.Engine.Enabled = true;
            Network.Instance.Config.File.Mode = FileConfigMode.OpenOrCreate;
            Network.Instance.Config.Database.Engine.Provider = DatabaseEngineProvider.MySql;
            Network.Instance.Config.ProcessPriorityClass = System.Diagnostics.ProcessPriorityClass.High;

            App.Run(Network.Instance, args);
        }
    }
}
/* { Cyxor } - Core Asynchronous Networking <http://www.cyxor.com/> */
