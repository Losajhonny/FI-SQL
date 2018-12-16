using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.tabla_simbolos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql.DML
{
    class Insertar : uInstruccion
    {
        private string id;
        private List<NodoExp> valores;
        private List<string> ids;
        private int line;
        private int colm;

        public Insertar(string id, List<NodoExp> valores, int line, int colm)
        {
            this.line = line;
            this.colm = colm;
            this.id = id;
            this.valores = valores;
        }

        public Insertar(string id, List<string> ids, List<NodoExp> valores, int line, int colm)
        {
            this.id = id;
            this.ids = ids;
            this.valores = valores;
            this.line = line;
            this.colm = colm;
        }

        public string Id { get => id; set => id = value; }
        public List<NodoExp> Valores { get => valores; set => valores = value; }
        public List<string> Ids { get => ids; set => ids = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }

        public object ejecutar(Entorno ent)
        {
            /*
             * 
             * VOY POR AQUI
             * 
             * 
             */
            return null;
        }
    }
}
