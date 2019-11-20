namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;

    public class ColumnApiModel : NameAndIdApiModel
    {
        public int Order { get; set; }
        public int TypeId { get; set; }
        public int TableId { get; set; }
        public bool NotNull { get; set; }
        public string EnumValues { get; set; }
    }
}
