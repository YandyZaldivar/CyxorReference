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

#if NET35 || NET40

using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    [AsyncMethodBuilder(typeof(AsyncTaskMethodBuilder<>))]
    public class ValueTask<TResult> : Task<TResult>
    {
        public ValueTask(Func<TResult> function) : base(function) { }
        public ValueTask(Func<TResult> function, CancellationToken cancellationToken) : base(function, cancellationToken) { }
        public ValueTask(Func<TResult> function, TaskCreationOptions creationOptions) : base(function, creationOptions) { }
        public ValueTask(Func<TResult> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(function, cancellationToken, creationOptions) { }

        public ValueTask(Func<object, TResult> function, object state) : base(function, state) { }
        public ValueTask(Func<object, TResult> function, object state, CancellationToken cancellationToken) : base(function, state, cancellationToken) { }
        public ValueTask(Func<object, TResult> function, object state, TaskCreationOptions creationOptions) : base(function, state, creationOptions) { }
        public ValueTask(Func<object, TResult> function, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(function, state, cancellationToken, creationOptions) { }
    }
}
#endif
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
