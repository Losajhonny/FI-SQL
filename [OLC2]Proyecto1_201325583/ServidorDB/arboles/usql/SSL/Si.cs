using ServidorDB.analizadores.usql;
using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql.SSL
{
    class Si : uInstruccion
    {
        private NodoExp exp;
        private List<uInstruccion> inst;
        private Sino sino;
        private int line;
        private int colm;

        public Si(NodoExp exp, List<uInstruccion> inst, Sino sino,
            int line, int colm)
        {
            this.exp = exp;
            this.inst = inst;
            this.sino = sino;
            this.line = line;
            this.colm = colm;
        }

        public Si(NodoExp exp, List<uInstruccion> inst,
            int line, int colm)
        {
            this.exp = exp;
            this.inst = inst;
            this.line = line;
            this.colm = colm;
        }

        public NodoExp Exp { get => exp; set => exp = value; }
        public List<uInstruccion> Inst { get => inst; set => inst = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }
        internal Sino Sino { get => sino; set => sino = value; }

        public void ejecutar(Entorno ent, Resultado res)
        {
            int val = Convert.ToInt32(res.Valor);
            if (val == 1)
            {
                Entorno nuevo = new Entorno(ent);
                for (int i = 0; i < inst.Count; i++)
                {
                    inst[i].ejecutar(nuevo);
                }
            }
            else
            {
                Entorno nuevo = new Entorno(ent);
                if (sino != null)
                {
                    sino.ejecutar(nuevo);
                }
            }
        }

        public object ejecutar(Entorno ent)
        {
            Resultado res = (Resultado)exp.ejecutar(ent);
            if (res.Tipo == Constante.BOOL)
            {
                ejecutar(ent, res);
            }
            else if (res.Tipo == Constante.INTEGER)
            {
                int v1 = Convert.ToInt32(res.Valor);
                if (v1 == 1 || v1 == 0)
                {
                    ejecutar(ent, res);
                }
                else
                {
                    string descripcion = "El tipo de dato bool no permite el valor " + res.Valor
                        + " unicamente acepta 0 o 1";
                    uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                    return new Resultado(Constante.ERROR, "");
                }
            }
            else
            {
                if (res.Tipo != Constante.ERROR)
                {
                    string descripcion = "Tipos incompatibles: " + Constante.getTipo(res.Tipo) + " no puede ser convertido a " + Constante.getTipo(Constante.BOOL);
                    uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                }
            }
            return null;
        }
    }
}
