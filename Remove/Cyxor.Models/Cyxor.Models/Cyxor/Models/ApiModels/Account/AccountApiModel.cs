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

using System.Collections.Generic;

namespace Cyxor.Models
{
    using Networking;

    public class AccountApiModel : IValidatable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int SecurityLevel { get; set; }

        public IEnumerable<ValidationError> Validate(Node node)
        {
            var name = Name;
            var result = node.Config.Names.Validate(ref name);

            if (result)
                Name = name;
            else
                yield return new ValidationError
                {
                    ErrorMessage = result.Comment,
                    MemberNames = new string[] { nameof(Name) }
                };
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
