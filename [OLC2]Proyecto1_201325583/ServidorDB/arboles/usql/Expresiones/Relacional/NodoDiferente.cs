using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.tabla_simbolos;

namespace ServidorDB.arboles.usql.Expresiones.Relacional
{
    class NodoDiferente : NodoExp
    {
        public NodoDiferente(NodoExp izq, NodoExp der, int line, int colm) : base(izq, der, line, colm) { }

        public override object ejecutar(Entorno ent)
        {
            Resultado r1 = (izq != null) ? (Resultado)izq.ejecutar(ent) : null;
            Resultado r2 = (der != null) ? (Resultado)der.ejecutar(ent) : null;

            return base.ejecutar(ent);
        }
    }
}
