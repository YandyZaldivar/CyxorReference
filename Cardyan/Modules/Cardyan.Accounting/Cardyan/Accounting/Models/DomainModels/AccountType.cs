using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Accounting.Models
{
    public enum AccountTypeValue
    {
        Active = 1,
        Pasive,
        Equity,
        Revenus,
        Incons
    }

    public class AccountType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(9, MinimumLength = 2)]
        public string Name { get; set; }

        [NotMapped]
        public AccountTypeValue Value
        {
            get => (AccountTypeValue)Enum.Parse(typeof(AccountTypeValue), Name);
            set
            {
                Id = (int)value;
                Name = value.ToString();
            }
        }

        [InverseProperty(nameof(Account.Type))]
        public HashSet<Account> Accounts { get; } = new HashSet<Account>();

        public AccountType()
        {
            Value = AccountTypeValue.Active;
        }

        public static AccountType[] Items { get; } = new AccountType[]
        {
            new AccountType { Id = (int)AccountTypeValue.Active, Value = AccountTypeValue.Active },
            new AccountType { Id = (int)AccountTypeValue.Pasive, Value = AccountTypeValue.Pasive },
            new AccountType { Id = (int)AccountTypeValue.Equity, Value = AccountTypeValue.Equity },
            new AccountType { Id = (int)AccountTypeValue.Revenus, Value = AccountTypeValue.Revenus },
            new AccountType { Id = (int)AccountTypeValue.Incons, Value = AccountTypeValue.Incons }
        };
    }
}
