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
    class DbAst
    {
        /*En la clase db tengo que recorrer la lista*/
        public static List<object> LISTA(ParseTreeNode padre)
        {
            List<object> ldb = new List<object>();
            for (int i = 0; i < padre.ChildNodes.Count; i++)
            {
                object d = DB(padre.ChildNodes[i]);
                if(d != null) { ldb.Add(d); }
            }
            return ldb;
        }

        public static object DB(ParseTreeNode padre)
        {
            if (padre.ChildNodes[0].Term.Name.Equals("PROCEDURE"))
            {
                return PROCEDURE(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("FUNCTION"))
            {
                return FUNCTION(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("OBJECT"))
            {
                return OBJECT(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("TABLA"))
            {
                return TABLA(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("USUARIOS"))
            {
                return USUARIOS(padre.ChildNodes[0]);
            }
            return null;
        }

        public static string[] PROCEDURE(ParseTreeNode padre)
        {
            string tmp = "proc";
            string path = MasterAst.PATH(padre.ChildNodes[3]);
            string[] val = { tmp, path };
            return val;
        }

        public static string[] FUNCTION(ParseTreeNode padre)
        {
            string tmp = "func";
            string path = MasterAst.PATH(padre.ChildNodes[3]);
            string[] val = { tmp, path };
            return val;
        }

        public static string[] OBJECT(ParseTreeNode padre)
        {
            string tmp = "object";
            string path = MasterAst.PATH(padre.ChildNodes[3]);
            string[] val = { tmp, path };
            return val;
        }

        public static List<string> USUARIOS(ParseTreeNode padre)
        {
            return LISTA_USER(padre.ChildNodes[3]);
        }

        public static List<string> LISTA_USER(ParseTreeNode padre)
        {
            List<string> lu = new List<string>();
            for(int i = 0; i < padre.ChildNodes.Count; i++)
            {
                lu.Add(MasterAst.NOMBRE(padre.ChildNodes[i]));
            }
            return lu;
        }

        public static Tabla TABLA(ParseTreeNode padre)
        {
            string nombre = null;
            string path = null;
            List<Atributo> rows = null;
            List<string> usuarios = null;
            List<object> etq = ETIQUETAS(padre.ChildNodes[3]);

            for(int i = 0; i < etq.Count; i++)
            {
                if(etq[i] is List<Atributo>)
                {
                    rows = (List<Atributo>)etq[i];
                }else if(etq[i] is string[])
                {
                    string[] val = (string[])etq[i];
                    if (val[0].Equals("nombre"))
                    {
                        nombre = val[1];
                    }
                    else
                    {
                        path = val[1];
                    }
                }else if(etq[i] is List<string>)
                {
                    usuarios = (List<string>)etq[i];
                }
            }
            Tabla t = new Tabla(nombre);
            t.Ruta = path;
            t.Atributos = rows;
            t.Usuarios = usuarios;
            t.crearDataTable();

            t.Line = padre.ChildNodes[1].Token.Location.Line;
            t.Colm = padre.ChildNodes[1].Token.Location.Column;
            return t;
        }

        public static List<object> ETIQUETAS(ParseTreeNode padre)
        {
            List<object> obj = new List<object>();
            for(int i = 0; i < padre.ChildNodes.Count; i++)
            {
                obj.Add(ETIQUETA(padre.ChildNodes[i]));
            }
            return obj;
        }

        public static object ETIQUETA(ParseTreeNode padre)
        {
            if (padre.ChildNodes[0].Term.Name.Equals("NOMBRE"))
            {
                string nombre = MasterAst.NOMBRE(padre.ChildNodes[0]);
                string[] val = { "nombre", nombre };
                return val;
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("PATH"))
            {
                string path = MasterAst.PATH(padre.ChildNodes[0]);
                string[] val = { "path", path };
                return val;
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("USUARIOS"))
            {
                return USUARIOS(padre.ChildNodes[0]);
            }
            else
            {
                return ROWS(padre.ChildNodes[0]);
            }
        }

        public static List<Atributo> ROWS(ParseTreeNode padre)
        {
            return ATRIBUTOS(padre.ChildNodes[3]);
        }

        public static List<Atributo> ATRIBUTOS(ParseTreeNode padre)
        {
            List<Atributo> atributos = new List<Atributo>();
            for(int i = 0; i < padre.ChildNodes.Count; i++)
            {
                Atributo atr = ATRI(padre.ChildNodes[i]);
                if(atr != null)
                {
                    atributos.Add(atr);
                }
            }
            return atributos;
        }
        
        public static Atributo ATRI(ParseTreeNode padre)
        {
            Atributo atr = ATRIBUTO(padre.ChildNodes[0]);
            if (atr != null)
            {
                string[] comp = COMPLE_FOR(padre.ChildNodes[1]);
                if (comp[0] == null)
                {
                    atr.Complemento = comp[1];
                }
                else
                {
                    atr.Tabla = comp[0];
                    atr.Attr = comp[1];
                }
                return atr;
            }
            return null;
        }

        public static Atributo ATRIBUTO(ParseTreeNode padre)
        {
            string id1 = padre.ChildNodes[1].Token.Text;
            string id2 = padre.ChildNodes[6].Token.Text;
            //int t1 = -1;
            //int t2 = -1;
            string nombre = padre.ChildNodes[3].Token.Text;
            /*for (int i = 0; i < Constante.TIPOS.Length; i++)
            {
                if (id1.ToLower().Equals(Constante.TIPOS[i]))
                {
                    t1 = i;
                    break;
                }
            }
            for (int i = 0; i < Constante.TIPOS.Length; i++)
            {
                if (id2.ToLower().Equals(Constante.TIPOS[i]))
                {
                    t2 = i;
                    break;
                }
            }

            if (t1 == t2 && t1 != -1)
            {*/
            Atributo at = new Atributo(id1, id2, nombre);
            at.Line = padre.ChildNodes[1].Token.Location.Line;
            at.Colm = padre.ChildNodes[1].Token.Location.Column;
            return at;
            /*}
            else
            {
                //debo reportar el error como semantico
            }
            return null;*/
        }

        public static string[] COMPLE_FOR(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 10)
            {
                string comp = padre.ChildNodes[3].Token.Text;
                string[] val = { null, comp };
                return val;
            }
            else
            {
                string tabla = padre.ChildNodes[3].Token.Text;
                string id = padre.ChildNodes[5].Token.Text;
                string[] val = { tabla, id };
                return val;
            }
        }
    }
}
