using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.tabla_simbolos;

namespace ServidorDB.arboles.usql.SSL
{
    class Sino : uInstruccion
    {
        private List<uInstruccion> inst;

        public Sino(List<uInstruccion> inst)
        {
            this.inst = inst;
        }

        public List<uInstruccion> Inst { get => inst; set => inst = value; }

        public object ejecutar(Entorno ent)
        {
            for (int i = 0; i < inst.Count; i++)
            {
                object obj = inst[i].ejecutar(ent);
                if (obj is Detener) return obj;
                if (obj is Retornar) return obj;
            }
            return null;
        }

        public object generar_booleano(Entorno ent)
        {
            throw new NotImplementedException();
        }
    }
}
