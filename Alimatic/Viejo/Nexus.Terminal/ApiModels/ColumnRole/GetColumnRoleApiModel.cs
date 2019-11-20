using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;

    [Model(ApiId.GetColumnRole)]
    public class GetColumnRoleApiModel
    {
        [Required]
        public NameOrIdApiModel RoleModel { get; set; }

        [Required]
        public NameOrIdApiModel ColumnModel { get; set; }
    }
}
