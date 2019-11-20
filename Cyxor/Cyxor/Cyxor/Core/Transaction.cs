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
using System.Collections.Generic;

namespace Cyxor.Core
{
    using Newtonsoft.Json;

    //public enum TransactionType
    //{
    //    Expenses,
    //    Incomes,
    //    Customs,
    //}

    public class Transaction
    {
        public long Id { get; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public decimal Money { get; set; }
        public int Quantity { get; set; } = 1;
        public string Description { get; set; }
        //public TransactionType Type { get; set; }

        [JsonIgnore]
        public Account Account { get; private set; }

        List<Category> CategoryList = new List<Category>();

        [JsonRequired]
        IEnumerable<long> CategoriesId
        {
            get { return from category in CategoryList select category.Id; }
            set
            {
                CategoryList.Clear();
                CategoryList.AddRange(from categoryId in value
                                      join category in Ecomania.Instance.Categories
                                      on categoryId equals category.Id
                                      select category);
            }
        }

        [JsonIgnore]
        public CategoryManager Categories { get; }

        [JsonIgnore]
        public NormalBalance Nature => Categories.FirstOrDefault()?.Nature ?? Account?.Nature ?? NormalBalance.None;

        [JsonConstructor]
        private Transaction(Account account, long id, string name, /*TransactionType type,*/ decimal money, int quantity, DateTime date, IEnumerable<long> categories, string description)
        {
            Id = id;
            Account = account;

            Name = name;
            //Type = type;
            Date = date;
            Money = money;
            Quantity = quantity;
            Description = description;

            CategoriesId = categories != null ? new List<long>(categories) : new List<long>();

            Categories = new CategoryManager(this, CategoryList);
        }

        internal static Transaction Create(Account account, string name, /*TransactionType type,*/ decimal money, int quantity, DateTime date, IEnumerable<long> categories, string description) =>
            new Transaction(account, ++account.TransactionsId, name ?? nameof(Transaction) + account.TransactionsId, /*type,*/ money, quantity, date, categories, description);

        public class CategoryManager : CollectionManager<Transaction, Category>
        {
            internal CategoryManager(Transaction transaction, List<Category> categories)
                : base(transaction, categories)
            { }
        }
    }
}
// {Accounter} - Copyright (C) 2017  Gravitonia AS