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
                            //el administrador tiene todos los permisos
                            t.Usuarios.Add(Constante.usuario_admin);
                            //el if es por cuestiones de pruebas
                            //se puede quitar el if con equals ("")
                            if (!Constante.usuario_actual.Equals("") &&
                                !Constante.usuario_actual.Equals(Constante.usuario_admin))
                            {
                                t.Usuarios.Add(Constante.usuario_actual);
                            }
                            
                            master.Dbs[Constante.db_actual].Tablas.Add(t.Nombre, t);
                            Constante.db_actual = "";
                            Constante.usuando_db_actual = false;
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
            master.generar_xml();
            return estado_aceptacion;
        }
    }
}
