/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Halo.Models
{
    public class CausaMuerteDirecta : EnumModel<TipoCausaMuerteDirecta>
    {
        [InverseProperty(nameof(Egreso.CausaMuerteDirecta))]
        public HashSet<Egreso> Egresos { get; set; }

        public CausaMuerteDirecta() => Egresos = new HashSet<Egreso>();
    }
}
/* { Halo.Server } */
