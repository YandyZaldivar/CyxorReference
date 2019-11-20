/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.Nexus.Models
{
    public class Table
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [InverseProperty(nameof(Row.Table))]
        public virtual ModelCollection<Row> Rows { get; set; }

        [InverseProperty(nameof(Column.Table))]
        public virtual ModelCollection<Column> Columns { get; set; }

        [InverseProperty(nameof(TableRole.Table))]
        public virtual ModelCollection<TableRole> Roles { get; set; }

        public Table()
        {
            Rows = new ModelCollection<Row>();
            Columns = new ModelCollection<Column>();
            Roles = new ModelCollection<TableRole>();
        }
    }
}
/* { Alimatic.Server } */
