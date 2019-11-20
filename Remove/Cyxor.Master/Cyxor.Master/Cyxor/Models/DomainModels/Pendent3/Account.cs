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

    public class Account : IValidatable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public bool Locked { get; set; }

        [Range(minimum: -1, maximum: int.MaxValue)]
        public int Security { get; set; }

        public string LastIp { get; set; }

        public byte FailedLogins { get; set; }

        public DateTime JoinDate { get; set; }

        public DateTime? LastLogin { get; set; }

        [Required]
        public virtual Profile Profile { get; set; }

        [InverseProperty(nameof(AccountRole.Account))]
        public virtual HashSet<AccountRole> Roles { get; set; }

        [NotMapped]
        public virtual Connection Connection { get; internal set; }

        public Account()
        {
            JoinDate = DateTime.Now;
            Profile = new Profile();
            Roles = new HashSet<AccountRole>();
        }

        public override int GetHashCode() => Name != null ? Serialization.Utilities.HashCode.GetFrom(Name) : 0;

        public IEnumerable<ValidationError> Validate(Node node)
        {
            var name = Name;
            var result = node.Config.Names.Validate(ref name);

            if (!result)
                yield return new ValidationError { ErrorMessage = result.Comment, MemberNames = new string[] { nameof(Name) } };
            else
                Name = name;

            foreach (var item in Profile.Validate(node))
                yield return item;
        }

        //public IEnumerable<ValidationResult> Validate(Node node)
        //{
        //    if (string.IsNullOrEmpty(Name))
        //        return new Result(ResultCode.NameNullOrEmpty);

        //    if (Security < -1)
        //        return new Result(ResultCode.AccountSecurityOutOfRange);

        //    if (node != null)
        //    {
        //        string name = Name;

        //        var result = node.Config.Names.Validate(ref name);

        //        if (!result)
        //            return result;

        //        Name = name;
        //    }

        //    return Profile.Validate(node);
        //}
    }
    */
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
