namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;
    using Cyxor.Networking;

    //[PacketConfig(ApiId.AddRowColumn)]
    public class AddRowColumnApiModel : RowColumnKeyApiModel
    {
        public ApiModel<string> ValueModel { get; set; }
    }
}
