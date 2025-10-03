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


        private int? _veiculoIdEditando = null;
        private int? _motoristaIdEditando = null;
        private int? _rotaIdEditando = null;
        private int? _precoIdEditando = null;
        private int? _viagemIdEditando = null;



        public Form_Inicial()
        {
            InitializeComponent();
            this.FormClosed += Form_Inicial_FormClosed;

            _veiculoManager = new VeiculoManager(
                txt_plac_veic,
                txt_mod_veic,
                txt_cmed_veic,
                txt_cmax_veic,
                dataGrid_veiculos,
                lbl_btn_exVeic,
                lbl_btn_veic
            );
            _motoristaManager = new MotoristaManager(
                txt_nome_mot,
                txt_tel_mot,
                txt_cnh_mot,
                dataGrid_motorista,
                lbl_cancelar_mot,
                lbl_salvar_mot
            );
            _rotaManager = new RotaManager(
                txt_origem_rota,
                txt_destino_rota,
                txt_distancia_rota,
                dataGrid_rotas,
                lbl_cancelar_rota,
                lbl_salvar_rota
            );
            _precoManager = new PrecoCombustivelManager(
                cb_combustivel,
                txt_combustivel,
                data_combustivel,
                dataGrid_pc,
                lbl_cancelar_pc,
                lbl_salvar_pc
             );
            _viagemManager = new ViagemManager(
                cb_veiculo,
                cb_motorista,  // aqui é o combo motorista
                cb_rota,
                dtp_dataSaida,
                dtp_dataChegada,
                txt_situacao,
                dataGrid_viagem,
                lbl_cancelar_viagem,
                lbl_salvar_viagem
 );

            _viagemManager.CarregarCombos();
        }


        //Eventos do Formulario Veículo

        private void btn_veiculo_salvar_Click(object sender, EventArgs e)
        {
            _veiculoManager.Salvar(_veiculoIdEditando);

            _veiculoIdEditando = null;

            lbl_btn_veic.Text = "Incluir";
            lbl_btn_exVeic.Text = "Excluir";
        }

        private void btn_veiculo_consultar_Click(object sender, EventArgs e)
        {
            _veiculoManager.Consultar();
        }

        private void btn_veiculo_excluir_Click(object sender, EventArgs e)
        {

            if (lbl_btn_exVeic.Text == "Cancelar")
            {
                _veiculoManager.CancelarEdicao(); // cancela edição
                return;
            }

            if (dataGrid_veiculos.SelectedRows.Count > 0)
            {
                int veiculoId = Convert.ToInt32(dataGrid_veiculos.SelectedRows[0].Cells["VeiculoId"].Value);
                _veiculoManager.Excluir(veiculoId);
            }
            else
            {
                MessageBox.Show("Selecione um veículo para excluir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_veiculo_editar_Click(object sender, EventArgs e)
        {
            if (dataGrid_veiculos.CurrentRow == null)
            {
                MessageBox.Show("Selecione um veículo para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _veiculoIdEditando = Convert.ToInt32(dataGrid_veiculos.CurrentRow.Cells["VeiculoId"].Value);

            _veiculoManager.Editar(_veiculoIdEditando.Value);

            // Opcional: muda o texto ou imagem do botão salvar para "Atualizar"
            lbl_btn_veic.Text = "Salvar"; ;
        }



        //Eventos do Formulario Motorista

        private void btn_motorista_consultar_Click(object sender, EventArgs e)
        {
            _motoristaManager.Consultar();
        }

        private void btn_motorista_salvar_Click(object sender, EventArgs e)
        {
            _motoristaManager.Salvar(_motoristaIdEditando);

            _motoristaIdEditando = null;

            lbl_salvar_mot.Text = "Incluir";
            lbl_cancelar_mot.Text = "Excluir";
        }

        private void btn_motorista_excluir_Click(object sender, EventArgs e)
        {

            if (lbl_cancelar_mot.Text == "Cancelar")
            {
                _motoristaManager.CancelarEdicao(); // cancela edição
                return;
            }

            if (dataGrid_motorista.SelectedRows.Count > 0)
            {
                int motoristaId = Convert.ToInt32(dataGrid_motorista.SelectedRows[0].Cells["MotoristaId"].Value);
                _motoristaManager.Excluir(motoristaId);
            }
            else
            {
                MessageBox.Show("Selecione um motorista para excluir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_motorista_editar_Click(object sender, EventArgs e)
        {
            if (dataGrid_motorista.CurrentRow == null)
            {
                MessageBox.Show("Selecione um motorista para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _motoristaIdEditando = Convert.ToInt32(dataGrid_motorista.CurrentRow.Cells["MotoristaId"].Value);

            _motoristaManager.Editar(_motoristaIdEditando.Value);

            // Opcional: muda o texto ou imagem do botão salvar para "Atualizar"
            lbl_salvar_mot.Text = "Salvar";
        }



        //Eventos do Formulario Rotas

        private void btn_rota_consultar_Click(object sender, EventArgs e)
        {
            _rotaManager.Consultar();
        }

        private void btn_rota_salvar_Click(object sender, EventArgs e)
        {
            _rotaManager.Salvar(_rotaIdEditando);

            _rotaIdEditando = null;

            lbl_salvar_rota.Text = "Incluir";
            lbl_cancelar_rota.Text = "Excluir";
        }

        private void btn_rota_excluir_Click(object sender, EventArgs e)
        {
            if (lbl_cancelar_rota.Text == "Cancelar")
            {
                _rotaManager.CancelarEdicao(); // cancela edição
                return;
            }

            if (dataGrid_rotas.SelectedRows.Count > 0)
            {
                int rotaId = Convert.ToInt32(dataGrid_rotas.SelectedRows[0].Cells["RotaId"].Value);
                _rotaManager.Excluir(rotaId);
            }
            else
            {
                MessageBox.Show("Selecione uma rota para excluir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_rota_editar_Click(object sender, EventArgs e)
        {
            if (dataGrid_rotas.CurrentRow == null)
            {
                MessageBox.Show("Selecione uma rota para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _rotaIdEditando = Convert.ToInt32(dataGrid_rotas.CurrentRow.Cells["RotaId"].Value);

            _rotaManager.Editar(_rotaIdEditando.Value);

            // Opcional: muda o texto ou imagem do botão salvar para "Atualizar"
            lbl_salvar_rota.Text = "Salvar";
        }



        //Eventos do Formulario Preço_Combustível

        private void btn_pc_salvar_Click(object sender, EventArgs e)
        {
            _precoManager.Salvar(_precoIdEditando);

            _precoIdEditando = null;

            lbl_salvar_pc.Text = "Incluir";
            lbl_cancelar_pc.Text = "Excluir";
        }

        private void btn_pc_consultar_Click(object sender, EventArgs e)
        {
            _precoManager.Consultar();
        }

        private void btn_pc_excluir_Click(object sender, EventArgs e)
        {

            if (lbl_cancelar_pc.Text == "Cancelar")
            {
                _precoManager.CancelarEdicao(); // cancela edição
                return;
            }

            if (dataGrid_pc.SelectedRows.Count > 0)
            {
                int precoId = Convert.ToInt32(dataGrid_pc.SelectedRows[0].Cells["PrecoId"].Value);
                _precoManager.Excluir(precoId);
            }
            else
            {
                MessageBox.Show("Selecione um preço para excluir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_pc_editar_Click(object sender, EventArgs e)
        {
            if (dataGrid_pc.CurrentRow == null)
            {
                MessageBox.Show("Selecione um preço para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _precoIdEditando = Convert.ToInt32(dataGrid_pc.CurrentRow.Cells["PrecoId"].Value);

            _precoManager.Editar(_precoIdEditando.Value);

            // Opcional: muda o texto ou imagem do botão salvar para "Atualizar"
            lbl_salvar_pc.Text = "Salvar";

        }



        //Eventos do Formulario Viagem

        private void btn_viagem_salvar_Click(object sender, EventArgs e)
        {
            _viagemManager.Salvar(_viagemIdEditando);

            _viagemIdEditando = null;

            lbl_salvar_viagem.Text = "Incluir";
            lbl_cancelar_viagem.Text = "Excluir";
        }

        private void btn_viagem_consultar_Click(object sender, EventArgs e)
        {
            _viagemManager.Consultar();
        }

        private void btn_viagem_excluir_Click(object sender, EventArgs e)
        {
            if (lbl_cancelar_viagem.Text == "Cancelar")
            {
                _viagemManager.CancelarEdicao(); // cancela edição
                return;
            }

            if (dataGrid_viagem.SelectedRows.Count > 0)
            {
                int viagemId = Convert.ToInt32(dataGrid_viagem.SelectedRows[0].Cells["ViagemId"].Value);
                _viagemManager.Excluir(viagemId);
            }
            else
            {
                MessageBox.Show("Selecione uma viagem para excluir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage5) // nome da sua aba de viagens
            {
                _viagemManager.CarregarCombos();
            }
        }

        private void btn_viagem_editar_Click(object sender, EventArgs e)
        {
            if (dataGrid_viagem.CurrentRow == null)
            {
                MessageBox.Show("Selecione uma viagem para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _viagemIdEditando = Convert.ToInt32(dataGrid_viagem.CurrentRow.Cells["ViagemId"].Value);

            _viagemManager.Editar(_viagemIdEditando.Value);

            // Opcional: muda o texto ou imagem do botão salvar para "Atualizar"
            lbl_salvar_viagem.Text = "Salvar";
        }

        private void Form_Inicial_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Ao fechar o Form_Inicial, mostra novamente o Form1 (perfil)
            Form1 formPerfil = new Form1();
            formPerfil.Show();
        }




        // Comandos prórpios da página 



    }
}
