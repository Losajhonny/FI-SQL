using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;

namespace ServidorDB.arboles.usql.DML
{
    class Actualizar : Insertar
    {
        private NodoExp exp;

        public Actualizar(string id, List<string> ids, List<NodoExp> valores, NodoExp exp, int line, int colm)
            : base(id, ids, valores, line, colm) { this.exp = exp; }

        public NodoExp Exp { get => exp; set => exp = value; }

        public override object ejecutar(Entorno ent)
        {
            //necesito los resultados de los datos a actualizar
            List<Resultado> resval = new List<Resultado>();
            bool hayError = false;

            for (int i = 0; i < valores.Count; i++)
            {
                Resultado res = (Resultado)valores[i].ejecutar(ent);
                if (res.Tipo == Constante.ERROR)
                {
                    hayError = true;
                }
                else
                {
                    resval.Add(res);
                }
            }

            string cadena_where = (exp != null) ? exp.generar_booleano(ent).ToString() : null;

            if (!hayError)
            {
                //como no hay error al obtener resultados entonces ahora hay que realizar la peticion
                //al dbms enviar  nombre tabla, listaresultado, line, colm
                PeticionDML.actualizar(id, resval, ids, cadena_where, line, colm);
            }
            return null;
        }

        
    }
}
