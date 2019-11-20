using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public class Category : KeyApiModel<int>
    {
        [Required]
        [StringLength(126, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(16380)]
        public string Description { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey(nameof(ParentId))]
        public Category Parent { get; set; }

        [InverseProperty(nameof(Product.Category))]
        public HashSet<Product> Products { get; } = new HashSet<Product>();

        [InverseProperty(nameof(WarehouseProduct.Category))]
        public HashSet<WarehouseProduct> WarehouseProducts { get; } = new HashSet<WarehouseProduct>();
    }
}
