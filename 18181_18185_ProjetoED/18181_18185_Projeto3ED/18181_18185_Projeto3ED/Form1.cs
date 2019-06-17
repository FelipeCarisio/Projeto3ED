﻿using System;
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
        private ListaSimples<Caminho> caminhos = new ListaSimples<Caminho>();
        Cidade origem, destino;
        public Form1()
        {
            InitializeComponent();
        }

        private void TxtCaminhos_DoubleClick(object sender, EventArgs e)
        {

        }

        private Rota[] obterRotas(Cidade origem, Cidade destino)
        {
            return null;
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (destino != origem)
            {
                Rota[] rotas = obterRotas(origem, destino);
                int menor = int.MaxValue;
                Rota m = null;
                foreach (Rota r in rotas)
                {
                    if (r.DistanciaTotal() < menor)
                    {
                        menor = r.DistanciaTotal();
                        m = r;
                    }
                    dataGridView1.RowCount++;
                    int col = 0;
                    foreach (int c in r.CodigosCidade())
                    {
                        dataGridView1[col, dataGridView1.RowCount - 1].Value = cidades.get(new Cidade(c)).Nome;
                        col++;
                    }
                }
                dataGridView2.RowCount++;
                int coluna = 0;
                foreach (int c in m.CodigosCidade())
                {
                    dataGridView2[coluna, 0].Value = cidades.get(new Cidade(c)).Nome;
                }
            }
            else
                MessageBox.Show("Você já está onde você quer chegar.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lerArquivos();
        }

        public void lerArquivos()
        {
            StreamReader leitor = new StreamReader("Z:\\Projeto3\\18181_18185_ProjetoED\\18181_18185_Projeto3ED\\18181_18185_Projeto3ED\\imagens e arquivos\\CidadesMarte.txt", Encoding.UTF7, true);
            string linha = leitor.ReadLine();
            cidades.Raiz = new NoArvore<Cidade>(new Cidade(linha));
            while (!leitor.EndOfStream)
            {
                linha = leitor.ReadLine();
                cidades.Incluir(new Cidade(linha));
            }

            leitor = new StreamReader("Z:\\Projeto3\\18181_18185_ProjetoED\\18181_18185_Projeto3ED\\18181_18185_Projeto3ED\\imagens e arquivos\\CaminhosEntreCidadesMarte.txt", Encoding.UTF7, true);

            while (!leitor.EndOfStream)
            {
                linha = leitor.ReadLine();
                caminhos.InserirAposFim(new Caminho(linha));
            }

            leitor.Close();

            cidades.OndeExibir = tpArvore;
        }

        private void tpArvore_Click(object sender, EventArgs e)
        {

        }

        private void lsbOrigem_SelectedIndexChanged(object sender, EventArgs e)
        {
            origem = cidades.get(new Cidade(lsbDestino.SelectedIndex));

        }

        private void lsbDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            destino = cidades.get(new Cidade(lsbDestino.SelectedIndex));
        }

        private void pbMapa_Click(object sender, EventArgs e)
        {

        }

        private void tpArvore_Enter(object sender, EventArgs e)
        {

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
                g.DrawString(Convert.ToString(raiz.Info.Nome), new Font("Comic Sans", 12),
                              new SolidBrush(Color.White), xf - 21, yf + 10);
            }
        }
    }
}