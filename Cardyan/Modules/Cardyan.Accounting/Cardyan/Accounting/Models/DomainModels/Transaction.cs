using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Accounting.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        [InverseProperty(nameof(AccountEntry.Transaction))]
        public HashSet<AccountEntry> Entries { get; } = new HashSet<AccountEntry>();
    }
}
