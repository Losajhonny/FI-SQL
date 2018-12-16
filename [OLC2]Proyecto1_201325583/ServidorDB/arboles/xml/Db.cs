using ServidorDB.otros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.xml
{
    public class Db : xInstruccion
    {
        /**
         * En esta clase manejara la creacion de los objetos usql como lo que son
         * tablas, procedimientos, funciones, objetos
         * @usuarios    esta almacenara una lista de usuarios quienes tiene permiso
         *              para manejar la base de datos
         * @ruta        Representa la ruta del archivo de esta clase
         */
        private string nombre;
        private string ruta;
        private int line;
        private int colm;

        private Dictionary<string, Tabla> tablas;
        private Dictionary<string, Funcion> funciones;
        private Dictionary<string, Procedimiento> procedimientos;
        private Dictionary<string, Objeto> objetos;
        private List<string> usuarios = new List<string>();

        private string ruta_proc;
        private string ruta_func;
        private string ruta_object;
        private string ruta_tabla;

        public Db(string nombre)
        {
            this.nombre = nombre;
            this.ruta = Constante.RUTA_DB + nombre + "." + Constante.EXTENSION;
            this.tablas = new Dictionary<string, Tabla>();
            this.funciones = new Dictionary<string, Funcion>();
            this.procedimientos = new Dictionary<string, Procedimiento>();
            this.objetos = new Dictionary<string, Objeto>();
            this.usuarios = new List<string>();
        }

        public Db(string nombre, string path)
        {
            this.nombre = nombre;
            this.ruta = path;
            this.tablas = new Dictionary<string, Tabla>();
            this.funciones = new Dictionary<string, Funcion>();
            this.procedimientos = new Dictionary<string, Procedimiento>();
            this.objetos = new Dictionary<string, Objeto>();
            this.usuarios = new List<string>();
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public string Ruta { get => ruta; set => ruta = value; }
        public Dictionary<string, Tabla> Tablas { get => tablas; set => tablas = value; }
        public Dictionary<string, Funcion> Funciones { get => funciones; set => funciones = value; }
        public Dictionary<string, Procedimiento> Procedimientos { get => procedimientos; set => procedimientos = value; }
        public Dictionary<string, Objeto> Objetos { get => objetos; set => objetos = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }
        public List<string> Usuarios { get => usuarios; set => usuarios = value; }
        public string Ruta_proc { get => ruta_proc; set => ruta_proc = value; }
        public string Ruta_func { get => ruta_func; set => ruta_func = value; }
        public string Ruta_object { get => ruta_object; set => ruta_object = value; }
        public string Ruta_tabla { get => ruta_tabla; set => ruta_tabla = value; }

        public string generar_procedure()
        {
            procedimientos.OrderBy(key => key.Key);
            string cadena = "";
            foreach(Procedimiento proc in procedimientos.Values)
            {
                cadena += "<proc>\n";
                cadena += "\t<nombre>" + proc.Nombre + "</nombre>\n";
                cadena += "\t<params>\n";
                foreach(Atributo attr in proc.Parametros)
                {
                    cadena += "\t\t<" + Constante.TIPOS[attr.Tipo] + ">" + attr.Nombre + "</" + Constante.TIPOS[attr.Tipo] + ">\n";
                }
                cadena += "\t</params>\n";
                cadena += "\t<usuarios>\n";
                foreach(string usuario in proc.Usuarios)
                {
                    cadena += "\t\t<usuario>" + usuario + "</usuario>\n";
                }
                cadena += "\t</usuarios>\n";
                cadena += "\t<src>~" + proc.Src + "~</src>\n";
                cadena += "</proc>\n";
            }
            string path = Constante.RUTA_PROCEDIMIENTOS + nombre + "." + Constante.EXTENSION;
            Constante.crear_archivo(path, cadena);
            return path;
        }

        public string generar_function()
        {
            funciones.OrderBy(key => key.Key);
            string cadena = "";
            foreach (Funcion fun in funciones.Values)
            {
                cadena += "<fun>\n";
                cadena += "\t<nombre>" + fun.Nombre + "</nombre>\n";
                cadena += "\t<params>\n";
                foreach (Atributo attr in fun.Parametros)
                {
                    cadena += "\t\t<" + Constante.TIPOS[attr.Tipo] + ">" + attr.Nombre + "</" + Constante.TIPOS[attr.Tipo] + ">\n";
                }
                cadena += "\t</params>\n";
                cadena += "\t<usuarios>\n";
                foreach (string usuario in fun.Usuarios)
                {
                    cadena += "\t\t<usuario>" + usuario + "</usuario>\n";
                }
                cadena += "\t</usuarios>\n";
                cadena += "\t<retorno>" + Constante.TIPOS[fun.Tipo] + "</retorno>\n";
                cadena += "\t<src>~" + fun.Src + "~</src>\n";
                cadena += "</fun>\n";
            }
            string path = Constante.RUTA_FUNCIONES + nombre + "." + Constante.EXTENSION;
            Constante.crear_archivo(path, cadena);
            return path;
        }

        public string generar_object()
        {
            objetos.OrderBy(key => key.Key);
            string cadena = "";
            foreach (Objeto obj in objetos.Values)
            {
                cadena += "<obj>\n";
                cadena += "\t<nombre>" + obj.Nombre + "</nombre>\n";
                cadena += "\t<attr>\n";
                foreach (Atributo attr in obj.Parametros)
                {
                    cadena += "\t\t<" + Constante.TIPOS[attr.Tipo] + ">" + attr.Nombre + "</" + Constante.TIPOS[attr.Tipo] + ">\n";
                }
                cadena += "\t</attr>\n";
                cadena += "\t<usuarios>\n";
                foreach (string usuario in obj.Usuarios)
                {
                    cadena += "\t\t<usuario>" + usuario + "</usuario>\n";
                }
                cadena += "\t</usuarios>\n";
                cadena += "</obj>\n";
            }
            string path = Constante.RUTA_OBJETO + nombre + "." + Constante.EXTENSION;
            Constante.crear_archivo(path, cadena);
            return path;
        }

        public string generar_xml()
        {
            string ruta_procedure = generar_procedure();
            string ruta_function = generar_function();
            string ruta_object = generar_object();

            string cadena = "";
            cadena += "<procedure>\n" +
                "\t<path>~" + ruta_procedure + "~</path>\n" +
                "</procedure>\n";
            cadena += "<function>\n" +
                "\t<path>~" + ruta_function + "~</path>\n" +
                "</function>\n";
            cadena += "<object>\n" +
                "\t<path>~" + ruta_object + "~</path>\n" +
                "</object>\n";
            tablas.OrderBy(key => key.Key);
            foreach (Tabla t in tablas.Values)
            {
                //antes crear el xml de los registros
                t.generar_xml();
                cadena += "<tabla>\n";
                cadena += "\t<nombre>" + t.Nombre + "</nombre>\n" +
                    "\t<path>~" + t.Ruta + "~</path>\n";
                cadena += "\t<rows>\n";
                foreach (Atributo attr in t.Atributos)
                {
                    cadena += "\t\t<" + Constante.TIPOS[attr.Tipo] + ">" + attr.Nombre + "</" + Constante.TIPOS[attr.Tipo] + ">\n";
                    if(attr.Complemento != null)
                    {
                        cadena += "\t\t<complemento>" + attr.Complemento + "</complemento>\n";
                    }
                    else
                    {
                        cadena += "\t\t<foranea>" + attr.Tabla + "." + attr.Attr + "</foranea>\n";
                    }
                }
                cadena += "\t</rows>\n";
                cadena += "\t<usuarios>\n";
                foreach (string usuario in t.Usuarios)
                {
                    cadena += "\t\t<usuario>" + usuario + "</usuario>\n";
                }
                cadena += "\t</usuarios>\n";
                cadena += "</tabla>\n";
            }

            cadena += "<usuarios>\n";
            foreach (string usuario in usuarios)
            {
                cadena += "\t<usuario>" + usuario + "</usuario>\n";
            }
            cadena += "</usuarios>\n";
            Constante.crear_archivo(ruta, cadena);
            return null;
        }

        public object cargar()
        {
            if (Constante.existe_archivo(ruta))
            {
                //necesita analizar el archivo
                string cadena = Constante.leer_archivo(ruta);
                List<object> lob = xSintactico.analizarDb(cadena);
                if(lob != null)
                {   //obtener los datos de db
                    for (int i = 0; i < lob.Count; i++)
                    {
                        if (lob[i] is List<string>)
                        {
                            usuarios = (List<string>)lob[i];
                        }
                        else if (lob[i] is Tabla)
                        {
                            Tabla t = (Tabla)lob[i];
                            //cuando estoy en este punto
                            //la tabla ya tiene cargado
                            //la ruta de los registros y atributos
                            //y usuarios ademas de tener creada el datatable
                            //entoces debo ejecutar t.cargar() para cargar
                            //los registros en esta tabla
                            for (int y = 0; y < t.Atributos.Count; y++)
                            {
                                //carga las variables donde deben ser
                                //El erro que trae es que las etiquetas no son iguales
                                object retorno = t.Atributos[y].cargar();
                                if(retorno != null)
                                {
                                    xSintactico.errores.Add(new analizadores.usql.uError(Constante.SEMANTICO,
                                        xSintactico.ERRORES_XML[xSintactico.ERROR_DB] + " : " + retorno.ToString(), "", t.Atributos[y].Line,
                                        t.Atributos[y].Colm));
                                }
                                else if (t.Atributos[y].Tipo == Constante.ID)
                                {

                                    string msg = "El atributo en la tabla "+t.Nombre+" debe ser de tipo primitivo";
                                    xSintactico.errores.Add(new analizadores.usql.uError(Constante.SEMANTICO,
                                        xSintactico.ERRORES_XML[xSintactico.ERROR_DB] + " : " + msg, "", t.Atributos[y].Line,
                                        t.Atributos[y].Colm));
                                }
                            }
                            t.crearDataTable();
                            t.cargar();
                            if (!tablas.ContainsKey(t.Nombre))
                            {
                                tablas.Add(t.Nombre, t);
                            }
                            else
                            {
                                string msg = "La tabla ya existe en la base de datos: " + nombre;
                                xSintactico.errores.Add(new analizadores.usql.uError(Constante.SEMANTICO,
                                    xSintactico.ERRORES_XML[xSintactico.ERROR_DB] + " : " + msg, "", t.Line,
                                    t.Colm));
                            }
                        }
                        else if (lob[i] is string[])
                        {
                            string[] proceso = (string[])lob[i];

                            if (proceso[0].Equals("proc"))
                            {
                                proceso_proc(proceso[1]);
                            }
                            else if (proceso[0].Equals("func"))
                            {
                                proceso_func(proceso[1]);
                            }
                            else
                            {   //object
                                proceso_obj(proceso[1]);
                            }
                            
                        }
                    }
                }
            }
            return null;
        }

        public void proceso_proc(string path)
        {
            //aqui debo analizar los procedimientos
            if (Constante.existe_archivo(path))
            {
                //tengo que leer el archivo
                string cadena = Constante.leer_archivo(path);
                List<Procedimiento> procs = xSintactico.analizarProc(cadena);
                //ya cargue los procedimientos
                //bien entonces hay que recorrer los atributos para cargar
                //las variables necesarias
                if(procs != null)
                {
                    for (int i = 0; i < procs.Count; i++)
                    {
                        bool banError = false;
                        for (int j = 0; j < procs[i].Parametros.Count; j++)
                        {
                            //cargar los atributos
                            object retorno = procs[i].Parametros[j].cargar();
                            if (retorno != null)
                            {
                                xSintactico.errores.Add(new analizadores.usql.uError(Constante.SEMANTICO,
                                    xSintactico.ERRORES_XML[xSintactico.ERROR_PROC] + " : " + retorno.ToString(), "", procs[i].Parametros[j].Line,
                                    procs[i].Parametros[j].Colm));
                                banError = true;
                                break;
                            }
                            else if (procs[i].Parametros[j].Tipo == Constante.ID)
                            {
                                xSintactico.errores.Add(new analizadores.usql.uError(Constante.SEMANTICO,
                                    xSintactico.ERRORES_XML[xSintactico.ERROR_PROC] + " : Solo se aceptan tipos primitivos", "", procs[i].Parametros[j].Line,
                                    procs[i].Parametros[j].Colm));
                                banError = true;
                                break;
                            }
                        }

                        if (!banError)
                        {   //quiere decir que no se encontro errores durante la carga de variables
                            if (!procedimientos.ContainsKey(procs[i].Nombre))
                            {
                                this.procedimientos.Add(procs[i].Nombre, procs[i]);
                            }
                            else
                            {
                                xSintactico.errores.Add(new analizadores.usql.uError(Constante.SEMANTICO,
                                    xSintactico.ERRORES_XML[xSintactico.ERROR_PROC] + " : " + "El procedimiento ya existe en la base de datos: " + nombre, "", procs[i].Line,
                                    procs[i].Colm));
                            }
                            
                        }
                        //sino entonces el msg error ya fue reportado seguir con los demas procs
                    }
                }
            }


        }

        public void proceso_func(string path)
        {
            //aqui debo analizar las funciones
            if (Constante.existe_archivo(path))
            {
                //tengo que leer el archivo
                string cadena = Constante.leer_archivo(path);
                List<Funcion> funcs = xSintactico.analizaarFunc(cadena);
                //ya cargue las funciones
                //bien entonces hay que recorrer los atributos para cargar
                //las variables necesarias
                if(funcs != null)
                {
                    for (int i = 0; i < funcs.Count; i++)
                    {
                        bool banError = false;
                        for (int j = 0; j < funcs[i].Parametros.Count; j++)
                        {
                            //cargar los atributos
                            object retorno = funcs[i].Parametros[j].cargar();
                            if (retorno != null)
                            {
                                xSintactico.errores.Add(new analizadores.usql.uError(Constante.SEMANTICO,
                                    xSintactico.ERRORES_XML[xSintactico.ERROR_FUN] + " : " + retorno.ToString(), "", funcs[i].Parametros[j].Line,
                                    funcs[i].Parametros[j].Colm));
                                banError = true;
                                break;
                            }
                            else if (funcs[i].Parametros[j].Tipo == Constante.ID)
                            {
                                xSintactico.errores.Add(new analizadores.usql.uError(Constante.SEMANTICO,
                                    xSintactico.ERRORES_XML[xSintactico.ERROR_FUN] + " : Solo se aceptan tipos primitivos", "", funcs[i].Parametros[j].Line,
                                    funcs[i].Parametros[j].Colm));
                                banError = true;
                                break;
                            }
                        }

                        if (!banError)
                        {   //quiere decir que no se encontro errores durante la carga de variables
                            //verificar el tipo
                            object ret = funcs[i].cargar();
                            //validado el tipo de retorno de la funcion
                            if (ret != null)
                            {
                                if (!funciones.ContainsKey(funcs[i].Nombre))
                                {
                                    this.funciones.Add(funcs[i].Nombre, funcs[i]);
                                }
                                else
                                {
                                    xSintactico.errores.Add(new analizadores.usql.uError(Constante.SEMANTICO,
                                        xSintactico.ERRORES_XML[xSintactico.ERROR_FUN] + " : " + "La funcion ya existe en la base de datos: " + nombre, "", funcs[i].Line,
                                        funcs[i].Colm));
                                }
                            }
                            else
                            {
                                xSintactico.errores.Add(new analizadores.usql.uError(Constante.SEMANTICO,
                                       xSintactico.ERRORES_XML[xSintactico.ERROR_FUN] + " : " + "Tipo de Retorno incorrecto"
                                       + " en la funcion: " + nombre, "", 0, 0));
                            }
                        }
                        //sino entonces el msg error ya fue reportado seguir con los demas procs
                    }
                }
            }


        }

        public void proceso_obj(string path)
        {
            //aqui debo analizar los objetos
            if (Constante.existe_archivo(path))
            {
                //tengo que leer el archivo
                string cadena = Constante.leer_archivo(path);
                List<Objeto> objs = xSintactico.analizarObj(cadena);
                //ya cargue los objetos
                //bien entonces hay que recorrer los atributos para cargar
                //las variables necesarias
                if (objs != null)
                {
                    for (int i = 0; i < objs.Count; i++)
                    {
                        bool banError = false;
                        for (int j = 0; j < objs[i].Parametros.Count; j++)
                        {
                            //cargar los atributos
                            object retorno = objs[i].Parametros[j].cargar();
                            if (retorno != null)
                            {
                                xSintactico.errores.Add(new analizadores.usql.uError(Constante.SEMANTICO,
                                    xSintactico.ERRORES_XML[xSintactico.ERROR_OBJ] + " : " + retorno.ToString(), "", objs[i].Parametros[j].Line,
                                    objs[i].Parametros[j].Colm));
                                banError = true;
                                break;
                            }
                            else if (objs[i].Parametros[j].Tipo == Constante.ID)
                            {
                                xSintactico.errores.Add(new analizadores.usql.uError(Constante.SEMANTICO,
                                    xSintactico.ERRORES_XML[xSintactico.ERROR_OBJ] + " : Solo se aceptan tipos primitivos", "", objs[i].Parametros[j].Line,
                                    objs[i].Parametros[j].Colm));
                                banError = true;
                                break;
                            }
                        }

                        if (!banError)
                        {   //quiere decir que no se encontro errores durante la carga de variables
                            if (!objetos.ContainsKey(objs[i].Nombre))
                            {
                                this.objetos.Add(objs[i].Nombre, objs[i]);
                            }
                            else
                            {
                                xSintactico.errores.Add(new analizadores.usql.uError(Constante.SEMANTICO,
                                    xSintactico.ERRORES_XML[xSintactico.ERROR_OBJ] + " : " + "El objeto ya existe en la base de datos: " + nombre, "", objs[i].Line,
                                    objs[i].Colm));
                            }
                        }
                        //sino entonces el msg error ya fue reportado seguir con los demas procs
                    }
                }
            }
        }
        
    }
}
