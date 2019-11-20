using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    using Cyxor.Networking;

    //[PacketConfig(ApiId.AddRow)]
    public class AddRowApiModel
    {
        public UserKeyApiModel UserModel { get; set; }

        [Required]
        public TableKeyApiModel TableModel { get; set; }
    }
}
