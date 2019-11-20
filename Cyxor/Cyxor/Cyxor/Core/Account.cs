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

    public enum AccountClasification
    {
        Result,
        Real,
    }

    public enum NormalBalance
    {
        None,
        Debit,
        Credit,
    }

    public class Account
    {
        public long Id { get; private set; }
        public string Name { get; set; }
        public decimal Capital { get; set; }
        public AccountClasification Type { get; set; }
        public NormalBalance Nature { get; set; }
        public string Description { get; set; }

        [JsonRequired]
        long CategoryId { get; set; }

        [JsonIgnore]
        public Category Category
        {
            get { return Ecomania.Instance.Categories.SingleOrDefault(p => p.Id == CategoryId); }
            set { CategoryId = value != null ? value.Id : -1; }
        }

        [JsonRequired]
        internal long TransactionsId { get; set; } = -1;

        [JsonIgnore]
        public TransactionsManager Transactions { get; }

        [JsonRequired, JsonProperty(nameof(Transaction) + "s", Order = 100)]
        List<Transaction> TransactionList = new List<Transaction>();

        [JsonConstructor]
        private Account(long id, string name, decimal capital, AccountClasification type, NormalBalance nature, IEnumerable<Transaction> transactions, long categoryId, string description)
        {
            Id = id;
            Name = name;
            Type = type;
            Nature = nature;
            Capital = capital;
            CategoryId = categoryId;
            Description = description;
            TransactionList = transactions != null ? new List<Transaction>(transactions) : new List<Transaction>();
            Transactions = new TransactionsManager(this, TransactionList);
        }

        internal static Account Create(long categoryId, string name, decimal capital, AccountClasification type, NormalBalance nature, IEnumerable<Transaction> transactions, string description) =>
            new Account(++Ecomania.Instance.AccountsId, name ?? nameof(Account) + Ecomania.Instance.AccountsId, capital, type, nature, transactions, categoryId, description);

        public class TransactionsManager : CollectionManager<Account, Transaction>
        {
            internal TransactionsManager(Account account, List<Transaction> transactions)
                : base(account, transactions)
            { }

            public override Transaction Add(Transaction item = null)
            {
                if (item == null)
                    item = Transaction.Create(account: Owner,
                                              name: null,
                                              //type: TransactionType.Expenses,
                                              money: 0.00M,
                                              quantity: 1,
                                              date: DateTime.Now,
                                              categories: null,
                                              description: null);

                return base.Add(item);
            }

            public IEnumerable<Serie> Report(Category category)
            {
                var date = Ecomania.Instance.Report.Date;

                var series = new List<Serie>();

                var transactions = new List<Transaction>();

                var CreateSerie = default(Action<NormalBalance>);

                var GetSubCategoryTransactions = default(Action<Category, List<Transaction>>);

                GetSubCategoryTransactions = (rootCategory, categoryTransactions) =>
                {
                    var categories = from item in Ecomania.Instance.Categories
                                     where item.Parent == rootCategory
                                     select item;

                    foreach (var item in categories)
                    {
                        var itemTransactions = from transaction in Items
                                               where transaction.Categories.SingleOrDefault(p => p.Name == item.Name) != null
                                               select transaction;

                        categoryTransactions.AddRange(itemTransactions.Except(categoryTransactions));

                        GetSubCategoryTransactions(item, categoryTransactions);
                    }
                };

                CreateSerie = nature =>
                {
                    var name = nature.ToString();

                    var records = new List<Record>();

                    var serieTransactions = (from transaction in transactions
                                             where transaction.Nature == nature
                                             select transaction).ToList();

                    switch (Ecomania.Instance.Report.Period)
                    {
                        case ReportPeriod.Day: name += $": {date.ToString("dd/MM/yy")}"; break;
                        case ReportPeriod.Week: name += $": {nameof(ReportPeriod.Week)}({Ecomania.Instance.Calendar.GetWeekOfYear(date)})/{date.Year}"; break;
                        case ReportPeriod.Month: name += $": {Ecomania.Instance.Calendar.GetMonthName(date.Month)}/{date.Year}"; break;
                        case ReportPeriod.Year: name += $": {date.Year}"; break;
                    }

                    if (Ecomania.Instance.Report.Mode == ReportMode.Categories)
                    {
                        foreach (var item in Ecomania.Instance.Categories)
                            records.Add(new Record
                            {
                                Name = item.Name,
                                Transactions = (from transaction in serieTransactions
                                                where transaction.Categories.Contains(item)
                                                select transaction).ToList()
                            });
                    }
                    else
                    {
                        switch (Ecomania.Instance.Report.Group)
                        {
                            case ReportGroup.None:
                            {
                                foreach (var transaction in serieTransactions)
                                    records.Add(new Record
                                    {
                                        Name = transaction.Name,
                                        Transactions = new List<Transaction> { transaction }
                                    });

                                break;
                            }

                            case ReportGroup.WeekDays:
                            {
                                for (var i = DayOfWeek.Monday; i <= DayOfWeek.Saturday; i++)
                                    records.Add(new Record
                                    {
                                        Name = Ecomania.Instance.Calendar.GetDayName((DayOfWeek)i),
                                        Transactions = (from transaction in serieTransactions
                                                        where transaction.Date.DayOfWeek == (DayOfWeek)i
                                                        select transaction).ToList()
                                    });

                                records.Add(new Record
                                {
                                    Name = Ecomania.Instance.Calendar.GetDayName(DayOfWeek.Sunday),
                                    Transactions = (from transaction in serieTransactions
                                                    where transaction.Date.DayOfWeek == DayOfWeek.Sunday
                                                    select transaction).ToList()
                                });

                                break;
                            }

                            case ReportGroup.MonthDays:
                            {
                                for (int i = 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
                                    records.Add(new Record
                                    {
                                        Name = i.ToString(),
                                        Transactions = (from transaction in serieTransactions
                                                        where transaction.Date.Day == i
                                                        select transaction).ToList()
                                    });

                                break;
                            }

                            case ReportGroup.YearMonths:
                            {
                                foreach (var monthName in Ecomania.Instance.Calendar.MonthNames)
                                    if (!string.IsNullOrWhiteSpace(monthName))
                                        records.Add(new Record
                                        {
                                            Name = monthName,
                                            Transactions = (from transaction in serieTransactions
                                                            where Ecomania.Instance.Calendar.GetMonthName(transaction.Date.Month) == monthName
                                                            select transaction).ToList()
                                        });

                                break;
                            }

                            case ReportGroup.YearWeeks:
                            {
                                var transactionsDictionary = new SortedDictionary<int, List<Transaction>>();

                                foreach (var transaction in serieTransactions)
                                {
                                    var transactionWeekOfYear = Ecomania.Instance.Calendar.GetWeekOfYear(transaction.Date);

                                    if (!transactionsDictionary.ContainsKey(transactionWeekOfYear))
                                        transactionsDictionary[transactionWeekOfYear] = new List<Transaction>();

                                    transactionsDictionary[transactionWeekOfYear].Add(transaction);
                                }

                                foreach (var key in transactionsDictionary.Keys)
                                    records.Add(new Record
                                    {
                                        Name = $"{nameof(ReportPeriod.Week)}({key})",
                                        Transactions = transactionsDictionary[key]
                                    });

                                break;
                            }

                            case ReportGroup.YearsRange:
                            {
                                var startYear = date.Year - (Ecomania.Instance.Report.YearsRange / 2);

                                name += $": {startYear}-{startYear + Ecomania.Instance.Report.YearsRange}";

                                for (int i = startYear, j = 0; j < Ecomania.Instance.Report.YearsRange; i++, j++)
                                    records.Add(new Record
                                    {
                                        Name = i.ToString(),
                                        Transactions = (from transaction in serieTransactions
                                                        where transaction.Date.Year == i
                                                        select transaction).ToList()
                                    });

                                break;
                            }
                        }
                    }

                    series.Add(new Serie { Name = name, Nature = nature, Records = records });
                };

                switch (Ecomania.Instance.Report.Type)
                {
                    case ReportType.All: transactions = Items.ToList(); break;
                    case ReportType.Custom: transactions = Items.Where(p => p.Nature == NormalBalance.None).ToList(); break;
                    case ReportType.Incomes: transactions = Items.Where(p => p.Nature == NormalBalance.Credit).ToList(); break;
                    case ReportType.Expenses: transactions = Items.Where(p => p.Nature == NormalBalance.Debit).ToList(); break;
                    case ReportType.InOut: transactions = Items.Where(p => p.Nature == NormalBalance.Debit || p.Nature == NormalBalance.Credit).ToList(); break;
                }

                switch (Ecomania.Instance.Report.Period)
                {
                    case ReportPeriod.Year: transactions = transactions.Where(p => p.Date.Year == date.Year).ToList(); break;
                    case ReportPeriod.Month: transactions = transactions.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month).ToList(); break;
                    case ReportPeriod.Day: transactions = transactions.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month && p.Date.Day == date.Day).ToList(); break;
                    case ReportPeriod.Week: transactions = transactions.Where(p => p.Date.Year == date.Year && Ecomania.Instance.Calendar.GetWeekOfYear(p.Date) == Ecomania.Instance.Calendar.GetWeekOfYear(date)).ToList(); break;
                }

                switch (Ecomania.Instance.Report.Mode)
                {
                    case ReportMode.Category: transactions = transactions.Where(p => p.Categories.Contains(category)).ToList(); break;
                    case ReportMode.SubCategory: GetSubCategoryTransactions(category, transactions = transactions.Where(p => p.Categories.Contains(category)).ToList()); break;
                }

                switch (Ecomania.Instance.Report.Type)
                {
                    case ReportType.Custom: CreateSerie(NormalBalance.None); break;
                    case ReportType.Incomes: CreateSerie(NormalBalance.Credit); break;
                    case ReportType.Expenses: CreateSerie(NormalBalance.Debit); break;

                    case ReportType.InOut:
                    {
                        CreateSerie(NormalBalance.Credit);
                        CreateSerie(NormalBalance.Debit);

                        break;
                    }
                    case ReportType.All:
                    {
                        CreateSerie(NormalBalance.None);
                        CreateSerie(NormalBalance.Credit);
                        CreateSerie(NormalBalance.Debit);

                        break;
                    }
                }

                return series;
            }
        }
    }
}
// {Accounter} - Copyright (C) 2017  Gravitonia AS