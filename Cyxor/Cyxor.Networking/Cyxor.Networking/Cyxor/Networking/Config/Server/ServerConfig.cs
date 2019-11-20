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
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;

namespace Cyxor.Networking.Config.Server
{
    public class ServerConfig : NodeConfig
    {
        public static readonly ServerConfig Default = new ServerConfig(readOnly: true);

        protected ServerConfig(bool readOnly) : this() { IsReadOnly = readOnly; }

        public ServerConfig()
        {
            Address = DefaultAddress;
        }

        [Description("TODO:")]
        public new SslServerConfig Ssl
        {
            get => base.Ssl as SslServerConfig ?? (Ssl = new SslServerConfig());
            set => base.Ssl = value;
        }

        CommandConfig commands;
        public CommandConfig Commands
        {
            get => commands ?? (Commands = new CommandConfig());
            set => SetProperty(ref commands, value);
        }

        UpdateConfig updates;
        public UpdateConfig Updates
        {
            get => updates ?? (Updates = new UpdateConfig());
            set => SetProperty(ref updates, value);
        }

        public new const string DefaultAddress = default;
        [DefaultValue(DefaultAddress)]
        [Description("TODO:")]
        public override string Address
        {
            get => base.Address;
            set => base.Address = value;
        }

        bool dualModeSocket = true;
        [DefaultValue(true)]
        [Description("Gets or sets a System.Boolean value that specifies whether the Server use a dual-mode socket for both IPv4 and IPv6 when listening for incoming connections.")]
        public bool DualModeSocket
        {
            get => dualModeSocket;
            set => SetProperty(ref dualModeSocket, value);
        }

        int backlog = Environment.ProcessorCount * 12;
        [Description("Maximum lenght of the queue of pending connections.")]
        public int Backlog
        {
            get => backlog;
            set => SetProperty(ref backlog, value);
        }

        int maxConcurrentAcceptTasks = Environment.ProcessorCount * 2;
        [Description("Maximum number of simultaneous socket accepting operations.")]
        public int MaxConcurrentAcceptTasks
        {
            get => maxConcurrentAcceptTasks;
            set => SetProperty(ref maxConcurrentAcceptTasks, value);
        }

        int maxConnections = -1;
        [Description("TODO:")]
        public int MaxConnections
        {
            get
            {
                if (maxConnections == -1)
                    return int.MaxValue;

                return maxConnections;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Incorrect number of server MaxConnections.");

                SetProperty(ref maxConnections, value);
            }
        }

        int loginDelaySeconds = 30;
        [Description("TODO:")]
        public int LoginDelaySeconds
        {
            get => loginDelaySeconds;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("The value must be a positive number.");

                SetProperty(ref loginDelaySeconds, value);
            }
        }

        public override Result Validate() =>base .Validate();
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
