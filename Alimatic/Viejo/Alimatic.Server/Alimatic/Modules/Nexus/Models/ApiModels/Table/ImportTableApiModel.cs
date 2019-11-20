namespace Alimatic.Nexus.Models
{
    using Cyxor.Networking;

    public enum ImportTableFormat
    {
        Csv,
        Xml,
        Json,
        File,
    }

    //[PacketConfig(ApiId.ImportTable)]
    public class ImportTableApiModel : AddTableApiModel
    {
        public ImportTableFormat Format { get; set; }
        public string Data { get; set; }
    }
}
