using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public class Warehouse : KeyApiModel<int>
    {
        [Required]
        [StringLength(126, MinimumLength = 1)]
        public string Code { get; set; }

        [StringLength(126, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(16380)]
        public string Description { get; set; }

        public int BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        public Branch Branch { get; set; }

        public int ValuationId { get; set; }

        [ForeignKey(nameof(ValuationId))]
        public Valuation Valuation { get; set; }

        public int StatisticId { get; set; }

        [ForeignKey(nameof(StatisticId))]
        public WarehouseStatistic Statistic { get; set; }

        [InverseProperty(nameof(WarehouseLocation.Warehouse))]
        public HashSet<WarehouseLocation> Locations { get; } = new HashSet<WarehouseLocation>();

        [InverseProperty(nameof(WarehouseProduct.Warehouse))]
        public HashSet<WarehouseProduct> Products { get; } = new HashSet<WarehouseProduct>();

        [InverseProperty(nameof(WarehouseTag.Warehouse))]
        public HashSet<WarehouseTag> Tags { get; } = new HashSet<WarehouseTag>();
    }
}
