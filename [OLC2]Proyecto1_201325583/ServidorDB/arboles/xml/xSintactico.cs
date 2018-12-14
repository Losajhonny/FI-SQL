using Irony.Parsing;
using ServidorDB.analizadores.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.xml
{
    class xSintactico
    {
        public static List<analizadores.usql.uError> errores = new List<analizadores.usql.uError>();

        public static List<object> analizarMaestro(string entrada)
        {
            MasterGrammar grammar = new MasterGrammar();
            Parser parser = new Parser(grammar);
            ParseTree arbol = parser.Parse(entrada);

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

            //para los errores buscarlos en arbol
            ParseTreeNode root = arbol.Root;
            if(root != null)
            {
                return ProcAst.LISTA(root);
            }
            return null;
        }


        /*
        
        public static ParseTreeNode analizar(string entrada)
        {
            RegGrammar grammar = new RegGrammar();
            Parser parser = new Parser(grammar);
            ParseTree arbol = parser.Parse(entrada);

            //para los errores buscarlos en arbol
            ParseTreeNode root = arbol.Root;
            return root;
        }

        public static ParseTreeNode analizar(string entrada)
        {
            RegGrammar grammar = new RegGrammar();
            Parser parser = new Parser(grammar);
            ParseTree arbol = parser.Parse(entrada);

            //para los errores buscarlos en arbol
            ParseTreeNode root = arbol.Root;
            return root;
        }

        public static ParseTreeNode analizar(string entrada)
        {
            RegGrammar grammar = new RegGrammar();
            Parser parser = new Parser(grammar);
            ParseTree arbol = parser.Parse(entrada);

            //para los errores buscarlos en arbol
            ParseTreeNode root = arbol.Root;
            return root;
        }

        public static ParseTreeNode analizar(string entrada)
        {
            RegGrammar grammar = new RegGrammar();
            Parser parser = new Parser(grammar);
            ParseTree arbol = parser.Parse(entrada);

            //para los errores buscarlos en arbol
            ParseTreeNode root = arbol.Root;
            return root;
        }

        public static ParseTreeNode analizar(string entrada)
        {
            RegGrammar grammar = new RegGrammar();
            Parser parser = new Parser(grammar);
            ParseTree arbol = parser.Parse(entrada);

            //para los errores buscarlos en arbol
            ParseTreeNode root = arbol.Root;
            return root;
        }

        public static ParseTreeNode analizar(string entrada)
        {
            RegGrammar grammar = new RegGrammar();
            Parser parser = new Parser(grammar);
            ParseTree arbol = parser.Parse(entrada);

            //para los errores buscarlos en arbol
            ParseTreeNode root = arbol.Root;
            return root;
        }*/
    }
}
