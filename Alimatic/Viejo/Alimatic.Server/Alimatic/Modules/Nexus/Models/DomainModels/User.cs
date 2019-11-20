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
    public class User
    {
        [Key]
        public int Id { get; set; }

        public int? AccountId { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [Range(minimum: -1, maximum: int.MaxValue)]
        public int SecurityId { get; set; }

        [ForeignKey(nameof(SecurityId))]
        public virtual Security Security { get; set; }

        [InverseProperty(nameof(Row.User))]
        public virtual ModelCollection<Row> Rows { get; set; }

        [InverseProperty(nameof(UserRole.User))]
        public virtual ModelCollection<UserRole> Roles { get; set; }

        public User()
        {
            Rows = new ModelCollection<Row>();
            Roles = new ModelCollection<UserRole>();
        }
    }
}
/* { Alimatic.Server } */
