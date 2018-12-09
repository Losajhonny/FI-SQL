using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.analizadores.usql
{
    class uSintactico
    {
        public static string grafo;
        public static List<uError> uerrores = new List<uError>();

        public static ParseTreeNode analizar(string entrada)
        {
            uGramatica grammar = new uGramatica();
            Parser parser = new Parser(grammar);
            ParseTree arbol = parser.Parse(entrada);

            //para los errores buscarlos en arbol
            ParseTreeNode root = arbol.Root;
            generar_grafoAST(root);
            return root;
        }

        /**
         * Genera una cadena donde contiene la estructura .dot
         * para la generacion de la imagen del arbol
         * */
        public static string generarAST(ParseTreeNode raiz)
        {
            grafo = "digraph grafo{\n";
            if (raiz != null)
            {
                grafo += "node [shape = egg];\n";
                grafo += raiz.GetHashCode() + "[label=\"" + escapar(raiz.ToString()) + "\", style = filled, color = lightblue];\n";
                recorrerAst(raiz.GetHashCode(), raiz);
            }
            grafo += "}";
            return grafo;
        }

        /**
         * Realiza el recorrido del arbol agregando la cadena
         * correspondiente al grafo
         * */
        private static void recorrerAst(int padre, ParseTreeNode hijos)
        {
            foreach (ParseTreeNode hijo in hijos.ChildNodes)
            {
                int nombreHijo = hijo.GetHashCode();
                grafo += nombreHijo + "[label=\"" + escapar(hijo.ToString()) +
                    "\", style = filled, color = lightblue];\n";
                grafo += padre + "->" + nombreHijo + ";\n";
                recorrerAst(nombreHijo, hijo);
            }
        }

        /**
         * Elimina el caracter de escape que puede generar problemas con
         * el lenguaje dot y agrega el caracter que lo reconoce
         * */
        private static String escapar(String cadena)
        {
            cadena = cadena.Replace("\\", "\\\\");
            cadena = cadena.Replace("\"", "\\\"");
            return cadena;
        }

        /**
         * Genera la imagen del grafo y lo almacena en la carpeta del proyecto
         * */
        public static void generar_grafoAST(ParseTreeNode raiz)
        {
            string grafo = generarAST(raiz);
            StreamWriter w = new StreamWriter("arbolAst.dot");
            w.WriteLine(grafo);
            w.Close();

            ProcessStartInfo p = new ProcessStartInfo("C:\\release\\bin\\dot.exe");
            p.WindowStyle = ProcessWindowStyle.Normal;
            p.RedirectStandardOutput = true;
            p.UseShellExecute = false;
            p.CreateNoWindow = true;
            p.WindowStyle = ProcessWindowStyle.Hidden;
            p.Arguments = "-Tpng arbolAst.dot -o arbolAst.png";
            Process.Start(p);
        }
    }
}
