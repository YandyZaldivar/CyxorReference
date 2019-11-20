using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Accounting.Models
{
    public enum AccountClasificationValue
    {
        Real = 1,
        Nominal
    }

    public class AccountClasification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(9, MinimumLength = 2)]
        public string Name { get; set; }

        [NotMapped]
        public AccountClasificationValue Value
        {
            get => (AccountClasificationValue)Enum.Parse(typeof(AccountClasificationValue), Name);
            set
            {
                Id = (int)value;
                Name = value.ToString();
            }
        }

        [InverseProperty(nameof(Account.Clasification))]
        public HashSet<Account> Accounts { get; } = new HashSet<Account>();

        public AccountClasification()
        {
            Value = AccountClasificationValue.Real;
        }

        public static AccountClasification[] Items { get; } = new AccountClasification[]
        {
            new AccountClasification { Id = (int)AccountClasificationValue.Real, Value = AccountClasificationValue.Real },
            new AccountClasification { Id = (int)AccountClasificationValue.Nominal, Value = AccountClasificationValue.Nominal },
        };
    }
}
