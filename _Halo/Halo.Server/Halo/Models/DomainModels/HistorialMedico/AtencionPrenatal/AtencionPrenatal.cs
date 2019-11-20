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
    public class AtencionPrenatal
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        public HistorialMedico HistorialMedico { get; set; }

        public Orina Orina { get; set; }

        public Condicion Condicion { get; set; }

        public bool? Reevaluacion { get; set; }

        public int? SemanasCaptacion { get; set; }

        public int? ControlesPrenatales { get; set; }

        public bool? EvaluadoComoRiesgo { get; set; }

        public Hemoglobina Hemoglobina { get; set; }

        public int? IndiceMasaCorporalId { get; set; }

        [ForeignKey(nameof(IndiceMasaCorporalId))]
        public IndiceMasaCorporal IndiceMasaCorporal { get; set; }

        public UltrasonidoGenetico UltrasonidoGenetico { get; set; }
    }
}
/* { Halo.Server } */
