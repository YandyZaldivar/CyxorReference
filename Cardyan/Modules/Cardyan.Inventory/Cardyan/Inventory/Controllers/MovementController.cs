/*
  { Cardyan } - Inventory module
  Copyright (C) 2018 Cardyan AS
*/

using System;
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

    //public class MovementController : Controller<Movement, int, MovementApiModel, CardyanDbContext>
    public class MovementController : CardyanDbContextController<Movement>
    {
        [Action(Hide = true)]
        public override Task Update(Movement model) => base.Update(model);

        [Action(Hide = true)]
        public override Task Delete(Movement model) => base.Delete(model);

        // TODO: Review documentation for concurrency tokens
        public override async Task<Movement> Create(Movement model)
        {
            var movement = default(Movement);
            var linkedMovement = default(Movement);

            var transaction = DbContext.Database.CurrentTransaction;

            var isTransactionOwner = transaction == null;

            if (transaction == null)
                transaction = await DbContext.Database.BeginTransactionAsync().ConfigureAwait(false);

            try
            {
                var movementTypeValue = MovementType.Items.Single(p => p.Id == model.TypeId).Value;

                if (model.LinkedMovementId is int linkedMovementId)
                    linkedMovement = await DbContext.Movements.Include(p => p.Products)
                        .Include(p => p.Warehouse).ThenInclude(p => p.Valuation)
                        .SingleAsync(p => p.Id == linkedMovementId).ConfigureAwait(false);

                if (linkedMovement != null)
                {
                    if (model.WarehouseId != linkedMovement.LinkedWarehouseId)
                        throw new ArgumentException($"The warehouse (Id: {model.WarehouseId}) " +
                            $"don't match the linked warehouse (Id: {linkedMovement.LinkedWarehouseId}) " +
                            $"specified in the 'Out' movement transference (Id: {linkedMovement.Id})");

                    model.LinkedWarehouseId = linkedMovement.WarehouseId;
                    model.Products = linkedMovement.Products;
                }

                var warehouse = await DbContext.Warehouses.Include(p => p.Statistic).SingleAsync(p => p.Id == model.WarehouseId);

                var query = DbContext.WarehouseProducts.Where(p => p.WarehouseId == model.WarehouseId && model.Products.Any(q => q.ProductId == p.ProductId));

                var warehouseProducts = await query.Include(p => p.Statistic).Include(p => p.Product).ThenInclude(p => p.Statistic).ToListAsync().ConfigureAwait(false);

                movement = await base.Create(model).ConfigureAwait(false);

                foreach (var movementProductApiModel in model.Products)
                {
                    var warehouseProduct = warehouseProducts.SingleOrDefault(p => p.ProductId == movementProductApiModel.ProductId);

                    if (warehouseProduct == null)
                        throw new ArgumentException($"The warehouse (Id: {model.WarehouseId}) " +
                            $"doesn't have an associated product (Id: {movementProductApiModel.ProductId})");

                    var product = warehouseProduct.Product;

                    if (movementTypeValue == MovementTypeValue.In)
                    {
                        if (warehouseProduct.Maximum is int maximum)
                            if (maximum - warehouseProduct.Statistic.Count < movementProductApiModel.Count)
                            {
                                throw new ArgumentException($"The count ({movementProductApiModel.Count}) " +
                                    $"exceed the maximum storage capacity ({warehouseProduct.Maximum}) " +
                                    $"of the warehouse (Id: {warehouseProduct.WarehouseId}) " +
                                    $"for the product (Id: {warehouseProduct.ProductId}) " +
                                    $"with a current existence of ({warehouseProduct.Statistic.Count}).");
                            }

                        // TODO: Review, we are taking FIFO as the price for the transference
                        //var price = movementProductApiModel.Price ?? ((movementProductApiModel.FifoAmount ?? 0) / movementProductApiModel.Count);
                        //var fifoPrice = movementProductApiModel.Price ?? ((movementProductApiModel.FifoAmount ?? 0) / movementProductApiModel.Count);
                        //var lifoPrice = movementProductApiModel.Price ?? ((movementProductApiModel.LifoAmount ?? 0) / movementProductApiModel.Count);
                        //var averagePrice = movementProductApiModel.Price ?? ((movementProductApiModel.AverageAmount ?? 0) / movementProductApiModel.Count);

                        var price = movementProductApiModel.Price ?? 0;

                        if (linkedMovement != null)
                            switch (linkedMovement.Warehouse.Valuation.Value)
                            {
                                case ValuationValue.Fifo: price = ((movementProductApiModel.FifoAmount ?? 0) / movementProductApiModel.Count); break;
                                case ValuationValue.Lifo: price = ((movementProductApiModel.LifoAmount ?? 0) / movementProductApiModel.Count); break;
                                case ValuationValue.Average: price = ((movementProductApiModel.AverageAmount ?? 0) / movementProductApiModel.Count); break;
                            }

                        if (warehouseProduct.Statistic.AveragePrice != price)
                        {
                            warehouseProduct.Statistic.AveragePrice = (warehouseProduct.Statistic.AverageAmount + movementProductApiModel.Amount)
                                / (warehouseProduct.Statistic.Count + movementProductApiModel.Count);

                            warehouse.Statistic.AveragePrice = (warehouse.Statistic.AverageAmount + movementProductApiModel.Amount)
                                / (warehouse.Statistic.Count + movementProductApiModel.Count);

                            product.Statistic.AveragePrice = (product.Statistic.AverageAmount + movementProductApiModel.Amount)
                                / (product.Statistic.Count + movementProductApiModel.Count);
                        }

                        var fifoValuation = new FifoExistence
                        {
                            MovementId = movement.Id,
                            WarehouseId = model.WarehouseId,
                            ProductId = product.Id,
                            Price = price,
                            Count = movementProductApiModel.Count,
                        };

                        DbContext.FifoExistences.Add(fifoValuation);

                        var lifoValuation = new LifoExistence
                        {
                            MovementId = movement.Id,
                            WarehouseId = model.WarehouseId,
                            ProductId = product.Id,
                            Price = price,
                            Count = movementProductApiModel.Count,
                        };

                        DbContext.LifoExistences.Add(lifoValuation);

                        product.Statistic.LastPrice = price;
                        product.Statistic.MinimumPrice = product.Statistic.MinimumPrice == 0 || product.Statistic.MinimumPrice > price ? price : product.Statistic.MinimumPrice;
                        product.Statistic.MaximumPrice = product.Statistic.MaximumPrice < price ? price : product.Statistic.MaximumPrice;
                        product.Statistic.InCount += movementProductApiModel.Count;
                        product.Statistic.InAmount += movementProductApiModel.Amount;

                        warehouse.Statistic.LastPrice = price;
                        warehouse.Statistic.MinimumPrice = warehouse.Statistic.MinimumPrice == 0 || warehouse.Statistic.MinimumPrice > price ? price : warehouse.Statistic.MinimumPrice;
                        warehouse.Statistic.MaximumPrice = warehouse.Statistic.MaximumPrice < price ? price : warehouse.Statistic.MaximumPrice;
                        warehouse.Statistic.InCount += movementProductApiModel.Count;
                        warehouse.Statistic.InAmount += movementProductApiModel.Amount;

                        warehouseProduct.Statistic.LastPrice = price;
                        warehouseProduct.Statistic.MinimumPrice = warehouseProduct.Statistic.MinimumPrice == 0 || warehouseProduct.Statistic.MinimumPrice > price ? price : warehouseProduct.Statistic.MinimumPrice;
                        warehouseProduct.Statistic.MaximumPrice = warehouseProduct.Statistic.MaximumPrice < price ? price : warehouseProduct.Statistic.MaximumPrice;
                        warehouseProduct.Statistic.InCount += movementProductApiModel.Count;
                        warehouseProduct.Statistic.InAmount += movementProductApiModel.Amount;
                    }
                    else
                    {
                        if (warehouseProduct.Statistic.Count - movementProductApiModel.Count < 0)
                            throw new ArgumentException($"The count ({movementProductApiModel.Count}) " +
                                $"exceed the current existence ({warehouseProduct.Statistic.Count}) " +
                                $"of the warehouse (Id: {warehouseProduct.WarehouseId}) " +
                                $"for the product (Id: {warehouseProduct.ProductId}).");

                        if (!movementProductApiModel.TakeFromMinimum)
                            if (warehouseProduct.Statistic.Count - movementProductApiModel.Count < warehouseProduct.Minimum)
                                throw new ArgumentException($"The count ({movementProductApiModel.Count}) " +
                                    $"exceed the minimum availability ({warehouseProduct.Minimum}) " +
                                    $"of the warehouse (Id: {warehouseProduct.WarehouseId}) " +
                                    $"for the product (Id: {warehouseProduct.ProductId}) " +
                                    $"with a current existence of ({warehouseProduct.Statistic.Count}).");

                        var fifoAmount = 0.00M;
                        var lifoAmount = 0.00M;
                        var count = movementProductApiModel.Count;

                        while (count > 0)
                        {
                            var fifoValuation = await DbContext.FifoExistences.OrderBy(p => p.DateTime).FirstAsync().ConfigureAwait(false);

                            var partialCount = fifoValuation.Count >= count ? count : fifoValuation.Count;

                            if (fifoValuation.Count - partialCount == 0)
                            {
                                DbContext.FifoExistences.Remove(fifoValuation);
                                await DbContext.SaveChangesAsync();
                            }
                            else
                                fifoValuation.Count -= partialCount;

                            count -= partialCount;
                            fifoAmount += partialCount * fifoValuation.Price;
                        }

                        count = movementProductApiModel.Count;

                        while (count > 0)
                        {
                            var lifoValuation = await DbContext.LifoExistences.OrderBy(p => p.DateTime).LastAsync().ConfigureAwait(false);

                            var partialCount = lifoValuation.Count >= count ? count : lifoValuation.Count;

                            if (lifoValuation.Count - partialCount == 0)
                            {
                                DbContext.LifoExistences.Remove(lifoValuation);
                                await DbContext.SaveChangesAsync();
                            }
                            else
                                lifoValuation.Count -= partialCount;

                            count -= partialCount;
                            lifoAmount += partialCount * lifoValuation.Price;
                        }

                        movementProductApiModel.FifoAmount = fifoAmount;
                        movementProductApiModel.LifoAmount = lifoAmount;
                        movementProductApiModel.AverageAmount = warehouseProduct.Statistic.AveragePrice * movementProductApiModel.Count;

                        product.Statistic.OutCount += movementProductApiModel.Count;
                        product.Statistic.FifoOutAmount += movementProductApiModel.FifoAmount.Value;
                        product.Statistic.LifoOutAmount += movementProductApiModel.LifoAmount.Value;

                        warehouse.Statistic.OutCount += movementProductApiModel.Count;
                        warehouse.Statistic.FifoOutAmount += movementProductApiModel.FifoAmount.Value;
                        warehouse.Statistic.LifoOutAmount += movementProductApiModel.LifoAmount.Value;

                        warehouseProduct.Statistic.OutCount += movementProductApiModel.Count;
                        warehouseProduct.Statistic.FifoOutAmount += movementProductApiModel.FifoAmount.Value;
                        warehouseProduct.Statistic.LifoOutAmount += movementProductApiModel.LifoAmount.Value;
                    }

                    DbContext.MovementProducts.Add(new MovementProduct
                    {
                        MovementId = movement.Id,
                        ProductId = movementProductApiModel.ProductId,
                        Count = movementProductApiModel.Count,
                        Price = movementProductApiModel.Price,

                        FifoAmount = movementProductApiModel.FifoAmount,
                        LifoAmount = movementProductApiModel.LifoAmount,
                        AverageAmount = movementProductApiModel.AverageAmount,
                    });
                }

                // TODO: Duda, when an entity is being tracked and a single property is changed
                //       how it is updated to the database? By marking all properties as modified
                //       or just the modified property? Review the generated SQL query.
                //var linkedMovementEntity = DbContext.Attach(linkedMovement)

                if (linkedMovement != null)
                    linkedMovement.LinkedMovementId = movement.Id;

                await DbContext.SaveChangesAsync().ConfigureAwait(false);

                if (model.AutoTransference)
                {
                    var transferMovementApiModel = new Movement
                    {
                        TypeId = 1,
                        LinkedMovementId = movement.Id,
                        WarehouseId = model.LinkedWarehouseId.Value,
                        Code = model.Code,
                        DateTime = model.DateTime,
                    };

                    await Create(transferMovementApiModel).ConfigureAwait(false);
                }

                if (isTransactionOwner)
                    transaction.Commit();
            }
            catch
            {
                if (isTransactionOwner)
                    transaction.Rollback();

                throw;
            }
            finally
            {
                if (isTransactionOwner)
                    transaction.Dispose();
            }

            return movement;
        }













        //public async Task<Dictionary<MovementApiModel, MovementApiModel>> Transferences()
        //{
        //    var transferences = new Dictionary<MovementApiModel, MovementApiModel>();

        //    var outTransferences = await DbContext.Movements.Include(p => p.Products).Where(p => p.LinkedWarehouseId != null && p.Type.Value == MovementTypeValue.Out).ToListAsync().ConfigureAwait(false);
        //    var inTransferences = await DbContext.Movements.Include(p => p.Products).Where(p => p.LinkedWarehouseId != null && p.Type.Value == MovementTypeValue.In).ToListAsync().ConfigureAwait(false);

        //    foreach (var outTransference in outTransferences)
        //    {
        //        var inTransference = inTransferences.SingleOrDefault(p => p.Code == outTransference.Code);
        //        transferences.Add(FromModel(outTransference), FromModel(inTransference));
        //    }

        //    return transferences;
        //}

        //public Task<ResponseListApiModel<MovementProductApiModel>> Products(TKeyListApiModel<int> apiModel)
        //    => List<MovementProductApiModel, MovementProduct>(apiModel, DbContext.MovementProducts.Where(p => p.MovementId == apiModel.Id));

        // TODO: Review documentation for concurrency tokens
        //public override async Task<IKeyApiModel<int>> Create(MovementApiModel apiModel)
        //{
        //    var linkedMovement = default(Movement);
        //    var keyApiModel = default(IKeyApiModel<int>);

        //    var transaction = DbContext.Database.CurrentTransaction;

        //    var isTransactionOwner = transaction == null;

        //    if (transaction == null)
        //        transaction = await DbContext.Database.BeginTransactionAsync().ConfigureAwait(false);

        //    try
        //    {
        //        var movementTypeValue = MovementType.Items.Single(p => p.Id == apiModel.TypeId).Value;

        //        if (apiModel.LinkedMovementId is int linkedMovementId)
        //            linkedMovement = await DbContext.Movements.Include(p => p.Products)
        //                .Include(p => p.Warehouse).ThenInclude(p => p.Valuation)
        //                .SingleAsync(p => p.Id == linkedMovementId).ConfigureAwait(false);

        //        if (linkedMovement != null)
        //        {
        //            if (apiModel.WarehouseId != linkedMovement.LinkedWarehouseId)
        //                throw new ArgumentException($"The warehouse (Id: {apiModel.WarehouseId}) " +
        //                    $"don't match the linked warehouse (Id: {linkedMovement.LinkedWarehouseId}) " +
        //                    $"specified in the 'Out' movement transference (Id: {linkedMovement.Id})");

        //            apiModel.LinkedWarehouseId = linkedMovement.WarehouseId;
        //            apiModel.Products = FromModels<MovementProductApiModel, MovementProduct>(linkedMovement.Products);
        //        }

        //        var warehouse = await DbContext.Warehouses.Include(p => p.Statistic).SingleAsync(p => p.Id == apiModel.WarehouseId);

        //        var query = DbContext.WarehouseProducts.Where(p => p.WarehouseId == apiModel.WarehouseId && apiModel.Products.Any(q => q.ProductId == p.ProductId));

        //        var warehouseProducts = await query.Include(p => p.Statistic).Include(p => p.Product).ThenInclude(p => p.Statistic).ToListAsync().ConfigureAwait(false);

        //        keyApiModel = await base.Create(apiModel).ConfigureAwait(false);

        //        foreach (var movementProductApiModel in apiModel.Products)
        //        {
        //            var warehouseProduct = warehouseProducts.SingleOrDefault(p => p.ProductId == movementProductApiModel.ProductId);

        //            if (warehouseProduct == null)
        //                throw new ArgumentException($"The warehouse (Id: {apiModel.WarehouseId}) " +
        //                    $"doesn't have an associated product (Id: {movementProductApiModel.ProductId})");

        //            var product = warehouseProduct.Product;

        //            if (movementTypeValue == MovementTypeValue.In)
        //            {
        //                if (warehouseProduct.Maximum is int maximum)
        //                    if (maximum - warehouseProduct.Statistic.Count < movementProductApiModel.Count)
        //                    {
        //                        throw new ArgumentException($"The count ({movementProductApiModel.Count}) " +
        //                            $"exceed the maximum storage capacity ({warehouseProduct.Maximum}) " +
        //                            $"of the warehouse (Id: {warehouseProduct.WarehouseId}) " +
        //                            $"for the product (Id: {warehouseProduct.ProductId}) " +
        //                            $"with a current existence of ({warehouseProduct.Statistic.Count}).");
        //                    }

        //                // TODO: Review, we are taking FIFO as the price for the transference
        //                //var price = movementProductApiModel.Price ?? ((movementProductApiModel.FifoAmount ?? 0) / movementProductApiModel.Count);
        //                //var fifoPrice = movementProductApiModel.Price ?? ((movementProductApiModel.FifoAmount ?? 0) / movementProductApiModel.Count);
        //                //var lifoPrice = movementProductApiModel.Price ?? ((movementProductApiModel.LifoAmount ?? 0) / movementProductApiModel.Count);
        //                //var averagePrice = movementProductApiModel.Price ?? ((movementProductApiModel.AverageAmount ?? 0) / movementProductApiModel.Count);

        //                var price = movementProductApiModel.Price ?? 0;

        //                if (linkedMovement != null)
        //                    switch (linkedMovement.Warehouse.Valuation.Value)
        //                    {
        //                        case ValuationValue.Fifo: price = ((movementProductApiModel.FifoAmount ?? 0) / movementProductApiModel.Count); break;
        //                        case ValuationValue.Lifo: price = ((movementProductApiModel.LifoAmount ?? 0) / movementProductApiModel.Count); break;
        //                        case ValuationValue.Average: price = ((movementProductApiModel.AverageAmount ?? 0) / movementProductApiModel.Count); break;
        //                    }

        //                if (warehouseProduct.Statistic.AveragePrice != price)
        //                {
        //                    warehouseProduct.Statistic.AveragePrice = (warehouseProduct.Statistic.AverageAmount + movementProductApiModel.Amount)
        //                        / (warehouseProduct.Statistic.Count + movementProductApiModel.Count);

        //                    warehouse.Statistic.AveragePrice = (warehouse.Statistic.AverageAmount + movementProductApiModel.Amount)
        //                        / (warehouse.Statistic.Count + movementProductApiModel.Count);

        //                    product.Statistic.AveragePrice = (product.Statistic.AverageAmount + movementProductApiModel.Amount)
        //                        / (product.Statistic.Count + movementProductApiModel.Count);
        //                }

        //                var fifoValuation = new FifoExistence
        //                {
        //                    MovementId = keyApiModel.Id,
        //                    WarehouseId = apiModel.WarehouseId,
        //                    ProductId = product.Id,
        //                    Price = price,
        //                    Count = movementProductApiModel.Count,
        //                };

        //                DbContext.FifoExistences.Add(fifoValuation);

        //                var lifoValuation = new LifoExistence
        //                {
        //                    MovementId = keyApiModel.Id,
        //                    WarehouseId = apiModel.WarehouseId,
        //                    ProductId = product.Id,
        //                    Price = price,
        //                    Count = movementProductApiModel.Count,
        //                };

        //                DbContext.LifoExistences.Add(lifoValuation);

        //                product.Statistic.LastPrice = price;
        //                product.Statistic.MinimumPrice = product.Statistic.MinimumPrice == 0 || product.Statistic.MinimumPrice > price ? price : product.Statistic.MinimumPrice;
        //                product.Statistic.MaximumPrice = product.Statistic.MaximumPrice < price ? price : product.Statistic.MaximumPrice;
        //                product.Statistic.InCount += movementProductApiModel.Count;
        //                product.Statistic.InAmount += movementProductApiModel.Amount;

        //                warehouse.Statistic.LastPrice = price;
        //                warehouse.Statistic.MinimumPrice = warehouse.Statistic.MinimumPrice == 0 || warehouse.Statistic.MinimumPrice > price ? price : warehouse.Statistic.MinimumPrice;
        //                warehouse.Statistic.MaximumPrice = warehouse.Statistic.MaximumPrice < price ? price : warehouse.Statistic.MaximumPrice;
        //                warehouse.Statistic.InCount += movementProductApiModel.Count;
        //                warehouse.Statistic.InAmount += movementProductApiModel.Amount;

        //                warehouseProduct.Statistic.LastPrice = price;
        //                warehouseProduct.Statistic.MinimumPrice = warehouseProduct.Statistic.MinimumPrice == 0 || warehouseProduct.Statistic.MinimumPrice > price ? price : warehouseProduct.Statistic.MinimumPrice;
        //                warehouseProduct.Statistic.MaximumPrice = warehouseProduct.Statistic.MaximumPrice < price ? price : warehouseProduct.Statistic.MaximumPrice;
        //                warehouseProduct.Statistic.InCount += movementProductApiModel.Count;
        //                warehouseProduct.Statistic.InAmount += movementProductApiModel.Amount;
        //            }
        //            else
        //            {
        //                if (warehouseProduct.Statistic.Count - movementProductApiModel.Count < 0)
        //                    throw new ArgumentException($"The count ({movementProductApiModel.Count}) " +
        //                        $"exceed the current existence ({warehouseProduct.Statistic.Count}) " +
        //                        $"of the warehouse (Id: {warehouseProduct.WarehouseId}) " +
        //                        $"for the product (Id: {warehouseProduct.ProductId}).");

        //                if (!movementProductApiModel.TakeFromMinimum)
        //                    if (warehouseProduct.Statistic.Count - movementProductApiModel.Count < warehouseProduct.Minimum)
        //                        throw new ArgumentException($"The count ({movementProductApiModel.Count}) " +
        //                            $"exceed the minimum availability ({warehouseProduct.Minimum}) " +
        //                            $"of the warehouse (Id: {warehouseProduct.WarehouseId}) " +
        //                            $"for the product (Id: {warehouseProduct.ProductId}) " +
        //                            $"with a current existence of ({warehouseProduct.Statistic.Count}).");

        //                var fifoAmount = 0.00M;
        //                var lifoAmount = 0.00M;
        //                var count = movementProductApiModel.Count;

        //                while (count > 0)
        //                {
        //                    var fifoValuation = await DbContext.FifoExistences.OrderBy(p => p.DateTime).FirstAsync().ConfigureAwait(false);

        //                    var partialCount = fifoValuation.Count >= count ? count : fifoValuation.Count;

        //                    if (fifoValuation.Count - partialCount == 0)
        //                    {
        //                        DbContext.FifoExistences.Remove(fifoValuation);
        //                        await DbContext.SaveChangesAsync();
        //                    }
        //                    else
        //                        fifoValuation.Count -= partialCount;

        //                    count -= partialCount;
        //                    fifoAmount += partialCount * fifoValuation.Price;
        //                }

        //                count = movementProductApiModel.Count;

        //                while (count > 0)
        //                {
        //                    var lifoValuation = await DbContext.LifoExistences.OrderBy(p => p.DateTime).LastAsync().ConfigureAwait(false);

        //                    var partialCount = lifoValuation.Count >= count ? count : lifoValuation.Count;

        //                    if (lifoValuation.Count - partialCount == 0)
        //                    {
        //                        DbContext.LifoExistences.Remove(lifoValuation);
        //                        await DbContext.SaveChangesAsync();
        //                    }
        //                    else
        //                        lifoValuation.Count -= partialCount;

        //                    count -= partialCount;
        //                    lifoAmount += partialCount * lifoValuation.Price;
        //                }

        //                movementProductApiModel.FifoAmount = fifoAmount;
        //                movementProductApiModel.LifoAmount = lifoAmount;
        //                movementProductApiModel.AverageAmount = warehouseProduct.Statistic.AveragePrice * movementProductApiModel.Count;

        //                product.Statistic.OutCount += movementProductApiModel.Count;
        //                product.Statistic.FifoOutAmount += movementProductApiModel.FifoAmount.Value;
        //                product.Statistic.LifoOutAmount += movementProductApiModel.LifoAmount.Value;

        //                warehouse.Statistic.OutCount += movementProductApiModel.Count;
        //                warehouse.Statistic.FifoOutAmount += movementProductApiModel.FifoAmount.Value;
        //                warehouse.Statistic.LifoOutAmount += movementProductApiModel.LifoAmount.Value;

        //                warehouseProduct.Statistic.OutCount += movementProductApiModel.Count;
        //                warehouseProduct.Statistic.FifoOutAmount += movementProductApiModel.FifoAmount.Value;
        //                warehouseProduct.Statistic.LifoOutAmount += movementProductApiModel.LifoAmount.Value;
        //            }

        //            DbContext.MovementProducts.Add(new MovementProduct
        //            {
        //                MovementId = keyApiModel.Id,
        //                ProductId = movementProductApiModel.ProductId,
        //                Count = movementProductApiModel.Count,
        //                Price = movementProductApiModel.Price,

        //                FifoAmount = movementProductApiModel.FifoAmount,
        //                LifoAmount = movementProductApiModel.LifoAmount,
        //                AverageAmount = movementProductApiModel.AverageAmount,
        //            });
        //        }

        //        // TODO: Duda, when an entity is being tracked and a single property is changed
        //        //       how it is updated to the database? By marking all properties as modified
        //        //       or just the modified property? Review the generated SQL query.
        //        //var linkedMovementEntity = DbContext.Attach(linkedMovement)

        //        if (linkedMovement != null)
        //            linkedMovement.LinkedMovementId = keyApiModel.Id;

        //        await DbContext.SaveChangesAsync().ConfigureAwait(false);

        //        if (apiModel.AutoTransference)
        //        {
        //            var transferMovementApiModel = new MovementApiModel
        //            {
        //                TypeId = 1,
        //                LinkedMovementId = keyApiModel.Id,
        //                WarehouseId = apiModel.LinkedWarehouseId.Value,
        //                Code = apiModel.Code,
        //                DateTime = apiModel.DateTime,
        //            };

        //            await Create(transferMovementApiModel).ConfigureAwait(false);
        //        }

        //        if (isTransactionOwner)
        //            transaction.Commit();
        //    }
        //    catch
        //    {
        //        if (isTransactionOwner)
        //            transaction.Rollback();

        //        throw;
        //    }
        //    finally
        //    {
        //        if (isTransactionOwner)
        //            transaction.Dispose();
        //    }

        //    return keyApiModel;
        //}
    }
}
/* { Cardyan } - Inventory module */
