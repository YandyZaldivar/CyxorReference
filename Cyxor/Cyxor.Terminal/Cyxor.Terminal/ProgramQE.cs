using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Cyxor.Terminal
{
    using Networking;

    using Newtonsoft.Json;
    using System.Collections.Generic;

    //class A
    //{
    //    public int Id { get; set; } = 889;
    //    public string Name { get; set; } = "ok";
    //    public B B { get; set; } = new B();
    //}

    //class B
    //{
    //    public bool Ok { get; set; } = true;
    //}


    class ProgramQE
    {

        static void MainQE(string[] args)
        {
            //var ss = JsonConvert.SerializeObject(new A());
            //var dd = JsonConvert.DeserializeObject<Dictionary<string, object>>(ss);
            //var dd = JsonConvert.DeserializeObject(ss);
            //var sg = dd.GetType();

            //(dd as Newtonsoft.Json.Linq.JObject).

            var client = new Client();

            client.Config.Proxy.Address = "127.0.0.1";
            client.Config.Proxy.Port = 808;
            client.Config.Proxy.Authentication = Networking.Config.Client.ProxyAuthentication.Basic;
            client.Config.Proxy.Enabled = true;

            client.ConnectAsync();

            Console.ReadKey(intercept: true);
        }

        static void Events_MessageLogged(object sender, Networking.Events.MessageLoggedEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Gray;

            if (e.Category == LogCategory.Error || e.Category == LogCategory.Fatal)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine(e.Message);
            }
        }


        //static async void Send(Client client)
        //{
        //    using (var packet = new Packet(client, "datadin divisiones"))
        //    {
        //        if (!await packet.CommandAsync())
        //            Console.WriteLine(packet.Response.Result.ToString());
        //        else
        //            Console.WriteLine(packet.Response.GetModel<string>().Length);
        //    }
        //}

        //static async void Receive(Client client)
        //{
        //    using (var packet = new Packet(client, "datadin divisiones"))
        //    {
        //        if (!await packet.CommandAsync())
        //            Console.WriteLine(packet.Response.Result.ToString());
        //        else
        //            Console.WriteLine(packet.Response.GetModel<string>().Length);
        //    }
        //}
    }
}







//Client.Instance.Config.Name = "Yandy";
//Client.Instance.Config.InsecurePassword = "nailex";
////Client.Instance.Config.AuthenticationMode = Networking.Config.AuthenticationMode.Basic;

//Client.Instance.Controllers.Register<Controllers.NexusController>(registerAllControllersInTheSameAssembly: true);

//var xy = Client.Instance.Config.IOBufferSize;

//App.Run(Client.Instance, $"{nameof(Cyxor)}.{nameof(Terminal)}");
//Console.WriteLine(App.Result);