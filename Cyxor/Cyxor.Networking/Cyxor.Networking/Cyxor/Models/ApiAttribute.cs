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

namespace Cyxor.Networking
{
    using Extensions;

    [AttributeUsage(AttributeTargets.Field)]
    public class ApiAttribute : Attribute
    {
        public string Route { get; }
        public bool Internal { get; }
        public Type ModelType { get; }
        public string Description { get; }

        public ApiAttribute(string route, Type modelType, string description)
        {
            Route = route;
            ModelType = modelType;
            Description = description;

            

            // TODO: Obtener el prefijo de la ruta del ModuleAttr a través del Modelo??
            //var modelAttr = ModelType.GetTypeInfo().GetCustomAttribute<ModelAttribute>(inherit: true);
        }

        //public ApiAttribute(string route, string description, bool @internal) : this(route, description) => Internal = @internal;
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
