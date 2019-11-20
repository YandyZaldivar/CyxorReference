using System.Windows.Forms;

using Touchless.Vision.Camera;

namespace Frameview
{
    public partial class SelectDeviceForm : Form
    {
        public Camera Camera { get; }

        public SelectDeviceForm()
        {
            InitializeComponent();

            ComboBox.Items.Clear();

            foreach (var camera in CameraService.AvailableCameras)
            {
                //if (!MainForm.Instance.Cameras.Contains(camera))
                //    ComboBox.Items.Add(camera);
            }

            if (ComboBox.Items.Count > 0)
                ComboBox.SelectedIndex = 0;
        }
    }
}
