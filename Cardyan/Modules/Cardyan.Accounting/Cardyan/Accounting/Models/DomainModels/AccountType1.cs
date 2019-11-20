using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Accounting.Models
{
    // Account Type



    // Modern clasification:
    // 1 - Assets               - Debit+
    // 2 - Liabilities          - Credit+
    // 3 - Equity / Capital     - Credit+
    // 4 - Income / Revenue     - Credit+
    // 5 - Expenses             - Debit+
    // 6 - Drawings             - Debit+

    // Traditional clasification:
    // 1 - Real
    // 2 - Personal
    // 3 - Nominal

    //public enum AccountTypeValue
    //{
    //    Debit = 1,
    //    Credit
    //}

    //public class AccountType
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    //    public int Id { get; set; }

    //    [Required]
    //    [StringLength(9, MinimumLength = 2)]
    //    public string Name { get; set; }

    //    [NotMapped]
    //    public AccountEntryTypeValue Value
    //    {
    //        get => (AccountEntryTypeValue)Enum.Parse(typeof(AccountEntryTypeValue), Name);
    //        set
    //        {
    //            Id = (int)value;
    //            Name = value.ToString();
    //        }
    //    }

    //    [InverseProperty(nameof(AccountEntry.Type))]
    //    public HashSet<AccountEntry> Movements { get; } = new HashSet<AccountEntry>();

    //    public AccountType()
    //    {
    //        Value = AccountEntryTypeValue.Debit;
    //    }

    //    public static AccountEntryType[] Items { get; } = new AccountEntryType[]
    //    {
    //        new AccountEntryType { Id = (int)AccountEntryTypeValue.Debit, Value = AccountEntryTypeValue.Debit },
    //        new AccountEntryType { Id = (int)AccountEntryTypeValue.Credit, Value = AccountEntryTypeValue.Credit },
    //    };
    //}
}
