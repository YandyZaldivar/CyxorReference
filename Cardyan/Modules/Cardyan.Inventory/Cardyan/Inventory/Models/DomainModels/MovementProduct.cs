using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public class MovementProduct : KeyApiModel<int, int>
    {
        [Key]
        public int MovementId { get => Id1; set => Id1 = value; }

        [ForeignKey(nameof(MovementId))]
        public Movement Movement { get; set; }

        [Key]
        public int ProductId { get => Id2; set => Id2 = value; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public int Count { get; set; }

        public decimal? Price { get; set; }

        [NotMapped]
        public decimal Amount => Count * (Price ?? 0);

        [NotMapped]
        public bool TakeFromMinimum { get; set; }

        public decimal? FifoAmount { get; set; }
        public decimal? LifoAmount { get; set; }
        public decimal? AverageAmount { get; set; }
    }
}
