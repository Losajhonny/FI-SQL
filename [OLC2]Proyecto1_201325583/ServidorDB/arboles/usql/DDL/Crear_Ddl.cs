using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.arboles.xml;
using ServidorDB.tabla_simbolos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql.DDL
{
    class Crear_Ddl : uInstruccion
    {
        private int tipo_crear;
        private string id;

        private List<Atributo> atributos;
        private List<uInstruccion> inst;    //al parecer siempre va a ser null
        private List<uInstruccion> decs;
        private int tipo_dato;
        
        private NodoExp password;

        //ahora necesito guardar el script de las instrucciones
        //para eso necesito la posicion del archivo
        //entonces almaceno la linea inicial al igual que la columna
        //y la final

        //ademas de eso por si ocurre otros errores necesito
        //line y colm normales
        private int line_init;
        private int colm_init;
        private int line_fint;
        private int colm_fint;
        private int line;
        private int colm;

        /// <summary>
        /// Constructor para crear una base de datos o usarla
        /// </summary>
        /// <param name="tipo_crear"></param>
        /// <param name="id"></param>
        public Crear_Ddl(int tipo_crear, string id)
        {   //para crear base de datos o usar una base de datos
            this.tipo_crear = tipo_crear;
            this.id = id;
        }

        /// <summary>
        /// Constructor para tablas y objetos
        /// </summary>
        /// <param name="tipo_crear"></param>
        /// <param name="id"></param>
        /// <param name="atributos"></param>
        public Crear_Ddl(int tipo_crear, string id, List<uInstruccion> decs)
        {   //para tabla y objeto
            this.tipo_crear = tipo_crear;
            this.decs = decs;
        }

        /// <summary>
        /// Constructor para funciones y procedimientos
        /// </summary>
        /// <param name="tipo_crear"></param>
        /// <param name="id"></param>
        /// <param name="decs"></param>
        /// <param name="inst"></param>
        /// <param name="linei"></param>
        /// <param name="linef"></param>
        /// <param name="colmi"></param>
        /// <param name="colmf"></param>
        public Crear_Ddl(int tipo_crear, int tipo_retorno, string id, List<uInstruccion> decs,
            List<uInstruccion> inst, int linei, int linef, int colmi, int colmf)
        {
            this.tipo_dato = tipo_retorno;
            this.tipo_crear = tipo_crear;
            this.id = id;
            this.decs = decs;
            this.inst = inst;
            this.line_init = linei;
            this.line_fint = linef;
            this.colm_init = colmi;
            this.colm_fint = colmf;
        }

        /// <summary>
        /// Constructor para el usuario
        /// </summary>
        /// <param name="tipo_crear"></param>
        /// <param name="id"></param>
        /// <param name="password"></param>
        public Crear_Ddl(int tipo_crear, string id, NodoExp password)
        {
            this.tipo_crear = tipo_crear;
            this.id = id;
            this.password = password;
        }

        /// <summary>
        /// Constructor para crear una base de datos o usarla
        /// </summary>
        /// <param name="tipo_crear"></param>
        /// <param name="id"></param>
        public Crear_Ddl(int tipo_crear, string id, int line, int colm)
        {   //para crear base de datos o usar una base de datos
            this.tipo_crear = tipo_crear;
            this.id = id;
            this.line = line;
            this.colm = colm;
        }

        /// <summary>
        /// Constructor para tablas y objetos
        /// </summary>
        /// <param name="tipo_crear"></param>
        /// <param name="id"></param>
        /// <param name="atributos"></param>
        public Crear_Ddl(int tipo_crear, string id, List<uInstruccion> decs,
            int line, int colm)
        {   //para tabla y objeto
            this.tipo_crear = tipo_crear;
            this.decs = decs;
            this.line = line;
            this.colm = colm;
        }

        /// <summary>
        /// Constructor para el usuario
        /// </summary>
        /// <param name="tipo_crear"></param>
        /// <param name="id"></param>
        /// <param name="password"></param>
        public Crear_Ddl(int tipo_crear, string id, NodoExp password, 
            int line, int colm)
        {
            this.tipo_crear = tipo_crear;
            this.id = id;
            this.password = password;
            this.line = line;
            this.colm = colm;
        }


        public int Tipo_crear { get => tipo_crear; set => tipo_crear = value; }
        public string Id { get => id; set => id = value; }
        public List<Atributo> Atributos { get => atributos; set => atributos = value; }
        public int Tipo_dato { get => tipo_dato; set => tipo_dato = value; }
        public int Line_init { get => line_init; set => line_init = value; }
        public int Colm_init { get => colm_init; set => colm_init = value; }
        public int Line_fint { get => line_fint; set => line_fint = value; }
        public int Colm_fint { get => colm_fint; set => colm_fint = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }
        internal List<uInstruccion> Inst { get => inst; set => inst = value; }
        internal NodoExp Password { get => password; set => password = value; }
        internal List<uInstruccion> Decs { get => decs; set => decs = value; }

        public object ejecutar(Entorno ent)
        {
            throw new NotImplementedException();
        }
    }
}
