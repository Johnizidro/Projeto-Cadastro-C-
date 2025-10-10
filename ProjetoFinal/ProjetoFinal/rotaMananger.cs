using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ProjetoFinal
{
    public class RotaManager
    {
        private int? _rotaIdEmEdicao = null;
        private TextBox _txtOrigem;
        private TextBox _txtDestino;
        private TextBox _txtDistancia;
        private DataGridView _grid;
        private string _connectionString = @"Data Source=C:\Users\joão\ProjetoFinal.db;Version=3;";
        private Label _lbl_cancelar_rota;
        private Label _lbl_salvar_rota;

        public RotaManager(TextBox txtOrigem, TextBox txtDestino, TextBox txtDistancia, DataGridView grid, Label lbl_cancelar_rota, Label lbl_salvar_rota)
        {
            _txtOrigem = txtOrigem;
            _txtDestino = txtDestino;
            _txtDistancia = txtDistancia;
            _grid = grid;
            _lbl_cancelar_rota = lbl_cancelar_rota;
            _lbl_salvar_rota = lbl_salvar_rota;
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
            // Validação Origem e Destino como você já tem
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

            // Formatar e validar distância
            FormatarValor(_txtDistancia);
            if (!ValidarDistancia(_txtDistancia))
            {
                _txtDistancia.Focus();
                return;
            }

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sql;

                    if (rotaId == null)
                    {
                        sql = @"INSERT INTO Rotas (Origem, Destino, Distancia)
                VALUES (@Origem, @Destino, @Distancia)";
                    }
                    else
                    {
                        sql = @"UPDATE Rotas SET Origem = @Origem, Destino = @Destino, Distancia = @Distancia
                WHERE RotaId = @RotaId";
                    }

                    using (var cmd = new SQLiteCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Origem", _txtOrigem.Text);
                        cmd.Parameters.AddWithValue("@Destino", _txtDestino.Text);
                        cmd.Parameters.AddWithValue("@Distancia", _txtDistancia.Text.Replace(",", "."));

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

        private bool ValidarDistancia(TextBox textBox)
        {
            if (decimal.TryParse(textBox.Text, NumberStyles.Any, new CultureInfo("pt-BR"), out decimal valor))
            {
                if (valor > 0 && valor <= 9999.99M)
                {
                    return true;
                }
            }

            MessageBox.Show("O valor da distância deve estar no formato correto (XX,XX) e ser maior que 0.", "Erro de Formatação", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
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
            _rotaIdEmEdicao = rotaId;
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

                                _lbl_cancelar_rota.Text = "Cancelar";
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
            _rotaIdEmEdicao = null;
            _lbl_cancelar_rota.Text = "Excluir";
            _lbl_salvar_rota.Text = "Incluir";// volta o texto do botão pro original
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
