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
using System.ComponentModel.DataAnnotations.Schema;

namespace Cyxor.Models
{
    using Networking;

    public class Phone
    {
        [Key]
        public virtual int Id { get; set; }

        [Phone]
        [Required(AllowEmptyStrings = false)]
        public virtual string Number { get; set; }

        [MaxLength(64, ErrorMessage = "The maximum allowable length of 'Phone.Description' column is 64")]
        public virtual string Description { get; set; }

        public virtual List<ProfilePhone> Profiles { get; set; }

        public Result Validate(Node node)
        {
            var result = Result.Success;

            try
            {
                GetType().GetRuntimeProperty(nameof(Number)).GetCustomAttribute<PhoneAttribute>(inherit: true).Validate(Number, nameof(Number));
            }
            catch (Exception exc)
            {
                result = new Result(ResultCode.Exception, exception: exc);
            }

            return result;
        }
    }
}
*/
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
