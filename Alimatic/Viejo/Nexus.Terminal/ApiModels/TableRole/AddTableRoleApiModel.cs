using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;

    [Model(ApiId.AddTableRole)]
    public class AddTableRoleApiModel : TableRoleKeyApiModel
    {
        public NameOrIdApiModel SecurityModel { get; set; }
        public NameOrIdApiModel PermissionModel { get; set; }
    }
}
