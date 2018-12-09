using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;

namespace ServidorDB.arboles.usql.Expresiones
{
    class NodoPrimitivo : NodoExp
    {
        private string valor;
        private int tipo;
        
        public NodoPrimitivo(string valor, int tipo, int line, int colm) : base(line, colm)
        {
            this.valor = valor;
            this.tipo = tipo;
        }

        public override object ejecutar(Entorno ent)
        {
            if(tipo == Constante.TEXT)
            {
                char[] ctrim = { '\"' };
                string nvalor = valor.Trim(ctrim);
                nvalor = nvalor.Replace("\\n", "\n");
                nvalor = nvalor.Replace("\\t", "\t");
                return new Resultado(tipo, nvalor);
            }
            else
            {
                return new Resultado(tipo, valor);
            }
        }
    }
}
