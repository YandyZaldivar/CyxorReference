namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;

    [Model(ApiId.UpdateTableRole)]
    public class UpdateTableRoleApiModel : TableRoleKeyApiModel
    {
        public NameOrIdApiModel NewSecurityModel { get; set; }
        public NameOrIdApiModel NewPermissionModel { get; set; }
    }
}
