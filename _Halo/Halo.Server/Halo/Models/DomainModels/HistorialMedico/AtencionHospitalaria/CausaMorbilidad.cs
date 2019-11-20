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
    public class CausaMorbilidad
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        public AtencionHospitalaria AtencionHospitalaria { get; set; }

        public bool ComplicacionesAborto { get; set; }

        public bool SepsisOrigenPulmonar { get; set; }

        public bool SepsisOrigenObstetrico { get; set; }

        public bool TrastornosHipertensivos { get; set; }

        public bool SepsisOrigenNoObstetrico { get; set; }

        public bool ComplicacionesHemorragicas { get; set; }

        public bool ComplicacionEnfermedadExistente { get; set; }

        public bool OtraCausa { get; set; }
    }
}
/* { Halo.Server } */
