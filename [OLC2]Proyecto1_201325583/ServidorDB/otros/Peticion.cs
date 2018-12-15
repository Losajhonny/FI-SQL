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

            //necesito saber si esta usando una base de datos
            if (Constante.usuando_db_actual)
            {
                //como se declaro usar db entonces seguir con el proceso
                //necesito buscar la base de datos
                //if (master.Dbs.ContainsKey(Constante.db_actual))
                //{
                //    //como si existe la base de datos entonces
                //    //colocarlo como la base actual
                //    //pero antes verificar si el usuario tiene permiso
                //    string usr = null;
                //    foreach (string user in master.Dbs[nombre].Usuarios)
                //    {
                //        if (user.Equals(Constante.usuario_actual)) { usr = user; break; }
                //    }

                //    if (usr != null)
                //    {   //como el usuario existe en el base  de datos entonces cargar el db
                //        Constante.usuando_db_actual = true;
                //        Constante.db_actual = nombre;
                //    }
                //    else
                //    {
                //        string msg = "El usuario " + Constante.usuario_actual + " no tiene permisos en la base de datos " + nombre;
                //        uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                //    }

                //    estado_aceptacion = true;
                //}
            }
            else
            {
                //error por no haber utilizado la instruccion usar
                string msg = "Se debe usar la instruccion 'usar' antes de crear una Tabla";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", t.Line, t.Colm));
            }
            return estado_aceptacion;
        }
    }
}
