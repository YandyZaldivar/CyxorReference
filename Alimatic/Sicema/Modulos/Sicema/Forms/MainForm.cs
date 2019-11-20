using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

using org.mariuszgromada.math.mxparser;

namespace Sicema
{
    public partial class MainForm : Form
    {
        bool init = false;

        public static MainForm Instance { get; private set; }

        List<Argument> Arguments = new List<Argument>();
        List<Argument> VarArguments = new List<Argument>();
        List<Argument> GridArguments = new List<Argument>();

        public MainForm()
        {
            Instance = this;

            InitializeComponent();



            DataGridView.Rows.Clear();
            DataGridView.Columns.Clear();

            DataGridView.Columns.Add("A", "A");
            DataGridView.Columns.Add("B", "B");
            DataGridView.Columns.Add("C", "C");
            DataGridView.Columns.Add("D", "D");
            DataGridView.Columns.Add("E", "E");

            init = true;

            for (var i = 0; i < 18; i++)
            {
                DataGridView.Rows.Add();
                DataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }

            //foreach (DataGridViewColumn column in DataGridView.Columns)
            //    foreach (DataGridViewRow row in DataGridView.Rows)
            //    {
            //        var argument = new Argument($"{column.Name}{row.Index}", double.NaN);
            //        Arguments.Add(argument);
            //        GridArguments.Add(argument);
            //    }

            DataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

            var jj = 6;
            var gg = new object();
            var fy = 0.4f;
            var ks = false;


            var sss = new Dictionary<int, Dictionary<string, bool>>();

            var aa1 = new Argument("aa1 = 4");
            var bb1 = new Argument("bb1 = 6 + aa1");
            var cc1 = new Argument("cc1 = aa1 + bb1");


            Arguments.Add(aa1);
            Arguments.Add(bb1);
            Arguments.Add(cc1);

            DataGridView.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.OutsetPartial;
            DataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;



            var dh = GetArgumentValue(cc1);
            var dl = GetArgumentValue(cc1);

            var ex = new Expression("bb1 + aa1");
            ex.addArguments(bb1, aa1);
            var df = ex.calculate();

            var dd = new Argument("x=5 + 2/3");
            //var df = dd.checkSyntax();
            //var xj = dd.getErrorMessage();
            //var hs = dd.getArgumentExpressionString();

            //var de = new Argument("x = 7 + y");
            //var dfe = de.checkSyntax();
            //var xje = de.getErrorMessage();
            //var hse = de.getArgumentExpressionString();


            //var hi = dd.getArgumentsNumber();

            //Expression ss = new Expression("r = y + 9");
            //var gu = ss.getMissingUserDefinedArguments();


            //UpdateArgumentDependencies();
        }

        //void UpdateArgumentDependencies()
        //{
        //    foreach (var arg in Arguments)
        //    {
        //        arg.removeAllArguments();
        //        arg.addArguments(Arguments.ToArray());
        //        //arg.addArguments(Arguments.Where(p => p.getArgumentName() != arg.getArgumentName()).ToArray());
        //    }
        //}

        double GetArgumentValue(Argument argument)
        {
            if (argument.checkSyntax())
                return argument.getArgumentValue();

            var expression = new Expression(argument.getArgumentExpressionString());

            var argumentNames = expression.getMissingUserDefinedArguments();

            if (argumentNames.Length > 0)
            {
                var arguments = Arguments.Where(p => argumentNames.Contains(p.getArgumentName()));
                //argument.addArguments(arguments.ToArray());

                foreach (var arg in arguments)
                {
                    if (argument.checkSyntax())
                        expression.addArguments(arg);
                    else
                    {
                        var value = GetArgumentValue(arg);
                        argument.addArguments(arg);

                        if (!arg.checkSyntax())
                            throw new InvalidOperationException();
                        else
                            expression.addArguments(arg);
                    }
                }
            }

            return expression.calculate();
        }

        private void DataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (!init)
                return;

            for (var i = e.RowIndex; i < e.RowIndex + e.RowCount; i++)
                foreach (DataGridViewColumn column in DataGridView.Columns)
                {
                    var argument = new Argument($"{column.Name}{i + 1}", double.NaN);

                    Arguments.Add(argument);
                    GridArguments.Add(argument);
                }

            var c = Arguments.Count();
        }

        private void DataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //for (var i = e.RowIndex; i < e.RowIndex + e.RowCount; i++)
            //    foreach (DataGridViewColumn column in DataGridView.Columns)
            //    {
            //        var argument = Arguments.Single(p => p.getArgumentName() == $"{column.Name}{i + 1}");

            //        Arguments.Remove(argument);
            //        GridArguments.Remove(argument);
            //    }
        }

        private void ColumnasAdicionarButton_Click(object sender, EventArgs e)
        {
            if (DataGridView.Columns.Contains(ColumnasTextBox.Text))
                MessageBox.Show($"Ya existe una columna llamada {ColumnasTextBox.Text}", "Costo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (Arguments.Exists(p => p.getArgumentName() == ColumnasTextBox.Text))
                MessageBox.Show($"Ya existe una variable llamada {ColumnasTextBox.Text}", "Costo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                foreach (DataGridViewRow row in DataGridView.Rows)
                {
                    var argument = new Argument($"{ColumnasTextBox.Text}{row.Index}", double.NaN);

                    Arguments.Add(argument);
                    GridArguments.Add(argument);
                }

                DataGridView.Columns.Add(ColumnasTextBox.Text, ColumnasTextBox.Text);
                ColumnasTextBox.Clear();
            }
        }

        private void ColumnasEliminarButton_Click(object sender, EventArgs e)
        {
            if (ColumnasComboBox.SelectedItem != null)
            {
                foreach (DataGridViewRow row in DataGridView.Rows)
                {
                    var argument = Arguments.Single(p => p.getArgumentName() == $"{ColumnasComboBox.Text}{row.Index}");

                    Arguments.Add(argument);
                    GridArguments.Add(argument);
                }

                DataGridView.Columns.Remove(ColumnasComboBox.Text);
                ColumnasComboBox.Items.Remove(ColumnasComboBox.SelectedItem);
                ColumnasTextBox.Clear();
            }
        }

        private void VariablesAdicionarButton_Click(object sender, EventArgs e)
        {
            var argument = new Argument(VariablesTextBox.Text);
            argument.setDescription(VariablesTextBox.Text);

            //if (!argument.checkSyntax())
            //    MessageBox.Show($"Error en la expresión: {argument.getErrorMessage()}", "Costo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (Arguments.Exists(p => p.getArgumentName() == argument.getArgumentName()))
                MessageBox.Show($"Ya existe una variable llamada {argument.getArgumentName()}", "Costo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (DataGridView.Columns.Contains(argument.getArgumentName()))
                MessageBox.Show($"Ya existe una columna llamada {argument.getArgumentName()}", "Costo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                Arguments.Add(argument);
                VarArguments.Add(argument);
                VariablesComboBox.Items.Add(argument.getDescription());
                VariablesTextBox.Clear();

                //UpdateArgumentDependencies();
            }
        }

        private void VariablesEliminarButton_Click(object sender, EventArgs e)
        {
            if (VariablesComboBox.SelectedItem != null)
            {
                var argument = Arguments.Single(p => p.getDescription() == VariablesComboBox.Text);

                Arguments.Remove(argument);
                VarArguments.Remove(argument);

                VariablesComboBox.Items.Remove(VariablesComboBox.SelectedItem);
                VariablesTextBox.Clear();
                VariablesLabel.Text = null;

                //UpdateArgumentDependencies();
            }
        }

        private void ColumnasComboBox_DropDown(object sender, EventArgs e)
        {
            ColumnasComboBox.Items.Clear();

            foreach (DataGridViewColumn column in DataGridView.Columns)
                ColumnasComboBox.Items.Add(column.Name);
        }

        private void ColumnasComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColumnasTextBox.Text = ColumnasComboBox.Text;
        }

        private void VariablesComboBox_DropDown(object sender, EventArgs e)
        {
            VariablesComboBox.Items.Clear();

            foreach (var argument in VarArguments)
                VariablesComboBox.Items.Add(argument.getDescription());
        }

        private void VariablesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            VariablesTextBox.Text = VariablesComboBox.Text;

            var argument = VarArguments.Single(p => p.getDescription() == VariablesTextBox.Text);
            VariablesLabel.Text = GetArgumentValue(argument).ToString();
            //VariablesLabel.Text = argument.getArgumentValue().ToString();
        }

        #region DataGridView
        private void DataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var cell = DataGridView[e.ColumnIndex, e.RowIndex];

            var argument = GridArguments.Single(p => p.getArgumentName() == $"{cell.OwningColumn.Name}{cell.RowIndex + 1}");

            //if (argument.getDescription() is string description)
            if (argument.getArgumentExpressionString() is string expressionText)
                cell.Value = expressionText;
        }

        private void DataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var cell = DataGridView[e.ColumnIndex, e.RowIndex];
            var content = cell.Value.ToString();

            if (string.IsNullOrEmpty(content))
                return;

            var argument = GridArguments.Single(p => p.getArgumentName() == $"{cell.OwningColumn.Name}{cell.RowIndex + 1}");

            if (content.TrimStart().StartsWith("="))
            {
                var expressionText = content.Split(new char[] { '=' }).Skip(1).Take(1).Single().Trim();
                argument.setArgumentExpressionString(expressionText);
                argument.setDescription(content);

                //FormulaTextBox.Text = expressionText;
                FormulaTextBox.Text = content;
                cell.Value = GetArgumentValue(argument);
            }
            else if (double.TryParse(content, out var value))
            {
                argument.setArgumentValue(value);
                FormulaTextBox.Text = value.ToString();
            }
        }

        private void DataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            var cell = DataGridView[e.ColumnIndex, e.RowIndex];

            var argument = GridArguments.Single(p => p.getArgumentName() == $"{cell.OwningColumn.Name}{cell.RowIndex + 1}");
        }
        #endregion

        #region Unused
        private void DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //var cell = DataGridView[e.ColumnIndex, e.RowIndex];

            //if (cell.Tag is Expression expression)
            //    FormulaTextBox.Text = expression.getDescription();
        }

        private void DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex < 0 || e.RowIndex < 0)
            //    return;

            //var cell = DataGridView[e.ColumnIndex, e.RowIndex];
            //var content = cell.Value.ToString();


            //if (content.StartsWith("="))
            //{
            //    content = content.Substring(content.IndexOf('=') + 1).Trim();

            //    var expression = new Expression(content, Arguments.ToArray());
            //    expression.setDescription(content);
            //    cell.Tag = expression;
            //    cell.Value = expression.calculate();
            //}
        }

        private void DataGridView_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            //if (e.StateChanged == DataGridViewElementStates.Selected)
            //    if (e.Cell.Tag is Expression expression)
            //        FormulaTextBox.Text = expression.getDescription();
        }
        #endregion
    }
}
