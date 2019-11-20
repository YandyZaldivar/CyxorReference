namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;
    using Cyxor.Networking;

    ////[PacketConfig(ApiId.GetColumn)]
    public class ColumnKeyApiModel : NameOrIdApiModel
    {
        public TableKeyApiModel TableModel { get; set; }
    }
}
