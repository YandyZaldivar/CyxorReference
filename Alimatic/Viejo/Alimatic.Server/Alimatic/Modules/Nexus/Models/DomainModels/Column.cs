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
    public class Column
    {
        [Key]
        public int Id { get; set; }

        public int Order { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        public bool NotNull { get; set; }

        // MySQL bug!!
        [StringLength(16380)]
        public string EnumValues { get; set; }

        public int TableId { get; set; }

        [ForeignKey(nameof(TableId))]
        public virtual Table Table { get; set; }

        public int TypeId { get; set; }

        [ForeignKey(nameof(TypeId))]
        public virtual ColumnType Type { get; set; }

        [InverseProperty(nameof(RowColumn.Column))]
        public virtual ModelCollection<RowColumn> Rows { get; set; }

        [InverseProperty(nameof(ColumnRole.Column))]
        public virtual ModelCollection<ColumnRole> Roles { get; set; }

        public Column()
        {
            Rows = new ModelCollection<RowColumn>();
            Roles = new ModelCollection<ColumnRole>();
        }
    }
}
/* { Alimatic.Server } */
