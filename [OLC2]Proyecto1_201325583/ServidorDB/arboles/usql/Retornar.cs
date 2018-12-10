using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.tabla_simbolos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql
{
    class Retornar : uInstruccion
    {
        private NodoExp exp;
        private int line;
        private int colm;

        public Retornar(NodoExp exp, int line, int colm)
        {
            this.exp = exp;
            this.line = line;
            this.colm = colm;
        }

        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }
        public NodoExp Exp { get => exp; set => exp = value; }

        public object ejecutar(Entorno ent)
        {
            return this;
        }
    }
}
