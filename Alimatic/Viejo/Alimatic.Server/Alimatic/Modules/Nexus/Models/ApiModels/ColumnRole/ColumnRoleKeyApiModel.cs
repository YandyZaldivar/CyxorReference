using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    public class ColumnRoleKeyApiModel
    {
        [Required]
        public RoleKeyApiModel RoleModel { get; set; }

        [Required]
        public ColumnKeyApiModel ColumnModel { get; set; }
    }
}
