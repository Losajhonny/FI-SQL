using Irony.Parsing;
using ServidorDB.arboles.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.analizadores.xml
{
    class MasterAst
    {
        //en la clase master recorrer la lista
        public static List<object> LISTA(ParseTreeNode padre)
        {
            List<object> lm = new List<object>();
            for(int i = 0; i < padre.ChildNodes.Count; i++)
            {
                object val = MAESTRO(padre.ChildNodes[i]);
                if(val != null) { lm.Add(val); }
            }
            return lm;
        }

        public static object MAESTRO(ParseTreeNode padre)
        {
            if (padre.ChildNodes[0].Term.Name.Equals("DB"))
            {
                return DB(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("USUARIO"))
            {
                return USUARIO(padre.ChildNodes[0]);
            }
            return null;
        }

        public static Db DB(ParseTreeNode padre)
        {
            string nombre = NOMBRE(padre.ChildNodes[3]);
            string path = PATH(padre.ChildNodes[4]);
            return new Db(nombre, path);
        }

        public static Usuario USUARIO(ParseTreeNode padre)
        {
            string nombre = NOMBRE(padre.ChildNodes[3]);
            string password = NOMBRE(padre.ChildNodes[4]);
            return new Usuario(nombre, password);
        }

        public static string PATH(ParseTreeNode padre)
        {
            string path = padre.ChildNodes[3].Token.Text;
            path = path.Trim('~');
            return path;
        }

        /*siempre me devuelve un texto en la posicion 3
         por lo tanto va a reemplzar algunos metodos*/
        public static string NOMBRE(ParseTreeNode padre)
        {
            return padre.ChildNodes[3].Token.Text;
        }
    }
}
