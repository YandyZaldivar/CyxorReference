/*
  {Accounter} - Personal Accounting Transactions
  Copyright (C) 2017  Gravitonia AS
  Authors:  Yandy Zaldivar
            Ramon Menendez
            John Maeland

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU Affero General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Cyxor.Core
{
    public class CollectionManager<TOwner, TItem> : IEnumerable<TItem> where TItem : class
    {
        protected TOwner Owner;
        protected List<TItem> Items;

        internal CollectionManager(TOwner owner, List<TItem> items)
        {
            Items = items;
            Owner = owner;
        }

        public void Clear() => Items.Clear();

        public IEnumerator<TItem> GetEnumerator() => ((IEnumerable<TItem>)Items).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TItem>)Items).GetEnumerator();

        public virtual TItem Add(TItem item = null)
        {
            if (item != null)
                if (!Items.Contains(item))
                    Items.Add(item);

            return item;
        }

        public virtual TItem Remove(TItem item)
        {
            Items.Remove(item);
            return item;
        }
    }
}
// {Accounter} - Copyright (C) 2017  Gravitonia AS