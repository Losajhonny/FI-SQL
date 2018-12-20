
using ServidorDB.analizadores.usql;
using ServidorDB.arboles.xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.otros
{
    class PeticionDDL
    {
        #region CREAR DDL
        public static bool crearDb(Db db)
        {
            bool estado_aceptacion = false;
            //realizar flujo
            //primero cargar todo el sistema de archivos
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;
            //ahora realizar la manipulacion y modificacion
            if (!master.Dbs.ContainsKey(db.Nombre))
            {
                String fechahora = Convert.ToString(DateTime.Now);
                Constante.rtb_consola.Text += ">> " + fechahora + " "+Constante.usuario_actual + " [Instruccion Crear][Crea BaseDatos: " + db.Nombre+"]\n";
                master.crear_base_datos(db.Nombre, db);
                estado_aceptacion = true;
            }
            //finalizando volver a escribir la estructura
            master.generar_xml();
            return estado_aceptacion;
        }

        public static bool crearUsuario(Usuario usr)
        {
            bool estado_aceptacion = false;
            //primero cargar todo el sistema de archivos
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            if (!master.Usuarios.ContainsKey(usr.Nombre))
            {
                String fechahora = Convert.ToString(DateTime.Now);
                Constante.rtb_consola.Text += ">> " + fechahora + " admin [El admin creo el usuario: "+usr.Nombre+"]\n";
                master.crear_usuario(usr.Nombre, usr);
                estado_aceptacion = true;
            }
            //finalizando volver a escribir la estructura
            master.generar_xml();
            return estado_aceptacion;
        }

        public static string loguear(Usuario usr)
        {
            string respuesta = "\"login\" : [ \"username\" : \"" + usr.Nombre + "\" , \"login\" : false ]";

            //primero cargar todo el sistema de archivos
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            if (master.Usuarios.ContainsKey(usr.Nombre))
            {
                if (master.Usuarios[usr.Nombre].Password.Equals(usr.Password))
                {
                    Constante.usuario_actual = usr.Nombre;
                    respuesta = "\"login\" : [ \"username\" : \""+usr.Nombre+"\" , \"login\" : true ]";
                    String fechahora = Convert.ToString(DateTime.Now);
                    Constante.rtb_consola.Text += ">> " + fechahora + " admin [El usuario " + usr.Nombre + " inicio sesion]\n";
                }
            }

            //finalizando volver a escribir la estructura
            master.generar_xml();
            return respuesta;
        }

        public static bool usarDb(string nombre, int line, int colm)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            //necesito buscar la base de datos
            if (master.Dbs.ContainsKey(nombre))
            {
                //como si existe la base de datos entonces
                //colocarlo como la base actual
                //pero antes verificar si el usuario tiene permiso
                string usr = null;
                foreach (string user in master.Dbs[nombre].Usuarios)
                {
                    if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                }

                if(usr != null)
                {   //como el usuario existe en el base  de datos entonces cargar el db
                    Constante.usuando_db_actual = true;
                    Constante.db_actual = nombre;
                    Constante.dbdb_actual = nombre;

                    if (master.Dbs.ContainsKey(Constante.dbdb_actual))
                    {
                        Constante.global.deleteSimbolDB();
                        //como ya se eliminaron proc en el entorno global entonces debo agregarlo otra vez
                        //como estos procedimientos son unicos por motivos de que la base de datos
                        //no permite valores repetidos entonces solo agregar
                        Db db = master.Dbs[Constante.db_actual];
                        foreach (Procedimiento procs in db.Procedimientos.Values)
                        {
                            Constante.global.agregar(new tabla_simbolos.Simbolo(tabla_simbolos.Simbolo.PROCEDIMIENTO, Constante.VOID, procs.Nombre, procs));
                        }
                        foreach (Funcion procs in db.Funciones.Values)
                        {
                            Constante.global.agregar(new tabla_simbolos.Simbolo(tabla_simbolos.Simbolo.FUNCION, Constante.VOID, procs.Nombre, procs));
                        }
                        foreach (Objeto procs in db.Objetos.Values)
                        {
                            Constante.global.agregar(new tabla_simbolos.Simbolo(tabla_simbolos.Simbolo.OBJETO, Constante.ID, procs.Nombre, procs));
                        }
                    }
                }
                else
                {
                    Constante.usuando_db_actual = false;
                    Constante.db_actual = "";
                    string msg = "El usuario " + Constante.usuario_actual + " no tiene permisos en la base de datos " + nombre;
                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                }

                estado_aceptacion = true;
            }
            master.generar_xml();
            return estado_aceptacion;
        }

        public static bool crearTabla(Tabla t)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            //ya tengo cargado el sistema de archivos
            //necesito validar si se ejecuto la instruccion usar
            if (Constante.usuando_db_actual)
            {
                //Si se ejecuto usar antes
                //validar si existe la base de datos
                if (master.Dbs.ContainsKey(Constante.db_actual))
                {
                    //Como si existe la base de datos
                    //entonces verificar si tiene permiso para insertar tabla
                    string usr = null;
                    foreach (string user in master.Dbs[Constante.db_actual].Usuarios)
                    {
                        if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                    }

                    if (usr != null)
                    {
                        //como si tiene permiso entonces insertar la tabla en la base de datos
                        //ahora necesito verificar si existe la tabla
                        if (!master.Dbs[Constante.db_actual].Tablas.ContainsKey(t.Nombre))
                        {   //Si no existe entonces agregar la tabla
                            //antes de agregarlo debo modificar los permisos
                            
                            //el if es por cuestiones de pruebas
                            //se puede quitar el if con equals ("")
                            if (!Constante.usuario_actual.Equals("") &&
                                !Constante.usuario_actual.Equals(Constante.usuario_admin))
                            {
                                t.Usuarios.Add(Constante.usuario_actual);
                            }

                            //el administrador tiene todos los permisos
                            t.Usuarios.Add(Constante.usuario_admin);

                            String fechahora = Convert.ToString(DateTime.Now);
                            Constante.rtb_consola.Text += ">> " + fechahora + " " + Constante.usuario_actual + " [Instruccion Crear][Crea tabla: " + t.Nombre + "]\n";
                            master.Dbs[Constante.db_actual].Tablas.Add(t.Nombre, t);
                            estado_aceptacion = true;
                        }
                        else
                        {
                            string msg = "La tabla: " + t.Nombre + " ya existe en la base de datos: " + Constante.db_actual;
                            uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", t.Line, t.Colm));
                        }
                    }
                    else
                    {
                        string msg = "El usuario " + Constante.usuario_actual + " no tiene permisos en la base de datos " + Constante.db_actual;
                        uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", t.Line, t.Colm));
                    }
                }
                else
                {
                    string msg = "La base de datos: " + Constante.db_actual + " no existe en el DBMS";
                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", t.Line, t.Colm));
                }
            }
            else
            {
                //error por no haber utilizado la instruccion usar
                string msg = "Se debe usar la instruccion 'usar' antes de crear una Tabla";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", t.Line, t.Colm));
            }
            Constante.db_actual = "";
            Constante.usuando_db_actual = false;
            master.generar_xml();
            return estado_aceptacion;
        }

        public static bool crearObjeto(Objeto obj)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            //ya tengo cargado el sistema de archivos
            //necesito validar si se ejecuto la instruccion usar
            if (Constante.usuando_db_actual)
            {
                //Si se ejecuto usar antes
                //validar si existe la base de datos
                if (master.Dbs.ContainsKey(Constante.db_actual))
                {
                    //Como si existe la base de datos
                    //entonces verificar si tiene permiso para insertar el objeto
                    string usr = null;
                    foreach (string user in master.Dbs[Constante.db_actual].Usuarios)
                    {
                        if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                    }

                    if (usr != null)
                    {
                        //como si tiene permiso entonces insertar el objeto en la base de datos
                        //ahora necesito verificar si existe el objeto
                        if (!master.Dbs[Constante.db_actual].Objetos.ContainsKey(obj.Nombre))
                        {   //Si no existe entonces agregar el objeto
                            //antes de agregarlo debo modificar los permisos
                            //el if es por cuestiones de pruebas
                            //se puede quitar el if con equals ("")
                            if (!Constante.usuario_actual.Equals("") &&
                                !Constante.usuario_actual.Equals(Constante.usuario_admin))
                            {
                                obj.Usuarios.Add(Constante.usuario_actual);
                            }

                            //el administrador tiene todos los permisos
                            //Despues coloco el admin para porteriores verificaciones
                            obj.Usuarios.Add(Constante.usuario_admin);

                            String fechahora = Convert.ToString(DateTime.Now);
                            Constante.rtb_consola.Text += ">> " + fechahora + " " + Constante.usuario_actual + " [Instruccion Crear][Crea objeto: " + obj.Nombre + "]\n";
                            master.Dbs[Constante.db_actual].Objetos.Add(obj.Nombre, obj);
                            estado_aceptacion = true;
                        }
                        else
                        {
                            string msg = "El objeto: " + obj.Nombre + " ya existe en la base de datos: " + Constante.db_actual;
                            uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", obj.Line, obj.Colm));
                        }
                    }
                    else
                    {
                        string msg = "El usuario " + Constante.usuario_actual + " no tiene permisos en la base de datos " + Constante.db_actual;
                        uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", obj.Line, obj.Colm));
                    }
                }
                else
                {
                    string msg = "La base de datos: " + Constante.db_actual + " no existe en el DBMS";
                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", obj.Line, obj.Colm));
                }
            }
            else
            {
                //error por no haber utilizado la instruccion usar
                string msg = "Se debe usar la instruccion 'usar' antes de crear un Objeto";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", obj.Line, obj.Colm));
            }

            Constante.db_actual = "";
            Constante.usuando_db_actual = false;
            master.generar_xml();
            return estado_aceptacion;
        }

        public static bool crearFuncion(Funcion fun)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            //ya tengo cargado el sistema de archivos
            //necesito validar si se ejecuto la instruccion usar
            if (Constante.usuando_db_actual)
            {
                //Si se ejecuto usar antes
                //validar si existe la base de datos
                if (master.Dbs.ContainsKey(Constante.db_actual))
                {
                    //Como si existe la base de datos
                    //entonces verificar si tiene permiso para insertar el objeto
                    string usr = null;
                    foreach (string user in master.Dbs[Constante.db_actual].Usuarios)
                    {
                        if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                    }

                    if (usr != null)
                    {
                        //como si tiene permiso entonces insertar el objeto en la base de datos
                        //ahora necesito verificar si existe el objeto
                        if (!master.Dbs[Constante.db_actual].Funciones.ContainsKey(fun.Nombre))
                        {   //Si no existe entonces agregar el objeto
                            //antes de agregarlo debo modificar los permisos
                            //el if es por cuestiones de pruebas
                            //se puede quitar el if con equals ("")
                            if (!Constante.usuario_actual.Equals("") &&
                                !Constante.usuario_actual.Equals(Constante.usuario_admin))
                            {
                                fun.Usuarios.Add(Constante.usuario_actual);
                            }

                            //el administrador tiene todos los permisos
                            //Despues coloco el admin para porteriores verificaciones
                            fun.Usuarios.Add(Constante.usuario_admin);

                            String fechahora = Convert.ToString(DateTime.Now);
                            Constante.rtb_consola.Text += ">> " + fechahora + " " + Constante.usuario_actual + " [Instruccion Crear][Crea funcion: " + fun.Nombre + "]\n";
                            master.Dbs[Constante.db_actual].Funciones.Add(fun.Nombre, fun);
                            estado_aceptacion = true;
                        }
                        else
                        {
                            string msg = "La Funcion: " + fun.Nombre + " ya existe en la base de datos: " + Constante.db_actual;
                            uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", fun.Line, fun.Colm));
                        }
                    }
                    else
                    {
                        string msg = "El usuario " + Constante.usuario_actual + " no tiene permisos en la base de datos " + Constante.db_actual;
                        uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", fun.Line, fun.Colm));
                    }
                }
                else
                {
                    string msg = "La base de datos: " + Constante.db_actual + " no existe en el DBMS";
                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", fun.Line, fun.Colm));
                }
            }
            else
            {
                //error por no haber utilizado la instruccion usar
                string msg = "Se debe usar la instruccion 'usar' antes de crear una Funcion";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", fun.Line, fun.Colm));
            }
            master.generar_xml();
            return estado_aceptacion;
        }

        public static bool crearProcedimiento(Procedimiento fun)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            //ya tengo cargado el sistema de archivos
            //necesito validar si se ejecuto la instruccion usar
            if (Constante.usuando_db_actual)
            {
                //Si se ejecuto usar antes
                //validar si existe la base de datos
                if (master.Dbs.ContainsKey(Constante.db_actual))
                {
                    //Como si existe la base de datos
                    //entonces verificar si tiene permiso para insertar el objeto
                    string usr = null;
                    foreach (string user in master.Dbs[Constante.db_actual].Usuarios)
                    {
                        if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                    }

                    if (usr != null)
                    {
                        //como si tiene permiso entonces insertar el objeto en la base de datos
                        //ahora necesito verificar si existe el objeto
                        if (!master.Dbs[Constante.db_actual].Procedimientos.ContainsKey(fun.Nombre))
                        {   //Si no existe entonces agregar el objeto
                            //antes de agregarlo debo modificar los permisos
                            //el if es por cuestiones de pruebas
                            //se puede quitar el if con equals ("")
                            if (!Constante.usuario_actual.Equals("") &&
                                !Constante.usuario_actual.Equals(Constante.usuario_admin))
                            {
                                fun.Usuarios.Add(Constante.usuario_actual);
                            }

                            //el administrador tiene todos los permisos
                            //Despues coloco el admin para porteriores verificaciones
                            fun.Usuarios.Add(Constante.usuario_admin);

                            String fechahora = Convert.ToString(DateTime.Now);
                            Constante.rtb_consola.Text += ">> " + fechahora + " " + Constante.usuario_actual + " [Instruccion Crear][Crea procedimiento: " + fun.Nombre + "]\n";
                            master.Dbs[Constante.db_actual].Procedimientos.Add(fun.Nombre, fun);
                            estado_aceptacion = true;
                        }
                        else
                        {
                            string msg = "El procedimiento: " + fun.Nombre + " ya existe en la base de datos: " + Constante.db_actual;
                            uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", fun.Line, fun.Colm));
                        }
                    }
                    else
                    {
                        string msg = "El usuario " + Constante.usuario_actual + " no tiene permisos en la base de datos " + Constante.db_actual;
                        uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", fun.Line, fun.Colm));
                    }
                }
                else
                {
                    string msg = "La base de datos: " + Constante.db_actual + " no existe en el DBMS";
                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", fun.Line, fun.Colm));
                }
            }
            else
            {
                //error por no haber utilizado la instruccion usar
                string msg = "Se debe usar la instruccion 'usar' antes de crear un procedimiento";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", fun.Line, fun.Colm));
            }
            master.generar_xml();
            return estado_aceptacion;
        }
        #endregion

        #region ALTER DDL
        public static bool quitarTabla(List<string> ids, string nombre, int line, int colm)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            //ya tengo cargado el sistema de archivos
            //necesito validar si se ejecuto la instruccion usar
            if (Constante.usuando_db_actual)
            {
                //Si se ejecuto usar antes
                //validar si existe la base de datos
                if (master.Dbs.ContainsKey(Constante.db_actual))
                {
                    //Como si existe la base de datos
                    //entonces verificar si tiene permiso para modificar el objeto
                    string usr = null;
                    foreach (string user in master.Dbs[Constante.db_actual].Usuarios)
                    {
                        if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                    }

                    if (usr != null)
                    {
                        //como si tengo permisos entonces buscar el objeto
                        if (master.Dbs[Constante.db_actual].Tablas.ContainsKey(nombre))
                        {
                            Tabla t = master.Dbs[Constante.db_actual].Tablas[nombre];
                            //verificar si el objeto tiene permisos de usuario
                            usr = null;
                            foreach (string user in t.Usuarios)
                            {
                                if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                            }
                            //verifico si el usuario actual tiene permisos para el objeto
                            if (usr != null)
                            {
                                List<string> si_existen = new List<string>();
                                //verificar que existen las columnas
                                for (int i = 0; i < ids.Count; i++)
                                {
                                    bool existe = false;
                                    foreach (Atributo atr in t.Atributos)
                                    {
                                        if (atr.Nombre.Equals(ids[i]))
                                        {
                                            t.Atributos.Remove(atr);
                                            si_existen.Add(ids[i]);
                                            existe = true;
                                            break;
                                        }
                                    }

                                    if (!existe)
                                    {
                                        string msg = "El atributo: " + ids[i] + " no existe en la tabla: " + t.Nombre;
                                        uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                    }
                                }

                                //ahora necesito eliminar las columnas asociadas al de los atributos
                                for(int i = 0; i < si_existen.Count; i++)
                                {
                                    t.Registros.Columns.Remove(si_existen[i]);
                                }
                                String fechahora = Convert.ToString(DateTime.Now);
                                Constante.rtb_consola.Text += ">> " + fechahora + " " + Constante.usuario_actual + " [Instruccion Alter][Quita atributos tabla: " + t.Nombre + "]\n";
                                estado_aceptacion = true;
                            }
                            else
                            {
                                string msg = "El usuario " + Constante.usuario_actual + " no tiene permiso de modificar la tabla " + t.Nombre;
                                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                            }
                        }
                        else
                        {
                            string msg = "El objeto: " + nombre + " no existe en la base de datos: " + Constante.db_actual;
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
                string msg = "Se debe usar la instruccion 'usar' antes de agregar atributos a una Tabla";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
            }
            Constante.db_actual = "";
            Constante.usuando_db_actual = false;
            master.generar_xml();
            return estado_aceptacion;
        }

        public static bool agregarTabla(string nombre, List<Atributo> atrs, int line, int colm)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            //ya tengo cargado el sistema de archivos
            //necesito validar si se ejecuto la instruccion usar
            if (Constante.usuando_db_actual)
            {
                //Si se ejecuto usar antes
                //validar si existe la base de datos
                if (master.Dbs.ContainsKey(Constante.db_actual))
                {
                    //Como si existe la base de datos
                    //entonces verificar si tiene permiso para modificar el objeto
                    string usr = null;
                    foreach (string user in master.Dbs[Constante.db_actual].Usuarios)
                    {
                        if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                    }

                    if (usr != null)
                    {
                        //como si tengo permisos entonces buscar el objeto
                        if (master.Dbs[Constante.db_actual].Tablas.ContainsKey(nombre))
                        {
                            Tabla t = master.Dbs[Constante.db_actual].Tablas[nombre];
                            //verificar si el objeto tiene permisos de usuario
                            usr = null;
                            foreach (string user in t.Usuarios)
                            {
                                if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                            }
                            //verifico si el usuario actual tiene permisos para el objeto
                            if (usr != null)
                            {
                                List<Atributo> agregar = new List<Atributo>();
                                //sirve para verificar si se agrego mas de algun atributo
                                bool agrego_atri = false;
                                //verificar si existe
                                for (int i = 0; i < atrs.Count; i++)
                                {   
                                    bool existe = false;

                                    foreach (Atributo atributo in t.Atributos)
                                    {
                                        if (atributo.Nombre.Equals(atrs[i].Nombre))
                                        {
                                            existe = true;
                                            break;
                                        }
                                    }

                                    if (existe)
                                    {
                                        string msg = "El atributo: " + atrs[i].Nombre + " ya existe en la tabla: " + t.Nombre;
                                        uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                    }
                                    else
                                    {
                                        agregar.Add(atrs[i]);
                                        t.Atributos.Add(atrs[i]);
                                        agrego_atri = true;
                                    }
                                }

                                if (agrego_atri)
                                {
                                    //logica para cambiar DataTable si perder registros
                                    //Solo agregar columnas
                                    //Le inserto las columnas
                                    DataTable dt = t.Registros;
                                    for (int i = 0; i < agregar.Count; i++)
                                    {
                                        //inserto columna por atributo
                                        //para simular la tabla con sus registros
                                        DataColumn dc = new DataColumn(agregar[i].Nombre);

                                        if (agregar[i].Tipo == Constante.INTEGER)
                                        {
                                            dc.DataType = System.Type.GetType("System.Int32");
                                            dt.Columns.Add(dc);
                                        }
                                        else if (agregar[i].Tipo == Constante.TEXT)
                                        {
                                            dc.DataType = System.Type.GetType("System.String");
                                            dt.Columns.Add(dc);
                                        }
                                        else if (agregar[i].Tipo == Constante.BOOL)
                                        {
                                            dc.DataType = System.Type.GetType("System.Boolean");
                                            dt.Columns.Add(dc);
                                        }
                                        else if (agregar[i].Tipo == Constante.DOUBLE)
                                        {
                                            dc.DataType = System.Type.GetType("System.Double");
                                            dt.Columns.Add(dc);
                                        }
                                        else if (agregar[i].Tipo == Constante.DATE)
                                        {
                                            dc.DataType = System.Type.GetType("System.DateTime");
                                            dt.Columns.Add(dc);
                                        }
                                        else if (agregar[i].Tipo == Constante.DATETIME)
                                        {
                                            dc.DataType = System.Type.GetType("System.DateTime");
                                            dt.Columns.Add(dc);
                                        }

                                    }
                                    String fechahora = Convert.ToString(DateTime.Now);
                                    Constante.rtb_consola.Text += ">> " + fechahora + " " + Constante.usuario_actual + " [Instruccion Alter][Agrega atributos tabla: " + t.Nombre + "]\n";
                                }
                                estado_aceptacion = true;
                            }
                            else
                            {
                                string msg = "El usuario " + Constante.usuario_actual + " no tiene permiso de modificar la tabla " + t.Nombre;
                                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                            }
                        }
                        else
                        {
                            string msg = "El objeto: " + nombre + " no existe en la base de datos: " + Constante.db_actual;
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
                string msg = "Se debe usar la instruccion 'usar' antes de agregar atributos a una Tabla";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
            }
            Constante.db_actual = "";
            Constante.usuando_db_actual = false;
            master.generar_xml();
            return estado_aceptacion;
        }

        public static bool quitarObjeto(List<string> ids, string nombre, int line, int colm)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            //ya tengo cargado el sistema de archivos
            //necesito validar si se ejecuto la instruccion usar
            if (Constante.usuando_db_actual)
            {
                //Si se ejecuto usar antes
                //validar si existe la base de datos
                if (master.Dbs.ContainsKey(Constante.db_actual))
                {
                    //Como si existe la base de datos
                    //entonces verificar si tiene permiso para modificar el objeto
                    string usr = null;
                    foreach (string user in master.Dbs[Constante.db_actual].Usuarios)
                    {
                        if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                    }

                    if (usr != null)
                    {
                        //como si tengo permisos entonces buscar el objeto
                        if (master.Dbs[Constante.db_actual].Objetos.ContainsKey(nombre))
                        {
                            Objeto obj = master.Dbs[Constante.db_actual].Objetos[nombre];

                            usr = null;
                            foreach (string user in obj.Usuarios)
                            {
                                if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                            }

                            if(usr != null)
                            {
                                //debo verificar si existen los atributos

                                //aqui debo de eliminar atributos
                                for (int i = 0; i < ids.Count; i++)
                                {
                                    bool existe = false;
                                    foreach (Atributo atr in obj.Parametros)
                                    {
                                        if (atr.Nombre.Equals(ids[i]))
                                        {
                                            obj.Parametros.Remove(atr);
                                            existe = true;
                                            break;
                                        }
                                    }

                                    if (!existe)
                                    {
                                        string msg = "El atributo: " + ids[i] + " no existe en el objeto: " + obj.Nombre;
                                        uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                    }
                                }
                                String fechahora = Convert.ToString(DateTime.Now);
                                Constante.rtb_consola.Text += ">> " + fechahora + " " + Constante.usuario_actual + " [Instruccion Alter][Quita atributos objeto: " + obj.Nombre + "]\n";
                                estado_aceptacion = true;
                            }
                            else
                            {
                                string msg = "El usuario " + Constante.usuario_actual + " no tiene permiso de modificar el objeto " + obj.Nombre;
                                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                            }
                        }
                        else
                        {
                            string msg = "El objeto: " + nombre + " no existe en la base de datos: " + Constante.db_actual;
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
                string msg = "Se debe usar la instruccion 'usar' antes de quitar atributos un Objeto";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
            }
            Constante.db_actual = "";
            Constante.usuando_db_actual = false;
            master.generar_xml();
            return estado_aceptacion;
        }

        public static bool agregarObjeto(string nombre, List<Atributo> atrs, int line, int colm)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            //ya tengo cargado el sistema de archivos
            //necesito validar si se ejecuto la instruccion usar
            if (Constante.usuando_db_actual)
            {
                //Si se ejecuto usar antes
                //validar si existe la base de datos
                if (master.Dbs.ContainsKey(Constante.db_actual))
                {
                    //Como si existe la base de datos
                    //entonces verificar si tiene permiso para modificar el objeto
                    string usr = null;
                    foreach (string user in master.Dbs[Constante.db_actual].Usuarios)
                    {
                        if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                    }

                    if (usr != null)
                    {
                        //como si tengo permisos entonces buscar el objeto
                        if (master.Dbs[Constante.db_actual].Objetos.ContainsKey(nombre))
                        {
                            Objeto obj = master.Dbs[Constante.db_actual].Objetos[nombre];
                            //verificar si el objeto tiene permisos de usuario
                            usr = null;
                            foreach (string user in obj.Usuarios)
                            {
                                if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                            }
                            //verifico si el usuario actual tiene permisos para el objeto
                            if(usr != null)
                            {
                                //verificar si existe
                                for (int i = 0; i < atrs.Count; i++)
                                {
                                    bool existe = false;

                                    foreach (Atributo atributo in obj.Parametros)
                                    {
                                        if (atributo.Nombre.Equals(atrs[i].Nombre))
                                        {
                                            existe = true;
                                            break;
                                        }
                                    }

                                    if (existe)
                                    {
                                        string msg = "El atributo: " + atrs[i].Nombre + " ya existe en el objeto: " + obj.Nombre;
                                        uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                    }
                                    else
                                    {
                                        obj.Parametros.Add(atrs[i]);
                                    }
                                }
                                String fechahora = Convert.ToString(DateTime.Now);
                                Constante.rtb_consola.Text += ">> " + fechahora + " " + Constante.usuario_actual + " [Instruccion Alter][Agrega atributos objeto: " + obj.Nombre + "]\n";
                                estado_aceptacion = true;
                            }
                            else
                            {
                                string msg = "El usuario " + Constante.usuario_actual + " no tiene permiso de modificar el objeto " + obj.Nombre;
                                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                            }
                        }
                        else
                        {
                            string msg = "El objeto: " + nombre + " no existe en la base de datos: " + Constante.db_actual;
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
                string msg = "Se debe usar la instruccion 'usar' antes de agregar atributos a un Objeto";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
            }
            Constante.db_actual = "";
            Constante.usuando_db_actual = false;
            master.generar_xml();
            return estado_aceptacion;
        }

        public static bool cambiarPassword(string id, string password, int line, int colm)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            /*No se necesita saber si anteriormente se uso la instruccion usar*/
            /*No se necesita que acceda a una base de datos*/

            //tengo que revisar si el usuario a modificar existe enn la base de datos

            //verificando si el usuarios es administrador
            if (Constante.usuario_actual.Equals(Constante.usuario_admin))
            {
                //si es administrador puede modificar los usuarios a su antojo
                //obtener el usuario al que hay que cambiar la contraseña
                if (master.Usuarios.ContainsKey(id))
                {
                    Usuario usr = master.Usuarios[id];
                    usr.Password = password;
                    String fechahora = Convert.ToString(DateTime.Now);
                    Constante.rtb_consola.Text += ">> " + fechahora + " " + Constante.usuario_actual + " [Instruccion Alter][Cambia password al usuario: " + usr.Nombre + "]\n";
                    estado_aceptacion = true;
                }
                else
                {
                    string msg = "El usuario: " + id + " no existe en el DBMS";
                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                }
            }
            else
            {
                //entonces no es el administrador que va a cambiar una contraseña
                //verificar si el usuario actual es igual al id
                if (master.Usuarios.ContainsKey(id))
                {
                    if (Constante.usuario_actual.Equals(id))
                    {
                        Usuario usr = master.Usuarios[id];
                        usr.Password = password;
                        String fechahora = Convert.ToString(DateTime.Now);
                        Constante.rtb_consola.Text += ">> " + fechahora + " " + Constante.usuario_actual + " [Instruccion Alter][Cambia password al usuario: " + usr.Nombre + "]\n";
                        estado_aceptacion = true;
                    }
                    else
                    {
                        string msg = "El usuario: " + Constante.usuario_actual + " no es administrador para modificar otros usuarios";
                        uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                    }
                }
                else
                {
                    string msg = "El usuario: " + id + " no existe en el DBMS";
                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                }
            }

            master.generar_xml();
            return estado_aceptacion;
        }
        #endregion

        #region DROP DDL
        public static bool dropTabla(string nombre, int line, int colm)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            //ya tengo cargado el sistema de archivos
            //necesito validar si se ejecuto la instruccion usar
            if (Constante.usuando_db_actual)
            {
                //Si se ejecuto usar antes
                //validar si existe la base de datos
                if (master.Dbs.ContainsKey(Constante.db_actual))
                {
                    //Como si existe la base de datos
                    //entonces verificar si tiene permiso para modificar el objeto
                    string usr = null;
                    foreach (string user in master.Dbs[Constante.db_actual].Usuarios)
                    {
                        if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                    }

                    if (usr != null)
                    {
                        //como si tengo permisos entonces buscar el objeto
                        if (master.Dbs[Constante.db_actual].Tablas.ContainsKey(nombre))
                        {
                            Tabla obj = master.Dbs[Constante.db_actual].Tablas[nombre];
                            //verificar si el objeto tiene permisos de usuario
                            usr = null;
                            foreach (string user in obj.Usuarios)
                            {
                                if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                            }
                            //verifico si el usuario actual tiene permisos para el objeto
                            if (usr != null || obj.Usuarios.Count == 0)
                            {
                                /*casos
                                 1. si el usuario actual es el administrador entonces eliminar
                                 
                                 2. sino no existe usuario entonces eliminar por motivos que no 
                                 tiene ningun usuario

                                 3. sino buscar el propietario que se supone que esta en la posicion
                                 0 de la lista si el usuario encontrado es igual al usuario actual
                                 entonces eliminar*/

                                /*PARA ELIMINAR UNA TABLA HAY QUE ELIMINAR SUS REGISTROS
                                 para eso necesito eliminar el archivo que guarda los registros*/

                                if (Constante.usuario_actual.Equals(Constante.usuario_admin))
                                {
                                    //como el usuario actual es el administrador entonces eliminar el objeto
                                    master.Dbs[Constante.db_actual].Tablas.Remove(nombre);
                                    string path = Constante.RUTA_TABLA + obj.Nombre + "." + Constante.EXTENSION;
                                    if (File.Exists(path))
                                    {
                                        File.Delete(path);
                                    }
                                }
                                else if (obj.Usuarios.Count == 0)
                                {
                                    //como por un error se elimino el administrador y no existe usuarios
                                    master.Dbs[Constante.db_actual].Tablas.Remove(nombre);
                                    string path = Constante.RUTA_TABLA + obj.Nombre + "." + Constante.EXTENSION;
                                    if (File.Exists(path))
                                    {
                                        File.Delete(path);
                                    }
                                }
                                else if (obj.Usuarios[0].Equals(Constante.usuario_actual))
                                {
                                    //si paso aqui es por que el usuario en la posicion [0] es el propietario
                                    master.Dbs[Constante.db_actual].Tablas.Remove(nombre);
                                    string path = Constante.RUTA_TABLA + obj.Nombre + "." + Constante.EXTENSION;
                                    if (File.Exists(path))
                                    {
                                        File.Delete(path);
                                    }
                                }
                                else
                                {
                                    string msg = "El usuario: " + Constante.usuario_actual + " no es el propietario. ";
                                    msg += "Solo el propietario puede eliminar la tabla";
                                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                }
                                String fechahora = Convert.ToString(DateTime.Now);
                                Constante.rtb_consola.Text += ">> " + fechahora + " " + Constante.usuario_actual + " [Instruccion Drop][Elimina tabla: " + nombre + "]\n";
                                estado_aceptacion = true;
                            }
                            else
                            {
                                string msg = "El usuario " + Constante.usuario_actual + " no tiene permiso de modificar la tabla: " + obj.Nombre;
                                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                            }
                        }
                        else
                        {
                            string msg = "La tabla: " + nombre + " no existe en la base de datos: " + Constante.db_actual;
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
                string msg = "Se debe usar la instruccion 'usar' antes de eliminar una tabla";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
            }
            master.generar_xml();
            return estado_aceptacion;
        }

        public static bool dropFuncion(string nombre, int line, int colm)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            //ya tengo cargado el sistema de archivos
            //necesito validar si se ejecuto la instruccion usar
            if (Constante.usuando_db_actual)
            {
                //Si se ejecuto usar antes
                //validar si existe la base de datos
                if (master.Dbs.ContainsKey(Constante.db_actual))
                {
                    //Como si existe la base de datos
                    //entonces verificar si tiene permiso para modificar el objeto
                    string usr = null;
                    foreach (string user in master.Dbs[Constante.db_actual].Usuarios)
                    {
                        if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                    }

                    if (usr != null)
                    {
                        //como si tengo permisos entonces buscar el objeto
                        if (master.Dbs[Constante.db_actual].Funciones.ContainsKey(nombre))
                        {
                            Funcion obj = master.Dbs[Constante.db_actual].Funciones[nombre];
                            //verificar si el objeto tiene permisos de usuario
                            usr = null;
                            foreach (string user in obj.Usuarios)
                            {
                                if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                            }
                            //verifico si el usuario actual tiene permisos para el objeto
                            if (usr != null || obj.Usuarios.Count == 0)
                            {
                                /*casos
                                 1. si el usuario actual es el administrador entonces eliminar
                                 
                                 2. sino no existe usuario entonces eliminar por motivos que no 
                                 tiene ningun usuario

                                 3. sino buscar el propietario que se supone que esta en la posicion
                                 0 de la lista si el usuario encontrado es igual al usuario actual
                                 entonces eliminar*/

                                if (Constante.usuario_actual.Equals(Constante.usuario_admin))
                                {
                                    //como el usuario actual es el administrador entonces eliminar el objeto
                                    master.Dbs[Constante.db_actual].Funciones.Remove(nombre);
                                }
                                else if (obj.Usuarios.Count == 0)
                                {
                                    //como por un error se elimino el administrador y no existe usuarios
                                    master.Dbs[Constante.db_actual].Funciones.Remove(nombre);
                                }
                                else if (obj.Usuarios[0].Equals(Constante.usuario_actual))
                                {
                                    //si paso aqui es por que el usuario en la posicion [0] es el propietario
                                    master.Dbs[Constante.db_actual].Funciones.Remove(nombre);
                                }
                                else
                                {
                                    string msg = "El usuario: " + Constante.usuario_actual + " no es el propietario. ";
                                    msg += "Solo el propietario puede eliminar la funcion";
                                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                }
                                String fechahora = Convert.ToString(DateTime.Now);
                                Constante.rtb_consola.Text += ">> " + fechahora + " " + Constante.usuario_actual + " [Instruccion Drop][Elimina funcion: " + nombre + "]\n";
                                estado_aceptacion = true;
                            }
                            else
                            {
                                string msg = "El usuario " + Constante.usuario_actual + " no tiene permiso de modificar la funcion: " + obj.Nombre;
                                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                            }
                        }
                        else
                        {
                            string msg = "La funcion: " + nombre + " no existe en la base de datos: " + Constante.db_actual;
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
                string msg = "Se debe usar la instruccion 'usar' antes de eliminar una funcion";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
            }
            master.generar_xml();
            return estado_aceptacion;
        }

        public static bool dropObjeto(string nombre, int line, int colm)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            //ya tengo cargado el sistema de archivos
            //necesito validar si se ejecuto la instruccion usar
            if (Constante.usuando_db_actual)
            {
                //Si se ejecuto usar antes
                //validar si existe la base de datos
                if (master.Dbs.ContainsKey(Constante.db_actual))
                {
                    //Como si existe la base de datos
                    //entonces verificar si tiene permiso para modificar el objeto
                    string usr = null;
                    foreach (string user in master.Dbs[Constante.db_actual].Usuarios)
                    {
                        if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                    }

                    if (usr != null)
                    {
                        //como si tengo permisos entonces buscar el objeto
                        if (master.Dbs[Constante.db_actual].Objetos.ContainsKey(nombre))
                        {
                            Objeto obj = master.Dbs[Constante.db_actual].Objetos[nombre];
                            //verificar si el objeto tiene permisos de usuario
                            usr = null;
                            foreach (string user in obj.Usuarios)
                            {
                                if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                            }
                            //verifico si el usuario actual tiene permisos para el objeto
                            if (usr != null || obj.Usuarios.Count == 0)
                            {
                                /*casos
                                 1. si el usuario actual es el administrador entonces eliminar
                                 
                                 2. sino no existe usuario entonces eliminar por motivos que no 
                                 tiene ningun usuario

                                 3. sino buscar el propietario que se supone que esta en la posicion
                                 0 de la lista si el usuario encontrado es igual al usuario actual
                                 entonces eliminar*/

                                if (Constante.usuario_actual.Equals(Constante.usuario_admin))
                                {
                                    //como el usuario actual es el administrador entonces eliminar el objeto
                                    master.Dbs[Constante.db_actual].Objetos.Remove(nombre);
                                }
                                else if (obj.Usuarios.Count == 0)
                                {
                                    //como por un error se elimino el administrador y no existe usuarios
                                    master.Dbs[Constante.db_actual].Objetos.Remove(nombre);
                                }
                                else if (obj.Usuarios[0].Equals(Constante.usuario_actual))
                                {
                                    //si paso aqui es por que el usuario en la posicion [0] es el propietario
                                    master.Dbs[Constante.db_actual].Objetos.Remove(nombre);
                                }
                                else
                                {
                                    string msg = "El usuario: " + Constante.usuario_actual + " no es el propietario. ";
                                    msg += "Solo el propietario puede eliminar el objeto";
                                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                }

                                String fechahora = Convert.ToString(DateTime.Now);
                                Constante.rtb_consola.Text += ">> " + fechahora + " " + Constante.usuario_actual + " [Instruccion Drop][Elimina objeto: " + nombre + "]\n";
                                estado_aceptacion = true;
                            }
                            else
                            {
                                string msg = "El usuario " + Constante.usuario_actual + " no tiene permiso de modificar el objeto: " + obj.Nombre;
                                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                            }
                        }
                        else
                        {
                            string msg = "El objeto: " + nombre + " no existe en la base de datos: " + Constante.db_actual;
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
                string msg = "Se debe usar la instruccion 'usar' antes de eliminar un objeto";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
            }
            master.generar_xml();
            return estado_aceptacion;
        }

        public static bool dropProcedimiento(string nombre, int line, int colm)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            //ya tengo cargado el sistema de archivos
            //necesito validar si se ejecuto la instruccion usar
            if (Constante.usuando_db_actual)
            {
                //Si se ejecuto usar antes
                //validar si existe la base de datos
                if (master.Dbs.ContainsKey(Constante.db_actual))
                {
                    //Como si existe la base de datos
                    //entonces verificar si tiene permiso para modificar el objeto
                    string usr = null;
                    foreach (string user in master.Dbs[Constante.db_actual].Usuarios)
                    {
                        if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                    }

                    if (usr != null)
                    {
                        //como si tengo permisos entonces buscar el objeto
                        if (master.Dbs[Constante.db_actual].Procedimientos.ContainsKey(nombre))
                        {
                            Procedimiento proc = master.Dbs[Constante.db_actual].Procedimientos[nombre];
                            //verificar si el objeto tiene permisos de usuario
                            usr = null;
                            foreach (string user in proc.Usuarios)
                            {
                                if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                            }
                            //verifico si el usuario actual tiene permisos para el objeto
                            if (usr != null || proc.Usuarios.Count == 0)
                            {
                                /*casos
                                 1. si el usuario actual es el administrador entonces eliminar
                                 
                                 2. sino no existe usuario entonces eliminar por motivos que no 
                                 tiene ningun usuario

                                 3. sino buscar el propietario que se supone que esta en la posicion
                                 0 de la lista si el usuario encontrado es igual al usuario actual
                                 entonces eliminar*/

                                if (Constante.usuario_actual.Equals(Constante.usuario_admin))
                                {
                                    //como el usuario actual es el administrador entonces eliminar el objeto
                                    master.Dbs[Constante.db_actual].Procedimientos.Remove(nombre);
                                }
                                else if (proc.Usuarios.Count == 0)
                                {
                                    //como por un error se elimino el administrador y no existe usuarios
                                    master.Dbs[Constante.db_actual].Procedimientos.Remove(nombre);
                                }
                                else if (proc.Usuarios[0].Equals(Constante.usuario_actual))
                                {
                                    //si paso aqui es por que el usuario en la posicion [0] es el propietario
                                    master.Dbs[Constante.db_actual].Procedimientos.Remove(nombre);
                                }
                                else
                                {
                                    string msg = "El usuario: " + Constante.usuario_actual + " no es el propietario. ";
                                    msg += "Solo el propietario puede eliminar el procedimiento";
                                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                }

                                String fechahora = Convert.ToString(DateTime.Now);
                                Constante.rtb_consola.Text += ">> " + fechahora + " " + Constante.usuario_actual + " [Instruccion Drop][Elimina procedimiento: " + nombre + "]\n";
                                estado_aceptacion = true;
                            }
                            else
                            {
                                string msg = "El usuario " + Constante.usuario_actual + " no tiene permiso de modificar el procedimiento: " + proc.Nombre;
                                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                            }
                        }
                        else
                        {
                            string msg = "El procedimiento: " + nombre + " no existe en la base de datos: " + Constante.db_actual;
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
                string msg = "Se debe usar la instruccion 'usar' antes de eliminar un procedimiento";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
            }
            master.generar_xml();
            return estado_aceptacion;
        }

        public static bool dropUsuario(string nombre, int line, int colm)
        {
            //como es usuario esta a nivel de la base de datos no necesito el usar
            //necesito verificar si el usuario existe
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            if (master.Usuarios.ContainsKey(nombre))
            {
                //verifico si el usuario que quiero eliminar no sea el administrador
                if (Constante.usuario_actual.Equals(Constante.usuario_admin))
                {
                    //si el lusuario actual es el administrador entonces puede eliminar usuarios

                    //si el usuario a eliminar es el administrador reportar como error
                    if (!nombre.Equals(Constante.usuario_admin))
                    {
                        master.Usuarios.Remove(nombre);
                        estado_aceptacion = true;

                        String fechahora = Convert.ToString(DateTime.Now);
                        Constante.rtb_consola.Text += ">> " + fechahora + " " + Constante.usuario_actual + " [Instruccion Drop][Elimina usuario: " + nombre + "]\n";

                        //como lo elimine debo eliminar los usuarios de este nombre en todos los objetos
                        //de la base de datos
                        foreach (Db db in master.Dbs.Values)
                        {
                            db.Usuarios.Remove(nombre);

                            foreach(Procedimiento proc in db.Procedimientos.Values)
                            {
                                proc.Usuarios.Remove(nombre);
                            }
                            foreach(Funcion func in db.Funciones.Values)
                            {
                                func.Usuarios.Remove(nombre);
                            }
                            foreach(Objeto obj in db.Objetos.Values)
                            {
                                obj.Usuarios.Remove(nombre);
                            }
                            foreach(Tabla t in db.Tablas.Values)
                            {
                                t.Usuarios.Remove(nombre);
                            }
                        }
                    }
                    else
                    {
                        string msg = "No se puede eliminar al usuario administrador";
                        uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                    }
                }
                else
                {
                    string msg = "El usuario: " + Constante.usuario_actual + " no puede eliminar a otro usuario" +
                        " Solo el administrador puede eliminar usuarios";
                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                }
            }
            else
            {
                string msg = "El usuario: " + nombre + " no existe en el DBMS";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
            }

            master.generar_xml();
            return estado_aceptacion;
        }
        #endregion
    }
}
