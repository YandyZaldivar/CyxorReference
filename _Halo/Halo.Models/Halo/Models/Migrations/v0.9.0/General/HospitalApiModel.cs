/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

namespace Halo.Models.Migrations.V0900
{
    using Cyxor.Models;

    public class HospitalApiModel : IdNombreApiModel
    {
        public int ProvinciaId { get; set; }
    }
}
/* { Halo.Server } */
