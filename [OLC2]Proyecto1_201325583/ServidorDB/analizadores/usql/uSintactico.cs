using Irony.Parsing;
using ServidorDB.arboles.xml;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;
using System;
using System.Collections.Generic;
using System.Data;
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
            List<arboles.usql.uInstruccion> inst = null;
            if(root != null)
            {
                inst = arboles.usql.uArbol.SENTENCIAS(root);
            }

            for(int i = 0; i < arbol.ParserMessages.Count; i++)
            {
                string tipo = "Sintactico/Lexico";
                int line = arbol.ParserMessages[i].Location.Line;
                int colm = arbol.ParserMessages[i].Location.Column;
                string descripcion = arbol.ParserMessages[i].Message;
                string lexema = "";
                uError er = new uError(tipo, descripcion, lexema, line, colm);
                uerrores.Add(er);
            }

            generar_grafoAST(root);
            return root;
        }

        public static void analizar_usql(string entrada)
        {
            //reiniciando todo
            Constante.tabla = null;
            Constante.mensaje = "";
            Constante.informacion_select = "";
            Constante.informacion_consola = "";
            uSintactico.uerrores.Clear();
            xSintactico.errores.Clear();

            Constante.crear_archivo(Constante.RUTA_USQL_SCRIPT, entrada);
            ParseTreeNode raiz = analizar(entrada);

            DateTime fechahora = DateTime.Now;
            string tiempo = Convert.ToString(fechahora);
            Constante.rtb_consola.Text += (">> " + tiempo + " admin [Analisis USQL]\n");
            
            if(raiz != null)
            {
                //por el momento solo ejecuto expresion suma
                //Constante.global = new tabla_simbolos.Entorno(null);

                Entorno subGlobal = new Entorno(Constante.global);
                List<arboles.usql.uInstruccion> inst = arboles.usql.uArbol.SENTENCIAS(raiz);

                //esto colocarlo en la otra pc
                /*Si estoy en el ultimo ambito en este caso necesito mostrar el error de detener
                 */
                object obj = null;
                for (int i = 0; i < inst.Count; i++)
                {

                    if (inst[i] is arboles.usql.SSL.Asignacion || inst[i] is arboles.usql.SSL.Declarar
                        || inst[i] is arboles.usql.SSL.Mientras || inst[i] is arboles.usql.SSL.Para || obj is arboles.usql.SSL.Si
                        || inst[i] is arboles.usql.SSL.Selecciona || inst[i] is arboles.usql.SSL.Imprimir)
                    {
                        String msg = "Las sentencia SSL no pertenece a este ambito";
                        uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, msg, "", 0, 0));
                    }
                    else
                    {

                        obj = inst[i].ejecutar(subGlobal);
                        if (obj is arboles.usql.Detener)
                        {
                            arboles.usql.Detener dt = (arboles.usql.Detener)obj;
                            String msg = "La sentencia detener no pertenece al ambito";
                            uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, msg, "", 0, 0));

                        }

                    }

                }

                if(Constante.tabla != null)
                {
                    //debo craear un string con los resultados
                        DataTable datos = Constante.tabla;
                        
                        if(datos.Rows.Count == 0)
                        {
                            //generando condigo plycs

                            string info = " \"paquete\" : \"usql\" , \"datos\" : ";
                            info += "[";
                            
                                info += "[";
                                for (int j = 0; j < datos.Columns.Count; j++)
                                {
                                    // "coluna" : "valor"
                                    info += "\"" + datos.Columns[j].ColumnName + "\" : ";
                                    info += "\"" + "      " + "\"";

                                    if (j != datos.Columns.Count - 1) { info += ","; }
                                }
                                info += "]";

                            info += "]";
                            Constante.informacion_select = info;
                        }
                        else
                        {
                            //generando condigo plycs

                            string info = " \"paquete\" : \"usql\" , \"datos\" : ";
                            info += "[";

                            for (int i = 0; i < datos.Rows.Count; i++)
                            {
                                //aqui viene una fila
                                info += "[";
                                for (int j = 0; j < datos.Columns.Count; j++)
                                {
                                    // "coluna" : "valor"
                                    info += "\"" + datos.Columns[j].ColumnName + "\" : ";
                                    info += "\"" + datos.Rows[i][datos.Columns[j]].ToString() + "\"";

                                    if(j != datos.Columns.Count - 1) { info += ","; }
                                }
                                info += "]";
                                if (i != datos.Rows.Count - 1) { info += ","; }
                            }
                            info += "]";
                            Constante.informacion_select = info;
                        }
                    }
            }
            
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
