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
    public partial class Form_Cadastro : Form
    {
        public Form_Cadastro()
        {
            InitializeComponent();
        }

        private void btn_cadastro_Click(object sender, EventArgs e)
        {

            string nome = txt_nome.Text.Trim();
            string email = txt_email.Text.Trim();
            string senha = txt_senha.Text.Trim();
           

            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(senha) )
            {
                MessageBox.Show("Por favor, preencha todos os campos.");
                return;
            }

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"INSERT INTO Usuarios 
                           (Nome, Email, Senha)
                           VALUES (@NomeUsuario, @EmailUsuario, @SenhaUsuario)";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@NomeUsuario", nome);
                        cmd.Parameters.AddWithValue("@EmailUsuario", email);
                        cmd.Parameters.AddWithValue("@SenhaUsuario", senha);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Usuário cadastrado com sucesso!");

                    // Voltar para tela de login
                    Login_Form loginForm = new Login_Form();
                    loginForm.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao realizar cadastro: " + ex.Message);
            }

            Login_Form telaCadastro = new Login_Form();
            telaCadastro.Show(); // Abre a tela de cadastro
            this.Hide();
        }
    }
}
