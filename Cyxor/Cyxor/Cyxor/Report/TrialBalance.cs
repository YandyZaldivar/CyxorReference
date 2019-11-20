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

namespace Cyxor
{
    class TrialBalance : IComparable<TrialBalance>
    {
        [Browsable(false)]
        public Account Account { get; }

        [DisplayName(nameof(Account))]
        public string Name => Account.Name;

        public string Description => Account.Description;

        public Core.AccountClasification Type => Account.Type;

        public decimal Debit => Account.Capital + Account.Transactions.Where(p => p.Nature == Core.NormalBalance.Credit).Sum(p => p.Money * p.Quantity);
        public decimal Credit => Account.Capital + Account.Transactions.Where(p => p.Nature == Core.NormalBalance.Debit).Sum(p => p.Money * p.Quantity);

        public decimal Balance => Account.Nature == Core.NormalBalance.Credit ? Credit - Debit : Debit - Credit;

        public TrialBalance(Account account)
        {
            Account = account;
        }

        public int CompareTo(TrialBalance other)
        {
            var result = Type.CompareTo(other.Type);

            if (result == 0)
                result = Name.CompareTo(other.Name);

            return result;
        }

        //decimal GetBalance(bool begin)
        //{
        //    var total = Account.Capital;

        //    foreach (var transaction in Account.Transactions)
        //    {
        //        if (Account.Type == Core.AccountType.Receivable)
        //            total += transaction.Money * transaction.Quantity * (transaction.Type == Core.TransactionType.Expenses ? 1.00M : -1.00M);
        //        else
        //            total += transaction.Money * transaction.Quantity * (transaction.Type == Core.TransactionType.Incomes ? 1.00M : -1.00M);
        //    }

        //    return total;
        //}
    }
}
// {Accounter} - Copyright (C) 2017  Gravitonia AS