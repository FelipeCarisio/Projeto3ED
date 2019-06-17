using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18181_18185_Projeto3ED
{
    class Cidade : IComparable<Cidade>
    {
        private string nome;
        private int cod, x, y,
        inicioCod = 0, tamanhoCod = 3,
        inicioNome = 3, tamanhoNome = 15,
        inicioY = 18, tamanhoY = 5,
        inicioX = 23, tamanhoX = 5;
        
        public Cidade(string linha)
        {
            this.Nome = linha.Substring(inicioNome, tamanhoNome);
            this.Cod = int.Parse(linha.Substring(inicioCod, tamanhoCod));
            this.X = int.Parse(linha.Substring(inicioX, tamanhoX).Trim(' '));
            this.Y = int.Parse(linha.Substring(inicioY, tamanhoY).Trim(' '));
        }

        public Cidade(int codP)
        {
            this.cod = codP;
        }
        

        public int Cod { get => cod; set => cod = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public string Nome { get => nome; set => nome = value; }

        public int CompareTo(Cidade other)
        {
            return this.cod - other.cod;
        }
    }
}
