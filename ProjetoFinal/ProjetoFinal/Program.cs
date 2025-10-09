using System;
using System.Windows.Forms;

namespace ProjetoFinal
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Login_Form()); // inicia com a tela de login
        }
    }
}