using ServidorDB.tabla_simbolos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql
{
    interface uInstruccion
    {
        object ejecutar(Entorno ent);
    }
}
