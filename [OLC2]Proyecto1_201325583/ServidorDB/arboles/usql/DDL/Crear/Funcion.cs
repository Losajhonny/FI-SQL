using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.tabla_simbolos;

namespace ServidorDB.arboles.usql.DDL.Crear
{
    class Funcion : Procedimiento
    {
        public Funcion(string id, int tipo, 
            List<uInstruccion> paramss, List<uInstruccion> inst, 
            int line, int colum) 
            : base(id, tipo, paramss, inst, line, colum) { }

        public override object ejecutar(Entorno ent)
        {
            return base.ejecutar(ent);
        }
    }
}
