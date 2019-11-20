using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public class WarehouseLocation : KeyApiModel<int, int>
    {
        [Key]
        public int WarehouseId { get => Id1; set => Id1 = value; }

        [ForeignKey(nameof(WarehouseId))]
        public Warehouse Warehouse { get; set; }

        [Key]
        public int LocationId { get => Id2; set => Id2 = value; }

        [Required]
        [StringLength(126, MinimumLength = 1)]
        public string Code { get; set; }

        [StringLength(126, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(16380)]
        public string Description { get; set; }

        //public int WarehouseId { get; set; }

        //[ForeignKey(nameof(WarehouseId))]
        //public Warehouse Warehouse { get; set; }

        [InverseProperty(nameof(WarehouseProduct.Location))]
        public HashSet<WarehouseProduct> Products { get; } = new HashSet<WarehouseProduct>();

        [InverseProperty(nameof(WarehouseLocationTag.Location))]
        public HashSet<WarehouseLocationTag> Tags { get; } = new HashSet<WarehouseLocationTag>();
    }
}
