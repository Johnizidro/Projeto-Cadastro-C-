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
            Form_Cadastro cadastroForm = new Form_Cadastro(this); // passa referência
            cadastroForm.Show();
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

                    string query = "SELECT Nome, Email FROM Usuarios WHERE Email = @Email AND Senha = @Senha";

                    using (var command = new SQLiteCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Senha", senha);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // encontrou usuário
                            {
                                string nomeUsuario = reader["Nome"].ToString();
                                string emailUsuario = reader["Email"].ToString();

                                this.NomeUsuarioLogado = nomeUsuario;
                                this.EmailUsuarioLogado = emailUsuario;

                                // Aqui: cria o Form1 sem passar parâmetro nenhum (construtor padrão)
                                Form1 formPerfil = new Form1();

                                formPerfil.Show();

                                this.Hide(); // esconde o login
                            }
                            else
                            {
                                MessageBox.Show("Usuário ou senha inválidos.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro no acesso ao banco: " + ex.Message);
            }
        }

        // Crie essas propriedades públicas na classe do Login_Form
        public string NomeUsuarioLogado { get; private set; }
        public string EmailUsuarioLogado { get; private set; }

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
