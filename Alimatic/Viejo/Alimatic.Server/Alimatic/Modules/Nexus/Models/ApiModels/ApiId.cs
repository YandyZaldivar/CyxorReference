using System.ComponentModel;

using Newtonsoft.Json;

namespace Alimatic.Nexus.Models
{
    enum ApiId
    {
        [JsonRequired, Description("Task<ApiId> GetApi() {...}")]
        GetApi,
        [JsonRequired, Description("Task<InitialDataApiModel> GetInitialData() {...}")]
        GetInitialData,
        [JsonRequired, Description("Task<SecuritiesApiModel> GetAllSecurities() {...}")]
        GetAllSecurities,
        [JsonRequired, Description("Task<ColumnsTypeApiModel> GetAllColumnTypes() {...}")]
        GetAllColumnTypes,
        [JsonRequired, Description("Task<PermissionsApiModel> GetAllPermissions() {...}")]
        GetAllPermissions,

        [JsonRequired, Description("Task<UsersApiModel> GetAllUsers() {...}")]
        UserList,
        [JsonRequired, Description("Task<UserApiModel> GetUser(GetUserApiModel getUserApiModel) {...}")]
        UserGet,
        [JsonRequired, Description("Task<UserApiModel> AddUser(AddUserApiModel addUserApiModel) {...}")]
        UserAdd,
        [JsonRequired, Description("Task<UserApiModel> UpdateUser(UpdateUserApiModel updateUserApiModel) {...}")]
        UserUpdate,
        [JsonRequired, Description("Task<UserApiModel> RemoveUser(RemoveUserApiModel removeUserApiModel) {...}")]
        UserRemove,
        [JsonRequired, Description("Task<RolesApiModel> GetUserRoles(GetUserRolesApiModel getUserRolesApiModel) {...}")]
        UserGetRoles,

        [JsonRequired, Description("Task<TablesApiModel> GetAllTables() {...}")]
        GetAllTables,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        GetTable,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        AddTable,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        UpdateTable,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        RemoveTable,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        GetTableData,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        ImportTable,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        ExportTable,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        AddTableRows,
        //[JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        //public const int RemoveTableRows = 29;

        [JsonRequired, Description("Task<RowsApiModel> GetAllRows() {...}")]
        GetAllRows,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        GetRow,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        AddRow,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        UpdateRow,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        RemoveRow,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        GetRowData,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        AddRowData,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        UpdateRowData,

        [JsonRequired, Description("Task<ColumnsApiModel> GetAllColumns() {...}")]
        GetAllColumns,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        GetColumn,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        AddColumn,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        UpdateColumn,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        RemoveColumn,

        [JsonRequired, Description("Task<RowColumnsApiModel> GetAllRowColumns() {...}")]
        GetAllRowColumns,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        GetRowColumn,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        AddRowColumn,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        UpdateRowColumn,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        RemoveRowColumn,

        [JsonRequired, Description("Task<RolesApiModel> GetAllRoles() {...}")]
        GetAllRoles,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        GetRole,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        AddRole,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        UpdateRole,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        RemoveRole,

        [JsonRequired, Description("Task<UserRolesApiModel> GetAllUserRoles() {...}")]
        GetAllUserRoles,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        GetUserRole,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        AddUserRole,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        UpdateUserRole,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        RemoveUserRole,

        [JsonRequired, Description("Task<TableRolesApiModel> GetAllTableRoles() {...}")]
        GetAllTableRoles,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        GetTableRole,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        AddTableRole,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        UpdateTableRole,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        RemoveTableRole,

        [JsonRequired, Description("Task<ColumnRolesApiModel> GetAllColumnRoles() {...}")]
        GetAllColumnRoles,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        GetColumnRole,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        AddColumnRole,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        UpdateColumnRole,
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        RemoveColumnRole,
    }

    /*
    class ApiId
    {
        [JsonRequired, Description("Task<ApiId> GetApi() {...}")]
        public const int GetApi = 0;
        [JsonRequired, Description("Task<InitialDataApiModel> GetInitialData() {...}")]
        public const int GetInitialData = 1;
        [JsonRequired, Description("Task<SecuritiesApiModel> GetAllSecurities() {...}")]
        public const int GetAllSecurities = 7;
        [JsonRequired, Description("Task<ColumnsTypeApiModel> GetAllColumnTypes() {...}")]
        public const int GetAllColumnTypes = 8;
        [JsonRequired, Description("Task<PermissionsApiModel> GetAllPermissions() {...}")]
        public const int GetAllPermissions = 9;

        [JsonRequired, Description("Task<UsersApiModel> GetAllUsers() {...}")]
        public const int GetAllUsers = 10;
        [JsonRequired, Description("Task<UserApiModel> GetUser(GetUserApiModel getUserApiModel) {...}")]
        public const int GetUser = 11;
        [JsonRequired, Description("Task<UserApiModel> AddUser(AddUserApiModel addUserApiModel) {...}")]
        public const int AddUser = 12;
        [JsonRequired, Description("Task<UserApiModel> UpdateUser(UpdateUserApiModel updateUserApiModel) {...}")]
        public const int UpdateUser = 13;
        [JsonRequired, Description("Task<UserApiModel> RemoveUser(RemoveUserApiModel removeUserApiModel) {...}")]
        public const int RemoveUser = 14;
        [JsonRequired, Description("Task<RolesApiModel> GetUserRoles(GetUserRolesApiModel getUserRolesApiModel) {...}")]
        public const int GetUserRoles = 15;

        [JsonRequired, Description("Task<TablesApiModel> GetAllTables() {...}")]
        public const int GetAllTables = 20;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int GetTable = 21;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int AddTable = 22;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int UpdateTable = 23;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int RemoveTable = 24;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int GetTableData = 25;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int ImportTable = 26;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int ExportTable = 27;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int AddTableRows = 28;
        //[JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        //public const int RemoveTableRows = 29;

        [JsonRequired, Description("Task<RowsApiModel> GetAllRows() {...}")]
        public const int GetAllRows = 30;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int GetRow = 31;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int AddRow = 32;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int UpdateRow = 33;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int RemoveRow = 34;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int GetRowData = 35;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int AddRowData = 36;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int UpdateRowData = 37;

        [JsonRequired, Description("Task<ColumnsApiModel> GetAllColumns() {...}")]
        public const int GetAllColumns = 40;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int GetColumn = 41;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int AddColumn = 42;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int UpdateColumn = 43;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int RemoveColumn = 44;

        [JsonRequired, Description("Task<RowColumnsApiModel> GetAllRowColumns() {...}")]
        public const int GetAllRowColumns = 50;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int GetRowColumn = 51;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int AddRowColumn = 52;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int UpdateRowColumn = 53;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int RemoveRowColumn = 54;

        [JsonRequired, Description("Task<RolesApiModel> GetAllRoles() {...}")]
        public const int GetAllRoles = 60;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int GetRole = 61;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int AddRole = 62;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int UpdateRole = 63;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int RemoveRole = 64;

        [JsonRequired, Description("Task<UserRolesApiModel> GetAllUserRoles() {...}")]
        public const int GetAllUserRoles = 70;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int GetUserRole = 71;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int AddUserRole = 72;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int UpdateUserRole = 73;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int RemoveUserRole = 74;

        [JsonRequired, Description("Task<TableRolesApiModel> GetAllTableRoles() {...}")]
        public const int GetAllTableRoles = 80;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int GetTableRole = 81;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int AddTableRole = 82;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int UpdateTableRole = 83;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int RemoveTableRole = 84;

        [JsonRequired, Description("Task<ColumnRolesApiModel> GetAllColumnRoles() {...}")]
        public const int GetAllColumnRoles = 90;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int GetColumnRole = 91;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int AddColumnRole = 92;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int UpdateColumnRole = 93;
        [JsonRequired, Description("Task<TableDataApiModel> GetTableData(GetTableApiModel getTableApiModel) {...}")]
        public const int RemoveColumnRole = 94;
    }
    */
}
