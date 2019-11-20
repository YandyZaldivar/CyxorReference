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
using System.ComponentModel.Design;
using System.Collections.Generic;

namespace Cyxor
{
    [DefaultProperty(nameof(Name))]
    [TypeConverter(typeof(CategoryConverter))]
    class Category
    {
        //public static Category NullCategory = new Category();

        [Browsable(false)]
        public Core.Category CoreCategory { get; set; }

        [DefaultValue(-1)]
        [Browsable(false)]
        public long Id => CoreCategory.Id;

        [DefaultValue(nameof(Category))]
        public string Name
        {
            get { return CoreCategory.Name; }
            set
            {
                CoreCategory.Name = value;

                //TODO: Move this to the property grid value changed??
                MainForm.Instance.UpdateCategoriesTreeView();
            }
        }

        [DefaultValue(Core.NormalBalance.None)]
        public Core.NormalBalance Nature
        {
            get => CoreCategory != null ? CoreCategory.Nature : Core.NormalBalance.None;
            set => CoreCategory.Nature = value;
        }

        [Browsable(false)]
        public Category Parent
        {
            get
            {
                //var id = CoreCategory.Parent != null ? CoreCategory.Parent.Id : -1;
                var id = CoreCategory?.Parent?.Id ?? -1;
                return Ecomania.Instance.Categories.SingleOrDefault(p => p.Id == id);
            }
            set { CoreCategory.Parent = value?.CoreCategory; }
        }

        [DefaultValue(null)]
        [DisplayName("Parent")]
        [TypeConverter(typeof(CategoryParentConverter))]
        public string ParentName
        {
            get { return Parent?.Name; }
            set
            {
                Parent = Ecomania.Instance.Categories.SingleOrDefault(p => p.ToString() == value);
                MainForm.Instance.UpdateCategoriesTreeView();
            }
        }

        [DefaultValue(null)]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string Description
        {
            get { return CoreCategory.Description; }
            set
            {
                CoreCategory.Description = value;
                MainForm.Instance.UpdateCategoriesTreeView();
            }
        }

        [Editor(typeof(EcomaniaCollectionEditor), typeof(UITypeEditor))]
        //public List<CategoryName> Categories { get; } = new List<CategoryName>();
        //public List<CategoryName> SubCategories => (from category in Ecomania.Instance.Categories where category.Parent == this select new CategoryName(category)).ToList();
        public List<Category> SubCategories => (from category in Ecomania.Instance.Categories where category.Parent == this select category).ToList();


        public Category() : this(null) { }

        public Category(Core.Category coreCategory)
        {
            CoreCategory = coreCategory ?? Core.Ecomania.Instance.Categories.Add();

            if (coreCategory == null)
                MainForm.Instance.UpdateCategoriesTreeView();
        }

        public override string ToString() => $"{Id} {Name}";

        public static implicit operator Category(Core.Category coreCategory) => new Category(coreCategory);
    }
}
// {Accounter} - Copyright (C) 2017  Gravitonia AS