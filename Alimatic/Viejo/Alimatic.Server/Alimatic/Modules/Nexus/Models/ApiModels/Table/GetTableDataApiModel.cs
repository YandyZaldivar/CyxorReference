namespace Alimatic.Nexus.Models
{
    using Cyxor.Networking;

    //[PacketConfig(ApiId.GetTableData)]
    public class GetTableDataApiModel : TableKeyApiModel
    {
        public int? StartRow { get; set; }
        public int? RowCount { get; set; }

        //public ApiModel<int> StartRow { get; set; }
        //public ApiModel<int> RowCount { get; set; }
    }
}
