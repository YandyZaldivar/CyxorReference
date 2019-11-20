using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Sicema
{
    class Sicema
    {
        public static Sicema Instance = new Sicema();

        public int Id { get; set; }
        public string Name { get; set; }

        [Browsable(false)]
        public TabControl TabControl { get; set; }

        const bool DefaultShowRightPanel = true;
        bool showRightPanel = DefaultShowRightPanel;
        [DefaultValue(true)]
        [DisplayName("Ver panel derecho")]
        public bool ShowRightPanel
        {
            get => showRightPanel;
            set
            {
                showRightPanel = value;
                TempForm.Instance.RightSplitContainer.Panel2Collapsed = !showRightPanel;
            }
        }

        const bool DefaultShowBottomPanel = true;
        bool showBottomtPanel = DefaultShowBottomPanel;
        [DefaultValue(true)]
        [DisplayName("Ver panel inferior")]
        public bool ShowBottomPanel
        {
            get => showBottomtPanel;
            set
            {
                showBottomtPanel = value;
                TempForm.Instance.BottomSplitContainer.Panel2Collapsed = !showBottomtPanel;
            }
        }

        readonly List<Book> books = new List<Book>();
        [DisplayName("Libros")]
        [Editor(typeof(BookCollectionEditor), typeof(UITypeEditor))]
        public List<Book> Books => books;

        public void Update()
        {
            foreach (var book in Books)
            {
                if (book.TabPage == null)
                {
                    book.TabPage = new TabPage(book.Name) { Name = book.Name };
                    TabControl.TabPages.Add(book.TabPage);

                    book.BookUserControl = new BookUserControl();
                    book.BookUserControl.Dock = DockStyle.Fill;
                    book.TabPage.Controls.Add(book.BookUserControl);
                }

                book.TabPage.Name = book.Name;
                book.TabPage.Text = $"{book.Name}{book.Text}";

                foreach (var sheet in book.Sheets)
                {
                    if (sheet.TabPage == null)
                    {
                        sheet.TabPage = new TabPage(sheet.Name) { Name = sheet.Name };
                        book.BookUserControl.TabControl.TabPages.Add(sheet.TabPage);

                        sheet.SheetUserControl = new SheetUserControl();
                        sheet.SheetUserControl.Dock = DockStyle.Fill;
                        sheet.TabPage.Controls.Add(sheet.SheetUserControl);
                    }

                    sheet.TabPage.Name = sheet.Name;
                    sheet.TabPage.Text = $"{sheet.Name}{sheet.Text}";
                }

                foreach (TabPage tabPage in book.BookUserControl.TabControl.TabPages)
                {
                    var found = false;

                    foreach (var sheet in book.Sheets)
                        if (sheet.TabPage == tabPage)
                            found = true;

                    if (!found)
                        book.BookUserControl.TabControl.TabPages.Remove(tabPage);
                }
            }

            foreach (TabPage tabPage in TabControl.TabPages)
            {
                var found = false;

                foreach (var book in Books)
                    if (book.TabPage == tabPage)
                        found = true;

                if (!found)
                    TabControl.TabPages.Remove(tabPage);
            }
        }
    }
}
