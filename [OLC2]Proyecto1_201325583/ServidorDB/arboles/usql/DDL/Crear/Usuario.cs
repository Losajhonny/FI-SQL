using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.tabla_simbolos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql.DDL.Crear
{
    class Usuario : uInstruccion
    {
        private string id;
        private NodoExp password;
        private int line;
        private int colm;

        public Usuario(string id, NodoExp password, int line, int colm)
        {
            this.id = id;
            this.password = password;
            this.line = line;
            this.colm = colm;
        }

        public string Id { get => id; set => id = value; }
        public NodoExp Password { get => password; set => password = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }

        public object ejecutar(Entorno ent)
        {
            return null;
        }
    }
}
