using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.tabla_simbolos
{
    class Simbolo
    {
        private int tipo;
        private string id;
        private object valor;

        public Simbolo(int tipo, string id, object valor)
        {
            this.tipo = tipo;
            this.id = id;
            this.valor = valor;
        }

        public int Tipo { get => tipo; set => tipo = value; }
        public string Id { get => id; set => id = value; }
        public object Valor { get => valor; set => valor = value; }
    }
}
