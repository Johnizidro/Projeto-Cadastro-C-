using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ProjetoFinal
{
    public class PrecoCombustivelManager
    {
        private ComboBox _cbCombustivel;
        private TextBox _txtPreco;
        private DateTimePicker _dtpDataConsulta;
        private DataGridView _grid;

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

        public void Salvar()
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

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string insert = @"INSERT INTO Preco_Combustivel (Combustivel, Preco, Data_Consulta)
                                      VALUES (@Combustivel, @Preco, @DataConsulta)";

                    using (var cmd = new SQLiteCommand(insert, conn))
                    {
                        cmd.Parameters.AddWithValue("@Combustivel", _cbCombustivel.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@Preco", preco);
                        cmd.Parameters.AddWithValue("@DataConsulta", _dtpDataConsulta.Value);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Preço cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LimparCampos();
                        AtualizarGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // ✅ Método corrigido — recebe precoId como parâmetro
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
