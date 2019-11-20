using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    public class TableRoleKeyApiModel
    {
        [Required]
        public RoleKeyApiModel RoleModel { get; set; }

        [Required]
        public TableKeyApiModel TableModel { get; set; }
    }
}
