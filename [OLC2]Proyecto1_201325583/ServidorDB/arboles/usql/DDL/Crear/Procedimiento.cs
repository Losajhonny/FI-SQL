using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.tabla_simbolos;

namespace ServidorDB.arboles.usql.DDL.Crear
{
    class Procedimiento : uInstruccion
    {
        protected string id;
        protected List<uInstruccion> paramss;
        protected int tipo;
        protected List<uInstruccion> inst;
        protected int line;
        protected int colum;

        public Procedimiento(string id, int tipo, List<uInstruccion> paramss,
            List<uInstruccion> inst, int line, int colum)
        {
            this.id = id;
            this.paramss = paramss;
            this.tipo = tipo;
            this.inst = inst;
            this.line = line;
            this.colum = colum;
        }

        public string Id { get => id; set => id = value; }
        public int Tipo { get => tipo; set => tipo = value; }
        public int Line { get => line; set => line = value; }
        public int Colum { get => colum; set => colum = value; }
        public List<uInstruccion> Paramss { get => paramss; set => paramss = value; }
        public List<uInstruccion> Inst { get => inst; set => inst = value; }

        public virtual object ejecutar(Entorno ent)
        {
            return null;
        }
    }
}
