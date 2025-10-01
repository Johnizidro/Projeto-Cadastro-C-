namespace ProjetoFinal
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            using (var loginForm = new Login_Form())
            {
                // Abre o login modal
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Se o login for OK, abre a tela principal
                    Application.Run(new Form_Inicial());
                }
                else
                {
                    // Se o login falhar ou for cancelado, fecha o app
                    Application.Exit();
                }
            }
        }
    }
}