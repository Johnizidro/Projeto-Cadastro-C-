namespace ProjetoFinal
{
    partial class Form_Cadastro
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Cadastro));
            txt_senha = new TextBox();
            label3 = new Label();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            txt_email = new TextBox();
            btn_cadastro = new Button();
            label1 = new Label();
            label4 = new Label();
            txt_nome = new TextBox();
            label5 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // txt_senha
            // 
            txt_senha.Font = new Font("Segoe UI", 11F);
            txt_senha.Location = new Point(72, 407);
            txt_senha.Name = "txt_senha";
            txt_senha.Size = new Size(221, 27);
            txt_senha.TabIndex = 13;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = SystemColors.Control;
            label3.Location = new Point(72, 384);
            label3.Name = "label3";
            label3.Size = new Size(55, 20);
            label3.TabIndex = 12;
            label3.Text = "Senha:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ButtonHighlight;
            label2.Location = new Point(72, 320);
            label2.Name = "label2";
            label2.Size = new Size(56, 20);
            label2.TabIndex = 11;
            label2.Text = "E-mail:";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(135, 81);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(113, 110);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 10;
            pictureBox1.TabStop = false;
            // 
            // txt_email
            // 
            txt_email.Font = new Font("Segoe UI", 11F);
            txt_email.Location = new Point(72, 343);
            txt_email.Name = "txt_email";
            txt_email.Size = new Size(221, 27);
            txt_email.TabIndex = 9;
            // 
            // btn_cadastro
            // 
            btn_cadastro.BackColor = Color.MidnightBlue;
            btn_cadastro.FlatStyle = FlatStyle.Flat;
            btn_cadastro.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_cadastro.ForeColor = SystemColors.Control;
            btn_cadastro.Location = new Point(133, 473);
            btn_cadastro.Name = "btn_cadastro";
            btn_cadastro.Size = new Size(103, 33);
            btn_cadastro.TabIndex = 8;
            btn_cadastro.Text = "Cadastrar";
            btn_cadastro.UseVisualStyleBackColor = false;
            btn_cadastro.Click += btn_cadastro_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.Control;
            label1.Location = new Point(139, 217);
            label1.Name = "label1";
            label1.Size = new Size(99, 30);
            label1.TabIndex = 7;
            label1.Text = "Cadastro";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = SystemColors.ButtonHighlight;
            label4.Location = new Point(72, 255);
            label4.Name = "label4";
            label4.Size = new Size(55, 20);
            label4.TabIndex = 15;
            label4.Text = "Nome:";
            // 
            // txt_nome
            // 
            txt_nome.Font = new Font("Segoe UI", 11F);
            txt_nome.Location = new Point(72, 278);
            txt_nome.Name = "txt_nome";
            txt_nome.Size = new Size(221, 27);
            txt_nome.TabIndex = 14;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 11F);
            label5.ForeColor = SystemColors.Control;
            label5.Location = new Point(155, 549);
            label5.Name = "label5";
            label5.Size = new Size(50, 20);
            label5.TabIndex = 16;
            label5.Text = "label5";
            // 
            // Form_Cadastro
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(370, 584);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(txt_nome);
            Controls.Add(txt_senha);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(pictureBox1);
            Controls.Add(txt_email);
            Controls.Add(btn_cadastro);
            Controls.Add(label1);
            Name = "Form_Cadastro";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cadastro";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txt_senha;
        private Label label3;
        private Label label2;
        private PictureBox pictureBox1;
        private TextBox txt_email;
        private Button btn_cadastro;
        private Label label1;
        private Label label4;
        private TextBox txt_nome;
        private Label label5;
    }
}