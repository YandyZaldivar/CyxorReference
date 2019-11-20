using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace Sicema
{
    [DisplayName("Hoja")]
    class Sheet
    {
        [DisplayName("Nombre")]
        public string Name { get; set; }

        [DisplayName("Texto")]
        public string Text { get; set; }

        [Browsable(false)]
        public TabPage TabPage { get; set; }

        [Browsable(false)]
        public SheetUserControl SheetUserControl { get; set; }

        public Sheet()
        {
            Name = $"H{(uint)Guid.NewGuid().ToString().GetHashCode()}";
        }

        public override string ToString() => Name;
    }
}
