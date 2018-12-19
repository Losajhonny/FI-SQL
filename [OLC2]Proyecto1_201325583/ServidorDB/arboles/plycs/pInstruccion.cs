using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.plycs
{
    interface pInstruccion
    {
        object ejecutar();

        object generar();
    }
}
