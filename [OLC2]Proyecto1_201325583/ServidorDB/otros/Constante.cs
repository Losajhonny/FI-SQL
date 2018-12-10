using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServidorDB.otros
{
    public class Constante
    {
        public static RichTextBox rtb_consola = new System.Windows.Forms.RichTextBox();
        public static string LEXICO = "Lexico";
        public static string SINTACTICO = "Sintactico";
        public static string SEMANTICO = "Semantico";

        public const int BOOL = 0;
        public const int INTEGER = 1;
        public const int DOUBLE = 2;
        public const int DATE = 3;
        public const int DATETIME = 4;
        public const int TEXT = 5;
        public const int ERROR = 6;
        public const int NONE = 7;

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
            //  BOOL          INTEGER       DOUBLE        DATE       DATETIME    TEXT        ERROR
            { BOOL,    INTEGER, DOUBLE, ERROR, ERROR, TEXT,  ERROR }, //BOOL
            { INTEGER, INTEGER, DOUBLE, ERROR, ERROR, TEXT,  ERROR }, //INTEGER
            { DOUBLE,  DOUBLE,  DOUBLE, ERROR, ERROR, TEXT,  ERROR }, //DOUBLE
            { ERROR,   ERROR,   ERROR,  ERROR, ERROR, TEXT,  ERROR }, //DATE
            { ERROR,   ERROR,   ERROR,  ERROR, ERROR, TEXT,  ERROR }, //DATETIME
            { TEXT,    TEXT,    TEXT,   ERROR, ERROR, TEXT,  ERROR }, //TEXT
            { ERROR,   ERROR,   ERROR,  ERROR, ERROR, ERROR, ERROR } //ERROR
        };

        public static int[,] resta =
        {
            //  BOOL          INTEGER       DOUBLE        DATE       DATETIME    TEXT        ERROR
            { ERROR,    INTEGER, DOUBLE, ERROR, ERROR, ERROR,  ERROR }, //BOOL
            { INTEGER,  INTEGER, DOUBLE, ERROR, ERROR, ERROR,  ERROR }, //INTEGER
            { DOUBLE,   DOUBLE,  DOUBLE, ERROR, ERROR, ERROR,  ERROR }, //DOUBLE
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, ERROR,  ERROR }, //DATE
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, ERROR,  ERROR }, //DATETIME
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, ERROR,  ERROR }, //TEXT
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, ERROR,  ERROR } //ERROR
        };

        public static int[,] multiplicacion =
        {
            //  BOOL          INTEGER       DOUBLE        DATE       DATETIME    TEXT        ERROR
            { BOOL,     INTEGER, DOUBLE, ERROR, ERROR, ERROR,  ERROR }, //BOOL
            { INTEGER,  INTEGER, DOUBLE, ERROR, ERROR, ERROR,  ERROR }, //INTEGER
            { DOUBLE,   DOUBLE,  DOUBLE, ERROR, ERROR, ERROR,  ERROR }, //DOUBLE
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, TEXT,   ERROR }, //DATE
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, TEXT,   ERROR }, //DATETIME
            { ERROR,    ERROR,   ERROR,  TEXT,  TEXT,  ERROR,  ERROR }, //TEXT
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, ERROR,  ERROR } //ERROR
        };

        public static int[,] division =
        {
            //  BOOL          INTEGER       DOUBLE        DATE       DATETIME    TEXT        ERROR
            { ERROR,    INTEGER, DOUBLE, ERROR, ERROR, ERROR,  ERROR }, //BOOL
            { INTEGER,  INTEGER, DOUBLE, ERROR, ERROR, ERROR,  ERROR }, //INTEGER
            { DOUBLE,   DOUBLE,  DOUBLE, ERROR, ERROR, ERROR,  ERROR }, //DOUBLE
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, TEXT,   ERROR }, //DATE
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, TEXT,   ERROR }, //DATETIME
            { ERROR,    ERROR,   ERROR,  TEXT,  TEXT,  ERROR,  ERROR }, //TEXT
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, ERROR,  ERROR } //ERROR
        };

        public static int[,] potencia =
        {
            //BOOL     INTEGER  DOUBLE   DATE  DATETIME TEXT   ERROR
            { ERROR,    INTEGER, DOUBLE, ERROR, ERROR, ERROR,  ERROR }, //BOOL
            { INTEGER,  INTEGER, DOUBLE, ERROR, ERROR, ERROR,  ERROR }, //INTEGER
            { DOUBLE,   DOUBLE,  DOUBLE, ERROR, ERROR, ERROR,  ERROR }, //DOUBLE
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, TEXT,   ERROR }, //DATE
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, TEXT,   ERROR }, //DATETIME
            { ERROR,    ERROR,   ERROR,  TEXT,  TEXT,  ERROR,  ERROR }, //TEXT
            { ERROR,    ERROR,   ERROR,  ERROR, ERROR, ERROR,  ERROR } //ERROR
        };
    }
}
