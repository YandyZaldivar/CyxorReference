/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Halo.Models
{
    public class Hemoglobina
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        public AtencionPrenatal AtencionPrenatal { get; set; }

        public double? PrimerTrimestre { get; set; }

        public double? SegundoTrimestre { get; set; }

        public double? TercerTrimestre { get; set; }
    }
}
/* { Halo.Server } */
