using Irony.Parsing;
using ServidorDB.arboles.usql.DCL;
using ServidorDB.arboles.usql.DDL;
using ServidorDB.arboles.usql.DML;
using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.arboles.usql.Expresiones.Aritmetica;
using ServidorDB.arboles.usql.Expresiones.Logica;
using ServidorDB.arboles.usql.Expresiones.Relacional;
using ServidorDB.arboles.usql.SSL;
using ServidorDB.arboles.xml;
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
                object obj = SENTENCIA(padre.ChildNodes[i]);
                if(obj != null) { sents.Add((uInstruccion)obj); }
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
                return DML(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("DCL"))
            {
                return DCL(padre.ChildNodes[0]);
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
                    return LLAMADA1(padre.ChildNodes[0]);
                }
                else if (padre.ChildNodes[0].Term.Name.ToLower().Equals("detener"))
                { // detener
                    return new Detener();
                }
            }
            else if (padre.ChildNodes[0].Term.Name.ToLower().Equals("retorno"))
            { // retorno
                return new Retornar(EXPRESION(padre.ChildNodes[1]),
                    padre.ChildNodes[0].Token.Location.Line,
                    padre.ChildNodes[0].Token.Location.Column);
            }
            return null;
        }

        public static uInstruccion LLAMADA1(ParseTreeNode padre)
        {
            if (padre.ChildNodes.Count == 4)
            {
                LLamada llama = new LLamada(null, padre.ChildNodes[0].Token.Location.Line,
                padre.ChildNodes[0].Token.Location.Column);
                llama.Id = padre.ChildNodes[0].Token.Text;
                llama.Paramss = LISTA_VALORES(padre.ChildNodes[2]);
                llama.Tipo = tabla_simbolos.Simbolo.PROCEDIMIENTO;
                return llama;
            }
            else
            {
                LLamada llama = new LLamada(null, padre.ChildNodes[0].Token.Location.Line,
                padre.ChildNodes[0].Token.Location.Column);
                llama.Id = padre.ChildNodes[0].Token.Text;
                llama.Paramss = new List<NodoExp>();
                llama.Tipo = tabla_simbolos.Simbolo.PROCEDIMIENTO;
                return llama;
            }
        }

        public static uInstruccion DML(ParseTreeNode padre)
        {
            if (padre.ChildNodes[0].Term.Name.Equals("INSERT"))
            {
                return INSERT(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("UPDATE"))
            {
                return UPDATE(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("SELECT"))
            {
                return SELECT(padre.ChildNodes[0]);
            }
            return null;
        }

        public static uInstruccion SELECT(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 5)
            {
                if (padre.ChildNodes[1].Term.Name.Equals("LISTA_ID"))
                {
                    Select tmp = FUNCIONES_SELECT(padre.ChildNodes[4]);
                    tmp.Campos = LISTA_ID(padre.ChildNodes[1]);
                    tmp.Tablas = LISTA_ID(padre.ChildNodes[3]);
                    tmp.Line = padre.ChildNodes[0].Token.Location.Line;
                    tmp.Colm = padre.ChildNodes[0].Token.Location.Column;
                    return tmp;
                }
                else
                {
                    Select tmp = FUNCIONES_SELECT(padre.ChildNodes[4]);
                    tmp.Campos = null;
                    tmp.Tablas = LISTA_ID(padre.ChildNodes[3]);
                    tmp.Line = padre.ChildNodes[0].Token.Location.Line;
                    tmp.Colm = padre.ChildNodes[0].Token.Location.Column;
                    return tmp;
                }
            }
            else if (padre.ChildNodes.Count == 4)
            {
                if (padre.ChildNodes[1].Term.Name.Equals("LISTA_ID"))
                {
                    List<string> campos = LISTA_ID(padre.ChildNodes[1]);
                    List<string>  tablas = LISTA_ID(padre.ChildNodes[3]);
                    int line = padre.ChildNodes[0].Token.Location.Line;
                    int colm = padre.ChildNodes[0].Token.Location.Column;
                    return new Select(campos, tablas, null, null, Constante.NONE);
                }
                else
                {
                    List<string> campos = null;
                    List<string> tablas = LISTA_ID(padre.ChildNodes[3]);
                    int line = padre.ChildNodes[0].Token.Location.Line;
                    int colm = padre.ChildNodes[0].Token.Location.Column;
                    return new Select(campos, tablas, null, null, Constante.NONE);
                }
            }
            return null;
        }

        public static Select FUNCIONES_SELECT(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 5)
            {
                if (padre.ChildNodes[4].Token.Text.ToLower().Equals("asc"))
                {
                    return new Select(null, null, EXPRESION(padre.ChildNodes[1]),
                        padre.ChildNodes[3].Token.Text, Constante.ASC);
                }
                else
                {
                    return new Select(null, null, EXPRESION(padre.ChildNodes[1]),
                        padre.ChildNodes[3].Token.Text, Constante.DESC);
                }
            }
            else if (padre.ChildNodes.Count == 4)
            {
                return new Select(null, null, EXPRESION(padre.ChildNodes[1]),
                        padre.ChildNodes[3].Token.Text, Constante.NONE);
            }
            else if (padre.ChildNodes.Count == 3)
            {
                if (padre.ChildNodes[2].Token.Text.ToLower().Equals("asc"))
                {
                    return new Select(null, null, null,
                        padre.ChildNodes[3].Token.Text, Constante.ASC);
                }
                else
                {
                    return new Select(null, null, null,
                        padre.ChildNodes[3].Token.Text, Constante.DESC);
                }
            }
            else if (padre.ChildNodes.Count == 2)
            {
                if (padre.ChildNodes[0].Token.Text.ToLower().Equals("donde"))
                {
                    return new Select(null, null, EXPRESION(padre.ChildNodes[1]),
                       null, Constante.NONE);
                }
                else
                {
                    return new Select(null, null, null,
                        padre.ChildNodes[1].Token.Text, Constante.NONE);
                }
               
            }
            return null;
        }

        public static uInstruccion UPDATE(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 12)
            {
                NodoExp exp = EXPRESION(padre.ChildNodes[11]);
                List<string> ids= LISTA_ID(padre.ChildNodes[4]);
                List<NodoExp> vals = LISTA_VALORES(padre.ChildNodes[8]);
                string id = padre.ChildNodes[2].Token.Text;
                int line = padre.ChildNodes[0].Token.Location.Line;
                int colm = padre.ChildNodes[0].Token.Location.Column;

                return new Actualizar(id, ids, vals, exp, line, colm);
            }
            else
            {
                List<string> ids = LISTA_ID(padre.ChildNodes[4]);
                List<NodoExp> vals = LISTA_VALORES(padre.ChildNodes[8]);
                string id = padre.ChildNodes[2].Token.Text;
                int line = padre.ChildNodes[0].Token.Location.Line;
                int colm = padre.ChildNodes[0].Token.Location.Column;

                return new Actualizar(id, ids, vals, null, line, colm);
            }
        }

        public static uInstruccion INSERT(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 7)
            {
                int line = padre.ChildNodes[0].Token.Location.Line;
                int colm = padre.ChildNodes[0].Token.Location.Column;

                return new Insertar(padre.ChildNodes[3].Token.Text
                    , LISTA_VALORES(padre.ChildNodes[5]), line, colm);
            }
            else if (padre.ChildNodes.Count == 11)
            {
                int line = padre.ChildNodes[0].Token.Location.Line;
                int colm = padre.ChildNodes[0].Token.Location.Column;

                return new Insertar(padre.ChildNodes[3].Token.Text,
                    LISTA_ID(padre.ChildNodes[5])
                    , LISTA_VALORES(padre.ChildNodes[9]), line, colm);
            }
            return null;
        }

        public static uInstruccion DCL(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count != 0)
            {
                int tipo_permiso = Constante.DENEGAR;
                if (padre.ChildNodes[0].Token.Text.ToLower().Equals("otorgar"))
                {
                    tipo_permiso = Constante.OTORGAR;
                }
                string usuario = padre.ChildNodes[2].Token.Text;
                string db = padre.ChildNodes[4].Token.Text;
                string objeto = null;

                if (!padre.ChildNodes[6].Token.Text.Equals("*"))
                {
                    objeto = padre.ChildNodes[6].Token.Text;
                }

                return new Permisos_Dcl(tipo_permiso, usuario, db, objeto,
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
                return ALTER(padre.ChildNodes[0]);
            }
            else
            {
                return DROP(padre.ChildNodes[0]);
            }
        }

        public static uInstruccion DROP(ParseTreeNode padre)
        {
            return new Eliminar_Ddl(OBJETO_USQL(padre.ChildNodes[1]),
                padre.ChildNodes[2].Token.Text, padre.ChildNodes[0].Token.Location.Line,
                padre.ChildNodes[0].Token.Location.Column);
        }

        public static int OBJETO_USQL(ParseTreeNode padre)
        {
            if (padre.ChildNodes[0].Token.Text.ToLower().Equals("tabla"))
            {
                return Constante.tTABLA;
            }
            else if (padre.ChildNodes[0].Token.Text.ToLower().Equals("base_datos"))
            {
                return Constante.tBASE_DATOS;
            }
            else if (padre.ChildNodes[0].Token.Text.ToLower().Equals("objeto"))
            {
                return Constante.tOBJETO;
            }
            else if (padre.ChildNodes[0].Token.Text.ToLower().Equals("usuario"))
            {
                return Constante.tUSUARIO;
            }
            else if (padre.ChildNodes[0].Token.Text.ToLower().Equals("procedimiento"))
            {
                return Constante.tPROCEDIMIENTO;
            }
            else
            {
                return Constante.tFUNCION;
            }
        }

        public static uInstruccion ALTER(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 5)
            {
                int tipo_alterar = Constante.tOBJETO;
                if (padre.ChildNodes[1].Token.Text.ToLower().Equals("tabla"))
                {
                    tipo_alterar = Constante.tTABLA;
                }
                //quitar atributos de la tabla u objeto
                return new Alterar_Ddl(tipo_alterar, padre.ChildNodes[2].Token.Text, LISTA_ID(padre.ChildNodes[4]),
                    padre.ChildNodes[1].Token.Location.Line, padre.ChildNodes[1].Token.Location.Column);
            }
            else if (padre.ChildNodes.Count == 7)
            {
                if (padre.ChildNodes[1].Token.Text.ToLower().Equals("tabla"))
                {
                    //Para agregar mas campos
                    List<Atributo> atributos = CAMPOS_TABLA(padre.ChildNodes[5]);
                    return new Alterar_Ddl(Constante.tTABLA, padre.ChildNodes[2].Token.Text,
                        atributos, padre.ChildNodes[1].Token.Location.Line,
                        padre.ChildNodes[1].Token.Location.Column);
                }
                else if (padre.ChildNodes[1].Token.Text.ToLower().Equals("objeto"))
                {
                    List<Declarar> declarars = CAMPOS_OBJETO(padre.ChildNodes[5]);
                    return new Alterar_Ddl(Constante.tOBJETO, padre.ChildNodes[2].Token.Text,
                        declarars, padre.ChildNodes[1].Token.Location.Line,
                        padre.ChildNodes[1].Token.Location.Column);
                }
                else
                {   //cambiar password de usuario
                    NodoExp ne = EXPRESION(padre.ChildNodes[6]);
                    return new Alterar_Ddl(Constante.tUSUARIO, padre.ChildNodes[2].Token.Text,
                       ne, padre.ChildNodes[1].Token.Location.Line,
                       padre.ChildNodes[1].Token.Location.Column);
                }
            }
            return null;
        }

        public static uInstruccion CREATE(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 10)
            {//creando funcion
                List<Declarar> paramss = PARAMETROS(padre.ChildNodes[4]);
                List<uInstruccion> inst = SENTENCIAS(padre.ChildNodes[8]);

                int linei, linef, colmi, colmf, line, colm;

                line = padre.ChildNodes[1].Token.Location.Line;
                colm = padre.ChildNodes[1].Token.Location.Column;

                linei = padre.ChildNodes[7].Token.Location.Line;
                colmi = padre.ChildNodes[7].Token.Location.Column;

                linef = padre.ChildNodes[9].Token.Location.Line;
                colmf = padre.ChildNodes[9].Token.Location.Column;

                Crear_Ddl cddl = new Crear_Ddl(Constante.tFUNCION, TIPO_DATO(padre.ChildNodes[6]),
                    padre.ChildNodes[2].Token.Text, paramss, inst,
                    linei, linef, colmi, colmf);

                cddl.Line = line;
                cddl.Colm = colm;
                return cddl;
            }
            else if (padre.ChildNodes.Count == 9)
            {//creando procedimiento
                List<Declarar> paramss = PARAMETROS(padre.ChildNodes[4]);
                List<uInstruccion> inst = SENTENCIAS(padre.ChildNodes[7]);

                int linei, linef, colmi, colmf, line, colm;

                line = padre.ChildNodes[1].Token.Location.Line;
                colm = padre.ChildNodes[1].Token.Location.Column;

                linei = padre.ChildNodes[6].Token.Location.Line;
                colmi = padre.ChildNodes[6].Token.Location.Column;

                linef = padre.ChildNodes[8].Token.Location.Line;
                colmf = padre.ChildNodes[8].Token.Location.Column;

                Crear_Ddl cddl = new Crear_Ddl(Constante.tPROCEDIMIENTO, Constante.VOID,
                    padre.ChildNodes[2].Token.Text, paramss, inst,
                    linei, linef, colmi, colmf);
                
                return cddl;
            }
            else if (padre.ChildNodes.Count == 8)
            {
                NodoExp ne = EXPRESION(padre.ChildNodes[6]);

                int line, colm;
                line = padre.ChildNodes[1].Token.Location.Line;
                colm = padre.ChildNodes[1].Token.Location.Column;

                Crear_Ddl cddl = new Crear_Ddl(Constante.tUSUARIO,
                    padre.ChildNodes[2].Token.Text, ne, line, colm);
                return cddl;
            }
            else if (padre.ChildNodes.Count == 7)
            {
                if (padre.ChildNodes[1].Token.Text.ToLower().Equals("objeto"))
                {
                    //como para obtener las declaraciones (parametros) de un objeto
                    //tiene la misma posicion que las declaraciones que un proc
                    //o funcion solo que cambia el token que se esta analizando
                    //id  |  variable
                    //en este caso seria solo id
                    List<Declarar> paramss = CAMPOS_OBJETO(padre.ChildNodes[4]);
                    int line, colm;

                    line = padre.ChildNodes[1].Token.Location.Line;
                    colm = padre.ChildNodes[1].Token.Location.Column;

                    Crear_Ddl cddl = new Crear_Ddl(Constante.tOBJETO,
                        padre.ChildNodes[2].Token.Text, paramss, line, colm);
                    return cddl;
                }
                else
                {//tabla
                    List<Atributo> atributos = CAMPOS_TABLA(padre.ChildNodes[4]);
                    Crear_Ddl cddl = new Crear_Ddl(Constante.tTABLA,
                        padre.ChildNodes[2].Token.Text, atributos,
                        padre.ChildNodes[1].Token.Location.Line,
                        padre.ChildNodes[1].Token.Location.Column);
                    return cddl;
                }
            }
            else if (padre.ChildNodes.Count == 4)
            {
                Crear_Ddl cddl = new Crear_Ddl(Constante.tBASE_DATOS, padre.ChildNodes[2].Token.Text,
                    padre.ChildNodes[1].Token.Location.Line,
                    padre.ChildNodes[1].Token.Location.Column);
                return cddl;
            }
            else if (padre.ChildNodes.Count == 3)
            {
                Crear_Ddl cddl = new Crear_Ddl(Constante.tUSAR, padre.ChildNodes[1].Token.Text,
                    padre.ChildNodes[0].Token.Location.Line,
                    padre.ChildNodes[0].Token.Location.Column);
                return cddl;
            }
            return null;
        }

        public static List<Atributo> CAMPOS_TABLA(ParseTreeNode padre)
        {
            List<Atributo> inst = new List<Atributo>();
            for (int i = 0; i < padre.ChildNodes.Count; i++)
            {
                inst.Add(CAMPO_TABLA(padre.ChildNodes[i]));
            }
            return inst;
        }

        public static Atributo CAMPO_TABLA(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 3)
            {
                //tiene complemento
                //validar si es una llave foranea
                object obj = COMPLEMENTO(padre.ChildNodes[2]);
                int tipo = TIPO_DATO_PR(padre.ChildNodes[0]);

                if(obj is string[])
                {
                    string[] comp = (string[])obj;
                    Atributo atr = new Atributo(tipo, padre.ChildNodes[1].Token.Text,
                        comp[0], comp[1]);
                    atr.Line = padre.ChildNodes[1].Token.Location.Line;
                    atr.Colm = padre.ChildNodes[1].Token.Location.Column;
                    return atr;
                }
                else
                {
                    Atributo atr = new Atributo(tipo, padre.ChildNodes[1].Token.Text,
                        obj.ToString());
                    atr.Line = padre.ChildNodes[1].Token.Location.Line;
                    atr.Colm = padre.ChildNodes[1].Token.Location.Column;
                    return atr;
                }
            }
            else
            {
                //atributo sin complemento
                //le asignamos nulo por defecto
                string complemento = "nonulo";
                int tipo = TIPO_DATO_PR(padre.ChildNodes[0]);

                Atributo atr = new Atributo(tipo, padre.ChildNodes[1].Token.Text);
                atr.Complemento = complemento;
                atr.Line = padre.ChildNodes[1].Token.Location.Line;
                atr.Colm = padre.ChildNodes[1].Token.Location.Column;
                return atr;
            }
        }

        public static object COMPLEMENTO(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 2)
            {   //retornara "nonulo"
                string val = "";
                val += padre.ChildNodes[0].Token.Text + padre.ChildNodes[1].Token.Text;
                return val;
            }
            else if(padre.ChildNodes.Count == 3)
            {   //retornara la llave foranea
                                //      tabla,      id
                string[] val = { padre.ChildNodes[1].Token.Text, padre.ChildNodes[2].Token.Text };
                return val;
            }
            else
            {   //retornara los demas
                return padre.ChildNodes[0].Token.Text;
            }
        }

        public static List<Declarar> PARAMETROS(ParseTreeNode padre)
        {
            List<Declarar> inst = new List<Declarar>();
            for (int i = 0; i < padre.ChildNodes.Count; i++)
            {
                inst.Add(PARAMETRO(padre.ChildNodes[i]));
            }
            return inst;
        }

        public static List<Declarar> CAMPOS_OBJETO(ParseTreeNode padre)
        {
            List<Declarar> inst = new List<Declarar>();
            for (int i = 0; i < padre.ChildNodes.Count; i++)
            {
                inst.Add(PARAMETRO(padre.ChildNodes[i]));
            }
            return inst;
        }

        public static Declarar PARAMETRO(ParseTreeNode padre)
        {
            //solo acepta tipos de datos primitivos
            List<string> vars = new List<string>();
            vars.Add(padre.ChildNodes[1].Token.Text);
            Declarar dec = new Declarar(vars, TIPO_DATO_PR(padre.ChildNodes[0]),
                padre.ChildNodes[1].Token.Location.Line,
                padre.ChildNodes[1].Token.Location.Column);
            return dec;
        }

        public static Declarar PARAMETRO_ID(ParseTreeNode padre)
        {
            //acepta tipos primitivos y de objetos
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

        public static List<NodoExp> LISTA_VALORES(ParseTreeNode padre)
        {
            List<NodoExp> ne = new List<NodoExp>();
            for(int i = 0; i < padre.ChildNodes.Count; i++)
            {
                object obj = EXPRESION(padre.ChildNodes[i]);
                if(obj != null)
                {
                    ne.Add((NodoExp)obj);
                }
            }
            return ne;
        }

        public static List<string> LISTA_ID(ParseTreeNode padre)
        {
            List<string> ids = new List<string>();
            for(int i = 0; i < padre.ChildNodes.Count; i++)
            {
                ids.Add(padre.ChildNodes[i].Token.Text);
            }
            return ids;
        }

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

        public static NodoExp CONTAR(ParseTreeNode padre)
        {
            uInstruccion seleccionar = SELECT(padre.ChildNodes[3]);
            return new Contar(seleccionar, padre.ChildNodes[0].Token.Location.Line,
                padre.ChildNodes[0].Token.Location.Column);
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
                return FUNCIONES(padre.ChildNodes[0]);
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
             //@var. id
                return new NodoLVariable(padre.ChildNodes[0].Token.Text, padre.ChildNodes[2].Token.Text,
                    padre.ChildNodes[0].Token.Location.Line,
                    padre.ChildNodes[0].Token.Location.Column);
            }
        }

        public static NodoExp FUNCIONES(ParseTreeNode padre)
        {
            if (padre.ChildNodes[0].Term.Name.Equals("CONTAR"))
            {
                return CONTAR(padre.ChildNodes[0]);
            }
            else if (padre.ChildNodes[0].Term.Name.Equals("LLAMADA"))
            {
                return LLAMADA2(padre.ChildNodes[0]);
            }
            return null;
        }


        //LLAMADA2 ES PARA FUNCIONES
        public static NodoExp LLAMADA2(ParseTreeNode padre)
        {
            if(padre.ChildNodes.Count == 4)
            {
                LLamada llama = new LLamada(null, padre.ChildNodes[0].Token.Location.Line,
                padre.ChildNodes[0].Token.Location.Column);
                llama.Id = padre.ChildNodes[0].Token.Text;
                llama.Paramss = LISTA_VALORES(padre.ChildNodes[2]);
                llama.Tipo = tabla_simbolos.Simbolo.FUNCION;
                return llama;
            }
            else
            {
                LLamada llama = new LLamada(null, padre.ChildNodes[0].Token.Location.Line,
                padre.ChildNodes[0].Token.Location.Column);
                llama.Id = padre.ChildNodes[0].Token.Text;
                llama.Paramss = new List<NodoExp>();
                llama.Tipo = tabla_simbolos.Simbolo.FUNCION;
                return llama;
            }
        }
        #endregion
    }
}
