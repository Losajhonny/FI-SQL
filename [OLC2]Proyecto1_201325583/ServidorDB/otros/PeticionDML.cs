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
            
            master.generar_xml();
            return estado_aceptacion;
        }


        public static DataTable seleccionar(List<string> campos, List<string> tablas, string condicion, string id_ordernar, int tordenar, int line, int colm)
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
                        //preceso de realizar el query
                        //primero debo tener una lista de tablas
                        List<DataTable> tables = new List<DataTable>();
                        List<Tabla> tbs = new List<Tabla>();

                        /*Despues verifico si el usuario tiene permisos para acceder a las tablas*/
                        bool hayError = false;
                        foreach(string tab in tablas)
                        {
                            if (master.Dbs[Constante.db_actual].Tablas.ContainsKey(tab))
                            {
                                tables.Add(master.Dbs[Constante.db_actual].Tablas[tab].Registros);
                                tbs.Add(master.Dbs[Constante.db_actual].Tablas[tab]);
                            }
                            else
                            {
                                //si no existe mas de alguna tabla detectar error y no realizar el select
                                hayError = true;
                                string msg = "La tabla: " + tab + " no existe en la base de datos: " + Constante.db_actual;
                                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                            }
                        }
                        //ya tengo la lista de tablas ahora
                        if (!hayError)
                        {
                            //como no hay error empezar con el recorrido de las tablas
                            DataTable nuevo = cartesianProducto(tables);
                            DataRow[] datos = null;

                            //aplicando seleccion
                            try
                            {
                                //aqui realizar la ordenacion
                                /*PARA EL ORDER BY DEBO BUSCAR EL ATRIBUTO
                                 EN LA TODAS LAS TABLAS EL PRIMERO EN CONCIDIR OBTENGO
                                 LA POSICION DE LA TABLA Y SE LO ASIGNO A LA VARIABLE ID
                                 PERO REALIZAR DESPUES YA NO HAY TIEMPO XD*/
                                if (condicion != null)
                                {
                                    datos = nuevo.Select(condicion);
                                }
                                else
                                {
                                    datos = nuevo.Select();
                                }
                            }
                            catch(Exception ex)
                            {
                                //string msg = ex.Message + " debe de especificar la tabla del atributo";
                                string msg = "No existe el atributo o no especifico la tabla en el atributo" +
                                    " en la clausula donde";
                                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                hayError = true;
                            }
                            
                            if (!hayError)
                            {
                                hayError = false;
                                DataTable dnu = null;
                                DataView dv = null;

                                try
                                {
                                    dnu = datos.CopyToDataTable();
                                    dnu.TableName = "datos";
                                    dv = new DataView();
                                    dv.Table = dnu;
                                }
                                catch (Exception ex) { hayError = true; }

                                if (campos != null)
                                {
                                    //realizar las proyecciones
                                    //lo que necesito

                                    //ya lo probe jajaj si funciona pero ahora debo de buscar sus atributos
                                    //y asignarles el numero de identificacion
                                    //para eso de de recorrer los campos y en cada tabla

                                    string[,] A = new string[campos.Count, 2];

                                    for (int x = 0; x < campos.Count; x++)
                                    {
                                        for (int y = 0; y < tbs.Count; y++)
                                        {
                                            bool salir_kzy = false;
                                            for (int z = 0; z < tbs[y].Atributos.Count; z++)
                                            {
                                                bool salir_kz = false;
                                                if (tbs[y].Atributos[z].Nombre.Equals(campos[x]))
                                                {
                                                    for (int k = 0; k < A.Length; k++)
                                                    {
                                                        if (A[k, 0] != null)
                                                        {
                                                            if (A[k, 0].Equals(campos[x])
                                                                && Int32.Parse(A[k, 1]) == y)
                                                            {
                                                                //salir de k y z
                                                                salir_kz = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            A[k, 0] = campos[x];
                                                            A[k, 1] = y.ToString();
                                                            //salir de k, z, y
                                                            salir_kzy = true;
                                                        }
                                                        if (salir_kz) { break; }
                                                        if (salir_kzy) { break; }
                                                    }
                                                }
                                                if (salir_kz) { break; }
                                                if (salir_kzy) { break; }
                                            }
                                            if (salir_kzy) { break; }
                                        }
                                    }

                                    string[] col = new string[campos.Count];

                                    int faltante = 0;

                                    for (int i = 0; i < col.Length; i++)
                                    {
                                        if (A[i, 0] != null)
                                        {
                                            col[i] = A[i, 0] + A[i, 1];
                                        }
                                        else
                                        {
                                            faltante++;
                                        }
                                    }

                                    if (faltante == 0 && !hayError)
                                    {
                                        //DataTable correct = dv.ToTable("datos", false, "b0", "b1");
                                        
                                        DataTable correct = dv.ToTable("datos", false, col);
                                        return correct;
                                    }
                                    else if (faltante > 0)
                                    {
                                        string msg = "Algunos atributos no pertenece a la(s) Tabla(s)";
                                        uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                        //retornar datatable vacio
                                        return null;
                                    }
                                }
                                else
                                {
                                    //como no hay proyecciones entonces solo devolver el datatable
                                    if (!hayError)
                                    {
                                        return dnu;
                                    }
                                    else
                                    {
                                        return null;
                                    }
                                }
                            }
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
                string msg = "Se debe usar la instruccion 'usar' antes de seleccionar valores de una o varias tablas";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
            }
            
            master.generar_xml();
            return null;
        }


        public static DataTable cartesianProducto(List<DataTable> tables)
        {
            DataTable ant = null;
            int noTabla = 0;
            foreach (DataTable actual in tables)
            {
                if (ant == null) { ant = actual; }
                else
                {
                    DataTable nuevo = new DataTable("nuevo");
                    //agrego todos los atributos
                    for (int i = 0; i < ant.Columns.Count; i++)
                    {
                        if(noTabla == 0)
                        {
                            nuevo.Columns.Add(new DataColumn(ant.Columns[i].ColumnName+noTabla.ToString()));
                        }
                        else
                        {
                            nuevo.Columns.Add(new DataColumn(ant.Columns[i].ColumnName));
                        }
                    }
                    noTabla++;

                    for (int i = 0; i < actual.Columns.Count; i++)
                    {
                        nuevo.Columns.Add(new DataColumn(actual.Columns[i].ColumnName + noTabla.ToString()));
                    }
                    noTabla++;

                    //aqui ya tengo la nueva tabla con sus columnas
                    //Ahora necesito agregar los registros de las tablas por posicion
                    //y no por nombre debido a que modifique el nombre
                    foreach (DataRow rr1 in ant.Rows)
                    {
                        foreach (DataRow rr2 in actual.Rows)
                        {
                            DataRow ndr = nuevo.NewRow();
                            int noCol = 0;

                            foreach (DataColumn dc in ant.Columns)
                            {
                                ndr[noCol] = rr1[dc];
                                noCol++;
                            }
                            foreach (DataColumn dc in actual.Columns)
                            {
                                ndr[noCol] = rr2[dc];
                                noCol++;
                            }
                            nuevo.Rows.Add(ndr);
                        }
                    }

                    ant = nuevo;
                }
            }
            return ant;
        }
    }
}
