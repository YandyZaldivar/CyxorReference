using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    //using Cyxor.Models;

    //public class Branch : KeyApiModel<int>
    public class Branch
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(126, MinimumLength = 1)]
        public string Code { get; set; }

        [StringLength(126, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(16380)]
        public string Description { get; set; }

        [InverseProperty(nameof(Warehouse.Branch))]
        public HashSet<Warehouse> Warehouses { get; } = new HashSet<Warehouse>();

        [InverseProperty(nameof(BranchTag.Branch))]
        public HashSet<BranchTag> Tags { get; } = new HashSet<BranchTag>();
    }
}
