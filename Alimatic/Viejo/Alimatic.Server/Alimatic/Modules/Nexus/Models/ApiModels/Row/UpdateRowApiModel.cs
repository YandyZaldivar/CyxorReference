namespace Alimatic.Nexus.Models
{
    using Cyxor.Networking;

    //[PacketConfig(ApiId.UpdateRow)]
    public class UpdateRowApiModel : RowKeyApiModel
    {
        public UserKeyApiModel NewUserModel { get; set; }
        public TableKeyApiModel NewTableModel { get; set; }
    }
}
