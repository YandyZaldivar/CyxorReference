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

namespace Cyxor.Networking.Filters
{
    using Extensions;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ActionFilterAttribute : Attribute
    {
        public int Group { get; set; }
        public int Order { get; set; }

        public object Filter { get; }
        public IActionFilter ActionFilter { get; }
        public IAsyncActionFilter AsyncActionFilter { get; }

        public ActionFilterAttribute(Type filterType, params object[] arguments)
        {
            if (!filterType.IsInterfaceImplemented<IActionFilter>() && !filterType.IsInterfaceImplemented<IAsyncActionFilter>())
                throw new ArgumentException($"The argument must implement interface {nameof(IActionFilter)} or {nameof(IAsyncActionFilter)}", nameof(filterType));

            Filter = Activator.CreateInstance(filterType, arguments);

            if (Filter is IActionFilter actionFilter)
                ActionFilter = actionFilter;
            else
                AsyncActionFilter = Filter as IAsyncActionFilter;

            if (Filter is IOrderedFilter orderedFilter)
            {
                orderedFilter.Group = Group;
                orderedFilter.Order = Order;
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
