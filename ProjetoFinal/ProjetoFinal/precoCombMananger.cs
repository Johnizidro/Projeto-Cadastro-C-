using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ProjetoFinal
{
    public class PrecoCombustivelManager
    {
        private ComboBox _cbCombustivel;
        private TextBox _txtPreco;
        private DateTimePicker _dtpDataConsulta;
        private DataGridView _grid;
        private string _connectionString = @"Data Source=C:\Users\joao.visousa\ProjetoFinal.db;Version=3;";

        public PrecoCombustivelManager(
            ComboBox cbCombustivel,
            TextBox txtPreco,
            DateTimePicker dtpDataConsulta,
            DataGridView grid)
        {
            _cbCombustivel = cbCombustivel;
            _txtPreco = txtPreco;
            _dtpDataConsulta = dtpDataConsulta;
            _grid = grid;
        }

        public void Salvar(int? precoId = null)
        {
            if (_cbCombustivel.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione o tipo de combustível!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_txtPreco.Text) || !decimal.TryParse(_txtPreco.Text, out decimal preco) || preco <= 0)
            {
                MessageBox.Show("Digite um preço válido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtPreco.Focus();
                return;
            }

            // Validação de data: só permite datas passadas
            if (_dtpDataConsulta.Value.Date >= DateTime.Today)
            {
                MessageBox.Show("A data da consulta deve ser anterior ao dia atual.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _dtpDataConsulta.Focus();
                return;
            }


            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sql;

                    if (precoId == null) // precoId é um int? que representa o ID a ser editado, ou null se for novo
                    {
                        sql = @"INSERT INTO Preco_Combustivel (Combustivel, Preco, Data_Consulta)
                    VALUES (@Combustivel, @Preco, @DataConsulta)";
                    }
                    else
                    {
                        sql = @"UPDATE Preco_Combustivel 
                    SET Combustivel = @Combustivel, Preco = @Preco, Data_Consulta = @DataConsulta
                    WHERE PrecoId = @PrecoId";
                    }

                    using (var cmd = new SQLiteCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Combustivel", _cbCombustivel.SelectedItem?.ToString() ?? "");
                        cmd.Parameters.AddWithValue("@Preco", decimal.Parse(_txtPreco.Text));
                        cmd.Parameters.AddWithValue("@DataConsulta", _dtpDataConsulta.Value);

                        if (precoId != null)
                            cmd.Parameters.AddWithValue("@PrecoId", precoId);

                        cmd.ExecuteNonQuery();

                        string mensagem = precoId == null ? "Preço salvo com sucesso!" : "Preço atualizado com sucesso!";
                        MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LimparCampos();
                        AtualizarGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar o preço de combustível: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public void Consultar()
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string select = "SELECT PrecoId, Combustivel, Preco, Data_Consulta FROM Preco_Combustivel";

                    using (var cmd = new SQLiteCommand(select, conn))
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
                MessageBox.Show("Erro ao consultar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

  
        public void Excluir(int precoId)
        {
            if (MessageBox.Show("Deseja realmente excluir este registro?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();

                    string sqlDelete = "DELETE FROM Preco_Combustivel WHERE PrecoId = @PrecoId";

                    using (var cmd = new SQLiteCommand(sqlDelete, connection))
                    {
                        cmd.Parameters.AddWithValue("@PrecoId", precoId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Registro excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    AtualizarGrid();
                    LimparCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir o registro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Editar(int precoId)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sqlSelect = "SELECT Combustivel, Preco, Data_Consulta FROM Preco_Combustivel WHERE PrecoId = @PrecoId";

                    using (var cmd = new SQLiteCommand(sqlSelect, connection))
                    {
                        cmd.Parameters.AddWithValue("@PrecoId", precoId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Preenche o ComboBox selecionando o item que corresponde ao banco
                                string combustivel = reader["Combustivel"].ToString();

                                int index = _cbCombustivel.FindStringExact(combustivel);
                                _cbCombustivel.SelectedIndex = index >= 0 ? index : -1;

                                // Preço como texto formatado
                                _txtPreco.Text = Convert.ToDecimal(reader["Preco"]).ToString("F2");

                                // Data consulta do banco convertida para DateTime
                                if (DateTime.TryParse(reader["Data_Consulta"].ToString(), out DateTime dataConsulta))
                                {
                                    _dtpDataConsulta.Value = dataConsulta;
                                }
                                else
                                {
                                    _dtpDataConsulta.Value = DateTime.Today;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Registro não encontrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            _cbCombustivel.SelectedIndex = -1;
            _txtPreco.Clear();
            _dtpDataConsulta.Value = DateTime.Today;
        }
    }
}
