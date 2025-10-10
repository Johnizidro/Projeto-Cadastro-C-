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
using System.Security.Cryptography;

namespace ProjetoFinal
{
   
    public partial class Form_Cadastro : Form
    {

        public static class SecurityHelper
        {
            public static string HashSenha(string senha)
            {
                using (SHA256 sha = SHA256.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(senha);
                    byte[] hash = sha.ComputeHash(bytes);
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
        }

        private Form _formLogin;

        public Form_Cadastro(Form formLogin)
        {
            InitializeComponent();
            _formLogin = formLogin;
        }

        private void btn_cadastro_Click(object sender, EventArgs e)
        {

            string nome = txt_nome.Text.Trim();
            string email = txt_email.Text.Trim();
            string senha = txt_senha.Text.Trim();
            string senhaHash = SecurityHelper.HashSenha(senha);


            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(senha))
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
                        cmd.Parameters.AddWithValue("@SenhaUsuario", senhaHash);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Usuário cadastrado com sucesso!");

                    _formLogin.Show();  // Volta para a tela de login
                    this.Close();  // Esconde o login
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao realizar cadastro: " + ex.Message);
            }

         
        }

        private void Form_Cadastro_FormClosing(object sender, FormClosingEventArgs e)
        {
            _formLogin.Show();
        }

        private void lbl_back_Click(object sender, EventArgs e)
        {
            _formLogin.Show();
            this.Close();
        }
    }
}
