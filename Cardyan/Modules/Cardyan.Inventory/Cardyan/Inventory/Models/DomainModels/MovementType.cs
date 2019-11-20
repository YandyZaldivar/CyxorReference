using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public enum MovementTypeValue
    {
        In = 1,
        Out,
    }

    public class MovementType : IKeyApiModel<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 2)]
        public string Name { get; set; }

        [NotMapped]
        public MovementTypeValue Value
        {
            get => (MovementTypeValue)Enum.Parse(typeof(MovementTypeValue), Name);
            set
            {
                Id = (int)value;
                Name = value.ToString();
            }
        }

        [InverseProperty(nameof(Movement.Type))]
        public HashSet<Movement> Movements { get; } = new HashSet<Movement>();

        public MovementType()
        {
            Value = MovementTypeValue.In;
        }

        public static MovementType[] Items { get; } = new MovementType[]
        {
            new MovementType { Id = 1, Value = MovementTypeValue.In },
            new MovementType { Id = 2, Value = MovementTypeValue.Out },
        };
    }
}
