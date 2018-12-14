using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.arbol
{
    public class Registro : xInstruccion
    {
        /**
         * Utilizando la clase atributo para almacenar un registro
         * variables de Atributo
         * 
         * atributo.@tipo       =   nombre del campo
         * atributo.@variable   =   valor de un campo
         */
        private List<Atributo> atributos;

        public Registro(List<Atributo> atributos)
        {
            this.atributos = atributos;
        }

        public List<Atributo> Atributos { get => atributos; set => atributos = value; }

        public string generar_atributos()
        {
            string xml_generado = "";
            for (int i = 0; i < atributos.Count; i++)
            {
                xml_generado += "\t" + atributos[i].generando_xml();
            }
            return xml_generado;
        }

        public string generando_xml()
        {
            string xml_generado = "";
            if (atributos.Count > 0) { xml_generado += "<row>\n"; }
            xml_generado += generar_atributos();
            if (atributos.Count > 0) { xml_generado += "</row>\n"; }

            return xml_generado;
        }
    }
}
