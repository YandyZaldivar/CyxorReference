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
    public class UserRole
    {
        [Key]
        public int RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }

        [Key]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
/* { Alimatic.Server } */
