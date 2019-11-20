using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using System.Reflection;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Nexus.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    class NexusController : BaseController
    {
        //[Action(ApiId.GetApi)]
        public ApiId GetApi() => new ApiId();

        //[Command("nexus api list", Description = "Get the Nexus API.")]
        //public ApiId GetApi(CommandArgs args) => Invoke<ApiId>();

        //[Action(ApiId.GetInitialData)]
        public async Task<InitialDataApiModel> GetInitialData()
        {
            var initialDataApiModel = new InitialDataApiModel
            {
                Users = await InvokeAsync<UserController, IEnumerable<UserApiModel>>(nameof(UserController.List)),
                //Roles = await InvokeAsync<RoleController, RolesApiModel>(nameof(RoleController.GetAllRoles)),
                Tables = await InvokeAsync<TableController, TablesApiModel>(nameof(TableController.GetAllTables)),
                Columns = await InvokeAsync<ColumnController, ColumnsApiModel>(nameof(ColumnController.GetAllColumns)),
                UserRoles = await InvokeAsync<UserRoleController, UserRolesApiModel>(nameof(UserRoleController.GetAllUserRoles)),
                Securities = await InvokeAsync<SecurityController, SecuritiesApiModel>(nameof(SecurityController.GetAllSecurities)),
                Permissions = await InvokeAsync<PermissionController, PermissionsApiModel>(nameof(PermissionController.GetAllPermissions)),
                ColumnTypes = await InvokeAsync<ColumnTypeController, ColumnTypesApiModel>(nameof(ColumnTypeController.GetAllColumnTypes)),
                TableRoles = await InvokeAsync<TableRoleController, TableRolesApiModel>(nameof(TableRoleController.GetAllTableRoles)),
                ColumnRoles = await InvokeAsync<ColumnRoleController, ColumnRolesApiModel>(nameof(ColumnRoleController.GetAllColumnRoles)),
            };

            // TODO: Complete connection accounts
            initialDataApiModel.User = initialDataApiModel.Users.SingleOrDefault(p => p.AccountId == Connection.Account?.Id);
            //initialDataApiModel.User = initialDataApiModel.Users.Entries.FirstOrDefault(p => p.AccountId == Connection.Account?.Id);

            return initialDataApiModel;
        }

        // nexus csv import $tableName $tableData
        public void ImportTableFromCsv()
        {

        }

        // nexus csv export $tableName $tableData
        public void ExportTableToCsv()
        {

        }
    }
}
