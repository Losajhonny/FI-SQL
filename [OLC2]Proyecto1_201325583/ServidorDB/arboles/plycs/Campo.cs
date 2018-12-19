using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.plycs
{
    class Campo
    {
        private string texto;
        private string valor;
        private int tipo;
        private int line;
        private int colm;

        public Campo(string texto, string valor, int tipo,
            int line, int colm)
        {
            this.texto = texto;
            this.valor = valor;
            this.tipo = tipo;
            this.line = line;
            this.colm = colm;
        }

        public string Texto { get => texto; set => texto = value; }
        public string Valor { get => valor; set => valor = value; }
        public int Tipo { get => tipo; set => tipo = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }
    }
}
