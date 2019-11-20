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
    public class Orina
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        public AtencionPrenatal AtencionPrenatal { get; set; }

        public bool? PrimerTrimestre { get; set; }

        public bool? SegundoTrimestre { get; set; }

        public bool? TercerTrimestre { get; set; }
    }
}
/* { Halo.Server } */
