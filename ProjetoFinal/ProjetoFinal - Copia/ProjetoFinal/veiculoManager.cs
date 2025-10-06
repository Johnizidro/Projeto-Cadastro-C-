using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ProjetoFinal
{
    public class VeiculoManager
    {
        private int? _veiculoIdEmEdicao = null;
      

        private TextBox _txtPlaca;
        private TextBox _txtModelo;
        private TextBox _txtConsumoMedio;
        private TextBox _txtCargaMaxima;
        private DataGridView _grid;
        private string _connectionString = @"Data Source=C:\Users\joao.visousa\ProjetoFinal.db;Version=3;";
        private Label _lbl_btn_exVeic;
        private Label _lbl_btn_veic;

        public VeiculoManager(
            TextBox txtPlaca,
            TextBox txtModelo,
            TextBox txtConsumoMedio,
            TextBox txtCargaMaxima,
            DataGridView grid,
            Label lbl_btn_exVeic,
            Label lbl_btn_veic)
        {
            _txtPlaca = txtPlaca;
            _txtModelo = txtModelo;
            _txtConsumoMedio = txtConsumoMedio;
            _txtCargaMaxima = txtCargaMaxima;
            _grid = grid;
            _lbl_btn_exVeic = lbl_btn_exVeic;
            _lbl_btn_veic = lbl_btn_veic;
        }

        public void Salvar(int? veiculoId = null)
        {
            // Validações

            // Placa: obrigatório, não vazio, pode ser alfanumérica
            if (string.IsNullOrWhiteSpace(_txtPlaca.Text))
            {
                MessageBox.Show("O campo Placa deve ser preenchido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtPlaca.Focus();
                return;
            }
            // Modelo: obrigatório, só letras (validação simples)
            if (string.IsNullOrWhiteSpace(_txtModelo.Text))
            {
                MessageBox.Show("O campo Modelo deve ser preenchido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtModelo.Focus();
                return;
            }
            else if (!_txtModelo.Text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                MessageBox.Show("O campo Modelo deve conter apenas letras!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtModelo.Focus();
                return;
            }

            // Consumo Medio: obrigatório, decimal válido e maior que zero
            if (string.IsNullOrWhiteSpace(_txtConsumoMedio.Text))
            {
                MessageBox.Show("O campo Consumo Médio deve ser preenchido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtConsumoMedio.Focus();
                return;
            }
            if (!decimal.TryParse(_txtConsumoMedio.Text, out decimal consumo) || consumo <= 0)
            {
                MessageBox.Show("O campo Consumo Médio deve conter um número decimal válido e maior que zero!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtConsumoMedio.Focus();
                return;
            }

            // Carga Maxima: obrigatório, decimal válido e maior que zero
            if (string.IsNullOrWhiteSpace(_txtCargaMaxima.Text))
            {
                MessageBox.Show("O campo Carga Máxima deve ser preenchido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtCargaMaxima.Focus();
                return;
            }
            if (!decimal.TryParse(_txtCargaMaxima.Text, out decimal carga) || carga <= 0)
            {
                MessageBox.Show("O campo Carga Máxima deve conter um número decimal válido e maior que zero!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtCargaMaxima.Focus();
                return;
            }

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sql;

                    if (veiculoId == null)
                    {
                        sql = @"INSERT INTO Veiculos (Placa, Modelo, Consumo_Medio, Carga_Maxima)
                        VALUES (@Placa, @Modelo, @ConsumoMedio, @CargaMaxima)";
                    }
                    else
                    {
                        sql = @"UPDATE Veiculos SET Placa = @Placa, Modelo = @Modelo, Consumo_Medio = @ConsumoMedio, Carga_Maxima = @CargaMaxima
                        WHERE VeiculoId = @VeiculoId";
                    }

                    using (var cmd = new SQLiteCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Placa", _txtPlaca.Text);
                        cmd.Parameters.AddWithValue("@Modelo", _txtModelo.Text);
                        cmd.Parameters.AddWithValue("@ConsumoMedio", consumo);
                        cmd.Parameters.AddWithValue("@CargaMaxima", carga);

                        if (veiculoId != null)
                            cmd.Parameters.AddWithValue("@VeiculoId", veiculoId);

                        cmd.ExecuteNonQuery();

                        string mensagem = veiculoId == null ? "Veículo salvo com sucesso!" : "Veículo atualizado com sucesso!";
                        MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LimparCampos();
                        AtualizarGrid();
                        _veiculoIdEmEdicao = null;
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


        public void Editar(int veiculoId)
        {
            _veiculoIdEmEdicao = veiculoId; // define modo edição

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sqlSelect = "SELECT Placa, Modelo, Consumo_Medio, Carga_Maxima FROM Veiculos WHERE VeiculoId = @VeiculoId";

                    using (var cmd = new SQLiteCommand(sqlSelect, connection))
                    {
                        cmd.Parameters.AddWithValue("@VeiculoId", veiculoId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _txtPlaca.Text = reader["Placa"].ToString();
                                _txtModelo.Text = reader["Modelo"].ToString();
                                _txtConsumoMedio.Text = reader["Consumo_Medio"].ToString();
                                _txtCargaMaxima.Text = reader["Carga_Maxima"].ToString();
                                
                                
                                 _lbl_btn_exVeic.Text = "Cancelar";
                                // 🟡 Altera o botão de "Excluir" para "Cancelar"
                            }
                            else
                            {
                                MessageBox.Show("Veículo não encontrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            _veiculoIdEmEdicao = null;
            _lbl_btn_exVeic.Text = "Excluir";
            _lbl_btn_veic.Text = "Incluir";// volta o texto do botão pro original
        }


    }
}