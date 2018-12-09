using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.analizadores.usql;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;

namespace ServidorDB.arboles.usql.Expresiones.Aritmetica
{
    class NodoUMenos : NodoExp
    {
        public NodoUMenos(NodoExp izq, int line, int colm) : base(izq, line, colm) { }

        public override object ejecutar(Entorno ent)
        {
            Resultado r1 = (izq != null) ? (Resultado)izq.ejecutar(ent) : null;

            if(r1.Tipo == Constante.INTEGER)
            {
                int v = Convert.ToInt32(r1.Valor);
                v = v * -1;
                r1.Valor = v.ToString();
                return r1;
            }
            else if (r1.Tipo == Constante.DOUBLE)
            {
                double v = Convert.ToDouble(r1.Valor, System.Globalization.CultureInfo.InvariantCulture);
                v = v * -1;
                r1.Valor = v.ToString();
                return r1;
            }
            else
            {
                if(r1.Tipo == Constante.ERROR)
                {
                    string descripcion = "Tipo de operando incorrecto para el operador '-'";
                    uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                }
                r1.Tipo = Constante.ERROR;
                r1.Valor = "";
                return r1;
            }
        }
    }
}
