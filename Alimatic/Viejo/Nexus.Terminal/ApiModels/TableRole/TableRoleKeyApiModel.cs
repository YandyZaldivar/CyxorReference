using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    public class TableRoleKeyApiModel
    {
        [Required]
        public NameOrIdApiModel RoleModel { get; set; }

        [Required]
        public NameOrIdApiModel TableModel { get; set; }
    }
}
