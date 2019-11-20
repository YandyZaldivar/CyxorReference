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
using System.Net.NetworkInformation;

namespace Cyxor.Networking
{
    public abstract partial class Node
    {
        public abstract class NodeNetworkInformation : NodeProperty
        {
            internal abstract void Internal();

            internal NodeNetworkInformation(Node node)
               : base(node) { }

            public static NetworkInterface PreferredNic;

            public EndPoint LocalEndPoint => Node.LocalEndPoint;

            public static string LocalHostName => Dns.GetHostName();

            // TODO: RESTORE THIS
            //public static bool IsNetworkAvailable { get; } = NetworkInterface.GetIsNetworkAvailable();

            static NodeNetworkInformation()
            {
                // TODO: RESTORE THIS
                //PreferredNic = UpdatePreferredAdapter();
                //NetworkChange.NetworkAddressChanged += (sender, e) => PreferredNic = UpdatePreferredAdapter();
            }

            public static IPAddress GetPreferredLocalIPAddress(bool getIPv6Address)
            {
                if (PreferredNic == null)
                    return default;

                var iPInterfaceProperties = PreferredNic.GetIPProperties();
                var addressFamily = getIPv6Address ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;

                foreach (var unicastIPAddressInformation in iPInterfaceProperties.UnicastAddresses)
                    if (unicastIPAddressInformation.Address.AddressFamily == addressFamily)
                        return unicastIPAddressInformation.Address;

                if (PreferredNic.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    return addressFamily == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Loopback : IPAddress.Loopback;

                return default;
            }

            public static IPAddress PreferredLocalIPv6Address => GetPreferredLocalIPAddress(getIPv6Address: true);
            public static IPAddress PreferredLocalIPv4Address => GetPreferredLocalIPAddress(getIPv6Address: false);

            //public static IPAddress PreferredLocalIPAddress
            //{
            //    get
            //    {
            //        if (Socket.OSSupportsIPv6)
            //            return PreferredLocalIPv6Address;

            //        return PreferredLocalIPv4Address;
            //    }
            //}

            public virtual Result Validate()
            {
#if NET35
                if (!Socket.OSSupportsIPv6 && !Socket.SupportsIPv4)
#else
                if (!Socket.OSSupportsIPv6 && !Socket.OSSupportsIPv4)
#endif
                    return new Result(ResultCode.NetworkIPUnsupported);

                var client = Node as Client;

                var endPoint = client != null ? client.RemoteEndPoint : Node.LocalEndPoint;

                // TODO: RESTORE THIS
                //if (!IsNetworkAvailable)
                //    if (client != null)
                //        if (endPoint is IPEndPoint)
                //            if (!IPAddress.IsLoopback((endPoint as IPEndPoint).Address))
                //                return new Result(ResultCode.NetworkUnavailable);

                return Result.Success;
            }

            static NetworkInterface UpdatePreferredAdapter()
            {
                return null;

                // TODO: RESTORE THIS
                //var bestLevel = 0;
                //var nics = NetworkInterface.GetAllNetworkInterfaces();

                //void ChooseNic(NetworkInterface nic, int level)
                //{
                //    if (bestLevel < level)
                //    {
                //        bestLevel = level;
                //        PreferredNic = nic;
                //    }
                //    else if (bestLevel == level)
                //        if (PreferredNic.Speed < nic.Speed)
                //            PreferredNic = nic;
                //}

                //foreach (var nic in nics)
                //{
                //    if (nic.OperationalStatus == OperationalStatus.Up)
                //    {
                //        if (PreferredNic == null)
                //            PreferredNic = nic;

                //        if (nic.Supports(NetworkInterfaceComponent.IPv4) && nic.Supports(NetworkInterfaceComponent.IPv6) &&
                //            nic.NetworkInterfaceType != NetworkInterfaceType.Loopback && nic.NetworkInterfaceType != NetworkInterfaceType.Unknown)
                //            ChooseNic(nic, 4);
                //        else if (nic.Supports(NetworkInterfaceComponent.IPv4) && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                //            nic.NetworkInterfaceType != NetworkInterfaceType.Unknown)
                //            ChooseNic(nic, 3);
                //        else if (nic.NetworkInterfaceType != NetworkInterfaceType.Loopback && nic.NetworkInterfaceType != NetworkInterfaceType.Unknown)
                //            ChooseNic(nic, 2);
                //        else if (nic.NetworkInterfaceType != NetworkInterfaceType.Unknown)
                //            ChooseNic(nic, 1);
                //        else
                //            ChooseNic(nic, 0);
                //    }
                //}

                //return PreferredNic;
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
