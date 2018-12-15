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
        private List<string> usuarios;

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
                this.registros.Columns.Add(dc);
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
                    cadena += "\t<" + Constante.TIPOS[atributos[j].Tipo] + ">";
                    cadena += "~" + registros.Rows[i][atributos[j].Nombre].ToString() + "~" ;
                    cadena += "</" + Constante.TIPOS[atributos[j].Tipo] + ">\n";
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
                            DataRow nr = registros.NewRow();
                            bool hayError = false;
                            for (int i = 0; i < atributos.Count; i++)
                            {
                                //devuelve un error si los tipos no son iguales
                                object retorno = atrs[i].cargar();

                                if (retorno != null)
                                {
                                    xSintactico.errores.Add(new analizadores.usql.uError(Constante.SEMANTICO,
                                        xSintactico.ERRORES_XML[xSintactico.ERROR_REG] + " : " + retorno.ToString(), "", atrs[i].Line,
                                        atrs[i].Colm));
                                    hayError = true;
                                    break;
                                }
                                else if (atrs[i].Tipo == Constante.ID)
                                {

                                    string msg = "El atributo en la tabla " + nombre + " debe ser de tipo primitivo";
                                    xSintactico.errores.Add(new analizadores.usql.uError(Constante.SEMANTICO,
                                        xSintactico.ERRORES_XML[xSintactico.ERROR_REG] + " : " + msg, "", atrs[i].Line,
                                        atrs[i].Colm));
                                    hayError = true;
                                    break;
                                }
                                nr[atributos[i].Nombre] = atrs[i].Nombre;
                            }
                            if (!hayError)
                            {
                                registros.Rows.Add(nr);
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
