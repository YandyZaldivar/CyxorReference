/*
  {Ecomine} - Personal Accounting Transactions
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
using System.Drawing;
using System.ComponentModel;

namespace Cyxor
{
    [DefaultProperty(nameof(YearsRange))]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    class Config
    {
        public override string ToString() => string.Empty;

        [DefaultValue(5)]
        public int YearsRange
        {
            get { return Core.Ecomania.Instance.Report.YearsRange; }
            set { Core.Ecomania.Instance.Report.YearsRange = value; }
        }

        [DefaultValue(DayOfWeek.Monday)]
        public DayOfWeek FirstWeekDay
        {
            get { return Core.Ecomania.Instance.Calendar.FirstDayOfWeek; }
            set { Core.Ecomania.Instance.Calendar.FirstDayOfWeek = value; }
        }

        public ChartConfig Chart { get; } = new ChartConfig();

        [DisplayName(nameof(BackColor))]
        public BackColor BColor { get; } = new BackColor();

        [DisplayName(nameof(ForeColor))]
        public ForeColor FColor { get; } = new ForeColor();

        [DefaultProperty(nameof(Customs))]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class BackColor
        {
            public override string ToString() => string.Empty;

            Core.ReportColor RColor = Core.Ecomania.Instance.Report.BackColor;

            [DefaultValue("")]
            public Color Customs
            {
                get { return Color.FromArgb(RColor.Customs.Red, RColor.Customs.Green, RColor.Customs.Blue); }
                set { RColor.Customs = new Core.RGBColor { Red = value.R, Green = value.G, Blue = value.B }; }
            }

            [DefaultValue("0, 128, 0")]
            public Color Incomes
            {
                get { return Color.FromArgb(RColor.Incomes.Red, RColor.Incomes.Green, RColor.Incomes.Blue); }
                set { RColor.Incomes = new Core.RGBColor { Red = value.R, Green = value.G, Blue = value.B }; }
            }

            public Color Expenses
            {
                get { return Color.FromArgb(RColor.Expenses.Red, RColor.Expenses.Green, RColor.Expenses.Blue); }
                set { RColor.Expenses = new Core.RGBColor { Red = value.R, Green = value.G, Blue = value.B }; }
            }
        }

        [DefaultProperty(nameof(Customs))]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class ForeColor
        {
            public override string ToString() => string.Empty;

            Core.ReportColor RColor = Core.Ecomania.Instance.Report.ForeColor;

            public Color Customs
            {
                get { return Color.FromArgb(RColor.Customs.Red, RColor.Customs.Green, RColor.Customs.Blue); }
                set { RColor.Customs = new Core.RGBColor { Red = value.R, Green = value.G, Blue = value.B }; }
            }

            public Color Incomes
            {
                get { return Color.FromArgb(RColor.Incomes.Red, RColor.Incomes.Green, RColor.Incomes.Blue); }
                set { RColor.Incomes = new Core.RGBColor { Red = value.R, Green = value.G, Blue = value.B }; }
            }

            public Color Expenses
            {
                get { return Color.FromArgb(RColor.Expenses.Red, RColor.Expenses.Green, RColor.Expenses.Blue); }
                set { RColor.Expenses = new Core.RGBColor { Red = value.R, Green = value.G, Blue = value.B }; }
            }
        }

        [DefaultProperty(nameof(IsValueShownAsLabel))]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class ChartConfig
        {
            public override string ToString() => string.Empty;

            Core.Ecomania.ReportManager Report = Core.Ecomania.Instance.Report;

            [DefaultValue(false)]
            public bool IsValueShownAsLabel
            {
                get { return Report.IsValueShownAsLabel; }
                set { Report.IsValueShownAsLabel = value; }
            }
        }
    }
}
// {Ecomine} - Copyright (C) 2017  Gravitonia AS