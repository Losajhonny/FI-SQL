using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.arbol
{
    public class Db : xInstruccion
    {
        /**
         * En esta clase manejara la creacion de los objetos usql como lo que son
         * tablas, procedimientos, funciones, objetos
         * @tablas      almacenara las tablas
         * @metodo      almacenara los procedimientos y funciones
         * @objetos     almacenara los objetos
         */
        private string nombre;
        private string ruta;

        private Dictionary<string, Tabla> tablas;
        private Dictionary<string, Funcion> funciones;
        private Dictionary<string, Procedimiento> procedimientos;
        private Dictionary<string, Objeto> objetos;

        public Db(string nombre, string ruta)
        {
            this.nombre = nombre;
            this.ruta = ruta;
            this.tablas = new Dictionary<string, Tabla>();
            this.funciones = new Dictionary<string, Funcion>();
            this.procedimientos = new Dictionary<string, Procedimiento>();
            this.objetos = new Dictionary<string, Objeto>();
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public string Ruta { get => ruta; set => ruta = value; }
        public Dictionary<string, Tabla> Tablas { get => tablas; set => tablas = value; }
        public Dictionary<string, Funcion> Funciones { get => funciones; set => funciones = value; }
        public Dictionary<string, Procedimiento> Procedimientos { get => procedimientos; set => procedimientos = value; }
        public Dictionary<string, Objeto> Objetos { get => objetos; set => objetos = value; }

        public string generando_objetos()
        {
            string ruta_objeto = "";
            //trabajando con los objetos
            string xml_objetos = "";
            //variable de concatenacion de xml
            objetos.OrderBy(key => key.Key);
            foreach (Objeto obj in objetos.Values)
            {
                xml_objetos += obj.generando_xml();
                //generar xml por cada objeto
            }
            //almacenar el archivo con el nombre de la base de datos
            ruta_objeto = Constante_dbms.RUTA_OBJETO + nombre + "." + Constante_dbms.EXTENSION;
            Constante_dbms.crear_archivo(ruta_objeto, xml_objetos);
            return ruta_objeto;
        }
        
        public string generando_xml()
        {
            //aqui debo generar el xml de tablas, funciones, procedimientos, objetos
            string xml_db = "";
            /*=================================== objetos ==============================================*/
            string ruta_objeto = generando_objetos();
            //codigo generado de un objeto
            xml_db = "<object>\n" +
                "\t<path>" + ruta_objeto + "</path>\n" +
                "</object>\n";

            /*=================================== tablas ==============================================*/
            /*Las tablas ya me generan su codigo xml para agregarlas*/
            tablas.OrderBy(key => key.Key);
            foreach (Tabla tab in tablas.Values)
            {
                xml_db += tab.generando_xml();
                //generar xml por cada objeto
            }

            return xml_db;
        }
    }
}
