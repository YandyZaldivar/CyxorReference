using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    using Cyxor.Networking;

    ////[PacketConfig(ApiId.AddColumnRole)]
    public class AddColumnRoleApiModel : ColumnRoleKeyApiModel
    {
        [Required]
        public PermissionKeyApiModel PermissionModel { get; set; }
    }
}
