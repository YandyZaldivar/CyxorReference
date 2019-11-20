// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.Datadin.Produccion.Models
{
    using Cyxor.Models;

    public class Role : KeyApiModel<int>
    {
        [Required]
        [StringLength(64, MinimumLength = 2)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        [InverseProperty(nameof(UserRole.Role))]
        public virtual List<UserRole> Users { get; set; }

        public Role() => Users = new ModelCollection<UserRole>();
    }
}
// { Alimatic.Datadin } - Backend
