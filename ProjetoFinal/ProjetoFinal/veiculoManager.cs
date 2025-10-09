using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Globalization;

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

           FormatarValor(_txtConsumoMedio); // Formatar valor do consumo
            if (!ValidarConsumoCarga(_txtConsumoMedio)) // Validar o valor do consumo
            {
                _txtConsumoMedio.Focus();
                return;
            }

            // Carga Máxima: formatação e validação do campo
            FormatarValor(_txtCargaMaxima); // Formatar valor da carga
            if (!ValidarConsumoCarga(_txtCargaMaxima)) // Validar o valor da carga
            {
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
                        cmd.Parameters.AddWithValue("@ConsumoMedio", _txtConsumoMedio.Text.Replace(",", ".")); // Converte a vírgula para ponto
                        cmd.Parameters.AddWithValue("@CargaMaxima", _txtCargaMaxima.Text.Replace(",", "."));

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

        private void FormatarValor(TextBox textBox)
        {
            // Obter o texto atual sem caracteres inválidos (apenas números e vírgulas)
            string valor = textBox.Text;
            valor = valor.Replace(",", "").Replace(".", ""); // Remover pontos e vírgulas antigos

            // Verifica se há algum valor e limita o tamanho total para 4 caracteres
            if (valor.Length > 4)
            {
                valor = valor.Substring(0, 4); // Limita a 4 caracteres
            }

            // Adiciona a vírgula na posição correta, caso o texto tenha 3 ou mais caracteres
            if (valor.Length > 2)
            {
                valor = valor.Insert(valor.Length - 2, ",");
            }

            // Atualiza o texto do TextBox com a formatação correta
            textBox.Text = valor;

            // Move o cursor para o final, para facilitar a digitação contínua
            textBox.SelectionStart = textBox.Text.Length;
        }

        private bool ValidarConsumoCarga(TextBox textBox)
        {
            // Tenta converter o texto para um decimal, considerando a vírgula
            if (decimal.TryParse(textBox.Text, NumberStyles.Any, new CultureInfo("pt-BR"), out decimal valor))
            {
                // Verifica se o valor está dentro dos limites: maior que 0 e até 9999,99
                if (valor > 0 && valor <= 9999.99M)
                {
                    return true;
                }
            }

            MessageBox.Show("O valor deve estar no formato correto (XX,XX) e ser maior que 0.", "Erro de Formatação", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
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