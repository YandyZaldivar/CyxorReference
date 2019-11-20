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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cyxor.Models
{
    using Networking;
    using Serialization;

    using static Security.Utilities.Converter;

    /*
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

        [Column(Order = 1)]
        public virtual bool Deleted { get; set; }

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

        public virtual bool Connected { get; set; }

        public virtual bool Locked { get; set; }

        public virtual int Security { get; set; }

        public virtual string LastIp { get; set; }

        public virtual short FailedLogins { get; set; }

        public virtual DateTime? LastLogin { get; set; }

        [Required]
        public virtual DateTime JoinDate { get; set; }

        [EnumDataType(typeof(AccountCreationSource))]
        public virtual AccountCreationSource CreationSource { get; set; }

        public virtual int RecruiterId { get; set; }

        [Range(1, 255, ErrorMessage = "Value for {0} must be between {1} and {2} hours.")]
        public virtual byte? ExpirationHours { get; set; }

        //public virtual string ActivationCode { get; set; }

        public string SRP_s { get; set; }
        public string SRP_v { get; set; }

        //[InverseProperty(nameof(AccountBan.Account))]
        //public virtual AccountBan Ban { get; set; }

        //[InverseProperty(nameof(AccountBan.BannedByAccount))]
        //public virtual List<AccountBan> AccountsBanned { get; set; }

        public virtual List<AddressBan> AddressesBanned { get; set; }

        public virtual AccountReset Reset { get; set; }

        [Required]
        public virtual AccountProfile Profile { get; set; }

        public virtual List<AccountRole> AccountRoles { get; set; }

        public virtual List<AccountGroup> AccountGroups { get; set; }

        [NotMapped]
        public virtual Connection Connection { get; internal set; }

        // TODO:
        // public virtual ICollection<Group> Group { get; set; }

        public Account()
        {
            JoinDate = DateTime.Now;
        }

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
    */
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
