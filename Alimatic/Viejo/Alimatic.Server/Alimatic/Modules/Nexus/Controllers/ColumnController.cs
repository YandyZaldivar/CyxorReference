using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Nexus.Controllers
{
    using Data;
    using Models;

    using Cyxor.Models;
    //using Cyxor.Networking;
    using Cyxor.Controllers;

    class ColumnController : BaseController
    {
        #region Get
        //[Action(ApiId.GetColumn)]
        public async Task<ColumnApiModel> GetColumn(GetColumnApiModel getColumnApiModel)
        {
            var entry = default(Column);

            if (getColumnApiModel.IsId)
                entry = await NexusDbContext.Columns.FindAsync(getColumnApiModel.Id);
            else
            {
                var table = await NexusDbContext.Tables.AsNoTracking().SingleAsync(p => getColumnApiModel.TableModel.IsId ?
                    p.Id == getColumnApiModel.TableModel.Id : p.Name == getColumnApiModel.TableModel.Name);

                entry = await NexusDbContext.Columns.AsNoTracking().SingleAsync(p => p.Name == getColumnApiModel.Name && p.TableId == table.Id);
            }

            return new ColumnApiModel { Id = entry.Id, Order = entry.Order, Name = entry.Name, TypeId = entry.TypeId, TableId = entry.TableId };
        }

        //[Command("nexus column get", Arguments = "$column [$table]",
        //    Description = "Get the Nexus column identified by the combination of $nameOrId and [$tableNameOrId].")]
        //public Task<ColumnApiModel> GetColumn(CommandArgs args) => InvokeAsync<ColumnApiModel>(new GetColumnApiModel
        //{
        //    NameOrId = args["$column"],
        //    TableModel = args["$table"] != null ? new TableKeyApiModel { NameOrId = args["$table"] } : null
        //});
        #endregion

        #region Add
        //[Action(ApiId.AddColumn)]
        public async Task<ColumnApiModel> AddColumn(AddColumnApiModel addColumnApiModel)
        {
            var type = default(ColumnType);

            var table = await NexusDbContext.Tables.AsNoTracking().SingleAsync(p => addColumnApiModel.TableModel.IsId ?
                p.Id == addColumnApiModel.TableModel.Id : p.Name == addColumnApiModel.TableModel.Name);

            if (addColumnApiModel.TypeModel != null)
                type = addColumnApiModel.TypeModel == null ? null : await NexusDbContext.ColumnTypes.AsNoTracking().SingleAsync(p =>
                    addColumnApiModel.TypeModel.IsId ? p.Id == addColumnApiModel.TypeModel.Id : p.Name == addColumnApiModel.TypeModel.Name);

            if (type == null)
                type = await NexusDbContext.ColumnTypes.SingleAsync(p => p.Name == nameof(TypeCode.String));

            var entry = NexusDbContext.Columns.Add(new Column { Name = addColumnApiModel.Name, TableId = table.Id, TypeId = type.Id, Order = addColumnApiModel.Order }).Entity;

            await NexusDbContext.SaveChangesAsync();

            return new ColumnApiModel { Id = entry.Id, Name = entry.Name, Order = entry.Order, TypeId = entry.TypeId, TableId = entry.TableId };
        }

        //[Command("nexus column add", Arguments = "$column $table [$order] [$type]",
        //    Description = "Add a new Nexus $column to the specified $table, with an optional [$order] and [$type].")]
        //public Task<ColumnApiModel> AddColumn(CommandArgs args) => InvokeAsync<ColumnApiModel>(new AddColumnApiModel
        //{
        //    Name = args["$column"],
        //    TableModel = new TableKeyApiModel { NameOrId = args["$table"] },
        //    Order = args["$order"] != null ? int.Parse(args["$order"]) : 0,
        //    TypeModel = args["$type"] != null ? new ColumnTypeKeyApiModel { NameOrId = args["$type"] } : null,
        //});
        #endregion

        #region Remove
        //[Action(ApiId.RemoveColumn)]
        public async Task RemoveColumn(RemoveColumnApiModel removeColumnApiModel)
        {
            var entry = default(Column);

            if (removeColumnApiModel.IsId)
                entry = await NexusDbContext.Columns.FindAsync(removeColumnApiModel.Id);
            else
            {
                var table = await NexusDbContext.Tables.AsNoTracking().SingleAsync(p => removeColumnApiModel.TableModel.IsId ?
                    p.Id == removeColumnApiModel.TableModel.Id : p.Name == removeColumnApiModel.TableModel.Name);

                entry = await NexusDbContext.Columns.SingleAsync(p => p.Name == removeColumnApiModel.Name && p.TableId == table.Id);
            }

            NexusDbContext.Columns.Remove(entry);
            await NexusDbContext.SaveChangesAsync();
        }

        //[Command("nexus column remove", Arguments = "$column [$table]",
        //    Description = "Remove the Nexus column with the specified combination of $nameOrId and [$tableNameOrId].")]
        //public Task RemoveColumn(CommandArgs args) => InvokeActionAsync(new RemoveColumnApiModel
        //{
        //    NameOrId = args["$column"],
        //    TableModel = args["$table"] != null ? new TableKeyApiModel { NameOrId = args["$table"] } : null
        //});
        #endregion

        #region Update
        //[Action(ApiId.UpdateColumn)]
        public async Task<ColumnApiModel> UpdateColumn(UpdateColumnApiModel updateColumnApiModel)
        {
            var entry = default(Column);

            if (updateColumnApiModel.IsId)
                entry = await NexusDbContext.Columns.FindAsync(updateColumnApiModel.Id);
            else
            {
                var table = await NexusDbContext.Tables.AsNoTracking().SingleAsync(p => updateColumnApiModel.TableModel.IsId ?
                    p.Id == updateColumnApiModel.TableModel.Id : p.Name == updateColumnApiModel.TableModel.Name);

                entry = await NexusDbContext.Columns.SingleAsync(p => p.Name == updateColumnApiModel.Name && p.TableId == table.Id);
            }

            entry.Order = updateColumnApiModel.NewOrderModel != null ? updateColumnApiModel.NewOrderModel.Value : 0;
            entry.Name = updateColumnApiModel.NewNameModel != null ? updateColumnApiModel.NewNameModel.Name : null;
            entry.EnumValues = updateColumnApiModel.NewEnumValues != null ? updateColumnApiModel.NewEnumValues.Value : null;
            entry.NotNull = updateColumnApiModel.NewNotNullModel != null ? updateColumnApiModel.NewNotNullModel.Value : false;

            if (updateColumnApiModel.NewTypeModel != null)
            {
                var columnType = updateColumnApiModel.NewTypeModel.IsId ? new ColumnType { Id = updateColumnApiModel.NewTypeModel.Id ?? 0 } :
                    await NexusDbContext.ColumnTypes.SingleAsync(p => p.Name == updateColumnApiModel.NewTypeModel.Name);

                entry.TypeId = columnType.Id;
            }

            if (updateColumnApiModel.NewTableModel != null)
            {
                var table = updateColumnApiModel.NewTableModel.IsId ? new Table { Id = updateColumnApiModel.NewTableModel.Id ?? 0 } :
                    await NexusDbContext.Tables.SingleAsync(p => p.Name == updateColumnApiModel.NewTableModel.Name);

                entry.TableId = table.Id;
            }

            NexusDbContext.Columns.Update(entry);

            await NexusDbContext.SaveChangesAsync();

            return new ColumnApiModel
            {
                Id = entry.Id,
                Name = entry.Name,
                Order = entry.Order,
                TypeId = entry.TypeId,
                TableId = entry.TableId,
                NotNull = entry.NotNull,
                EnumValues = entry.EnumValues,
            };
        }

        //[Command("nexus column update", Arguments = "$column [$table] [$new-name] [$new-order] [$new-type] [$new-table] [$new-not-null] [$new-enum-values]",
        //    Description = "Update the Nexus column identified by the combination of $column and [$table] with the specified arguments.")]
        //public Task<ColumnApiModel> UpdateColumn(CommandArgs args) => InvokeAsync<ColumnApiModel>(new UpdateColumnApiModel
        //{
        //    NameOrId = args["$column"],
        //    TableModel = args["$table"] != null ? new TableKeyApiModel { NameOrId = args["$table"] } : null,
        //    NewNameModel = args["$new-name"] != null ? new NameApiModel { Name = args["$new-name"] } : null,
        //    NewTableModel = args["$new-table"] != null ? new TableKeyApiModel { NameOrId = args["$new-table"] } : null,
        //    NewTypeModel = args["$new-type"] != null ? new ColumnTypeKeyApiModel { NameOrId = args["$new-type"] } : null,
        //    NewOrderModel = args["$new-order"] != null ? new ApiModel<int> { Value = int.Parse(args["$new-order"]) } : null,
        //    NewEnumValues = args["$new-enum-values"] != null ? new ApiModel<string> { Value = args["$new-enum-values"] } : null,
        //    NewNotNullModel = args["$new-not-null"] != null ? new ApiModel<bool> { Value = bool.Parse(args["$new-not-null"]) } : null,
        //});
        #endregion

        #region GetAll
        //[Action(ApiId.GetAllColumns)]
        public async Task<IEnumerable<ColumnApiModel>> GetAllColumns()
        {
            var entries = new List<ColumnApiModel>();

            foreach (var entry in await NexusDbContext.Columns.AsNoTracking().ToListAsync())
                entries.Add(new ColumnApiModel { Id = entry.Id, Name = entry.Name, Order = entry.Order, TypeId = entry.TypeId, TableId = entry.TableId });

            return entries;
        }

        //[Command("nexus column list", Description = "Get all columns in the Nexus.")]
        //public Task<ColumnsApiModel> GetAllColumns(CommandArgs args) => InvokeAsync<ColumnsApiModel>();
        #endregion
    }
}
