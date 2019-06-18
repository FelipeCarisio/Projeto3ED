using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18181_18185_Projeto3ED
{
    public class Caminho : IComparable<Caminho>, IGravarEmArquivo
    {
        private int codOrigem, codDestino, distancia, preco, tempo,
                    inicioCodO = 0, tamanhoCodO = 3,
                    inicioCodD = 3, tamanhoCodD = 3,
                    inicioD = 6, tamanhoD = 5,
                    inicioT = 11, tamanhoT = 4,
                    inicioP = 15, tamanhoP = 5;

        public int CodOrigem { get => codOrigem; set => codOrigem = value; }
        public int CodDestino { get => codDestino; set => codDestino = value; }
        public int Distancia { get => distancia; set => distancia = value; }
        public int Preco { get => preco; set => preco = value; }
        public int Tempo { get => tempo; set => tempo = value; }

        public Caminho (int codO, int codD, int D)
        {
            this.CodDestino = codD;
            this.CodOrigem = codO;
            this.Distancia = D;
        }
        public Caminho(string Linha)
        {
            this.CodOrigem = int.Parse(Linha.Substring(inicioCodO, tamanhoCodO));
            this.CodDestino = int.Parse(Linha.Substring(inicioCodD, tamanhoCodD));
            this.Tempo = int.Parse(Linha.Substring(inicioT, tamanhoT));
            this.Preco = int.Parse(Linha.Substring(inicioP, tamanhoP));
            this.Distancia = int.Parse(Linha.Substring(inicioD, tamanhoD));
        }

        public int CompareTo(Caminho outro)
        {
            return this.Distancia - outro.Distancia;
        }

        public int ComparaTempo(Caminho outro)
        {
            return this.Tempo - outro.Tempo;
        }

        public int ComparaPreco(Caminho outro)
        {
            return this.Preco - outro.Preco;
        }

        public string ParaArquivo()
        {
            throw new NotImplementedException();
        }
    }
}
