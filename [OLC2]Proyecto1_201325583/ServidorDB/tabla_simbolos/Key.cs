using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.tabla_simbolos
{
    class Key
    {
        public string id;
        public int tipo_simbolo;

        public Key(string id, int tipo_simbolo)
        {
            this.id = id;
            this.tipo_simbolo = tipo_simbolo;
        }
    }
}
