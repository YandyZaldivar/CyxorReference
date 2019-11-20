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
using System.Security;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cyxor.Models
{
    using Networking;
    using Serialization;

    using static Security.Utilities.Converter;

    /*
    public class AccountReset
    {
        [Key]
        public int AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }

        [Range(1, 255, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public byte? ExpirationTime { get; set; }

        public string SRP_s { get; set; }

        public string SRP_v { get; set; }

        [NotMapped]
        [EmailAddress]
        public string Email { get; set; }

        [CyxorIgnore]
        SecureString password;
        [NotMapped]
        public SecureString Password
        {
            get { return password; }
            set { (password = value)?.MakeReadOnly(); }
        }

        [NotMapped]
        public string InsecurePassword
        {
            get { return FromSecureString(Password); }
            set { Password = ToSecureString(value); }
        }

        public AccountReset()
        {
            ExpirationTime = 48;
        }

        internal Result Validate()
        {
            if (!string.IsNullOrEmpty(Email))
                if (!Utilities.IsValidEmailFormat(Email))
                    return new Result(ResultCode.EmailInvalidFormat);

            return Result.Success;
        }
    }
    */
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
