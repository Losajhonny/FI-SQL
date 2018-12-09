using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql.Expresiones.Relacional
{
    class NodoMenIgual : NodoExp
    {
        public NodoMenIgual(NodoExp izq, NodoExp der, int line, int colm) : base(izq, der, line, colm)
        {
        }
    }
}
