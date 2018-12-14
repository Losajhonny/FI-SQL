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

        public object ejecutar(Entorno ent)
        {
            Resultado res = (Resultado)exp.ejecutar(ent);
            int estado_condicion = 0;
            if(res.Tipo == Constante.BOOL || res.Tipo == Constante.INTEGER)
            {
                if(res.Tipo == Constante.INTEGER)
                {
                    if (Convert.ToInt32(res.Valor) == 1
                            || Convert.ToInt32(res.Valor) == 0)
                    {
                        estado_condicion = Convert.ToInt32(res.Valor);
                    }
                    else
                    {
                        string descripcion = "El tipo de dato bool no permite el valor " + res.Valor
                        + " unicamente acepta 0 o 1";
                        uSintactico.uerrores.Add(new uError(Constante.SI, Constante.SEMANTICO, descripcion, null, line, colm));
                    }
                }
                else
                {
                    estado_condicion = Convert.ToInt32(res.Valor);
                }

                if(estado_condicion == 1)
                {
                    Entorno nuevo = new Entorno(ent);
                    nuevo.Tent = Constante.SI;
                    for (int i = 0; i < inst.Count; i++)
                    {
                        object obj = inst[i].ejecutar(nuevo);
                        if (obj is Detener) return obj;
                        if (obj is Retornar) return obj;
                    }
                }
                else
                {
                    Entorno nuevo = new Entorno(ent);
                    nuevo.Tent = Constante.SINO;
                    if (sino != null)
                    {
                        object obj = sino.ejecutar(nuevo);
                        if (obj is Detener) return obj;
                        if (obj is Retornar) return obj;
                    }
                }
            }
            else
            {
                if (res.Tipo != Constante.ERROR)
                {
                    string descripcion = "Tipos incompatibles: " + Constante.getTipo(res.Tipo) + " no puede ser convertido a " + Constante.getTipo(Constante.BOOL);
                    uSintactico.uerrores.Add(new uError(Constante.SI, Constante.SEMANTICO, descripcion, null, line, colm));
                }
            }
            return null;
        }
    }
}
