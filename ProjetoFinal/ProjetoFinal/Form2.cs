using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ProjetoFinal
{
    public partial class Login_Form : Form
    {
        public Login_Form()
        {
            InitializeComponent();
        }

        private void lbl_cadastro_Click(object sender, EventArgs e)
        {
            Form_Cadastro telaCadastro = new Form_Cadastro();
            telaCadastro.Show(); // Abre a tela de cadastro
            this.Hide();
        }

        private void btn_entrar_Click(object sender, EventArgs e)
        {

            string email = txt_email.Text.Trim();
            string senha = txt_senha.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Por favor, preencha os campos de email e senha.");
                return;
            }

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT COUNT(*) FROM Usuarios WHERE Email = @Email AND Senha = @Senha";

                    using (var command = new SQLiteCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Senha", senha);

                        long count = (long)command.ExecuteScalar();

                        if (count > 0)
                        {
                            // Login válido: retorna OK para o ShowDialog
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Usuário ou senha inválidos.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro no acesso ao banco: " + ex.Message);
            }
        }

        private void Login_Form_Load(object sender, EventArgs e)
        {
            if (!Database.TestarConexao())
            {
                MessageBox.Show("Erro ao conectar com o banco. Verifique o caminho ou permissões.");
                this.Close(); // ou Application.Exit(); se quiser fechar tudo
            }

        }
    }
}
