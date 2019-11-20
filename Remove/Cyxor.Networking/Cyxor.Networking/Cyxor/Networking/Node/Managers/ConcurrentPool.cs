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
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Cyxor.Networking
{
    using Extensions;

    using static Utilities.Threading;

    sealed class ConcurrentPool<T>
    {
        Func<T> ItemFactory;

        ConcurrentStack<T> Pool { get; }
        internal bool IsTDisposable { get; }
        internal Queue<int> PopHistory { get; }
        internal InterlockedInt PopCounter { get; }

        //internal int Count => Pool.Count;
        internal int Count => Pool.Skip(0).Count();

        internal ConcurrentPool(Func<T> itemFactory, int initialCount = 0)
        {
            if (initialCount < 0)
                throw new ArgumentOutOfRangeException();

            ItemFactory = itemFactory;

            PopHistory = new Queue<int>(10);
            PopCounter = new InterlockedInt();

            //if (Utilities.Reflection.IsInterfaceImplemented(typeof(T), nameof(IDisposable)))
            if (typeof(T).IsInterfaceImplemented(typeof(IDisposable)))
                IsTDisposable = true;

            if (initialCount == 0)
                Pool = new ConcurrentStack<T>();
            else
            {
                var items = new T[initialCount];

                for (int i = 0; i < initialCount; i++)
                    items[i] = ItemFactory();

                Pool = new ConcurrentStack<T>(items);
            }
        }

        internal T Pop()
        {
            PopCounter.Increment();

            T item;

            //if (Count == 0)
            //    item = ItemFactory();
            if (!Pool.TryPop(out item))
                item = ItemFactory();

            return item;
        }

        internal void Clear()
        {
            var item = default(T);

            while (Pool.TryPop(out item))
                if (IsTDisposable)
                    (item as IDisposable)?.Dispose();

            PopHistory.Clear();
            PopCounter.Exchange(0);
        }

        internal void Push(T t) => Pool.Push(t);
        internal T CreateNewItem() => ItemFactory();
        internal void PushRange(T[] items) => Pool.PushRange(items);
        //internal bool TryPop(out T result) => Pool.TryPop(out result);
        internal void TryPopRange(T[] items) => Pool.TryPopRange(items);
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
