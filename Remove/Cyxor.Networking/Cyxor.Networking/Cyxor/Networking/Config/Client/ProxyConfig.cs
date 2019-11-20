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

using System.ComponentModel;

namespace Cyxor.Networking.Config.Client
{
    [Description("TODO:")]
    public class ProxyConfig : ConfigProperty
    {
        public ProxyConfig() { }

        public const bool DefaultEnabled = false;
        bool enabled = DefaultEnabled;
        [Description("TODO:")]
        [DefaultValue(DefaultEnabled)]
        public virtual bool Enabled
        {
            get => enabled;
            set => SetProperty(ref enabled, value);
        }

        public const string DefaultAddress = "localhost";
        string address = DefaultAddress;
        [Description("TODO:")]
        public virtual string Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }

        public const int DefaultPort = 3128;
        int port = DefaultPort;
        [DefaultValue(DefaultPort)]
        [Description("TODO:")]
        public virtual int Port
        {
            get => port;
            set => SetProperty(ref port, value);
        }

        public const string DefaultUserName = null;
        string userName = DefaultUserName;
        [Description("TODO:")]
        public virtual string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        public const string DefaultPassword = null;
        string password = DefaultPassword;
        [Description("TODO:")]
        [PasswordPropertyText(true)]
        public virtual string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public const string DefaultDomain = null;
        string domain = DefaultDomain;
        [Description("TODO:")]
        public virtual string Domain
        {
            get => domain;
            set => SetProperty(ref domain, value);
        }

        public const bool DefaultPreAuthenticate = false;
        bool preAuthenticate = DefaultPreAuthenticate;
        [Description("TODO:")]
        [DefaultValue(DefaultPreAuthenticate)]
        public virtual bool PreAuthenticate
        {
            get => preAuthenticate;
            set => SetProperty(ref preAuthenticate, value);
        }

        //public const HttpVersion DefaultHttpVersion = HttpVersion.Http_1_0;
        //HttpVersion httpVersion = DefaultHttpVersion;
        //[Description("TODO:")]
        //[DefaultValue(DefaultHttpVersion)]
        //public virtual HttpVersion HttpVersion { get => httpVersion; set => SetProperty(ref httpVersion, value); }

        public const ProxyAuthentication DefaultAuthentication = ProxyAuthentication.Basic;
        ProxyAuthentication authentication = DefaultAuthentication;
        [Description("TODO:")]
        [DefaultValue(DefaultAuthentication)]
        public virtual ProxyAuthentication Authentication
        {
            get => authentication;
            set => SetProperty(ref authentication, value);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
