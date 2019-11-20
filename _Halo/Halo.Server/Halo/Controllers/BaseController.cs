/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

namespace Halo.Controllers
{
    using Data;

    using Cyxor.Controllers;

    class BaseController : MasterController
    {
        protected HaloDbContext HaloDbContext;

        [ScopeInitializer]
        public virtual void Initialize(HaloDbContext haloDbContext) => HaloDbContext = haloDbContext;
    }
}
/* { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave */
