using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.analizadores.usql
{
    class uError
    {
        private string tipo;
        private string descripcion;
        private string lexema;
        private int line, colm;
        private string tambito;

        public uError(string tipo, string descripcion, string lexema, int line, int colm)
        {
            this.tipo = tipo;
            this.descripcion = descripcion;
            this.lexema = lexema;
            this.line = line;
            this.colm = colm;
        }

        public uError(string tambito, string tipo, string descripcion, string lexema, int line, int colm)
        {
            this.tipo = tipo;
            this.descripcion = descripcion;
            this.lexema = lexema;
            this.line = line;
            this.colm = colm;
            this.tambito = tambito;
        }

        public string Tipo { get => tipo; set => tipo = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public string Lexema { get => lexema; set => lexema = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }
        public string Tambito { get => tambito; set => tambito = value; }
    }
}
