/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

namespace Halo.Models.Migrations.V0900
{
    public class RecienNacidoApiModel
    {
        public int? Peso { get; set; }
        public int? Apgar1 { get; set; }
        public int? Apgar2 { get; set; }
        public bool? Fallecido { get; set; }
        public int? EdadGestacional { get; set; }
    }
}
/* { Halo.Server } */
