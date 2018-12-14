using Irony.Parsing;
using ServidorDB.arboles.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.analizadores.xml
{
    class ObjAst
    {
        public static List<Objeto> LISTA(ParseTreeNode padre)
        {
            List<Objeto> lf = new List<Objeto>();
            for (int i = 0; i < padre.ChildNodes.Count; i++)
            {
                Objeto obj = OBJ(padre.ChildNodes[i]);
                if(obj != null) { lf.Add(obj); }
            }
            return lf;
        }

        public static Objeto OBJ(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 8)
            {
                string nombre = "";
                List<Atributo> attr = null;
                List<string> usuarios = null;

                List<object> lfuncion = LOBJ(padre.ChildNodes[3]);

                for (int i = 0; i < lfuncion.Count; i++)
                {
                    if (lfuncion[i] is List<string>)
                    {
                        usuarios = (List<string>)lfuncion[i];
                    }
                    else if (lfuncion[i] is List<Atributo>)
                    {
                        attr = (List<Atributo>)lfuncion[i];
                    }
                    else if (lfuncion[i] is string[])
                    {
                        string[] val = (string[])lfuncion[i];
                        nombre = val[1];
                    }
                }

                Objeto f = new Objeto(nombre);
                f.Parametros = attr;
                f.Usuarios = usuarios;

                f.Line = padre.ChildNodes[1].Token.Location.Line;
                f.Colm = padre.ChildNodes[1].Token.Location.Column;
                return f;
            }
            return null;
        }

        public static List<object> LOBJ(ParseTreeNode padre)
        {
            List<object> lf = new List<object>();
            for (int i = 0; i < padre.ChildNodes.Count; i++)
            {
                lf.Add(COBJ(padre.ChildNodes[i]));
            }
            return lf;
        }

        public static object COBJ(ParseTreeNode padre)
        {
            if (padre.ChildNodes[0].Term.Name.Equals("NOMBRE"))
            {
                string nombre = MasterAst.NOMBRE(padre.ChildNodes[0]);
                string[] val = { "nombre", nombre };
                return val;
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("ATTR"))
            {
                return ATTR(padre.ChildNodes[0]);
            }
            else
            {
                return DbAst.USUARIOS(padre.ChildNodes[0]);
            }
        }

        public static List<Atributo> ATTR(ParseTreeNode padre)
        {
            return ProcAst.ATRIBUTOS(padre.ChildNodes[3]);
        }
    }
}
