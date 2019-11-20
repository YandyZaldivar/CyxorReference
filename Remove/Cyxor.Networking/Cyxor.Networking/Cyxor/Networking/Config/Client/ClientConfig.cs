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
using System.Xml.Serialization;

using Newtonsoft.Json;

namespace Cyxor.Networking.Config.Client
{
    using Serialization;

    public class ClientConfig : NodeConfig
    {
        public static readonly ClientConfig Default = new ClientConfig(readOnly: true);

        protected ClientConfig(bool readOnly) : this()
        {
            IsReadOnly = readOnly;
        }

        public ClientConfig()
        {
            Address = DefaultAddress;
        }

        public new const string DefaultAddress = "localhost";
        [DefaultValue(DefaultAddress)]
        [Description("TODO:")]
        public override string Address
        {
            get => base.Address;
            set => base.Address = value;
        }

        public const string DefaultRemoteCommandTokenName = "server";
        string remoteCommandTokenName = DefaultRemoteCommandTokenName;
        [DefaultValue(DefaultRemoteCommandTokenName)]
        [Description("TODO:")]
        public string RemoteCommandTokenName
        {
            get => remoteCommandTokenName;
            set => SetProperty(ref remoteCommandTokenName, value);
        }

        ClientServices services;
        public ClientServices Services
        {
            get => services;
            set => SetProperty(ref services, value);
        }

        bool loginReset;
        public bool LoginReset
        {
            get => loginReset;
            set => SetProperty(ref loginReset, value);
        }

        Serializer customData;
        [XmlIgnore]
        [JsonIgnore]
        public Serializer CustomData
        {
            get => customData;
            set => SetProperty(ref customData, value);
        }

        //string activationCode;
        //// TODO: Make secure

        //public string ActivationCode
        //{
        //   get { return activationCode; }
        //   set { SetProperty(ref activationCode, value); }
        //}

        ProxyConfig proxy;
        [Description("TODO:")]
        [DefaultValue(null)]
        [Category(nameof(ConfigProperty))]
        public ProxyConfig Proxy
        {
            get => proxy ?? (Proxy = new ProxyConfig());
            set => SetProperty(ref proxy, value);
        }

        [Description("TODO:")]
        [Category(nameof(ConfigProperty))]
        public new SslClientConfig Ssl
        {
            get => base.Ssl as SslClientConfig ?? (Ssl = new SslClientConfig());
            set => base.Ssl = value;
        }

        int syncTimeout = 3000;
        public int SyncTimeout
        {
            get => syncTimeout;
            set => SetProperty(ref syncTimeout, value);
        }

        const bool DefaultTcpNoDelay = false;
        bool tcpNoDelay = DefaultTcpNoDelay;
        [DefaultValue(DefaultTcpNoDelay)]
        [Description("TODO:")]
        public bool TcpNoDelay
        {
            get => tcpNoDelay;
            set => SetProperty(ref tcpNoDelay, value);
        }

        //public LingerOption LingerOption
        //{
        //   get;
        //   set; 
        //}

        const bool DefaultSavePassword = false;
        bool savePassword = DefaultSavePassword;
        [DefaultValue(DefaultSavePassword)]
        [Description("TODO:")]
        public bool SavePassword
        {
            get => savePassword;
            set
            {
                SetProperty(ref savePassword, value);

                if (savePassword)
                    StoredPassword = InsecurePassword;
            }
        }

        const string DefaultStoredPassword = null;
        string storedPassword = DefaultStoredPassword;
        //[PasswordPropertyText]
        [DefaultValue(DefaultStoredPassword)]
        [Description("TODO:")]
        public string StoredPassword
        {
            get => storedPassword;
            set
            {
                SetProperty(ref storedPassword, value);

                if (savePassword)
                    InsecurePassword = storedPassword;
            }
        }

        SecureString password = new SecureString();
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public SecureString Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);

                password?.MakeReadOnly();

                if (savePassword)
                    storedPassword = InsecurePassword;
            }
        }

        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public string InsecurePassword
        {
            get => Security.Utilities.Converter.FromSecureString(Password);
            set => Password = Security.Utilities.Converter.ToSecureString(value);
        }

        SecureString activationCode = new SecureString();
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public SecureString ActivationCode
        {
            get => activationCode;
            set => SetProperty(ref activationCode, value);
        }

        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public string InsecureActivationCode
        {
            get => Security.Utilities.Converter.FromSecureString(activationCode);
            set => ActivationCode = Security.Utilities.Converter.ToSecureString(value);
        }

        public override Result Validate()
        {
            var result = base.Validate();

            if (!result)
                return result;

            if (Password.Length > 0)
                if (string.IsNullOrEmpty(Name))
                    return new Result(ResultCode.NameNullOrEmpty);

            return result;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
