﻿using Irony.Parsing;
using ServidorDB.analizadores.usql;
using ServidorDB.analizadores.xml;
using ServidorDB.arboles.xml;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServidorDB
{
    public partial class ServidorDb : Form
    {
        public ServidorDb()
        {
            //Tomar en cuenta para general servidorDb
            //necesito crear el usuario de administrador
            Usuario usr = new Usuario(Constante.usuario_admin, Constante.password_admin);
            PeticionDDL.crearUsuario(usr);
            //Finalizando la insercion de crear usuario
            //si existe no notificar que existe
            


            Constante.rtb_consola.AcceptsTab = true;
            Constante.rtb_consola.Location = new System.Drawing.Point(12, 422);
            Constante.rtb_consola.Name = "rtb_consola";
            Constante.rtb_consola.Size = new System.Drawing.Size(776, 221);
            Constante.rtb_consola.TabIndex = 3;
            Constante.rtb_consola.Text = "";
            this.Controls.Add(Constante.rtb_consola);
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Constante.crear_archivo(Constante.RUTA_USQL_SCRIPT, richTextBox1.Text);
            ParseTreeNode raiz = uSintactico.analizar(richTextBox1.Text);
            Constante.rtb_consola.Text = "";
            uSintactico.uerrores.Clear();
            if (raiz == null)
            {
                string msg = "<< raiz null Mostrando errores >>\n";
                for (int i = 0; i < uSintactico.uerrores.Count; i++)
                {
                    msg += "Descripcion: " + uSintactico.uerrores[i].Descripcion + " Lexema: " + uSintactico.uerrores[i].Lexema + "\n";
                }
                richTextBox2.Text = msg;
            }
            else
            {
                //por el momento solo ejecuto expresion suma
                //Constante.global = new tabla_simbolos.Entorno(null);

                Entorno subGlobal = new Entorno(Constante.global);
                List<arboles.usql.uInstruccion> inst = arboles.usql.uArbol.SENTENCIAS(raiz);

                //esto colocarlo en la otra pc
                /*Si estoy en el ultimo ambito en este caso necesito mostrar el error de detener
                 */
                for (int i = 0; i < inst.Count; i++)
                {
                    object obj = inst[i].ejecutar(subGlobal);
                    if (obj is arboles.usql.Detener)
                    {
                        arboles.usql.Detener dt = (arboles.usql.Detener)obj;
                        String msg = "La sentencia detener no pertenece al ambito";
                        uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, msg, "", 0, 0));

                    }
                }

                string msg1 = "";
                for (int i = 0; i < uSintactico.uerrores.Count; i++)
                {
                    msg1 += "Descripcion: " + uSintactico.uerrores[i].Descripcion + "\n";// + " Lexema: " + uSintactico.uerrores[i].Lexema + "\n";
                }
                richTextBox2.Text = "\n<< Mostrando errores >>\n" + msg1;
            }


            /*
            Maestro m = new Maestro();
            List<string> reg = new List<string>()
            {
                "hola", "reg", "hsla"
            };

            Atributo a1 = new Atributo(Constante.BOOL, "a", "auto"); a1.Registros = reg;
            Atributo a2 = new Atributo(Constante.INTEGER, "b", "primarykey"); a2.Registros = reg;
            Atributo a3 = new Atributo(Constante.INTEGER, "c", "tabla", "attr");
            List<Atributo> ats = new List<Atributo>()
            {
                a1, a2, a3
            };

            Usuario u1 = new Usuario("u1", "u1");
            Usuario u2 = new Usuario("u2", "u2");
            Usuario u3 = new Usuario("u3", "u3");
            List<string> urs = new List<string>
            {
                "u1", "u2", "u3"
            };

            Db db1 = new Db("db1");
            Db db2 = new Db("db2");
            Db db3 = new Db("db3");

            Objeto obj1 = new Objeto("obj1");
            Objeto obj2 = new Objeto("obj1");
            Objeto obj3 = new Objeto("obj1");

            Funcion f1 = new Funcion(Constante.INTEGER, "a", "f1");
            Funcion f2 = new Funcion(Constante.INTEGER, "a", "f2");
            Funcion f3 = new Funcion(Constante.INTEGER, "a", "f3");

            Procedimiento p1 = new Procedimiento(Constante.VOID, "a", "p1");
            Procedimiento p2 = new Procedimiento(Constante.VOID, "a", "p2");
            Procedimiento p3 = new Procedimiento(Constante.VOID, "a", "p3");

            Tabla t1 = new Tabla("t1");
            Tabla t2 = new Tabla("t2");
            Tabla t3 = new Tabla("t3");
            #region
            t1.Atributos = ats;
            t1.Usuarios = urs;

            t2.Atributos = ats;
            t2.Usuarios = urs;

            t3.Atributos = ats;
            t3.Usuarios = urs;

            p1.Parametros = ats;
            p1.Usuarios = urs;

            p2.Parametros = ats;
            p2.Usuarios = urs;

            p3.Parametros = ats;
            p3.Usuarios = urs;

            f1.Parametros = ats;
            f1.Usuarios = urs;

            f2.Parametros = ats;
            f2.Usuarios = urs;

            f3.Parametros = ats;
            f3.Usuarios = urs;

            obj1.Parametros = ats;
            obj1.Usuarios = urs;

            obj2.Parametros = ats;
            obj2.Usuarios = urs;

            obj3.Parametros = ats;
            obj3.Usuarios = urs;
            #endregion

            t1.crearDataTable();
            t2.crearDataTable();
            t3.crearDataTable();

            string[] registro = { "2", "daf", "asdf" };

            t1.registrar_prueba(registro);
            t2.registrar_prueba(registro);
            t3.registrar_prueba(registro);

            m.crear_base_datos("db1", db1);
            m.crear_base_datos("db2", db2);
            m.crear_base_datos("db3", db3);

            m.crear_usuario("u1", u1);
            m.crear_usuario("u2", u2);
            m.crear_usuario("u3", u3);

            m.crear_objeto("db1", "obj1", obj1);
            m.crear_objeto("db1", "obj2", obj2);
            m.crear_objeto("db1", "obj3", obj3);

            m.crear_objeto("db2", "obj1", obj1);
            m.crear_objeto("db2", "obj2", obj2);
            m.crear_objeto("db2", "obj3", obj3);

            m.crear_objeto("db3", "obj1", obj1);
            m.crear_objeto("db3", "obj2", obj2);
            m.crear_objeto("db3", "obj3", obj3);

            m.crear_procedimiento("db1", "p1", p1);
            m.crear_procedimiento("db1", "p2", p2);
            m.crear_procedimiento("db1", "p3", p3);

            m.crear_procedimiento("db2", "p1", p1);
            m.crear_procedimiento("db2", "p2", p2);
            m.crear_procedimiento("db2", "p3", p3);

            m.crear_procedimiento("db3", "p1", p1);
            m.crear_procedimiento("db3", "p2", p2);
            m.crear_procedimiento("db3", "p3", p3);

            m.crear_funcion("db1", "f1", f1);
            m.crear_funcion("db1", "f2", f2);
            m.crear_funcion("db1", "f3", f3);

            m.crear_funcion("db2", "f1", f1);
            m.crear_funcion("db2", "f2", f2);
            m.crear_funcion("db2", "f3", f3);

            m.crear_funcion("db3", "f1", f1);
            m.crear_funcion("db3", "f2", f2);
            m.crear_funcion("db3", "f3", f3);

            m.crear_tabla("db1", "t1", t1);
            m.crear_tabla("db1", "t2", t2);
            m.crear_tabla("db1", "t3", t3);

            m.crear_tabla("db2", "t1", t1);
            m.crear_tabla("db2", "t2", t2);
            m.crear_tabla("db2", "t3", t3);

            m.crear_tabla("db3", "t1", t1);
            m.crear_tabla("db3", "t2", t2);
            m.crear_tabla("db3", "t3", t3);

            db1.Usuarios = urs;
            db2.Usuarios = urs;
            db3.Usuarios = urs;

            m.generar_xml();
            */

            DataTable dt1 = new DataTable("t1");
            DataTable dt2 = new DataTable("t2");

            DataColumn dc11 = new DataColumn("d1", System.Type.GetType("System.Int32")); //pk
            DataColumn dc21 = new DataColumn("d2", System.Type.GetType("System.String"));//null
            DataColumn dc31 = new DataColumn("d3", System.Type.GetType("System.Int32")); //no null
            DataColumn dc41 = new DataColumn("d4", System.Type.GetType("System.Int32")); //autoincrementable 
            DataColumn dc51 = new DataColumn("d5", System.Type.GetType("System.Int32")); //foreign key
            DataColumn dc61 = new DataColumn("d6", System.Type.GetType("System.String")); //unique key

            DataColumn dc12 = new DataColumn("d7", System.Type.GetType("System.Int32")); //pk
            DataColumn dc22 = new DataColumn("d8", System.Type.GetType("System.String"));//null
            DataColumn dc32 = new DataColumn("d9", System.Type.GetType("System.Int32")); //no null
            DataColumn dc42 = new DataColumn("d10", System.Type.GetType("System.Int32")); //autoincrementable
            DataColumn dc52 = new DataColumn("d11", System.Type.GetType("System.String")); //unique key
            #region rr
            dt1.Columns.Add(dc11); dt1.Columns.Add(dc21); dt1.Columns.Add(dc31); dt1.Columns.Add(dc41);
            dt1.Columns.Add(dc51); dt1.Columns.Add(dc61);
            DataColumn[] pkt1 = { dc11 };
            //ahora los constraints
            dt1.PrimaryKey = pkt1;
            dc21.AllowDBNull = true;
            dc31.AllowDBNull = false;
            dc41.AutoIncrement = true;
            dc41.AutoIncrementSeed = 1;
            dc41.AutoIncrementStep = 1;
            dc51.AllowDBNull = false;
            dc61.Unique = true;

            dt2.Columns.Add(dc12); dt2.Columns.Add(dc22); dt2.Columns.Add(dc32);
            dt2.Columns.Add(dc42); dt2.Columns.Add(dc52);
            DataColumn[] pkt2 = { dc12 };
            //ahora los constraints
            dt2.PrimaryKey = pkt2;
            dc22.AllowDBNull = true;
            dc32.AllowDBNull = false;
            dc42.AutoIncrement = true;
            dc42.AutoIncrementSeed = 1;
            dc42.AutoIncrementStep = 1;
            dc52.Unique = true;
            #endregion
            ForeignKeyConstraint fkc = new ForeignKeyConstraint("t2fk", dc12, dc51);
            dt1.Constraints.Add(fkc);

            DataRow dr = dt1.NewRow();
            dr["d1"] = 1;
            //d2 null
            dr["d3"] = 1;
            //d4 auto
            dr["d5"] = 1;
            dr["d6"] = "";
            dt1.Rows.Add(dr);

            DataRow dr1 = dt1.NewRow();
            dr1["d1"] = 2;
            //d2 null
            dr1["d3"] = 2;
            //d4 auto
            dr1["d5"] = 1;
            dr1["d6"] = 2.ToString();
            dt1.Rows.Add(dr1);

            DataRow dr2 = dt2.NewRow();
            dr2["d7"] = 1;
            //d8 null
            dr2["d9"] = 1;
            //d10 auto
            //dr2["d11"] = "hola r1";
            dt2.Rows.Add(dr2);

            //DataRow[] con = dt1.Select("d1 where d1 = 1");
            
            var query = from dtt1 in dt1.AsEnumerable()
                         from dtt2 in dt2.AsEnumerable()
                         select new { dt1, dt2 };


            var combinedRows = from a in dt1.AsEnumerable()
                               from b in dt2.AsEnumerable()
                               select new {
                                   a,
                                   b
                               };
                               
            var dt3 = new DataTable();

            for(int i = 0; i < dt1.Columns.Count; i++)
            {
                dt3.Columns.Add(new DataColumn(dt1.Columns[i].ColumnName));
            }

            for (int i = 0; i < dt2.Columns.Count; i++)
            {
                dt3.Columns.Add(new DataColumn(dt2.Columns[i].ColumnName));
            }
            
            foreach(DataRow rr1 in dt1.Rows)
            {
                foreach(DataRow rr2 in dt2.Rows)
                {
                    DataRow nuevo = dt3.NewRow();

                    foreach (DataColumn dc in dt1.Columns)
                    {
                        nuevo[dc.ColumnName] = rr1[dc];
                    }

                    foreach (DataColumn dc in dt2.Columns)
                    {
                        nuevo[dc.ColumnName] = rr2[dc];
                    }

                    dt3.Rows.Add(nuevo);
                }
            }

            DataSet dsnuevo = new DataSet("db");
            dsnuevo.Tables.Add(dt1);
            dsnuevo.Tables.Add(dt2);
            

            //dt1.Merge(dt2, false, MissingSchemaAction.Add);

            String date = "10-10-2018";
            DateTime dttttt = DateTime.Parse(date);
            DataColumn dcc = new DataColumn("b", System.Type.GetType("System.DateTime"));
            
            
            //Constante.sistema_archivo = (Maestro)Constante.sistema_archivo.cargar();

            //    string msg1 = "";
            //    for (int i = 0; i < xSintactico.errores.Count; i++)
            //    {
            //        msg1 += "Descripcion: " + xSintactico.errores[i].Descripcion + " " + xSintactico.errores[i].Line.ToString() + ":" + xSintactico.errores[i].Colm + "\n";
            //    }
            //    richTextBox2.Text = msg1;

            //ParseTreeNode root = analizar(Constante.leer_archivo("C://DBMS//DBS//db1.usac"));
            //if(root == null)
            //{
            //    Constante.rtb_consola.Text = "error";
            //}
            //else
            //{
            //    //List<List<Atributo>> lm = RowAst.ROWS(root);
            //    List<object> lm = DbAst.LISTA(root);
            //}
        }


        public static ParseTreeNode analizar(string entrada)
        {
            DbGrammar grammar = new DbGrammar();
            Parser parser = new Parser(grammar);
            ParseTree arbol = parser.Parse(entrada);

            //para los errores buscarlos en arbol
            ParseTreeNode root = arbol.Root;
            return root;
        }

        private void ServidorDb_Load(object sender, EventArgs e)
        {
            
        }
    }
}
