using ServidorDB.otros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.xml
{
    public class Atributo : xInstruccion
    {
        /**
         * No me genera nada solo hace referencia a un atributo
         * de un objeto, proc, fun o tabla
         */

        /*nombre es el nombre de una variable o id*/
        /*tabla ref en caso de un producto cartesiano
         guardar el atributo con el nombre de la tabla
         que se unio*/

        /*
         *  ------      --------        ----------------
         *    t1           t2              t1    x    t2
         *  a | b       a  |  b         a  |  b  |  a  |  b
         *  ------      --------        ----------------
         *  1 | 2       3  |  4         1  |  2  |  3  |  4
         *  9 | 1       9  |  6         1  |  2  |  9  |  6
         *                              9  |  1  |  3  |  4
         *                              9  |  1  |  9  |  6
         */

        private string ti1;
        private string ti2;
        private int line;
        private int colm;

        private string tabla_ref;
        private string nombre;
        private int tipo;
        private string complemento;
        private List<string> registros;

        /*Estos atributos sirven por si el complemento es una fornaea entonces complemento va ser = null*/
        private string tabla;
        private string attr;

        public Atributo(int tipo, string nombre)
        {
            this.tipo = tipo;
            this.nombre = nombre;
            this.registros = new List<string>();
        }

        public Atributo(string t1, string t2, string nombre)
        {
            this.ti1 = t1;
            this.ti2 = t2;
            this.nombre = nombre;
            this.registros = new List<string>();
        }

        public Atributo(int tipo, string nombre, string complemento)
        {
            this.tipo = tipo;
            this.nombre = nombre;
            this.complemento = complemento;
            this.registros = new List<string>();
        }

        public Atributo(int tipo, string nombre, string tabla, string attr)
        {
            this.tipo = tipo;
            this.nombre = nombre;
            this.tabla = tabla;
            this.attr = attr;
            this.registros = new List<string>();
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public int Tipo { get => tipo; set => tipo = value; }
        public string Complemento { get => complemento; set => complemento = value; }
        public string Tabla { get => tabla; set => tabla = value; }
        public string Attr { get => attr; set => attr = value; }
        public List<string> Registros { get => registros; set => registros = value; }
        public string Tabla_ref { get => tabla_ref; set => tabla_ref = value; }
        public string Ti1 { get => ti1; set => ti1 = value; }
        public string Ti2 { get => ti2; set => ti2 = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }

        public object cargar()
        {
            //verificar si las etiqueta ti1 y ti2 son iguales
            if (ti1.Equals(ti2))
            {
                //bandera para encontrar el tipo de dato
                bool encontrado = false;
                for (int i = 0; i < Constante.TIPOS.Length; i++)
                {   //verificar si existe entre los tipos que existen
                    if (Constante.TIPOS[i].Equals(ti1))
                    {   //cuando se encontro la posicion es el numero al que pertenece el tipo
                        tipo = i;
                        encontrado = true;
                        break;
                    }
                }
                //si no se encontro definirlo como tipo de dato id
                if (!encontrado) { tipo = Constante.ID; }
            }
            else
            {
                string msg = "El tipo de dato no coniciden con las etiquetas (" + ti1 + ");(" + ti2 + ")";
                return msg;
            }
            /*Retorna el mensaje de error para saber en que analizador se analizo
             en caso de no tener error retornara null*/
            return null;
        }

        public string generar_xml()
        {
            throw new NotImplementedException();
        }
    }
}
