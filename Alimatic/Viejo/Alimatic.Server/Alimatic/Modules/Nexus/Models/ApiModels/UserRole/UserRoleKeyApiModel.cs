using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    public class UserRoleKeyApiModel
    {
        [Required]
        public UserKeyApiModel UserModel { get; set; }

        [Required]
        public RoleKeyApiModel RoleModel { get; set; }
    }
}
