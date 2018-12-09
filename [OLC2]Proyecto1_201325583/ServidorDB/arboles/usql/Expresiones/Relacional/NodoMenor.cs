using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql.Expresiones.Relacional
{
    class NodoMenor : NodoExp
    {
        public NodoMenor(NodoExp izq, NodoExp der, int line, int colm) : base(izq, der, line, colm)
        {
        }
    }
}
