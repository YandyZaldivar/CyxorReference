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
    public class FallaOrganica
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        public CriterioMorbilidad CriterioMorbilidad { get; set; }

        public bool Renal { get; set; }

        public bool Cardiaca { get; set; }

        public bool Vascular { get; set; }

        public bool Hepatica { get; set; }

        public bool Cerebral { get; set; }

        public bool Metabolica { get; set; }

        public bool Coagulacion { get; set; }

        public bool Respiratoria { get; set; }
    }
}
/* { Halo.Server } */
