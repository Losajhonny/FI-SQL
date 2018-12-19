using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.analizadores.usql
{
    class uGramatica : Grammar
    {
        public string[] word_reserved = { "text", "integer", "double", "bool", "date", "datetime", "crear", "base_datos", "tabla",
                "objeto", "procedimiento", "funcion", "retorno", "usuario", "colocar", "password", "usar", "agregar",
                "alterar", "quitar", "cambiar", "eliminar", "insertar", "en", "actualizar", "donde", "valores", "order_por", "borrar",
                "asc", "desc", "seleccionar", "de", "otorgar", "permisos", "denegar", "declarar", "si", "sino", "selecciona", "caso",
                "defecto", "para", "mientras", "detener", "imprimir", "fecha", "fecha_hora", "contar", "backup", "usqldump", "completo",
                "restaurar", "no", "nulo", "autoincrementable", "llave_primaria", "llave_foranea", "unico"
                };

        #region NO TERMINALES
        NonTerminal EXP = new NonTerminal("EXP"),
            DDL = new NonTerminal("DDL"),
            DML = new NonTerminal("DML"),
            DCL = new NonTerminal("DCL"),
            SSL = new NonTerminal("SSL"),
            SENTENCIA = new NonTerminal("SENTENCIA"),
            SENTENCIAS = new NonTerminal("SENTENCIAS"),
            
            OPERACION = new NonTerminal("OPERACION"),
            CREATE = new NonTerminal("CREATE"),
            ALTER = new NonTerminal("ALTER"),
            DROP = new NonTerminal("DROP"),
            SELECT = new NonTerminal("SELECT"),
            FUNCIONES_SELECT = new NonTerminal("FUNCIONES_SELECT"),
            UPDATE = new NonTerminal("UPDATE"),
            INSERT = new NonTerminal("INSERT"),
            DELETE = new NonTerminal("DELETE"),
            CAMPOS_TABLA = new NonTerminal("CAMPOS_TABLA"),
            CAMPO_TABLA = new NonTerminal("CAMPO_TABLA"),
            CAMPOS_OBJETO = new NonTerminal("CAMPOS_OBJETO"),
            CAMPO_OBJETO = new NonTerminal("CAMPO_OBJETO"),
            TIPO_DATO = new NonTerminal("TIPO_DATO"),
            TIPO_DATO_PR = new NonTerminal("TIPO_DATO_PR"),
            COMPLEMENTO = new NonTerminal("COMPLEMENTO"),
            PARAMETROS = new NonTerminal("PARAMETROS"),
            PARAMETRO = new NonTerminal("PARAMETRO"),
            LLAMADA = new NonTerminal("LLAMADA"),
            ARITMETICA = new NonTerminal("ARITMETICA"),
            RELACIONAL = new NonTerminal("RELACIONAL"),
            LOGICA = new NonTerminal("LOGICA"),
            PRIMITIVOS = new NonTerminal("PRIMITIVOS"),
            VARIABLES = new NonTerminal("VARIABLES"),
            FUNCIONES = new NonTerminal("FUNCIONES"),

            DECLARACION = new NonTerminal("DECLARACION"),
            ASIGNACION = new NonTerminal("ASIGNACION"),
            SI = new NonTerminal("SI"),
            SELECCIONA = new NonTerminal("SELECCIONA"),
            CASOS = new NonTerminal("CASOS"),
            CASO = new NonTerminal("CASO"),
            DEFECTO = new NonTerminal("DEFECTO"),
            PARA = new NonTerminal("PARA"),
            MIENTRAS = new NonTerminal("MIENTRAS"),
            IMPRIMIR = new NonTerminal("IMPRIMIR"),
            CONTAR = new NonTerminal("CONTAR"),

            BACKUP = new NonTerminal("BACKUP"),
            RESTORE = new NonTerminal("RESTORE"),

            LISTA_VARIABLES = new NonTerminal("LISTA_VARIABLES"),
            LISTA_VALORES = new NonTerminal("LISTA_VALORES"),            
            LISTA_ID = new NonTerminal("LISTA_ID"),
            OBJETO_USQL = new NonTerminal("OBJETO_USQL");
        #endregion

        public uGramatica() : base(false)
        {
            MarkReservedWords(word_reserved);

            #region EXPRESION REGULAR
            CommentTerminal comment_line = new CommentTerminal("comment_line", "#", "\n", "\r\n");
            CommentTerminal comment_block = new CommentTerminal("comment_block", "#*", "*#");
            NonGrammarTerminals.Add(comment_line);
            NonGrammarTerminals.Add(comment_block);

            NumberLiteral integer = new NumberLiteral("integer");
            StringLiteral text = new StringLiteral("text", "\"", StringOptions.AllowsAllEscapes);
            RegexBasedTerminal double_ = new RegexBasedTerminal("double", @"[0-9]+[\.][0-9]+");
            RegexBasedTerminal variable = new RegexBasedTerminal("variable", "@[A-Za-z][A-Za-z0-9_]*");
            IdentifierTerminal id = new IdentifierTerminal("id");
            RegexBasedTerminal date = new RegexBasedTerminal("date", "[0-9][0-9][-][0-9][0-9][-][0-9][0-9][0-9][0-9]");
            RegexBasedTerminal datetime = new RegexBasedTerminal("datetime", "[0-9][0-9][-][0-9][0-9][-][0-9][0-9][0-9][0-9][ ][0-9][0-9][:][0-9][0-9][:][0-9][0-9]");
            #endregion

            #region TERMINALES
            Terminal pr_text = ToTerm("text"),
                pr_integer = ToTerm("integer"),
                pr_double = ToTerm("double"),
                pr_bool = ToTerm("bool"),
                pr_date = ToTerm("date"),
                pr_datetime = ToTerm("datetime"),

                pr_crear = ToTerm("crear"),
                pr_base_datos = ToTerm("base_datos"),
                pr_tabla = ToTerm("tabla"),
                pr_objeto = ToTerm("objeto"),
                pr_procedimiento = ToTerm("procedimiento"),
                pr_funcion = ToTerm("funcion"),
                pr_retorno = ToTerm("retorno"),
                pr_usuario = ToTerm("usuario"),
                pr_colocar = ToTerm("colocar"),
                pr_password = ToTerm("password"),
                pr_usar = ToTerm("usar"),
                pr_agregar = ToTerm("agregar"),
                pr_alterar = ToTerm("alterar"),
                pr_quitar = ToTerm("quitar"),
                pr_cambiar = ToTerm("cambiar"),
                pr_eliminar = ToTerm("eliminar"),
                pr_insertar = ToTerm("insertar"),
                pr_en = ToTerm("en"),
                pr_actualizar = ToTerm("actualizar"),
                pr_donde = ToTerm("donde"),
                pr_valores = ToTerm("valores"),
                pr_borrar = ToTerm("borrar"),
                pr_ordenar_por = ToTerm("ordenar_por"),
                pr_asc = ToTerm("asc"),
                pr_desc = ToTerm("desc"),
                pr_seleccionar = ToTerm("seleccionar"),
                pr_de = ToTerm("de"),
                pr_otorgar = ToTerm("otorgar"),
                pr_permisos = ToTerm("permisos"),
                pr_denegar = ToTerm("denegar"),
                pr_declarar = ToTerm("declarar"),
                pr_si = ToTerm("si"),
                pr_sino = ToTerm("sino"),
                pr_selecciona = ToTerm("selecciona"),
                pr_caso = ToTerm("caso"),
                pr_defecto = ToTerm("defecto"),
                pr_para = ToTerm("para"),
                pr_mientras = ToTerm("mientras"),
                pr_detener = ToTerm("detener"),
                pr_imprimir = ToTerm("imprimir"),
                pr_fecha = ToTerm("fecha"),
                pr_fecha_hora = ToTerm("fecha_hora"),
                pr_contar = ToTerm("contar"),

                pr_backup = ToTerm("backup"),
                pr_usqldump = ToTerm("usqldump"),
                pr_completo = ToTerm("completo"),
                pr_restaurar = ToTerm("restaurar"),

                pr_no = ToTerm("no"),
                pr_nulo = ToTerm("nulo"),
                pr_autoincrementable = ToTerm("autoincrementable"),
                pr_llave_primaria = ToTerm("llave_primaria"),
                pr_llave_foranea = ToTerm("llave_foranea"),
                pr_unico = ToTerm("unico"),

                pari = ToTerm("("),
                pard = ToTerm(")"),
                llai = ToTerm("{"),
                llad = ToTerm("}"),
                ptcoma = ToTerm(";"),
                coma = ToTerm(","),
                igual = ToTerm("="),
                punto = ToTerm("."),
                dospuntos = ToTerm(":"),
                incremento = ToTerm("++"),
                decremento = ToTerm("--"),
                dmenor = ToTerm("<<"),
                dmayor = ToTerm(">>"),

                mas = ToTerm("+"),
                menos = ToTerm("-"),
                por = ToTerm("*"),
                div = ToTerm("/"),
                pot = ToTerm("^"),

                digual = ToTerm("=="),
                diferente = ToTerm("!="),
                menor = ToTerm("<"),
                mayor = ToTerm(">"),
                menigual = ToTerm("<="),
                mayigual = ToTerm(">="),

                and = ToTerm("&&"),
                or = ToTerm("||"),
                not = ToTerm("!");
            #endregion

            #region SENTENCIAS DDL
            DDL.Rule = CREATE
                    | ALTER + ptcoma
                    | DROP + ptcoma
                    ;

            /*--------------------- MODULO DE ELIMINAR --------------------------*/
            DROP.Rule = pr_eliminar + OBJETO_USQL + id
                        ;

            OBJETO_USQL.Rule = pr_tabla
                            | pr_base_datos
                            | pr_objeto
                            | pr_usuario
                            | pr_procedimiento
                            | pr_funcion
                            ;

            /*-------------------- MODULO DE ALTERAR ----------------------------*/
            ALTER.Rule =
                //ALTERAR TABLA
                pr_alterar + pr_tabla + id + pr_agregar + pari + CAMPOS_TABLA + pard
                | pr_alterar + pr_tabla + id + pr_quitar + LISTA_ID
                //ALTERAR OBJETO
                | pr_alterar + pr_objeto + id + pr_agregar + pari + CAMPOS_OBJETO + pard
                | pr_alterar + pr_objeto + id + pr_quitar + LISTA_ID
                //ALTERAR USUARIO
                | pr_alterar + pr_usuario + id + pr_cambiar + pr_password + igual + EXP
                ;

            /*--------------------------- MODULO DE CREAR ------------------------*/
            CREATE.Rule =
                //CREAR BASE DE DATOS
                pr_crear + pr_base_datos + id + ptcoma
                //CREAR UNA TABLA
                | pr_crear + pr_tabla + id + pari + CAMPOS_TABLA + pard + ptcoma
                //CREAR UN OBJETO
                | pr_crear + pr_objeto + id + pari + CAMPOS_OBJETO + pard + ptcoma
                //CREAR UN PROCEDIMIENTO
                | pr_crear + pr_procedimiento + id + pari + PARAMETROS + pard + llai + SENTENCIAS + llad
                //CREAR UNA FUNCION
                | pr_crear + pr_funcion + id + pari + PARAMETROS + pard + TIPO_DATO + llai + SENTENCIAS + llad
                //CREAR UN USUARIO
                | pr_crear + pr_usuario + id + pr_colocar + pr_password + igual + EXP + ptcoma
                //USAR BASE DE DATOS
                | pr_usar + id + ptcoma
                ;
            #endregion

            #region SENTENCIAS DML
            DML.Rule = SELECT + ptcoma
                    | UPDATE + ptcoma
                    | INSERT + ptcoma
                    | DELETE + ptcoma
                    ;

            /*--------------------- MODULO DE SELECCIONAR -----------------------*/
            SELECT.Rule = pr_seleccionar + LISTA_ID + pr_de + LISTA_ID + FUNCIONES_SELECT
                        | pr_seleccionar + por + pr_de + LISTA_ID + FUNCIONES_SELECT
                        | pr_seleccionar + LISTA_ID + pr_de + LISTA_ID
                        | pr_seleccionar + por + pr_de + LISTA_ID
                       ;

            FUNCIONES_SELECT.Rule = pr_donde + EXP
                                  | pr_donde + EXP + pr_ordenar_por + id
                                  | pr_donde + EXP + pr_ordenar_por + id + pr_asc
                                  | pr_donde + EXP + pr_ordenar_por + id + pr_desc
                                  | pr_ordenar_por + id
                                  | pr_ordenar_por + id + pr_asc
                                  | pr_ordenar_por + id + pr_desc
                                  ;

            /*----------------------- MODULO DE DELETE --------------------------*/
            DELETE.Rule = pr_borrar + pr_en + pr_tabla + id + pr_donde + EXP
                    | pr_borrar + pr_en + pr_tabla + id;

            /*-------------------- MODULO DE UPDATE ------------------------------*/
            UPDATE.Rule = pr_actualizar + pr_tabla + id + pari + LISTA_ID + pard + pr_valores +
                          pari + LISTA_VALORES + pard + pr_donde + EXP
                        | pr_actualizar + pr_tabla + id + pari + LISTA_ID + pard + pr_valores +
                          pari + LISTA_VALORES + pard;

            /*--------------------- MODULO DE INSERT -----------------------------*/
            INSERT.Rule = pr_insertar + pr_en + pr_tabla + id + pari + LISTA_ID + pard + pr_valores + pari + LISTA_VALORES + pard
                        | pr_insertar + pr_en + pr_tabla + id + pari + LISTA_VALORES + pard
                        ;
            #endregion

            #region SENTENCIAS DCL
            DCL.Rule = pr_otorgar + pr_permisos + id + coma + id + punto + id + ptcoma
                    | pr_otorgar + pr_permisos + id + coma + id + punto + por + ptcoma
                    | pr_denegar + pr_permisos + id + coma + id + punto + id + ptcoma
                    | pr_denegar + pr_permisos + id + coma + id + punto + por + ptcoma
                    ;
            #endregion

            #region SENTENCIAS SSL
            SSL.Rule = 
                      DECLARACION + ptcoma
                    | ASIGNACION + ptcoma
                    | IMPRIMIR + ptcoma
                    | SI
                    | SELECCIONA
                    | PARA
                    | MIENTRAS
                    ;
            /*------------------------------- IMPRIMIR -------------------------*/
            IMPRIMIR.Rule = pr_imprimir + pari + EXP + pard;

            /*------------------------------- MIENTRAS -------------------------*/
            MIENTRAS.Rule = pr_mientras + pari + EXP + pard + llai + SENTENCIAS + llad;

            /*------------------------------- PARA -------------------------*/
            PARA.Rule = pr_para + pari + 
                pr_declarar + variable + pr_integer + igual + EXP + ptcoma + 
                EXP + 
                ptcoma + OPERACION + pard + 
                llai + SENTENCIAS + llad
                      ;

            OPERACION.Rule = incremento
                           | decremento
                           ;
            /*------------------------------- SELECCIONA ------------------------------*/
            SELECCIONA.Rule = pr_selecciona + pari + EXP + pard + llai + CASOS + DEFECTO + llad
                            | pr_selecciona + pari + EXP + pard + llai + CASOS + llad
                            ;

            CASOS.Rule = MakePlusRule(CASOS, CASO);

            CASO.Rule = pr_caso + EXP + dospuntos + SENTENCIAS;

            DEFECTO.Rule = pr_defecto + dospuntos + SENTENCIAS;

            /*------------------------------- SI ------------------------------*/
            SI.Rule = pr_si + pari + EXP + pard + llai + SENTENCIAS + llad
                    | pr_si + pari + EXP + pard + llai + SENTENCIAS + llad + pr_sino + llai + SENTENCIAS + llad
                    ;

            /*----------------------- ASIGNACION ---------------------------*/
            ASIGNACION.Rule =
                          variable + punto + id + igual + EXP
                        | variable + igual + EXP
                        ;

            /*----------------------- DECLARACION ---------------------------*/
            DECLARACION.Rule =
                      //DECLARACION DE VARIABLES
                      pr_declarar + LISTA_VARIABLES + TIPO_DATO + igual + EXP
                    | pr_declarar + LISTA_VARIABLES + TIPO_DATO
                    //DECLARACION DE OBJETOS
                    | pr_declarar + variable + id
                    ;
            #endregion

            #region BACKUP Y RESTORE
            /*------------------------------- RESTORE -------------------------*/
            RESTORE.Rule = pr_restaurar + pr_usqldump + EXP + ptcoma
                        | pr_restaurar + pr_completo + EXP + ptcoma
                        ;

            /*------------------------------- BACKUP -------------------------*/
            BACKUP.Rule = pr_backup + pr_usqldump + id + id + ptcoma
                        | pr_backup + pr_completo + id + id + ptcoma;
            #endregion

            #region LISTAS
            LISTA_VARIABLES.Rule = MakePlusRule(LISTA_VARIABLES, coma, variable);

            LISTA_ID.Rule = MakePlusRule(LISTA_ID, coma, id);

            LISTA_VALORES.Rule = MakePlusRule(LISTA_VALORES, coma, EXP);
            #endregion

            #region SENTENCIAS E INSTRUCCIONES
            /*------------------------ SENTENCIAS ---------------------------*/
            SENTENCIAS.Rule = MakeStarRule(SENTENCIAS, SENTENCIA);

            SENTENCIA.Rule = DDL
                           | DML
                           | DCL
                           | SSL
                           | BACKUP
                           | RESTORE
                           | LLAMADA + ptcoma
                           | pr_retorno + EXP + ptcoma
                           | pr_detener + ptcoma
                           ;
            #endregion

            #region OTROS
            /*------------------------------- CONTAR -------------------------*/
            CONTAR.Rule = pr_contar + pari + dmenor + SELECT + dmayor + pard;

            /*-------------------- LLAMADA A FUNCIONES O PROCEDIMIENTOS -------------*/
            LLAMADA.Rule = id + pari + LISTA_VALORES + pard
                                  | id + pari + pard;

            /*---------- PARAMETROS PARA PROCEDIMIENTOS Y FUNCIONES ---------------*/
            PARAMETROS.Rule = MakeStarRule(PARAMETROS, coma, PARAMETRO);

            PARAMETRO.Rule = TIPO_DATO_PR + variable;

            /*------------- CAMPOS Y COMPLEMENTOS DE UN OBJETO --------------------*/
            CAMPOS_OBJETO.Rule = MakePlusRule(CAMPOS_OBJETO, coma, CAMPO_OBJETO);

            CAMPO_OBJETO.Rule = TIPO_DATO_PR + id;

            /*------------- CAMPOS Y COMPLEMENTOS DE UNA TABLA --------------------*/
            CAMPOS_TABLA.Rule = MakePlusRule(CAMPOS_TABLA, coma, CAMPO_TABLA);

            CAMPO_TABLA.Rule = TIPO_DATO_PR + id + COMPLEMENTO
                            | TIPO_DATO_PR + id
                            ;

            COMPLEMENTO.Rule = pr_no + pr_nulo
                            | pr_nulo
                            | pr_autoincrementable
                            | pr_llave_primaria
                            | pr_llave_foranea + id + id
                            | pr_unico
                            ;
            #endregion

            #region TIPO DE DATO
            /*---------------------- TIPO DE DATO ----------------------------------*/
            TIPO_DATO.Rule = TIPO_DATO_PR
                            | id
                            ;

            //tipo de dato primitivos
            TIPO_DATO_PR.Rule = pr_text
                            | pr_integer
                            | pr_double
                            | pr_bool
                            | pr_date
                            | pr_datetime
                            ;
            #endregion

            #region EXPRESIONES
            /*---------------------- EXPRESIONES ----------------------------------*/
            EXP.Rule =
                       //PARENTESIS
                      pari + EXP + pard
                    //EXPRESIONES ARITMETICAS
                    | ARITMETICA
                       //EXPRESIONES RELACIONALES
                    | RELACIONAL
                       //EXPRESIONES LOGICAS
                    | LOGICA
                       //FUNCIONES QUE RETORNAN VALOR
                    | FUNCIONES
                       //PRIMITIVOS
                    | PRIMITIVOS
                       //VARIABLE
                    | VARIABLES
                    ;

            ARITMETICA.Rule = EXP + mas + EXP
                    | EXP + menos + EXP
                    | EXP + por + EXP
                    | EXP + div + EXP
                    | EXP + pot + EXP
                    | menos + EXP
                    ;

            RELACIONAL.Rule = EXP + digual + EXP
                    | EXP + diferente + EXP
                    | EXP + menor + EXP
                    | EXP + mayor + EXP
                    | EXP + menigual + EXP
                    | EXP + mayigual + EXP
                    ;

            LOGICA.Rule = EXP + and + EXP
                    | EXP + or + EXP
                    | not + EXP
                    ;

            PRIMITIVOS.Rule = integer
                    | text
                    | double_
                    | date
                    | datetime
                    ;

            FUNCIONES.Rule = pr_fecha + pari + pard
                    | pr_fecha_hora + pari + pard
                    | LLAMADA
                    | CONTAR
                    ;

            VARIABLES.Rule = variable  //variable normal
                    | id               //identificador
                    | id + punto + id  //para el select
                    | variable + punto + id  //para declaracion de objetos
                    ;

            #endregion

            #region PRECEDENCIA Y ASOCIATIVIDAD
            RegisterOperators(10, Associativity.Left, and);
            RegisterOperators(20, Associativity.Left, or);
            RegisterOperators(30, Associativity.Right, not);
            RegisterOperators(40, Associativity.Left, digual, diferente, mayor, menor,
                mayigual, menigual);
            RegisterOperators(50, Associativity.Left, mas, menos);
            RegisterOperators(60, Associativity.Left, por, div);
            RegisterOperators(70, Associativity.Right, pot);
            #endregion

            this.Root = SENTENCIAS;
        }
    }
}
