/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System;

using Newtonsoft.Json;

namespace Halo.Models
{
    [JsonObject(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
    public class EgresoApiModel
    {
        public bool? Fallecida { get; set; }
        public DateTime? Fecha { get; set; }
        public int? CausaMuerteDirectaId { get; set; }
        public int? CausaMuerteIndirectaId { get; set; }
        public RecienNacidoApiModel RecienNacido { get; set; }
    }
}
/* { Halo.Server } */
