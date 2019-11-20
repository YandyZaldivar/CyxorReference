using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public enum PropertyTypeValue
    {
        Boolean = 1,
        Char,
        Byte,
        SByte,
        Int16,
        UInt16,
        Int32,
        Int64Enum,
        UInt32,
        Int64,
        UInt64,
        Single,
        Double,
        Decimal,
        Guid,
        Binary,
        File,
        String,
        StringEnum,
        Text,
        LongText,
        Date,
        Time,
        DateTime,
        TimeSpan,
        DateTimeOffset,
    }

    public class PropertyType : IKeyApiModel<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string Name { get; set; }

        [NotMapped]
        public PropertyTypeValue Value
        {
            get => (PropertyTypeValue)Enum.Parse(typeof(PropertyTypeValue), Name);
            set
            {
                Id = (int)value;
                Name = value.ToString();
            }
        }

        [InverseProperty(nameof(Property.Type))]
        public HashSet<Property> Properties { get; } = new HashSet<Property>();

        public PropertyType()
        {
            Value = PropertyTypeValue.String;
        }

        public static PropertyType[] Items { get; } = new PropertyType[]
        {
            new PropertyType { Id = 1, Value = PropertyTypeValue.Binary },
            new PropertyType { Id = 2, Value = PropertyTypeValue.Boolean },
            new PropertyType { Id = 3, Value = PropertyTypeValue.Byte },
            new PropertyType { Id = 4, Value = PropertyTypeValue.Char },
            new PropertyType { Id = 5, Value = PropertyTypeValue.Date },
            new PropertyType { Id = 6, Value = PropertyTypeValue.DateTime },
            new PropertyType { Id = 7, Value = PropertyTypeValue.DateTimeOffset },
            new PropertyType { Id = 8, Value = PropertyTypeValue.Decimal },
            new PropertyType { Id = 9, Value = PropertyTypeValue.Double },
            new PropertyType { Id = 10, Value = PropertyTypeValue.File },
            new PropertyType { Id = 11, Value = PropertyTypeValue.Guid },
            new PropertyType { Id = 12, Value = PropertyTypeValue.Int16 },
            new PropertyType { Id = 13, Value = PropertyTypeValue.Int32 },
            new PropertyType { Id = 14, Value = PropertyTypeValue.Int64 },
            new PropertyType { Id = 15, Value = PropertyTypeValue.Int64Enum },
            new PropertyType { Id = 16, Value = PropertyTypeValue.LongText },
            new PropertyType { Id = 17, Value = PropertyTypeValue.SByte },
            new PropertyType { Id = 18, Value = PropertyTypeValue.Single },
            new PropertyType { Id = 19, Value = PropertyTypeValue.String },
            new PropertyType { Id = 20, Value = PropertyTypeValue.StringEnum },
            new PropertyType { Id = 21, Value = PropertyTypeValue.Text },
            new PropertyType { Id = 22, Value = PropertyTypeValue.Time },
            new PropertyType { Id = 23, Value = PropertyTypeValue.TimeSpan },
            new PropertyType { Id = 24, Value = PropertyTypeValue.UInt16 },
            new PropertyType { Id = 25, Value = PropertyTypeValue.UInt32 },
            new PropertyType { Id = 26, Value = PropertyTypeValue.UInt64 },
        };
    }
}
