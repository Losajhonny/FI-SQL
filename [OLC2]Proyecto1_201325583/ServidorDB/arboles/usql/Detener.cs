using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.tabla_simbolos;

namespace ServidorDB.arboles.usql
{
    class Detener : uInstruccion
    {
        public object ejecutar(Entorno ent)
        {
            return this;
        }

        public object generar_booleano(Entorno ent)
        {
            throw new NotImplementedException();
        }
    }
}
