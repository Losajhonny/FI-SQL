using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;

namespace ServidorDB.arboles.usql.Expresiones
{
    class Contar : NodoExp
    {
        private uInstruccion select;
        
        public Contar(uInstruccion select, int line, int colm) : base(line, colm)
        {
            this.select = select;
        }

        public override object ejecutar(Entorno ent)
        {
            object obj = (select != null) ? select.ejecutar(ent) : null;

            if (obj != null)
            {
                DataTable dt = (DataTable)obj;

                int i = 0;
                while (i < dt.Rows.Count)
                {
                    i++;
                }
                return new Resultado(Constante.INTEGER, i.ToString());
            }
            return new Resultado(Constante.INTEGER, "0");
        }
    }
}
