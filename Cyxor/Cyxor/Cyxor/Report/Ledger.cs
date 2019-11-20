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
    class Ledger : IComparable<Ledger>
    {
        [Browsable(false)]
        public Transaction Transaction { get; }

        [DisplayName("Date - Time")]
        public string Date => Transaction.Date.ToString("dd/MM/yy - HH:mm:ss");
        public string Name => Transaction.Name;
        public string Description => Transaction.Description;
        public string Account => Transaction.Account.Name;

        public static int MaxCategories = 10;
        public static int MaxCategoryDeepLevel = 0;

        [DisplayName(" ")]
        public string Category => GetCategoryName(level: 0);
        [DisplayName(" ")]
        public string Category1 => GetCategoryName(level: 1);
        [DisplayName(" ")]
        public string Category2 => GetCategoryName(level: 2);
        [DisplayName(" ")]
        public string Category3 => GetCategoryName(level: 3);
        [DisplayName(" ")]
        public string Category4 => GetCategoryName(level: 4);
        [DisplayName(" ")]
        public string Category5 => GetCategoryName(level: 5);
        [DisplayName(" ")]
        public string Category6 => GetCategoryName(level: 6);
        [DisplayName(" ")]
        public string Category7 => GetCategoryName(level: 7);
        [DisplayName(" ")]
        public string Category8 => GetCategoryName(level: 8);
        [DisplayName(" ")]
        public string Category9 => GetCategoryName(level: 9);
        [DisplayName(" ")]
        public string Category10 => GetCategoryName(level: 10);
        [DisplayName(" ")]
        public string Category11 => GetCategoryName(level: 11);
        [DisplayName(" ")]
        public string Category12 => GetCategoryName(level: 12);
        [DisplayName(" ")]
        public string Category13 => GetCategoryName(level: 13);
        [DisplayName(" ")]
        public string Category14 => GetCategoryName(level: 14);
        [DisplayName(" ")]
        public string Category15 => GetCategoryName(level: 15);
        [DisplayName(" ")]
        public string Category16 => GetCategoryName(level: 16);
        [DisplayName(" ")]
        public string Category17 => GetCategoryName(level: 17);
        [DisplayName(" ")]
        public string Category18 => GetCategoryName(level: 18);
        [DisplayName(" ")]
        public string Category19 => GetCategoryName(level: 19);
        [DisplayName(" ")]
        public string Category20 => GetCategoryName(level: 20);

        public decimal Begin => Math.Round(GetBalance(begin: true), 2);
        public decimal Incomes => Transaction.Nature == Core.NormalBalance.Credit ? Transaction.Money * Transaction.Quantity : 0.00M;
        public decimal Expenses => Transaction.Nature == Core.NormalBalance.Debit ? Transaction.Money * Transaction.Quantity : 0.00M;
        public decimal Ending => Math.Round(GetBalance(begin: false), 2);

        public Ledger(Transaction transaction)
        {
            Transaction = transaction;
        }

        public int CompareTo(Ledger other)
        {
            var result = Account.CompareTo(other.Account);

            if (result != 0)
                return result;

            return Transaction.Date.CompareTo(other.Transaction.Date);
        }

        string GetCategoryName(int level = 0)
        {
            var categories = new List<Category>();
            var category = Transaction.Categories.Count == 0 ? null : Transaction.Categories[0].Category;

            while (category != null)
            {
                categories.Add(category);
                category = category.Parent;
            }

            categories.Reverse();

            MaxCategoryDeepLevel = MaxCategoryDeepLevel < categories.Count ? categories.Count : MaxCategoryDeepLevel;

            if (categories.Count <= level)
                return null;

            return categories[level].Name;
        }

        decimal GetBalance(bool begin)
        {
            var total = Transaction.Account.Capital;

            foreach (var major in Ecomania.Instance.Ledger.Where(p => p.Transaction.Account == Transaction.Account))
            {
                if (begin && major == this)
                    break;

                var transaction = major.Transaction;

                if (Transaction.Account.Nature == Core.NormalBalance.Debit)
                    total += transaction.Money * transaction.Quantity * (transaction.Nature == Core.NormalBalance.Debit ? 1.00M : -1.00M);
                else
                    total += transaction.Money * transaction.Quantity * (transaction.Nature == Core.NormalBalance.Credit ? 1.00M : -1.00M);

                if (major == this)
                    break;
            }

            return total;
        }
    }
}
// {Accounter} - Copyright (C) 2017  Gravitonia AS