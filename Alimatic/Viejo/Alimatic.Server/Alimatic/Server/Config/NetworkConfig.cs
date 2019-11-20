/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System.ComponentModel;
using System.Xml.Serialization;

namespace Alimatic
{
    //using Cyxor.Networking.Config;
    using Cyxor.Networking.Config.Server;

    [XmlType(nameof(NetworkConfig))]
    public class NetworkConfig : MasterConfig
    {
        public static readonly new NetworkConfig Default = new NetworkConfig(readOnly: true);

        //public new const string DefaultName = "Alimatic.PT";

        protected NetworkConfig(bool readOnly) : base(readOnly) { }
        
        public NetworkConfig() { }

        [Description("TODO:")]
        public new NetworkDatabaseConfig Database
        {
            get { return base.Database as NetworkDatabaseConfig ?? (Database = new NetworkDatabaseConfig()); }
            set { base.Database = value; }
        }
    }
}
/* { Alimatic.Server } */
