using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Reflection;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Cyxor
{
    public partial class MainForm : Form
    {
        public static MainForm Instance;

        Core.Serializer Serializer = Core.Serializer.Json;
        string DataFilePath { get; } = "CyxorDB";

        public bool UnsavedChanges { get; set; }

        public MainForm()
        {
            InitializeComponent();

            Instance = this;

            WindowState = FormWindowState.Maximized;
            Text = "Ecomine: Personal Accounting Transactions";

            Core.Ecomania.Instance.Reset();

            switch (Serializer)
            {
                case Core.Serializer.Json: DataFilePath += ".json"; break;
                case Core.Serializer.Cyxor: DataFilePath += ".cyxor"; break;

                default: throw new InvalidOperationException();
            }

            ListView.FullRowSelect = true;
            ListView.ShowItemToolTips = true;

            ListView.ItemSelectionChanged += (sender, e) =>
            {
                if (e.IsSelected)
                {
                    Ecomania.Instance.Transaction = e.Item.Tag as Transaction;
                    PropertyGrid.Refresh();
                }
            };

            //MonthCalendar.TodayDate = DateTime.Today;
            //MonthCalendarPanel.BorderStyle = BorderStyle.Fixed3D;

            TreeView.ShowNodeToolTips = true;

            TreeView.AfterSelect += TreeView_AfterSelect;

            //HelpTipButton.Click += HelpTipButton_Click;
            //TransactionsAddButton.Click += TransactionsAddButton_Click;

            PropertyGrid.PropertyValueChanged += PropertyGrid_PropertyValueChanged;

            Shown += Form1_Shown;

            SaveToolStripButton.Click += (s, e) => SaveEcomania();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //var monthCalendarXOffset = (MonthCalendar.Parent.Width - MonthCalendar.Width) / 2;
            //var monthCalendarYOffset = (MonthCalendar.Parent.Height - MonthCalendar.Height) / 2;

            //MonthCalendar.Location = new Point(monthCalendarXOffset, monthCalendarYOffset);

            //Text = $"Monitor {{Name = {Screen.PrimaryScreen.DeviceName}, BitsPerPixel = {Screen.PrimaryScreen.BitsPerPixel}, Size = {Screen.PrimaryScreen.Bounds.Size}}} - Ecomania: {{ Size = {Size}}}";
        }

        private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            var fullUpdate = false;

            if (e.ChangedItem.Parent.Label == nameof(Cyxor.Report))
                if (e.ChangedItem.Label == nameof(Cyxor.Report.Table))
                    fullUpdate = true;

            Update(fullUpdate);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ToolTip.ToolTipTitle = "lariralala";
            ToolTip.Show("Esto es una prueba loca.", this, 10, 30, 4000);
            //PropertyGrid.SelectedGridItem.
            GridItem df;
        }

        public static GridItemCollection GetAllGridEntries(PropertyGrid grid)
        {
            if (grid == null)
                throw new ArgumentNullException("grid");

            object view = grid.GetType().GetField("gridView", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(grid);
            return (GridItemCollection)view.GetType().InvokeMember("GetAllGridEntries", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, view, null);
        }

        private void HelpTipButton_Click(object sender, EventArgs e)
        {
            var dd = GetAllGridEntries(PropertyGrid);
            var gy = dd["Capital"];



            ToolTip.ToolTipTitle = "mekans44";
            ToolTip.IsBalloon = false;
            ToolTip.Show("New transaction created.", this, 100, 315, 2700);
        }

        private void TransactionsAddButton_Click(object sender, EventArgs e)
        {
            //TransactionsAddButton.Enabled = false;
            var transaction = new Transaction();
            Ecomania.Instance.Account.Transactions.Add(transaction);
            PropertyGrid.Refresh();
            ToolTip.ToolTipTitle = transaction.Name;
            ToolTip.Show("New transaction created.", this, 100, 315, 2700);

            var timer = new Timer { Interval = 3000 };

            timer.Tick += (timerSender, timerE) =>
            {
                //TransactionsAddButton.Enabled = true;
                timer.Stop();
            };

            timer.Start();
        }

        bool SaveEcomania()
        {
            var bytes = Core.Ecomania.Instance.Save(Serializer);

            try
            {
                File.WriteAllBytes(DataFilePath, bytes);
                SaveToolStripButton.Enabled = false;
            }
            catch { return false; }

            return true;
        }

        bool LoadEcomania()
        {
            var bytes = (byte[])null;
            var result = true;

            try
            {
                if (File.Exists(DataFilePath))
                {
                    bytes = File.ReadAllBytes(DataFilePath);
                    Core.Ecomania.Instance.Load(bytes, Serializer);
                    //SaveToolStripButton.Enabled = false;
                }
                else
                    SaveEcomania();
            }
            catch (Exception exc)
            {
                result = false;
            }

            Ecomania.Instance.Populate();

            //MonthCalendar.FirstDayOfWeek = (Day)Enum.Parse(typeof(Day), Core.Ecomania.Instance.Calendar.FirstDayOfWeek.ToString());

            PropertyGrid.SelectedObject = Ecomania.Instance;
            PropertyGrid.ExpandAllGridItems();

            UpdateCategoriesTreeView();

            Report();

            return result;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!LoadEcomania())
            {
                var message = $"Can't read or create the file \"{DataFilePath}\", " +
                    "verify you have write access to the location or the file is not in use. \n" +
                    "This application will quit.";

                MessageBox.Show(message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Update(fullUpdate: true);
            SaveToolStripButton.Enabled = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!SaveEcomania())
            {
                var message = $"Can't save data to \"{DataFilePath}\", " +
                    "verify you have write access to the location. \n" +
                    "If you exit the application you may loose data. " +
                    "Do you want to quit the application anyway?";

                var dialogResult = MessageBox.Show(message, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                if (dialogResult == DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        public void Report()
        {
            var UpdateListView = new Action<Core.NormalBalance, IEnumerable<Transaction>, ListViewGroup>((nature, transactions, group) =>
            //internal void UpdateTransactionsListView(Core.TransactionType type, IEnumerable<Transaction> transactions, ListViewGroup group = null)
            {
                var count = transactions != null ? transactions.Count() : 0;
                ListView.Columns[0].Text = $"Id({ListView.Items.Count + count})";

                var backColor = Core.Ecomania.Instance.Report.BackColor.Customs;
                var foreColor = Core.Ecomania.Instance.Report.ForeColor.Customs;

                if (nature == Core.NormalBalance.Credit)
                {
                    backColor = Core.Ecomania.Instance.Report.BackColor.Incomes;
                    foreColor = Core.Ecomania.Instance.Report.ForeColor.Incomes;
                }
                else if (nature == Core.NormalBalance.Debit)
                {
                    backColor = Core.Ecomania.Instance.Report.BackColor.Expenses;
                    foreColor = Core.Ecomania.Instance.Report.ForeColor.Expenses;
                }

                var AddItem = new Func<string, Transaction, ListViewItem>((name, transaction) =>
                {
                    var item = ListView.Items.Add(transaction?.Id.ToString() ?? "-Empty-");
                    item.Tag = transaction;
                    item.Group = group;
                    item.Font = new Font("Consolas", 9, FontStyle.Bold);
                    item.BackColor = Color.FromArgb(backColor.Red, backColor.Green, backColor.Blue);
                    item.ForeColor = Color.FromArgb(foreColor.Red, foreColor.Green, foreColor.Blue);

                    if (transaction == null)
                    {
                        item.UseItemStyleForSubItems = false;

                        var subItem = item.SubItems.Add(string.Empty);
                        subItem.BackColor = Color.FromArgb(0, 255, 255, 255);

                        subItem = item.SubItems.Add(string.Empty);
                        subItem.BackColor = Color.FromArgb(0, 255, 255, 255);
                    }

                    return item;
                });

                if (count == 0)
                    AddItem("", null);

                foreach (var transaction in transactions)
                {
                    var item = AddItem("", transaction);

                    var delimitedCategoriesToolTip = string.Empty;

                    if (transaction.Categories.Count == 0)
                        delimitedCategoriesToolTip = "No categories";
                    else
                        delimitedCategoriesToolTip = transaction.Categories[0].Name;

                    for (int i = 1; i < transaction.Categories.Count; i++)
                        delimitedCategoriesToolTip += $", {transaction.Categories[i].Name}";

                    item.ToolTipText = $"{transaction.Name} {Environment.NewLine} {Environment.NewLine}" +
                                       $"       Type: {transaction.Nature} {Environment.NewLine}" +
                                       $"       Date: {transaction.Date} {Environment.NewLine}" +
                                       $" Categories: [{delimitedCategoriesToolTip}] {Environment.NewLine}" +
                                       $"Description: {transaction.Description}";

                    item.SubItems.Add(transaction.Money.ToString());
                    item.SubItems.Add(transaction.Quantity.ToString());
                }
            });

            ListView.Visible = false;
            ListView.SuspendLayout();
            ListView.Items.Clear();
            ListView.Groups.Clear();

            Chart.Series.Clear();
            Chart.Legends.Clear();
            Chart.ChartAreas.Clear();

            var chartArea = Chart.ChartAreas.Add("MainArea");
            chartArea.AxisX.Interval = 1;
            //chartArea.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Days;

            if (Ecomania.Instance.Account == null)
                return;

            var category = Ecomania.Instance.Categories.SingleOrDefault(p => p == TreeView.SelectedNode.Tag);
            var transactionsReport = Ecomania.Instance.Account.CoreAccount.Transactions.Report(category?.CoreCategory);

            var customs = 0.00M;
            var incomes = 0.00M;
            var expenses = 0.00M;

            foreach (var serie in transactionsReport)
            {
                var chartSerie = Chart.Series.Add($"[{serie.Name}]");
                chartSerie.IsValueShownAsLabel = Core.Ecomania.Instance.Report.IsValueShownAsLabel;

                switch (serie.Nature)
                {
                    case Core.NormalBalance.None: chartSerie.Color = Ecomania.Instance.Config.BColor.Customs; break;
                    case Core.NormalBalance.Credit: chartSerie.Color = Ecomania.Instance.Config.BColor.Incomes; break;
                    case Core.NormalBalance.Debit: chartSerie.Color = Ecomania.Instance.Config.BColor.Expenses; break;
                }

                if (serie.Records.Count() == 0)
                {
                    var group = new ListViewGroup($"[{serie.Name}]");
                    ListView.Groups.Add(group);
                    UpdateListView(serie.Nature, new List<Transaction>(), group);
                }

                foreach (var record in serie.Records)
                {
                    var dataPoint = chartSerie.Points.Add((double)Math.Round(record.Total, 2));
                    dataPoint.AxisLabel = record.Name;

                    var subtotal = record.Transactions.Sum(p => Math.Round(p.Money * p.Quantity * 1.00M, 2));
                    var group = new ListViewGroup($"[{serie.Name}]: {record.Name}  ({subtotal})");
                    ListView.Groups.Add(group);

                    var transactions = from coreTransaction in record.Transactions
                                       join transaction in Ecomania.Instance.Account.Transactions
                                       on coreTransaction.Id equals transaction.Id
                                       select transaction;

                    switch (serie.Nature)
                    {
                        case Core.NormalBalance.None: customs += record.Total; break;
                        case Core.NormalBalance.Credit: incomes += record.Total; break;
                        case Core.NormalBalance.Debit: expenses += record.Total; break;
                    }

                    UpdateListView(serie.Nature, transactions, group);
                }
            }

            UpdateReportChart();

            UpdateStatusBar(incomes, expenses);

            //ToolStripStatusLabel3.Text = $"Incomes: {incomes} - Expenses: {expenses} = {incomes - expenses}";

            //var balance = Ecomania.Instance.Account.Balance;
            //incomes = Ecomania.Instance.Account.Transactions.Where(p => p.Type == Core.TransactionType.Incomes).Sum(p => p.Money * p.Quantity);
            //expenses = Ecomania.Instance.Account.Transactions.Where(p => p.Type == Core.TransactionType.Expenses).Sum(p => p.Money * p.Quantity);

            //ToolStripStatusLabel2.Text = $"Account: {balance} + Incomes: {incomes} - Expenses: {expenses} = {(balance + incomes) - expenses}";

            ListView.ResumeLayout(performLayout: false);
            ListView.PerformLayout();
            ListView.Visible = true;
        }

        void UpdateStatusBar(decimal incomes, decimal expenses)
        {
            RightBottomLabel.Text = $"Incomes: {incomes} - Expenses: {expenses} = {incomes - expenses}";

            var balance = Ecomania.Instance.Account.Capital;
            incomes = Ecomania.Instance.Account.Transactions.Where(p => p.Nature == Core.NormalBalance.Credit).Sum(p => p.Money * p.Quantity);
            expenses = Ecomania.Instance.Account.Transactions.Where(p => p.Nature == Core.NormalBalance.Debit).Sum(p => p.Money * p.Quantity);

            MiddleBottomLabel.Text = $"Account: {balance} + Incomes: {incomes} - Expenses: {expenses} = {(balance + incomes) - expenses}";
        }

        void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == TreeView.Nodes[0])
                Ecomania.Instance.Category = null;

            else if (e.Node.Tag != Ecomania.Instance.Category)
            {
                Ecomania.Instance.Category = Ecomania.Instance.Categories.Single(p => p == e.Node.Tag);

                var reportMode = Core.Ecomania.Instance.Report.Mode;

                if (reportMode == Core.ReportMode.Category || reportMode == Core.ReportMode.SubCategory)
                    Report();
            }
        }

        public void UpdateCategoriesTreeView()
        {
            TreeView.Nodes.Clear();

            var topNode = TreeView.Nodes.Add(nameof(Ecomania.Instance.Categories));
            topNode.ToolTipText = "When selected if the ReportMode is set to view by categories " +
                "all categorized transactions will be displayed.";

            var nodes = new List<TreeNode>(Ecomania.Instance.Categories.Count);
            var categories = new List<Category>(Ecomania.Instance.Categories);

            while (categories.Count > 0)
                foreach (var category in categories)
                {
                    var node = (TreeNode)null;

                    if (category.Parent == null)
                        node = topNode;
                    else
                        node = nodes.SingleOrDefault(p => p.Tag == category.Parent);

                    if (node != null)
                    {
                        nodes.Add(node = node.Nodes.Add(category.Name));
                        categories.Remove(category);
                        node.Tag = category;
                        break;
                    }
                }

            topNode.NodeFont = new Font("Segoe UI", 8, FontStyle.Bold | FontStyle.Underline);
            TreeView.FullRowSelect = true;
            TreeView.ExpandAll();
            TreeView.SelectedNode = topNode;
        }

        /*
        internal void UpdateTransactionsListView(Core.TransactionType type, IEnumerable<Transaction> transactions, ListViewGroup group = null)
        {
            var count = transactions != null ? transactions.Count() : 0;
            ListView.Columns[0].Text = $"Id({ListView.Items.Count + count})";

            var backColor = Core.Ecomania.Instance.Report.BackColor.Customs;
            var foreColor = Core.Ecomania.Instance.Report.ForeColor.Customs;

            if (type == Core.TransactionType.Incomes)
            {
                backColor = Core.Ecomania.Instance.Report.BackColor.Incomes;
                foreColor = Core.Ecomania.Instance.Report.ForeColor.Incomes;
            }
            else if (type == Core.TransactionType.Expenses)
            {
                backColor = Core.Ecomania.Instance.Report.BackColor.Expenses;
                foreColor = Core.Ecomania.Instance.Report.ForeColor.Expenses;
            }

            var AddItem = new Func<string, Transaction, ListViewItem>((name, transaction) =>
            {
                var item = ListView.Items.Add(transaction?.Id.ToString() ?? "-Empty-");
                item.Tag = transaction;
                item.Group = group;
                item.Font = new Font("Consolas", 9, FontStyle.Bold);
                item.BackColor = Color.FromArgb(backColor.Red, backColor.Green, backColor.Blue);
                item.ForeColor = Color.FromArgb(foreColor.Red, foreColor.Green, foreColor.Blue);

                if (transaction == null)
                {
                    item.UseItemStyleForSubItems = false;

                    var subItem = item.SubItems.Add(string.Empty);
                    subItem.BackColor = Color.FromArgb(0, 255, 255, 255);

                    subItem = item.SubItems.Add(string.Empty);
                    subItem.BackColor = Color.FromArgb(0, 255, 255, 255);
                }

                return item;
            });

            if (count == 0)
                AddItem("", null);

            foreach (var transaction in transactions)
            {
                var item = AddItem("", transaction);

                var delimitedCategoriesToolTip = string.Empty;

                if (transaction.Categories.Count == 0)
                    delimitedCategoriesToolTip = "No categories";
                else
                    delimitedCategoriesToolTip = transaction.Categories[0].Name;

                for (int i = 1; i < transaction.Categories.Count; i++)
                    delimitedCategoriesToolTip += $", {transaction.Categories[i].Name}";

                item.ToolTipText = $"{transaction.Name} {Environment.NewLine} {Environment.NewLine}" +
                                   $"       Type: {transaction.Type} {Environment.NewLine}" +
                                   $"       Date: {transaction.Date} {Environment.NewLine}" +
                                   $" Categories: [{delimitedCategoriesToolTip}] {Environment.NewLine}" +
                                   $"Description: {transaction.Description}";

                item.SubItems.Add(transaction.Money.ToString());
                item.SubItems.Add(transaction.Quantity.ToString());
            }
        }
        */

        void UpdateReportChart()
        {

        }

        private void MonthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            Ecomania.Instance.Report.Date = e.Start;
        }

        private void MonthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            //MonthCalendar.SelectionStart
        }

        public void UpdateReportTable(bool fullUpdate)
        {
            if (!fullUpdate)
            {
                DataGridView.Refresh();
                return;
            }

            var layout = false;

            try
            {
                Ledger.MaxCategoryDeepLevel = 0;

                var bindingSource = new ReportBindingSource();

                TableToolStripLabel.Text = $"{Ecomania.Instance.Report.Table}:";

                switch (Ecomania.Instance.Report.Table)
                {
                    case Core.ReportTable.GeneralLedger: bindingSource.DataSource = Ecomania.Instance.Ledger; break;
                    case Core.ReportTable.Subledger: bindingSource.DataSource = Ecomania.Instance.Subledger; break;
                    case Core.ReportTable.TrialBalance: bindingSource.DataSource = Ecomania.Instance.TrialBalance; break;
                }

                if ((bindingSource.DataSource as ICollection).Count == 0)
                    return;

                DataGridView.Visible = false;
                DataGridView.SuspendLayout();

                layout = true;

                DataGridView.DataSource = bindingSource;

                DataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                BindingNavigator.BindingSource = DataGridView.DataSource as BindingSource;

                var levelCount = 0;

                foreach (DataGridViewColumn column in DataGridView.Columns)
                {
                    if (column.ValueType == typeof(decimal))
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    else
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    if (column.DataPropertyName.StartsWith(nameof(Ledger.Category), StringComparison.InvariantCultureIgnoreCase))
                        column.Visible = levelCount++ < Ledger.MaxCategoryDeepLevel;
                }

                if (DataGridView.Columns.Contains(nameof(Ledger.Description)))
                    DataGridView.Columns[nameof(Ledger.Description)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                //var lastColumn = DataGridView.Columns.GetLastColumn(DataGridViewElementStates.None, DataGridViewElementStates.None);
                //lastColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                if (layout)
                {
                    DataGridView.ResumeLayout(performLayout: false);
                    DataGridView.PerformLayout();
                    DataGridView.Visible = true;
                }
            }
        }

        private void RefreshMajorToolStripButton_Click(object sender, EventArgs e)
        {
            UpdateReportTable(fullUpdate: true);
        }

        public void Update(bool fullUpdate)
        {
            UpdateCategoriesTreeView();
            //UpdateTransactionsListView();
            Report();
            UpdateReportTable(fullUpdate);

            SaveToolStripButton.Enabled = true;
        }
    }
}