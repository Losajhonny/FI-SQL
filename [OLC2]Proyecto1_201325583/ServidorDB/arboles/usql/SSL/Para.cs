using ServidorDB.analizadores.usql;
using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.arboles.usql.Expresiones.Aritmetica;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql.SSL
{
    class Para : uInstruccion
    {
        private List<uInstruccion> inst;
        private Declarar declarar;
        private Asignacion asignar;
        private NodoExp exp;
        private string op;
        private int line;
        private int colm;

        public Para(List<uInstruccion> inst, Declarar declarar, NodoExp exp, string op, int line, int colm)
        {
            this.inst = inst;
            this.declarar = declarar;
            this.exp = exp;
            this.op = op;
            this.line = line;
            this.colm = colm;

            NodoLVariable np1 = new NodoLVariable(declarar.Variables[0], line, colm);
            NodoPrimitivo np2 = new NodoPrimitivo("1",
                Constante.INTEGER,
                line, colm);

            if (op.Equals("++"))
            {
                NodoMas ns = new NodoMas(np1, np2, line, colm);
                asignar = new Asignacion(declarar.Variables[0], ns, line, colm);
            }
            else
            {
                NodoMenos nm = new NodoMenos(np1, np2, line, colm);
                asignar = new Asignacion(declarar.Variables[0], nm, line, colm);
            }
        }

        public string Op { get => op; set => op = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }
        public Declarar Declarar { get => declarar; set => declarar = value; }
        public NodoExp Exp { get => exp; set => exp = value; }
        internal List<uInstruccion> Inst { get => inst; set => inst = value; }

        public object ejecutar(Entorno ent)
        {
            int estado_condicion = 1;
            //inicializando el estado_condicion
            int bandera = 0;
            //bandera para ejecutar solo una vez la declaracion
            Entorno tmp = ent;
            //obtengo mi entorno actual como temporal
            while (estado_condicion == 1)
            {   //iniciando ciclo
                estado_condicion = 0;
                //inicializando el estado_condicion
                if (bandera == 0)
                {
                    tmp = new Entorno(tmp);
                    tmp.Tent = Constante.PARA;
                    //creo mi nuevo entorno para la declaracion
                    declarar.ejecutar(tmp);
                    //ejecutamos la declaracion solo una vez
                    bandera = 1;
                }
                Entorno nuevo = new Entorno(tmp);
                nuevo.Tent = Constante.PARA;
                //el tmp es el entorno actual
                //como no quiero que se realicen declaraciones
                //con la misma variable que la declaracion inicial del para
                //entonces movemos el simbolo a el nuevo entorno
                nuevo.agregar(declarar.Variables[0], tmp.getSimbolo_Entorno_Actual(declarar.Variables[0]));
                //creo un entorno para las instruccion
                Resultado res = (Resultado)exp.ejecutar(nuevo);
                //ejecuto la condicion
                if(res.Tipo == Constante.BOOL || res.Tipo == Constante.INTEGER)
                {   //verificando que sea del tipo correcto
                    if(res.Tipo == Constante.INTEGER)
                    {   //en caso de integer verificar si se puede convertir a bool
                        if(Convert.ToInt32(res.Valor) == 1
                            || Convert.ToInt32(res.Valor) == 0)
                        {
                            estado_condicion = Convert.ToInt32(res.Valor);
                        }
                        else
                        {
                            string descripcion = "El tipo de dato bool no permite el valor " + res.Valor
                            + " unicamente acepta 0 o 1";
                            uSintactico.uerrores.Add(new uError(nuevo.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                        }
                    }
                    else
                    {
                        estado_condicion = Convert.ToInt32(res.Valor);
                    }

                    if(estado_condicion == 1)
                    {   //en caso de que sea true la condicion : ejecutar inst
                        for(int i = 0; i < inst.Count; i++)
                        {   //ejecutando inst con el entorno para estas
                            object obj = inst[i].ejecutar(nuevo);
                            if (obj is Detener) return null;
                            if (obj is Retornar) return obj;
                        }
                        //falta ejecutar la operacion "--" o "++"
                        asignar.ejecutar(nuevo);
                    }
                }
                else
                {
                    if (res.Tipo != Constante.ERROR)
                    {
                        string descripcion = "Tipos incompatibles: " + Constante.getTipo(res.Tipo) + " no puede ser convertido a " + Constante.getTipo(Constante.BOOL);
                        uSintactico.uerrores.Add(new uError(nuevo.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                    }
                }
            }
            return null;
        }

        public object generar_booleano(Entorno ent)
        {
            throw new NotImplementedException();
        }
    }
}
