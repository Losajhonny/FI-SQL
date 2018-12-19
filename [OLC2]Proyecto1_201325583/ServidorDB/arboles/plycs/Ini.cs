using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.plycs
{
    class Ini
    {
        private string numero;
        private Paquete paquete;
        private int line;
        private int colm;

        public Ini(string numero, Paquete p, int line, int colm)
        {
            this.numero = numero;
            this.paquete = p;
            this.line = line;
            this.colm = colm;
        }
    }
}
