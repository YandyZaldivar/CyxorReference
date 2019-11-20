using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    using Cyxor.Networking;

    //[PacketConfig(ApiId.UpdateColumnRole)]
    public class UpdateColumnRoleApiModel : ColumnRoleKeyApiModel
    {
        public PermissionKeyApiModel NewPermissionModel { get; set; }
    }
}
