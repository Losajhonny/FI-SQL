using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql.Expresiones
{
    class NodoLVariable : NodoExp
    {
        private string val1;
        private string val2;

        public NodoLVariable(string val1, int line, int colm) : base(line, colm)
        {
            this.val1 = val1;
        }

        public NodoLVariable(string val1, string val2, int line, int colm) : base(line, colm)
        {
            this.val1 = val1;
            this.val2 = val2;
        }
    }
}
