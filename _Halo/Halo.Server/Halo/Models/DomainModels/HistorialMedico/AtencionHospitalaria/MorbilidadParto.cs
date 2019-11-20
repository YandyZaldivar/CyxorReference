/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Halo.Models
{
    public class MorbilidadParto : EnumModel<TipoMorbilidadParto>
    {
        [InverseProperty(nameof(AtencionHospitalaria.MorbilidadParto))]
        public HashSet<AtencionHospitalaria> AtencionesHospitalarias { get; set; }

        public MorbilidadParto() => AtencionesHospitalarias = new HashSet<AtencionHospitalaria>();
    }
}
/* { Halo.Server } */
