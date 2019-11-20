namespace Alimatic.Nexus.Models
{
    public class ColumnApiModel : NameAndIdApiModel
    {
        public int Order { get; set; }
        public int TypeId { get; set; }
        public int TableId { get; set; }
    }
}
