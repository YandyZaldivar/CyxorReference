using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public class BranchTag : KeyApiModel<int, int>
    {
        [Key]
        public int BranchId { get => Id1; set => Id1 = value; }

        [ForeignKey(nameof(BranchId))]
        public Branch Branch { get; set; }

        [Key]
        public int TagId { get => Id2; set => Id2 = value; }

        [ForeignKey(nameof(TagId))]
        public Tag Tag { get; set; }
    }
}
