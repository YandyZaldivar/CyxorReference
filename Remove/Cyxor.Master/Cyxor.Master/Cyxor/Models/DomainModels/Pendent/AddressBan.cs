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
using System.Net;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cyxor.Models
{
    using Networking;

    /*
    public class AddressBan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Address { get; set; }

        public string RangeAddress { get; set; }

        [Required]
        public DateTime BanDate { get; set; } = DateTime.Now;

        public string Reason { get; set; }

        public int? BannedBy { get; set; }

        [ForeignKey(nameof(BannedBy))]
        public virtual Account Account { get; set; }

        public AddressBan()
        {
            BanDate = DateTime.Now;
        }

        public Result Validate(Node node)
        {
            var result = Result.Success;

            var ipAddress = default(IPAddress);

            if (!IPAddress.TryParse(Address, out ipAddress) || !IPAddress.TryParse(RangeAddress, out ipAddress))
                result = new Result(ResultCode.Error, $"Invalid address string in {nameof(AddressBan)} model");

            return result;
        }
    }
    */
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
