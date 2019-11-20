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
using System.Net.Security;
using System.ComponentModel;
using System.Security.Authentication;

namespace Cyxor.Networking.Config
{
#if NET35
    /// <summary>
    /// The EncryptionPolicy to use.
    /// </summary>
    public enum EncryptionPolicy
    {
        /// <summary>
        /// Require encryption and never allow a NULL cipher.
        /// </summary>
        RequireEncryption = 0,

        /// <summary>
        /// Prefer that full encryption be used, but allow a NULL cipher (no encryption) if the server agrees. Has not effect in .NET35.
        /// </summary>
        AllowNoEncryption = 1,

        /// <summary>
        /// Allow no encryption and request that a NULL cipher be used if the other endpoint can handle a NULL cipher. Has not effect in .NET35.
        /// </summary>
        NoEncryption = 2
    }
#endif

    public class SslConfig : ConfigProperty
    {
        public SslConfig() { }

        public const bool DefaultEnabled = false;
        bool enabled = DefaultEnabled;
        [Description("TODO:")]
        [DefaultValue(DefaultEnabled)]
        public bool Enabled
        {
            get => enabled;
            set => SetProperty(ref enabled, value);
        }

        public const bool DefaultCheckCertificateRevocation = true;
        bool checkCertificateRevocation = DefaultCheckCertificateRevocation;
        [Description("TODO:")]
        [DefaultValue(DefaultCheckCertificateRevocation)]
        public bool CheckCertificateRevocation
        {
            get => checkCertificateRevocation;
            set => SetProperty(ref checkCertificateRevocation, value);
        }

        public const SslProtocols DefaultProtocols = SslProtocols.Tls;
        SslProtocols protocols = DefaultProtocols;
        [Description("TODO:")]
        [DefaultValue(DefaultProtocols)]
        public SslProtocols Protocols
        {
            get => protocols;
            set => SetProperty(ref protocols, value);
        }

        public const EncryptionPolicy DefaultEncryptionPolicy = EncryptionPolicy.RequireEncryption;
        EncryptionPolicy encryptionPolicy = DefaultEncryptionPolicy;
        [Description("TODO:")]
        [DefaultValue(DefaultEncryptionPolicy)]
        public EncryptionPolicy EncryptionPolicy
        {
            get => encryptionPolicy;
            set => SetProperty(ref encryptionPolicy, value);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
