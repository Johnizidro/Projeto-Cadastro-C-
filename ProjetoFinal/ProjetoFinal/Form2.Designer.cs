namespace ProjetoFinal
{
    partial class Login_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login_Form));
            label1 = new Label();
            btn_entrar = new Button();
            txt_email = new TextBox();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            label3 = new Label();
            txt_senha = new TextBox();
            label4 = new Label();
            lbl_cadastro = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.Control;
            label1.Location = new Point(134, 213);
            label1.Name = "label1";
            label1.Size = new Size(66, 30);
            label1.TabIndex = 0;
            label1.Text = "Login";
            // 
            // btn_entrar
            // 
            btn_entrar.FlatStyle = FlatStyle.Flat;
            btn_entrar.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_entrar.ForeColor = Color.MidnightBlue;
            btn_entrar.Location = new Point(122, 435);
            btn_entrar.Name = "btn_entrar";
            btn_entrar.Size = new Size(103, 33);
            btn_entrar.TabIndex = 1;
            btn_entrar.Text = "Entrar";
            btn_entrar.UseVisualStyleBackColor = true;
            btn_entrar.Click += btn_entrar_Click;
            // 
            // txt_email
            // 
            txt_email.Font = new Font("Segoe UI", 11F);
            txt_email.Location = new Point(66, 282);
            txt_email.Name = "txt_email";
            txt_email.Size = new Size(220, 27);
            txt_email.TabIndex = 2;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(122, 59);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(113, 110);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ButtonHighlight;
            label2.Location = new Point(66, 259);
            label2.Name = "label2";
            label2.Size = new Size(56, 20);
            label2.TabIndex = 4;
            label2.Text = "E-mail:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = SystemColors.Control;
            label3.Location = new Point(66, 330);
            label3.Name = "label3";
            label3.Size = new Size(55, 20);
            label3.TabIndex = 5;
            label3.Text = "Senha:";
            // 
            // txt_senha
            // 
            txt_senha.Font = new Font("Segoe UI", 11F);
            txt_senha.Location = new Point(66, 353);
            txt_senha.Name = "txt_senha";
            txt_senha.PasswordChar = '*';
            txt_senha.Size = new Size(220, 27);
            txt_senha.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 11F);
            label4.ForeColor = SystemColors.Control;
            label4.Location = new Point(75, 490);
            label4.Name = "label4";
            label4.Size = new Size(116, 20);
            label4.TabIndex = 7;
            label4.Text = "Para se registar, ";
            // 
            // lbl_cadastro
            // 
            lbl_cadastro.AutoSize = true;
            lbl_cadastro.BackColor = Color.Transparent;
            lbl_cadastro.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_cadastro.ForeColor = SystemColors.Control;
            lbl_cadastro.Location = new Point(182, 490);
            lbl_cadastro.Name = "lbl_cadastro";
            lbl_cadastro.Size = new Size(89, 20);
            lbl_cadastro.TabIndex = 8;
            lbl_cadastro.Text = "clique aqui!";
            lbl_cadastro.Click += lbl_cadastro_Click;
            // 
            // Login_Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(370, 584);
            Controls.Add(lbl_cadastro);
            Controls.Add(label4);
            Controls.Add(txt_senha);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(pictureBox1);
            Controls.Add(txt_email);
            Controls.Add(btn_entrar);
            Controls.Add(label1);
            Name = "Login_Form";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            Load += Login_Form_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btn_entrar;
        private TextBox txt_email;
        private PictureBox pictureBox1;
        private Label label2;
        private Label label3;
        private TextBox txt_senha;
        private Label label4;
        private Label lbl_cadastro;
    }
}