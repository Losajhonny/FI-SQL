using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql.DML
{
    class Select :  uInstruccion
    {
        private List<string> campos;
        private List<string> tablas;
        private NodoExp condicion;
        private string id_ordenar;
        private int tordenar;
        private int line;
        private int colm;

        public Select(List<string> campos, List<string> tablas, NodoExp condicion,
            string id_ordenar, int tordenar)
        {
            this.campos = campos;
            this.tablas = tablas;
            this.condicion = condicion;
            this.id_ordenar = id_ordenar;
            this.tordenar = tordenar;
        }

        public List<string> Campos { get => campos; set => campos = value; }
        public List<string> Tablas { get => tablas; set => tablas = value; }
        public string Id_ordenar { get => id_ordenar; set => id_ordenar = value; }
        public int Tordenar { get => tordenar; set => tordenar = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }
        internal NodoExp Condicion { get => condicion; set => condicion = value; }

        public object ejecutar(Entorno ent)
        {
            return null;
        }

        public object generar_booleano(Entorno ent)
        {
            throw new NotImplementedException();
        }
    }
}
