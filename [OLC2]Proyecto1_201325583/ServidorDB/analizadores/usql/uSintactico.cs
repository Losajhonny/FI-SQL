using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.analizadores.usql
{
    class uSintactico
    {
        public static List<uError> uerrores = new List<uError>();

        public static ParseTreeNode analizar(string entrada)
        {
            uGramatica grammar = new uGramatica();
            Parser parser = new Parser(grammar);
            ParseTree arbol = parser.Parse(entrada);

            //para los errores buscarlos en arbol
            ParseTreeNode root = arbol.Root;
            return root;
        }
    }
}
