/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Halo.Models
{
    public class Parto : EnumModel<TipoParto>
    {
        [InverseProperty(nameof(AtencionHospitalaria.Parto))]
        public HashSet<AtencionHospitalaria> AtencionesHospitalarias { get; set; }

        public Parto() => AtencionesHospitalarias = new HashSet<AtencionHospitalaria>();
    }
}
/* { Halo.Server } */
