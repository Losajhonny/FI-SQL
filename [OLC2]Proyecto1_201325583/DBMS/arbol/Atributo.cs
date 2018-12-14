using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.arbol
{
    public class Atributo : xInstruccion
    {
        /**
         * Almacena un atributo de un objeto, procedimiento, funcion o de una tabla
         * @variable    sera el nombre de una variable
         * @tipo        sera el tipo de dato de la variable
         */

        private string variable;
        private string tipo;

        public Atributo(string variable, string tipo)
        {
            this.variable = variable;
            this.tipo = tipo;
        }

        public string Variable { get => variable; set => variable = value; }
        public string Tipo { get => tipo; set => tipo = value; }

        public string generando_xml()
        {
            return "<" + tipo + ">" + variable + "</" + tipo + ">\n";
        }
    }
}
