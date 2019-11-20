// https://en.wikipedia.org/wiki/Normal_balance
// https://www.accountingtools.com/articles/what-is-the-normal-balance-for-an-account.html

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Accounting.Models
{
    public enum AccountNormalBalanceValue
    {
        Debit = 1,
        Credit
    }

    public class AccountNormalBalance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(9, MinimumLength = 2)]
        public string Name { get; set; }

        [NotMapped]
        public AccountNormalBalanceValue Value
        {
            get => (AccountNormalBalanceValue)Enum.Parse(typeof(AccountNormalBalanceValue), Name);
            set
            {
                Id = (int)value;
                Name = value.ToString();
            }
        }

        [InverseProperty(nameof(Account.NormalBalance))]
        public HashSet<Account> Accounts { get; } = new HashSet<Account>();

        public AccountNormalBalance()
        {
            Value = AccountNormalBalanceValue.Debit;
        }

        public static AccountNormalBalance[] Items { get; } = new AccountNormalBalance[]
        {
            new AccountNormalBalance { Id = (int)AccountNormalBalanceValue.Debit, Value = AccountNormalBalanceValue.Debit },
            new AccountNormalBalance { Id = (int)AccountNormalBalanceValue.Credit, Value = AccountNormalBalanceValue.Credit },
        };
    }
}
