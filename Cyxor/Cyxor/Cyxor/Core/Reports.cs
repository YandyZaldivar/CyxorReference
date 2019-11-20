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
    public class Serie
    {
        public string Name { get; set; }
        //public TransactionType Type { get; set; }
        public NormalBalance Nature { get; set; }
        public IEnumerable<Record> Records { get; set; }
    }

    public class Record
    {
        public string Name { get; set; }
        public decimal Total => Transactions.Sum(p => p.Money * p.Quantity);
        public IEnumerable<Transaction> Transactions { get; set; }
    }

    public struct RGBColor
    {
        public byte Alpha { get; set; }
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
    }

    public class ReportColor
    {
        public RGBColor Incomes { get; set; }
        public RGBColor Expenses { get; set; }
        public RGBColor Customs { get; set; }
    }

    public enum ReportType
    {
        All,
        InOut,
        Expenses,
        Incomes,
        Custom,
    }

    public enum ReportMode
    {
        Transactions,
        Categories,
        Category,
        SubCategory,
    }

    public enum ReportPeriod
    {
        All,
        Day,
        Week,
        Month,
        Year,
    }

    public enum ReportGroup
    {
        None,
        WeekDays,
        MonthDays,
        YearWeeks,
        YearMonths,
        YearsRange,
    }

    public enum ReportTable
    {
        GeneralLedger,
        Subledger,
        TrialBalance,
    }
}
// {Accounter} - Copyright (C) 2017  Gravitonia AS