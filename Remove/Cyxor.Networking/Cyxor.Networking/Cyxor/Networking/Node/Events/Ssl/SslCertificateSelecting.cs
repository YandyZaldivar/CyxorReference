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
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Cyxor.Networking.Events
{
    public sealed class SslCertificateSelectingEventArgs : AsyncActionEventArgs
    {
        public override int EventId => Node.NodeEventsId.SslCertificateSelecting;

        public string TargetHost { get; }
        public Connection Connection { get; }
        public X509Certificate Certificate { get; set; }
        public X509Certificate RemoteCertificate { get; }
        public IEnumerable<string> AcceptableIssuers { get; }
        public X509CertificateCollection LocalCertificates { get; }

        public SslCertificateSelectingEventArgs(Connection connection, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
            : base(connection.Node)
        {
            Connection = connection;
            TargetHost = targetHost;
            LocalCertificates = localCertificates;
            RemoteCertificate = remoteCertificate;
            AcceptableIssuers = acceptableIssuers;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
