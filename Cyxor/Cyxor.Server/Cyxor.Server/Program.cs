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

using System.Diagnostics;

namespace Cyxor.Server
{
    using Cyxor.Networking;
    using Cyxor.Networking.Config;
    using Cyxor.Networking.Config.Server;

    using Cyxor.Controllers;
    using System.Threading.Tasks;

    public class Program
    {
        static async void Test()
        {
            //await Task.Delay(3000);

            //Client.Instance.Config.Port = 24100;

            var rr = await Client.Instance.ConnectAsync();

            //var command = Console.ReadLine();

            using (var packet = new Packet(Client.Instance) { Route = "company", Model = new Company { Name = "dddf" } })
            {
                var rt = await packet.QueryAsync();
                var company = packet.Response.GetModel<Company>();


                var company2 = rt.GetModel<Company>();
            }
        }

        public static void Main(string[] args)
        {
            Network.Instance.Config.AppAutoStart = true;
            Network.Instance.Config.Name = $"Cyxor.Server";
            Network.Instance.Config.Plugins.Enabled = true;
            Network.Instance.Config.ExclusiveProcess = true;
            Network.Instance.Config.Database.Enabled = true;
            //Network.Instance.Config.UserVersion = Version.Value;
            Network.Instance.Config.Database.Engine.Enabled = true;
            //Network.Instance.Config.Database.Engine.Port = 12485;
            Network.Instance.Config.Database.Engine.Provider = DatabaseEngineProvider.MySql;
            Network.Instance.Config.ProcessPriorityClass = ProcessPriorityClass.High;

            Network.Instance.Config.File.Mode = FileConfigMode.OpenOrCreate;

            Network.Instance.Config.Loaded += (s, e) =>
            {
                Network.Instance.Config.Port = 24190;
                Network.Instance.Config.Database.Enabled = true;
            };

            App.Run(Network.Instance, args);
        }
    }

    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MyController : Controller
    {
        [Action(Route = "client", IsFullRoute = true)]
        public async Task<Company> Send()
        {
            //Node.Log("client");

            if (!Client.Instance.IsConnected)
            {
                Client.Instance.Config.Port = 24100;
                await Client.Instance.ConnectAsync();
            }

            //using (var packet = new Packet(Client.Instance) { Route = "company", Model = new Company { Name = "dddf" } })
            //using (var packet = new Packet(Client.Instance) { Route = "company", Model = new Company { Name = "dddf" } })
            //    if (await packet.QueryAsync())
            //        return packet.Response.GetModel<Company>();

            using (var packet = new Packet(Client.Instance) { Model = "company {Id: 272, Name: 'Pompeya' }" })
                if (await packet.CommandAsync())
                    return packet.Response.GetModel<Company>(JsonBackingSerializer.Instance);
                {
                    //var value = packet.Response.GetModel<string>();
                    //var company = Utilities.Json.Deserialize<Company>(value);
                    //return company;
                }

            //using (var packet = new Packet(Client.Instance) { Route = "company", Model = new Company { Id = 272, Name = "Pompeya" } })
            //    if (await packet.QueryAsync())
            //        return packet.Response.GetModel<Company>();

            return default;
        }

        int Counter = 0;

        public string Test1()
            => "test1";

        public string Test2(string aa)
            => "test2" + aa;

        [Action(Route = "company", IsFullRoute = true)]
        public Company Test3(Company company)
        {
            //Node.Log("server");
            company.Name += $"_Hanlded_{Counter++}";
            return company;
        }
    }
}
/* { Cyxor } - Core Asynchronous Networking <http://www.cyxor.com/> */
