using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18181_18185_Projeto3ED
{
    class Rota : IComparable<Rota>, IGravarEmArquivo
    {
        private FilaLista<Caminho> Caminhos;
        
        public Rota()
        {
            Caminhos = new FilaLista<Caminho>();
        }

        public void AdicionarCaminho(Caminho novo)
        {
            Caminhos.Enfileirar(novo);
        }

        public int DistanciaTotal()
        {
            int d = 0;
            Caminho[] vet = Caminhos.ToArray();
            for(int i = 0; i < Caminhos.Tamanho(); i++)
            {
                d += vet[i].Distancia;
            }
            return d;
        }

        public int PrecoTotal()
        {
            int p = 0;
            Caminho[] vet = Caminhos.ToArray();
            for (int i = 0; i < Caminhos.Tamanho(); i++)
            {
                p += vet[i].Preco;
            }
            return p;
        }

        public int TempoTotal()
        {
            int t = 0;
            Caminho[] vet = Caminhos.ToArray();
            for (int i = 0; i < Caminhos.Tamanho(); i++)
            {
                t += vet[i].Tempo;
            }
            return t;
        }

        public int[] CodigosCidade()
        {
            int[] vet = new int[Caminhos.Tamanho() + 1];
            Caminho[] vetC = Caminhos.ToArray();
            int i, f;

            for ( i = 0, f = 1; i < Caminhos.Tamanho(); i += 2, f++)
            {
                if (f % 2 == 0)
                    vet[i + 1] = vetC[f - 1].CodDestino;
                else
                {
                    vet[i] = vetC[f - 1].CodOrigem;
                    vet[i + 1] = vetC[f - 1].CodDestino;
                }
            }

            return vet;
        }

        public string ParaArquivo()
        {
            string r = "";
            foreach(Caminho s in Caminhos.ToArray())
            {
                r += s.ToString() + ", ";
            }

            return r;
        }

        public int CompareTo(Rota outro)
        {
            return this.DistanciaTotal() - outro.DistanciaTotal();
        }
    }
}
