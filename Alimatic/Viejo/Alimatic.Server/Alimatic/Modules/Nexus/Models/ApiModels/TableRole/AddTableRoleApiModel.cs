using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    using Cyxor.Networking;

    //[PacketConfig(ApiId.AddTableRole)]
    public class AddTableRoleApiModel : TableRoleKeyApiModel
    {
        public SecurityKeyApiModel SecurityModel { get; set; }
        public PermissionKeyApiModel PermissionModel { get; set; }
    }
}
