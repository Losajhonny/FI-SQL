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
    class Mientras : uInstruccion
    {
        private NodoExp exp;
        private int line;
        private int colm;
        private List<uInstruccion> inst;

        public Mientras(NodoExp exp, List<uInstruccion> inst, int line, int colm)
        {
            this.exp = exp;
            this.inst = inst;
            this.line = line;
            this.colm = colm;
        }

        public NodoExp Exp { get => exp; set => exp = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }
        public List<uInstruccion> Inst { get => inst; set => inst = value; }

        public object ejecutar(Entorno ent)
        {
            int estado_condicion = 1;
            //inicializando el estado_condicion
            while (estado_condicion == 1)
            {   //iniciando ciclo
                estado_condicion = 0;
                //inicializando el estado_condicion
                Entorno nuevo = new Entorno(ent);
                //creo mi nuevo entorno
                Resultado res = (Resultado)exp.ejecutar(nuevo);
                //ejecuto la condicion
                if (res.Tipo == Constante.BOOL || res.Tipo == Constante.INTEGER)
                {   //verificando que sea del tipo correcto
                    if (res.Tipo == Constante.INTEGER)
                    {   //en caso de integer verificar si se puede convertir a bool
                        if (Convert.ToInt32(res.Valor) == 1
                            || Convert.ToInt32(res.Valor) == 0)
                        {
                            estado_condicion = Convert.ToInt32(res.Valor);
                        }
                        else
                        {
                            string descripcion = "El tipo de dato bool no permite el valor " + res.Valor
                            + " unicamente acepta 0 o 1";
                            uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                        }
                    }
                    else
                    {
                        estado_condicion = Convert.ToInt32(res.Valor);
                    }

                    if (estado_condicion == 1)
                    {   //en caso de que sea true la condicion : ejecutar inst
                        for (int i = 0; i < inst.Count; i++)
                        {   //ejecutando inst con el entorno para estas
                            inst[i].ejecutar(nuevo);
                        }
                    }
                }
            }
            return null;
        }
    }
}
