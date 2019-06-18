using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _18181_18185_Projeto3ED
{
    public partial class Form1 : Form
    {
        private Arvore<Cidade> cidades = new Arvore<Cidade>();
        int quantosdados = 0;
        private FilaLista<Caminho> caminhos = new FilaLista<Caminho>();
        bool podeMostrar = false;
        Cidade origem, destino;
        int[,] rotasMatriz;
        public Form1()
        {
            InitializeComponent();
        }

        private void exibirDgv(Rota rota, DataGridView onde)
        {
            int quantasCidades = rota.CodigosCidade().Length;

            onde.RowCount++;

            if (onde.ColumnCount < quantasCidades)
                onde.ColumnCount = quantasCidades;    
                
            for(int a = 0; a < quantasCidades; a++)
            {
                onde[a, onde.RowCount - 1].Value = rota.CodigosCidade()[a]; 
            }
        }

        private void TxtCaminhos_DoubleClick(object sender, EventArgs e)
        {

        }

        private ListaSimples<Rota> obterRotas(Cidade origem, Cidade destino)
        {
            int codOrigem = origem.Cod;
            int codDestino = destino.Cod;
            ListaSimples<Rota> caminhos = new ListaSimples<Rota>();

            achaProx(ref caminhos, codOrigem, codDestino);

            return caminhos;
        }

        private void achaProx(ref ListaSimples<Rota> lista, int codO, int codD)
        {
            for (int i = 0; i < quantosdados ; i++)
            {
                if(rotasMatriz[i,0] == codO)
                    for(int a = 0; a < quantosdados; a++)
                    {
                        ListaSimples<Caminho> novaRota = new ListaSimples<Caminho>();

                        if (rotasMatriz[i,a] > 0)
                        {
                            novaRota.InserirAntesDoInicio(new Caminho(rotasMatriz[i,0],rotasMatriz[a,0],rotasMatriz[i,a]));

                            if (rotasMatriz[0, a] == codD)
                            {
                                Rota RotaPronta = new Rota();
                                while (!novaRota.EstaVazia)
                                {
                                    RotaPronta.AdicionarCaminho(novaRota.Primeiro.Info);
                                    novaRota.Remover(novaRota.Primeiro.Info);
                                }
                                lista.InserirAposFim(RotaPronta);
                            }
                            else
                            {
                                achaProx(ref lista, rotasMatriz[a, 0], codD);
                            }
                        }
                        else
                        if (a == quantosdados)
                        {
                            novaRota.Remover(novaRota.Ultimo.Info);
                            achaProx(ref lista, novaRota.Ultimo.Info.CodOrigem, codD);
                        }
                    }
            }
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            dataGridView1.RowCount = 0;
            dataGridView1.ColumnCount = 6;

            dataGridView2.RowCount = 0;
            dataGridView2.ColumnCount = 6;

            if (destino != origem)
            {
                ListaSimples<Rota> rotas = obterRotas(origem, destino);

                rotas.Atual = rotas.Primeiro;

                Rota nova, menor = new Rota();
                menor.AdicionarCaminho(new Caminho(-1, -1, int.MaxValue));
                while (rotas.Atual != null)
                {
                    nova = rotas.Atual.Info;
                    if (nova.DistanciaTotal() < menor.DistanciaTotal())
                        menor = nova;
                    
                    exibirDgv(nova, dataGridView1);
                    rotas.Atual = rotas.Atual.Prox;
                }
                exibirDgv(menor, dataGridView2);
                podeMostrar = true;
                
            }
            else
                MessageBox.Show("Você já está onde você quer chegar.");

            marcarOrigem();
            marcarDestino();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lerArquivos();
        }

        public void lerArquivos()
        {
            StreamReader leitor = new StreamReader("Z:\\Projeto3\\CidadesMarte.txt", Encoding.UTF7, true);

            string linha = leitor.ReadLine();

            cidades.Raiz = new NoArvore<Cidade>(new Cidade(linha));

            while (!leitor.EndOfStream)
            {
                linha = leitor.ReadLine();
                cidades.Incluir(new Cidade(linha));
            }

            leitor.Close();

            rotasMatriz = new int[cidades.QuantosDados + 1, cidades.QuantosDados + 1];

            leitor = new StreamReader("Z:\\Projeto3\\CidadesMarteOrdenado.txt", Encoding.UTF7, true);

            int indice = 1;

            rotasMatriz[0, 0] = -1;

            while (!leitor.EndOfStream)
            {
                linha = leitor.ReadLine();
                Cidade nova = new Cidade(linha);
                cidades.Atual = new NoArvore<Cidade>(nova);
             
                rotasMatriz[indice, 0] = nova.Cod;
                rotasMatriz[0, indice] = nova.Cod;

                lsbOrigem.Items.Add(nova.Cod + " - " + nova.Nome);
                lsbDestino.Items.Add(nova.Cod + " - " + nova.Nome);

                indice++;
            }

            leitor.Close();

            leitor = new StreamReader("Z:\\Projeto3\\CaminhosEntreCidadesMarte.txt", Encoding.UTF7, true);

            while (!leitor.EndOfStream)
            {
                linha = leitor.ReadLine();
                Caminho novo = new Caminho(linha);
                caminhos.InserirAposFim(novo);

                for(int i = 1; i < cidades.QuantosDados + 1; i++)
                {
                    if(i - 1 == novo.CodOrigem)
                    {
                        for(int a = 1; a < cidades.QuantosDados + 1; a++)
                        {
                            if(a - 1 == novo.CodDestino)
                            {
                                rotasMatriz[i, a] = novo.Distancia;
                            }
                        }
                    }
                }
            }

            leitor.Close();
            quantosdados = cidades.QuantosDados;
            cidades.OndeExibir = tpArvore;
        }

        private void marcarOrigem()
        {
            if (lsbOrigem.SelectedIndex >= 0)
            {
                int codSelecionado = lsbOrigem.SelectedIndex;

                SolidBrush brush = new SolidBrush(Color.Blue);

                Graphics g = pbMapa.CreateGraphics();

                RectangleF retangulo = new RectangleF(origem.Y * pbMapa.Width / 4096, origem.X * pbMapa.Height / 2048, 8, 8);
                g.FillEllipse(brush, retangulo);
            }
        }

        private void marcarDestino()
        {
            if (lsbDestino.SelectedIndex >= 0)
            {
                int codSelecionado = lsbDestino.SelectedIndex;

                SolidBrush brush = new SolidBrush(Color.Red);

                Graphics g = pbMapa.CreateGraphics();

                RectangleF retangulo = new RectangleF(destino.Y * pbMapa.Width / 4096, destino.X * pbMapa.Height / 2048, 8, 8);
                g.FillEllipse(brush, retangulo);
            }
        }

        private void lsbOrigem_SelectedIndexChanged(object sender, EventArgs e)
        {
            int codSelecionado = lsbOrigem.SelectedIndex;

            origem = cidades.get(new Cidade(codSelecionado));

            pbMapa.Refresh();

            marcarOrigem();
            marcarDestino();
        }

        private void lsbDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            int codSelecionado = lsbDestino.SelectedIndex;

            destino = cidades.get(new Cidade(codSelecionado));

            pbMapa.Refresh();

            marcarDestino();
            marcarOrigem();
        }

        private void tpArvore_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            desenhaArvore(true, cidades.Raiz, (int)tpArvore.Width / 2, 0, Math.PI / 2,
                                 Math.PI / 3.5, 450, g);
        }

        private void marcaCidades(NoArvore<Cidade> att, PaintEventArgs e)
        {
            if (att != null)
            {
                SolidBrush brush = new SolidBrush(Color.Black);

                RectangleF retangulo = new RectangleF(  att.Info.Y * pbMapa.Width / 4096, att.Info.X * pbMapa.Height / 2048, 8, 8);

                e.Graphics.FillEllipse(brush, retangulo);
                e.Graphics.DrawString(Convert.ToString(att.Info.Nome), new Font("Comic Sans", 9),
                             new SolidBrush(Color.Black), att.Info.Y * pbMapa.Width / 4096, att.Info.X * pbMapa.Height / 2048 - 13);

                marcaCidades(att.Dir, e);
                marcaCidades(att.Esq, e);
            }
        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            marcaCidades(cidades.Raiz, e);
        }

        public void tracaCaminho(int xOrigem, int yOrigem, int xDestino, int yDestino)
        {
            Pen caneta = new Pen(Color.Blue);

            caneta.Width = 4;

            Graphics g = pbMapa.CreateGraphics();

            g.DrawLine(caneta, xOrigem, yOrigem, xDestino, yDestino);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (podeMostrar == true)
            {
                pbMapa.Refresh();

                Cidade origem, destino;

                int codCidade = 0, codCidade2 = 0;

                for (int i = 0; i < dataGridView1.ColumnCount; i += 2)
                {
                    if (dataGridView1[i + 1, dataGridView1.CurrentRow.Index].Value != null)
                    {
                        codCidade = (int)dataGridView1[i, dataGridView1.CurrentRow.Index].Value;
                        codCidade2 = (int)dataGridView1[i + 1, dataGridView1.CurrentRow.Index].Value;

                        origem = cidades.get(new Cidade(codCidade));
                        destino = cidades.get(new Cidade(codCidade2));

                        tracaCaminho(origem.Y * pbMapa.Width / 4096, origem.X * pbMapa.Height / 2048, destino.Y * pbMapa.Width / 4096, destino.X * pbMapa.Height / 2048);
                        marcarOrigem();
                        marcarDestino();
                    }

                    podeMostrar = false;
                }
            }
        }

        private void desenhaArvore(bool primeiraVez, NoArvore<Cidade> raiz,
                   int x, int y, double angulo, double incremento,
                   double comprimento, Graphics g)
        {
            int xf, yf;
            if (raiz != null)
            {
                Pen caneta = new Pen(Color.Red);
                xf = (int)Math.Round(x + Math.Cos(angulo) * comprimento);
                yf = (int)Math.Round(y + Math.Sin(angulo) * comprimento);
                if (primeiraVez)
                    yf = 25;
                g.DrawLine(caneta, x, y, xf, yf);
                // sleep(100);
                desenhaArvore(false, raiz.Esq, xf, yf, Math.PI / 2 + incremento,
                                                 incremento * 0.65, comprimento * 0.75, g);
                desenhaArvore(false, raiz.Dir, xf, yf, Math.PI / 2 - incremento,
                                                  incremento * 0.65, comprimento * 0.75, g);
                // sleep(100);
                SolidBrush preenchimento = new SolidBrush(Color.Black);
                g.FillEllipse(preenchimento, xf - 30, yf - 30, 110, 80);
                g.DrawString(Convert.ToString(raiz.Info.Cod + " - " + raiz.Info.Nome), new Font("Comic Sans", 12),
                              new SolidBrush(Color.White), xf - 21, yf + 10);
            }
        }
    }
}