using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    using Cyxor.Networking;

    public class UserRoleKeyApiModel
    {
        [Required]
        public NameOrIdApiModel UserModel { get; set; }

        [Required]
        public NameOrIdApiModel RoleModel { get; set; }
    }
}
