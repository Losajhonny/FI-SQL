using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.tabla_simbolos
{
    class Simbolo
    {
        //solo eso hace referencia la tabla de simbolos
        //esto servira para cargar las funciones, proc, obj
        //en una tabla de simbolos
        public const int VARIABLE = 0;
        public const int FUNCION = 1;
        public const int PROCEDIMIENTO = 2;
        public const int OBJETO = 3;

        private int tipo;
        private int tipo_simbolo;
        private string id;
        private object valor;

        public Simbolo(int tipo, string id, object valor)
        {
            this.tipo = tipo;
            this.id = id;
            this.valor = valor;
        }

        public Simbolo(int tipo_simbolo, int tipo, string id, object valor)
        {
            this.tipo_simbolo = tipo_simbolo;
            this.tipo = tipo;
            this.valor = valor;
            this.id = id;
        }

        public int Tipo { get => tipo; set => tipo = value; }
        public string Id { get => id; set => id = value; }
        public object Valor { get => valor; set => valor = value; }
        public int Tipo_simbolo { get => tipo_simbolo; set => tipo_simbolo = value; }
    }
}
