using ServidorDB.otros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.xml
{
    public class Procedimiento : Funcion
    {
        /**
         * Herada de funcion tiene los mismos parametros, usuarios, etc
         * Lo que cambia es que no va a tener tipo se le asigna
         * como Constante.VOID
         * Ademas tiene funcion diferente
         */

        public Procedimiento(int tipo, string nombre) : base(tipo, nombre)
        {
            this.ruta = Constante.RUTA_PROCEDIMIENTOS + nombre + "." + Constante.EXTENSION;
        }

        public Procedimiento(int tipo, string nombre, string src) : base(tipo, nombre, src)
        {
            this.ruta = Constante.RUTA_PROCEDIMIENTOS + nombre + "." + Constante.EXTENSION;
        }
    }
}
