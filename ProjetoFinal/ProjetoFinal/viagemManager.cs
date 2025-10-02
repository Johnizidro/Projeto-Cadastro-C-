using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ProjetoFinal
{
    public class ViagemManager
    {
        private ComboBox _cbVeiculo;
        private ComboBox _cbMotorista;
        private ComboBox _cbRota;
        private DateTimePicker _dtpDataSaida;
        private DateTimePicker _dtpDataChegada;
        private TextBox _txtSituacao; // ou ComboBox, se preferir
        private DataGridView _grid;

        public ViagemManager(
            ComboBox cbVeiculo,
            ComboBox cbMotorista,
            ComboBox cbRota,
            DateTimePicker dtpDataSaida,
            DateTimePicker dtpDataChegada,
            TextBox txtSituacao, // se usar combo, trocar aqui também
            DataGridView grid)
        {
            _cbVeiculo = cbVeiculo;
            _cbMotorista = cbMotorista;
            _cbRota = cbRota;
            _dtpDataSaida = dtpDataSaida;
            _dtpDataChegada = dtpDataChegada;
            _txtSituacao = txtSituacao;
            _grid = grid;
        }

        public void CarregarCombos()
        {
            CarregarMotoristas();
            CarregarVeiculos();
            CarregarRotas();
        }

        private void CarregarMotoristas()
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT MotoristaId, Nome FROM Motoristas ORDER BY Nome";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            _cbMotorista.DataSource = dt;
                            _cbMotorista.DisplayMember = "Nome";
                            _cbMotorista.ValueMember = "MotoristaId";
                            _cbMotorista.SelectedIndex = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar motoristas: " + ex.Message);
            }
        }

        private void CarregarVeiculos()
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT VeiculoId, Placa FROM Veiculos ORDER BY Placa";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            _cbVeiculo.DataSource = dt;
                            _cbVeiculo.DisplayMember = "Placa";
                            _cbVeiculo.ValueMember = "VeiculoId";
                            _cbVeiculo.SelectedIndex = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar veículos: " + ex.Message);
            }
        }

        private void CarregarRotas()
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT RotaId, Origem FROM Rotas ORDER BY Origem";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            _cbRota.DataSource = dt;
                            _cbRota.DisplayMember = "Origem";
                            _cbRota.ValueMember = "RotaId";
                            _cbRota.SelectedIndex = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar rotas: " + ex.Message);
            }
        }

        public void Salvar(int? viagemId = null)
        {
            if (_cbVeiculo.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um veículo.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_cbMotorista.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um motorista.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_cbRota.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione uma rota.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_dtpDataChegada.Value.Date < _dtpDataSaida.Value.Date)
            {
                MessageBox.Show("Data de chegada não pode ser anterior à data de saída.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_txtSituacao.Text))
            {
                MessageBox.Show("Informe a situação da viagem.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtSituacao.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(_txtSituacao.Text))
            {
                MessageBox.Show("Informe a situação da viagem.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtSituacao.Focus();
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(_txtSituacao.Text.Trim(), @"^[a-zA-ZÀ-ÿ\s]+$"))
            {
                MessageBox.Show("A situação deve conter apenas letras.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtSituacao.Focus();
                return;
            }


            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql;

                    if (viagemId == null)
                    {
                        sql = @"
                    INSERT INTO Viagens (VeiculoId, MotoristaId, RotaId, Data_Saida, Data_Chegada, Situacao)
                    VALUES (@VeiculoId, @MotoristaId, @RotaId, @DataSaida, @DataChegada, @Situacao)";
                    }
                    else
                    {
                        sql = @"
                    UPDATE Viagens SET
                        VeiculoId = @VeiculoId,
                        MotoristaId = @MotoristaId,
                        RotaId = @RotaId,
                        Data_Saida = @DataSaida,
                        Data_Chegada = @DataChegada,
                        Situacao = @Situacao
                    WHERE ViagemId = @ViagemId";
                    }

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@VeiculoId", _cbVeiculo.SelectedValue);
                        cmd.Parameters.AddWithValue("@MotoristaId", _cbMotorista.SelectedValue);
                        cmd.Parameters.AddWithValue("@RotaId", _cbRota.SelectedValue);
                        cmd.Parameters.AddWithValue("@DataSaida", _dtpDataSaida.Value.Date);
                        cmd.Parameters.AddWithValue("@DataChegada", _dtpDataChegada.Value.Date);
                        cmd.Parameters.AddWithValue("@Situacao", _txtSituacao.Text.Trim());

                        if (viagemId != null)
                            cmd.Parameters.AddWithValue("@ViagemId", viagemId);

                        cmd.ExecuteNonQuery();

                        string msg = viagemId == null
                            ? "Viagem cadastrada com sucesso!"
                            : "Viagem atualizada com sucesso!";

                        MessageBox.Show(msg, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LimparCampos();
                        AtualizarGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar viagem: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void Consultar()
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string select = @"
                        SELECT v.ViagemId, ve.Placa, m.Nome, r.Origem, v.Data_Saida, v.Data_Chegada, v.Situacao
                        FROM Viagens v
                        INNER JOIN Veiculos ve ON v.VeiculoId = ve.VeiculoId
                        INNER JOIN Motoristas m ON v.MotoristaId = m.MotoristaId
                        INNER JOIN Rotas r ON v.RotaId = r.RotaId";

                    using (var cmd = new SQLiteCommand(select, conn))
                    {
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            _grid.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao consultar viagens: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Excluir(int viagemId)
        {
            if (MessageBox.Show("Deseja realmente excluir esta viagem?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string delete = "DELETE FROM Viagens WHERE ViagemId = @ViagemId";

                    using (var cmd = new SQLiteCommand(delete, conn))
                    {
                        cmd.Parameters.AddWithValue("@ViagemId", viagemId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Viagem excluída com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    AtualizarGrid();
                    LimparCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir viagem: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Editar(int viagemId)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"SELECT * FROM Viagens WHERE ViagemId = @ViagemId";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ViagemId", viagemId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _cbVeiculo.SelectedValue = Convert.ToInt32(reader["VeiculoId"]);
                                _cbMotorista.SelectedValue = Convert.ToInt32(reader["MotoristaId"]);
                                _cbRota.SelectedValue = Convert.ToInt32(reader["RotaId"]);
                                _dtpDataSaida.Value = Convert.ToDateTime(reader["Data_Saida"]);
                                _dtpDataChegada.Value = Convert.ToDateTime(reader["Data_Chegada"]);
                                _txtSituacao.Text = reader["Situacao"].ToString();

                                // Aqui você não precisa armazenar o ID. Você passará ele depois ao chamar Salvar(viagemId)
                            }
                            else
                            {
                                MessageBox.Show("Viagem não encontrada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar viagem para edição: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void AtualizarGrid()
        {
            Consultar();
        }

        public void LimparCampos()
        {
            _cbVeiculo.SelectedIndex = -1;
            _cbMotorista.SelectedIndex = -1;
            _cbRota.SelectedIndex = -1;
            _dtpDataSaida.Value = DateTime.Today;
            _dtpDataChegada.Value = DateTime.Today;
            _txtSituacao.Clear();
        }
    }
}
