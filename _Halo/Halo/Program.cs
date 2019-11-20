using System;
using System.Windows.Forms;

namespace Halo
{
//Instituciones y profesionales participantes:
// - Dra.Mercedes Piloto Padrón
//   (Ministerio de Salud Pública. MINSAP)
// - Dra.Mireya Álvarez Toste
//   (Instituto Nacional de Higiene, Epidemiología y Microbiología.INHEM)

//Correo:   halo @infomed.sld.cu

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
            Application.Run(new FondoForm());
        }
    }
}
