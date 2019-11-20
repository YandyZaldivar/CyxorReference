using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Cyxor.Terminal
{
    using Networking;

    class Program
    {
        static ConcurrentStack<Client> ConnectedClients 
            = new ConcurrentStack<Client>();
        static ConcurrentStack<Client> DisconnectedClients 
            = new ConcurrentStack<Client>();

        static void Main2(string[] args)
        {
            for (var i = 0; i < Environment.ProcessorCount * 2; i++)
                Add();

            for (var i = 0; i < Environment.ProcessorCount * 2; i++)
                Del();

            Console.ReadKey(intercept: true);
        }

        static async void Add()
        {
            await Task.Delay(new Random().Next(100));

            if (!DisconnectedClients.TryPop(out var client))
                client = new Client();

            using (var packet = new Packet(client, "datadin empresa list"))
                await packet.CommandAsync();

            ConnectedClients.Push(client);

            Add();
        }

        static async void Del()
        {
            await Task.Delay(new Random().Next(250));

            if (ConnectedClients.TryPop(out var client))
            {
                await client.DisconnectAsync();
                DisconnectedClients.Push(client);
            }

            Del();
        }
    }
}
























//Client.Instance.Config.Name = "Yandy";
//Client.Instance.Config.InsecurePassword = "nailex";
////Client.Instance.Config.AuthenticationMode = Networking.Config.AuthenticationMode.Basic;

//Client.Instance.Controllers.Register<Controllers.NexusController>(registerAllControllersInTheSameAssembly: true);

//var xy = Client.Instance.Config.IOBufferSize;

//App.Run(Client.Instance, $"{nameof(Cyxor)}.{nameof(Terminal)}");
//Console.WriteLine(App.Result);