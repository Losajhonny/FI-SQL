using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.arboles.xml;
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
            /*ESTO ME SIRVE PARA ESTOS SELECT
             
            SELECT * DE TABLA_N DONDE TABLAX.ID > TABLAX.ID || TABLAY.ID = TABLAX.ID
            SELECT * DE TABLA DONDE ID > 19 && 19 < 19

            PERO NO PARA

            SELECT * DE TABLA_N DONDE ID == ID && ID < ID && VALOR > ID
            COMO VIENE MUCHAS TABLAS DEBE ESPECIFICAR DE QUE ATRIBUTO ESTARA SELECCIONANDO
             */

            Constante.lista = tablas;
            //ahora me voy a la generar_booleano en la parte de id. id donde
            //el primer id != @
            //recordando que la base de datos esta activa en el actual
            string cond = null;
            if (Constante.usuando_db_actual)
            {
                //como se uso la sentencia usar ya debio de actualizar el sistema de archivos
                //en memoria y en archivo
                //por lo que puedo reazliar la busquedad de tablas con el master de la constante
                Db db = Constante.sistema_archivo.Dbs[Constante.db_actual];
                Dictionary<string, Tabla> tabs = new Dictionary<string, Tabla>();
                List<Tabla> ttmp = new List<Tabla>();
                bool hayError = false;
                foreach (string tab in tablas)
                {
                    if (db.Tablas.ContainsKey(tab))
                    {
                        ttmp.Add(db.Tablas[tab]);
                        try { tabs.Add(tab, db.Tablas[tab]); } catch (Exception ex) { };
                    }
                    else
                    {
                        //si no existe mas de alguna tabla detectar error y no realizar el select
                        hayError = true;
                    }
                }
                if (!hayError)
                {
                    Constante.tablas = tabs;
                    Constante.tablasl = ttmp;
                    cond = (condicion != null) ? condicion.generar_booleano(ent).ToString() : null;
                }
            }
            return PeticionDML.seleccionar(campos, tablas, cond, id_ordenar, tordenar, line, colm);
        }

        public object generar_booleano(Entorno ent)
        {
            throw new NotImplementedException();
        }
    }
}
