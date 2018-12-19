using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.analizadores.usql;
using ServidorDB.arboles.xml;
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
            //val 1 es la variable que puede venir con @
            //val 2 es la variable despues del punto
            if (val2 == null)
            {
                Simbolo s = ent.getSimbolo_Entorno(val1, Simbolo.VARIABLE);
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
                //es casi similar al de la asignacion
                //busco la variable
                Simbolo s = ent.getSimbolo_Entorno(val1, Simbolo.VARIABLE);
                if (s != null)
                {
                    //obtengo el objeto
                    Objeto obj = (Objeto)s.Valor;
                    for (int i = 0; i < obj.Parametros.Count; i++)
                    {
                        //busco el atributo
                        Atributo tmp = obj.Parametros[i];
                        if (tmp.Nombre.Equals(val2))
                        {   //como ya esta agregado solo retorno un resultado con el tipo y valor
                            return new Resultado(tmp.Tipo, obj.Valores[i]);
                        }
                    }

                    //reportar el error que no existe el atributo del objeto
                    string descripcion = "Atributo " + val2 + " no encontrado en el objeto: " + val1;
                    uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                }
                else
                {
                    string descripcion = "Variable " + val1 + " no existe";
                    uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                }
                return new Resultado(Constante.ERROR, "");
            }
        }

        public override object generar_booleano(Entorno ent)
        {
            //val 1 es la variable que puede venir con @
            //val 2 es la variable despues del punto
            if (val2 == null)
            {
                if(val1[0] == '@')
                {
                    Simbolo s = ent.getSimbolo_Entorno(val1, Simbolo.VARIABLE);
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
                /*como esto solo me genera el booleano de un where*/
                //buscar la tabla
                if(val1[0] != '@')
                {

                    if (Constante.tablas.ContainsKey(val1))
                    {
                        //si contiene informacion
                        for (int i = 0; i < Constante.tablasl.Count; i++)
                        {
                            if (Constante.tablasl[i].Nombre.Equals(val1))
                            {
                                return new Resultado(Constante.ID, val2 + i.ToString());
                            }
                        }
                    }
                    else
                    {
                        //string descripcion = "La tabla: " + val1 + " no existe en la base de datos";
                        //uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                        return new Resultado(Constante.ID, val2);
                    }
                }
                else
                {
                    //es casi similar al de la asignacion
                    //busco la variable
                    Simbolo s = ent.getSimbolo_Entorno(val1, Simbolo.VARIABLE);
                    if (s != null)
                    {
                        //obtengo el objeto
                        Objeto obj = (Objeto)s.Valor;
                        for (int i = 0; i < obj.Parametros.Count; i++)
                        {
                            //busco el atributo
                            Atributo tmp = obj.Parametros[i];
                            if (tmp.Nombre.Equals(val2))
                            {   //como ya esta agregado solo retorno un resultado con el tipo y valor
                                return new Resultado(tmp.Tipo, obj.Valores[i]);
                            }
                        }

                        //reportar el error que no existe el atributo del objeto
                        string descripcion = "Atributo " + val2 + " no encontrado en el objeto: " + val1;
                        uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                    }
                    else
                    {
                        string descripcion = "Variable " + val1 + " no existe";
                        uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                    }
                    return new Resultado(Constante.ERROR, "");
                }
                return new Resultado(Constante.ERROR, "");
            }
        }
    }
}
