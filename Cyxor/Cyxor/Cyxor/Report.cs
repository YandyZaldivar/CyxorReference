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
using System.ComponentModel;

namespace Cyxor
{
    [DefaultProperty(nameof(Type))]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    class Report
    {
        //[Browsable(false)]
        public DateTime Date
        {
            get { return Core.Ecomania.Instance.Report.Date; }
            set { Core.Ecomania.Instance.Report.Date = value; Update(); }
        }

        [DefaultValue(Core.ReportType.All)]
        public Core.ReportType Type
        {
            get { return Core.Ecomania.Instance.Report.Type; }
            set { Core.Ecomania.Instance.Report.Type = value; Update(); }
        }

        [DefaultValue(Core.ReportMode.Transactions)]
        public Core.ReportMode Mode
        {
            get { return Core.Ecomania.Instance.Report.Mode; }
            set { Core.Ecomania.Instance.Report.Mode = value; Update(); }
        }

        [DefaultValue(Core.ReportPeriod.All)]
        public Core.ReportPeriod Period
        {
            get { return Core.Ecomania.Instance.Report.Period; }
            set { Core.Ecomania.Instance.Report.Period = value; Update(); }
        }

        [DefaultValue(Core.ReportGroup.None)]
        public Core.ReportGroup Group
        {
            get { return Core.Ecomania.Instance.Report.Group; }
            set { Core.Ecomania.Instance.Report.Group = value; Update(); }
        }

        [DefaultValue(Core.ReportTable.GeneralLedger)]
        public Core.ReportTable Table
        {
            get { return Core.Ecomania.Instance.Report.Table; }
            set { Core.Ecomania.Instance.Report.Table = value; Update(); }
        }

        public override string ToString() => $"{Type.ToString()} + {Mode.ToString()}";

        public void Update()
        {
            //Form1.Instance.Report();
            //Form1.Instance.ReportButton_DropDownItemClicked(null, null);
        }
    }
}
// {Accounter} - Copyright (C) 2017  Gravitonia AS