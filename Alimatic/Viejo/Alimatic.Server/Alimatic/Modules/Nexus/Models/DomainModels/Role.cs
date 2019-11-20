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
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [InverseProperty(nameof(UserRole.Role))]
        public virtual ModelCollection<UserRole> Users { get; set; }

        [InverseProperty(nameof(TableRole.Role))]
        public virtual ModelCollection<TableRole> Tables { get; set; }

        [InverseProperty(nameof(ColumnRole.Role))]
        public virtual ModelCollection<ColumnRole> Columns { get; set; }

        public Role()
        {
            Users = new ModelCollection<UserRole>();
            Tables = new ModelCollection<TableRole>();
            Columns = new ModelCollection<ColumnRole>();
        }
    }
}
/* { Alimatic.Server } */
