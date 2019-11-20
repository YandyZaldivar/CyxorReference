using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;
    using Cyxor.Networking;

    //[PacketConfig(ApiId.UpdateRowColumn)]
    public class UpdateRowColumnApiModel : RowColumnKeyApiModel
    {
        [Required]
        public ApiModel<string> NewValueModel { get; set; }
    }
}
