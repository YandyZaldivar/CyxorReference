using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public class Tag : KeyApiModel<int>
    {
        [Required]
        [StringLength(126, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(16380)]
        public string Description { get; set; }

        [InverseProperty(nameof(BranchTag.Tag))]
        public HashSet<BranchTag> Branches { get; } = new HashSet<BranchTag>();

        [InverseProperty(nameof(WarehouseLocationTag.Tag))]
        public HashSet<WarehouseLocationTag> Locations { get; } = new HashSet<WarehouseLocationTag>();

        [InverseProperty(nameof(WarehouseTag.Tag))]
        public HashSet<WarehouseTag> Warehouses { get; } = new HashSet<WarehouseTag>();

        [InverseProperty(nameof(ProductTag.Tag))]
        public HashSet<ProductTag> Products { get; } = new HashSet<ProductTag>();

        [InverseProperty(nameof(MovementTag.Tag))]
        public HashSet<MovementTag> Movements { get; } = new HashSet<MovementTag>();

        [InverseProperty(nameof(AssociateTag.Tag))]
        public HashSet<AssociateTag> Associates { get; } = new HashSet<AssociateTag>();

        [InverseProperty(nameof(WarehouseProductTag.Tag))]
        public HashSet<WarehouseProductTag> WarehouseProducts { get; } = new HashSet<WarehouseProductTag>();
    }
}
