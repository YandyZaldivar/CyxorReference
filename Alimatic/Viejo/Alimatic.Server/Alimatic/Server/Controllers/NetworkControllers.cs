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
        public partial class NetworkControllers : MasterControllers
        {
            protected Network Network;

            protected internal NetworkControllers(Network network) : base(network)
            {
                Network = network;
            }
        }
    }
}
/* { Alimatic.Server } */
