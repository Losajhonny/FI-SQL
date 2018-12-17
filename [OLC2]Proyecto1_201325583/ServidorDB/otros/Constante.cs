using ServidorDB.arboles.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServidorDB.otros
{
    public class Constante
    {
        public static bool usuando_db_actual = false;
        public static string usuario_actual = "admin";
        public static string db_actual = "";

        public static string usuario_admin = "admin";
        public static string password_admin = "admin";

        public static RichTextBox rtb_consola = new System.Windows.Forms.RichTextBox();

        public const int ASC = 0;
        public const int DESC = 1;

        public const int OTORGAR = 0;
        public const int DENEGAR = 1;


        public const int tBASE_DATOS = 0;
        public const int tTABLA = 1;
        public const int tOBJETO = 2;
        public const int tPROCEDIMIENTO = 3;
        public const int tFUNCION = 4;
        public const int tUSUARIO = 5;
        public const int tUSAR = 6;
        public const int tAGREGAR = 7;
        public const int tQUITAR = 8;
        public const int tCAMBIAR = 9;

        public static string LEXICO = "Lexico";
        public static string SINTACTICO = "Sintactico";
        public static string SEMANTICO = "Semantico";
        public static string LOGICO = "Logico";
        public static string PARSER = "Analizador";

        public static string[] TIPOS = { "bool", "integer", "double", "date", "datetime", "text", "error", "id", "void" };
        public const int BOOL = 0;
        public const int INTEGER = 1;
        public const int DOUBLE = 2;
        public const int DATE = 3;
        public const int DATETIME = 4;
        public const int TEXT = 5;
        public const int ERROR = 6;
        public const int ID = 7; //Constante para identificar objetos
        public const int VOID = 8;
        public const int NONE = 9;

        public const string SI = "Si";
        public const string SINO = "Sino";
        public const string SELECCIONA = "Selecciona";
        public const string PARA = "Para";
        public const string MIENTRAS = "Mientras";
        public const string PROCEDIMIENTO = "Procedimiento";
        public const string FUNCION = "Funcion";
        public const string TABLA = "Tabla";
        public const string OBJETO = "Objeto";

        public const string BASE_DATOS = "Base_Datos";
        public const string USUARIO = "Usuario";

        //SISTEMA DE ARCHIVOS
        public const string RUTA_MAESTRO = "C:\\DBMS\\";
        public const string RUTA_DB = "C:\\DBMS\\DBS\\";
        public const string RUTA_TABLA = "C:\\DBMS\\TABLAS\\";
        public const string RUTA_PROCEDIMIENTOS = "C:\\DBMS\\PROCEDIMIENTOS\\";
        public const string RUTA_FUNCIONES = "C:\\DBMS\\FUNCIONES\\";
        public const string RUTA_OBJETO = "C:\\DBMS\\OBJETOS\\";
        public const string EXTENSION = "usac";

        public static Maestro sistema_archivo = new Maestro();

        public static void crear_archivo(string path, string cadena)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                StreamWriter sw = new StreamWriter(path);
                sw.Write(cadena);
                sw.Close();
            }
            catch (Exception ex) { }
        }

        public static bool existe_archivo(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    return true;
                }
            }catch(Exception ex) { }
            return false;
        }

        public static string leer_archivo(string path)
        {
            string cadena = "";
            try
            {
                string line = null;
                StreamReader sr = new StreamReader(path);
                while((line = sr.ReadLine()) != null)
                {
                    cadena += line + "\n";
                }
                sr.Close();
            }catch(Exception ex) { }
            return cadena;
        }

        public static string getTipo(int tipo)
        {
            switch (tipo)
            {
                case BOOL:
                    return "bool";
                case INTEGER:
                    return "integer";
                case DOUBLE:
                    return "double";
                case DATE:
                    return "date";
                case DATETIME:
                    return "datetime";
                case TEXT:
                    return "text";
                default:
                    return "";
            }
        }

        public static int[,] suma = 
        {
            //  BOOL          INTEGER       DOUBLE        DATE       DATETIME    TEXT        ERROR  ID(OBJETO)
            { BOOL,    INTEGER, DOUBLE, ERROR, ERROR, TEXT,  ERROR, ERROR }, //BOOL
            { INTEGER, INTEGER, DOUBLE, ERROR, ERROR, TEXT,  ERROR, ERROR }, //INTEGER
            { DOUBLE,  DOUBLE,  DOUBLE, ERROR, ERROR, TEXT,  ERROR, ERROR }, //DOUBLE
            { ERROR,   ERROR,   ERROR,  ERROR, ERROR, TEXT,  ERROR, ERROR }, //DATE
            { ERROR,   ERROR,   ERROR,  ERROR, ERROR, TEXT,  ERROR, ERROR }, //DATETIME
            { TEXT,    TEXT,    TEXT,   ERROR, ERROR, TEXT,  ERROR, ERROR }, //TEXT
            { ERROR,   ERROR,   ERROR,  ERROR, ERROR, ERROR, ERROR, ERROR }, //ERROR
            { ERROR,   ERROR,   ERROR,  ERROR, ERROR, ERROR, ERROR, ERROR }  //ID
        };

        public static int[,] resta =
        {
            //  BOOL          INTEGER       DOUBLE        DATE       DATETIME    TEXT        ERROR  ID(OBJETO)
            { ERROR,    INTEGER, DOUBLE, ERROR, ERROR, ERROR,  ERROR, ERROR }, //BOOL
            { INTEGER,  INTEGER, DOUBLE, ERROR, ERROR, ERROR,  ERROR, ERROR }, //INTEGER
            { DOUBLE,   DOUBLE,  DOUBLE, ERROR, ERROR, ERROR,  ERROR, ERROR }, //DOUBLE
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, ERROR,  ERROR, ERROR }, //DATE
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, ERROR,  ERROR, ERROR }, //DATETIME
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, ERROR,  ERROR, ERROR }, //TEXT
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, ERROR,  ERROR, ERROR }, //ERROR
            { ERROR,   ERROR,   ERROR,  ERROR, ERROR, ERROR, ERROR, ERROR }  //ID
        };

        public static int[,] multiplicacion =
        {
            //  BOOL          INTEGER       DOUBLE        DATE       DATETIME    TEXT        ERROR ID(OBJETO)
            { BOOL,     INTEGER, DOUBLE, ERROR, ERROR, ERROR,  ERROR, ERROR }, //BOOL
            { INTEGER,  INTEGER, DOUBLE, ERROR, ERROR, ERROR,  ERROR, ERROR }, //INTEGER
            { DOUBLE,   DOUBLE,  DOUBLE, ERROR, ERROR, ERROR,  ERROR, ERROR }, //DOUBLE
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, TEXT,   ERROR, ERROR }, //DATE
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, TEXT,   ERROR, ERROR }, //DATETIME
            { ERROR,    ERROR,   ERROR,  TEXT,  TEXT,  ERROR,  ERROR, ERROR }, //TEXT
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, ERROR,  ERROR, ERROR }, //ERROR
            { ERROR,   ERROR,   ERROR,  ERROR, ERROR, ERROR, ERROR, ERROR }  //ID
        };

        public static int[,] division =
        {
            //  BOOL          INTEGER       DOUBLE        DATE       DATETIME    TEXT        ERROR  ID(OBJETO)
            { ERROR,    INTEGER, DOUBLE, ERROR, ERROR, ERROR,  ERROR, ERROR }, //BOOL
            { INTEGER,  INTEGER, DOUBLE, ERROR, ERROR, ERROR,  ERROR, ERROR }, //INTEGER
            { DOUBLE,   DOUBLE,  DOUBLE, ERROR, ERROR, ERROR,  ERROR, ERROR }, //DOUBLE
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, TEXT,   ERROR, ERROR }, //DATE
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, TEXT,   ERROR, ERROR }, //DATETIME
            { ERROR,    ERROR,   ERROR,  TEXT,  TEXT,  ERROR,  ERROR, ERROR }, //TEXT
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, ERROR,  ERROR, ERROR }, //ERROR
            { ERROR,   ERROR,   ERROR,  ERROR, ERROR, ERROR, ERROR, ERROR }  //ID
        };

        public static int[,] potencia =
        {
            //BOOL     INTEGER  DOUBLE   DATE  DATETIME TEXT   ERROR  ID(OBJETO)
            { ERROR,    INTEGER, DOUBLE, ERROR, ERROR, ERROR,  ERROR, ERROR }, //BOOL
            { INTEGER,  INTEGER, DOUBLE, ERROR, ERROR, ERROR,  ERROR, ERROR }, //INTEGER
            { DOUBLE,   DOUBLE,  DOUBLE, ERROR, ERROR, ERROR,  ERROR, ERROR }, //DOUBLE
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, TEXT,   ERROR, ERROR }, //DATE
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, TEXT,   ERROR, ERROR }, //DATETIME
            { ERROR,    ERROR,   ERROR,  TEXT,  TEXT,  ERROR,  ERROR, ERROR }, //TEXT
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, ERROR,  ERROR, ERROR }, //ERROR
            { ERROR,   ERROR,   ERROR,  ERROR, ERROR, ERROR, ERROR, ERROR }  //ID
        };
    }
}
