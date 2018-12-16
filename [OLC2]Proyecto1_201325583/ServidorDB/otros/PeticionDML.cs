using ServidorDB.analizadores.usql;
using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.arboles.xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.otros
{
    class PeticionDML
    {
        public static bool insertarNormal(string id, List<Resultado> vals, int line, int colm)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            if (Constante.usuando_db_actual)
            {
                /*Como ya estoy en un db ahora solo debo acceder por motivos de que
                 se utilizo la funcion usar .. si entro  aqui es por que si existia
                 la base de datos*/
                //xD igual verifico por si las moscas jajaja 
                if (master.Dbs.ContainsKey(Constante.db_actual))
                {
                    //bueno aqui ya debe exister
                    //Como si existe la base de datos
                    //entonces verificar si tiene permiso para modificar el objeto
                    string usr = null;
                    foreach (string user in master.Dbs[Constante.db_actual].Usuarios)
                    {
                        if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                    }

                    if (usr != null)
                    {
                        if (master.Dbs[Constante.db_actual].Tablas.ContainsKey(id))
                        {
                            Tabla t = master.Dbs[Constante.db_actual].Tablas[id];
                            //verificar si el objeto tiene permisos de usuario
                            usr = null;
                            foreach (string user in t.Usuarios)
                            {
                                if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                            }

                            if (usr != null)
                            {
                                //como si tiene permisos para modificar la tabla entonces procedo a insertar
                                //el valor de la lista de resultados
                                if(vals.Count == t.Atributos.Count)
                                {
                                    //la validacion es por que se tienen que ingresar todos los valores

                                    /*ahora biene lo chido jajaja
                                     necesito verificar el tipo de los valores con el tipo de los atributos
                                     por que el datatable tiene asignados las columna segun el tipo
                                     por lo tanto seria de convertir el valor e insertar sin errores*/
                                    DataRow dr = t.Registros.NewRow();
                                    bool hayError = false;
                                    for(int i = 0; i < t.Atributos.Count; i++)
                                    {
                                        if(vals[i].Tipo == t.Atributos[i].Tipo)
                                        {
                                            try
                                            {
                                                //necesito convertirlo
                                                if (t.Atributos[i].Tipo == Constante.INTEGER)
                                                {
                                                    int v = Int32.Parse(vals[i].Valor);
                                                    dr[t.Atributos[i].Nombre] = v;
                                                }
                                                else if (t.Atributos[i].Tipo == Constante.TEXT)
                                                {
                                                    dr[t.Atributos[i].Nombre] = vals[i].Valor;
                                                }
                                                else if (t.Atributos[i].Tipo == Constante.BOOL)
                                                {
                                                    dr[t.Atributos[i].Nombre] = Boolean.Parse(vals[i].Valor);
                                                }
                                                else if (t.Atributos[i].Tipo == Constante.DOUBLE)
                                                {
                                                    dr[t.Atributos[i].Nombre] = Double.Parse(vals[i].Valor, System.Globalization.CultureInfo.InvariantCulture);
                                                }
                                                else if (t.Atributos[i].Tipo == Constante.DATE)
                                                {
                                                    DateTime dtt = DateTime.Parse(vals[i].Valor);
                                                    dr[t.Atributos[i].Nombre] = dtt;
                                                }
                                                else if (t.Atributos[i].Tipo == Constante.DATETIME)
                                                {
                                                    DateTime dtt = DateTime.Parse(vals[i].Valor);
                                                    dr[t.Atributos[i].Nombre] = dtt;
                                                }
                                                else
                                                {
                                                    hayError = true;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                hayError = true;
                                                //string msg = ex.Message;
                                                //uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                            }
                                        }
                                        else
                                        {
                                            hayError = true;
                                        }
                                    }
                                    if (!hayError)
                                    {
                                        //debo validar la llave foranea antes de ingresar un registro
                                        /*
                                         
                                         
                                         
                                         
                                         
                                         
                                         
                                                VALIDAR LLAVE FORANEA
                                         
                                         
                                         
                                         
                                         
                                         
                                         */
                                        //entonces insertar el datarow

                                        //tengo que devolver el mensaje por motivos de errores
                                        try
                                        {
                                            t.Registros.Rows.Add(dr);
                                        }
                                        catch (Exception ex)
                                        {
                                            string msg = ex.Message;
                                            uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                        }
                                    }
                                    else
                                    {
                                        string msg = "Los Valores tiene que ser del mismo tipo que los atributos de la tabla: " + t.Nombre;
                                        uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                    }
                                }
                                else
                                {
                                    string msg = "Los Valores no coinciden con el tamaño de columnas de la tabla " + t.Nombre;
                                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                }
                            }
                            else
                            {
                                string msg = "El usuario " + Constante.usuario_actual + " no tiene permiso de modificar la tabla " + t.Nombre;
                                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                            }
                        }
                        else
                        {
                            string msg = "La tabla: " + id + " no existe en la base de datos: " + Constante.db_actual;
                            uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                        }
                    }
                    else
                    {
                        string msg = "El usuario " + Constante.usuario_actual + " no tiene permisos en la base de datos " + Constante.db_actual;
                        uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                    }
                }
                else
                {
                    string msg = "La base de datos: " + Constante.db_actual + " no existe en el DBMS";
                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                }
            }
            else
            {
                //error por no haber utilizado la instruccion usar
                string msg = "Se debe usar la instruccion 'usar' antes de insertar valores a una Tabla";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
            }

            Constante.db_actual = "";
            Constante.usuando_db_actual = false;
            master.generar_xml();
            return estado_aceptacion;
        }

        public static bool insertarEspecial(string id, List<Resultado> vals, List<string> ids, int line, int colm)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            if (Constante.usuando_db_actual)
            {
                /*Como ya estoy en un db ahora solo debo acceder por motivos de que
                 se utilizo la funcion usar .. si entro  aqui es por que si existia
                 la base de datos*/
                //xD igual verifico por si las moscas jajaja 
                if (master.Dbs.ContainsKey(Constante.db_actual))
                {
                    //bueno aqui ya debe exister
                    //Como si existe la base de datos
                    //entonces verificar si tiene permiso para modificar el objeto
                    string usr = null;
                    foreach (string user in master.Dbs[Constante.db_actual].Usuarios)
                    {
                        if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                    }

                    if (usr != null)
                    {
                        if (master.Dbs[Constante.db_actual].Tablas.ContainsKey(id))
                        {
                            Tabla t = master.Dbs[Constante.db_actual].Tablas[id];
                            //verificar si el objeto tiene permisos de usuario
                            usr = null;
                            foreach (string user in t.Usuarios)
                            {
                                if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                            }

                            if (usr != null)
                            {
                                //como si tiene permisos para modificar la tabla entonces procedo a insertar
                                //la insercion cambia por el motivo que ahora tengo ids de atributo
                                if (vals.Count == ids.Count && ids.Count <= t.Atributos.Count)
                                {
                                    //ahora debo recorrer los ids buscando la columna de este identificador
                                    //en dado caso que no exista la columna creo que el data table ya lo
                                    //reporta por lo necesito de la cadena donde lo reporta

                                    /*ahora biene lo chido jajaja
                                     necesito verificar el tipo de los valores con el tipo de los atributos
                                     por que el datatable tiene asignados las columna segun el tipo
                                     por lo tanto seria de convertir el valor e insertar sin errores
                                     
                                     ademas de recorrer los ids y vals en esta ocasion no se psaran de los
                                     limites de los atributos de la tabla*/
                                    DataRow dr = t.Registros.NewRow();
                                    bool hayError = false;

                                    Dictionary<string, Atributo> tmp = new Dictionary<string, Atributo>();
                                    foreach(Atributo atr in t.Atributos)
                                    {
                                        tmp.Add(atr.Nombre, atr);
                                    }

                                    for(int i = 0; i < ids.Count; i++)
                                    {
                                        if (tmp.ContainsKey(ids[i]))
                                        {
                                            try
                                            {
                                                if (tmp[ids[i]].Tipo == Constante.INTEGER)
                                                {
                                                    int v = Int32.Parse(vals[i].Valor);
                                                    dr[tmp[ids[i]].Nombre] = v;
                                                }
                                                else if (tmp[ids[i]].Tipo == Constante.TEXT)
                                                {
                                                    dr[tmp[ids[i]].Nombre] = vals[i].Valor;
                                                }
                                                else if (tmp[ids[i]].Tipo == Constante.BOOL)
                                                {
                                                    dr[tmp[ids[i]].Nombre] = Boolean.Parse(vals[i].Valor);
                                                }
                                                else if (tmp[ids[i]].Tipo == Constante.DOUBLE)
                                                {
                                                    dr[tmp[ids[i]].Nombre] = Double.Parse(vals[i].Valor, System.Globalization.CultureInfo.InvariantCulture);
                                                }
                                                else if (tmp[ids[i]].Tipo == Constante.DATE)
                                                {
                                                    DateTime dtt = DateTime.Parse(vals[i].Valor);
                                                    dr[tmp[ids[i]].Nombre] = dtt;
                                                }
                                                else if (tmp[ids[i]].Tipo == Constante.DATETIME)
                                                {
                                                    DateTime dtt = DateTime.Parse(vals[i].Valor);
                                                    dr[tmp[ids[i]].Nombre] = dtt;
                                                }
                                                else
                                                {
                                                    hayError = true;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                hayError = true;
                                                //string msg = ex.Message;
                                                //uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                            }
                                        }
                                        else
                                        {
                                            dr[tmp[ids[i]].Nombre] = DBNull.Value;
                                        }
                                    }

                                    if (!hayError)
                                    {
                                        //debo validar la llave foranea antes de ingresar un registro
                                        /*
                                         
                                         
                                         
                                         
                                         
                                         
                                         
                                                VALIDAR LLAVE FORANEA
                                         
                                         
                                         
                                         
                                         
                                         
                                         */
                                        //entonces insertar el datarow

                                        try
                                        {
                                            t.Registros.Rows.Add(dr);
                                        }
                                        catch (Exception ex)
                                        {
                                            string msg = ex.Message;
                                            uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                        }
                                    }
                                    else
                                    {
                                        string msg = "Los Valores tiene que ser del mismo tipo que los atributos de la tabla: " + t.Nombre;
                                        uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                    }
                                }
                                else
                                {
                                    string msg = "Los Valores no coinciden con el tamaño de los identificadores definidos" +
                                        " o del tamaño de los atributos de la tabla: " + t.Nombre;
                                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                }
                            }
                            else
                            {
                                string msg = "El usuario " + Constante.usuario_actual + " no tiene permiso de modificar la tabla " + t.Nombre;
                                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                            }
                        }
                        else
                        {
                            string msg = "La tabla: " + id + " no existe en la base de datos: " + Constante.db_actual;
                            uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                        }
                    }
                    else
                    {
                        string msg = "El usuario " + Constante.usuario_actual + " no tiene permisos en la base de datos " + Constante.db_actual;
                        uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                    }
                }
                else
                {
                    string msg = "La base de datos: " + Constante.db_actual + " no existe en el DBMS";
                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                }
            }
            else
            {
                //error por no haber utilizado la instruccion usar
                string msg = "Se debe usar la instruccion 'usar' antes de insertar valores a una Tabla";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
            }

            Constante.db_actual = "";
            Constante.usuando_db_actual = false;
            master.generar_xml();
            return estado_aceptacion;
        }
    
        public static bool actualizar(string id, List<Resultado> vals, List<string> ids, string condicion, int line, int colm)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            if (Constante.usuando_db_actual)
            {
                /*Como ya estoy en un db ahora solo debo acceder por motivos de que
                 se utilizo la funcion usar .. si entro  aqui es por que si existia
                 la base de datos*/
                //xD igual verifico por si las moscas jajaja 
                if (master.Dbs.ContainsKey(Constante.db_actual))
                {
                    //bueno aqui ya debe exister
                    //Como si existe la base de datos
                    //entonces verificar si tiene permiso para modificar el objeto
                    string usr = null;
                    foreach (string user in master.Dbs[Constante.db_actual].Usuarios)
                    {
                        if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                    }

                    if (usr != null)
                    {
                        if (master.Dbs[Constante.db_actual].Tablas.ContainsKey(id))
                        {
                            Tabla t = master.Dbs[Constante.db_actual].Tablas[id];
                            //verificar si el objeto tiene permisos de usuario
                            usr = null;
                            foreach (string user in t.Usuarios)
                            {
                                if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                            }

                            if (usr != null)
                            {
                                //como si tiene permisos para modificar la tabla entonces procedo a insertar
                                //la insercion cambia por el motivo que ahora tengo ids de atributo
                                if (vals.Count == ids.Count && ids.Count <= t.Atributos.Count)
                                {
                                    /*Para actuazliar lo pienso de esta manera segun la condicion 
                                     1. SI la condicion no es null debo realizar un select a la tabla con la condicion
                                        despues tengo los rows entonces busco las columnas a actualizar y les coloco los datos
                                        segun el tipo de dato*/

                                        //ya tengo los registros a modificar entonces debo actualizarlas
                                        DataRow[] dr = (condicion != null) ? t.Registros.Select(condicion) : t.Registros.Select();

                                        bool hayError = false;
                                        //utilizo un diccionario solo para validar si existe el id
                                        Dictionary<string, Atributo> tmp = new Dictionary<string, Atributo>();
                                        foreach (Atributo atr in t.Atributos)
                                        {
                                            tmp.Add(atr.Nombre, atr);
                                        }

                                        for (int x = 0; x < dr.Length; x++)
                                        {
                                            for (int i = 0; i < ids.Count; i++)
                                            {
                                                try
                                                {
                                                    if (tmp.ContainsKey(ids[i]))
                                                    {
                                                        if (tmp[ids[i]].Tipo == Constante.INTEGER)
                                                        {
                                                            int v = Int32.Parse(vals[i].Valor);
                                                            dr[x][tmp[ids[i]].Nombre] = v;
                                                        }
                                                        else if (tmp[ids[i]].Tipo == Constante.TEXT)
                                                        {
                                                            dr[x][tmp[ids[i]].Nombre] = vals[i].Valor;
                                                        }
                                                        else if (tmp[ids[i]].Tipo == Constante.BOOL)
                                                        {
                                                            dr[x][tmp[ids[i]].Nombre] = Boolean.Parse(vals[i].Valor);
                                                        }
                                                        else if (tmp[ids[i]].Tipo == Constante.DOUBLE)
                                                        {
                                                            dr[x][tmp[ids[i]].Nombre] = Double.Parse(vals[i].Valor, System.Globalization.CultureInfo.InvariantCulture);
                                                        }
                                                        else if (tmp[ids[i]].Tipo == Constante.DATE)
                                                        {
                                                            DateTime dtt = DateTime.Parse(vals[i].Valor);
                                                            dr[x][tmp[ids[i]].Nombre] = dtt;
                                                        }
                                                        else if (tmp[ids[i]].Tipo == Constante.DATETIME)
                                                        {
                                                            DateTime dtt = DateTime.Parse(vals[i].Valor);
                                                            dr[x][tmp[ids[i]].Nombre] = dtt;
                                                        }
                                                        else
                                                        {
                                                            hayError = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        dr[x][tmp[ids[i]].Nombre] = DBNull.Value;
                                                    }
                                                }catch(Exception ex)
                                                {
                                                    hayError = true;
                                                    string msg = ex.Message;
                                                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                                }
                                            }
                                            if (hayError) { break; }

                                        }

                                        //if (hayError)
                                        //{
                                        //    string msg = "Ocurrio un error en la actualizacion de registros";
                                        //    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                        //}
                                }
                                else
                                {
                                    string msg = "Los Valores no coinciden con el tamaño de los identificadores definidos" +
                                        " o del tamaño de los atributos de la tabla: " + t.Nombre;
                                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                }
                            }
                            else
                            {
                                string msg = "El usuario " + Constante.usuario_actual + " no tiene permiso de modificar la tabla " + t.Nombre;
                                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                            }
                        }
                        else
                        {
                            string msg = "La tabla: " + id + " no existe en la base de datos: " + Constante.db_actual;
                            uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                        }
                    }
                    else
                    {
                        string msg = "El usuario " + Constante.usuario_actual + " no tiene permisos en la base de datos " + Constante.db_actual;
                        uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                    }
                }
                else
                {
                    string msg = "La base de datos: " + Constante.db_actual + " no existe en el DBMS";
                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                }
            }
            else
            {
                //error por no haber utilizado la instruccion usar
                string msg = "Se debe usar la instruccion 'usar' antes de insertar valores a una Tabla";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
            }

            Constante.db_actual = "";
            Constante.usuando_db_actual = false;
            master.generar_xml();
            return estado_aceptacion;
        }
    }
}
