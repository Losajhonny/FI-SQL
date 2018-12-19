using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.analizadores.plycs
{
    class pSintactico
    {
        public static arboles.plycs.PlyAst analizar(string entrada)
        {
            pGramatica grammar = new pGramatica();
            Parser parser = new Parser(grammar);
            ParseTree arbol = parser.Parse(entrada);

            //para los errores buscarlos en arbol
            ParseTreeNode root = arbol.Root;
            if(root != null)
            {
                //return arboles.plycs.pArbol.INI(root);
            }
            return null;
        }
    }
}
