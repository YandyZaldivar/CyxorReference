/*
  { Frameview } - Sistema de videoconferencia por imágenes
  Copyright (C) 2018 Alimatic
  Authors:  Ramón Menéndez
            Yandy Zaldivar
*/

using System.Drawing;
using System.Windows.Forms;

namespace Frameview
{
    using Cyxor.Networking;

    public partial class LoginForm : Form
    {
        public string ClientName => LoginTextBox.Text;

        public LoginForm()
        {
            InitializeComponent();

            Network.Instance.Events.DisconnectCompleted += (s, e) =>
            {
                LoginTextBox.Enabled = true;
                LoginButton.Text = "Conectar";
                LoginButton.BackColor = Color.Black;
                LoginButton.Enabled = true;
            };

            LoginButton.Click += async (s, e) =>
            {
                var result = Result.Success;

                try
                {
                    if ((LoginTextBox.Text?.Length ?? 0) < 2)
                    {
                        result = new Result(ResultCode.Error, comment: "El nombre de cliente debe ser de al menos dos caracteres");
                        return;
                    }

                    LoginTextBox.Enabled = false;
                    LoginButton.Text = "Conectando...";
                    LoginButton.BackColor = Color.Silver;
                    LoginButton.Enabled = false;

                    //Network.Instance.Config.Address = "192.168.64.1";
                    Network.Instance.Config.Address = "10.10.2.28";
                    Network.Instance.Config.SynchronizationContext = System.Threading.SynchronizationContext.Current;
                    Network.Instance.Config.EventDispatching = Cyxor.Networking.Config.EventDispatching.Synchronized;

                    if (!await Network.Instance.ConnectAsync())
                    {
                        result = Network.Instance.GetResult();
                        return;
                    }

                    using (var packet = new Packet(Network.Instance, "camera connect", ClientName))
                        if (await packet.QueryAsync())
                            Network.Instance.IsMaster = packet.Response.GetModel<bool>();
                        else
                            result = packet.Response.Result;
                }
                finally
                {
                    if (result)
                    {
                        LoginButton.Text = "Conectado";
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        LoginTextBox.Enabled = true;
                        LoginButton.Text = "Conectar";
                        LoginButton.BackColor = Color.Black;
                        LoginButton.Enabled = true;

                        if (Network.Instance.IsConnected)
                            await Network.Instance.DisconnectAsync();

                        MessageBox.Show($"{result}.", nameof(Frameview), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
            };
        }
    }
}
/* { Frameview } - Sistema de videoconferencia por imágenes */
