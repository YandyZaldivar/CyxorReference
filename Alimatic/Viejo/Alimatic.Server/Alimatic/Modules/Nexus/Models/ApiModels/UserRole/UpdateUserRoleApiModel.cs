using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    using Cyxor.Networking;

    //[PacketConfig(ApiId.UpdateUserRole)]
    public class UpdateUserRoleApiModel : UserRoleKeyApiModel
    {
        [Required]
        public UserKeyApiModel NewUserModel { get; set; }

        [Required]
        public RoleKeyApiModel NewRoleModel { get; set; }
    }
}
