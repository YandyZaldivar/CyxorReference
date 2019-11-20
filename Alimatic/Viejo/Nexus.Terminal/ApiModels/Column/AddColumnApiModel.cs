namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;

    [Model(ApiId.AddColumn)]
    public class AddColumnApiModel : NameApiModel
    {
        public int Order { get; set; }

        public NameOrIdApiModel TypeModel { get; set; }
        public NameOrIdApiModel TableModel { get; set; }
    }
}
