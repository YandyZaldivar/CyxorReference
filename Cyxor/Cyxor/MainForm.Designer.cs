namespace Cyxor
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node2");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node5");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node6");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Node3", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Node4");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Node1", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Node8");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Node7");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Categories", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7,
            treeNode8});
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "1234567890"),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "3948745798.00", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)))),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "56"),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "Transportation")}, -1);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.PropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.MajorPanel = new System.Windows.Forms.Panel();
            this.DataGridView = new System.Windows.Forms.DataGridView();
            this.BindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.TableToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.RefreshMajorToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.SaveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.ChartPanel = new System.Windows.Forms.Panel();
            this.Chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ChartX = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.RichTextBoxPanel = new System.Windows.Forms.Panel();
            this.RichTextBox = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.TreeView = new System.Windows.Forms.TreeView();
            this.ListView = new System.Windows.Forms.ListView();
            this.IdColumHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MoneyColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.QuantityColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.miniToolStrip = new System.Windows.Forms.ToolStrip();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.TopPanel = new System.Windows.Forms.Panel();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.LeftBottomLabel = new System.Windows.Forms.Label();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.MiddleBottomLabel = new System.Windows.Forms.Label();
            this.RightBottomLabel = new System.Windows.Forms.Label();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.MajorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingNavigator)).BeginInit();
            this.BindingNavigator.SuspendLayout();
            this.ChartPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartX)).BeginInit();
            this.RichTextBoxPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Panel1MinSize = 257;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Panel2MinSize = 540;
            this.splitContainer1.Size = new System.Drawing.Size(1008, 487);
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.propertyGrid1);
            this.panel2.Controls.Add(this.PropertyGrid);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(249, 479);
            this.panel2.TabIndex = 5;
            // 
            // PropertyGrid
            // 
            this.PropertyGrid.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.PropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyGrid.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PropertyGrid.HelpVisible = false;
            this.PropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.PropertyGrid.Name = "PropertyGrid";
            this.PropertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.PropertyGrid.Size = new System.Drawing.Size(249, 479);
            this.PropertyGrid.TabIndex = 1;
            this.PropertyGrid.ToolbarVisible = false;
            // 
            // splitContainer3
            // 
            this.splitContainer3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer5);
            this.splitContainer3.Panel1.Controls.Add(this.button1);
            this.splitContainer3.Panel1.Padding = new System.Windows.Forms.Padding(4);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer3.Panel2.Padding = new System.Windows.Forms.Padding(4);
            this.splitContainer3.Panel2MinSize = 257;
            this.splitContainer3.Size = new System.Drawing.Size(747, 487);
            this.splitContainer3.SplitterDistance = 486;
            this.splitContainer3.TabIndex = 0;
            // 
            // splitContainer5
            // 
            this.splitContainer5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(81)))), ((int)(((byte)(0)))));
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer5.Location = new System.Drawing.Point(4, 4);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer5.Panel1.Controls.Add(this.MajorPanel);
            this.splitContainer5.Panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer5.Panel2.Controls.Add(this.ChartPanel);
            this.splitContainer5.Panel2.Controls.Add(this.RichTextBoxPanel);
            this.splitContainer5.Panel2.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.splitContainer5.Size = new System.Drawing.Size(478, 479);
            this.splitContainer5.SplitterDistance = 238;
            this.splitContainer5.TabIndex = 3;
            // 
            // MajorPanel
            // 
            this.MajorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MajorPanel.Controls.Add(this.DataGridView);
            this.MajorPanel.Controls.Add(this.BindingNavigator);
            this.MajorPanel.Controls.Add(this.statusStrip2);
            this.MajorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MajorPanel.Location = new System.Drawing.Point(0, 0);
            this.MajorPanel.Name = "MajorPanel";
            this.MajorPanel.Size = new System.Drawing.Size(478, 234);
            this.MajorPanel.TabIndex = 0;
            // 
            // DataGridView
            // 
            this.DataGridView.BackgroundColor = System.Drawing.Color.White;
            this.DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView.Location = new System.Drawing.Point(0, 25);
            this.DataGridView.Name = "DataGridView";
            this.DataGridView.Size = new System.Drawing.Size(476, 185);
            this.DataGridView.TabIndex = 5;
            // 
            // BindingNavigator
            // 
            this.BindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.BindingNavigator.CountItem = this.bindingNavigatorCountItem;
            this.BindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.BindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TableToolStripLabel,
            this.toolStripSeparator1,
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.RefreshMajorToolStripButton,
            this.SaveToolStripButton});
            this.BindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.BindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.BindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.BindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.BindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.BindingNavigator.Name = "BindingNavigator";
            this.BindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
            this.BindingNavigator.Size = new System.Drawing.Size(476, 25);
            this.BindingNavigator.TabIndex = 4;
            this.BindingNavigator.Text = "bindingNavigator1";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // TableToolStripLabel
            // 
            this.TableToolStripLabel.Name = "TableToolStripLabel";
            this.TableToolStripLabel.Size = new System.Drawing.Size(86, 22);
            this.TableToolStripLabel.Text = "General ledger:";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // RefreshMajorToolStripButton
            // 
            this.RefreshMajorToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.RefreshMajorToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("RefreshMajorToolStripButton.Image")));
            this.RefreshMajorToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RefreshMajorToolStripButton.Name = "RefreshMajorToolStripButton";
            this.RefreshMajorToolStripButton.Size = new System.Drawing.Size(50, 22);
            this.RefreshMajorToolStripButton.Text = "Refresh";
            this.RefreshMajorToolStripButton.Click += new System.EventHandler(this.RefreshMajorToolStripButton_Click);
            // 
            // SaveToolStripButton
            // 
            this.SaveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SaveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveToolStripButton.Image")));
            this.SaveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveToolStripButton.Name = "SaveToolStripButton";
            this.SaveToolStripButton.Size = new System.Drawing.Size(35, 22);
            this.SaveToolStripButton.Text = "Save";
            // 
            // statusStrip2
            // 
            this.statusStrip2.Location = new System.Drawing.Point(0, 210);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(476, 22);
            this.statusStrip2.TabIndex = 6;
            this.statusStrip2.Text = "statusStrip2";
            // 
            // ChartPanel
            // 
            this.ChartPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ChartPanel.Controls.Add(this.Chart);
            this.ChartPanel.Controls.Add(this.ChartX);
            this.ChartPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartPanel.Location = new System.Drawing.Point(0, 4);
            this.ChartPanel.Name = "ChartPanel";
            this.ChartPanel.Size = new System.Drawing.Size(478, 233);
            this.ChartPanel.TabIndex = 0;
            // 
            // Chart
            // 
            chartArea1.Name = "ChartArea1";
            this.Chart.ChartAreas.Add(chartArea1);
            this.Chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.Chart.Legends.Add(legend1);
            this.Chart.Location = new System.Drawing.Point(0, 0);
            this.Chart.Name = "Chart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.Chart.Series.Add(series1);
            this.Chart.Size = new System.Drawing.Size(476, 231);
            this.Chart.TabIndex = 6;
            this.Chart.Text = "chart1";
            // 
            // ChartX
            // 
            chartArea2.AxisX.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDot;
            chartArea2.AxisX.LineWidth = 2;
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisY.IsInterlaced = true;
            chartArea2.AxisY.IsLabelAutoFit = false;
            chartArea2.AxisY.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea2.AxisY.LineWidth = 2;
            chartArea2.AxisY.MajorGrid.Enabled = false;
            chartArea2.AxisY.MinorTickMark.Enabled = true;
            chartArea2.Name = "ChartArea1";
            this.ChartX.ChartAreas.Add(chartArea2);
            this.ChartX.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.DockedToChartArea = "ChartArea1";
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend2.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row;
            legend2.Name = "Legend";
            this.ChartX.Legends.Add(legend2);
            this.ChartX.Location = new System.Drawing.Point(0, 0);
            this.ChartX.Name = "ChartX";
            series2.ChartArea = "ChartArea1";
            series2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series2.IsValueShownAsLabel = true;
            series2.LabelForeColor = System.Drawing.Color.Blue;
            series2.Legend = "Legend";
            series2.Name = "Incomes";
            series2.YValuesPerPoint = 6;
            series3.ChartArea = "ChartArea1";
            series3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series3.IsValueShownAsLabel = true;
            series3.LabelForeColor = System.Drawing.Color.Red;
            series3.Legend = "Legend";
            series3.Name = "Expenses";
            this.ChartX.Series.Add(series2);
            this.ChartX.Series.Add(series3);
            this.ChartX.Size = new System.Drawing.Size(476, 231);
            this.ChartX.TabIndex = 3;
            this.ChartX.Text = "chart1";
            this.ChartX.Visible = false;
            // 
            // RichTextBoxPanel
            // 
            this.RichTextBoxPanel.Controls.Add(this.RichTextBox);
            this.RichTextBoxPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichTextBoxPanel.Location = new System.Drawing.Point(0, 4);
            this.RichTextBoxPanel.Name = "RichTextBoxPanel";
            this.RichTextBoxPanel.Padding = new System.Windows.Forms.Padding(4);
            this.RichTextBoxPanel.Size = new System.Drawing.Size(478, 233);
            this.RichTextBoxPanel.TabIndex = 5;
            // 
            // RichTextBox
            // 
            this.RichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichTextBox.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RichTextBox.Location = new System.Drawing.Point(4, 4);
            this.RichTextBox.Name = "RichTextBox";
            this.RichTextBox.Size = new System.Drawing.Size(470, 225);
            this.RichTextBox.TabIndex = 5;
            this.RichTextBox.Text = resources.GetString("RichTextBox.Text");
            this.RichTextBox.Visible = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(373, 120);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // splitContainer4
            // 
            this.splitContainer4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(81)))), ((int)(((byte)(0)))));
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer4.Location = new System.Drawing.Point(4, 4);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer4.Panel1.Controls.Add(this.TreeView);
            this.splitContainer4.Panel1.Padding = new System.Windows.Forms.Padding(1, 1, 1, 4);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer4.Panel2.Controls.Add(this.ListView);
            this.splitContainer4.Panel2.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.splitContainer4.Size = new System.Drawing.Size(249, 479);
            this.splitContainer4.SplitterDistance = 162;
            this.splitContainer4.TabIndex = 1;
            // 
            // TreeView
            // 
            this.TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TreeView.FullRowSelect = true;
            this.TreeView.ItemHeight = 20;
            this.TreeView.LineColor = System.Drawing.Color.Blue;
            this.TreeView.Location = new System.Drawing.Point(1, 1);
            this.TreeView.Name = "TreeView";
            treeNode1.Name = "Node2";
            treeNode1.Text = "Node2";
            treeNode2.Name = "Node5";
            treeNode2.Text = "Node5";
            treeNode3.Name = "Node6";
            treeNode3.Text = "Node6";
            treeNode4.Name = "Node3";
            treeNode4.Text = "Node3";
            treeNode5.Name = "Node4";
            treeNode5.Text = "Node4";
            treeNode6.Name = "Node1";
            treeNode6.Text = "Node1";
            treeNode7.Name = "Node8";
            treeNode7.Text = "Node8";
            treeNode8.Name = "Node7";
            treeNode8.Text = "Node7";
            treeNode9.Name = "Node0";
            treeNode9.NodeFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode9.Text = "Categories";
            this.TreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode9});
            this.TreeView.Size = new System.Drawing.Size(247, 157);
            this.TreeView.TabIndex = 0;
            // 
            // ListView
            // 
            this.ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IdColumHeader,
            this.MoneyColumnHeader,
            this.QuantityColumnHeader});
            this.ListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            listViewGroup1.Header = "ListViewGroup";
            listViewGroup1.Name = "listViewGroup1";
            listViewGroup2.Header = "ListViewGroup";
            listViewGroup2.Name = "listViewGroup2";
            this.ListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.ListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.ListView.Location = new System.Drawing.Point(0, 4);
            this.ListView.Name = "ListView";
            this.ListView.Size = new System.Drawing.Size(249, 309);
            this.ListView.TabIndex = 2;
            this.ListView.UseCompatibleStateImageBehavior = false;
            this.ListView.View = System.Windows.Forms.View.Details;
            // 
            // IdColumHeader
            // 
            this.IdColumHeader.Text = "Id";
            this.IdColumHeader.Width = 80;
            // 
            // MoneyColumnHeader
            // 
            this.MoneyColumnHeader.Text = "Money";
            this.MoneyColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.MoneyColumnHeader.Width = 100;
            // 
            // QuantityColumnHeader
            // 
            this.QuantityColumnHeader.Text = "Units";
            this.QuantityColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.QuantityColumnHeader.Width = 48;
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.CanOverflow = false;
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.miniToolStrip.Location = new System.Drawing.Point(184, 3);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Size = new System.Drawing.Size(247, 25);
            this.miniToolStrip.TabIndex = 1;
            // 
            // ToolTip
            // 
            this.ToolTip.IsBalloon = true;
            this.ToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ToolTip.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // TopPanel
            // 
            this.TopPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(1008, 25);
            this.TopPanel.TabIndex = 7;
            // 
            // BottomPanel
            // 
            this.BottomPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.BottomPanel.Controls.Add(this.splitContainer2);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BottomPanel.Location = new System.Drawing.Point(0, 512);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(1008, 25);
            this.BottomPanel.TabIndex = 8;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.LeftBottomLabel);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer6);
            this.splitContainer2.Size = new System.Drawing.Size(1008, 25);
            this.splitContainer2.SplitterDistance = 257;
            this.splitContainer2.TabIndex = 0;
            // 
            // LeftBottomLabel
            // 
            this.LeftBottomLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LeftBottomLabel.ForeColor = System.Drawing.Color.White;
            this.LeftBottomLabel.Location = new System.Drawing.Point(0, 0);
            this.LeftBottomLabel.Name = "LeftBottomLabel";
            this.LeftBottomLabel.Size = new System.Drawing.Size(257, 25);
            this.LeftBottomLabel.TabIndex = 0;
            this.LeftBottomLabel.Text = "LeftBottomLabel";
            this.LeftBottomLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.MiddleBottomLabel);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.RightBottomLabel);
            this.splitContainer6.Size = new System.Drawing.Size(747, 25);
            this.splitContainer6.SplitterDistance = 486;
            this.splitContainer6.TabIndex = 0;
            // 
            // MiddleBottomLabel
            // 
            this.MiddleBottomLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MiddleBottomLabel.ForeColor = System.Drawing.Color.White;
            this.MiddleBottomLabel.Location = new System.Drawing.Point(0, 0);
            this.MiddleBottomLabel.Name = "MiddleBottomLabel";
            this.MiddleBottomLabel.Size = new System.Drawing.Size(486, 25);
            this.MiddleBottomLabel.TabIndex = 1;
            this.MiddleBottomLabel.Text = "MiddleBottomLabel";
            this.MiddleBottomLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RightBottomLabel
            // 
            this.RightBottomLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightBottomLabel.ForeColor = System.Drawing.Color.White;
            this.RightBottomLabel.Location = new System.Drawing.Point(0, 0);
            this.RightBottomLabel.Name = "RightBottomLabel";
            this.RightBottomLabel.Size = new System.Drawing.Size(257, 25);
            this.RightBottomLabel.TabIndex = 1;
            this.RightBottomLabel.Text = "RightBottomLabel";
            this.RightBottomLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(83, 88);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(130, 130);
            this.propertyGrid1.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.ClientSize = new System.Drawing.Size(1008, 537);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.TopPanel);
            this.Controls.Add(this.BottomPanel);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ecomania";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.MajorPanel.ResumeLayout(false);
            this.MajorPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingNavigator)).EndInit();
            this.BindingNavigator.ResumeLayout(false);
            this.BindingNavigator.PerformLayout();
            this.ChartPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartX)).EndInit();
            this.RichTextBoxPanel.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.BottomPanel.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.ToolStrip miniToolStrip;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.ListView ListView;
        private System.Windows.Forms.ColumnHeader IdColumHeader;
        private System.Windows.Forms.ColumnHeader MoneyColumnHeader;
        private System.Windows.Forms.ColumnHeader QuantityColumnHeader;
        public System.Windows.Forms.TreeView TreeView;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.Panel ChartPanel;
        private System.Windows.Forms.DataVisualization.Charting.Chart ChartX;
        private System.Windows.Forms.Panel MajorPanel;
        private System.Windows.Forms.DataGridView DataGridView;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.BindingNavigator BindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton RefreshMajorToolStripButton;
        private System.Windows.Forms.Panel RichTextBoxPanel;
        private System.Windows.Forms.RichTextBox RichTextBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel TableToolStripLabel;
        private System.Windows.Forms.DataVisualization.Charting.Chart Chart;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.PropertyGrid PropertyGrid;
        private System.Windows.Forms.ToolStripButton SaveToolStripButton;
        private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label LeftBottomLabel;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.Label MiddleBottomLabel;
        private System.Windows.Forms.Label RightBottomLabel;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
    }
}

