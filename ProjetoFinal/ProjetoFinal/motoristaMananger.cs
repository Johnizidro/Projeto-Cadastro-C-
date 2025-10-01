using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinal
{
    public class MotoristaManager
    {
        private TextBox _txtNome;
        private TextBox _txtTelefone;
        private TextBox _txtCNH;
        private DataGridView _grid;
        private string _connectionString = @"Data Source=C:\Users\joao.visousa\ProjetoFinal.db;Version=3;";

        public MotoristaManager(TextBox txtNome, TextBox txtTelefone, TextBox txtCNH, DataGridView grid)
        {
            _txtNome = txtNome;
            _txtTelefone = txtTelefone;
            _txtCNH = txtCNH;
            _grid = grid;
        }

        public void Salvar()
        {
            // Validação dos campos
            if (string.IsNullOrWhiteSpace(_txtNome.Text))
            {
                MessageBox.Show("O campo Nome deve ser preenchido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtNome.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(_txtTelefone.Text))
            {
                MessageBox.Show("O campo Telefone deve ser preenchido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtTelefone.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(_txtCNH.Text))
            {
                MessageBox.Show("O campo CNH deve ser preenchido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtCNH.Focus();
                return;
            }

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sqlInsert = @"INSERT INTO Motoristas (Nome, Telefone, CNH)
                                 VALUES (@Nome, @Telefone, @CNH)";

                    using (var cmd = new SQLiteCommand(sqlInsert, connection))
                    {
                        cmd.Parameters.AddWithValue("@Nome", _txtNome.Text);
                        cmd.Parameters.AddWithValue("@Telefone", _txtTelefone.Text);
                        cmd.Parameters.AddWithValue("@CNH", _txtCNH.Text);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Motorista salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
