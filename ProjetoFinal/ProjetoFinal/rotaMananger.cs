using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ProjetoFinal
{
    public class RotaManager
    {
        private TextBox _txtOrigem;
        private TextBox _txtDestino;
        private TextBox _txtDistancia;
        private DataGridView _grid;
        private string _connectionString = @"Data Source=C:\Users\joao.visousa\ProjetoFinal.db;Version=3;";

        public RotaManager(TextBox txtOrigem, TextBox txtDestino, TextBox txtDistancia, DataGridView grid)
        {
            _txtOrigem = txtOrigem;
            _txtDestino = txtDestino;
            _txtDistancia = txtDistancia;
            _grid = grid;
        }

        public void Consultar()
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sqlSelect = "SELECT RotaId, Origem, Destino, Distancia FROM Rotas";

                    using (var cmd = new SQLiteCommand(sqlSelect, connection))
                    {
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            var dt = new DataTable();
                            adapter.Fill(dt);

                            _grid.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao consultar rotas: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Salvar(int? rotaId = null)
        {
            // Validação Origem: obrigatório, apenas letras e espaços
            if (string.IsNullOrWhiteSpace(_txtOrigem.Text))
            {
                MessageBox.Show("O campo Origem deve ser preenchido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtOrigem.Focus();
                return;
            }
            else if (!_txtOrigem.Text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                MessageBox.Show("O campo Origem deve conter apenas letras!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtOrigem.Focus();
                return;
            }

            // Validação Destino: obrigatório, apenas letras e espaços
            if (string.IsNullOrWhiteSpace(_txtDestino.Text))
            {
                MessageBox.Show("O campo Destino deve ser preenchido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtDestino.Focus();
                return;
            }
            else if (!_txtDestino.Text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                MessageBox.Show("O campo Destino deve conter apenas letras!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtDestino.Focus();
                return;
            }

            // Validação Distância: obrigatório, decimal válido e maior que zero
            if (string.IsNullOrWhiteSpace(_txtDistancia.Text))
            {
                MessageBox.Show("O campo Distância deve ser preenchido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtDistancia.Focus();
                return;
            }

            if (!decimal.TryParse(_txtDistancia.Text, out decimal distancia) || distancia <= 0)
            {
                MessageBox.Show("O campo Distância deve conter um apenas números!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtDistancia.Focus();
                return;
            }

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sql;

                    if (rotaId == null) // novo registro
                    {
                        sql = @"INSERT INTO Rotas (Origem, Destino, Distancia)
                        VALUES (@Origem, @Destino, @Distancia)";
                    }
                    else // update
                    {
                        sql = @"UPDATE Rotas SET Origem = @Origem, Destino = @Destino, Distancia = @Distancia
                        WHERE RotaId = @RotaId";
                    }

                    using (var cmd = new SQLiteCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Origem", _txtOrigem.Text);
                        cmd.Parameters.AddWithValue("@Destino", _txtDestino.Text);
                        cmd.Parameters.AddWithValue("@Distancia", distancia);

                        if (rotaId != null)
                            cmd.Parameters.AddWithValue("@RotaId", rotaId);

                        cmd.ExecuteNonQuery();

                        string mensagem = rotaId == null ? "Rota salva com sucesso!" : "Rota atualizada com sucesso!";
                        MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LimparCampos();
                        AtualizarGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar a rota: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void Excluir(int rotaId)
        {
            var confirm = MessageBox.Show("Tem certeza que deseja excluir a rota selecionada?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sqlDelete = "DELETE FROM Rotas WHERE RotaId = @RotaId";

                    using (var cmd = new SQLiteCommand(sqlDelete, connection))
                    {
                        cmd.Parameters.AddWithValue("@RotaId", rotaId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Rota excluída com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            AtualizarGrid();
                        }
                        else
                        {
                            MessageBox.Show("Nenhuma rota foi excluída.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir a rota: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void Editar(int rotaId)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sqlSelect = "SELECT Origem, Destino, Distancia FROM Rotas WHERE RotaId = @RotaId";

                    using (var cmd = new SQLiteCommand(sqlSelect, connection))
                    {
                        cmd.Parameters.AddWithValue("@RotaId", rotaId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _txtOrigem.Text = reader["Origem"].ToString();
                                _txtDestino.Text = reader["Destino"].ToString();
                                _txtDistancia.Text = reader["Distancia"].ToString();

                            }
                            else
                            {
                                MessageBox.Show("Rota não encontrada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        public void AtualizarGrid()
        {
            Consultar();
        }

        public void LimparCampos()
        {
            _txtOrigem.Clear();
            _txtDestino.Clear();
            _txtDistancia.Clear();
            _txtOrigem.Focus();
        }
    }
}
