using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public class MovementTag : KeyApiModel<int, int>
    {
        [Key]
        public int MovementId { get => Id1; set => Id1 = value; }

        [ForeignKey(nameof(MovementId))]
        public Movement Movement { get; set; }

        [Key]
        public int TagId { get => Id2; set => Id2 = value; }

        [ForeignKey(nameof(TagId))]
        public Tag Tag { get; set; }
    }
}
