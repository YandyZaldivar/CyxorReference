using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Accounting.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [StringLength(126, MinimumLength = 1)]
        public string Code { get; set; }

        [Required]
        [StringLength(126, MinimumLength = 2)]
        public string Description { get; set; }

        public int NormalBalanceId { get; set; }

        [ForeignKey(nameof(NormalBalanceId))]
        public AccountNormalBalance NormalBalance { get; set; }

        public decimal Balance { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey(nameof(ParentId))]
        public Account Parent { get; set; }

        public int ClasificationId { get; set; }

        [ForeignKey(nameof(ClasificationId))]
        public AccountClasification Clasification { get; set; }

        public int TypeId { get; set; }

        [ForeignKey(nameof(TypeId))]
        public AccountType Type { get; set; }

        [InverseProperty(nameof(AccountEntry.Account))]
        public HashSet<AccountEntry> Entries { get; } = new HashSet<AccountEntry>();
    }
}
