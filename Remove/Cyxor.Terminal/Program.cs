using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Cyxor.Terminal
{
    using Networking;
    using Newtonsoft.Json.Schema;

    enum Operation
    {
        None,
        Connect,
        Disconnect,
    }

    class Program
    {
        


        static Operation Operation;
        static ConcurrentStack<Client> ConnectedClients = new ConcurrentStack<Client>();
        static ConcurrentStack<Client> DisconnectedClients = new ConcurrentStack<Client>();
        //static Utilities.Threading.InterlockedInt Counter = new Utilities.Threading.InterlockedInt();
        //static Utilities.Threading.InterlockedInt OperationInterlocked = new Utilities.Threading.InterlockedInt();

        //static Operation SetOperation(Operation operation)
        //{
        //    if (OperationInterlocked.Value == 0)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Red;
        //        Console.WriteLine("Error, Operation None");
        //    }
        //    else if (operation == Operation.Connect)
        //    {
        //        if (OperationInterlocked.CompareExchange(1, 2)
        //    }
        //}

        static void Main()
        {
            Operation = Operation.Connect;

            for (var i = 0; i < 10; i++)
                DisconnectedClients.Push(new Client());

            for (var i = 0; i < Environment.ProcessorCount * 2; i++)
                Connect();

            for (var i = 0; i < Environment.ProcessorCount * 2; i++)
                Disconnect();

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

        static async void Connect()
        {
            try
            {
                //await Utilities.Task.Delay(new Random().Next(100));

                if (Operation == Operation.None)
                    return;
                else if (Operation == Operation.Disconnect)
                {
                    await Utilities.Task.Delay(100);
                    Connect();
                    return;
                }

                if (!DisconnectedClients.TryPop(out var client))
                {
                    Operation = Operation.Disconnect;
                    return;
                }
                else
                {
                    client = new Client();
                    client.Events.MessageLogged += Events_MessageLogged;
                }

                var result = Result.Success;

                if (!(result = await client.ConnectAsync()))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("%%%%%%%%%%%%%" + result.ToString());
                }
                else
                {
                    //Counter.Increment();
                    //Console.ForegroundColor = ConsoleColor.DarkGreen;
                    //Console.WriteLine($"{Counter}: Connected");

                    using (var packet = new Packet(client, "datadin2 divisiones"))
                    {
                        if (!await packet.CommandAsync())
                        {
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.WriteLine(packet.Response.Result.ToString());
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(packet.Response.GetModel<string>().Length);
                        }
                    }

                    ConnectedClients.Push(client);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(ex.ToString());
                Operation = Operation.None;
                return;
            }

            Connect();
        }

        static async void Disconnect()
        {
            try
            {
                //await Utilities.Task.Delay(new Random().Next(100));

                //await Utilities.Task.Delay(100);

                if (Operation == Operation.None)
                    return;
                else if (Operation == Operation.Connect)
                {
                    await Utilities.Task.Delay(100);
                    Disconnect();
                    return;
                }

                if (ConnectedClients.TryPop(out var client))
                {
                    await client.DisconnectAsync();

                    //Counter.Decrement();
                    //Console.ForegroundColor = ConsoleColor.DarkGreen;
                    //Console.WriteLine($"{Counter}: Disconnected");

                    DisconnectedClients.Push(client);
                }
                else
                {
                    Operation = Operation.Connect;
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(ex.ToString());
                Operation = Operation.None;
                return;
            }

            Disconnect();
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