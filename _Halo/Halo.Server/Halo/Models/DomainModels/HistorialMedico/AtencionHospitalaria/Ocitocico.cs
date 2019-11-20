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
    public class Ocitocico
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        public AtencionHospitalaria AtencionHospitalaria { get; set; }

        public bool Ocitocina { get; set; }
        public bool Ergonovina { get; set; }
        public bool Misoprostol { get; set; }
        public bool AcidoTranexamico { get; set; }
    }
}
/* { Halo.Server } */
