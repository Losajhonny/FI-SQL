using Irony.Parsing;
using ServidorDB.arboles.xml;
using ServidorDB.otros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.analizadores.xml
{
    class ProcAst
    {
        public static List<Procedimiento> LISTA(ParseTreeNode padre)
        {
            List<Procedimiento> lf = new List<Procedimiento>();
            for (int i = 0; i < padre.ChildNodes.Count; i++)
            {
                Procedimiento pr = FUNCION(padre.ChildNodes[i]);
                if (pr != null) { lf.Add(pr); }
            }
            return lf;
        }

        public static Procedimiento FUNCION(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 8)
            {
                string nombre = "";
                string src = "";
                List<Atributo> param = new List<Atributo>();
                List<string> usuarios = new List<string>();

                List<object> lfuncion = LPROC(padre.ChildNodes[3]);

                for (int i = 0; i < lfuncion.Count; i++)
                {
                    if (lfuncion[i] is List<string>)
                    {
                        usuarios = (List<string>)lfuncion[i];
                    }
                    else if (lfuncion[i] is List<Atributo>)
                    {
                        param = (List<Atributo>)lfuncion[i];
                    }
                    else if (lfuncion[i] is string[])
                    {
                        string[] val = (string[])lfuncion[i];
                        if (val[0].Equals("nombre"))
                        {
                            nombre = val[1];
                        }
                        else
                        {
                            src = val[1];
                        }
                    }
                }

                Procedimiento f = new Procedimiento(Constante.VOID, nombre);
                f.Src = src;
                f.Parametros = param;
                f.Usuarios = usuarios;

                f.Line = padre.ChildNodes[1].Token.Location.Line;
                f.Colm = padre.ChildNodes[1].Token.Location.Column;

                return f;
            }
            return null;
        }

        public static List<object> LPROC(ParseTreeNode padre)
        {
            List<object> lf = new List<object>();
            for (int i = 0; i < padre.ChildNodes.Count; i++)
            {
                lf.Add(CPROC(padre.ChildNodes[i]));
            }
            return lf;
        }

        public static object CPROC(ParseTreeNode padre)
        {
            if (padre.ChildNodes[0].Term.Name.Equals("NOMBRE"))
            {
                string nombre = MasterAst.NOMBRE(padre.ChildNodes[0]);
                string[] val = { "nombre", nombre };
                return val;
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("PARAMS"))
            {
                return PARAMS(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("USUARIOS"))
            {
                return DbAst.USUARIOS(padre.ChildNodes[0]);
            }
            else
            {
                string cad = SRC(padre.ChildNodes[0]);
                string[] val = { "src", cad };
                return val;
            }
        }

        public static List<Atributo> PARAMS(ParseTreeNode padre)
        {
            return ATRIBUTOS(padre.ChildNodes[3]);
        }

        public static string SRC(ParseTreeNode padre)
        {
            return padre.ChildNodes[3].Token.Text.Trim('~');
        }

        public static List<Atributo> ATRIBUTOS(ParseTreeNode padre)
        {
            List<Atributo> la = new List<Atributo>();
            for (int i = 0; i < padre.ChildNodes.Count; i++)
            {
                la.Add(DbAst.ATRIBUTO(padre.ChildNodes[i]));
            }
            return la;
        }
    }
}
