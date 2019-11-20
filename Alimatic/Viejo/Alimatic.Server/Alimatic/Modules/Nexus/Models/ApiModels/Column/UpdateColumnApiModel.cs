namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;
    using Cyxor.Networking;

    //[PacketConfig(ApiId.UpdateColumn)]
    public class UpdateColumnApiModel : ColumnKeyApiModel
    {
        public NameApiModel NewNameModel { get; set; }
        public ApiModel<int> NewOrderModel { get; set; }
        public ApiModel<bool> NewNotNullModel { get; set; }
        public ApiModel<string> NewEnumValues { get; set; }
        public TableKeyApiModel NewTableModel { get; set; }
        public ColumnTypeKeyApiModel NewTypeModel { get; set; }
    }
}
