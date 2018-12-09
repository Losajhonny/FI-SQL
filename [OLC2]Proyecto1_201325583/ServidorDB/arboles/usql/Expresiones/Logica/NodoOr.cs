using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql.Expresiones.Logica
{
    class NodoOr : NodoExp
    {
        public NodoOr(NodoExp izq, NodoExp der, int line, int colm) : base(izq, der, line, colm)
        {
        }
    }
}
