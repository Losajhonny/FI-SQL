using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;

namespace ServidorDB.arboles.usql.Expresiones
{
    class NodoExp : uInstruccion
    {
        /*
         * @izq             almacena una operacion del lado izquierdo
         * @der             almacena una operacion del lado derecho
         * @line            almacena la fila de un operador o de un valor
         * @colm            almacena la columna de un operador o de un valor
         */

        protected NodoExp izq;
        protected NodoExp der;
        protected int line;
        protected int colm;

        public NodoExp(NodoExp izq, NodoExp der, int line, int colm)
        {
            this.izq = izq;
            this.der = der;
            this.line = line;
            this.colm = colm;
        }

        public NodoExp(NodoExp izq, int line, int colm)
        {
            this.izq = izq;
            this.line = line;
            this.colm = colm;
        }

        public NodoExp(int line, int colm)
        {
            this.line = line;
            this.colm = colm;
        }

        public int getBooleano(int value)
        {
            if (value >= 1)
                return 1;
            return 0;
        }

        public NodoExp Izq { get => izq; set => izq = value; }
        public NodoExp Der { get => der; set => der = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }

        public virtual object ejecutar(Entorno ent)
        {
            return null;
        }
    }
}
