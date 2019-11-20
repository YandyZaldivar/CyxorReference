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
using System.Globalization;

namespace Cyxor
{
    public class CategoryConverter : ExpandableObjectConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            => sourceType == typeof(string);

        //public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        //    => Ecomania.Instance.Account = Ecomania.Instance.Accounts.Single(p => p.Name == value as string);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            => Ecomania.Instance.Categories.Single(p => p.ToString() == value as string);

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            => new StandardValuesCollection(Ecomania.Instance.Categories);
    }
}
// {Accounter} - Copyright (C) 2017  Gravitonia AS