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

    //public class MovementTypeController : Controller<MovementType, int, MovementTypeApiModel, CardyanDbContext>
    public class MovementTypeController : CardyanDbContextController<MovementType>
    {
        [Action(Hide = true)]
        public override Task<MovementType> Create(MovementType model)
            => base.Create(model);

        [Action(Hide = true)]
        public override Task Update(MovementType model)
            => base.Update(model);

        [Action(Hide = true)]
        public override Task Delete(MovementType model)
            => base.Delete(model);
    }
}
/* { Cardyan } - Inventory module */
