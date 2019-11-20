using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public class WarehouseTag : KeyApiModel<int, int>
    {
        [Key]
        public virtual int WarehouseId { get => Id1; set => Id1 = value; }

        [ForeignKey(nameof(WarehouseId))]
        public virtual Warehouse Warehouse { get; set; }

        [Key]
        public virtual int TagId { get => Id2; set => Id2 = value; }

        [ForeignKey(nameof(TagId))]
        public virtual Tag Tag { get; set; }
    }
}
