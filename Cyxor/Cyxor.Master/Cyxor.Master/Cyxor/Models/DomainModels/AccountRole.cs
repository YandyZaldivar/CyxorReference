/*
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

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cyxor.Models
{
    public class AccountRole : KeyApiModel<int, int>
    {
        [Key]
        public int AccountId { get => Id1; set => Id1 = value; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }

        [Key]
        public virtual int RoleId { get => Id2; set => Id2 = value; }

        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
