using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Sicema
{
    public class SheetCollectionEditor : CollectionEditor
    {
        List<Book> Books;
        Label ErrorLabel;
        CollectionForm Form;
        PropertyGrid PropertyGrid;

        public SheetCollectionEditor(Type type) : base(type)
        {

        }

        protected override CollectionForm CreateCollectionForm()
        {
            Form = base.CreateCollectionForm();
            Form.Text = "Editor de libros";
            PropertyGrid = Form.Controls[0].Controls[5] as PropertyGrid;
            Form.FormClosed += Form_FormClosed;

            //PropertyGrid.PropertyValueChanged += PropertyGrid_PropertyValueChanged;

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

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
            => Sicema.Instance.Update();
    }
}
