using ServidorDB.otros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.xml
{
    public class Maestro : xInstruccion
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
        private string ruta;

        public Dictionary<string, Db> Dbs { get => dbs; set => dbs = value; }
        public Dictionary<string, Usuario> Usuarios { get => usuarios; set => usuarios = value; }
        public string Ruta { get => ruta; set => ruta = value; }

        public Maestro()
        {
            this.ruta = Constante.RUTA_MAESTRO + "master." + Constante.EXTENSION;
            this.dbs = new Dictionary<string, Db>();
            this.usuarios = new Dictionary<string, Usuario>();
        }

        public string generar_xml()
        {
            string cadena = "";
            dbs.OrderBy(key => key.Key);
            foreach (Db db in dbs.Values)
            {
                db.generar_xml();
                cadena += "<db>\n" +
                    "\t<nombre>" + db.Nombre + "</nombre>\n" +
                    "\t<path>~" + db.Ruta + "~</path>\n" +
                    "</db>\n";
            }
            usuarios.OrderBy(key => key.Key);
            foreach(Usuario usr in usuarios.Values)
            {
                cadena += "<usuario>\n" +
                    "\t<nombre>" + usr.Nombre + "</nombre>\n" +
                    "\t<password>" + usr.Password + "</password>\n" +
                    "</usuario>\n";
            }
            Constante.crear_archivo(ruta, cadena);
            return null;
        }

        #region LOGICA
        public void crear_base_datos(string nombre, Db ndb)
        {
            dbs.Add(nombre, ndb);
        }

        public void crear_usuario(string nuser, Usuario user)
        {
            usuarios.Add(nuser, user);
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


        #endregion

        public object cargar()
        {
            if (Constante.existe_archivo(ruta))
            {
                string cadena = Constante.leer_archivo(ruta);
                List<object> lob = xSintactico.analizarMaestro(cadena);
                //verificar si se genero el arbol
                if(lob != null)
                {
                    //recorrer los objetos
                    for (int i = 0; i < lob.Count; i++)
                    {
                        if (lob[i] is Usuario)
                        {
                            Usuario usr = (Usuario)lob[i];
                            usuarios.Add(usr.Nombre, usr);
                        }
                        else
                        {
                            Db ddb = (Db)lob[i];
                            //pero necesito ejecutar la base de datos para que recupere su informacion
                            ddb.cargar();
                            dbs.Add(ddb.Nombre, ddb);
                        }
                    }
                    return this;
                }
            }
            return new Maestro();
        }
    }
}
