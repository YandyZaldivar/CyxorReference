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
    public class AntecedenteGinecoObstetrico
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        public HistorialMedico HistorialMedico { get; set; }

        public int? PartosVaginales { get; set; }

        public int? Gestaciones { get; set; }

        public int? Ectopicos { get; set; }

        public int? Cesarias { get; set; }

        public int? Abortos { get; set; }

        public int? Muertos { get; set; }

        public int? Vivos { get; set; }

        public int? Molas { get; set; }

        [DataType(DataType.Date)]
        public DateTime? UltimaGestacion { get; set; }
    }
}
/* { Halo.Server } */
