using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ProjetoFinal
{
    public class PrecoCombustivelManager
    {

        private int? _precoIdEmEdicao = null;
        private ComboBox _cbCombustivel;
        private TextBox _txtPreco;
        private DateTimePicker _dtpDataConsulta;
        private DataGridView _grid;
        private string _connectionString = @"Data Source=C:\Users\joão\ProjetoFinal.db;Version=3;";
        private Label _lbl_cancelar_pc;
        private Label _lbl_salvar_pc;

        public PrecoCombustivelManager(
            ComboBox cbCombustivel,
            TextBox txtPreco,
            DateTimePicker dtpDataConsulta,
            DataGridView grid, 
            Label lbl_cancelar_pc, 
            Label lbl_salvar_pc)
        {
            _cbCombustivel = cbCombustivel;
            _txtPreco = txtPreco;
            _dtpDataConsulta = dtpDataConsulta;
            _grid = grid;
            _lbl_cancelar_pc = lbl_cancelar_pc;
            _lbl_salvar_pc = lbl_salvar_pc;
        }

        public void Salvar(int? precoId = null)
        {
            if (_cbCombustivel.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione o tipo de combustível!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Formatar e validar preço
            FormatarValor(_txtPreco);
            if (!ValidarPreco(_txtPreco))
            {
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

                    if (precoId == null) // novo registro
                    {
                        sql = @"INSERT INTO Preco_Combustivel (Combustivel, Preco, Data_Consulta)
                VALUES (@Combustivel, @Preco, @DataConsulta)";
                    }
                    else // atualização
                    {
                        sql = @"UPDATE Preco_Combustivel 
                SET Combustivel = @Combustivel, Preco = @Preco, Data_Consulta = @DataConsulta
                WHERE PrecoId = @PrecoId";
                    }

                    using (var cmd = new SQLiteCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Combustivel", _cbCombustivel.SelectedItem?.ToString() ?? "");
                        cmd.Parameters.AddWithValue("@Preco", decimal.Parse(_txtPreco.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture));
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
        private void FormatarValor(TextBox textBox)
        {
            string valor = textBox.Text;
            valor = valor.Replace(",", "").Replace(".", ""); // Remove pontos e vírgulas

            if (valor.Length > 4)
            {
                valor = valor.Substring(0, 4);
            }

            if (valor.Length > 2)
            {
                valor = valor.Insert(valor.Length - 2, ",");
            }

            textBox.Text = valor;
            textBox.SelectionStart = textBox.Text.Length;
        }

        private bool ValidarPreco(TextBox textBox)
        {
            if (decimal.TryParse(textBox.Text, NumberStyles.Any, new CultureInfo("pt-BR"), out decimal valor))
            {
                if (valor > 0 && valor <= 9999.99M)
                {
                    return true;
                }
            }

            MessageBox.Show("O preço deve estar no formato correto (XX,XX) e ser maior que 0.", "Erro de Formatação", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
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
            _precoIdEmEdicao = precoId;
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

                                _lbl_cancelar_pc.Text = "Cancelar";
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
            _precoIdEmEdicao = null;
            _lbl_cancelar_pc.Text = "Excluir";
            _lbl_salvar_pc.Text = "Incluir";// volta o texto do botão pro original
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
