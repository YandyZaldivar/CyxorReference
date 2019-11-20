// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.Datadin.Produccion.Models
{
    using Cyxor.Models;

    public class User : KeyApiModel<int>
    {
        public int? AccountId { get; set; }

        public int? EnterpriseId { get; set; }

        [ForeignKey(nameof(EnterpriseId))]
        public Enterprise Enterprise { get; set; }

        [StringLength(64)]
        public string Name { get; set; }

        [StringLength(8192)]
        public string Password { get; set; }

        [StringLength(64)]
        public string Email { get; set; }

        public int Permission { get; set; }

        public int SecurityLevel { get; set; }

        [InverseProperty(nameof(UserRole.User))]
        public virtual HashSet<UserRole> Roles { get; set; } = new HashSet<UserRole>();

        [InverseProperty(nameof(UserModel.User))]
        public virtual HashSet<UserModel> Models { get; set; } = new HashSet<UserModel>();
    }
}
// { Alimatic.Datadin } - Backend
