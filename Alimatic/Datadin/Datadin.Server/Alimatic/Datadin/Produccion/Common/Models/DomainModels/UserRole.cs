// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.Datadin.Produccion.Models
{
    using Cyxor.Models;

    public class UserRole : KeyApiModel<int, int>
    {
        [Key]
        public int UserId { get => Id1; set => Id1 = value; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [Key]
        public virtual int RoleId { get => Id2; set => Id2 = value; }

        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; }
    }
}
// { Alimatic.Datadin } - Backend
