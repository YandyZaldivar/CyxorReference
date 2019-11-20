using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Halo
{
    public partial class NudControl : UserControl
    {
        const string DefaultLabelText = "  ";
        static string DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        public decimal Value
        {
            get => NumericUpDown.Value;
            set => NumericUpDown.Value = value > Maximum ? Maximum : value < Minimum ? Minimum : value;
        }

        public decimal Maximum
        {
            get => NumericUpDown.Maximum;
            set => NumericUpDown.Maximum = value;
        }

        public decimal Minimum
        {
            get => NumericUpDown.Minimum;
            set => NumericUpDown.Minimum = value;
        }

        public int DecimalPlaces
        {
            get => NumericUpDown.DecimalPlaces;
            set => NumericUpDown.DecimalPlaces = value;
        }

        public NumericUpDown NumericUpDownControl => NumericUpDown;

        public void SetFocus() => NumericUpDown.Focus();

        public NudControl()
        {
            InitializeComponent();

            Label.Visible = true;

            Label.Text = DefaultLabelText;

            Label.Click += delegate { NumericUpDown.Focus(); };

            NumericUpDown.ForeColor = NumericUpDown.Value == Minimum ? Color.White : Color.Black;

            Resize += delegate
            {
                if (Panel.Width != NumericUpDown.Width - 17)
                    Panel.Width = NumericUpDown.Width - 17;

                if (Panel.Height != NumericUpDown.Height - 4)
                    Panel.Height = NumericUpDown.Height - 4;

                Panel.Location = new Point(1, 2);
            };

            NumericUpDown.ValueChanged += delegate
            {
                NumericUpDown.ForeColor = NumericUpDown.Value == Minimum ? Color.White : Color.Black;
                Label.Text = NumericUpDown.Value != Minimum ? NumericUpDown.Value.ToString() : DefaultLabelText;

                if (NumericUpDown.Focused && string.IsNullOrWhiteSpace(Label.Text) && !Label.Font.Underline)
                    Label.Font = new Font(Label.Font, FontStyle.Underline);
                else if (!string.IsNullOrWhiteSpace(Label.Text) && Label.Font.Underline)
                    Label.Font = new Font(Label.Font, FontStyle.Regular);
            };

            NumericUpDown.GotFocus += delegate
            {
                if (string.IsNullOrWhiteSpace(Label.Text))
                    Label.Font = new Font(Label.Font, FontStyle.Underline);

                Label.ForeColor = Color.Blue;
            };

            NumericUpDown.LostFocus += delegate
            {
                Label.Text = Value == Minimum ? DefaultLabelText : Value.ToString();

                if (Label.Font.Underline)
                    Label.Font = new Font(Label.Font, FontStyle.Regular);

                Label.ForeColor = Label.DefaultForeColor;
            };

            NumericUpDown.KeyPress += (s, e) =>
            {
                e.Handled = true;

                if (e.KeyChar == DecimalSeparator[0] && DecimalPlaces > 0 && !Label.Text.Contains(DecimalSeparator))
                    Label.Text = Label.Text == DefaultLabelText ? DecimalSeparator : Label.Text + DecimalSeparator;
                else if (e.KeyChar >= '0' && e.KeyChar <= '9')
                {
                    var key = e.KeyChar.ToString();

                    if (key == "0" && string.IsNullOrWhiteSpace(Label.Text))
                        return;

                    if (Label.Text.Contains(DecimalSeparator))
                    {
                        var index = Label.Text.IndexOf(DecimalSeparator) + 1;
                        if (Label.Text.Substring(index).Length == DecimalPlaces)
                            return;
                    }

                    var value = Label.Text == DefaultLabelText ? string.Empty : Label.Text;
                    Value = Label.Text == DefaultLabelText ? e.KeyChar - '0' : decimal.Parse($"{value}{key}");

                    if (Value < Maximum)
                        Label.Text = value + key;

                }
                else if (e.KeyChar == (int)Keys.Back && Label.Text != DefaultLabelText)
                {
                    var value = Label.Text.Substring(0, Label.Text.Length - 1);
                    Value = string.IsNullOrEmpty(value) || value == DecimalSeparator ? Minimum : decimal.Parse(value);
                    Label.Text = string.IsNullOrEmpty(value) ? DefaultLabelText : value;
                }

                if (Label.Text == DefaultLabelText)
                {
                    if (!Label.Font.Underline)
                        Label.Font = new Font(Label.Font, FontStyle.Underline);

                    if (Label.ForeColor != Color.Blue)
                        Label.ForeColor = Color.Blue;
                }
                else
                {
                    if (Label.Font.Underline)
                        Label.Font = new Font(Label.Font, FontStyle.Regular);

                    if (!decimal.TryParse(Label.Text, out var dValue))
                        dValue = -1;
                        
                    Label.ForeColor = dValue < Minimum ? Color.Red : Color.Blue;
                }
            };
        }
    }
}
