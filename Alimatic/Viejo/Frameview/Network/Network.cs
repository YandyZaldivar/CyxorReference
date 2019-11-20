/*
  { Frameview } - Sistema de videoconferencia por imágenes
  Copyright (C) 2018 Alimatic
  Authors:  Ramón Menéndez
            Yandy Zaldivar
*/

using System;

namespace Frameview
{
    using Cyxor.Networking;

    class Network : Client
    {
        public static new Network Instance => LazyInstance.Value;
        static Lazy<Network> LazyInstance = new Lazy<Network>(() => new Network());

        public bool Active { get; set; }
        public bool IsMaster { get; set; }

        public new NetworkConfig Config
        {
            get => base.Config as NetworkConfig;
            protected set => base.Config = value;
        }

        private Network()
        {
            Config = new NetworkConfig();
        }
    }
}
/* { Frameview } - Sistema de videoconferencia por imágenes */
