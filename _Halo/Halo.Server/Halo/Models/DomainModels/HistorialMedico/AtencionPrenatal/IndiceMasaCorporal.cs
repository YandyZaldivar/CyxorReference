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
    public class IndiceMasaCorporal : EnumModel<TipoIndiceMasaCorporal>
    {
        [InverseProperty(nameof(AtencionPrenatal.IndiceMasaCorporal))]
        public HashSet<AtencionPrenatal> AtencionesPrenatales { get; set; }

        public IndiceMasaCorporal() => AtencionesPrenatales = new HashSet<AtencionPrenatal>();
    }
}
/* { Halo.Server } */
