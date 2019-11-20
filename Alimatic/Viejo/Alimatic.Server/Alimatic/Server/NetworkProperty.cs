/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

namespace Alimatic
{
    public partial class Network
    {
        public class NetworkProperty : MasterProperty
        {
            protected Network Network { get; }

            protected NetworkProperty(Network network) : base(network)
            {
                Network = network;
            }
        }
    }
}
/* { Alimatic.Server } */
