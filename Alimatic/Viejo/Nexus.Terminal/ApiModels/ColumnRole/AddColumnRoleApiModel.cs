using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;

    [Model(ApiId.AddColumnRole)]
    public class AddColumnRoleApiModel : GetColumnRoleApiModel
    {
        [Required]
        public NameOrIdApiModel PermissionModel { get; set; }
    }
}
