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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cyxor.Models
{
    /*
    using Networking;

    public class Profile : IValidatable
    {
        [Key]
        public int AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }

        [StringLength(32, MinimumLength = 2)]
        public string DisplayName { get; set; }

        [StringLength(64, MinimumLength = 2)]
        public string FirstName { get; set; }

        [StringLength(64, MinimumLength = 2)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}".Trim();

        [StringLength(64, MinimumLength = 2)]
        public string City { get; set; }

        [StringLength(64, MinimumLength = 2)]
        public string Country { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        [MaxLength(64 * 1024)]
        public byte[] Avatar { get; set; }

        //public Result Validate(Node node)
        //{
        //    //if (!string.IsNullOrEmpty(Email))
        //    //    if (!Utilities.IsValidEmailFormat(Email))
        //    //        return new Result(ResultCode.EmailInvalidFormat);

        //    // TODO: Validate Avatar
        //    // 256 x 256 max pixels
        //    // 196KB max size

        //    return Result.Success;
        //}

        public IEnumerable<ValidationError> Validate(Node node)
        {
            //if (!string.IsNullOrEmpty(Email))
            //    if (!Utilities.IsValidEmailFormat(Email))
            //        return new Result(ResultCode.EmailInvalidFormat);

            // TODO: Validate Avatar
            // 256 x 256 max pixels
            // 196KB max size

            return null;
        }
    }
    */
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
