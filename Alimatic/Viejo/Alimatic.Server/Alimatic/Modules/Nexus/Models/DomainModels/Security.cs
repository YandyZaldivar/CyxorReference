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
    public enum SecurityValue
    {
        None = 1,
        User,
        Moderator,
        Administrator,
    }

    public class Security
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string Name { get; set; }

        [NotMapped]
        public SecurityValue Value
        {
            get => (SecurityValue)Enum.Parse(typeof(SecurityValue), Name);
            set => Name = value.ToString();
        }

        [InverseProperty(nameof(User.Security))]
        public virtual ModelCollection<User> Accounts { get; set; }

        [InverseProperty(nameof(TableRole.Security))]
        public virtual ModelCollection<TableRole> TableRoles { get; set; }

        public Security()
        {
            Value = SecurityValue.User;
            Accounts = new ModelCollection<User>();
            TableRoles = new ModelCollection<TableRole>();
        }

        public static int GetIdOrDefaultId(string nameOrId) => nameOrId == null ? (int)(SecurityValue.User) :
            (int)((SecurityValue)Enum.Parse(typeof(SecurityValue), nameOrId, ignoreCase: true));
    }
}
/* { Alimatic.Server } */
