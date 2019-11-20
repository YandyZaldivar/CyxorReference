using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Halo
{
    public partial class GestionarPacienteControl : UserControl
    {
        [DefaultValue("Título")]
        public string Titulo
        {
            get => TitleLabel.Text;
            set => TitleLabel.Text = value;
        }

        [DefaultValue(null)]
        public string Error
        {
            get => ErrorLabel.Text;
            set
            {
                ErrorLabel.Text = value;

                if (!string.IsNullOrEmpty(ErrorLabel.Text))
                    System.Media.SystemSounds.Hand.Play();
            }
        }

        [DefaultValue("Confirme la gestión del paciente")]
        public string Message
        {
            get => ConfirmarGroupBox.Text;
            set => ConfirmarGroupBox.Text = value;
        }

        public GroupBox GroupBox => ConfirmarGroupBox;

        public event EventHandler AceptarButtonClick
        {
            add => AceptarButton.Click += value;
            remove => AceptarButton.Click -= value;
        }

        public event EventHandler CancelarButtonClick
        {
            add => CancelarButton.Click += value;
            remove => CancelarButton.Click -= value;
        }

        public event MouseEventHandler AceptarButtonMouseUp
        {
            add => AceptarButton.MouseUp += value;
            remove => AceptarButton.MouseUp -= value;
        }

        public event MouseEventHandler CancelarButtonMouseUp
        {
            add => CancelarButton.MouseUp += value;
            remove => CancelarButton.MouseUp -= value;
        }

        public Color HotColor
        {
            get => TitleLabel.ForeColor;
            set
            {
                TitleLabel.ForeColor = value;
                AceptarButton.ForeColor = value;
                AceptarButton.FlatAppearance.BorderColor = value;
            }
        }

        public Color LightColor
        {
            get => AceptarButton.FlatAppearance.MouseDownBackColor;
            set => AceptarButton.FlatAppearance.MouseDownBackColor = value;
        }

        public GestionarPacienteControl()
        {
            InitializeComponent();
        }
    }
}
