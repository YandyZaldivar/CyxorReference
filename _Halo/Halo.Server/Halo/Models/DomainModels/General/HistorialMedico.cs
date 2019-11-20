/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Newtonsoft.Json;

namespace Halo.Models
{
    [JsonObject(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
    public class HistorialMedico
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        [JsonIgnore]
        public Paciente Paciente { get; set; }

        public Egreso Egreso { get; set; }

        public AtencionPrenatal AtencionPrenatal { get; set; }

        public AtencionHospitalaria AtencionHospitalaria { get; set; }

        public AntecedenteGinecoObstetrico AntecedenteGinecoObstetrico { get; set; }
    }
}
/* { Halo.Server } */
