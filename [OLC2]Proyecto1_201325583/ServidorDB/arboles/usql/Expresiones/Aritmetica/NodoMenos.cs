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
    class NodoMenos : NodoExp
    {
        public NodoMenos(NodoExp izq, NodoExp der, int line, int colm) : base(izq, der, line, colm) { }

        public override object ejecutar(Entorno ent)
        {
            Resultado r1 = (izq != null) ? (Resultado)izq.ejecutar(ent) : null;
            Resultado r2 = (der != null) ? (Resultado)der.ejecutar(ent) : null;
            int tipo = Constante.resta[r1.Tipo, r2.Tipo];

            if(r1.Tipo == Constante.INTEGER && r2.Tipo == Constante.INTEGER)
            {
                int v1 = Convert.ToInt32(r1.Valor);
                int v2 = Convert.ToInt32(r2.Valor);
                return new Resultado(tipo, (v1 - v2).ToString());
            }
            else if((r1.Tipo == Constante.INTEGER || r1.Tipo == Constante.DOUBLE)
                && (r2.Tipo == Constante.INTEGER || r2.Tipo == Constante.DOUBLE))
            {
                double v1 = Convert.ToDouble(r1.Valor, System.Globalization.CultureInfo.InvariantCulture);
                double v2 = Convert.ToDouble(r2.Valor, System.Globalization.CultureInfo.InvariantCulture);
                return new Resultado(tipo, (v1 - v2).ToString());
            }
            else
            {
                if(!(r1.Tipo == Constante.ERROR || r2.Tipo == Constante.ERROR))
                {
                    string descripcion = "Tipos de operandos incorrectos para el operador '-'\nprimer tipo: "
                        + Constante.getTipo(r1.Tipo) + "\nsegundo tipo: " + Constante.getTipo(r2.Tipo);
                    uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                }
                return new Resultado(Constante.ERROR, "");
            }
        }

        public override object generar_booleano(Entorno ent)
        {
            Resultado r1 = (izq != null) ? (Resultado)izq.generar_booleano(ent) : null;
            Resultado r2 = (der != null) ? (Resultado)der.generar_booleano(ent) : null;
            int tipo = Constante.resta[r1.Tipo, r2.Tipo];

            if (r1.Tipo == Constante.INTEGER && r2.Tipo == Constante.INTEGER)
            {
                int v1 = Convert.ToInt32(r1.Valor);
                int v2 = Convert.ToInt32(r2.Valor);
                return new Resultado(tipo, (v1 - v2).ToString());
            }
            else if ((r1.Tipo == Constante.INTEGER || r1.Tipo == Constante.DOUBLE)
                && (r2.Tipo == Constante.INTEGER || r2.Tipo == Constante.DOUBLE))
            {
                double v1 = Convert.ToDouble(r1.Valor, System.Globalization.CultureInfo.InvariantCulture);
                double v2 = Convert.ToDouble(r2.Valor, System.Globalization.CultureInfo.InvariantCulture);
                return new Resultado(tipo, (v1 - v2).ToString());
            }
            else if (r1.Tipo == Constante.ID || r2.Tipo == Constante.ID)
            {
                return new Resultado(Constante.ID, r1.Valor + r2.Valor);
            }
            else
            {
                if (!(r1.Tipo == Constante.ERROR || r2.Tipo == Constante.ERROR))
                {
                    string descripcion = "Tipos de operandos incorrectos para el operador '-'\nprimer tipo: "
                        + Constante.getTipo(r1.Tipo) + "\nsegundo tipo: " + Constante.getTipo(r2.Tipo);
                    uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                }
                return new Resultado(Constante.ERROR, "");
            }
        }
    }
}
