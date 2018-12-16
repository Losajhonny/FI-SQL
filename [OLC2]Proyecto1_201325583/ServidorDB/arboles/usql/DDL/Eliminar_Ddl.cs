using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;

namespace ServidorDB.arboles.usql.DDL
{
    class Eliminar_Ddl : uInstruccion
    {
        private int tipo_usql; //es un tipo usql xD tablas, proc, fun, obj, .....
        private string id;
        private int line;
        private int colm;

        public Eliminar_Ddl(int tipo_usql, string id, int line, int colm)
        {
            this.tipo_usql = tipo_usql;
            this.id = id;
            this.line = line;
            this.colm = colm;
        }

        public int Tipo_usql { get => tipo_usql; set => tipo_usql = value; }
        public string Id { get => id; set => id = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }

        public object ejecutar(Entorno ent)
        {
            switch (tipo_usql)
            {
                case Constante.tBASE_DATOS:
                    break;
                case Constante.tTABLA:
                    dropTabla();
                    break;
                case Constante.tOBJETO:
                    dropObjeto();
                    break;
                case Constante.tUSUARIO:
                    dropUsuario();
                    break;
                case Constante.tPROCEDIMIENTO:
                    dropProcedimiento();
                    break;
                default://tfuncion
                    dropFuncion();
                    break;
            }
            return null;
        }

        public void dropTabla()
        {
            PeticionDDL.dropTabla(id, line, colm);
        }

        public void dropFuncion()
        {
            PeticionDDL.dropFuncion(id, line, colm);
        }

        public void dropObjeto()
        {
            PeticionDDL.dropObjeto(id, line, colm);
        }

        public void dropProcedimiento()
        {
            PeticionDDL.dropProcedimiento(id, line, colm);
        }

        public void dropUsuario()
        {
            //para el usuario no verifico los permisos
            //solamente el administrador puede eliminar un usuario pero no asi mismo
            PeticionDDL.dropUsuario(id, line, colm);
        }

        public string generar_booleano(Entorno ent)
        {
            return "";
        }

        object uInstruccion.generar_booleano(Entorno ent)
        {
            throw new NotImplementedException();
        }
    }
}
