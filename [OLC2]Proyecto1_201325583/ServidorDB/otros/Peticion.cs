using ServidorDB.analizadores.usql;
using ServidorDB.arboles.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.otros
{
    class Peticion
    {
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
                master.crear_usuario(usr.Nombre, usr);
                estado_aceptacion = true;
            }
            //finalizando volver a escribir la estructura
            master.generar_xml();
            return estado_aceptacion;
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
                }
                else
                {
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

                            master.Dbs[Constante.db_actual].Objetos.Add(obj.Nombre, obj);
                            Constante.db_actual = "";
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
                            estado_aceptacion = true;
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
                string msg = "Se debe usar la instruccion 'usar' antes de quitar un Objeto";
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
                            for(int i = 0; i < atrs.Count; i++)
                            {
                                obj.Parametros.Add(atrs[i]);
                            }
                            estado_aceptacion = true;
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
                string msg = "Se debe usar la instruccion 'usar' antes de crear un Objeto";
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
    }
}
