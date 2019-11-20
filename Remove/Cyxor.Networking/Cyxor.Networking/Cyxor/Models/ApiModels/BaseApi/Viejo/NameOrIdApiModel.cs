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

using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace Cyxor.Models
{
    public class NameOrIdApiModel
    {
        [Required]
#if !NET35 && !NET40
        [MaxLength(32)]
#endif
        [System.ComponentModel.Description("Required")]
        public string NameOrId { get; set; }

        [JsonIgnore]
        public bool IsId => Id != null;
        [JsonIgnore]
        public string Name => IsId ? null : NameOrId;
        [JsonIgnore]
        public int? Id => int.TryParse(NameOrId, out var id) ? (int?)id : null;
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
