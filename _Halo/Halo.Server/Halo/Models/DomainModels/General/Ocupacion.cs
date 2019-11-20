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
    public class Ocupacion : EnumModel<TipoOcupacion>
    {
        [InverseProperty(nameof(Paciente.Ocupacion))]
        public virtual HashSet<Paciente> Pacientes { get; set; }

        public Ocupacion() => Pacientes = new HashSet<Paciente>();
    }
}
/* { Halo.Server } */
