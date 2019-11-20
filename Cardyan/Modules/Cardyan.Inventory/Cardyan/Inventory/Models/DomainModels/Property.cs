using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public class Property : KeyApiModel<int>
    {
        [Required]
        [StringLength(126, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(16380)]
        public string Description { get; set; }

        public int TypeId { get; set; }

        [ForeignKey(nameof(TypeId))]
        public PropertyType Type { get; set; }

        [InverseProperty(nameof(ProductProperty.Property))]
        public HashSet<ProductProperty> Products { get; } = new HashSet<ProductProperty>();
    }
}
