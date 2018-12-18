using ServidorDB.analizadores.usql;
using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.arboles.usql.SSL;
using ServidorDB.arboles.xml;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql
{
    class LLamada : NodoExp
    {
        private int tipo;
        private string id;
        private Funcion funcion;
        private Procedimiento procedimiento;
        private List<NodoExp> paramss;

        public LLamada(Procedimiento proc, int line, int colm) : base(line, colm)
        {
            this.procedimiento = proc;
        }

        public LLamada(Funcion funcion, int line, int colm) : base(line, colm)
        {
            this.funcion = funcion;
        }
        
        public int Tipo { get => tipo; set => tipo = value; }
        public string Id { get => id; set => id = value; }
        public List<NodoExp> Paramss { get => paramss; set => paramss = value; }

        public override object ejecutar(Entorno ent)
        {
            if (tipo == Simbolo.FUNCION)
            {
                return ejecutar_funcion(ent);
            }
            else
            {   //ejecuto un procedimiento
                ejecutar_proc(ent);
            }
            return new Resultado(Constante.ERROR, "") ;
        }

        public void ejecutar_proc(Entorno ent)
        {
            Simbolo s = ent.getSimbolo_Entorno(id, tipo);

            if (s != null)
            {
                Procedimiento fun = (Procedimiento)s.Valor;

                //    //antes de ejecutar las instrucciones debo de comparar la cantidad de parametros
                //    //con la de atributos

                if (paramss.Count == fun.Decs.Count)
                {
                    //        //tiene que ser iguales debido a que se les asignara el valor
                    List<Resultado> ress = new List<Resultado>();
                    for (int i = 0; i < paramss.Count; i++)
                    {
                        Resultado res = (Resultado)paramss[i].ejecutar(ent);
                        ress.Add(res);
                    }

                    Entorno local = new Entorno(ent);
                    bool hayError = false;
                    for (int i = 0; i < paramss.Count; i++)
                    {
                        if (fun.Decs[i].Tipo == ress[i].Tipo)
                        {
                            Simbolo sym = new Simbolo(Simbolo.VARIABLE, ress[i].Tipo, fun.Decs[i].Variables[0], ress[i].Valor);
                            if (!local.existe(fun.Decs[i].Variables[0], Simbolo.VARIABLE))
                            {
                                local.agregar(sym);
                            }
                            else
                            {
                                hayError = true;
                            }
                        }
                    }

                    if (!hayError)
                    {
                        List<uInstruccion> inst = (List<uInstruccion>)fun.ejecutar(ent);

                        if (inst != null)
                        {
                            for (int i = 0; i < inst.Count; i++)
                            {
                                object obj = inst[i].ejecutar(local);
                                if (obj is Detener)
                                {
                                    //error;
                                    string msg = "En el procedimiento no es permitido la instruccion detener";
                                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                }
                                else if (obj is Retornar)
                                {
                                    string msg = "En el procedimiento no es permitido la instruccion retornar";
                                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                }
                            }
                        }
                    }
                }
            }
        }

        public object ejecutar_funcion(Entorno ent)
        {
            //ejecuto una funcion
            //que hago para ejecutar
            //esto debe de devolver una funcion si no error
            //busco la funcion en la tabla de simbolos como ya fue cargada

            Simbolo s = ent.getSimbolo_Entorno(id, tipo);

            if (s != null)
            {
                Funcion fun = (Funcion)s.Valor;

                //    //antes de ejecutar las instrucciones debo de comparar la cantidad de parametros
                //    //con la de atributos

                if (paramss.Count == fun.Decs.Count)
                {
                    //        //tiene que ser iguales debido a que se les asignara el valor
                    List<Resultado> ress = new List<Resultado>();
                    for (int i = 0; i < paramss.Count; i++)
                    {
                        Resultado res = (Resultado)paramss[i].ejecutar(ent);
                        ress.Add(res);
                    }

                    Entorno local = new Entorno(ent);
                    bool hayError = false;
                    for (int i = 0; i < paramss.Count; i++)
                    {
                        if (fun.Decs[i].Tipo == ress[i].Tipo)
                        {
                            Simbolo sym = new Simbolo(Simbolo.VARIABLE, ress[i].Tipo, fun.Decs[i].Variables[0], ress[i].Valor);
                            if (!local.existe(fun.Decs[i].Variables[0], Simbolo.VARIABLE))
                            {
                                local.agregar(sym);
                            }
                            else
                            {
                                hayError = true;
                            }
                        }
                    }

                    if (!hayError)
                    {
                        List<uInstruccion> inst = (List<uInstruccion>)fun.ejecutar(ent);

                        if (inst != null)
                        {
                            bool hayRetorno = false;

                            for (int i = 0; i < inst.Count; i++)
                            {
                                object obj = inst[i].ejecutar(local);
                                if (obj is Detener)
                                {
                                    //error;
                                    string msg = "En la funcion no es permitido la instruccion detener";
                                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                }
                                else if (obj is Retornar)
                                {
                                    //como tendo retornar debo realizar su ejecucion
                                    //por que me devuelve a ella misma
                                    Retornar ret = (Retornar)obj;
                                    Resultado rr = (ret.Exp != null) ? (Resultado)ret.Exp.ejecutar(local) : null;

                                    if (rr != null)
                                    {
                                        if (rr.Tipo == fun.Tipo)
                                        {
                                            hayRetorno = true;
                                            return rr;
                                        }
                                        else
                                        {
                                            string msg = "El tipo de dato de Retorno: " + Constante.getTipo(rr.Tipo) + " no se puede " +
                                                "convertir a el tipo de la funcion: " + Constante.getTipo(fun.Tipo);
                                            uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                            return new Resultado(Constante.ERROR, "");
                                        }


                                    }
                                }
                            }

                            if (!hayRetorno)
                            {
                                string msg = "La funcion: " + fun.Nombre + " no retorna ningun valor ";
                                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                return new Resultado(Constante.ERROR, "");
                            }
                        }
                    }

                    //        //verifico si los tipos son correctos
                    //        //ya tengo los resultados ahora debo de asignarselos a los atributos
                    //        //debo simular declaracioens con esos atributos para que no se puedan
                    //        //crear en ese ambito otros atributos del mismo nombre

                    //        bool hayError = false;
                    //        List<Declarar> decs = new List<Declarar>();

                    //        for(int i = 0; i < paramss.Count; i++)
                    //        {
                    //            if(ress[i].Tipo == fun.Decs[i].Tipo)
                    //            {
                    //                Declarar dec = fun.Decs[i] ;
                    //                dec.Exp = paramss[i];
                    //                decs.Add(dec);
                    //            }
                    //            else
                    //            {
                    //                hayError = true;
                    //                break;
                    //            }
                    //        }

                    //        if (!hayError)
                    //        {
                    //            //solo obtengo las instruccioines en fun.ejecutar
                    //            List<uInstruccion> inst = (List<uInstruccion>)fun.ejecutar(ent);

                    //            //sin error debo ejecutar en un entorno nuevo
                    //            Entorno local_funcion = new Entorno(ent);

                    //            //ahora simulo las declaracioens de los atributos
                    //            for (int i = 0; i < decs.Count; i++)
                    //            {
                    //                decs[i].ejecutar(local_funcion);
                    //            }

                    //            //procedo a realizar las instrucciones
                    //            if(inst != null)
                    //            {

                    //            }
                    //            else
                    //            {
                    //                string msg = "Ocurrio un error al cargar sus instrucciones de la funcion: " + fun.Nombre;
                    //                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                    //            }
                    //        }
                    //        else
                    //        {
                    //            string msg = "La funcion: " + fun.Nombre + " tiene error de tipos en los parametros";
                    //            uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                    //        }

                    //    }
                    //    else
                    //    {
                    //        string msg = "La funcion: " + fun.Nombre + " no coincide con los parametros";
                    //        uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                    //    }
                    //}
                    //else
                    //{
                    //    string msg = "La funcion no existe en la base de datos: " + Constante.db_actual;
                    //    msg += "\nPuede que este accediendo a una funcion de otra base de datos" +
                    //        "\nDe ser asi debe utilizar la instruccion usar";
                    //    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                }
            }
            return new Resultado(Constante.ERROR, "");
        }
    }
}
