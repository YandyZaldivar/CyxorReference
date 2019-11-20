using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public class ProductProperty : KeyApiModel<int, int>
    {
        [Key]
        public int ProductId { get => Id1; set => Id1 = value; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        [Key]
        public int PropertyId { get => Id2; set => Id2 = value; }

        [ForeignKey(nameof(PropertyId))]
        public Property Property { get; set; }

        [StringLength(16380)]
        public string Value { get; set; }
    }
}
