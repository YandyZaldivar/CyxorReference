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

namespace Cyxor.Core
{
    using Newtonsoft.Json;

    public class Category
    {
        public long Id { get; private set; }

        public string Name { get; set; }

        public string Description { get; set; }

        NormalBalance nature;
        public NormalBalance Nature
        {
            get
            {
                if (nature != NormalBalance.None)
                    return nature;

                if (Parent == null)
                    return Account?.Nature ?? nature;

                return Parent.Nature;
            }
            set => nature = value;
        }

        [JsonRequired]
        long ParentId { get; set; }

        [JsonIgnore]
        public Category Parent
        {
            get { return Ecomania.Instance.Categories.SingleOrDefault(p => p.Id == ParentId); }
            set { ParentId = value != null ? value.Id : -1; }
        }

        [JsonIgnore]
        public Category Root
        {
            get
            {
                var parent = Parent;

                while (parent?.Parent != null)
                    parent = parent.Parent;

                return parent ?? this;
            }
        }

        [JsonIgnore]
        public Account Account => Ecomania.Instance.Accounts.SingleOrDefault(p => p.Category == Root);

        [JsonConstructor]
        private Category(long id, string name, NormalBalance nature, long parentId, string description)
        {
            Id = id;
            Name = name;
            Nature = nature;
            ParentId = parentId;
            Description = description;
        }

        internal static Category Create(string name, NormalBalance nature, long parentId, string description) =>
            new Category(++Ecomania.Instance.CategoriesId, name ?? nameof(Category) + Ecomania.Instance.CategoriesId, nature, parentId, description);
    }
}
// {Accounter} - Copyright (C) 2017  Gravitonia AS