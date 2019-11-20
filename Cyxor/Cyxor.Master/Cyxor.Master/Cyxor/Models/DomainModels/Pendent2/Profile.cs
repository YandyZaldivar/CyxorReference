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
/*
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cyxor.Models
{
    using Networking;

    public class Profile
    {
        [Key]
        public virtual int AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }

        [StringLength(32, MinimumLength = 2, ErrorMessage = "The maximum allowable length of 'DisplayName' column is 32")]
        public virtual string DisplayName { get; set; }

        [StringLength(64, MinimumLength = 2, ErrorMessage = "The maximum allowable length of 'FirstName' column is 64")]
        public virtual string FirstName { get; set; }

        [StringLength(64, MinimumLength = 2, ErrorMessage = "The maximum allowable length of 'LastName' column is 64")]
        public virtual string LastName { get; set; }

        public virtual string FullName => $"{FirstName} {LastName}";

        [MaxLength(32, ErrorMessage = "The maximum allowable length of 'City' column is 32")]
        public virtual string City { get; set; }

        [MaxLength(32, ErrorMessage = "The maximum allowable length of 'Country' column is 32")]
        public virtual string Country { get; set; }

        public virtual int? PhoneId { get; set; }

        [Phone]
        [ForeignKey(nameof(PhoneId))]
        public virtual Phone Phone { get; set; }

        public virtual int? EmailId { get; set; }

        [EmailAddress]
        [ForeignKey(nameof(EmailId))]
        public virtual Email Email { get; set; }

        public virtual List<ProfileEmail> Emails { get; set; }

        public virtual List<ProfilePhone> Phones { get; set; }

        [DataType(DataType.Date)]
        public virtual DateTime? Birthday { get; set; }

        [MaxLength(196 * 1024, ErrorMessage = "The maximum allowable length of 'Avatar' column is 196KB")]
        public virtual byte[] Avatar { get; set; }

        public Result Validate(Node node)
        {
            if (!string.IsNullOrEmpty(Email))
                if (!Utilities.IsValidEmailFormat(Email))
                    return new Result(ResultCode.EmailInvalidFormat);

            // TODO: Validate Avatar
            // 256 x 256 max pixels
            // 196KB max size

            return Result.Success;
        }
    }
}
*/
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
