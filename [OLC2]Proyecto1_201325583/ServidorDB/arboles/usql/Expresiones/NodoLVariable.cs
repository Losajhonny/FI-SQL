using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.analizadores.usql;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;

namespace ServidorDB.arboles.usql.Expresiones
{
    class NodoLVariable : NodoExp
    {
        private string val1;
        private string val2;

        public NodoLVariable(string val1, int line, int colm) : base(line, colm)
        {
            this.val1 = val1;
        }

        public NodoLVariable(string val1, string val2, int line, int colm) : base(line, colm)
        {
            this.val1 = val1;
            this.val2 = val2;
        }

        public override object ejecutar(Entorno ent)
        {
            if(val2 == null)
            {
                Simbolo s = ent.getSimbolo_Entorno(val1);
                if(s != null)
                {
                    return new Resultado(s.Tipo, s.Valor.ToString());
                }
                else
                {
                    string descripcion = "No se pudo encontrar el Simbolo\nSimbolo: " + val1;
                    uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                    return new Resultado(Constante.ERROR, "");
                }
            }
            else
            {
                //falta el de atributos
                return null;
            }
        }

        public override object generar_booleano(Entorno ent)
        {
            if (val2 == null)
            {
                if(val1[0] == '@')
                {
                    Simbolo s = ent.getSimbolo_Entorno(val1);
                    if (s != null)
                    {
                        return new Resultado(s.Tipo, s.Valor.ToString());
                    }
                    else
                    {
                        string descripcion = "No se pudo encontrar el Simbolo\nSimbolo: " + val1;
                        uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                        return new Resultado(Constante.ERROR, "");
                    }
                }
                else
                {
                    return new Resultado(Constante.ID, val1) ;
                }
            }
            else
            {
                //falta el de atributos
                return null;
            }
        }
    }
}
