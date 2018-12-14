using Irony.Parsing;
using ServidorDB.arboles.usql.DDL.Crear;
using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.arboles.usql.Expresiones.Aritmetica;
using ServidorDB.arboles.usql.Expresiones.Logica;
using ServidorDB.arboles.usql.Expresiones.Relacional;
using ServidorDB.arboles.usql.SSL;
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
        public static List<uInstruccion> SENTENCIAS(ParseTreeNode padre)
        {
            List<uInstruccion> sents = new List<uInstruccion>();
            for(int i = 0; i < padre.ChildNodes.Count; i++)
            {
                sents.Add(SENTENCIA(padre.ChildNodes[i]));
            }
            return sents;
        }

        public static uInstruccion SENTENCIA(ParseTreeNode padre)
        {
            if (padre.ChildNodes[0].Term.Name.Equals("DDL"))
            {
                return DDL(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("DML"))
            {

            }
            else if (padre.ChildNodes[0].Term.Name.Equals("DCL"))
            {

            }
            else if (padre.ChildNodes[0].Term.Name.Equals("SSL"))
            {
                return SSL(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("BACKUP"))
            {

            }
            else if (padre.ChildNodes[0].Term.Name.Equals("RESTORE"))
            {

            }
            else if (padre.ChildNodes.Count == 2)
            {
                if (padre.ChildNodes[0].Term.Name.Equals("LLAMADA"))
                {

                }
                else
                { // detener
                    return new Detener();
                }
            }
            else
            { // retorno
                return new Retornar(EXPRESION(padre.ChildNodes[1]),
                    padre.ChildNodes[0].Token.Location.Line,
                    padre.ChildNodes[0].Token.Location.Column);
            }
            return null;
        }

        #region DDL
        public static uInstruccion DDL(ParseTreeNode padre)
        {
            if (padre.ChildNodes[0].Term.Name.Equals("CREATE"))
            {
                return CREATE(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("ALTER"))
            {

            }
            else
            {

            }
            return null;
        }

        public static uInstruccion CREATE(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 10)
            {//creando funcion
                List<uInstruccion> paramss = PARAMETROS(padre.ChildNodes[4]);
                List<uInstruccion> inst = SENTENCIAS(padre.ChildNodes[8]);
                Funcion fun = new Funcion(padre.ChildNodes[2].Token.Text,
                    TIPO_DATO(padre.ChildNodes[6]), paramss, inst,
                    padre.ChildNodes[0].Token.Location.Line,
                    padre.ChildNodes[0].Token.Location.Column);
                return fun;
            }
            else if (padre.ChildNodes.Count == 9)
            {//creando procedimiento
                List<uInstruccion> paramss = PARAMETROS(padre.ChildNodes[4]);
                List<uInstruccion> inst = SENTENCIAS(padre.ChildNodes[7]);
                Procedimiento proc = new Procedimiento(padre.ChildNodes[2].Token.Text,
                    Constante.VOID, paramss, inst,
                    padre.ChildNodes[0].Token.Location.Line,
                    padre.ChildNodes[0].Token.Location.Column);
                return proc;
            }
            else if (padre.ChildNodes.Count == 8)
            {
                NodoExp ne = EXPRESION(padre.ChildNodes[6]);
                return new Usuario(padre.ChildNodes[2].Token.Text,
                    ne, padre.ChildNodes[0].Token.Location.Line,
                    padre.ChildNodes[0].Token.Location.Column);
            }
            else if (padre.ChildNodes.Count == 7)
            {
                if (padre.ChildNodes[1].Token.Text.ToLower().Equals("objeto"))
                {

                }
                else
                {//tabla

                }
            }
            return null;
        }

        public static List<uInstruccion> PARAMETROS(ParseTreeNode padre)
        {
            List<uInstruccion> inst = new List<uInstruccion>();
            for(int i = 0; i < padre.ChildNodes.Count; i++)
            {
                inst.Add(PARAMETRO(padre.ChildNodes[i]));
            }
            return inst;
        }

        public static Declarar PARAMETRO(ParseTreeNode padre)
        {
            List<string> vars = new List<string>();
            vars.Add(padre.ChildNodes[1].Token.Text);
            Declarar dec = new Declarar(vars, TIPO_DATO(padre.ChildNodes[0]),
                padre.ChildNodes[1].Token.Location.Line,
                padre.ChildNodes[1].Token.Location.Column);
            return dec;
        }
        #endregion

        #region SSL
        public static uInstruccion SSL(ParseTreeNode padre)
        {
            if (padre.ChildNodes[0].Term.Name.Equals("DECLARACION"))
            {
                return DECLARACION(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("ASIGNACION"))
            {
                return ASIGNACION(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("IMPRIMIR"))
            {
                return IMPRIMIR(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("SI"))
            {
                return SI(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("SELECCIONA"))
            {
                return SELECCIONA(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("PARA"))
            {
                return PARA(padre.ChildNodes[0]);
            }
            else
            {
                return MIENTRAS(padre.ChildNodes[0]);
            }
        }

        public static Mientras MIENTRAS(ParseTreeNode padre)
        {
            return new Mientras(EXPRESION(padre.ChildNodes[2]),
                SENTENCIAS(padre.ChildNodes[5]),
                padre.ChildNodes[0].Token.Location.Line,
                padre.ChildNodes[0].Token.Location.Column);
        }

        public static Para PARA(ParseTreeNode padre)
        {
            List<string> lista = new List<string>();
            lista.Add(padre.ChildNodes[3].Token.Text);
            Declarar dec = new Declarar(lista, Constante.INTEGER,
                EXPRESION(padre.ChildNodes[6]), padre.ChildNodes[2].Token.Location.Line,
                padre.ChildNodes[2].Token.Location.Column);

            return new Para(SENTENCIAS(padre.ChildNodes[13]), dec,
                EXPRESION(padre.ChildNodes[8]),
                padre.ChildNodes[10].ChildNodes[0].Token.Text,
                padre.ChildNodes[0].Token.Location.Line,
                padre.ChildNodes[0].Token.Location.Line);

        }
        
        public static Si SI(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 7)
            {
                return new Si(EXPRESION(padre.ChildNodes[2]),
                    SENTENCIAS(padre.ChildNodes[5]),
                    padre.ChildNodes[0].Token.Location.Line,
                    padre.ChildNodes[0].Token.Location.Column);
            }
            else
            {
                Sino sino = new Sino(SENTENCIAS(padre.ChildNodes[9]));
                return new Si(EXPRESION(padre.ChildNodes[2]),
                    SENTENCIAS(padre.ChildNodes[5]), sino,
                    padre.ChildNodes[0].Token.Location.Line,
                    padre.ChildNodes[0].Token.Location.Column);
            }
        }

        public static Selecciona SELECCIONA(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 8)
            {
                return new Selecciona(EXPRESION(padre.ChildNodes[2]),
                    CASOS(padre.ChildNodes[5]),
                    DEFECTO(padre.ChildNodes[6]),
                    padre.ChildNodes[0].Token.Location.Line,
                    padre.ChildNodes[0].Token.Location.Column);
            }
            else
            {
                return new Selecciona(EXPRESION(padre.ChildNodes[2]),
                    CASOS(padre.ChildNodes[5]),
                    padre.ChildNodes[0].Token.Location.Line,
                    padre.ChildNodes[0].Token.Location.Column);
            }
        }

        public static List<uInstruccion> CASOS(ParseTreeNode padre)
        {
            List<uInstruccion> casos = new List<uInstruccion>();
            for(int i = 0; i < padre.ChildNodes.Count; i++)
            {
                casos.Add(CASO(padre.ChildNodes[i]));
            }
            return casos;
        }

        public static Caso CASO(ParseTreeNode padre)
        {
            return new Caso(EXPRESION(padre.ChildNodes[1]), SENTENCIAS(padre.ChildNodes[3]),
                padre.ChildNodes[0].Token.Location.Line,
                padre.ChildNodes[0].Token.Location.Column);
        }

        public static Defecto DEFECTO(ParseTreeNode padre)
        {
            return new Defecto(SENTENCIAS(padre.ChildNodes[2]));
        }

        public static Imprimir IMPRIMIR(ParseTreeNode padre)
        {
            return new Imprimir(EXPRESION(padre.ChildNodes[2]),
                padre.ChildNodes[0].Token.Location.Line,
                padre.ChildNodes[0].Token.Location.Column);
        }

        public static Asignacion ASIGNACION(ParseTreeNode padre)
        {
            if (padre.ChildNodes.Count == 5)
            {
                return new Asignacion(padre.ChildNodes[0].Token.Text,
                    padre.ChildNodes[2].Token.Text,
                    EXPRESION(padre.ChildNodes[4]),
                    padre.ChildNodes[3].Token.Location.Line,
                    padre.ChildNodes[3].Token.Location.Column);
            }
            else
            {
                return new Asignacion(padre.ChildNodes[0].Token.Text,
                    EXPRESION(padre.ChildNodes[2]),
                    padre.ChildNodes[1].Token.Location.Line,
                    padre.ChildNodes[1].Token.Location.Column);
            }
        }

        public static Declarar DECLARACION(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 5)
            {
                List<string> vars = LISTA_VARIABLES(padre.ChildNodes[1]);
                int tipo = TIPO_DATO(padre.ChildNodes[2]);
                NodoExp ne = EXPRESION(padre.ChildNodes[4]);
                
                return new Declarar(vars, tipo, ne, padre.ChildNodes[0].Token.Location.Line,
                    padre.ChildNodes[0].Token.Location.Column);
            }
            else
            {
                if (padre.ChildNodes[1].Term.Name.Equals("LISTA_VARIABLES"))
                {
                    List<string> vars = LISTA_VARIABLES(padre.ChildNodes[1]);
                    int tipo = TIPO_DATO(padre.ChildNodes[2]);
                    return new Declarar(vars, tipo, padre.ChildNodes[0].Token.Location.Line,
                    padre.ChildNodes[0].Token.Location.Column);
                }
                else
                {
                    return new Declarar(padre.ChildNodes[1].Token.Text,
                        padre.ChildNodes[2].Token.Text,
                        Constante.ID, padre.ChildNodes[0].Token.Location.Line,
                        padre.ChildNodes[0].Token.Location.Column);
                }
            }
        }
        #endregion

        #region Otros
        public static int TIPO_DATO(ParseTreeNode padre)
        {
            if (padre.ChildNodes[0].Term.Name.Equals("TIPO_DATO_PR"))
            {
                return TIPO_DATO_PR(padre.ChildNodes[0]);
            }
            else
            {
                return Constante.ID;
            }
        }

        public static int TIPO_DATO_PR(ParseTreeNode padre)
        {
            if (padre.ChildNodes[0].Token.Text.ToLower().Equals("text"))
            {
                return Constante.TEXT;
            }
            else if (padre.ChildNodes[0].Token.Text.ToLower().Equals("integer"))
            {
                return Constante.INTEGER;
            }
            else if (padre.ChildNodes[0].Token.Text.ToLower().Equals("double"))
            {
                return Constante.DOUBLE;
            }
            else if (padre.ChildNodes[0].Token.Text.ToLower().Equals("bool"))
            {
                return Constante.BOOL;
            }
            else if (padre.ChildNodes[0].Token.Text.ToLower().Equals("date"))
            {
                return Constante.DATE;
            }
            else
            {
                return Constante.DATETIME;
            }
        }

        public static List<string> LISTA_VARIABLES(ParseTreeNode padre)
        {
            List<string> lv = new List<string>();
            for(int i = 0; i < padre.ChildNodes.Count; i++)
            {
                lv.Add(padre.ChildNodes[i].Token.Text);
            }
            return lv;
        }
        #endregion

        #region EXPRESIONES
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
        #endregion
    }
}
