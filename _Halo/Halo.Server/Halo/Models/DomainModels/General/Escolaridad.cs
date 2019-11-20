/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Halo.Models
{
    public class Escolaridad : EnumModel<TipoEscolaridad>
    {
        [InverseProperty(nameof(Paciente.Escolaridad))]
        public virtual HashSet<Paciente> Pacientes { get; set; }

        public Escolaridad() => Pacientes = new HashSet<Paciente>();
    }
}
/* { Halo.Server } */
