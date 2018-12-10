using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.analizadores.usql;
using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;

namespace ServidorDB.arboles.usql.SSL
{
    class Selecciona : uInstruccion
    {
        private NodoExp exp;
        private List<uInstruccion> casos;
        private Defecto defecto;
        private int line;
        private int colm;

        public Selecciona(NodoExp exp, List<uInstruccion> casos, Defecto defecto, int line, int colm)
        {
            this.exp = exp;
            this.casos = casos;
            this.defecto = defecto;
            this.line = line;
            this.colm = colm;
        }

        public Selecciona(NodoExp exp, List<uInstruccion> casos, int line, int colm)
        {
            this.exp = exp;
            this.casos = casos;
            this.defecto = null;
            this.line = line;
            this.colm = colm;
        }

        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }
        internal List<uInstruccion> Casos { get => casos; set => casos = value; }
        internal Defecto Defecto { get => defecto; set => defecto = value; }
        internal NodoExp Exp { get => exp; set => exp = value; }

        public object ejecutar(Entorno ent)
        {   //debo crear un nuevo entorno para este ambito
            Resultado res = (Resultado)exp.ejecutar(ent);
            Entorno nuevo = new Entorno(ent);

            if(res.Tipo == Constante.TEXT
                || res.Tipo == Constante.INTEGER
                || res.Tipo == Constante.DOUBLE)
            {
                for (int i = 0; i < casos.Count; i++)
                {
                    ((Caso)casos[i]).Comp = res;
                    object obj = casos[i].ejecutar(nuevo);
                    if (obj is Detener) return null;
                    if (obj is Retornar) return obj;
                }

                if (defecto != null)
                {
                    object obj = defecto.ejecutar(nuevo);
                    if (obj is Detener) return null;
                    if (obj is Retornar) return obj;
                }
            }
            else
            {
                if (res.Tipo != Constante.ERROR)
                {
                    string descripcion = "Tipo de dato no permitido: " + Constante.getTipo(res.Tipo) + ". unicamente text, integer, double";
                    uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                }
            }
            return null;
        }
    }
}
