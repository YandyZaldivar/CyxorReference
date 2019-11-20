using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Nexus.Controllers
{
    using Data;
    using Models;

    using Cyxor.Models;
    using Cyxor.Controllers;

    class RowColumnController : BaseController
    {
        /*
        Task<RowColumn> FindAsync(RowColumnKeyApiModel rowColumnKeyApiModel) =>
            NexusDbContext.RowColumns.Include(p => p.Row).Include(p => p.Column).ThenInclude(p => p.Table).SingleAsync(p => (rowColumnKeyApiModel.RowModel.Id == p.RowId) &&
            rowColumnKeyApiModel.ColumnModel.IsId ? p.ColumnId == rowColumnKeyApiModel.ColumnModel.Id : ((p.Column.Name == rowColumnKeyApiModel.ColumnModel.Name) &&
            (rowColumnKeyApiModel.ColumnModel.TableModel.IsId ? p.Column.TableId == rowColumnKeyApiModel.ColumnModel.TableModel.Id : p.Column.Table.Name == rowColumnKeyApiModel.ColumnModel.TableModel.Name)));
            //((rowColumnKeyApiModel.ColumnModel.IsId ? p.ColumnId == rowColumnKeyApiModel.ColumnModel.Id : p.Column.Name == rowColumnKeyApiModel.ColumnModel.Name) &&
            //(rowColumnKeyApiModel.ColumnModel.TableModel.IsId ? p.Column.TableId == rowColumnKeyApiModel.ColumnModel.TableModel.Id : p.Column.Table.Name == rowColumnKeyApiModel.ColumnModel.TableModel.Name)));
            */

        //Task<RowColumn> FindAsync(RowColumnKeyApiModel rowColumnKeyApiModel) =>
        //    NexusDbContext.RowColumns.Include(p => p.Row).Include(p => p.Column).ThenInclude(p => p.Table).SingleAsync(p => (rowColumnKeyApiModel.RowModel.Id == p.RowId) &&
        //    (rowColumnKeyApiModel.ColumnModel.IsId ? p.ColumnId == rowColumnKeyApiModel.ColumnModel.Id : ((p.Column.Name == rowColumnKeyApiModel.ColumnModel.Name) &&
        //    (rowColumnKeyApiModel.ColumnModel.TableModel.IsId ? p.Column.TableId == rowColumnKeyApiModel.ColumnModel.TableModel.Id : (p.Column.Table.Name == rowColumnKeyApiModel.ColumnModel.TableModel.Name)))));

        Task<RowColumn> FindAsync(RowColumnKeyApiModel rowColumnKeyApiModel) =>
            NexusDbContext.RowColumns.Include(p => p.Row).Include(p => p.Column).ThenInclude(p => p.Table).SingleAsync(p => (rowColumnKeyApiModel.RowModel.Id == p.RowId) && p.ColumnId == rowColumnKeyApiModel.ColumnModel.Id);

        RowColumnApiModel NewRowColumnApiModel(RowColumn rowColumn) =>
            new RowColumnApiModel
            {
                RowId = rowColumn.RowId,
                ColumnId = rowColumn.ColumnId,
                Value = rowColumn.Value,
            };

        #region Get
        //[Action(ApiId.GetRowColumn)]
        public async Task<RowColumnApiModel> GetRowColumn(GetRowColumnApiModel getRowColumnApiModel)
            => NewRowColumnApiModel(await FindAsync(getRowColumnApiModel));

        //[Command("nexus row-column get", Arguments = "$row $column [$table]",
        //    Description = "Get the Nexus row-column identified by $row and $column [$table].")]
        //public Task<RowColumnApiModel> GetRowColumn(CommandArgs args) => InvokeAsync<RowColumnApiModel>(new GetRowColumnApiModel
        //{
        //    RowModel = new RowKeyApiModel { Id = int.Parse(args["$row"]) },
        //    ColumnModel = new ColumnKeyApiModel
        //    {
        //        NameOrId = args["$column"],
        //        TableModel = args["$table"] != null ? new TableKeyApiModel { NameOrId = args["$table"] } : null
        //    }
        //});
        #endregion

        #region Add
        //[Action(ApiId.AddRowColumn)]
        public async Task<RowColumnApiModel> AddRowColumn(AddRowColumnApiModel addRowColumnApiModel)
        {
            var rowId = addRowColumnApiModel.RowModel.Id;
            var columnId = addRowColumnApiModel.ColumnModel.IsId ? (int)addRowColumnApiModel.ColumnModel.Id : 0;

            if (rowId == 0 || columnId == 0)
            {
                var modelsId = (from row in NexusDbContext.Rows
                                where addRowColumnApiModel.RowModel.Id == row.Id
                                from column in NexusDbContext.Columns
                                join table in NexusDbContext.Tables on column.TableId equals table.Id
                                where addRowColumnApiModel.ColumnModel.IsId ? column.Id == addRowColumnApiModel.ColumnModel.Id : column.Name == addRowColumnApiModel.ColumnModel.Name &&
                                addRowColumnApiModel.ColumnModel.TableModel.IsId ? table.Id == addRowColumnApiModel.ColumnModel.TableModel.Id : table.Name == addRowColumnApiModel.ColumnModel.TableModel.Name
                                select new { RowId = row.Id, ColumnId = column.Id }).Single();

                rowId = modelsId.RowId;
                columnId = modelsId.ColumnId;
            }

            var rowColumn = NexusDbContext.RowColumns.Add(new RowColumn
            {
                RowId = rowId,
                ColumnId = columnId,
                Value = addRowColumnApiModel.ValueModel?.Value
            }).Entity;

            await NexusDbContext.SaveChangesAsync();

            return NewRowColumnApiModel(rowColumn);
        }

        //[Command("nexus row-column add", Arguments = "$row $column [$table] [$value]",
        //    Description = "Add a new Nexus row-column identified by $row, $column and [$table] with the optional argument [$value]. The key values $row, $column and [$table] " +
        //    "means a row-column can be identified by the combination of a row id + column id or using the optional [$table] argument as row id column name + table id or name. " +
        //    "That is because a column can be uniquely identified by an Id or by the combination of the column name + the table id or name which the column belongs to.")]
        //public Task<RowColumnApiModel> AddRowColumn(CommandArgs args) => InvokeAsync<RowColumnApiModel>(new AddRowColumnApiModel
        //{
        //    RowModel = new RowKeyApiModel { Id = int.Parse(args["$row"]) },
        //    ColumnModel = new ColumnKeyApiModel
        //    {
        //        NameOrId = args["$column"],
        //        TableModel = args["$table"] != null ? new TableKeyApiModel { NameOrId = args["$table"] } : null
        //    },
        //    ValueModel = args["$value"] != null ? new ApiModel<string> { Value = args["$value"] } : null,
        //});
        #endregion

        #region Remove
        //[Action(ApiId.RemoveRowColumn)]
        public async Task<RowColumnApiModel> RemoveRowColumn(RemoveRowColumnApiModel removeRowColumnApiModel)
        {
            var rowColumn = await FindAsync(removeRowColumnApiModel);
            NexusDbContext.RowColumns.Remove(rowColumn);
            await NexusDbContext.SaveChangesAsync();
            return NewRowColumnApiModel(rowColumn);
        }

        //[Command("nexus row-column remove", Arguments = "$row $column [$table]",
        //    Description = "Remove the Nexus row-column identified by $row and $column [$table].")]
        //public Task<RowColumnApiModel> RemoveRowColumn(CommandArgs args) => InvokeAsync<RowColumnApiModel>(new RemoveRowColumnApiModel
        //{
        //    RowModel = new RowKeyApiModel { Id = int.Parse(args["$row"]) },
        //    ColumnModel = new ColumnKeyApiModel
        //    {
        //        NameOrId = args["$column"],
        //        TableModel = args["$table"] != null ? new TableKeyApiModel { NameOrId = args["$table"] } : null
        //    }
        //});
        #endregion

        #region Update
        //[Action(ApiId.UpdateRowColumn)]
        public async Task<RowColumnApiModel> UpdateRowColumn(UpdateRowColumnApiModel updateRowColumnApiModel)
        {
            var rowColumn = await FindAsync(updateRowColumnApiModel);
            rowColumn.Value = updateRowColumnApiModel.NewValueModel?.Value;
            await NexusDbContext.SaveChangesAsync();
            return NewRowColumnApiModel(rowColumn);
        }

        //[Command("nexus row-column update", Arguments = "$row $column [$table] [$new-value]",
        //    Description = "Update the Nexus row-column identified by $row $column with the optional argument [$new-permission]. All variables can denote a name or id.")]
        //public Task<RowColumnApiModel> UpdateRowColumn(CommandArgs args) => InvokeAsync<RowColumnApiModel>(new UpdateRowColumnApiModel
        //{
        //    RowModel = new RowKeyApiModel { Id = int.Parse(args["$row"]) },
        //    ColumnModel = new ColumnKeyApiModel
        //    {
        //        NameOrId = args["$column"],
        //        TableModel = args["$table"] != null ? new TableKeyApiModel { NameOrId = args["$table"] } : null
        //    },
        //    NewValueModel = args["$new-value"] != null ? new ApiModel<string> { Value = args["$new-value"] } : null
        //});
        #endregion

        #region GetAll
        //[Action(ApiId.GetAllRowColumns)]
        public async Task<IEnumerable<RowColumnApiModel>> GetAllRowColumns()
        {
            var entries = new List<RowColumnApiModel>();

            foreach (var entry in await NexusDbContext.RowColumns.AsNoTracking().ToListAsync())
                entries.Add(NewRowColumnApiModel(entry));

            return entries;
        }

        //[Command("nexus row-column list", Description = "Get all row-columns in the Nexus.")]
        //public Task<RowColumnsApiModel> GetAllRowColumns(CommandArgs args) => InvokeAsync<RowColumnsApiModel>();
        #endregion
    }
}
