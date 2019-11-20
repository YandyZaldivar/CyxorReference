using System;
using System.Drawing;
using System.Windows.Forms;

namespace Halo
{
    public partial class FondoForm : Form
    {
        public MainForm MainForm { get; set; }

        public FondoForm()
        {
            InitializeComponent();

            Opacity = 0.0;

            Cursor = Cursors.AppStarting;
        }

        //async void FondoForm_Shown(object sender, EventArgs e)
        void FondoForm_Shown(object sender, EventArgs e)
        {
            var screen = Screen.FromControl(this);

            Width = screen.Bounds.Width;
            Height = screen.Bounds.Width;
            Location = new Point(0, 0);

            if (Width < 1024 || Height < 600)
            {
                var message = "La pantalla en la que intenta abrir la aplicación no cumple con " +
                    "los requisitos mínimos de resolución (1024 x 600) requeridos por Halo. " +
                    $"Su configuración actual es ({Width} x {Height}).";
                MessageBox.Show(message, "Halo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            var initForm = new InitForm();
            initForm.ShowDialog(this);

            Opacity = 1.0;

            //await Cyxor.Networking.Utilities.Task.Delay(500);

            Cursor = Cursors.AppStarting;

            MainForm = new MainForm { FondoForm = this };
            MainForm.ShowDialog(this);
        }

        void FondoForm_Paint(object sender, PaintEventArgs e)
        {
            var screen = Screen.FromControl(this);

            if (Width != screen.Bounds.Width)
                Width = screen.Bounds.Width;

            if (Height != screen.Bounds.Height)
                Height = screen.Bounds.Height;

            e.Graphics.DrawImage(PictureBox.Image, new Rectangle(32, 32, PictureBox.Width, PictureBox.Height));

            //var font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            //var text1 = "Sistema Nacional de Vigilancia a la";
            //var text2 = "Morbilidad Materna Extremadamente Grave";
            //var text3 = "Ministerio de Salud Pública";
            //var text4 = "CUBA";

            //var text1Size = e.Graphics.MeasureString(text1, font);
            //var text2Size = e.Graphics.MeasureString(text2, font);
            //var text3Size = e.Graphics.MeasureString(text3, font);
            //var text4Size = e.Graphics.MeasureString(text4, font);

            //var x = Width / 2;
            //var y = Height / 2 - SPLogoPictureBox.Height / 2;

            //e.Graphics.DrawString(text1, font, Brushes.DarkGreen, x - text1Size.Width / 2, y - 70);
            //e.Graphics.DrawString(text2, font, Brushes.DarkGreen, x - text2Size.Width / 2, y - 40);
            //e.Graphics.DrawString(text3, font, Brushes.DarkGreen, x - text3Size.Width / 2, y + 210);
            //e.Graphics.DrawString(text4, font, Brushes.DarkGreen, x - text4Size.Width / 2, y + 240);

            //x = Width / 2 - SPLogoPictureBox.Width / 2;

            //e.Graphics.DrawImage(SPLogoPictureBox.Image, new Rectangle(x, y, SPLogoPictureBox.Width, SPLogoPictureBox.Height));
        }
    }
}
