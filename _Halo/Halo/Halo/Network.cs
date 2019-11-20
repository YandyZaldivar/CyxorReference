/*
  { Halo } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System;
using System.Collections.Generic;

namespace Halo
{
    using Models;
    using Cyxor.Networking;

    class Network : Client
    {
        public static new Network Instance => LazyInstance.Value;
        static Lazy<Network> LazyInstance = new Lazy<Network>(() => new Network());

        //SortedDictionary<int, PacienteApiModel> Pacientes = new SortedDictionary<int, PacienteApiModel>();
    }
}
/* { Halo } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave */
