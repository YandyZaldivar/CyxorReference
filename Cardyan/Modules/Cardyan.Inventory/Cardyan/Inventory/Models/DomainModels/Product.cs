using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public class Product : KeyApiModel<int>
    {
        [Required]
        [StringLength(126, MinimumLength = 1)]
        public string Code { get; set; }

        [StringLength(126, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(16380)]
        public string Description { get; set; }

        public int MeasurementUnitId { get; set; }

        [ForeignKey(nameof(MeasurementUnitId))]
        public MeasurementUnit MeasurementUnit { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        public int StatisticId { get; set; }

        [ForeignKey(nameof(StatisticId))]
        public ProductStatistic Statistic { get; set; }

        [InverseProperty(nameof(Image.Product))]
        public HashSet<Image> Images { get; } = new HashSet<Image>();

        [InverseProperty(nameof(WarehouseProduct.Product))]
        public HashSet<WarehouseProduct> Warehouses { get; } = new HashSet<WarehouseProduct>();

        [InverseProperty(nameof(ProductProperty.Product))]
        public HashSet<ProductProperty> Properties { get; } = new HashSet<ProductProperty>();

        [InverseProperty(nameof(ProductTag.Product))]
        public HashSet<ProductTag> Tags { get; } = new HashSet<ProductTag>();
    }
}
