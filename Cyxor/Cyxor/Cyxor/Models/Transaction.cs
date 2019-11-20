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
using System.Drawing.Design;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Cyxor
{
    [DefaultProperty(nameof(Name))]
    [TypeConverter(typeof(TransactionConverter))]
    class Transaction
    {
        [Browsable(false)]
        public Account Account { get; set; }

        [Browsable(false)]
        public Ledger Ledger { get; set; }

        [Browsable(false)]
        public Core.Transaction CoreTransaction { get; set; }

        [DefaultValue(-1)]
        [Browsable(false)]
        public long Id => CoreTransaction != null ? CoreTransaction.Id : 88;

        [DefaultValue(nameof(Transaction))]
        public string Name
        {
            get { return CoreTransaction?.Name; }
            set { CoreTransaction.Name = value; }
        }

        [DefaultValue("0.00")]
        public decimal Money
        {
            get { return CoreTransaction != null ? CoreTransaction.Money : 0.00M; }
            set { CoreTransaction.Money = value; }
        }

        [DefaultValue(1)]
        public int Quantity
        {
            get { return CoreTransaction != null ? CoreTransaction.Quantity : 1; }
            set { CoreTransaction.Quantity = value; }
        }

        [DefaultValue(null)]
        public DateTime Date
        {
            get { return CoreTransaction != null ? CoreTransaction.Date : default(DateTime); }
            set { CoreTransaction.Date = value; }
        }

        [DefaultValue(Core.NormalBalance.Debit)]
        public Core.NormalBalance Nature
        {
            get { return CoreTransaction != null ? CoreTransaction.Nature : Core.NormalBalance.Debit; }
        }

        [DefaultValue(null)]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string Description
        {
            get
            {
                //Ecomania.Instance.Transaction = CoreTransaction == null ? Ecomania.Instance.Transaction : this;
                return CoreTransaction?.Description;
            }
            set
            {
                CoreTransaction.Description = value;
                //Ecomania.Instance.Transaction = this;
            }
        }

        [Editor(typeof(EcomaniaCollectionEditor), typeof(UITypeEditor))]
        //public List<CategoryName> Categories { get; } = new List<CategoryName>();
        public List<CategoryName> Categories => (from category in CoreTransaction.Categories select new CategoryName(category)).ToList();

        [Browsable(false)]
        public IEnumerable<Category> CategoryList => from category in Categories select category.Category;

        public Transaction() : this(null) { }

        public Transaction(Core.Transaction coreTransaction)
        {
            Account = Ecomania.Instance.Account;

            CoreTransaction = coreTransaction ?? Account.CoreAccount.Transactions.Add();

            Ecomania.Instance.Transaction = this;

            //foreach (var category in CoreTransaction.Categories)
            //    Categories.Add(new CategoryName(category));

            Ledger = new Ledger(this);

            Ecomania.Instance.Ledger.Add(Ledger);
        }

        public override string ToString() => Id.ToString();

        public static implicit operator Transaction(Core.Transaction coreTransaction) => new Transaction(coreTransaction);
    }
}
// {Accounter} - Copyright (C) 2017  Gravitonia AS