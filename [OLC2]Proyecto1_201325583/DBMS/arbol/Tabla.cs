using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.arbol
{
    public class Tabla : xInstruccion
    {
        private string nombre;
        private string ruta;
        private List<Atributo> atributos;
        private List<Registro> registros;

        public Tabla(string nombre, List<Atributo> atributos)
        {
            this.nombre = nombre;
            this.atributos = atributos;
            this.registros = new List<Registro>();
        }

        public Tabla(string nombre, List<Atributo> atributos, List<Registro> registros)
        {
            this.nombre = nombre;
            this.atributos = atributos;
            this.registros = registros;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public List<Atributo> Atributos { get => atributos; set => atributos = value; }
        public string Ruta { get => ruta; set => ruta = value; }
        public List<Registro> Registros { get => registros; set => registros = value; }

        /**
         * Va a generar un archivo de registros
         * Y va a retornar la estructura de una tabla
         */
        public string generando_xml()
        {
            //debo generar los registros de la tabla
            string xml_generado = generar_registros();
            //ahora necesito generar el archivo xml de los registros
            string ruta = Constante_dbms.RUTA_TABLA + nombre + "." + Constante_dbms.EXTENSION;
            Constante_dbms.crear_archivo(ruta, xml_generado);
            //generar el xml de la tabla
            xml_generado = "<tabla>\n" +
                "\t<nombre>" + nombre + "</nombre>\n" +
                "\t<path>" + ruta + "</path>\n" +
                "\t<rows>\n" +
                generando_atributos() +
                "\t</rows>\n" +
                "</tabla>\n";
            return xml_generado;
        }

        private string generando_atributos()
        {
            string xml_generado = "";
            for (int i = 0; i < atributos.Count; i++)
            {
                xml_generado += "\t\t" + atributos[i].generando_xml();
            }
            return xml_generado;
        }

        private string generar_registros()
        {
            string xml_generado = "";
            for (int i = 0; i < registros.Count; i++)
            {
                xml_generado += registros[i].generando_xml();
            }
            return xml_generado;
        }
    }
}
