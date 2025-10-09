using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoFinal
{
    public partial class Form1 : Form


    {
        private string NomeUsuario;
        private string EmailUsuario;

        public Form1(string nome, string email)
        {
            InitializeComponent();
            NomeUsuario = nome;
            EmailUsuario = email;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            lbl_nome_user.Text = NomeUsuario ?? "Nome não disponível";
            lbl_email_user.Text = EmailUsuario ?? "Email não disponível";
        }

        private void btn_entrar_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form_Inicial formInicial = new Form_Inicial(this);  // passa o próprio perfil como referência
            formInicial.Show();

        }

        private void btn_sair_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Deseja realmente sair?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Login_Form login = new Login_Form();
                login.Show();
                this.Hide();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!Application.OpenForms.OfType<Login_Form>().Any())
            {
                Application.Exit();
            }
        }
    }
}
