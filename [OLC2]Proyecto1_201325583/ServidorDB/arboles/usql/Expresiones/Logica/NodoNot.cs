using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql.Expresiones.Logica
{
    class NodoNot : NodoExp
    {
        public NodoNot(NodoExp izq, int line, int colm) : base(izq, line, colm)
        {
        }
    }
}
