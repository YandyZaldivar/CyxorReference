namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;
    using Cyxor.Networking;

    ////[PacketConfig(ApiId.AddColumn)]
    public class AddColumnApiModel : NameApiModel
    {
        public int Order { get; set; }

        public TableKeyApiModel TableModel { get; set; }
        public ColumnTypeKeyApiModel TypeModel { get; set; }
    }
}
