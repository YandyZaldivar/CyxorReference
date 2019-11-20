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
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cyxor.Models
{
    using Networking;

    public class Email
    {
        [Key]
        public virtual int Id { get; set; }

        [EmailAddress]
        [MaxLength(128, ErrorMessage = "The maximum allowable length of 'Email.Value' column is 128")]
        public virtual string Value { get; set; }

        [MaxLength(128, ErrorMessage = "The maximum allowable length of 'Email.Value' column is 128")]
        public virtual string Description { get; set; }

        public virtual List<ProfileEmail> Profiles { get; set; }

        public Result Validate(Node node)
        {
            var result = Result.Success;

            try
            {
                GetType().GetRuntimeProperty(nameof(Value)).GetCustomAttribute<EmailAddressAttribute>(inherit: true).Validate(Value, nameof(Value));
            }
            catch (Exception exc)
            {
                result = new Result(ResultCode.Exception, exception: exc);
            }

            return Result.Success;
        }
    }
}
*/
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
