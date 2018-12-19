using ServidorDB.analizadores.usql;
using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.arboles.xml;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql.SSL
{
    class Declarar : uInstruccion
    {
        /*
         * Realiza una o mas declaraciones
         * 
         * @variables       Lista de varibles a declarar
         * @id              Es una variable para declarar un objeto
         * @id_objeto       Es un identificador de un objeto
         * @tipo            En caso de que sea declaracion de objeto tendra como valor Constante.NONE
         * @exp             Es la expresion a asignar a las variables
         * @line            Linea de la palabra reservada "declarar"
         * @colm            Columna de la palabra reservada "declarar"
         */

        private List<string> variables;
        private string id;
        private string id_objeto;
        private int tipo;
        private NodoExp exp;
        private int line;
        private int colm;

        /**
         * Constructor Para variables con valor
         */
        public Declarar(List<string> variables, int tipo, NodoExp exp, int line, int colm)
        {
            this.variables = variables;
            this.tipo = tipo;
            this.exp = exp;
            this.line = line;
            this.colm = colm;
        }

        /**
         * Constructor Para variables sin valor
         */
        public Declarar(List<string> variables, int tipo, int line, int colm)
        {
            this.variables = variables;
            this.tipo = tipo;
            this.exp = null;
            this.line = line;
            this.colm = colm;
        }

        /**
         * Constructor para declaracion de objetos
         */
        public Declarar(string id, string id_objeto, int tipo, int line, int colm)
        {
            this.id = id;
            this.tipo = tipo;
            this.line = line;
            this.colm = colm;
            this.id_objeto = id_objeto;
        }

        public List<string> Variables { get => variables; set => variables = value; }
        public int Tipo { get => tipo; set => tipo = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }
        public NodoExp Exp { get => exp; set => exp = value; }
        public string Id { get => id; set => id = value; }
        public string Id_objeto { get => id_objeto; set => id_objeto = value; }

        public object ejecutar(Entorno ent)
        {
            if (tipo != Constante.ID)
            {
                //si la declaracion no es un objeto
                //variables normales
                //debo ejecutar la expresion para asignar a las variables
                Resultado res = (exp != null) ? (Resultado)exp.ejecutar(ent) : null;
                string valor = string.Empty;
                bool hayError = false;
                if (res != null)
                {
                    if (res.Tipo == tipo)
                    {
                        valor = res.Valor;
                    }
                    else if (res.Tipo == Constante.INTEGER && tipo == Constante.BOOL)
                    {
                        int v = Convert.ToInt32(res.Valor);
                        if(v == 1 || v == 0)
                        {
                            valor = res.Valor;
                        }
                        else
                        {
                            hayError = true;
                            string descripcion = "El tipo de dato bool no permite el valor " + res.Valor
                                + " unicamente acepta 0 o 1";
                            uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                        }
                    }
                    else
                    {
                        if (res.Tipo != Constante.ERROR)
                        {
                            hayError = true;
                            string descripcion = "Tipos incompatibles: " + Constante.getTipo(res.Tipo) + " no puede ser convertido a " + Constante.getTipo(tipo);
                            uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                        }
                    }
                }

                for (int i = 0; i < variables.Count && !hayError; i++)
                {
                    //ver como ingresar el valor en el entorno
                    if (!ent.existe(variables[i], Simbolo.VARIABLE))
                    {
                        Simbolo s = new Simbolo(Simbolo.VARIABLE ,tipo, variables[i], valor);
                        ent.agregar(s);
                        //ent.agregar(variables[i], s);
                    }
                    else
                    {
                        string descripcion = "Variable " + variables[i] + " ya esta definida en este ambito";
                        uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                    }
                }
            }
            else
            {
                //es un ID por lo tanto es una declaracion de un objeto
                //necesito realizar la instancia del objeto

                //el nombre sera id en la tabla de simbolo

                /*DEBO BUSCAR EL OBJETO EN EL ENTORNO GLOBAL*/
                Simbolo s = Constante.global.getSimbolo_Entorno_Actual(id_objeto, Simbolo.OBJETO);

                /*Debo realizar una copia de los parametros y debo crear una lista donde iran los valores de
                 esos parametros ---- por lo tando el tamaño de la lista debe ser el tamaño de los parametros*/
                if(s != null)
                {
                    Objeto nuevo = new Objeto(s.Id);
                    /*Como los parametros no los modifico entonces lo paso de igual forma*/
                    nuevo.Parametros = ((Objeto)s.Valor).Parametros;
                    nuevo.Valores = new string[nuevo.Parametros.Count];

                    //le voy a colocar valores por defecto para no tener errores mas adelante

                    for (int i = 0; i < nuevo.Parametros.Count; i++)
                    {
                        switch (nuevo.Parametros[i].Tipo)
                        {
                            case Constante.INTEGER:
                                nuevo.Valores[i] = "0";
                                break;
                            case Constante.TEXT:
                                nuevo.Valores[i] = "";
                                break;
                            case Constante.BOOL:
                                nuevo.Valores[i] = "0";
                                break;
                            case Constante.DATE:
                                nuevo.Valores[i] = "01-01-1900";
                                break;
                            case Constante.DATETIME:
                                nuevo.Valores[i] = "01-01-1900 00:00:00";
                                break;
                            case Constante.DOUBLE:
                                nuevo.Valores[i] = "0";
                                break;
                            default:
                                nuevo.Valores[i] = "";
                                break;
                        }
                    }

                    //aqui ya tengo la clase objeto instanciada entonces solo debo metermo la tabla de simbolos
                    if(!ent.existe(id, Simbolo.VARIABLE))
                    {
                        Simbolo sim = new Simbolo(Simbolo.VARIABLE, Constante.ID, id, nuevo);
                        ent.agregar(sim);
                    }
                    else
                    {
                        string descripcion = "Variable " + id + " ya esta definida en este ambito";
                        uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                    }
                }
                else
                {
                    string descripcion = "No existe el objeto: " + id +" en la base de datos actual";
                    uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                }
            
            }

            //instancia de objetos
            //todavia no hago esto
            //instancias de objeto
            //buscar el objeto
            //Simbolo sobjeto = ent.getSimbolo_Entorno(id_objeto);
            //if (sobjeto != null)
            //{
            //    if (!ent.existe(id))
            //    {
            //        Simbolo s = new Simbolo(tipo, id, sobjeto.Valor);
            //        ent.agregar(id, s);
            //    }
            //    else
            //    {
            //        string descripcion = "Variable " + id + " ya esta definida en este ambito";
            //        uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
            //    }
            //}
            //else
            //{
            //    string descripcion = "El objeto '" + id_objeto + "' no existe";
            //    uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
            //}


            return null;
        }

        public object generar_booleano(Entorno ent)
        {
            throw new NotImplementedException();
        }
    }
}
