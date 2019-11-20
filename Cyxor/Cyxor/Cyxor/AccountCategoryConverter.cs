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
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Cyxor
{
    public class AccountCategoryConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var categoryExcept = new List<Category> { context.Instance as Category };

            var categories = Ecomania.Instance.Categories.Except(categoryExcept, CategoryComparer.Instance);

            //var categoryNames = (from category in categories select category.Name).ToList();
            var categoryNames = (from category in categories select category).ToList();
            //categoryNames.Insert(0, "-Null-");
            categoryNames.Insert(0, null);

            return new StandardValuesCollection(categoryNames);
        }
    }
}
// {Accounter} - Copyright (C) 2017  Gravitonia AS