using Irony.Parsing;
using ServidorDB.analizadores.xml;
using ServidorDB.otros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.xml
{
    class xSintactico
    {
        public static string[] ERRORES_XML = {"Archivo Master", "Archivo Db", "Archivo FUNCION", "Archivo PROCEDIMIENTO",
            "Archivo OBJETO", "Archivo REGISTRO" };

        public static int ERROR_MASTER = 0;
        public static int ERROR_DB = 1;
        public static int ERROR_FUN = 2;
        public static int ERROR_PROC = 3;
        public static int ERROR_OBJ = 4;
        public static int ERROR_REG = 5;

        public static List<analizadores.usql.uError> errores = new List<analizadores.usql.uError>();

        public static List<object> analizarMaestro(string entrada)
        {
            MasterGrammar grammar = new MasterGrammar();
            Parser parser = new Parser(grammar);
            ParseTree arbol = parser.Parse(entrada);

            for (int i = 0; i < arbol.ParserMessages.Count; i++)
            {
                errores.Add(new analizadores.usql.uError(Constante.PARSER,
                    ERRORES_XML[ERROR_MASTER] + " : " + arbol.ParserMessages[i].Message, "",
                    arbol.ParserMessages[i].Location.Line, arbol.ParserMessages[i].Location.Line));
            }

            //para los errores buscarlos en arbol
            ParseTreeNode root = arbol.Root;
            if(root != null)
            {
                return MasterAst.LISTA(root);
            }
            return null;
        }

        public static List<object> analizarDb(string entrada)
        {
            DbGrammar grammar = new DbGrammar();
            Parser parser = new Parser(grammar);
            ParseTree arbol = parser.Parse(entrada);

            for (int i = 0; i < arbol.ParserMessages.Count; i++)
            {
                errores.Add(new analizadores.usql.uError(Constante.PARSER,
                    ERRORES_XML[ERROR_DB] + " : " + arbol.ParserMessages[i].Message, "",
                    arbol.ParserMessages[i].Location.Line, arbol.ParserMessages[i].Location.Line));
            }

            //para los errores buscarlos en arbol
            ParseTreeNode root = arbol.Root;
            if (root != null)
            {
                return DbAst.LISTA(root);
            }
            return null;
        }

        public static List<List<Atributo>> analizarRegistros(string entrada)
        {
            RegGrammar grammar = new RegGrammar();
            Parser parser = new Parser(grammar);
            ParseTree arbol = parser.Parse(entrada);

            for (int i = 0; i < arbol.ParserMessages.Count; i++)
            {
                errores.Add(new analizadores.usql.uError(Constante.PARSER,
                    ERRORES_XML[ERROR_REG] + " : " + arbol.ParserMessages[i].Message, "",
                    arbol.ParserMessages[i].Location.Line, arbol.ParserMessages[i].Location.Line));
            }

            //para los errores buscarlos en arbol
            ParseTreeNode root = arbol.Root;
            if(root != null)
            {
                return RowAst.ROWS(root);
            }
            return null;
        }

        public static List<Procedimiento> analizarProc(string entrada)
        {
            ProcGrammar grammar = new ProcGrammar();
            Parser parser = new Parser(grammar);
            ParseTree arbol = parser.Parse(entrada);

            for (int i = 0; i < arbol.ParserMessages.Count; i++)
            {
                errores.Add(new analizadores.usql.uError(Constante.PARSER,
                    ERRORES_XML[ERROR_PROC] + " : " + arbol.ParserMessages[i].Message, "",
                    arbol.ParserMessages[i].Location.Line, arbol.ParserMessages[i].Location.Line));
            }

            //para los errores buscarlos en arbol
            ParseTreeNode root = arbol.Root;
            if(root != null)
            {
                return ProcAst.LISTA(root);
            }
            return null;
        }

        public static List<Funcion> analizaarFunc(string entrada)
        {
            FunGrammar grammar = new FunGrammar();
            Parser parser = new Parser(grammar);
            ParseTree arbol = parser.Parse(entrada);

            for (int i = 0; i < arbol.ParserMessages.Count; i++)
            {
                errores.Add(new analizadores.usql.uError(Constante.PARSER,
                    ERRORES_XML[ERROR_FUN] + " : " + arbol.ParserMessages[i].Message, "",
                    arbol.ParserMessages[i].Location.Line, arbol.ParserMessages[i].Location.Line));
            }

            //para los errores buscarlos en arbol
            ParseTreeNode root = arbol.Root;
            if (root != null)
            {
                return FunAst.LISTA(root);
            }
            return null;
        }

        public static List<Objeto> analizarObj(string entrada)
        {
            ObjGrammar grammar = new ObjGrammar();
            Parser parser = new Parser(grammar);
            ParseTree arbol = parser.Parse(entrada);

            for (int i = 0; i < arbol.ParserMessages.Count; i++)
            {
                errores.Add(new analizadores.usql.uError(Constante.PARSER,
                    ERRORES_XML[ERROR_OBJ] + " : " + arbol.ParserMessages[i].Message, "",
                    arbol.ParserMessages[i].Location.Line, arbol.ParserMessages[i].Location.Line));
            }

            //para los errores buscarlos en arbol
            ParseTreeNode root = arbol.Root;
            if (root != null)
            {
                return ObjAst.LISTA(root);
            }
            return null;
        }
    }
}
