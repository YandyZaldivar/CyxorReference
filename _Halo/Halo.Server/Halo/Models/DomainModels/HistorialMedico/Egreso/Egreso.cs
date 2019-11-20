/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Newtonsoft.Json;

namespace Halo.Models
{
    [JsonObject(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
    public class Egreso
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        public HistorialMedico HistorialMedico { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Fecha { get; set; }

        public bool? Fallecida { get; set; }

        public RecienNacido RecienNacido { get; set; }

        public int? CausaMuerteDirectaId { get; set; }

        [ForeignKey(nameof(CausaMuerteDirectaId))]
        public CausaMuerteDirecta CausaMuerteDirecta { get; set; }

        public int? CausaMuerteIndirectaId { get; set; }

        [ForeignKey(nameof(CausaMuerteIndirectaId))]
        public CausaMuerteIndirecta CausaMuerteIndirecta { get; set; }
    }
}
/* { Halo.Server } */
