using DBMS.arbol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS
{
    public class Constante_dbms
    {
        //SISTEMA DE ARCHIVOS
        public const string RUTA_MAESTRO = "C:\\DBMS";
        public const string RUTA_DB = "C:\\DBMS\\DBS\\";
        public const string RUTA_TABLA = "C:\\DBMS\\TABLAS\\";
        public const string RUTA_METODO = "C:\\DBMS\\METODOS\\";
        public const string RUTA_OBJETO = "C:\\DBMS\\OBJETOS\\";
        public const string EXTENSION = "usac";

        public static Maestro sistema_archivo = null;

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
            catch(Exception ex) { }
        }

    }
}
