using ServidorDB.analizadores.usql;
using ServidorDB.arboles.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.otros
{
    class PeticionDCL
    {
        public static bool otorgarUsuario(string usuario, string db, string objeto, int line, int colm)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            //solo el administrador puede dar permisos por lo que hay que validar
            if (Constante.usuario_actual.Equals(Constante.usuario_admin))
            {
                //entonces solo el admin ejecutara permisos
                //debo analizar la base de datos
                if (master.Dbs.ContainsKey(db))
                {
                    Db basedatos = master.Dbs[db];
                    //tambien le doy permisos a la base de datos o si no no podra acceder a sus objetos
                    //ahora verificar si el usuario ya tiene permiso para no agregarlo otra vez
                    bool ban = false; //para verificar si existe usuario agregado a un objeto
                    for (int i = 0; i < basedatos.Usuarios.Count; i++)
                    {
                        if (basedatos.Usuarios[i].Equals(usuario)) { ban = true; break; }
                    }
                    if (!ban) { basedatos.Usuarios.Add(usuario); }

                    String fechahora = Convert.ToString(DateTime.Now);
                    Constante.mensaje += ">> Se otorgo permiso un usuario : " + usuario + " al objeto : " + objeto + "\n";
                    Constante.rtb_consola.Text += ">> " + fechahora + " " + Constante.usuario_actual + " [Instruccion Otorgar][Permiso de usuario:"+usuario+" al objeto:" + objeto + "]\n";

                    if (objeto != null)
                    {
                        bool ban2 = false; //para verificar si existe el objeto
                        /*Como no se que objeto es ya sea procedimiento, funcion, objeto, etc
                        entonces voy a buscar todos los objetos con el mismo nombre para otorgar
                        permisos por nombre*/
                        foreach (Tabla t in basedatos.Tablas.Values)
                        {
                            if (t.Nombre.Equals(objeto))
                            {
                                ban = false;
                                for (int i = 0; i < t.Usuarios.Count; i++)
                                {
                                    if (t.Usuarios[i].Equals(usuario)) { ban = true; break; }
                                }
                                if (!ban) { t.Usuarios.Add(usuario); }
                                ban2 = true;
                            }
                        }
                        foreach (Objeto t in basedatos.Objetos.Values)
                        {
                            if (t.Nombre.Equals(objeto))
                            {
                                ban = false;
                                for (int i = 0; i < t.Usuarios.Count; i++)
                                {
                                    if (t.Usuarios[i].Equals(usuario)) { ban = true; break; }
                                }
                                if (!ban) { t.Usuarios.Add(usuario); }
                                ban2 = true;
                            }
                        }
                        foreach (Procedimiento t in basedatos.Procedimientos.Values)
                        {
                            if (t.Nombre.Equals(objeto))
                            {
                                ban = false;
                                for (int i = 0; i < t.Usuarios.Count; i++)
                                {
                                    if (t.Usuarios[i].Equals(usuario)) { ban = true; break; }
                                }
                                if (!ban) { t.Usuarios.Add(usuario); }
                                ban2 = true;
                            }
                        }
                        foreach (Funcion t in basedatos.Funciones.Values)
                        {
                            if (t.Nombre.Equals(objeto))
                            {
                                ban = false;
                                for (int i = 0; i < t.Usuarios.Count; i++)
                                {
                                    if (t.Usuarios[i].Equals(usuario)) { ban = true; break; }
                                }
                                if (!ban) { t.Usuarios.Add(usuario); }
                                ban2 = true;
                            }
                        }

                        if (!ban2)
                        {
                            //entonces el objeto no existe en ninguno de los ObjetosUsql
                            string msg = "El objeto: " + objeto + " no existe en el la base de datos: " + db;
                            uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                        }
                    }
                    else
                    {
                        /*Como el objeto es null quiere decir que es * por lo que se dara permisos
                         a todos los objetos de la base de datos*/
                        foreach (Tabla t in basedatos.Tablas.Values)
                        {
                            ban = false;
                            for (int i = 0; i < t.Usuarios.Count; i++)
                            {
                                if (t.Usuarios[i].Equals(usuario)) { ban = true; break; }
                            }
                            if (!ban) { t.Usuarios.Add(usuario); }
                        }
                        foreach (Objeto t in basedatos.Objetos.Values)
                        {
                            ban = false;
                            for (int i = 0; i < t.Usuarios.Count; i++)
                            {
                                if (t.Usuarios[i].Equals(usuario)) { ban = true; break; }
                            }
                            if (!ban) { t.Usuarios.Add(usuario); }
                        }
                        foreach (Procedimiento t in basedatos.Procedimientos.Values)
                        {
                            ban = false;
                            for (int i = 0; i < t.Usuarios.Count; i++)
                            {
                                if (t.Usuarios[i].Equals(usuario)) { ban = true; break; }
                            }
                            if (!ban) { t.Usuarios.Add(usuario); }
                        }
                        foreach (Funcion t in basedatos.Funciones.Values)
                        {
                            ban = false;
                            for (int i = 0; i < t.Usuarios.Count; i++)
                            {
                                if (t.Usuarios[i].Equals(usuario)) { ban = true; break; }
                            }
                            if (!ban) { t.Usuarios.Add(usuario); }
                        }
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
                //error
                string msg = "Solo el administrador puede otorgar permisos";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
            } 

            master.generar_xml();
            return estado_aceptacion;
        }

        public static bool denegarUsuario(string usuario, string db, string objeto, int line, int colm)
        {
            bool estado_aceptacion = false;
            Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();
            Maestro master = Constante.sistema_archivo;

            //solo el administrador puede dar permisos por lo que hay que validar
            if (Constante.usuario_actual.Equals(Constante.usuario_admin))
            {
                //entonces solo el admin ejecutara permisos
                //debo analizar la base de datos
                if (master.Dbs.ContainsKey(db))
                {
                    Db basedatos = master.Dbs[db];
                    //tambien le doy permisos a la base de datos o si no no podra acceder a sus objetos
                    //ahora verificar si el usuario ya tiene permiso para no agregarlo otra vez
                    bool ban = false; //para verificar si existe usuario agregado a un objeto
                    for (int i = 0; i < basedatos.Usuarios.Count; i++)
                    {
                        if (basedatos.Usuarios[i].Equals(usuario)) { ban = true; break; }
                    }
                    if (!ban) { basedatos.Usuarios.Remove(usuario); }
                    String fechahora = Convert.ToString(DateTime.Now);
                    Constante.mensaje += ">> Se denego un usuario : " + usuario + " al objeto : " + objeto + "\n";
                    Constante.rtb_consola.Text += ">> " + fechahora + " " + Constante.usuario_actual + " [Instruccion Denegar][Denegar de usuario:" + usuario + " al objeto:" + objeto + "]\n";
                    if (objeto != null)
                    {
                        bool ban2 = false; //para verificar si existe el objeto
                        /*Como no se que objeto es ya sea procedimiento, funcion, objeto, etc
                        entonces voy a buscar todos los objetos con el mismo nombre para otorgar
                        permisos por nombre*/
                        foreach (Tabla t in basedatos.Tablas.Values)
                        {
                            if (t.Nombre.Equals(objeto))
                            {
                                ban = false;
                                for (int i = 0; i < t.Usuarios.Count; i++)
                                {
                                    if (t.Usuarios[i].Equals(usuario)) { ban = true; break; }
                                }
                                if (!ban) { t.Usuarios.Remove(usuario); }
                                ban2 = true;
                            }
                        }
                        foreach (Objeto t in basedatos.Objetos.Values)
                        {
                            if (t.Nombre.Equals(objeto))
                            {
                                ban = false;
                                for (int i = 0; i < t.Usuarios.Count; i++)
                                {
                                    if (t.Usuarios[i].Equals(usuario)) { ban = true; break; }
                                }
                                if (!ban) { t.Usuarios.Remove(usuario); }
                                ban2 = true;
                            }
                        }
                        foreach (Procedimiento t in basedatos.Procedimientos.Values)
                        {
                            if (t.Nombre.Equals(objeto))
                            {
                                ban = false;
                                for (int i = 0; i < t.Usuarios.Count; i++)
                                {
                                    if (t.Usuarios[i].Equals(usuario)) { ban = true; break; }
                                }
                                if (!ban) { t.Usuarios.Remove(usuario); }
                                ban2 = true;
                            }
                        }
                        foreach (Funcion t in basedatos.Funciones.Values)
                        {
                            if (t.Nombre.Equals(objeto))
                            {
                                ban = false;
                                for (int i = 0; i < t.Usuarios.Count; i++)
                                {
                                    if (t.Usuarios[i].Equals(usuario)) { ban = true; break; }
                                }
                                if (!ban) { t.Usuarios.Remove(usuario); }
                                ban2 = true;
                            }
                        }

                        if (!ban2)
                        {
                            //entonces el objeto no existe en ninguno de los ObjetosUsql
                            string msg = "El objeto: " + objeto + " no existe en el la base de datos: " + db;
                            uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                        }
                    }
                    else
                    {
                        /*Como el objeto es null quiere decir que es * por lo que se dara permisos
                         a todos los objetos de la base de datos*/
                        foreach (Tabla t in basedatos.Tablas.Values)
                        {
                            ban = false;
                            for (int i = 0; i < t.Usuarios.Count; i++)
                            {
                                if (t.Usuarios[i].Equals(usuario)) { ban = true; break; }
                            }
                            if (!ban) { t.Usuarios.Remove(usuario); }
                        }
                        foreach (Objeto t in basedatos.Objetos.Values)
                        {
                            ban = false;
                            for (int i = 0; i < t.Usuarios.Count; i++)
                            {
                                if (t.Usuarios[i].Equals(usuario)) { ban = true; break; }
                            }
                            if (!ban) { t.Usuarios.Remove(usuario); }
                        }
                        foreach (Procedimiento t in basedatos.Procedimientos.Values)
                        {
                            ban = false;
                            for (int i = 0; i < t.Usuarios.Count; i++)
                            {
                                if (t.Usuarios[i].Equals(usuario)) { ban = true; break; }
                            }
                            if (!ban) { t.Usuarios.Remove(usuario); }
                        }
                        foreach (Funcion t in basedatos.Funciones.Values)
                        {
                            ban = false;
                            for (int i = 0; i < t.Usuarios.Count; i++)
                            {
                                if (t.Usuarios[i].Equals(usuario)) { ban = true; break; }
                            }
                            if (!ban) { t.Usuarios.Remove(usuario); }
                        }
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
                //error
                string msg = "Solo el administrador puede denegar permisos";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
            }

            master.generar_xml();
            return estado_aceptacion;
        }
    }
}
