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
using System.Text;
using System.Linq;
//using System.Xml.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace Cyxor.Core
{
    using Newtonsoft.Json;

    public class Ecomania
    {
        public static Ecomania Instance = new Ecomania();

        public Version Version { get; set; } = new Version($"0.{DateTime.Today.ToString("yy")}.{DateTime.Today.ToString("MM")}.{DateTime.Today.ToString("dd")}");

        [JsonRequired, JsonProperty(Order = 0)]
        public DateTime LastModifiedDate { get; private set; }

        [JsonRequired, JsonProperty(Order = 1)]
        public CalendarManager Calendar { get; } = new CalendarManager();

        [JsonRequired, JsonProperty(Order = 2)]
        public ReportManager Report { get; } = new ReportManager();

        [JsonRequired, JsonProperty(Order = 3)]
        public long CategoriesId { get; internal set; } = -1;

        [JsonRequired, JsonProperty("Categories", Order = 4)]
        List<Category> CategoryList = new List<Category>();

        [JsonRequired, JsonProperty(Order = 5)]
        public long AccountsId { get; internal set; } = -1;

        [JsonRequired, JsonProperty("Accounts", Order = 6)]
        List<Account> AccountList = new List<Account>();

        [JsonRequired, JsonProperty("AccountElements", Order = 7)]
        List<string> AccountElements = new List<string>();

        [JsonIgnore]
        public CategoryManager Categories { get; }

        [JsonIgnore]
        public AccountManager Accounts { get; }

        JsonSerializerSettings JsonSettings = new JsonSerializerSettings();

        private Ecomania()
        {
            Accounts = new AccountManager(this, AccountList);
            Categories = new CategoryManager(this, CategoryList);

            JsonSettings.Formatting = Formatting.Indented;
        }

        public void Reset()
        {
            Accounts.Clear();
            Categories.Clear();

            AccountsId = -1;
            CategoriesId = -1;
        }

        void OnSerialize() { }

        void OnDeserialize() { }

        public void Load(byte[] value, Serializer serializer)
        {
            AccountsId = 0;
            CategoriesId = 0;

            Accounts.Clear();
            Categories.Clear();

            if (serializer == Serializer.Json)
                JsonConvert.PopulateObject(Encoding.UTF8.GetString(value), this, JsonSettings);
            else if (serializer == Serializer.Cyxor)
                throw new NotImplementedException();
            else
                throw new InvalidOperationException();

            OnSerialize();
        }

        public byte[] Save(Serializer serializer)
        {
            LastModifiedDate = DateTime.Now;

            var text = string.Empty;
            var value = (byte[])null;

            if (serializer == Serializer.Json)
                text = JsonConvert.SerializeObject(this, JsonSettings);
            else if (serializer == Serializer.Cyxor)
                throw new NotImplementedException();
            else
                throw new InvalidOperationException();

            OnDeserialize();

            return value ?? Encoding.UTF8.GetBytes(text);
        }

        public class ReportManager
        {
            public int YearsRange { get; set; } = 5;

            public DateTime Date { get; set; } = DateTime.Today;

            public ReportType Type { get; set; }
            public ReportMode Mode { get; set; }
            public ReportPeriod Period { get; set; }
            public ReportGroup Group { get; set; }
            public ReportTable Table { get; set; }

            public bool IsValueShownAsLabel { get; set; }

            public ReportColor BackColor { get; set; } = new ReportColor
            {
                Customs = new RGBColor { Alpha = 0, Red = 0, Green = 0, Blue = 128 },
                Incomes = new RGBColor { Alpha = 0, Red = 0, Green = 128, Blue = 0 },
                Expenses = new RGBColor { Alpha = 0, Red = 255, Green = 96, Blue = 0 }
            };

            public ReportColor ForeColor { get; set; } = new ReportColor
            {
                Customs = new RGBColor { Alpha = 0, Red = 224, Green = 224, Blue = 255 },
                Incomes = new RGBColor { Alpha = 0, Red = 224, Green = 255, Blue = 224 },
                Expenses = new RGBColor { Alpha = 0, Red = 255, Green = 224, Blue = 224 }
            };
        }

        public class CalendarManager
        {
            public DayOfWeek FirstDayOfWeek { get; set; } = DayOfWeek.Monday;
            public CalendarWeekRule WeekRule { get; set; } = CalendarWeekRule.FirstDay;

            [JsonIgnore]
            public Calendar Value => DateTimeFormatInfo.InvariantInfo.Calendar;

            [JsonIgnore]
            public IEnumerable<string> DayNames => DateTimeFormatInfo.InvariantInfo.DayNames;

            [JsonIgnore]
            public IEnumerable<string> MonthNames => DateTimeFormatInfo.InvariantInfo.MonthNames;

            public DateTime AddWeeks(DateTime date, int weeks) => Value.AddWeeks(date, weeks);
            public int GetWeekOfYear(DateTime date) => Value.GetWeekOfYear(date, WeekRule, FirstDayOfWeek);
            public string GetDayName(DayOfWeek dayOfWeek) => DateTimeFormatInfo.InvariantInfo.GetDayName(dayOfWeek);
            public string GetMonthName(int month) => DateTimeFormatInfo.InvariantInfo.GetMonthName(month);
        }

        public class AccountManager : CollectionManager<Ecomania, Account>
        {
            internal AccountManager(Ecomania ecomania, List<Account> items)
                : base(ecomania, items)
            { }

            public override Account Add(Account item = null)
            {
                if (item == null)
                    item = Account.Create(categoryId: -1, name: null, capital: 0.00M, type: AccountClasification.Result, nature: NormalBalance.Debit, transactions: null, description: null);

                return base.Add(item);
            }
        }

        public class CategoryManager : CollectionManager<Ecomania, Category>
        {
            internal CategoryManager(Ecomania ecomania, List<Category> items)
                : base(ecomania, items)
            { }

            public override Category Add(Category item = null)
            {
                if (item == null)
                    item = Category.Create(name: null, nature: NormalBalance.None, parentId: -1, description: null);

                return base.Add(item);
            }
        }
    }
}
// {Accounter} - Copyright (C) 2017  Gravitonia AS