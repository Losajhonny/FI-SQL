﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.analizadores.usql;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;

namespace ServidorDB.arboles.usql.Expresiones.Logica
{
    class NodoAnd : NodoExp
    {
        public NodoAnd(NodoExp izq, NodoExp der, int line, int colm) : base(izq, der, line, colm) { }

        public override object ejecutar(Entorno ent)
        {
            Resultado r1 = (izq != null) ? (Resultado)izq.ejecutar(ent) : null;
            Resultado r2 = (der != null) ? (Resultado)der.ejecutar(ent) : null;
            int tipo = Constante.BOOL;

            if(r1.Tipo == Constante.BOOL && r2.Tipo == Constante.BOOL)
            {
                int v1 = Convert.ToInt32(r1.Valor);
                int v2 = Convert.ToInt32(r2.Valor);
                return new Resultado(tipo, (v1 * v2).ToString());
            }
            else if (r1.Tipo == Constante.INTEGER && r2.Tipo == Constante.INTEGER)
            {
                int v1 = Convert.ToInt32(r1.Valor);
                int v2 = Convert.ToInt32(r2.Valor);
                if ((v1 == 1 || v1 == 0) && (v2 == 1 || v2 == 0))
                {
                    return new Resultado(tipo, (v1 * v2).ToString());
                }
                else
                {
                    if (v1 > 1)
                    {
                        string descripcion = "El tipo de dato bool no permite el valor " + r1.Valor
                        + " unicamente acepta 0 o 1";
                        uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                    }
                    else
                    {
                        string descripcion = "El tipo de dato bool no permite el valor " + r2.Valor
                        + " unicamente acepta 0 o 1";
                        uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                    }
                    return new Resultado(Constante.ERROR, "");
                }
            }
            else
            {
                if (!(r1.Tipo == Constante.ERROR || r2.Tipo == Constante.ERROR))
                {
                    string descripcion = "Tipos de operandos incorrectos para el operador '&&'\nprimer tipo: "
                        + Constante.getTipo(r1.Tipo) + "\nsegundo tipo: " + Constante.getTipo(r2.Tipo);
                    uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                }
                return new Resultado(Constante.ERROR, "");
            }
        }

        public override object generar_booleano(Entorno ent)
        {
            string r1 = (izq != null) ? izq.generar_booleano(ent).ToString() : null;
            string r2 = (der != null) ? der.generar_booleano(ent).ToString() : null;
            string cadena = "";
            
            cadena += r1 + " and " + r2;
            return cadena;
        }
    }
}
