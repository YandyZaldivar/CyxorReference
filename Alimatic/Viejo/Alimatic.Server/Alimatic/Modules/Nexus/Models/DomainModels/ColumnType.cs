/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.Nexus.Models
{
    public class ColumnType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string Name { get; set; }

        [NotMapped]
        public ColumnTypeValue Value
        {
            get => (ColumnTypeValue)Enum.Parse(typeof(ColumnTypeValue), Name);
            set
            {
                Id = (int)value;
                Name = value.ToString();
            }
        }

        [InverseProperty(nameof(Column.Type))]
        public virtual ModelCollection<Column> Columns { get; set; }

        public ColumnType()
        {
            Value = ColumnTypeValue.String;
            Columns = new ModelCollection<Column>();
        }
    }
}
/* { Alimatic.Server } */
