using Irony.Parsing;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace ServidorDB
{
    public partial class ServidorDb : Form
    {
        public const int CONCATENAR = 0;
        public const int REEMPLAZAR = 1;
        public const int MAX_VALUE = 8192;
        public const int DELAY = 50;
        public bool conectado = true;

        public string paquete = ""; //me indica en que paquete estoy
        public string validar = ""; //me indica el numero de transaccion que estoy realizando
        public string instruccion = "";
        public string usuario = "";
        public string password = "";

        public ServidorDb()
        {
            //Tomar en cuenta para general servidorDb
            //necesito crear el usuario de administrador
            Usuario usr = new Usuario(Constante.usuario_admin, Constante.password_admin);
            PeticionDDL.crearUsuario(usr);
            //Finalizando la insercion de crear usuario
            //si existe no notificar que existe
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            Constante.rtb_consola = this.consola;


            //string prueba = "@ident = 10+2*2^2+2-1+1*2^2^2*1;\n";
            //prueba += "@ident = 1010101\n";
            //prueba += "@ident = 1010101;\n";
            //prueba += "@ident = 1010101;\n";
            //prueba += "@ident = 1010101;\n";
            //uSintactico.analizar(prueba);

            Iniciar_Servidor();
        }

        public void Conexion()
        {
            //Iniciando servidor
            Socket servidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Conexion mediante ipadrees y tcp usando socketes
            IPEndPoint LocalEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6400);
            //asocia el ipendpoint al servidor
            servidor.Bind(LocalEndPoint);
            //limite de espera de un servidor
            //en este caso le dejaremos con 5 maximo de informacion por paquete
            servidor.Listen(10);

            DateTime fechahora = DateTime.Now;
            string tiempo = Convert.ToString(fechahora);
            setTextConsola(">> " + tiempo + " admin [Servidor DB Conexion inicial]\n", CONCATENAR);

            while (conectado)
            {   //como siempre va a estar conextado a menos que se cierre la aplicacion
                 fechahora = DateTime.Now;
                 tiempo = Convert.ToString(fechahora);

                setTextConsola(">> " + tiempo + " admin [Esperando conexion...]\n", CONCATENAR);
                //en modo espera
                Socket handler = servidor.Accept();
                //conexion acpetada


                string dato_recivido = "";
                while (conectado)
                {
                    byte[] info_recivido = new byte[MAX_VALUE];//Despues ver la capacidad de informacion enviada
                                                          //cadena data para capturar los bytes recividos
                    dato_recivido = "";

                    int bytesRec = handler.Receive(info_recivido);   //aqui siempre me va a estar escuchando hasta que le llegue el paquete fin
                    dato_recivido += Encoding.ASCII.GetString(info_recivido, 0, bytesRec);    //codificando los bytes a una cadena de caracteres
                    
                    fechahora = DateTime.Now;
                    tiempo = Convert.ToString(fechahora);

                    //siempre me va a venir valores en pares ordenados
                    //entonces debo delimitarlos con ':'
                    char[] delimit = { '~' };

                    string[] pares = dato_recivido.Split(delimit);
                    //primera posicion es la instruccion o paquete , etc
                    //segunda posicion es el valor

                    if (pares[0].ToLower().Equals("validar"))
                    {
                        validar = pares[1];
                    }
                    else if (pares[0].ToLower().Equals("paquete"))
                    {
                        if (pares[1].ToLower().Equals("fin"))
                        {
                            break;
                        }
                        else { paquete = pares[1]; }

                    }
                    else if (pares[0].ToLower().Equals("usuario"))
                    {
                        usuario = pares[1];
                    }
                    else if (pares[0].ToLower().Equals("password"))
                    {
                        password = pares[1];
                    }
                    else if (pares[0].ToLower().Equals("instruccion"))
                    {
                        instruccion = pares[1];
                    }
                    //solo estos subpaquetes se enviaran en el proceso media vez termine con "fin"
                    //entonces salir del bucle y realizar la transaccion
                    //setTextConsola("Text received : " + dato_recivido + "\n", CONCATENAR);
                    Thread.Sleep(DELAY);
                }

                //aqui se realiza el proceso de la transaccion
                //setTextConsola("Text received : " + dato_recivido, CONCATENAR);
                //por el momento solo quiero recibir una transaccion asi que aqui le dejo conectado = false

                realizandoPeticiones(handler);
                

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }

            servidor.Close();
        }

        public void realizandoPeticiones(Socket handler)
        {
            if (paquete.ToLower().Equals("login"))
            {
                peticionLogin(handler);
            }
            else if (paquete.ToLower().Equals("usql"))
            {
                peticionSql(handler);
            }
        }

        public void peticionLogin(Socket handler)
        {
            //Paquete en entro debo enviar el paquete de salida
            /*
             * [
                “validar”: 1500,
                “login”: [
                    "username":"nombre",
                    "password":"pass"
                    ]
                ]
             */

            Usuario usr = new Usuario(usuario, password);
            String fechahora = Convert.ToString(DateTime.Now);
            Constante.rtb_consola.Text += ">> " + fechahora + " admin [Autenticacion por parte del servidor][usuario = '" + usuario + "' password = '" + password + "']\n";


            string respuesta = "[ \"validar\" : " + validar + "," + PeticionDDL.loguear(usr).ToString() + " ]";
            Constante.rtb_consola.Text += "Enviando Paquete:login\n" +
                    respuesta + "\n";
            //envio la respuesta
            byte[] msg = new byte[MAX_VALUE];
            msg = Encoding.ASCII.GetBytes(respuesta);
            handler.Send(msg);
            //intervalor de tiempo
            Thread.Sleep(DELAY);


            /*[
                “validar”: 1500,
                “login”: [
                    "username":"nombre",
                    "login":true        <- o false
                    ]
                ]
             */
        }

        public void peticionSql(Socket handler)
        {
            byte[] msg = new byte[MAX_VALUE];
            string respuesta = Constante.NOTHING;
            /*Aqui ya tengo las instrucciones a ejecutar por parte
             de servidor web*/
            uSintactico.analizar_usql(instruccion);

            /*Debo de enviar varios datos de informacion
             1. Lista de Errores
             2. Informacion ultimo select que se realizo
             3. Log que se realizo
             4. Consola de sql */

            /*Enviando lista de errores--recorrer lista de uSintactico y xSintactico
             */
            respuesta = Constante.NOTHING;
            string error = "";
            for(int i = 0; i < uSintactico.uerrores.Count; i++)
            {
                error += "[" +
                    "\"Tipo Error\" : \"" + uSintactico.uerrores[i].Tipo + "\" , " +
                    "\"Descripcion\" : \"" + uSintactico.uerrores[i].Descripcion + "\" ," +
                    
                    "\"fila\" : \"" + uSintactico.uerrores[i].Line.ToString() + "\" ," +
                    "\"col\" : \"" + uSintactico.uerrores[i].Colm.ToString() + "\" ," +
                    "\"lexema\" : \"" + uSintactico.uerrores[i].Lexema + " ." + "\" ," +
                    "\"lenguaje\" : \"Lenguaje USQL\"" +
                    "]";
                if (i != uSintactico.uerrores.Count - 1) { error += ","; }
            }
            for (int i = 0; i < xSintactico.errores.Count; i++)
            {
                error += "[" +
                   "\"Tipo Error\" : \"" + xSintactico.errores[i].Tipo + "\" , " +
                   "\"Descripcion\" : \"" + xSintactico.errores[i].Descripcion + "\" ," +
                   
                   "\"fila\" : \"" + xSintactico.errores[i].Line.ToString() + "\" ," +
                   "\"col\" : \"" + xSintactico.errores[i].Colm.ToString() + "\" ," +
                   "\"lexema\" : \"" + xSintactico.errores[i].Lexema + " ." + "\" ," +
                   "\"lenguaje\" : \"Lenguaje XML\"" +
                   "]";
                if (i != xSintactico.errores.Count - 1) { error += ","; }
            }
            if (!error.Equals(""))
            {
                error = "[" +
                    "\"validar\" : " + validar + " ," +
                    "\"paquete\" : \"error\" ," +
                    "\"tipo\" : \"tipotexto\" , " +
                    "\"msg\" : \"msgtexto\" , " +
                    "\"datos\" : " + error +
                    "]";
                respuesta = error;
                Constante.rtb_consola.Text += "Enviando Paquete:error\n" +
                    respuesta + "\n";
            }
            msg = new byte[MAX_VALUE];
            msg = Encoding.ASCII.GetBytes(respuesta);
            handler.Send(msg);              // -------> ENVIANDO ARCHIVO DE ERRORES
            Thread.Sleep(DELAY);

            /*Enviando informacion del select
            */
            respuesta = Constante.NOTHING;
            if (!Constante.informacion_select.Equals(""))
            {
                respuesta = "[ \"validar\" : " + validar + "," + Constante.informacion_select + " ]";
                Constante.rtb_consola.Text += "Enviando Paquete:usql\n" +
                    respuesta + "\n";
            }
            msg = new byte[MAX_VALUE];
            msg = Encoding.ASCII.GetBytes(respuesta);
            handler.Send(msg);              // -------> ENVIANDO INFORMACION SELECT
            Thread.Sleep(DELAY);


            /*Enviando mensaje de lo que se ha realizado
             */
            respuesta = Constante.NOTHING;
            if (!Constante.mensaje.Equals(""))
            {
                respuesta = "[ \"validar\" : " + validar + "," + " \"paquete\" : \"usql\" , \"datos\" : [" + "~" + Constante.mensaje + "~" + "]" + " ]";
                Constante.rtb_consola.Text += "Enviando Paquete:usql\n" +
                    respuesta + "\n";
            }
            msg = new byte[MAX_VALUE];
            msg = Encoding.ASCII.GetBytes(respuesta);
            handler.Send(msg);              // --------> ENVIANDO LOG DE QUE SE REALIZO
            Thread.Sleep(DELAY);

            /*Enviando mensaje de lo que se ha realizado
             */
            respuesta = Constante.NOTHING;
            if (!Constante.informacion_consola.Equals(""))
            {
                respuesta = "[ \"validar\" : " + validar + "," + " \"paquete\" : \"usql\" , \"datos\" : [" + "~" + Constante.informacion_consola + "~" + "]" + " ]";
                Constante.rtb_consola.Text += "Enviando Paquete:usql\n" +
                    respuesta + "\n";
            }
            msg = new byte[MAX_VALUE];
            msg = Encoding.ASCII.GetBytes(respuesta);
            handler.Send(msg);              // --------> ENVIANDO Consola DE QUE SE REALIZO
            Thread.Sleep(DELAY);
        }

        delegate void StringChangeText(string text, int tipo);

        private void setTextConsola(string text, int tipo)
        {
            if (Constante.rtb_consola.InvokeRequired)
            {
                StringChangeText d = new StringChangeText(setTextConsola);
                try
                {
                    this.Invoke(d, new object[] { text, tipo });
                }
                catch(Exception ex) { conectado = false; }
                
            }
            else if (tipo == REEMPLAZAR)
            {
                Constante.rtb_consola.Text = text;
            }
            else
            {
                Constante.rtb_consola.Text += text;
            }
        }

        public void Iniciar_Servidor()
        {
            Thread t = new Thread(new ThreadStart(Conexion));
            t.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
        
        private void ServidorDb_FormClosed(object sender, FormClosedEventArgs e)
        {
            conectado = false;
            Application.Exit();
        }

        private void ServidorDb_FormClosing(object sender, FormClosingEventArgs e)
        {
            conectado = false;
            Application.ExitThread();
        }

        private void ServidorDb_Load(object sender, EventArgs e)
        {

        }
    }
}
