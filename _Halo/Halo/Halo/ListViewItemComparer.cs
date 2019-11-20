using System;
using System.Collections;
using System.Windows.Forms;

namespace Halo
{
    using Models;

    class ListViewItemComparer : IComparer
    {
        readonly int Column;
        readonly SortOrder SortOrder;
        readonly ToolStripMenuItem CheckedToolStripMenuItem;

        public ListViewItemComparer(SortOrder sortOrder, int column, ToolStripMenuItem checkedToolStripMenuItem)
        {
            Column = column;
            SortOrder = sortOrder;
            CheckedToolStripMenuItem = checkedToolStripMenuItem;
        }

        public int Compare(object x, object y)
        {
            var v1 = default(string);
            var v2 = default(string);

            var i1 = x as ListViewItem;
            var i2 = y as ListViewItem;

            var p1 = i1.Tag as PacienteApiModel;
            var p2 = i2.Tag as PacienteApiModel;

            v1 = i1.SubItems[Column].Text;
            v2 = i2.SubItems[Column].Text;

            if (Column == 1)
            {
                if (CheckedToolStripMenuItem.Name == nameof(MainForm.HospitalProvinciaToolStripMenuItem))
                {
                    v1 = (p1.HospitalProvincia?.Id ?? 0).ToString();
                    v2 = (p2.HospitalProvincia?.Id ?? 0).ToString();
                }
            }
            else if (Column == 2)
            {
                if (SortOrder == SortOrder.Descending)
                    return DateTime.Compare(p2.FechaIngreso.Value, p1.FechaIngreso.Value);

                return DateTime.Compare(p1.FechaIngreso.Value, p2.FechaIngreso.Value);
            }

            var numeric = decimal.TryParse(v1, out var r1);
            numeric &= decimal.TryParse(v2, out var r2);

            if (SortOrder == SortOrder.Descending)
            {
                if (numeric)
                    return decimal.Compare(r2, r1);

                return string.Compare(v2, v1);
            }

            if (numeric)
                return decimal.Compare(r1, r2);

            return string.Compare(v1, v2);
        }
    }
}
