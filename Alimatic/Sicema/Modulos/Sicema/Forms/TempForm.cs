using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sicema
{
    public partial class TempForm : Form
    {
        public static TempForm Instance { get; private set; }

        public TempForm()
        {
            Instance = this;

            InitializeComponent();

            PropertyGrid.PropertyValueChanged += PropertyGrid_PropertyValueChanged;

            PropertyGrid.SelectedObject = Sicema.Instance;
            Sicema.Instance.TabControl = TabControl;

            PropertyGrid.ExpandAllGridItems();
        }

        void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //if (e.ChangedItem.Label == "Ver panel derecho")
            //    e.ChangedItem. = e.OldValue;
                //MessageBox.Show("okok");
        }
    }
}
