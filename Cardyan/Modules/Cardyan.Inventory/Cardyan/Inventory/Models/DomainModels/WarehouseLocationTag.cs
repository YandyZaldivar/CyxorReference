using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public class WarehouseLocationTag : KeyApiModel<int, int, int>
    {
        [Key]
        public int WarehouseId { get => Id1; set => Id1 = value; }

        [ForeignKey(nameof(WarehouseId))]
        public Warehouse Warehouse { get; set; }

        [Key]
        public int LocationId { get => Id2; set => Id2 = value; }

        [ForeignKey("WarehouseId, LocationId")]
        public WarehouseLocation Location { get; set; }

        [Key]
        public int TagId { get => Id3; set => Id3 = value; }

        [ForeignKey(nameof(TagId))]
        public Tag Tag { get; set; }
    }
}
