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
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel.Design;

namespace Cyxor
{
    class EcomaniaCollectionEditor : CollectionEditor
    {
        Type ItemType;
        Label ErrorLabel;

        public EcomaniaCollectionEditor(Type type) : base(type)
        {
            ItemType = type.GetGenericArguments()[0];
        }

        protected override bool CanSelectMultipleInstances() => false;

        protected override CollectionForm CreateCollectionForm()
        {
            var collectionForm = base.CreateCollectionForm();

            collectionForm.StartPosition = FormStartPosition.CenterParent;

            var listBox = collectionForm.Controls[0].Controls[4] as ListBox;
            var propertyGrid = collectionForm.Controls[0].Controls[5] as PropertyGrid;

            propertyGrid.ToolbarVisible = false;
            propertyGrid.Font = new Font("Segoe UI", 9);
            propertyGrid.PropertySort = PropertySort.NoSort;
            propertyGrid.PropertyValueChanged += (s, e) => MainForm.Instance.Update(fullUpdate: false);

            var addButton = collectionForm.Controls[0].Controls[1].Controls[0] as Button;
            var delButton = collectionForm.Controls[0].Controls[1].Controls[1] as Button;

            ErrorLabel = new Label
            {
                AutoSize = true,
                ForeColor = Color.Red,
                Left = collectionForm.Controls[0].Left,
                Top = collectionForm.Controls[0].Height - 10,
                Font = collectionForm.Controls[0].Controls[2].Font
            };

            collectionForm.Controls.Add(ErrorLabel);
            ErrorLabel.BringToFront();

            var CheckCanAddItems = new Action(() =>
            {
                ErrorLabel.Text = null;

                if (ItemType == typeof(CategoryName))
                {
                    var categories = from item in listBox.Items.OfType<object>()
                                     join category in Ecomania.Instance.Categories
                                     on item.ToString() equals category.Name
                                     select category;

                    var categoriesExcept = Ecomania.Instance.Categories.Except(categories, CategoryComparer.Instance);

                    addButton.Visible = categoriesExcept.Count() > 0;

                    if (!addButton.Visible)
                        ErrorLabel.Text = "No more categories available";
                }
                else if (ItemType == typeof(Transaction))
                {
                    addButton.Visible = Ecomania.Instance.Account != null;

                    if (!addButton.Visible)
                        ErrorLabel.Text = "You must add and select an account before adding transactions";
                }
                else
                    addButton.Visible = true;
            });

            listBox.SelectedIndexChanged += (sender, e) =>
            {
                var item = listBox.SelectedItem;

                if (item == null)
                    return;

                if (ItemType == typeof(Account))
                    Ecomania.Instance.Account = item.GetType().GetProperty("Value").GetValue(item, default(object[])) as Account;
                else if (ItemType == typeof(Transaction))
                    Ecomania.Instance.Transaction = item.GetType().GetProperty("Value").GetValue(item, default(object[])) as Transaction;

                MainForm.Instance.PropertyGrid.Refresh();
            };

            collectionForm.Shown += (sender, e) => CheckCanAddItems();

            addButton.Click += (sender, e) => CheckCanAddItems();
            delButton.Click += (sender, e) => CheckCanAddItems();

            var acceptButton = collectionForm.AcceptButton as Button;
            var cancelButton = collectionForm.CancelButton as Button;

            var acceptPanel = new Panel { Size = acceptButton.Size };
            var cancelPanel = new Panel { Size = cancelButton.Size };

            var parentControl = acceptButton.Parent as TableLayoutPanel;

            parentControl.Controls.Remove(cancelButton);
            parentControl.Controls.Remove(acceptButton);
            parentControl.Controls.Add(cancelPanel);
            parentControl.Controls.Add(acceptPanel);
            acceptPanel.Controls.Add(acceptButton);
            acceptButton.Dock = DockStyle.Fill;

            collectionForm.FormClosing += (sender, e) => acceptButton.PerformClick();

            return collectionForm;
        }

        protected override object CreateInstance(Type itemType)
        {
            MainForm.Instance.Update(fullUpdate: true);
            return base.CreateInstance(itemType);
        }

        protected override bool CanRemoveInstance(object value)
        {
            ErrorLabel.Text = null;

            switch (value.GetType().Name)
            {
                case nameof(Category):
                {
                    var category = value as Category;

                    foreach (var item in Ecomania.Instance.Categories)
                        if (item.Parent == category)
                        {
                            ErrorLabel.Text = "The category has subcategories, you must remove them all first";
                            return false;
                        }

                    return true;
                }
            }

            return base.CanRemoveInstance(value);
        }

        protected override void DestroyInstance(object instance)
        {
            switch (instance.GetType().Name)
            {
                case nameof(Account):
                {
                    var account = instance as Account;

                    Ecomania.Instance.Accounts.Remove(account);
                    Core.Ecomania.Instance.Accounts.Remove(account.CoreAccount);

                    if (Ecomania.Instance.Account == account)
                    {
                        Ecomania.Instance.Account = Ecomania.Instance.Accounts.FirstOrDefault();

                        if (Ecomania.Instance.Account == null)
                            MainForm.Instance.PropertyGrid.Refresh();
                    }

                    break;
                }

                case nameof(Category):
                {
                    var category = instance as Category;

                    foreach (var account in Ecomania.Instance.Accounts)
                        foreach (var transaction in account.Transactions)
                            foreach (var categoryName in transaction.Categories)
                                if (categoryName.Category.ToString() == category.ToString())
                                {
                                    transaction.CoreTransaction.Categories.Remove(category.CoreCategory);
                                    break;
                                }

                    Ecomania.Instance.Categories.Remove(category);
                    Core.Ecomania.Instance.Categories.Remove(category.CoreCategory);

                    break;
                }

                case nameof(Transaction):
                {
                    var transaction = instance as Transaction;

                    Ecomania.Instance.Transactions.Remove(transaction);
                    Ecomania.Instance.Account.CoreAccount.Transactions.Remove(transaction.CoreTransaction);

                    if (Ecomania.Instance.Transaction == transaction)
                    {
                        Ecomania.Instance.Transaction = transaction.Account.Transactions.FirstOrDefault(p => p != transaction);

                        if (Ecomania.Instance.Transaction == null)
                            MainForm.Instance.PropertyGrid.Refresh();
                    }

                    break;
                }

                case nameof(CategoryName):
                {
                    var categoryName = instance as CategoryName;

                    Ecomania.Instance.Transaction.Categories.Remove(categoryName);
                    categoryName.Transaction.CoreTransaction.Categories.Remove(categoryName.Category.CoreCategory);

                    break;
                }
            }

            base.DestroyInstance(instance);
            MainForm.Instance.Update(fullUpdate: true);
        }
    }
}
// {Accounter} - Copyright (C) 2017  Gravitonia AS