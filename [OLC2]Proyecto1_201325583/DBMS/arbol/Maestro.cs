using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.arbol
{
    public class Maestro
    {
        /**
         * Esta clase representa la raiz del dbms es donde manejara
         * la creacion de la base de datos y usuarios
         * 
         * La validacion de que si existe un OBJETO USQL lo realiza
         * el modulo de ServidorDB en la tabla de simbolos
         * aqui solo realizo la funcion de insertar o modificar los
         * OBJETOS USQL
         * 
         * Lo eficiente de utilizar Dictionary es de poder
         * obtener un objeto con un key
         * 
         * @dbs         Almacena todos los dbs que se han creado
         * @users       Guarda toda la informacion de los usuarios
         */
        private Dictionary<string, Db> dbs;
        private Dictionary<string, Usuario> usuarios;

        public Dictionary<string, Db> Dbs { get => dbs; set => dbs = value; }
        public Dictionary<string, Usuario> Usuarios { get => usuarios; set => usuarios = value; }

        public Maestro()
        {
            dbs = new Dictionary<string, Db>();
            usuarios = new Dictionary<string, Usuario>();
        }

        /**
         * Funciones para manejo de dbms
         */

        /**
         * Funcion para crear una base de datos
         */
        public void crear_base_datos(string nombre, string ruta)
        {
            Db db = new Db(nombre, ruta);
            //creo una base de datos con key = nombre de la base de datos
            dbs.Add(nombre, db);
        }

        public void crear_usuario(string nuser, string pass, string ruta, List<string> permisos)
        {
            
        }

        public bool crear_tabla(string ndb, string ntabla, Tabla tabla)
        {
            if (dbs.ContainsKey(ndb))
            {
                Db db = dbs[ndb];
                //Obtengo la base de datos
                db.Tablas.Add(ntabla, tabla);
                //agrego la tabla a la base de datos
                return true;
            }
            return false;
        }

        public bool crear_procedimiento(string ndb, string nproc, Procedimiento proc)
        {
            if (dbs.ContainsKey(ndb))
            {
                Db db = dbs[ndb];
                //obtener base de datos
                db.Procedimientos.Add(nproc, proc);
                //agregando el procedimiento
                return true;
            }
            return false;
        }

        public bool crear_funcion(string ndb, string nfuncion, Funcion fun)
        {
            if (dbs.ContainsKey(ndb))
            {
                Db db = dbs[ndb];
                //obtener base de datos
                db.Funciones.Add(nfuncion, fun);
                //agregando una funcion
                return true;
            }
            return false;
        }

        public bool crear_objeto(string ndb, string nobj, Objeto obj)
        {
            if (dbs.ContainsKey(ndb))
            {
                Db db = dbs[ndb];
                db.Objetos.Add(nobj, obj);
                return true;
            }
            return false;
        }

        public void escritura_xml()
        {
            //no sirve lo que voy hacer pero si de prueba
            string val = "";
            foreach(Db db in dbs.Values)
            {
                val = db.generando_xml();
            }
        }
    }
}
