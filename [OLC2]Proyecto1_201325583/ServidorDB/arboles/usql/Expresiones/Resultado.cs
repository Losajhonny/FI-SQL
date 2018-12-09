using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql.Expresiones
{
    class Resultado
    {
        private int tipo;
        private string valor;

        public Resultado(int tipo, string valor)
        {
            this.tipo = tipo;
            this.valor = valor;
        }

        public int Tipo { get => tipo; set => tipo = value; }
        public string Valor { get => valor; set => valor = value; }
    }
}
