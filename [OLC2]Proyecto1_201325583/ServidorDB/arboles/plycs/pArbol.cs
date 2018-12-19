using Irony.Parsing;
using ServidorDB.otros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.plycs
{
    class pArbol
    {
        public static Ini INI(ParseTreeNode padre)
        {
            Ini ini = new Ini(padre.ChildNodes[3].Token.Text,
                PAQUETE(padre.ChildNodes[5]),
                padre.ChildNodes[0].Token.Location.Line,
                padre.ChildNodes[0].Token.Location.Column);
            return ini;
        }

        public static Paquete PAQUETE(ParseTreeNode padre)
        {
            if (padre.ChildNodes[0].Term.Name.Equals("PAQUETE_LOGIN"))
            {
                return PAQUETE_LOGIN(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("PAQUETE_FIN"))
            {
                return PAQUETE_FIN(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("PAQUETE_ERROR"))
            {
                return PAQUETE_ERROR(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("PAQUETE_INST"))
            {
                return PAQUETE_INST(padre.ChildNodes[0]);
            }
            else
            {   //aqui deberia ir el paquete reporte
                return null;
            }
        }

        public static Paquete PAQUETE_ERROR(ParseTreeNode padre)
        {
            int line = padre.ChildNodes[0].Token.Location.Line;
            int colm = padre.ChildNodes[0].Token.Location.Column;

            string tipo = padre.ChildNodes[6].Token.Text.Trim('\"');
            string msg = padre.ChildNodes[10].Token.Text.Trim('\"');

            Paquete p = new Paquete(line, colm);
            p.Tipo_paquete = Paquete.ERROR;
            p.Estado_paquete = Paquete.RECIBIDO;
            p.Tipo_error = tipo;
            p.Msg = msg;
            p.Campos = FILAS(padre.ChildNodes[14]);
            return p;
        }

        public static Paquete PAQUETE_LOGIN(ParseTreeNode padre)
        {
            int line = padre.ChildNodes[0].Token.Location.Line;
            int colm = padre.ChildNodes[0].Token.Location.Column;

            if (padre.ChildNodes[9].Term.Name.Equals("BOOLEANO"))
            {
                Paquete p = new Paquete(line, colm);
                p.Usuario = padre.ChildNodes[5].Token.Text.Trim('\"');
                p.Login = BOOLEANO(padre.ChildNodes[9]);
                p.Tipo_paquete = Paquete.LOGIN;
                p.Estado_paquete = Paquete.RECIBIDO;
                return p;
            }
            else
            {
                Paquete p = new Paquete(line, colm);
                p.Usuario = padre.ChildNodes[5].Token.Text.Trim('\"');
                p.Password = padre.ChildNodes[9].Token.Text.Trim('\"');
                p.Tipo_paquete = Paquete.LOGIN;
                p.Estado_paquete = Paquete.ENVIO;
                return p;
            }
        }

        public static Paquete PAQUETE_FIN(ParseTreeNode padre)
        {
            int line = padre.ChildNodes[0].Token.Location.Line;
            int colm = padre.ChildNodes[0].Token.Location.Column;

            Paquete p = new Paquete(line, colm);
            p.Tipo_paquete = Paquete.FIN;
            p.Estado_paquete = Paquete.ENVIO;

            return p;
        }

        public static Paquete PAQUETE_INST(ParseTreeNode padre)
        {
            int line = padre.ChildNodes[0].Token.Location.Line;
            int colm = padre.ChildNodes[0].Token.Location.Column;

            if (padre.ChildNodes[6].Term.Name.Equals("DATOS"))
            {
                object obj = DATOS(padre.ChildNodes[6]);
                List<List<Campo>> lc = null;
                string respuesta = null;
                if (obj is List<List<Campo>>)
                {
                    lc = (List<List<Campo>>)obj;
                }
                else
                {
                    respuesta = obj.ToString();
                }
                Paquete p = new Paquete(line, colm);
                p.Instruccion = null;
                p.Respuesta = respuesta;
                p.Campos = lc;
                p.Tipo_paquete = Paquete.INST;
                p.Estado_paquete = Paquete.RECIBIDO;
                return p;
            }
            else
            {
                string inst = padre.ChildNodes[6].Token.Text.Trim('\"');
                Paquete p = new Paquete(line, colm);
                p.Instruccion = inst;
                p.Respuesta = null;
                p.Campos = null;
                p.Tipo_paquete = Paquete.INST;
                p.Estado_paquete = Paquete.ENVIO;
                return p;
            }
        }

        public static string BOOLEANO(ParseTreeNode padre)
        {
            return padre.ChildNodes[0].Token.Text;
        }

        public static object DATOS(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 3)
            {
                return FILAS(padre.ChildNodes[2]);
            }
            else
            {
                return padre.ChildNodes[0].Token.Text.Trim('\"');
            }
        }

        public static List<List<Campo>> FILAS(ParseTreeNode padre)
        {
            List<List<Campo>> filas = new List<List<Campo>>();
            for (int i = 0; i < padre.ChildNodes.Count; i++)
            {
                filas.Add(FILA(padre.ChildNodes[i]));
            }
            return filas;
        }

        public static List<Campo> FILA(ParseTreeNode padre)
        {
            return CAMPOS(padre.ChildNodes[1]);
        }

        public static List<Campo> CAMPOS(ParseTreeNode padre)
        {
            List<Campo> campos = new List<Campo>();
            for (int i = 0; i < padre.ChildNodes.Count; i++)
            {
                campos.Add(CAMPO(padre.ChildNodes[i]));
            }
            return campos;
        }

        public static Campo CAMPO(ParseTreeNode padre)
        {
            int tipo = -1;
            string valor = "";
            if (padre.ChildNodes[2].Term.Name.Equals("integer"))
            {
                tipo = Constante.INTEGER;
                valor = padre.ChildNodes[2].Token.Text;
            }
            else if (padre.ChildNodes[2].Term.Name.Equals("text"))
            {
                tipo = Constante.TEXT;
                valor = padre.ChildNodes[2].Token.Text.Trim('\"');
            }
            else
            {
                tipo = Constante.DOUBLE;
                valor = padre.ChildNodes[2].Token.Text;
            }
            return new Campo(padre.ChildNodes[0].Token.Text.Trim('\"'),
                valor ,
                tipo, padre.ChildNodes[0].Token.Location.Line,
                padre.ChildNodes[0].Token.Location.Column);
        }
    }
}
