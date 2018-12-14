using Irony.Parsing;
using ServidorDB.arboles.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.analizadores.xml
{
    class FunAst
    {
        private static List<int> lines = new List<int>();
        private static List<int> colms = new List<int>();

        public static List<Funcion> LISTA(ParseTreeNode padre)
        {
            List<Funcion> lf = new List<Funcion>();
            for (int i = 0; i < padre.ChildNodes.Count; i++)
            {
                Funcion obj = FUNCION(padre.ChildNodes[i]);
                if(obj != null) { lf.Add(obj); }
            }
            return lf;
        }

        public static Funcion FUNCION(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 8)
            {
                string nombre = "";
                string tipo = "";
                string src = "";
                List<Atributo> param = null;
                List<string> usuarios = null;

                List<object> lfuncion = LFUNCION(padre.ChildNodes[3]);

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
                        else if (val[0].Equals("tipo"))
                        {
                            tipo = val[1];
                        }
                        else
                        {
                            src = val[1];
                        }
                    }
                }

                Funcion f = new Funcion(-1, nombre);
                f.Tipo_ = tipo;
                f.Src = src;
                f.Parametros = param;
                f.Usuarios = usuarios;

                f.Line = padre.ChildNodes[1].Token.Location.Line;
                f.Colm = padre.ChildNodes[1].Token.Location.Column;

                return f;
            }
            return null;
        }

        public static List<object> LFUNCION(ParseTreeNode padre)
        {
            lines.Clear();
            colms.Clear();
            List<object> lf = new List<object>();
            for (int i = 0; i < padre.ChildNodes.Count; i++)
            {
                lf.Add(CFUNCION(padre.ChildNodes[i]));
            }
            return lf;
        }

        public static object CFUNCION(ParseTreeNode padre)
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
            else if (padre.ChildNodes[0].Term.Name.Equals("RETORNO"))
            {
                string tipo = RETORNO(padre.ChildNodes[0]);
                string[] val = { "tipo", tipo };
                return val;
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

        public static string RETORNO(ParseTreeNode padre)
        {
            lines.Add(padre.ChildNodes[3].Token.Location.Line);
            colms.Add(padre.ChildNodes[3].Token.Location.Column);
            return MasterAst.NOMBRE(padre);
        }

        public static string SRC(ParseTreeNode padre)
        {
            return padre.ChildNodes[3].Token.Text.Trim('~');
        }

        public static List<Atributo> ATRIBUTOS(ParseTreeNode padre)
        {
            List<Atributo> la = new List<Atributo>();
            for(int i = 0; i < padre.ChildNodes.Count; i++)
            {
                la.Add(DbAst.ATRIBUTO(padre.ChildNodes[i]));
            }
            return la;
        }        
    }
}
