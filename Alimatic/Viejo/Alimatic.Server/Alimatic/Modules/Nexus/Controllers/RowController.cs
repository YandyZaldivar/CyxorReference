using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Nexus.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    class RowController : BaseController
    {
        #region Get
        //[Action(ApiId.GetRow)]
        public async Task<RowApiModel> GetRow(GetRowApiModel getRowApiModel)
        {
            var row = await NexusDbContext.Rows.FindAsync(getRowApiModel.Id);
            return new RowApiModel { Id = row.Id, UserId = row.UserId, TableId = row.TableId };
        }

        //[Command("nexus row get", Arguments = "$id", Description = "Get the Nexus row identified by $id.")]
        //public Task<RowApiModel> GetRow(CommandArgs args) => InvokeAsync<RowApiModel>(new GetRowApiModel { Id = int.Parse(args["$id"]) });
        #endregion

        #region Add
        //[Action(ApiId.AddRow)]
        public async Task<RowApiModel> AddRow(AddRowApiModel addRowApiModel)
        {
            var user = default(User);

            if (addRowApiModel.UserModel != null)
                user = await NexusDbContext.Users.AsNoTracking().SingleOrDefaultAsync(p => addRowApiModel.UserModel.IsId ?
                p.Id == addRowApiModel.UserModel.Id : p.Name == addRowApiModel.UserModel.Name);

            var table = await NexusDbContext.Tables.AsNoTracking().SingleAsync(p => addRowApiModel.TableModel.IsId ?
                p.Id == addRowApiModel.TableModel.Id : p.Name == addRowApiModel.TableModel.Name);

            var row = NexusDbContext.Rows.Add(new Row { UserId = user?.Id, TableId = table.Id }).Entity;

            await NexusDbContext.SaveChangesAsync();

            return new RowApiModel { Id = row.Id, TableId = row.TableId, UserId = row.UserId };
        }

        //[Command("nexus row add", Arguments = "$table [$user]",
        //    Description = "Add a new Nexus row with the specified $tableNameOrId and optional $userNameOrId.")]
        //public Task<RowApiModel> AddRow(CommandArgs args) => InvokeAsync<RowApiModel>(new AddRowApiModel
        //{
        //    TableModel = new TableKeyApiModel { NameOrId = args["$table"] },
        //    UserModel = args["$user"] != null ? new UserKeyApiModel { NameOrId = args["$user"] } : null,
        //});
        #endregion

        #region Remove
        //[Action(ApiId.RemoveRow)]
        public async Task<RowApiModel> RemoveRow(RemoveRowApiModel removeRowApiModel)
        {


            var row = NexusDbContext.Rows.Remove(new Row { Id = removeRowApiModel.Id }).Entity;
            await NexusDbContext.SaveChangesAsync();
            return new RowApiModel { Id = row.Id, TableId = row.TableId, UserId = row.UserId };
        }

        //[Command("nexus row remove", Arguments = "$id", Description = "Remove the Nexus row with the specified $id.")]
        //public Task<RowApiModel> RemoveRow(CommandArgs args) => InvokeAsync<RowApiModel>(new RemoveRowApiModel { Id = int.Parse(args["$id"]) });
        #endregion

        #region Update
        //[Action(ApiId.UpdateRow)]
        public async Task<RowApiModel> UpdateRow(UpdateRowApiModel updateRowApiModel)
        {
            var row = await NexusDbContext.Rows.FindAsync(updateRowApiModel.Id);

            if (updateRowApiModel.NewUserModel != null)
            {
                var user = updateRowApiModel.NewUserModel.IsId ? new User { Id = updateRowApiModel.NewUserModel.Id ?? 0 } :
                    await NexusDbContext.Users.SingleAsync(p => p.Name == updateRowApiModel.NewUserModel.Name);

                row.UserId = user.Id;
            }

            if (updateRowApiModel.NewTableModel != null)
            {
                var user = updateRowApiModel.NewTableModel.IsId ? new Table { Id = updateRowApiModel.NewTableModel.Id ?? 0 } :
                    await NexusDbContext.Tables.SingleAsync(p => p.Name == updateRowApiModel.NewTableModel.Name);

                row.UserId = user.Id;
            }

            NexusDbContext.Rows.Update(row);
            await NexusDbContext.SaveChangesAsync();

            return new RowApiModel { Id = row.Id, UserId = row.UserId, TableId = row.TableId };
        }

        //[Command("nexus row update", Arguments = "$id [$new-table] [$new-user]",
        //    Description = "Update the Nexus row identified by $id with the optional arguments of $newTableNameOrId and $newUserNameOrId.")]
        //public Task<RowApiModel> UpdateRow(CommandArgs args) => InvokeAsync<RowApiModel>(new UpdateRowApiModel
        //{
        //    Id = int.Parse(args["$id"]),
        //    NewUserModel = args["$new-user"] != null ? new UserKeyApiModel { NameOrId = args["$new-user"] } : null,
        //    NewTableModel = args["$new-table"] != null ? new TableKeyApiModel { NameOrId = args["$new-table"] } : null,
        //});
        #endregion

        #region GetAll
        //[Action(ApiId.GetAllRows)]
        public async Task<IEnumerable<RowApiModel>> GetAllRows()
        {
            var entries = new List<RowApiModel>();

            foreach (var entry in await NexusDbContext.Rows.AsNoTracking().ToListAsync())
                entries.Add(new RowApiModel { Id = entry.Id, UserId = entry.UserId, TableId = entry.TableId });

            return entries;
        }

        //[Command("nexus row list", Description = "Get all rows in the Nexus.")]
        //public Task<RowsApiModel> GetAllRows(CommandArgs args) => InvokeAsync<RowsApiModel>();
        #endregion
    }
}
