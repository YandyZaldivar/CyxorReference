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
using System.Collections.Generic;

namespace Cyxor
{
    class CategoryComparer : IEqualityComparer<Category>
    {
        public static CategoryComparer Instance = new CategoryComparer();

        public bool Equals(Category x, Category y) => x?.Id == y?.Id;
        public int GetHashCode(Category obj) => (obj.Name + obj.Id.ToString()).GetHashCode();
    }
}
// {Accounter} - Copyright (C) 2017  Gravitonia AS