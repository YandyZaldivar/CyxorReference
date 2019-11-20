using System.Collections.Generic;

namespace Alimatic.Nexus.Models
{
    public class InitialDataApiModel
    {
        //public UserApiModel User { get; set; }

        //public RowsApiModel Rows { get; set; }
        public UsersApiModel Users { get; set; }
        public RolesApiModel Roles { get; set; }
        public TablesApiModel Tables { get; set; }
        public ColumnsApiModel Columns { get; set; }
        public UserRolesApiModel UserRoles { get; set; }
        //public RowColumnsApiModel RowColumns { get; set; }
        public SecuritiesApiModel Securities { get; set; }
        public TableRolesApiModel TableRoles { get; set; }
        public ColumnRolesApiModel ColumnRoles { get; set; }
        public PermissionsApiModel Permissions { get; set; }
        public ColumnTypesApiModel ColumnTypes { get; set; }
    }
}
