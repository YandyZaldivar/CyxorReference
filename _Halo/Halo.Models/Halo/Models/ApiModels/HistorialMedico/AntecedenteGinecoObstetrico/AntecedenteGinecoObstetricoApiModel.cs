/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System;

namespace Halo.Models
{
    public class AntecedenteGinecoObstetricoApiModel
    {
        public int? Vivos { get; set; }
        public int? Molas { get; set; }
        public int? Abortos { get; set; }
        public int? Muertos { get; set; }
        public int? Cesareas { get; set; }
        public int? Ectopicos { get; set; }
        public int? Gestaciones { get; set; }
        public int? PartosVaginales { get; set; }
        public DateTime? UltimaGestacion { get; set; }
    }
}
/* { Halo.Server } */
