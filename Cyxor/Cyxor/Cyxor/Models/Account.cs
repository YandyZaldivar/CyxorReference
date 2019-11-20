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
    [TypeConverter(typeof(AccountConverter))]
    class Account
    {
        [Browsable(false)]
        public Core.Account CoreAccount { get; set; }

        [Browsable(false)]
        public TrialBalance TrialBalance { get; set; }

        [DefaultValue(-1)]
        [Browsable(false)]
        public long Id => CoreAccount.Id;

        [DefaultValue(nameof(Account))]
        public string Name
        {
            get { return CoreAccount.Name; }
            set { CoreAccount.Name = value; }
        }

        [DefaultValue(typeof(decimal), "0.00")]
        public decimal Capital
        {
            get { return CoreAccount.Capital; }
            set { CoreAccount.Capital = value; }
        }

        [DefaultValue(Core.AccountClasification.Result)]
        public Core.AccountClasification Type
        {
            get { return CoreAccount != null ? CoreAccount.Type : Core.AccountClasification.Result; }
            set { CoreAccount.Type = value; }
        }

        [DefaultValue(Core.NormalBalance.Debit)]
        public Core.NormalBalance Nature
        {
            get { return CoreAccount != null ? CoreAccount.Nature : Core.NormalBalance.Debit; }
            set { CoreAccount.Nature = value; }
        }

        [DefaultValue(null)]
        public Category Category
        {
            get
            {
                return Ecomania.Instance.Categories.SingleOrDefault(p => p.CoreCategory == CoreAccount?.Category);
            }
            set
            {
                if (CoreAccount != null)
                    CoreAccount.Category = value.CoreCategory;
            }
        }

        [DefaultValue(null)]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string Description
        {
            get { return CoreAccount.Description; }
            set { CoreAccount.Description = value; }
        }

        [Browsable(false)]
        public List<Transaction> Transactions { get; } = new List<Transaction>();

        public Account() : this(null) { }

        public Account(Core.Account coreAccount)
        {
            CoreAccount = coreAccount ?? Core.Ecomania.Instance.Accounts.Add();

            Ecomania.Instance.Account = this;

            foreach (var transaction in CoreAccount.Transactions)
                Transactions.Add(transaction);

            TrialBalance = new TrialBalance(this);

            Ecomania.Instance.TrialBalance.Add(TrialBalance);
        }

        public static implicit operator Account(Core.Account coreAccount) => new Account(coreAccount);

        public override string ToString() => Name;
    }
}
// {Accounter} - Copyright (C) 2017  Gravitonia AS