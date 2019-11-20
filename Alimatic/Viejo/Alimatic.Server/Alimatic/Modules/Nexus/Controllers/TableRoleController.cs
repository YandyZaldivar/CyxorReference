using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Nexus.Controllers
{
    using Data;
    using Models;

    using Cyxor.Networking;
    using Cyxor.Controllers;

    class TableRoleController : BaseController
    {
        Task<TableRole> FindAsync(TableRoleKeyApiModel tableRoleKeyApiModel) =>
            (from table in NexusDbContext.Tables
             where tableRoleKeyApiModel.TableModel.IsId ? table.Id == tableRoleKeyApiModel.TableModel.Id : table.Name == tableRoleKeyApiModel.TableModel.Name
             from role in NexusDbContext.Roles
             where tableRoleKeyApiModel.RoleModel.IsId ? role.Id == tableRoleKeyApiModel.RoleModel.Id : role.Name == tableRoleKeyApiModel.RoleModel.Name
             from tableRole in NexusDbContext.TableRoles
             where tableRole.TableId == table.Id && tableRole.RoleId == role.Id
             select tableRole).SingleAsync();

        //var tableRole = await NexusDbContext.TableRoles.Include(p => p.Role).Include(p => p.Table).SingleAsync(p =>
        //    (updateTableRoleApiModel.RoleModel.IsId ? p.RoleId == updateTableRoleApiModel.RoleModel.Id : p.Role.Name == updateTableRoleApiModel.RoleModel.Name) &&
        //    (updateTableRoleApiModel.TableModel.IsId ? p.TableId == updateTableRoleApiModel.TableModel.Id : p.Table.Name == updateTableRoleApiModel.TableModel.Name));

        TableRoleApiModel NewTableRoleApiModel(TableRole tableRole) =>
            new TableRoleApiModel
            {
                RoleId = tableRole.RoleId,
                TableId = tableRole.TableId,
                SecurityId = tableRole.SecurityId,
                PermissionId = tableRole.PermissionId
            };

        #region Get
        //[Action(ApiId.GetTableRole)]
        public async Task<TableRoleApiModel> GetTableRole(GetTableRoleApiModel getTableRoleApiModel)
        {
            var tableRole = await FindAsync(getTableRoleApiModel);
            return NewTableRoleApiModel(tableRole);
        }

        //[Command("nexus table-role get", Arguments = "$role $table",
        //    Description = "Get the Nexus table-role identified by $role and $table.")]
        //public Task<TableRoleApiModel> GetTableRole(CommandArgs args) => InvokeAsync<TableRoleApiModel>(new GetTableRoleApiModel
        //{
        //    RoleModel = new RoleKeyApiModel { NameOrId = args["$role"] },
        //    TableModel = new TableKeyApiModel { NameOrId = args["$table"] },
        //});
        #endregion

        #region Add
        //[Action(ApiId.AddTableRole)]
        public async Task<TableRoleApiModel> AddTableRole(AddTableRoleApiModel addTableRoleApiModel)
        {
            var roleId = addTableRoleApiModel.RoleModel.IsId ? (int)addTableRoleApiModel.RoleModel.Id : 0;
            var tableId = addTableRoleApiModel.TableModel.IsId ? (int)addTableRoleApiModel.TableModel.Id : 0;

            if (roleId == 0 || tableId == 0)
            {
                var modelsId = (from role in NexusDbContext.Roles
                                where addTableRoleApiModel.RoleModel.IsId ? role.Id == addTableRoleApiModel.RoleModel.Id : role.Name == addTableRoleApiModel.RoleModel.Name
                                from table in NexusDbContext.Tables
                                where addTableRoleApiModel.TableModel.IsId ? table.Id == addTableRoleApiModel.TableModel.Id : table.Name == addTableRoleApiModel.TableModel.Name
                                select new { RoleId = role.Id, TableId = table.Id }).Single();

                roleId = modelsId.RoleId;
                tableId = modelsId.TableId;
            }

            var tableRole = NexusDbContext.TableRoles.Add(new TableRole
            {
                TableId = tableId,
                RoleId = roleId,
                SecurityId = (int)Utilities.Enum.GetConstantOrDefault<SecurityValue>(addTableRoleApiModel.SecurityModel?.NameOrId),
                PermissionId = (int)Utilities.Enum.GetConstantOrDefault<PermissionValue>(addTableRoleApiModel.PermissionModel?.NameOrId),
            }).Entity;

            await NexusDbContext.SaveChangesAsync();

            return NewTableRoleApiModel(tableRole);
        }

        //[Command("nexus table-role add", Arguments = "$role $table [$security] [$permission]",
        //    Description = "Add a new Nexus table-role with the specified association of $tableNameOrId and $roleNameOrId and the optional arguments $securityNameOrId and $permissionNameOrId. " +
        //    "If $securityNameOrId or $permissionNameOrId are not set they will default to 'Security.User' and 'Permission.Read' respectively.")]
        //public Task<TableRoleApiModel> AddTableRole(CommandArgs args) => InvokeAsync<TableRoleApiModel>(new AddTableRoleApiModel
        //{
        //    RoleModel = new RoleKeyApiModel { NameOrId = args["$role"] },
        //    TableModel = new TableKeyApiModel { NameOrId = args["$table"] },
        //    SecurityModel = args["$security"] != null ? new SecurityKeyApiModel { NameOrId = args["$security"] } : null,
        //    PermissionModel = args["$permission"] != null ? new PermissionKeyApiModel { NameOrId = args["$permission"] } : null,
        //});
        #endregion

        #region Remove
        //[Action(ApiId.RemoveTableRole)]
        public async Task<TableRoleApiModel> RemoveTableRole(RemoveTableRoleApiModel removeTableRoleApiModel)
        {
            var tableRole = await FindAsync(removeTableRoleApiModel);
            NexusDbContext.TableRoles.Remove(tableRole);
            await NexusDbContext.SaveChangesAsync();
            return NewTableRoleApiModel(tableRole);
        }

        //[Command("nexus table-role remove", Arguments = "$role $table",
        //    Description = "Remove the Nexus table-role identified by $role and $table.")]
        //public Task<TableRoleApiModel> RemoveTableRole(CommandArgs args) => InvokeAsync<TableRoleApiModel>(new RemoveTableRoleApiModel
        //{
        //    RoleModel = new RoleKeyApiModel { NameOrId = args["$role"] },
        //    TableModel = new TableKeyApiModel { NameOrId = args["$table"] },
        //});
        #endregion

        #region Update
        //[Action(ApiId.UpdateTableRole)]
        public async Task<TableRoleApiModel> UpdateTableRole(UpdateTableRoleApiModel updateTableRoleApiModel)
        {
            var tableRole = await FindAsync(updateTableRoleApiModel);

            if (updateTableRoleApiModel.NewSecurityModel != null)
                tableRole.SecurityId = (int)Utilities.Enum.GetConstantOrDefault<SecurityValue>(updateTableRoleApiModel.NewSecurityModel?.NameOrId);

            if (updateTableRoleApiModel.NewPermissionModel != null)
                tableRole.PermissionId = (int)Utilities.Enum.GetConstantOrDefault<PermissionValue>(updateTableRoleApiModel.NewPermissionModel?.NameOrId);

            await NexusDbContext.SaveChangesAsync();

            return NewTableRoleApiModel(tableRole);
        }

        //[Command("nexus table-role update", Arguments = "$role $table [$new-security] [$new-permission]",
        //    Description = "Update the Nexus table-role identified by $table and $role with the optional arguments [$new-security] and [$new-permission]. All variables can denote a name or id.")]
        //public Task<TableRoleApiModel> UpdateTableRole(CommandArgs args) => InvokeAsync<TableRoleApiModel>(new UpdateTableRoleApiModel
        //{
        //    RoleModel = new RoleKeyApiModel { NameOrId = args["$role"] },
        //    TableModel = new TableKeyApiModel { NameOrId = args["$table"] },
        //    NewSecurityModel = args["$new-security"] != null ? new SecurityKeyApiModel { NameOrId = args["$new-security"] } : null,
        //    NewPermissionModel = args["$new-permission"] != null ? new PermissionKeyApiModel { NameOrId = args["$new-permission"] } : null,
        //});
        #endregion

        #region GetAll
        //[Action(ApiId.GetAllTableRoles)]
        public async Task<IEnumerable<TableRoleApiModel>> GetAllTableRoles()
        {
            var entries = new List<TableRoleApiModel>();

            foreach (var entry in await NexusDbContext.TableRoles.AsNoTracking().ToListAsync())
                entries.Add(NewTableRoleApiModel(entry));

            return entries;
        }

        //[Command("nexus table-role list", Description = "Get all table-roles in the Nexus.")]
        //public Task<TableRolesApiModel> GetAllTableRoles(CommandArgs args) => InvokeAsync<TableRolesApiModel>();
        #endregion
    }
}
