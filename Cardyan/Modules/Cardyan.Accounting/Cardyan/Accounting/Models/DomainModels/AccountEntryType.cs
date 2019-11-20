using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Accounting.Models
{
    public enum AccountEntryTypeValue
    {
        Debit = 1,
        Credit
    }

    public class AccountEntryType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(9, MinimumLength = 2)]
        public string Name { get; set; }

        [NotMapped]
        public AccountEntryTypeValue Value
        {
            get => (AccountEntryTypeValue)Enum.Parse(typeof(AccountEntryTypeValue), Name);
            set
            {
                Id = (int)value;
                Name = value.ToString();
            }
        }

        [InverseProperty(nameof(AccountEntry.Type))]
        public HashSet<AccountEntry> Movements { get; } = new HashSet<AccountEntry>();

        public AccountEntryType()
        {
            Value = AccountEntryTypeValue.Debit;
        }

        public static AccountEntryType[] Items { get; } = new AccountEntryType[]
        {
            new AccountEntryType { Id = (int)AccountEntryTypeValue.Debit, Value = AccountEntryTypeValue.Debit },
            new AccountEntryType { Id = (int)AccountEntryTypeValue.Credit, Value = AccountEntryTypeValue.Credit },
        };
    }
}
