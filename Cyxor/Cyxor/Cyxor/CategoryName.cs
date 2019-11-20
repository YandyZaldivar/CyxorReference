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
using System.Drawing.Design;
using System.ComponentModel.Design;

namespace Cyxor
{
    class CategoryName
    {
        [Browsable(false)]
        public Category Category { get; set; }

        [Browsable(false)]
        public Transaction Transaction { get; set; }

        string name;
        [TypeConverter(typeof(CategoryNameConverter))]
        public string Name
        {
            get { return Category?.Name; }
            set
            {
                Transaction.CoreTransaction.Categories.Remove(Category.CoreCategory);
                Category = Ecomania.Instance.Categories.Single(p => p.ToString() == value);
                Transaction.CoreTransaction.Categories.Add(Category.CoreCategory);
            }
        }

        [DisplayName("Parent")]
        public string ParentName => Category?.ParentName;

        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string Description => Category?.Description;

        public CategoryName() : this(null) { }

        public CategoryName(Category category)
        {
            Transaction = Ecomania.Instance.Transaction;

            if (category == null)
            {
                if (Transaction != null)
                    category = Ecomania.Instance.Categories.Except(Transaction.CategoryList, CategoryComparer.Instance).FirstOrDefault();

                if (category == null)
                    category = Ecomania.Instance.Categories.SingleOrDefault();

                category = Transaction.CoreTransaction.Categories.Add(category.CoreCategory);
            }

            Category = category;
            name = Category?.Name;
        }

        public static implicit operator CategoryName(Category category) => new CategoryName(category);
    }
}
// {Accounter} - Copyright (C) 2017  Gravitonia AS