using ServidorDB.analizadores.usql;
using ServidorDB.otros;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.xml
{
    public class Tabla : xInstruccion
    {
        private string nombre;
        private string ruta;
        private List<Atributo> atributos;
        private List<string> usuarios = new List<string>();

        private int line;
        private int colm;

        private DataTable registros;

        private List<string[]> regs;
        private int incremento;

        //en caso de un select join va a ver multiples llaves primarias
        //private List<string> llaves_primarias;

        public Tabla(string nombre)
        {
            this.incremento = 0;
            this.nombre = nombre;
            this.ruta = Constante.RUTA_TABLA + nombre + "." + Constante.EXTENSION;
            this.atributos = new List<Atributo>();
            this.usuarios = new List<string>();
            this.crearDataTable();
        }

        public void registrar_prueba(string[] registro)
        {
            DataRow dr = registros.NewRow();
            if (registro.Length == atributos.Count)
            {
                for (int i = 0; i < atributos.Count; i++)
                {
                    dr[atributos[i].Nombre] = registro[i];
                }
                registros.Rows.Add(dr);
            }
            //else no registra nada
        }

        public void Especificar_constraints(DataColumn dc, Atributo atributo)
        {
            if(atributo.Complemento != null)
            {
                if (atributo.Complemento.ToLower().Equals("nonulo"))
                {
                    dc.AllowDBNull = false;
                }
                else if (atributo.Complemento.ToLower().Equals("nulo"))
                {
                    dc.AllowDBNull = true;
                }
                else if (atributo.Complemento.ToLower().Equals("autoincrementable"))
                {
                    dc.AutoIncrement = true;
                    dc.AutoIncrementSeed = 1;
                    dc.AutoIncrementStep = 1;
                }
                else if (atributo.Complemento.ToLower().Equals("unico"))
                {
                    dc.AllowDBNull = false;
                    UniqueConstraint uc1 = new UniqueConstraint(dc);
                    registros.Constraints.Add(uc1);
                }
                else
                {
                    DataColumn[] pk = { dc };
                    registros.PrimaryKey = pk;
                }
            }
        }

        public void crearDataTable()
        {
            this.registros = new DataTable(nombre);

            //colocar las columna que se necesita para los registros
            //dependiendo de la lista de atributos se crearan
            for (int i = 0; i < this.atributos.Count; i++)
            {
                //inserto columna por atributo
                //para simular la tabla con sus registros

                DataColumn dc = new DataColumn(this.atributos[i].Nombre);

                /*Colocar los unicos, null, nonull etc*/
                
                this.registros.Columns.Add(dc);
                Especificar_constraints(dc, this.atributos[i]);

                /*realizando pruebas para insertar
                 definiendo el tipo de dato que soporta la columna*/
                if (atributos[i].Tipo == Constante.INTEGER)
                {
                    dc.DataType = System.Type.GetType("System.Int32");
                }
                else if (atributos[i].Tipo == Constante.TEXT)
                {
                    dc.DataType = System.Type.GetType("System.String");
                }
                else if (atributos[i].Tipo == Constante.BOOL)
                {
                    dc.DataType = System.Type.GetType("System.Boolean");
                }
                else if (atributos[i].Tipo == Constante.DOUBLE)
                {
                    dc.DataType = System.Type.GetType("System.Double");
                }
                else if (atributos[i].Tipo == Constante.DATE)
                {
                    dc.DataType = System.Type.GetType("System.DateTime");
                }
                else if (atributos[i].Tipo == Constante.DATETIME)
                {
                    dc.DataType = System.Type.GetType("System.DateTime");
                }
                /*Aqui termina el codigo de prueba*/
            }
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public List<Atributo> Atributos { get => atributos; set => atributos = value; }
        public string Ruta { get => ruta; set => ruta = value; }
        public List<string> Usuarios { get => usuarios; set => usuarios = value; }
        public DataTable Registros { get => registros; set => registros = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }

        public string generar_xml()
        {
            string cadena = "";
            //Recorriendo los registros de la tabla
            for (int i = 0; i < registros.Rows.Count; i++)
            {
                cadena += "<row>\n";
                for (int j = 0; j < atributos.Count; j++)
                {
                    cadena += "\t<" + atributos[j].Nombre + ">";
                    cadena += "~" + registros.Rows[i][atributos[j].Nombre].ToString() + "~" ;
                    cadena += "</" + atributos[j].Nombre + ">\n";
                }
                cadena += "</row>\n";
            }
            Constante.crear_archivo(ruta, cadena);
            return null;
        }

        public object cargar()
        {
            //la tabla ya tiene cargado
            //la ruta de los registros y atributos
            //y usuarios ademas de tener creada el datatable
            //tambien tiene configurado sus atributos

            //verifico si la ruta existe para los registros
            if (Constante.existe_archivo(ruta))
            {
                //obtengo la cadena a analizar
                string cadena = Constante.leer_archivo(ruta);
                //realizar el analisis y reconocer el ast
                List<List<Atributo>> ob = xSintactico.analizarRegistros(cadena);
                //verificar si el arbol no es nulo
                if(ob != null)
                {
                    //analizando registros
                    foreach (List<Atributo> atrs in ob)
                    {
                        //este es un solo registro donde tiene todas las columna de la tabla
                        //verificar si la cantidad de la lista es igual a la de la
                        //lista de atributos de la tabla
                        if(atrs.Count == atributos.Count)
                        {
                            //me imagino que ya tiene especificado el tipo de dato en la columna
                            DataRow nr = registros.NewRow();
                            bool hayError = false;
                            for (int i = 0; i < atributos.Count; i++)
                            {
                                //devuelve un error si los tipos no son iguales
                                object retorno = atrs[i].cargar();
                                //carga si es igual el tipo
                                //en este caso el tipo realiza la funcion de nombre de la variable

                                if (retorno != null)
                                {
                                    xSintactico.errores.Add(new analizadores.usql.uError(Constante.SEMANTICO,
                                        xSintactico.ERRORES_XML[xSintactico.ERROR_REG] + " : " + retorno.ToString(), "", atrs[i].Line,
                                        atrs[i].Colm));
                                    hayError = true;
                                    break;
                                }
                                /*NO VA PORQUE SI ES ID EN ESTE CASO ES EL NOMBRE DE LA VARIABLE*/
                                //else if (atrs[i].Tipo == Constante.ID)
                                //{

                                //    string msg = "El atributo en la tabla " + nombre + " debe ser de tipo primitivo";
                                //    xSintactico.errores.Add(new analizadores.usql.uError(Constante.SEMANTICO,
                                //        xSintactico.ERRORES_XML[xSintactico.ERROR_REG] + " : " + msg, "", atrs[i].Line,
                                //        atrs[i].Colm));
                                //    hayError = true;
                                //    break;
                                //}

                                try
                                {
                                    if (atributos[i].Tipo == Constante.INTEGER)
                                    {
                                        int v = Int32.Parse(atrs[i].Nombre);
                                        nr[atributos[i].Nombre] = v;
                                    }
                                    else if (atributos[i].Tipo == Constante.TEXT)
                                    {
                                        nr[atributos[i].Nombre] = atrs[i].Nombre;
                                    }
                                    else if (atributos[i].Tipo == Constante.BOOL)
                                    {
                                        nr[atributos[i].Nombre] = Boolean.Parse(atrs[i].Nombre);
                                    }
                                    else if (atributos[i].Tipo == Constante.DOUBLE)
                                    {
                                        nr[atributos[i].Nombre] = Double.Parse(atrs[i].Nombre, System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    else if (atributos[i].Tipo == Constante.DATE)
                                    {
                                        DateTime dtt = DateTime.Parse(atrs[i].Nombre);
                                        nr[atributos[i].Nombre] = dtt;
                                    }
                                    else if (atributos[i].Tipo == Constante.DATETIME)
                                    {
                                        DateTime dtt = DateTime.Parse(atrs[i].Nombre);
                                        nr[atributos[i].Nombre] = dtt;
                                    }
                                }catch(Exception ex)
                                {

                                }
                            }
                            if (!hayError)
                            {
                                try
                                {
                                    registros.Rows.Add(nr);
                                }
                                catch (Exception ex)
                                {
                                    string msg = ex.Message;
                                    uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, "", line, colm));
                                }

                                
                            }
                        }
                        else
                        {
                            string msg = "El tamaño de las columnas del registro no " +
                                "coincide con el tamaño de las columnas";
                            xSintactico.errores.Add(new analizadores.usql.uError(Constante.LOGICO,
                                xSintactico.ERRORES_XML[xSintactico.ERROR_REG] + " : " + msg, "", 0, 0));
                        }
                    }
                }
            }
            return null;
        }
        
    }
}
