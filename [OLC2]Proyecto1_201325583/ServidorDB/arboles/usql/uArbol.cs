using Irony.Parsing;
using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.arboles.usql.Expresiones.Aritmetica;
using ServidorDB.arboles.usql.Expresiones.Logica;
using ServidorDB.arboles.usql.Expresiones.Relacional;
using ServidorDB.otros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql
{
    class uArbol
    {
        public static NodoExp EXPRESION(ParseTreeNode padre)
        {
            if(padre.ChildNodes[0].Term.Name.Equals("ARITMETICA"))
            {
                return ARITMETICA(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("RELACIONAL"))
            {
                return RELACIONAL(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("LOGICA"))
            {
                return LOGICA(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("FUNCIONES"))
            {

            }
            else if (padre.ChildNodes[0].Term.Name.Equals("PRIMITIVOS"))
            {
                return PRIMITIVOS(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("VARIABLES"))
            {
                return VARIABLES(padre.ChildNodes[0]);
            }
            else
            {// ( E )
                return EXPRESION(padre.ChildNodes[1]);
            }
            return null;
        }

        public static NodoExp ARITMETICA(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 3)
            {
                NodoExp izq = EXPRESION(padre.ChildNodes[0]);
                NodoExp der = EXPRESION(padre.ChildNodes[2]);

                if (padre.ChildNodes[1].Token.Text.Equals("+"))
                {
                    return new NodoMas(izq, der, padre.ChildNodes[1].Token.Location.Line, padre.ChildNodes[1].Token.Location.Column);
                }
                else if (padre.ChildNodes[1].Token.Text.Equals("-"))
                {
                    return new NodoMenos(izq, der, padre.ChildNodes[1].Token.Location.Line, padre.ChildNodes[1].Token.Location.Column);
                }
                else if (padre.ChildNodes[1].Token.Text.Equals("*"))
                {
                    return new NodoPor(izq, der, padre.ChildNodes[1].Token.Location.Line, padre.ChildNodes[1].Token.Location.Column);
                }
                else if (padre.ChildNodes[1].Token.Text.Equals("/"))
                {
                    return new NodoDiv(izq, der, padre.ChildNodes[1].Token.Location.Line, padre.ChildNodes[1].Token.Location.Column);
                }
                else
                {
                    return new NodoPot(izq, der, padre.ChildNodes[1].Token.Location.Line, padre.ChildNodes[1].Token.Location.Column);
                }
            }
            else
            {
                NodoExp izq = EXPRESION(padre.ChildNodes[1]);
                return new NodoUMenos(izq, padre.ChildNodes[0].Token.Location.Line, padre.ChildNodes[0].Token.Location.Column);
            }
        }

        public static NodoExp RELACIONAL(ParseTreeNode padre)
        {
            NodoExp izq = EXPRESION(padre.ChildNodes[0]);
            NodoExp der = EXPRESION(padre.ChildNodes[2]);
            
            if (padre.ChildNodes[1].Token.Text.Equals("=="))
            {
                return new NodoDIgual(izq, der, padre.ChildNodes[1].Token.Location.Line, padre.ChildNodes[1].Token.Location.Column);
            }
            else if (padre.ChildNodes[1].Token.Text.Equals("!="))
            {
                return new NodoDiferente(izq, der, padre.ChildNodes[1].Token.Location.Line, padre.ChildNodes[1].Token.Location.Column);
            }
            else if (padre.ChildNodes[1].Token.Text.Equals("<"))
            {
                return new NodoMenor(izq, der, padre.ChildNodes[1].Token.Location.Line, padre.ChildNodes[1].Token.Location.Column);
            }
            else if (padre.ChildNodes[1].Token.Text.Equals(">"))
            {
                return new NodoMayor(izq, der, padre.ChildNodes[1].Token.Location.Line, padre.ChildNodes[1].Token.Location.Column);
            }
            else if (padre.ChildNodes[1].Token.Text.Equals("<="))
            {
                return new NodoMenIgual(izq, der, padre.ChildNodes[1].Token.Location.Line, padre.ChildNodes[1].Token.Location.Column);
            }
            else
            {
                return new NodoMayIgual(izq, der, padre.ChildNodes[1].Token.Location.Line, padre.ChildNodes[1].Token.Location.Column);
            }
        }

        public static NodoExp LOGICA(ParseTreeNode padre)
        {
            if (padre.ChildNodes.Count == 3)
            {
                NodoExp izq = EXPRESION(padre.ChildNodes[0]);
                NodoExp der = EXPRESION(padre.ChildNodes[2]);
                
                if (padre.ChildNodes[1].Token.Text.Equals("&&"))
                {
                    return new NodoAnd(izq, der, padre.ChildNodes[1].Token.Location.Line, padre.ChildNodes[1].Token.Location.Column);
                }
                else
                {
                    return new NodoOr(izq, der, padre.ChildNodes[1].Token.Location.Line, padre.ChildNodes[1].Token.Location.Column);
                }
            }
            else
            {
                NodoExp izq = EXPRESION(padre.ChildNodes[1]);
                return new NodoNot(izq, padre.ChildNodes[0].Token.Location.Line, padre.ChildNodes[0].Token.Location.Column);
            }
        }

        public static NodoExp PRIMITIVOS(ParseTreeNode padre)
        {
            string valor = padre.ChildNodes[0].Token.Text;
            int tipo = Constante.ERROR;

            if (padre.ChildNodes[0].Term.Name.ToLower().Equals("integer"))
            {
                tipo = Constante.INTEGER;
            }
            else if (padre.ChildNodes[0].Term.Name.ToLower().Equals("text"))
            {
                tipo = Constante.TEXT;
            }
            else if (padre.ChildNodes[0].Term.Name.ToLower().Equals("double"))
            {
                tipo = Constante.DOUBLE;
            }
            else if (padre.ChildNodes[0].Term.Name.ToLower().Equals("date"))
            {
                tipo = Constante.DATE;
            }
            else
            {
                tipo = Constante.DATETIME;
            }
            return new NodoPrimitivo(valor, tipo, padre.ChildNodes[0].Token.Location.Line, padre.ChildNodes[0].Token.Location.Column);
        }

        public static NodoExp VARIABLES(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 1)
            {
                return new NodoLVariable(padre.ChildNodes[0].Token.Text,
                    padre.ChildNodes[0].Token.Location.Line,
                    padre.ChildNodes[0].Token.Location.Column);
            }
            else
            {// id . id
                return new NodoLVariable(padre.ChildNodes[0].Token.Text, padre.ChildNodes[2].Token.Text,
                    padre.ChildNodes[0].Token.Location.Line,
                    padre.ChildNodes[0].Token.Location.Column);
            }
        }
    }
}
