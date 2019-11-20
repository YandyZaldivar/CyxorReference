using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public class Existence : KeyApiModel<int, int, int>
    {
        [Key]
        public int MovementId { get => Id1; set => Id1 = value; }

        [ForeignKey(nameof(MovementId))]
        public Movement Movement { get; set; }

        [Key]
        public int WarehouseId { get => Id2; set => Id2 = value; }

        [ForeignKey(nameof(WarehouseId))]
        public Warehouse Warehouse { get; set; }

        [Key]
        public int ProductId { get => Id3; set => Id3 = value; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}
