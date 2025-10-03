using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ProjetoFinal
{
    public class MotoristaManager
    {
        private int? _motoristaIdEmEdicao = null;
        private TextBox _txtNome;
        private TextBox _txtTelefone;
        private TextBox _txtCNH;
        private DataGridView _grid;
        private string _connectionString = @"Data Source=C:\Users\joao.visousa\ProjetoFinal.db;Version=3;";
        private Label _lbl_cancelar_mot;
        private Label _lbl_salvar_mot;

        public MotoristaManager(TextBox txtNome, TextBox txtTelefone, TextBox txtCNH, DataGridView grid, Label lbl_cancelar_mot, Label lbl_salvar_mot)
        {
            _txtNome = txtNome;
            _txtTelefone = txtTelefone;
            _txtCNH = txtCNH;
            _grid = grid;
            _lbl_cancelar_mot = lbl_cancelar_mot;
            _lbl_salvar_mot = lbl_salvar_mot;
        }

        public void Salvar(int? motoristaId = null)
        {
            if (string.IsNullOrWhiteSpace(_txtNome.Text) || !Regex.IsMatch(_txtNome.Text, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("O campo Nome deve ser preenchido apenas com letras!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtNome.Focus();
                return;
            }

            // Validação Telefone - só números
            if (string.IsNullOrWhiteSpace(_txtTelefone.Text) || !Regex.IsMatch(_txtTelefone.Text, @"^\d+$"))
            {
                MessageBox.Show("O campo Telefone deve conter apenas números!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtTelefone.Focus();
                return;
            }

            // Validação CNH - só números
            if (string.IsNullOrWhiteSpace(_txtCNH.Text) || !Regex.IsMatch(_txtCNH.Text, @"^\d+$"))
            {
                MessageBox.Show("O campo CNH deve conter apenas números!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtCNH.Focus();
                return;
            }

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sql;

                    if (motoristaId == null)  // motoristaId é um int? que representa o ID a ser editado, ou null se for novo
                    {
                        sql = @"INSERT INTO Motoristas (Nome, Telefone, CNH)
                    VALUES (@Nome, @Telefone, @CNH)";
                    }
                    else
                    {
                        sql = @"UPDATE Motoristas SET Nome = @Nome, Telefone = @Telefone, CNH = @CNH
                    WHERE MotoristaId = @MotoristaId";
                    }

                    using (var cmd = new SQLiteCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Nome", _txtNome.Text);
                        cmd.Parameters.AddWithValue("@Telefone", _txtTelefone.Text);
                        cmd.Parameters.AddWithValue("@CNH", _txtCNH.Text);

                        if (motoristaId != null)
                            cmd.Parameters.AddWithValue("@MotoristaId", motoristaId);

                        cmd.ExecuteNonQuery();

                        string mensagem = motoristaId == null ? "Motorista salvo com sucesso!" : "Motorista atualizado com sucesso!";
                        MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LimparCampos();
                        AtualizarGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar o motorista: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public void Consultar()
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sqlSelect = "SELECT MotoristaId, Nome, Telefone, CNH FROM Motoristas";

                    using (var cmd = new SQLiteCommand(sqlSelect, connection))
                    {
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            var dt = new System.Data.DataTable();
                            adapter.Fill(dt);

                            _grid.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao consultar motoristas: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Excluir(int motoristaId) {
            if (motoristaId <= 0)
            {
                MessageBox.Show("Selecione um motorista válido para excluir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Tem certeza que deseja excluir o motorista selecionado?", "Confirmar exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sqlDelete = "DELETE FROM Motoristas WHERE MotoristaId = @MotoristaId";

                    using (var cmd = new SQLiteCommand(sqlDelete, connection))
                    {
                        cmd.Parameters.AddWithValue("@MotoristaId", motoristaId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Motorista excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            AtualizarGrid();
                        }
                        else
                        {
                            MessageBox.Show("Motorista não encontrado para exclusão.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir motorista: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Editar(int motoristaId)
        {
            _motoristaIdEmEdicao = motoristaId;
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sqlSelect = "SELECT Nome, Telefone, CNH FROM Motoristas WHERE MotoristaId = @MotoristaId";

                    using (var cmd = new SQLiteCommand(sqlSelect, connection))
                    {
                        cmd.Parameters.AddWithValue("@MotoristaId", motoristaId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _txtNome.Text = reader["Nome"].ToString();
                                _txtTelefone.Text = reader["Telefone"].ToString();
                                _txtCNH.Text = reader["CNH"].ToString();

                                _lbl_cancelar_mot.Text = "Cancelar";
                             
                            }
                            else
                            {
                                MessageBox.Show("Motorista não encontrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados para edição: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CancelarEdicao()
        {
            var resultado = MessageBox.Show(
                "Tem certeza que deseja cancelar a edição? As alterações não salvas serão perdidas.",
                "Confirmar cancelamento",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado != DialogResult.Yes)
                return;

            LimparCampos();
            _motoristaIdEmEdicao = null;
            _lbl_cancelar_mot.Text = "Excluir";
            _lbl_salvar_mot.Text = "Incluir";// volta o texto do botão pro original
        }
        public void AtualizarGrid()
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sqlSelect = "SELECT MotoristaId, Nome, Telefone, CNH FROM Motoristas";

                    using (var cmd = new SQLiteCommand(sqlSelect, connection))
                    {
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            var dt = new System.Data.DataTable();
                            adapter.Fill(dt);

                            _grid.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar o grid: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void LimparCampos() { 
            _txtNome.Clear(); 
            _txtTelefone.Clear(); 
            _txtCNH.Clear(); 
        }
    }
}
