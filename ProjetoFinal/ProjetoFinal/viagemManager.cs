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

        public void Salvar()
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

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string insert = @"
                        INSERT INTO Viagens (VeiculoId, MotoristaId, RotaId, Data_Saida, Data_Chegada, Situacao)
                        VALUES (@VeiculoId, @MotoristaId, @RotaId, @DataSaida, @DataChegada, @Situacao)";

                    using (var cmd = new SQLiteCommand(insert, conn))
                    {
                        cmd.Parameters.AddWithValue("@VeiculoId", _cbVeiculo.SelectedValue);
                        cmd.Parameters.AddWithValue("@MotoristaId", _cbMotorista.SelectedValue);
                        cmd.Parameters.AddWithValue("@RotaId", _cbRota.SelectedValue);
                        cmd.Parameters.AddWithValue("@DataSaida", _dtpDataSaida.Value.Date);
                        cmd.Parameters.AddWithValue("@DataChegada", _dtpDataChegada.Value.Date);
                        cmd.Parameters.AddWithValue("@Situacao", _txtSituacao.Text.Trim());

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Viagem cadastrada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
