using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoFinal
{
    public partial class Form_Inicial : Form
    {
        // 1. Declarar o gerenciador como campo da classe
        private VeiculoManager _veiculoManager;
        private MotoristaManager _motoristaManager;
        private RotaManager _rotaManager;
        private PrecoCombustivelManager _precoManager;
        private ViagemManager _viagemManager;


        public Form_Inicial()
        {
            InitializeComponent();

            // 2. Inicializar o gerenciador passando os controles do formulário
            _veiculoManager = new VeiculoManager(
                txt_plac_veic,
                txt_mod_veic,
                txt_cmed_veic,
                txt_cmax_veic,
                dataGrid_veiculos
            );

            _motoristaManager = new MotoristaManager(
                txt_nome_mot,
                txt_tel_mot,
                txt_cnh_mot,
                dataGrid_motorista
            );

            _rotaManager = new RotaManager(
                txt_origem_rota,
                txt_destino_rota,
                txt_distancia_rota,
                dataGrid_rotas
            );
            _precoManager = new PrecoCombustivelManager(
                cb_combustivel,
                txt_combustivel,
                data_combustivel,
                dataGrid_pc
             );

            _viagemManager = new ViagemManager(
                cb_veiculo,
                cb_motorista,  // aqui é o combo motorista
                cb_rota,
                dtp_dataSaida,
                dtp_dataChegada,
                txt_situacao,
                dataGrid_viagem
 );
            _viagemManager.CarregarCombos();


        }

        private void btn_veiculo_salvar_Click(object sender, EventArgs e)
        {
            _veiculoManager.Salvar();
        }

        private void btn_veiculo_consultar_Click(object sender, EventArgs e)
        {
            _veiculoManager.Consultar();
        }

        private void btn_veiculo_excluir_Click(object sender, EventArgs e)
        {
            if (dataGrid_veiculos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione uma linha para excluir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Supondo que a primeira coluna seja VeiculoId
            int veiculoId = Convert.ToInt32(dataGrid_veiculos.SelectedRows[0].Cells["VeiculoId"].Value);
            _veiculoManager.Excluir(veiculoId);
        }

        private void btn_motorista_consultar_Click(object sender, EventArgs e)
        {
            _motoristaManager.Consultar();
        }

        private void btn_motorista_salvar_Click(object sender, EventArgs e)
        {
            _motoristaManager.Salvar();
        }

        private void btn_motorista_excluir_Click(object sender, EventArgs e)
        {

            if (dataGrid_motorista.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um motorista para excluir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Pegando o ID do motorista selecionado no DataGridView
            int motoristaId = Convert.ToInt32(dataGrid_motorista.SelectedRows[0].Cells["MotoristaId"].Value);

            _motoristaManager.Excluir(motoristaId);
        }

        private void btn_rota_consultar_Click(object sender, EventArgs e)
        {
            _rotaManager.Consultar();
        }

        private void btn_rota_salvar_Click(object sender, EventArgs e)
        {
            _rotaManager.Salvar();
        }

        private void btn_rota_excluir_Click(object sender, EventArgs e)
        {
            if (dataGrid_rotas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione uma rota para excluir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int rotaId = Convert.ToInt32(dataGrid_rotas.SelectedRows[0].Cells["RotaId"].Value);
            _rotaManager.Excluir(rotaId);
        }

        private void btn_pc_salvar_Click(object sender, EventArgs e)
        {
            _precoManager.Salvar();
        }

        private void btn_pc_consultar_Click(object sender, EventArgs e)
        {
            _precoManager.Consultar();
        }

        private void btn_pc_excluir_Click(object sender, EventArgs e)
        {

            if (dataGrid_pc.CurrentRow != null)
            {
                int precoId = Convert.ToInt32(dataGrid_pc.CurrentRow.Cells["PrecoId"].Value);
                _precoManager.Excluir(precoId);
            }
            else
            {
                MessageBox.Show("Selecione um registro para excluir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_viagem_salvar_Click(object sender, EventArgs e)
        {
            _viagemManager.Salvar();
        }

        private void btn_viagem_consultar_Click(object sender, EventArgs e)
        {
            _viagemManager.Consultar();
        }

        private void btn_viagem_excluir_Click(object sender, EventArgs e)
        {
            if (dataGrid_viagem.CurrentRow != null)
            {
                int viagemId = Convert.ToInt32(dataGrid_viagem.CurrentRow.Cells["ViagemId"].Value);
                _viagemManager.Excluir(viagemId);
            }
            else
            {
                MessageBox.Show("Selecione uma viagem para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage5) // nome da sua aba de viagens
            {
                _viagemManager.CarregarCombos();
            }
        }
    }
}
