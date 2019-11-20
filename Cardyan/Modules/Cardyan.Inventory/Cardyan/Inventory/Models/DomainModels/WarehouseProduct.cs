using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public class WarehouseProduct : KeyApiModel<int, int>
    {
        [Key]
        public int WarehouseId { get => Id1; set => Id1 = value; }

        [ForeignKey(nameof(WarehouseId))]
        public Warehouse Warehouse { get; set; }

        [Key]
        public int ProductId { get => Id2; set => Id2 = value; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        [StringLength(16380)]
        public string Description { get; set; }

        public int? Minimum { get; set; }

        public int? Maximum { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        public int? LocationId { get; set; }

        [ForeignKey("WarehouseId, LocationId")]
        public WarehouseLocation Location { get; set; }

        public int StatisticId { get; set; }

        [ForeignKey(nameof(StatisticId))]
        public WarehouseProductStatistic Statistic { get; set; }
    }
}
