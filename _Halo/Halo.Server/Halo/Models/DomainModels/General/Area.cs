/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Halo.Models
{
    [Description("Los posibles valores son [Null, Rural, Urbana]")]
    public class Area : EnumModel<TipoArea>
    {
        [InverseProperty(nameof(Direccion.Area))]
        public virtual HashSet<Direccion> Direcciones { get; set; }

        public Area() => Direcciones = new HashSet<Direccion>();
    }
}
/* { Halo.Server } */
