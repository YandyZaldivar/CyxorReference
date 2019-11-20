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
    [Model("disconnect", Description = "Disconnects the node instance gracefully, waiting for all pending tasks to " +
        "complete. Use $millisecondsTimeout equal -1 or omit that argument to wait indeterminately. If the supplied " +
        "time interval elapsed, the node is disconnected immediately. Specify a $reason to inform clients of the " +
        "disconnection. Both arguments are optional, but in case of specifying both they must follow the order " +
        "displayed in the syntax.")]
    public class DisconnectApiModel
    {
        public string Reason { get; set; }
        public int MillisecondsTimeout { get; set; }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
