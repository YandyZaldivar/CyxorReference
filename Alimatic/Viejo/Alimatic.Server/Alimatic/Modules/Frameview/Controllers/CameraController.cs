/*
  { Alimatic.Server.Frameview } - Sistema de videoconferencia por imágenes
  Copyright (C) 2018 Alimatic
  Authors:  Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Alimatic.Frameview.Controllers
{
    using Cyxor.Models;
    using Cyxor.Networking;
    using Cyxor.Controllers;
    using Cyxor.Networking.Events.Server;

    [Model("camera send")]
    class Frame
    {
        public string Name { get; set; }
        public byte[] Bytes { get; set; }
    }

    class CameraController : Controller
    {
        static Frame Frame;
        static Connection MasterConnection;
        static string MasterName = "Master";
        static string MasterFile = "Master.txt";
        static ConcurrentDictionary<string, Connection> ClientNames;
        static ConcurrentDictionary<Connection, Frame> ClientFrames;

        static CameraController()
        {
            ClientNames = new ConcurrentDictionary<string, Connection>();
            ClientFrames = new ConcurrentDictionary<Connection, Frame>();
            Network.Instance.Events.ClientDisconnected += ClientDisconnected;

            if (File.Exists(MasterFile))
                MasterName = File.ReadAllText(MasterFile);
        }

        public void SetMaster(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Contains(' ') || name.Length < 2)
                throw new ArgumentException("The camera master name must contains no spaces and be at least two characters long");

            MasterName = name;
            File.WriteAllText(MasterFile, name);
        }

        public string GetMaster() => MasterName;

        static async void ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            if (e.Connection == MasterConnection)
            {
                Frame.Bytes = null;

                var connections = new List<Connection>(ClientFrames.Select(p => p.Key));

                if (connections.Count > 0)
                    using (var packet = new Packet(connections, "camera leave", Frame.Name))
                        await packet.SendAsync();

                MasterConnection = null;
                ClientNames.TryRemove(MasterName, out var connection);
            }
            else if (ClientFrames.TryRemove(e.Connection, out var frame))
            {
                if (MasterConnection?.Active ?? false)
                    using (var packet = new Packet(MasterConnection, "camera leave", frame.Name))
                        await packet.SendAsync();

                ClientNames.TryRemove(frame.Name, out var connection);
            }
        }

        static async void ClientLoop(Connection connection)
        {
            if (!connection.Active)
                return;

            var bytes = Frame?.Bytes;

            if (bytes != null)
                using (var packet = new Packet(connection) { Model = Frame })
                    await packet.QueryAsync();

            while (connection.Active && bytes == Frame?.Bytes)
                await Task.Delay(10);

            ClientLoop(connection);
        }

        static async void MasterLoop(Connection connection, Frame frame)
        {
            if (!connection.Active)
                return;

            var bytes = frame.Bytes;

            var masterConnection = MasterConnection;

            if (bytes != null && (masterConnection?.Active ?? false))
                using (var packet = new Packet(masterConnection) { Model = frame })
                    await packet.QueryAsync();

            while (connection.Active && bytes == frame.Bytes)
                await Task.Delay(10);

            MasterLoop(connection, frame);
        }

        public bool Connect(string name)
        {
            var index = name.IndexOf('@');
            var masterName = default(string);

            if (index != -1)
            {
                masterName = name.Substring(index + 1);
                name = name.Substring(0, index);
            }

            if (!ClientNames.TryAdd(name, Connection))
            {
                Connection.DisconnectAsync(new Result(comment: $"El nombre de cliente '{name}' ya está en uso."));
                return false;
            }

            if (masterName == MasterName)
            {
                MasterConnection = Connection;
                Frame = new Frame { Name = name };
                return true;
            }
            else
            {
                var frame = new Frame { Name = name };
                ClientFrames.TryAdd(Connection, frame);
                MasterLoop(Connection, frame);
                ClientLoop(Connection);
                return false;
            }
        }

        public void Send(Frame frame)
        {
            //if (frame.Name == MasterName)
            if (Connection == MasterConnection)
                Frame.Bytes = frame.Bytes;
            else
            {
                ClientFrames.TryGetValue(Connection, out var clientFrame);
                clientFrame.Bytes = frame.Bytes;


            }
        }
    }
}
/* { Alimatic.Server.Frameview } - Sistema de videoconferencia por imágenes */
