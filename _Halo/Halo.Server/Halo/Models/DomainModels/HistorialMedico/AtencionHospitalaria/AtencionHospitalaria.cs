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
    public class AtencionHospitalaria
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        public HistorialMedico HistorialMedico { get; set; }

        public bool? UsoSulfatoMagnesio { get; set; }

        public int? PartoId { get; set; }

        [ForeignKey(nameof(PartoId))]
        public Parto Parto { get; set; }

        public int? MorbilidadPartoId { get; set; }

        [ForeignKey(nameof(MorbilidadPartoId))]
        public MorbilidadParto MorbilidadParto { get; set; }

        public Ocitocico Ocitocico { get; set; }

        public Hemorragia Hemorragia { get; set; }

        public LugarIngreso LugarIngreso { get; set; }

        public CausaMorbilidad CausaMorbilidad { get; set; }

        public CriterioMorbilidad CriterioMorbilidad { get; set; }

        public IntervencionQuirurgica IntervencionQuirurgica { get; set; }
    }
}
/* { Halo.Server } */
