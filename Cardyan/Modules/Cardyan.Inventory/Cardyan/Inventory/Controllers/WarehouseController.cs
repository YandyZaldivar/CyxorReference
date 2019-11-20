/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Cardyan.Inventory.Controllers
{
    using Data;
    using Models;

    using Cyxor.Models;
    using Cyxor.Controllers;

    //public class WarehouseController : Controller<Warehouse, int, Warehouse, CardyanDbContext>
    public class WarehouseController : CardyanDbContextController<Warehouse>
    {
        //public Task<ResponseListApiModel<WarehouseProductApiModel>> Products(TKeyListApiModel<int> apiModel)
        //    => List<WarehouseProductApiModel, WarehouseProduct>(apiModel, DbContext.WarehouseProducts.Where(p => p.WarehouseId == apiModel.Id));

        //public async Task<WarehouseStatisticApiModel> Statistic(KeyApiModel<int> apiModel)
        //    => FromModel<WarehouseStatisticApiModel, WarehouseStatistic>((await DbContext.Warehouses.Include(p => p.Statistic).SingleAsync(p => p.Id == apiModel.Id).ConfigureAwait(false)).Statistic);

        public async override Task<Warehouse> Create(Warehouse model)
        {
            var warehouseStatistic = new WarehouseStatistic();
            DbContext.WarehouseStatistics.Add(warehouseStatistic);

            model.Statistic = warehouseStatistic;

            DbContext.Warehouses.Add(model);

            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return model;
        }

        public override async Task Delete(Warehouse model)
        {
            var warehouse = await DbContext.Warehouses.Include(p => p.Statistic).SingleAsync(p => p.Id == model.Id).ConfigureAwait(false);
            var warehouseStatistic = warehouse.Statistic;

            DbContext.Warehouses.Remove(warehouse);
            DbContext.WarehouseStatistics.Remove(warehouseStatistic);

            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }


    //public class WarehouseController : Controller<Warehouse, int, WarehouseApiModel, CardyanDbContext>
    //{
    //    public Task<ResponseListApiModel<WarehouseProductApiModel>> Products(TKeyListApiModel<int> apiModel)
    //        => List<WarehouseProductApiModel, WarehouseProduct>(apiModel, DbContext.WarehouseProducts.Where(p => p.WarehouseId == apiModel.Id));

    //    public async Task<WarehouseStatisticApiModel> Statistic(KeyApiModel<int> apiModel)
    //        => FromModel<WarehouseStatisticApiModel, WarehouseStatistic>((await DbContext.Warehouses.Include(p => p.Statistic).SingleAsync(p => p.Id == apiModel.Id).ConfigureAwait(false)).Statistic);

    //    public async override Task<IKeyApiModel<int>> Create(WarehouseApiModel apiModel)
    //    {
    //        var warehouseStatistic = new WarehouseStatistic();
    //        DbContext.WarehouseStatistics.Add(warehouseStatistic);

    //        var warehouse = ToModel(apiModel);
    //        warehouse.Statistic = warehouseStatistic;

    //        DbContext.Warehouses.Add(warehouse);

    //        await DbContext.SaveChangesAsync().ConfigureAwait(false);
    //        return new KeyApiModel<int> { Id = warehouse.Id };
    //    }

    //    public override async Task Delete(KeyApiModel<int> keyApiModel)
    //    {
    //        var warehouse = await DbContext.Warehouses.Include(p => p.Statistic).SingleAsync(p => p.Id == keyApiModel.Id).ConfigureAwait(false);
    //        var warehouseStatistic = warehouse.Statistic;

    //        DbContext.Warehouses.Remove(warehouse);
    //        DbContext.WarehouseStatistics.Remove(warehouseStatistic);

    //        await DbContext.SaveChangesAsync().ConfigureAwait(false);
    //    }
    //}
}
/* { Cardyan } - Inventory module */
