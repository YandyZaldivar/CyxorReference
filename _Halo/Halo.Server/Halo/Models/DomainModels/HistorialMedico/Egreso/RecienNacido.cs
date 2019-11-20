/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Halo.Models
{
    public class RecienNacido
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        public Egreso Egreso { get; set; }

        public int? Peso { get; set; }
        public int? Peso2 { get; set; }

        public int? Apgar { get; set; }
        public int? Apgar2 { get; set; }

        public bool? Fallecido { get; set; }

        public bool? Multiplicidad { get; set; }
    }
}
/* { Halo.Server } */
