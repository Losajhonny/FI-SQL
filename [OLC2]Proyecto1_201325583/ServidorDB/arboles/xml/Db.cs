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
        private List<string> usuarios;

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
                                for (int j = 0; j < Constante.TIPOS.Length; j++)
                                {
                                    string t1 = t.Atributos[y].Ti1;
                                    string t2 = t.Atributos[y].Ti2;
                                    if (Constante.TIPOS[j].Equals(t1) &&
                                        Constante.TIPOS[j].Equals(t2))
                                    {
                                        t.Atributos[y].Tipo = j;
                                        break;
                                    }
                                    //solo para diferenciar
                                    t.Atributos[y].Tipo = Constante.VOID;
                                }
                                if (t.Atributos[y].Tipo == Constante.VOID)
                                {
                                    string msg = "El tipo de dato debe ser primitivo";
                                    xSintactico.errores.Add(new analizadores.usql.uError(Constante.SEMANTICO, msg, "", t.Atributos[y].Line, t.Atributos[y].Colm));
                                }
                            }
                            //verificar los tipos encaso de no ser primitivos su valor sera el -2;
                            t.cargar();
                            //agregar la tabla a la base de datos
                            tablas.Add(t.Nombre, t);
                        }
                        else if (lob[i] is string[])
                        {
                            //aqui me viene el path de los procedimiento
                            //funciones, objetos
                            //aqui valido si es un
                            /*=============================
                                    aqui voy        
                            ===============================*/
                            string[] proceso = (string[])lob[i];
                            string cad = "";

                            if (proceso[0].Equals("proc"))
                            {
                                //como es procedimiento entonces
                                //leer el archivo
                                cad = Constante.leer_archivo(proceso[1]);
                                List<Procedimiento> procs = xSintactico.analizarProc(cad);
                                //ya tengo la lista entonces ir al los atributos
                                //para controlar el tipo de dato
                                for(int j = 0; j < procs.Count; j++)
                                {
                                    bool hay_error = false;
                                    //estoy recorriendo los procedimientos
                                    for(int k = 0; k < procs[j].Parametros.Count; k++)
                                    {
                                        //aqui estoy en los atributos de un procedimiento
                                        //aqui solo tiene atributos primitivos
                                        string t1 = procs[j].Parametros[k].Ti1;
                                        string t2 = procs[j].Parametros[k].Ti2;
                                        //ahora verificar si son iguales
                                        if (t1.Equals(t2))
                                        {
                                            //ahora verificar si son primitivos
                                            for(int y = 0; y < Constante.TIPOS.Length; y++)
                                            {
                                                if (Constante.TIPOS[y].Equals(t1))
                                                {
                                                    procs[j].Parametros[k].Tipo = y;
                                                    this.procedimientos.Add(procs[j].Nombre ,procs[j]);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (proceso[0].Equals("func"))
                            {

                            }
                            else
                            {   //object

                            }
                            
                        }
                    }
                }
            }
            return null;
        }
    }
}
