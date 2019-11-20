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

namespace Cyxor.Models
{
    [Model("delay", Description = "Creates a delay in the console input equals to the provided value in " +
        "milliseconds or equivalent to 1000ms if no value is specified. This was the first API created mostly " +
        "used for testing purposes.")]
    public class DelayApiModel
    {
        public int Milliseconds { get; set; } = 1000;
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
