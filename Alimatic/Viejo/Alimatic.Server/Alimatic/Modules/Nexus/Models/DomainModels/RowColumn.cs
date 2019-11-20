/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.Nexus.Models
{
    public class RowColumn
    {
        [Key]
        public int RowId { get; set; }

        [ForeignKey(nameof(RowId))]
        public Row Row { get; set; }

        [Key]
        public int ColumnId { get; set; }
        
        [ForeignKey(nameof(ColumnId))]
        public Column Column { get; set; }

        public string Value { get; set; }
    }
}
/* { Alimatic.Server } */
