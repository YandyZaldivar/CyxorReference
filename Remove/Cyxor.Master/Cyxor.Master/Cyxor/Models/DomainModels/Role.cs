﻿/*
  { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/>
  Copyright (C) 2017  Yandy Zaldivar

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU Affero General Public License as
  published by the Free Software Foundation, either version 3 of the
  License, or (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU Affero General Public License for more details.

  You should have received a copy of the GNU Affero General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cyxor.Models
{
    public class Role : KeyApiModel<int>
    {
        [Required]
        [StringLength(64, MinimumLength = 2)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        [InverseProperty(nameof(AccountRole.Role))]
        public virtual List<AccountRole> Accounts { get; set; }

        public Role() => Accounts = new ModelCollection<AccountRole>();
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
