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

    //public class ValuationController : Controller<Valuation, int, ValuationApiModel, CardyanDbContext>
    public class ValuationController : CardyanDbContextController<Valuation>
    {
        [Action(Hide = true)]
        public override Task<Valuation> Create(Valuation model) => base.Create(model);

        [Action(Hide = true)]
        public override Task Update(Valuation model) => base.Update(model);

        [Action(Hide = true)]
        public override Task Delete(Valuation model) => base.Delete(model);
    }
}
/* { Cardyan } - Inventory module */
