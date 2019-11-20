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
    public enum OriginValue
    {
        None,
        Free,
        Server,
        Poweruser,
        Invitation,
    }

    public class Origin
    {
        [Key]
        public virtual int Id { get; set; }

        //TODO: [AlternateKey]
        [MaxLength(64, ErrorMessage = "The maximum allowable length of 'Phone.Description' column is 64")]
        public virtual string Value { get; set; }

        [MaxLength(64, ErrorMessage = "The maximum allowable length of 'Phone.Description' column is 64")]
        public virtual string Description { get; set; }

        public virtual int? RecruiterId { get; set; }

        [ForeignKey(nameof(RecruiterId))]
        public virtual Account Recruiter { get; set; }

        [InverseProperty(nameof(Account.Origin))]
        public virtual List<Account> Accounts { get; set; }

        [NotMapped]
        public virtual OriginValue OriginValue
        {
            get => (OriginValue)Enum.Parse(typeof(OriginValue), Value);
            set => Value = value.ToString();
        }
    }
}
*/
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
