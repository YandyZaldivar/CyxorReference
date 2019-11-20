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
    public class CausaMuerteIndirecta : EnumModel<TipoCausaMuerteIndirecta>
    {
        [InverseProperty(nameof(Egreso.CausaMuerteIndirecta))]
        public HashSet<Egreso> Egresos { get; set; }

        public CausaMuerteIndirecta() => Egresos = new HashSet<Egreso>();
    }
}
/* { Halo.Server } */
