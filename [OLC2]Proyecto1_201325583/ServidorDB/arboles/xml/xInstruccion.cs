using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.xml
{
    interface xInstruccion
    {
        string generar_xml();

        object cargar();
    }
}
