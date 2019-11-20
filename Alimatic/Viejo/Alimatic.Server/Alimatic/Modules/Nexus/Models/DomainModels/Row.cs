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
    public class Row
    {
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public int TableId { get; set; }

        [ForeignKey(nameof(TableId))]
        public virtual Table Table { get; set; }

        [InverseProperty(nameof(RowColumn.Row))]
        public virtual ModelCollection<RowColumn> Columns { get; set; }

        public Row()
        {
            Columns = new ModelCollection<RowColumn>();
        }
    }
}
/* { Alimatic.Server } */
