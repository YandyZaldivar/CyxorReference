using System.Windows.Forms;

namespace Halo
{
    public partial class InitForm : Form
    {
        public InitForm()
        {
            InitializeComponent();

            Cursor = Cursors.AppStarting;

            Shown += async delegate
            {
                await Cyxor.Networking.Utilities.Task.Delay(2500);
                DialogResult = DialogResult.OK;
            };
        }
    }
}
