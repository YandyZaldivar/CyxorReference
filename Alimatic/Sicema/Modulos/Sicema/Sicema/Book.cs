using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Sicema
{
    [DisplayName("Libro")]
    class Book : IEqualityComparer<Book>
    {
        [DisplayName("Nombre")]
        public string Name { get; set; }

        [DisplayName("Texto")]
        public string Text { get; set; }

        [Browsable(false)]
        public TabPage TabPage { get; set; }

        [Browsable(false)]
        public BookUserControl BookUserControl { get; set; }

        [DisplayName("Hojas")]
        [Editor(typeof(SheetCollectionEditor), typeof(UITypeEditor))]
        public List<Sheet> Sheets { get; } = new List<Sheet>();

        public Book()
        {
            Name = $"L{(uint)Guid.NewGuid().ToString().GetHashCode()}";
        }

        public override string ToString() => Name;

        public bool Equals(Book x, Book y) => x.Name == y.Name;

        public int GetHashCode(Book obj) => obj.Name.GetHashCode();

        public void Update()
        {
            if (BookUserControl == null)
                BookUserControl = new BookUserControl();
        }
    }
}
