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
    /// Api: <c>help</c>
    ///
    /// Description: The Cyxor console is interactive. All the API defined is accessible through clear text.
    /// An API is defined by a unique path and an optional model. The model, if present, contains the data
    /// you pass to the API function formatted as a valid JSON string. For getting help information about an API
    /// you must write <c>help { api: "the desired api path" }</c>. To see the available API list you can write
    /// <c>api list</c>
    /// </summary>
    [Model("help", Description = "The Cyxor console is interactive. All the API defined is accessible through clear " +
        "text. An API is defined by a unique path and an optional model. The model, if present, contains the data " +
        "you pass to the API function formatted as a valid JSON string. For getting help information about an API " +
        "you must write 'help { api: \"the desired api path\" }'. To see the available API list you can write " +
        "'api list'.")]
    public class HelpApiModel
    {
        public string Api { get; set; }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
