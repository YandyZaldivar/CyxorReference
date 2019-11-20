namespace Alimatic.Nexus.Models
{
    using Cyxor.Networking;

    //[PacketConfig(ApiId.ExportTable)]
    public class ExportTableApiModel : AddTableApiModel
    {
        public ImportTableFormat Format { get; set; }
        public string Data { get; set; }
    }
}
