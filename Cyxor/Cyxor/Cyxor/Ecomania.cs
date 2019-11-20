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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace Cyxor
{
    class Ecomania
    {
        public static Ecomania Instance = new Ecomania();

        [ParenthesizePropertyName(true)]
        [Editor(typeof(EcomaniaCollectionEditor), typeof(UITypeEditor))]
        public List<Account> Accounts { get; set; } = new List<Account>();

        [ParenthesizePropertyName(true)]
        [Editor(typeof(EcomaniaCollectionEditor), typeof(UITypeEditor))]
        public List<Category> Categories { get; set; } = new List<Category>();

        [ParenthesizePropertyName(true)]
        [Editor(typeof(EcomaniaCollectionEditor), typeof(UITypeEditor))]
        public List<Transaction> Transactions { get; set; }

        [ParenthesizePropertyName(true)]
        [Editor(typeof(EcomaniaCollectionEditor), typeof(UITypeEditor))]
        List<string> AccountElements = new List<string>();

        [Browsable(false)]
        public SortedSet<Ledger> Ledger { get; } = new SortedSet<Ledger>();

        [Browsable(false)]
        public List<Ledger> Subledger => Ledger.Where(p => p.Account == Account.Name).ToList();

        [Browsable(false)]
        public SortedSet<TrialBalance> TrialBalance { get; } = new SortedSet<TrialBalance>();

        public Report Report { get; } = new Report();

        [Browsable(false)]
        public Category Category { get; set; }

        Account account;
        public Account Account
        {
            get { return account; }
            set
            {
                // TODO: Select a proper transaction when the account changes

                var expand = account == null && value != null;

                account = value;

                Transactions = account?.Transactions;

                Transaction = Transactions?.Count > 0 ? Transactions[Transactions.Count - 1] : null;

                if (expand)
                    MainForm.Instance.PropertyGrid.ExpandAllGridItems();

                //Form1.Instance.PropertyGrid.Refresh();
            }
        }

        Transaction transaction;
        public Transaction Transaction
        {
            get { return transaction; }
            set
            {
                var expand = transaction == null && value != null;

                transaction = value;

                if (expand)
                    MainForm.Instance.PropertyGrid.ExpandAllGridItems();
            }
        }

        public Config Config { get; } = new Config();

        private Ecomania()
        {

        }

        public void Populate()
        {
            Accounts.Clear();
            Categories.Clear();

            foreach (var category in Core.Ecomania.Instance.Categories)
                Categories.Add(category);

            foreach (var account in Core.Ecomania.Instance.Accounts)
                Accounts.Add(account);

            if (Accounts.Count > 0)
            {
                Account = Accounts[0];

                if (Account.Transactions.Count > 0)
                    Transaction = Account.Transactions[Account.Transactions.Count - 1];
            }
        }
    }
}
// {Accounter} - Copyright (C) 2017  Gravitonia AS