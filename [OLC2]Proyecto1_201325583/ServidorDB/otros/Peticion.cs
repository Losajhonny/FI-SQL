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
    }
}
