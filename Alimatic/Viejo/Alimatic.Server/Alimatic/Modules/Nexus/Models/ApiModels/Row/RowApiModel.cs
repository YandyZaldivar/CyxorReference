namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;

    public class RowApiModel : IdApiModel
    {
        public int? UserId { get; set; }
        public int TableId { get; set; }
    }
}
