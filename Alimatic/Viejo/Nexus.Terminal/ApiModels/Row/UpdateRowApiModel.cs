namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;

    [Model(ApiId.UpdateRow)]
    public class UpdateRowApiModel : RowKeyApiModel
    {
        public NameOrIdApiModel NewUserModel { get; set; }
        public NameOrIdApiModel NewTableModel { get; set; }
    }
}
