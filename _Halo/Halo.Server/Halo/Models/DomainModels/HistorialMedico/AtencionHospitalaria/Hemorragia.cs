/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Halo.Models
{
    public class Hemorragia
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        public AtencionHospitalaria AtencionHospitalaria { get; set; }

        public bool PrimeraMitad { get; set; }
        public bool SegundaMitad { get; set; }
        public bool Posparto { get; set; }
    }
}
/* { Halo.Server } */
