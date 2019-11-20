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
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cyxor.Networking
{
    using Controllers;

    public abstract partial class Node
    {
        internal sealed class ScopeInitializer
        {
            object[] Parameters { get; }
            MethodInfo MethodInfo { get; }
            IEnumerable<ParameterInfo> ParametersInfo { get; }

            internal ScopeInitializer(MethodInfo methodInfo)
            {
                MethodInfo = methodInfo;

                var requestParametersInfo = new List<ParameterInfo>();

                foreach (var parameter in MethodInfo.GetParameters())
                    requestParametersInfo.Add(parameter);

                ParametersInfo = new ReadOnlyCollection<ParameterInfo>(requestParametersInfo);
                Parameters = new object[ParametersInfo.Count()];
            }

            internal void Invoke(Controller controller, IServiceScope scope)
            {
                var i = 0;

                foreach (var parametersInfo in ParametersInfo)
                    Parameters[i++] = scope.GetService(parametersInfo.ParameterType);

                MethodInfo.Invoke(controller, Parameters);
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
