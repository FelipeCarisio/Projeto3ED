using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18181_18185_Projeto3ED
{
    class Rota
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
            int i ;
            for ( i = 0; i < Caminhos.Tamanho(); i++)
            {
                vet[i] = vetC[i].CodOrigem;
            }
            vet[i] = vetC[i].CodDestino;
            return vet;
        }

    }
}
