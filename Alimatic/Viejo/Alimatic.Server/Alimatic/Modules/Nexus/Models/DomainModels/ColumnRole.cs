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
    public class ColumnRole
    {
        [Key]
        public int RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; }

        [Key]
        public int ColumnId { get; set; }

        [ForeignKey(nameof(ColumnId))]
        public virtual Column Column { get; set; }

        public int PermissionId { get; set; }

        [ForeignKey(nameof(PermissionId))]
        public virtual Permission Permission { get; set; }
    }
}
/* { Alimatic.Server } */
