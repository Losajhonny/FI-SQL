using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.analizadores.usql;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;

namespace ServidorDB.arboles.usql.Expresiones.Logica
{
    class NodoNot : NodoExp
    {
        public NodoNot(NodoExp izq, int line, int colm) : base(izq, line, colm) { }

        public override object ejecutar(Entorno ent)
        {
            Resultado r1 = (izq != null) ? (Resultado)izq.ejecutar(ent) : null;
            int tipo = Constante.BOOL;

            if (r1.Tipo == Constante.BOOL)
            {
                int v1 = Convert.ToInt32(r1.Valor);
                int v = (v1 == 0) ? 1 : 0;
                return new Resultado(tipo, v.ToString());
            }
            else
            {
                if (!(r1.Tipo == Constante.ERROR))
                {
                    string descripcion = "Tipos de operandos incorrectos para el operador '!'";
                    uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                }
                return new Resultado(Constante.ERROR, "");
            }
        }
    }
}
