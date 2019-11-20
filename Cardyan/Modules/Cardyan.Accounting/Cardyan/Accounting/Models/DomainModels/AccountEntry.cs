using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Accounting.Models
{
    public class AccountEntry
    {
        //[Key]
        //public int Id { get; set; }

        public decimal Amount { get; set; }

        public int TypeId { get; set; }

        [ForeignKey(nameof(TypeId))]
        public AccountEntryType Type { get; set; }

        public int AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }

        public int TransactionId { get; set; }

        [ForeignKey(nameof(TransactionId))]
        public Transaction Transaction { get; set; }
    }
}
