using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public class Image : KeyApiModel<int>
    {
        [StringLength(126, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(16380)]
        public string Description { get; set; }

        [StringLength(126)]
        public string Header { get; set; }

        [Required]
        [MaxLength(262144)]
        public byte[] Data { get; set; }

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
