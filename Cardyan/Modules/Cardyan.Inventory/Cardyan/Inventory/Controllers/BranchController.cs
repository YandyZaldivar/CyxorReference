/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

namespace Cardyan.Inventory.Controllers
{
    using Models;

    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

    public class BranchController : CardyanDbContextController<Branch>
    {
        public IEnumerable<Warehouse> DeepModel()
        {
            var dt = DbContext.Warehouses.Include(p => p.Branch).Include(p => p.Locations).Include(p => p.Products).Include(p => p.Statistic).ToList();

            var warehouse = new Warehouse { BranchId = 36, Code = "ddd" };
            var newWarehouse = AgileObjects.AgileMapper.Mapper.Map(warehouse).ToANew<Warehouse>();

            return dt;
        }
    }
}
/* { Cardyan } - Inventory module */
