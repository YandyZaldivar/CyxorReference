using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sicema
{
    public partial class SheetUserControl : UserControl
    {
        public SheetUserControl()
        {
            InitializeComponent();

            DataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

            DataGridView.ColumnAdded += DataGridView_ColumnAdded;
        }

        void DataGridView_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }
    }
}
