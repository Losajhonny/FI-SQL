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
    class Caso : uInstruccion
    {
        private NodoExp exp;
        private Resultado comp;
        private List<uInstruccion> inst;
        private int line;
        private int colm;

        public Caso(NodoExp exp, List<uInstruccion> inst, int line, int colm)
        {
            this.line = line;
            this.colm = colm;
            this.exp = exp;
            this.inst = inst;
        }

        public NodoExp Exp { get => exp; set => exp = value; }
        public List<uInstruccion> Inst { get => inst; set => inst = value; }
        public Resultado Comp { get => comp; set => comp = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }

        public object ejecutar(Entorno ent)
        {
            Resultado res = (Resultado)exp.ejecutar(ent);
            if(res.Tipo == comp.Tipo)
            {
                if (res.Valor.Equals(comp.Valor))
                {   //si son iguales entonces ejecutar uinstruccion
                    for(int i = 0; i < inst.Count; i++)
                    {
                        object obj = inst[i].ejecutar(ent);
                        if (obj is Detener) return obj;
                        if (obj is Retornar) return obj;
                    }
                }
            }
            else
            {
                if (res.Tipo != Constante.ERROR)
                {
                    string descripcion = "Tipos incompatibles: " + Constante.getTipo(res.Tipo) + " no puede ser convertido a " + Constante.getTipo(comp.Tipo);
                    uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                }
            }
            return null;
        }
    }
}
