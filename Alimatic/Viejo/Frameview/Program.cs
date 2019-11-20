using System;
using System.Windows.Forms;

namespace Frameview
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            //Cyxor.Networking.App.Run(Network.Instance, "allal");
            //Console.ReadLine();
        }
    }
}
