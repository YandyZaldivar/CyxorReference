using System;
using System.Windows.Forms;

namespace Frameview
{
    using Cyxor.Networking;

    public partial class InitForm : Form
    {
        public InitForm()
        {
            InitializeComponent();

            Shown += async (s, e) =>
            {
                await Utilities.Task.Delay(1000);

                Server.Instance.Config.Port = 29540;
                Server.Instance.Config.Name = "FrameviewServer";
                Server.Instance.Config.ExclusiveProcess = true;

                Server.Instance.Config.SynchronizationContext = System.Threading.SynchronizationContext.Current;
                Server.Instance.Config.EventDispatching = Cyxor.Networking.Config.EventDispatching.Synchronized;

                if (await Server.Instance.ConnectAsync())
                    DialogResult = DialogResult.OK;
                else
                {
                    var message = Server.Instance.GetResult().Comment;
                    MessageBox.Show(message, nameof(Frameview), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Environment.Exit(0);
                }
            };
        }
    }
}
