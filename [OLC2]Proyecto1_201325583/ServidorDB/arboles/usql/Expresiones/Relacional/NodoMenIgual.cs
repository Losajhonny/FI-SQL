using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.analizadores.usql;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;

namespace ServidorDB.arboles.usql.Expresiones.Relacional
{
    class NodoMenIgual : NodoExp
    {
        public NodoMenIgual(NodoExp izq, NodoExp der, int line, int colm) : base(izq, der, line, colm) { }

        public override object ejecutar(Entorno ent)
        {
            Resultado r1 = (izq != null) ? (Resultado)izq.ejecutar(ent) : null;
            Resultado r2 = (der != null) ? (Resultado)der.ejecutar(ent) : null;
            int tipo = Constante.BOOL;

            if ((r1.Tipo == Constante.INTEGER || r1.Tipo == Constante.DOUBLE)
                && (r2.Tipo == Constante.INTEGER || r2.Tipo == Constante.DOUBLE))
            {
                double v1 = Convert.ToDouble(r1.Valor, System.Globalization.CultureInfo.InvariantCulture);
                double v2 = Convert.ToDouble(r2.Valor, System.Globalization.CultureInfo.InvariantCulture);
                int v = 0;
                if (v1 <= v2)
                {
                    v = 1;
                }
                return new Resultado(tipo, v.ToString());
            }
            else if (r1.Tipo == Constante.TEXT && r2.Tipo == Constante.TEXT)
            {
                int v1 = 0;
                int v2 = 0;
                int v = 0;

                for (int i = 0; i < r1.Valor.Length; i++)
                {
                    v1 += (int)r1.Valor[i];
                }
                for (int i = 0; i < r2.Valor.Length; i++)
                {
                    v2 += (int)r2.Valor[i];
                }
                if (v1 <= v2)
                {
                    v = 1;
                }
                return new Resultado(tipo, v.ToString());
            }
            else
            {
                if (!(r1.Tipo == Constante.ERROR || r2.Tipo == Constante.ERROR))
                {
                    string descripcion = "Tipos de operandos incorrectos para el operador <='\nprimer tipo: "
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
            string cadena = "";

            if (r1.Tipo == Constante.TEXT || r1.Tipo == Constante.DATE || r1.Tipo == Constante.DATETIME)
            {
                cadena += "'" + r1.Valor + "' ";
            }
            else
            {
                cadena += r1.Valor + " ";
            }
            cadena += "<= ";
            if (r2.Tipo == Constante.TEXT || r2.Tipo == Constante.DATE || r2.Tipo == Constante.DATETIME)
            {
                cadena += "'" + r2.Valor + "' ";
            }
            else
            {
                cadena += r2.Valor + " ";
            }
            return cadena;
        }
    }
}
