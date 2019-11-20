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
    public class LugarIngreso
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        public AtencionHospitalaria AtencionHospitalaria { get; set; }

        public bool CuidadosPerinatales { get; set; }

        public bool UnidadCuidadosIntensivos { get; set; }
    }
}
/* { Halo.Server } */
