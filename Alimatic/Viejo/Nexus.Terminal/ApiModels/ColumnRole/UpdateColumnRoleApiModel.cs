using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;

    [Model(ApiId.UpdateColumnRole)]
    public class UpdateColumnRoleApiModel : GetColumnRoleApiModel
    {
        public NameOrIdApiModel NewPermissionModel { get; set; }
    }
}
