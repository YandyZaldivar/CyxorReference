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
    public class EnfermedadEspecifica
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        public CriterioMorbilidad CriterioMorbilidad { get; set; }

        public bool Eclampsia { get; set; }

        public bool ShockSeptico { get; set; }

        public bool ShockHipovolemico { get; set; }
    }
}
/* { Halo.Server } */
