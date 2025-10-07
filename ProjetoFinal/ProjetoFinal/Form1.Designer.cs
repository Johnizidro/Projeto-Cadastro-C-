namespace ProjetoFinal
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pictureBox1 = new PictureBox();
            lbl_nome_user = new Label();
            lbl_email_user = new Label();
            btn_entrar = new Button();
            btn_sair = new Button();
            monthCalendar1 = new MonthCalendar();
            label1 = new Label();
            label2 = new Label();
            groupBox1 = new GroupBox();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(269, 13);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(85, 83);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // lbl_nome_user
            // 
            lbl_nome_user.AutoSize = true;
            lbl_nome_user.BackColor = Color.Transparent;
            lbl_nome_user.Font = new Font("Segoe UI", 11F);
            lbl_nome_user.ForeColor = SystemColors.Control;
            lbl_nome_user.Location = new Point(78, 13);
            lbl_nome_user.Name = "lbl_nome_user";
            lbl_nome_user.Size = new Size(69, 20);
            lbl_nome_user.TabIndex = 1;
            lbl_nome_user.Text = "**********";
            // 
            // lbl_email_user
            // 
            lbl_email_user.AutoSize = true;
            lbl_email_user.BackColor = Color.Transparent;
            lbl_email_user.Font = new Font("Segoe UI", 11F);
            lbl_email_user.ForeColor = SystemColors.Control;
            lbl_email_user.Location = new Point(78, 66);
            lbl_email_user.Name = "lbl_email_user";
            lbl_email_user.Size = new Size(57, 20);
            lbl_email_user.TabIndex = 2;
            lbl_email_user.Text = "********";
            // 
            // btn_entrar
            // 
            btn_entrar.BackColor = Color.MidnightBlue;
            btn_entrar.FlatStyle = FlatStyle.Flat;
            btn_entrar.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold);
            btn_entrar.ForeColor = SystemColors.ButtonHighlight;
            btn_entrar.Location = new Point(591, 395);
            btn_entrar.Name = "btn_entrar";
            btn_entrar.Size = new Size(91, 41);
            btn_entrar.TabIndex = 3;
            btn_entrar.Text = "Entrar";
            btn_entrar.UseVisualStyleBackColor = false;
            btn_entrar.Click += btn_entrar_Click;
            // 
            // btn_sair
            // 
            btn_sair.BackColor = Color.White;
            btn_sair.FlatStyle = FlatStyle.Flat;
            btn_sair.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold);
            btn_sair.ForeColor = Color.MidnightBlue;
            btn_sair.Location = new Point(697, 395);
            btn_sair.Name = "btn_sair";
            btn_sair.Size = new Size(91, 41);
            btn_sair.TabIndex = 4;
            btn_sair.Text = "Sair";
            btn_sair.UseVisualStyleBackColor = false;
            btn_sair.Click += btn_sair_Click;
            // 
            // monthCalendar1
            // 
            monthCalendar1.BackColor = SystemColors.ButtonFace;
            monthCalendar1.Location = new Point(18, 18);
            monthCalendar1.Name = "monthCalendar1";
            monthCalendar1.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label1.ForeColor = SystemColors.Control;
            label1.Location = new Point(16, 13);
            label1.Name = "label1";
            label1.Size = new Size(59, 21);
            label1.TabIndex = 6;
            label1.Text = "Nome:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label2.ForeColor = SystemColors.Control;
            label2.Location = new Point(16, 65);
            label2.Name = "label2";
            label2.Size = new Size(52, 21);
            label2.TabIndex = 7;
            label2.Text = "Email:";
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.Transparent;
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(lbl_email_user);
            groupBox1.Controls.Add(lbl_nome_user);
            groupBox1.Controls.Add(pictureBox1);
            groupBox1.Location = new Point(428, 18);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(360, 162);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11F);
            label3.ForeColor = SystemColors.ButtonHighlight;
            label3.Location = new Point(16, 139);
            label3.Name = "label3";
            label3.Size = new Size(198, 20);
            label3.TabIndex = 9;
            label3.Text = "\"Bem-vindo colaborador(a)\"";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(800, 451);
            Controls.Add(groupBox1);
            Controls.Add(monthCalendar1);
            Controls.Add(btn_sair);
            Controls.Add(btn_entrar);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Perfil";
            FormClosed += Form1_FormClosed;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private Label lbl_nome_user;
        private Label lbl_email_user;
        private Button btn_entrar;
        private Button btn_sair;
        private MonthCalendar monthCalendar1;
        private Label label1;
        private Label label2;
        private GroupBox groupBox1;
        private Label label3;
    }
}