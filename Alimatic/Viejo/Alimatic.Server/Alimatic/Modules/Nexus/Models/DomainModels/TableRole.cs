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
    public class TableRole
    {
        [Key]
        public int RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; }

        [Key]
        public int TableId { get; set; }

        [ForeignKey(nameof(TableId))]
        public virtual Table Table { get; set; }

        public int PermissionId { get; set; }

        [ForeignKey(nameof(PermissionId))]
        public virtual Permission Permission { get; set; }

        public int SecurityId { get; set; }

        [ForeignKey(nameof(SecurityId))]
        public virtual Security Security { get; set; }

        public bool OverrideColumnsPermission { get; set; }
    }
}
/* { Alimatic.Server } */
