/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

namespace Cardyan.Inventory.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;
    using Cyxor.Models;
    using System.Threading.Tasks;

    //public class PropertyTypeController : Controller<PropertyType, int, PropertyTypeApiModel, CardyanDbContext>
    public class PropertyTypeController : CardyanDbContextController<PropertyType>
    {
        [Action(Hide = true)]
        public override Task<PropertyType> Create(PropertyType model) => base.Create(model);

        [Action(Hide = true)]
        public override Task Delete(PropertyType model) => base.Delete(model);

        [Action(Hide = true)]
        public override Task Update(PropertyType model) => base.Update(model);
    }
}
/* { Cardyan } - Inventory module */
