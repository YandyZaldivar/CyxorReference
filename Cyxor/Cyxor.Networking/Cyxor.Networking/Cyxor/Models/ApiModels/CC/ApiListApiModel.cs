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
    /// <summary>
    /// Api: <c>api list</c>
    ///
    /// Description: Shows all available API if executed directly on the node. When executed remotely only
    /// shows the available API for the current connection. A particular API is available for a connection
    /// if the connection has a security level greater or equal than the security level required by the API.
    /// </summary>
    [Model("api list", Description = "Shows all available API if executed directly on the node. When executed " +
        "remotely only shows the available API for the current connection. A particular API is available for a " +
        "connection if the connection has a security level greater or equal than the security level required by " +
        "the API.")]
    public class ApiListApiModel { }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
