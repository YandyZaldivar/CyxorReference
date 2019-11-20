using System;

namespace Nexus.Terminal
{
    using Cyxor;
    using Cyxor.Networking;

    using Alimatic.Nexus.Models;

    class Program
    {
        static async void TestAsync()
        {
            var xx = await Client.Instance.ConnectAsync();

            using (var packet = new Packet(Client.Instance) { Id = ApiId.InitialData, Model = "x" })
            {
                var result = await packet.QueryAsync();

                //var cu = packet.Result.Reply.GetMessage<InitialDataApiModel>();
            }
        }

        static void Main(string[] args)
        {
            //App.Run(Client.Instance, args: args);

            TestAsync();

            Console.WriteLine("Press a key to exit...");
            Console.ReadKey(intercept: true);
        }
    }
}