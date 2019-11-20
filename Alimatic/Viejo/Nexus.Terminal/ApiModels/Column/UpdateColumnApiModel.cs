namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;

    [Model(ApiId.UpdateColumn)]
    public class UpdateColumnApiModel : GetColumnApiModel
    {
        public int? NewOrder { get; set; }
        public NameApiModel NewNameModel { get; set; }
        public NameOrIdApiModel NewTypeModel { get; set; }
        public NameOrIdApiModel NewTableModel { get; set; }
    }
}
