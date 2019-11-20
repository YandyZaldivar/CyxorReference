using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Nexus.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    using Utilities = Cyxor.Networking.Utilities;

    class ColumnRoleController : BaseController
    {
        Task<ColumnRole> FindAsync(ColumnRoleKeyApiModel columnRoleKeyApiModel) =>
            NexusDbContext.ColumnRoles.Include(p => p.Role).Include(p => p.Column).ThenInclude(p => p.Table).SingleAsync(p =>
            (columnRoleKeyApiModel.RoleModel.IsId ? p.RoleId == columnRoleKeyApiModel.RoleModel.Id : p.Role.Name == columnRoleKeyApiModel.RoleModel.Name) &&
            ((columnRoleKeyApiModel.ColumnModel.IsId ? p.ColumnId == columnRoleKeyApiModel.ColumnModel.Id : p.Column.Name == columnRoleKeyApiModel.ColumnModel.Name) &&
            (columnRoleKeyApiModel.ColumnModel.TableModel.IsId ? p.Column.TableId == columnRoleKeyApiModel.ColumnModel.TableModel.Id : p.Column.Table.Name == columnRoleKeyApiModel.ColumnModel.TableModel.Name)));

        ColumnRoleApiModel NewColumnRoleApiModel(ColumnRole columnRole) =>
            new ColumnRoleApiModel
            {
                RoleId = columnRole.RoleId,
                ColumnId = columnRole.ColumnId,
                PermissionId = columnRole.PermissionId
            };

        #region Get
        //[Action(ApiId.GetColumnRole)]
        public async Task<ColumnRoleApiModel> GetColumnRole(GetColumnRoleApiModel getColumnRoleApiModel)
        {
            var columnRole = await FindAsync(getColumnRoleApiModel);
            return NewColumnRoleApiModel(columnRole);
        }

        //[Command("nexus column-role get", Arguments = "$role $column [$table]",
        //    Description = "Get the Nexus column-role identified by $role and $column.")]
        //public Task<ColumnRoleApiModel> GetColumnRole(CommandArgs args) => InvokeAsync<ColumnRoleApiModel>(new GetColumnRoleApiModel
        //{
        //    RoleModel = new RoleKeyApiModel { NameOrId = args["$role"] },
        //    ColumnModel = new ColumnKeyApiModel
        //    {
        //        NameOrId = args["$column"],
        //        TableModel = args["$table"] != null ? new TableKeyApiModel { NameOrId = args["$table"] } : null
        //    }
        //});
        #endregion

        #region Add
        //[Action(ApiId.AddColumnRole)]
        public async Task<ColumnRoleApiModel> AddColumnRole(AddColumnRoleApiModel addColumnRoleApiModel)
        {
            var roleId = addColumnRoleApiModel.RoleModel.IsId ? (int)addColumnRoleApiModel.RoleModel.Id : 0;
            var columnId = addColumnRoleApiModel.ColumnModel.IsId ? (int)addColumnRoleApiModel.ColumnModel.Id : 0;

            if (roleId == 0 || columnId == 0)
            {
                var modelsId = (from role in NexusDbContext.Roles
                               where addColumnRoleApiModel.RoleModel.IsId ? role.Id == addColumnRoleApiModel.RoleModel.Id : role.Name == addColumnRoleApiModel.RoleModel.Name
                               from column in NexusDbContext.Columns
                               join table in NexusDbContext.Tables on column.TableId equals table.Id
                               where addColumnRoleApiModel.ColumnModel.IsId ? column.Id == addColumnRoleApiModel.ColumnModel.Id : column.Name == addColumnRoleApiModel.ColumnModel.Name &&
                               addColumnRoleApiModel.ColumnModel.TableModel.IsId ? table.Id == addColumnRoleApiModel.ColumnModel.TableModel.Id : table.Name == addColumnRoleApiModel.ColumnModel.TableModel.Name
                               select new { RoleId = role.Id, ColumnId = column.Id }).Single();

                roleId = modelsId.RoleId;
                columnId = modelsId.ColumnId;
            }

            var columnRole = NexusDbContext.ColumnRoles.Add(new ColumnRole
            {
                RoleId = roleId,
                ColumnId = columnId,
                PermissionId = (int)Utilities.Enum.GetConstantOrDefault<PermissionValue>(addColumnRoleApiModel.PermissionModel?.NameOrId)
            }).Entity;

            await NexusDbContext.SaveChangesAsync();

            return NewColumnRoleApiModel(columnRole);
        }

        //[Command("nexus column-role add", Arguments = "$role $column [$table] [$permission]",
        //    Description = "Add a new Nexus column-role identified by $role, $column and [$table] with the optional argument $permission. " +
        //    "If $permission is not set it will default to 'Permission.Read'. The key values $role, $column and [$table] means a column-role " +
        //    "can be identified by the combination of a role id or name + column id or using the optional $table argument as role id or name " +
        //    "+ column name + table id or name. That is because while a role and table can be uniquely identified by a name a column can only be " +
        //    "identified by its id or the combination of the column name + the table id or name which the column belongs to.")]
        //public Task<ColumnRoleApiModel> AddColumnRole(CommandArgs args) => InvokeAsync<ColumnRoleApiModel>(new AddColumnRoleApiModel
        //{
        //    RoleModel = new RoleKeyApiModel { NameOrId = args["$role"] },
        //    ColumnModel = new ColumnKeyApiModel
        //    {
        //        NameOrId = args["$column"],
        //        TableModel = args["$table"] != null ? new TableKeyApiModel { NameOrId = args["$table"] } : null
        //    },
        //    PermissionModel = args["$permission"] != null ? new PermissionKeyApiModel { NameOrId = args["$permission"] } : null,
        //});
        #endregion

        #region Remove
        //[Action(ApiId.RemoveColumnRole)]
        public async Task<ColumnRoleApiModel> RemoveColumnRole(RemoveColumnRoleApiModel removeColumnRoleApiModel)
        {
            var columnRole = await FindAsync(removeColumnRoleApiModel);
            NexusDbContext.ColumnRoles.Remove(columnRole);
            await NexusDbContext.SaveChangesAsync();
            return NewColumnRoleApiModel(columnRole);
        }

        //[Command("nexus column-role remove", Arguments = "$role $column [$table]",
        //    Description = "Remove the Nexus column-role identified by $role and $column [$table].")]
        //public Task<ColumnRoleApiModel> RemoveColumnRole(CommandArgs args) => InvokeAsync<ColumnRoleApiModel>(new RemoveColumnRoleApiModel
        //{
        //    RoleModel = new RoleKeyApiModel { NameOrId = args["$role"] },
        //    ColumnModel = new ColumnKeyApiModel
        //    {
        //        NameOrId = args["$column"],
        //        TableModel = args["$table"] != null ? new TableKeyApiModel { NameOrId = args["$table"] } : null
        //    }
        //});
        #endregion

        #region Update
        //[Action(ApiId.UpdateColumnRole)]
        public async Task<ColumnRoleApiModel> UpdateColumnRole(UpdateColumnRoleApiModel updateColumnRoleApiModel)
        {
            var columnRole = await FindAsync(updateColumnRoleApiModel);

            if (updateColumnRoleApiModel.NewPermissionModel != null)
                columnRole.PermissionId = (int)Utilities.Enum.GetConstantOrDefault<PermissionValue>(updateColumnRoleApiModel.NewPermissionModel?.NameOrId);

            await NexusDbContext.SaveChangesAsync();

            return NewColumnRoleApiModel(columnRole);
        }

        //[Command("nexus column-role update", Arguments = "$role $column [$table] [$new-permission]",
        //    Description = "Update the Nexus column-role identified by $role and $column [$table] with the optional argument [$new-permission]. All variables can denote a name or id.")]
        //public Task<ColumnRoleApiModel> UpdateColumnRole(CommandArgs args) => InvokeAsync<ColumnRoleApiModel>(new UpdateColumnRoleApiModel
        //{
        //    RoleModel = new RoleKeyApiModel { NameOrId = args["$role"] },
        //    ColumnModel = new ColumnKeyApiModel
        //    {
        //        NameOrId = args["$column"],
        //        TableModel = args["$table"] != null ? new TableKeyApiModel { NameOrId = args["$table"] } : null
        //    },
        //    NewPermissionModel = args["$new-permission"] != null ? new PermissionKeyApiModel { NameOrId = args["$new-permission"] } : null
        //});
        #endregion

        #region GetAll
        //[Action(ApiId.GetAllColumnRoles)]
        public async Task<IEnumerable<ColumnRoleApiModel>> GetAllColumnRoles()
        {
            var entries = new List<ColumnRoleApiModel>();

            foreach (var entry in await NexusDbContext.ColumnRoles.AsNoTracking().ToListAsync())
                entries.Add(NewColumnRoleApiModel(entry));

            return entries;
        }

        //[Command("nexus column-role list", Description = "Get all column-roles in the Nexus.")]
        //public Task<ColumnRolesApiModel> GetAllColumnRoles(CommandArgs args) => InvokeAsync<ColumnRolesApiModel>();
        #endregion
    }
}
