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
using System.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cyxor.Models
{
    using Networking;
    using Serialization;

    using static Security.Utilities.Converter;

    public class Account : IDisposable
    {
        [CyxorIgnore]
        internal bool NamePreview { get; set; }
        [CyxorIgnore]
        internal bool EmailPreview { get; set; }
        [CyxorIgnore]
        internal bool HashPreview { get; set; }

        [Key]
        public virtual int Id { get; set; }

        [Required]
        [Column(Order = 0)]
        public virtual string Name { get; set; }

        [CyxorIgnore]
        SecureString password;
        [NotMapped]
        public virtual SecureString Password
        {
            get { return password; }
            set { (password = value)?.MakeReadOnly(); }
        }

        [NotMapped]
        [DataType(DataType.Password)]
        public virtual string InsecurePassword
        {
            get { return FromSecureString(Password); }
            set { Password = ToSecureString(value); }
        }

        [Column(Order = 1)]
        public virtual string PasswordHash { get; set; }
        
        public virtual bool Deleted { get; set; }

        public virtual bool Connected { get; set; }

        public virtual bool Locked { get; set; }

        public virtual int Security { get; set; }

        public virtual string LastIp { get; set; }

        public virtual byte FailedLogins { get; set; }

        public virtual DateTime? LastLogin { get; set; }

        public virtual DateTime JoinDate { get; set; } = DateTime.Now;

        public virtual int OriginId { get; set; }

        [Required]
        [ForeignKey(nameof(OriginId))]
        public virtual Origin Origin { get; set; } = new Origin { OriginValue = OriginValue.Server };

        [Required]
        public virtual Profile Profile { get; set; } = new Profile();

        public virtual List<AccountRole> Roles { get; set; }

        [NotMapped]
        public virtual Connection Connection { get; internal set; }

        [NotMapped]
        public virtual OriginValue AccountOrigin { get; set; }

        public override int GetHashCode() => Name != null ? Utilities.HashCode.GetFrom(Name) : 0;

        void IDisposable.Dispose() => Password?.Dispose();

        public Result Validate(Node node)
        {
            if (string.IsNullOrEmpty(Name))
                return new Result(ResultCode.NameNullOrEmpty);

            if (Security < -1)
                return new Result(ResultCode.AccountSecurityOutOfRange);

            if (node != null)
            {
                string name = Name;

                var result = node.Config.Names.Validate(ref name);

                if (!result)
                    return result;

                Name = name;
            }

            return Profile.Validate(node);
        }
    }
}
*/
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
