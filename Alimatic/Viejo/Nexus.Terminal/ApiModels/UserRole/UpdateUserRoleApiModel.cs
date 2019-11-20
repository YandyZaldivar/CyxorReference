using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;

    [Model(ApiId.UpdateUserRole)]
    public class UpdateUserRoleApiModel : UserRoleKeyApiModel
    {
        [Required]
        public NameOrIdApiModel NewUserModel { get; set; }

        [Required]
        public NameOrIdApiModel NewRoleModel { get; set; }
    }
}
