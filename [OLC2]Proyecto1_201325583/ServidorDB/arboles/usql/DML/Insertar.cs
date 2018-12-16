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
    class Insertar : uInstruccion
    {
        protected string id; //nombre de la tabla
        protected List<NodoExp> valores;
        protected List<string> ids;
        protected int line;
        protected int colm;

        public Insertar(string id, List<NodoExp> valores, int line, int colm)
        {
            this.ids = null;
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

        public virtual object ejecutar(Entorno ent)
        {
            //el numero de los valores deben ser igual que el numero de atributos de la tabla
            //para ello necesito primero obtener valores y despues validar
            List<Resultado> resval = new List<Resultado>();
            bool hayError = false;

            for (int i = 0; i < valores.Count; i++)
            {
                Resultado res = (Resultado)valores[i].ejecutar(ent);
                if(res.Tipo == Constante.ERROR)
                {
                    hayError = true;
                }
                else
                {
                    resval.Add(res);
                }
            }

            if (!hayError)
            {
                //como no hay error al obtener resultados entonces ahora hay que realizar la peticion
                //al dbms enviar  nombre tabla, listaresultado, line, colm
                if(ids == null)
                {
                    PeticionDML.insertarNormal(id, resval, line, colm);
                }
                else
                {
                    PeticionDML.insertarEspecial(id, resval, ids, line, colm);
                }
            }
            return null;
        }

        public virtual object generar_booleano(Entorno ent)
        {
            throw new NotImplementedException();
        }
    }
}
