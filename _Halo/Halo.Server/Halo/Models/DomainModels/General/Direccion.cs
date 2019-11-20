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
    public class Direccion
    {
        [Key]
        public int PacienteId { get; set; }

        [ForeignKey(nameof(PacienteId))]
        public Paciente Paciente { get; set; }

        public int? AreaId { get; set; }

        [ForeignKey(nameof(AreaId))]
        public Area Area { get; set; }

        public int? MunicipioId { get; set; }

        [ForeignKey(nameof(MunicipioId))]
        public Municipio Municipio { get; set; }

        public int? ProvinciaId => Provincia?.Id;

        public Provincia Provincia => Municipio?.Provincia;
    }
}
/* { Halo.Server } */
