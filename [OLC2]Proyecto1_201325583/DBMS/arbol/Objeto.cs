using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.arbol
{
    public class Objeto : xInstruccion
    {
        /**
         * Esta clase realiza el almacenamiento a memoria de un objeto
         * @nombre      identificador del objeto
         * @atributos   lista de atributos de un objeto
         */

        protected string nombre;
        protected string ruta;
        protected List<Atributo> atributos;

        public Objeto(string nombre)
        {
            this.nombre = nombre;
            this.atributos = new List<Atributo>();
        }

        protected string generar_atributos()
        {
            string xml_generado = "";
            for (int i = 0; i < atributos.Count; i++)
            {
                xml_generado += "\t\t" + atributos[i].generando_xml();
            }
            return xml_generado;
        }

        private string generar_attr()
        {
            string xml_generado = "";
            if (atributos.Count > 0) { xml_generado += "\t<attr>\n"; }
            xml_generado += generar_atributos();
            if (atributos.Count > 0) { xml_generado += "\t</attr>\n"; }
            return xml_generado;
        }

        public virtual string generando_xml()
        {
            string xml_generado = "<obj>\n" +
                "\t<nombre>" + nombre + "</nombre>\n";
            xml_generado += generar_attr();
            xml_generado += "</obj>\n";
            return xml_generado;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public List<Atributo> Atributos { get => atributos; set => atributos = value; }
        public string Ruta { get => ruta; set => ruta = value; }
    }
}
