namespace Alimatic.Nexus.Models
{
    using Cyxor.Networking;

    //[PacketConfig(ApiId.UpdateTableRole)]
    public class UpdateTableRoleApiModel : TableRoleKeyApiModel
    {
        public SecurityKeyApiModel NewSecurityModel { get; set; }
        public PermissionKeyApiModel NewPermissionModel { get; set; }
    }
}
