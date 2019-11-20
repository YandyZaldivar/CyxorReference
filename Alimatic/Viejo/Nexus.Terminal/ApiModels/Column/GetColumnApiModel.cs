namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;

    [Model(ApiId.GetColumn)]
    public class GetColumnApiModel : NameOrIdApiModel
    {
        public NameOrIdApiModel TableModel { get; set; }
    }
}
