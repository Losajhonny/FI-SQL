using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.arbol
{
    public class Funcion : Objeto
    {
        /**
         * @tipo    Representa el tipo de retorno de la funcion
         * @nombre  Nombre de la funcion
         */
        protected int tipo;

        public Funcion(int tipo, string nombre) : base(nombre)
        {
            this.tipo = tipo;
        }

        public int Tipo { get => tipo; set => tipo = value; }

        protected string generar_params()
        {
            string xml_generado = "";
            if (atributos.Count > 0) { xml_generado += "\t<params>\n"; }
            xml_generado += generar_atributos();
            if (atributos.Count > 0) { xml_generado += "\t</params>\n"; }
            return xml_generado;
        }

        public override string generando_xml()
        {
            string xml_generado = "<fun>\n" +
                "\t<nombre>" + nombre + "</nombre>\n";
            xml_generado += generar_params();
            //src pendiente
            //tipo de retorno
            xml_generado += "</fun>\n";
            return xml_generado;
        }
    }
}
