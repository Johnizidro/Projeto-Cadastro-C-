using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ProjetoFinal
{
    public class VeiculoManager
    {
        private TextBox _txtPlaca;
        private TextBox _txtModelo;
        private TextBox _txtConsumoMedio;
        private TextBox _txtCargaMaxima;
        private DataGridView _grid;
        private string _connectionString = @"Data Source=C:\Users\joao.visousa\ProjetoFinal.db;Version=3;";

        public VeiculoManager(
            TextBox txtPlaca,
            TextBox txtModelo,
            TextBox txtConsumoMedio,
            TextBox txtCargaMaxima,
            DataGridView grid)
        {
            _txtPlaca = txtPlaca;
            _txtModelo = txtModelo;
            _txtConsumoMedio = txtConsumoMedio;
            _txtCargaMaxima = txtCargaMaxima;
            _grid = grid;
        }

        public void Salvar()
        {
            // Validação dos campos
            if (string.IsNullOrWhiteSpace(_txtPlaca.Text))
            {
                MessageBox.Show("O campo Placa deve ser preenchido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtPlaca.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(_txtModelo.Text))
            {
                MessageBox.Show("O campo Modelo deve ser preenchido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtModelo.Focus();
                return;
            }

            if (!decimal.TryParse(_txtConsumoMedio.Text, out decimal consumoMedio))
            {
                MessageBox.Show("O campo Consumo Médio deve ser um número válido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtConsumoMedio.Focus();
                return;
            }

            if (!decimal.TryParse(_txtCargaMaxima.Text, out decimal cargaMaxima))
            {
                MessageBox.Show("O campo Carga Máxima deve ser um número válido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtCargaMaxima.Focus();
                return;
            }

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    // Aqui você pode adaptar pra INSERT ou UPDATE, por enquanto vou colocar INSERT

                    string sqlInsert = @"INSERT INTO Veiculos (Placa, Modelo, Consumo_Medio, Carga_Maxima)
                                     VALUES (@Placa, @Modelo, @ConsumoMedio, @CargaMaxima)";

                    using (var cmd = new SQLiteCommand(sqlInsert, connection))
                    {
                        cmd.Parameters.AddWithValue("@Placa", _txtPlaca.Text);
                        cmd.Parameters.AddWithValue("@Modelo", _txtModelo.Text);
                        cmd.Parameters.AddWithValue("@ConsumoMedio", consumoMedio);
                        cmd.Parameters.AddWithValue("@CargaMaxima", cargaMaxima);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Veículo salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        LimparCampos();
                        AtualizarGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar o veículo: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public void Consultar()
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sqlSelect = "SELECT VeiculoId, Placa, Modelo, Consumo_Medio, Carga_Maxima FROM Veiculos";

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
                MessageBox.Show($"Erro ao consultar veículos: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Excluir(int veiculoId)
        {
            if (veiculoId <= 0)
            {
                MessageBox.Show("Selecione um veículo válido para excluir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Tem certeza que deseja excluir o veículo selecionado?", "Confirmar exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sqlDelete = "DELETE FROM Veiculos WHERE VeiculoId = @VeiculoId";

                    using (var cmd = new SQLiteCommand(sqlDelete, connection))
                    {
                        cmd.Parameters.AddWithValue("@VeiculoId", veiculoId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Veículo excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            AtualizarGrid();
                        }
                        else
                        {
                            MessageBox.Show("Veículo não encontrado para exclusão.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir veículo: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AtualizarGrid()
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sqlSelect = "SELECT VeiculoId, Placa, Modelo, Consumo_Medio, Carga_Maxima FROM Veiculos";

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

        public void LimparCampos()
        {
            _txtPlaca.Clear();
            _txtModelo.Clear();
            _txtConsumoMedio.Clear();
            _txtCargaMaxima.Clear();

            // Opcional: coloca o foco no primeiro campo
            _txtPlaca.Focus();
        }
    }
}