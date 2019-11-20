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
using System.Security;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

using Newtonsoft.Json;

namespace Cyxor.Networking.Config.Server
{
    using Extensions;
    using Serialization;

    [Description("TODO:")]
    public class SslServerConfig : SslConfig
    {
        public SslServerConfig() { }

        protected override void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            certificate?.Dispose();
            certificate = null;

            base.SetProperty<T>(ref property, value, propertyName);
        }

        [CyxorIgnore]
        X509Certificate certificate;
        [JsonIgnore]
        public X509Certificate Certificate
        {
            get
            {
                if (certificate != null)
                    return certificate;

                if (CertificateFileName == null)
                {
                    if (SecurePassword != null && SecurePassword.Length == 0)
                        certificate = new X509Certificate(CertificateBytes);
                    else
                        certificate = new X509Certificate(CertificateBytes, Password, KeyStorageFlags);
                }
                else if (SecurePassword == null || SecurePassword.Length == 0)
                    certificate = new X509Certificate(CertificateFileName);
                else
                    certificate = new X509Certificate(CertificateFileName, Password, KeyStorageFlags);

                return certificate;
            }
        }

        public const X509KeyStorageFlags DefaultKeyStorageFlags = X509KeyStorageFlags.DefaultKeySet;
        X509KeyStorageFlags keyStorageFlags = DefaultKeyStorageFlags;
        [Description("TODO:")]
        [DefaultValue(DefaultKeyStorageFlags)]
        public X509KeyStorageFlags KeyStorageFlags
        {
            get => keyStorageFlags;
            set => SetProperty(ref keyStorageFlags, value);
        }

        public const bool DefaultClientCertificateRequired = false;
        bool clientCertificateRequired = DefaultClientCertificateRequired;
        [Description("TODO:")]
        [DefaultValue(DefaultClientCertificateRequired)]
        public bool ClientCertificateRequired
        {
            get => clientCertificateRequired;
            set => SetProperty(ref clientCertificateRequired, value);
        }

        public const byte[] DefaultCertificateBytes = null;
        byte[] certificateBytes = DefaultCertificateBytes;
        [Description("TODO:")]
        [DefaultValue(DefaultCertificateBytes)]
        public byte[] CertificateBytes
        {
            get => certificateBytes;
            set => SetProperty(ref certificateBytes, value);
        }

        public const string DefaultCertificateFileName = null;
        string certificateFileName = DefaultCertificateFileName;
        [Description("TODO:")]
        [DefaultValue(DefaultCertificateFileName)]
        public string CertificateFileName
        {
            get => certificateFileName;
            set => SetProperty(ref certificateFileName, value);
        }

        public const string DefaultPassword = "cyxor";
        [Description("TODO:")]
        [DefaultValue(DefaultPassword)]
        public string Password
        {
            get => Security.Utilities.Converter.FromSecureString(securePassword);
            set => SecurePassword = Security.Utilities.Converter.ToSecureString(value);
        }

        [CyxorIgnore]
        SecureString securePassword;
        [JsonIgnore]
        public SecureString SecurePassword
        {
            get => securePassword;
            set => SetProperty(ref securePassword, value);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
