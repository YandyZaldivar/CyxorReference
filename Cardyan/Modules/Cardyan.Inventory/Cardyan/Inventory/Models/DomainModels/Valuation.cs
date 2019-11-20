using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public enum ValuationValue
    {
        Average = 1,
        Fifo,
        Lifo,
    }

    public class Valuation : IKeyApiModel<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Name { get; set; }

        [NotMapped]
        public ValuationValue Value
        {
            get => (ValuationValue)Enum.Parse(typeof(ValuationValue), Name);
            set
            {
                Id = (int)value;
                Name = value.ToString();
            }
        }

        [InverseProperty(nameof(Warehouse.Valuation))]
        public HashSet<Warehouse> Warehouses { get; } = new HashSet<Warehouse>();

        public Valuation()
        {
            Value = ValuationValue.Fifo;
        }

        public static Valuation[] Items { get; } = new Valuation[]
        {
            new Valuation { Id = 1, Value = ValuationValue.Average },
            new Valuation { Id = 2, Value = ValuationValue.Fifo },
            new Valuation { Id = 3, Value = ValuationValue.Lifo },
        };
    }
}
