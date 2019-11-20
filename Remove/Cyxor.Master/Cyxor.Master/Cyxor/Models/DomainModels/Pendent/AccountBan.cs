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

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cyxor.Models
{
    using Networking;

    /*
    public class AccountBan
    {
        [Key]
        [Column(Order = 0)]
        public virtual int AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }

        [Column(Order = 1)]
        [Required]
        public virtual DateTime BanDate { get; set; }

        public virtual DateTime? UnbanDate { get; set; }

        public virtual int? BannedBy { get; set; }

        [ForeignKey(nameof(BannedBy))]
        public virtual Account BannedByAccount { get; set; }

        public virtual string Reason { get; set; }

        public AccountBan()
        {
            BanDate = DateTime.Now;
        }

        public Result Validate(Node node)
        {
            return Result.Success;
        }
    }
    */
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
