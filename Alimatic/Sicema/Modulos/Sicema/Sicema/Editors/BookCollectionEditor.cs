using System;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;

namespace Sicema
{
    public class BookCollectionEditor : CollectionEditor
    {
        List<Book> Books;
        Label ErrorLabel;
        CollectionForm Form;
        PropertyGrid PropertyGrid;

        public BookCollectionEditor(Type type) : base(type)
        {

        }

        protected override CollectionForm CreateCollectionForm()
        {
            Form = base.CreateCollectionForm();
            Form.Text = "Editor de libros";
            PropertyGrid = Form.Controls[0].Controls[5] as PropertyGrid;
            Form.FormClosed += CollectionForm_FormClosed;
            Form.FormClosing += Form_FormClosing;

            PropertyGrid.PropertyValueChanged += PropertyGrid_PropertyValueChanged;

            ErrorLabel = new Label
            {
                AutoSize = true,
                ForeColor = Color.Red,
                Left = Form.Controls[0].Left,
                Top = Form.Controls[0].Height - 10,
                Font = Form.Controls[0].Controls[2].Font
            };

            Form.Controls.Add(ErrorLabel);
            ErrorLabel.BringToFront();

            return Form;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Form.DialogResult == DialogResult.OK)
                foreach (var book in Books)
                    if (Books.Where(p => p != book).Any(p => p.Name == book.Name))
                    {
                        MessageBox.Show($"Cambios cancelados porque el libro '{book.Name}' está duplicado");
                        CancelChanges();
                    }
        }

        protected override object CreateInstance(Type itemType) => base.CreateInstance(itemType);

        private void CollectionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Sicema.Instance.Update();
        }

        protected override object SetItems(object editValue, object[] value)
        {
            Books = base.SetItems(editValue, value) as List<Book>;

            CheckBookDuplication();

            return Books;
        }

        private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
            //=> e.ChangedItem.
            => CheckBookDuplication();

        bool CheckBookDuplication()
        {
            //this.des

            foreach (var book in Books)
                if (Books.Where(p => p != book).Any(p => p.Name == book.Name))
                {
                    ErrorLabel.Text = $"El libro '{book.Name}' está duplicado";
                    (Form.AcceptButton as Button).Enabled = false;
                    return true;
                }

            (Form.AcceptButton as Button).Enabled = true;

            return false;
        }
    }
}
