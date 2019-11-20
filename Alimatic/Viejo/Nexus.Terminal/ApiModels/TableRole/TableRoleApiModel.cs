namespace Alimatic.Nexus.Models
{
    public class TableRoleApiModel
    {
        public int RoleId { get; set; }
        public int TableId { get; set; }
        public int SecurityId { get; set; }
        public int PermissionId { get; set; }
    }
}
