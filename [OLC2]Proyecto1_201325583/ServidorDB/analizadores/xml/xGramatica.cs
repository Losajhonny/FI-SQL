using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.analizadores.xml
{
    class xGramatica : Grammar
    {
        #region NO TERMINALES
        NonTerminal MAESTRO = new NonTerminal("MAESTRO"),
            INIT = new NonTerminal("INIT"),
            DB = new NonTerminal("DB"),
            MDB = new NonTerminal("MDB"),
            NOMBRE = new NonTerminal("NOMBRE"),
            PATH = new NonTerminal("PATH"),
            USUARIO = new NonTerminal("USUARIO"),
            PASSWORD = new NonTerminal("PASSWORD"),
            ETIQUETA_MASTER = new NonTerminal("ETIQUETA_MASTER"),

            ETIQUETAS_DB = new NonTerminal("ETIQUETAS_DB"),

            PROCEDURE = new NonTerminal("PROCEDURE"),
            FUNCTION = new NonTerminal("FUNCTION"),
            OBJECT = new NonTerminal("OBJECT"),

            TABLA = new NonTerminal("TABLA"),
            ETIQUETA_TABLA = new NonTerminal("ETIQUETA_TABLA"),
            CUERPO_TABLA = new NonTerminal("CUERPO_TABLA"),
            ROWS = new NonTerminal("ROWS"),
            ATRIS_TABLA = new NonTerminal("ATRIS_TABLA"),
            ATRI_TABLA = new NonTerminal("ATRI_TABLA"),
            OPCION_COMP = new NonTerminal("OPCION_COMP"),

            LISTA_FUNCION = new NonTerminal("LISTA_FUNCION"),
            FUNCION = new NonTerminal("FUNCION"),
            CUERPO_FUNCION = new NonTerminal("CUERPO_FUNCION"),
            ETIQUETA_FUNCION = new NonTerminal("ETIQUETA_FUNCION"),
            RETORNO = new NonTerminal("RETORNO"),
            ATTR_VAL = new NonTerminal("ATTR_VAL"),

            ROW = new NonTerminal("ROW"),
            REGISTRO = new NonTerminal("REGISTRO"),

            CUERPO_PROCEDIMIENTO = new NonTerminal("CUERPO_PROCEDIMIENTO"),
            ETIQUETA_PROCEDIMIENTO = new NonTerminal("ETIQUETA_PROCEDIMIENTO"),
            PROCEDIMIENTO = new NonTerminal("PROCEDIMIENTO"),
            LISTA_PROCEDIMIENTO = new NonTerminal("LISTA_PROCEDIMIENTO"),
            PARAMS = new NonTerminal("PARAMS"),
            VARIABLES = new NonTerminal("VARIABLES"),
            VARIABLE = new NonTerminal("VARIABLE"),
            SRC = new NonTerminal("SRC"),
            
            OBJETO = new NonTerminal("OBJETO"),
            ETIQUETA_OBJETO = new NonTerminal("ETIQUETA_OBJETO"),
            CUERPO_OBJETO =new NonTerminal("CUERPO_OBJETO"),
            LISTA_OBJETO = new NonTerminal("LISTA_OBJETO"),
            ATTR = new NonTerminal("ATTR"),
            USER = new NonTerminal("USER"),
            LISTA_USER = new NonTerminal("LISTA_USER"),
            USUARIOS = new NonTerminal("USUARIOS"),
            ATRIBUTO = new NonTerminal("ATRIBUTO"),
            ATRIBUTOS = new NonTerminal("ATRIBUTOS"),
            REGISTROS = new NonTerminal("REGISTROS")
            
            
            ;
        #endregion

        public xGramatica() : base(false)
        {
            StringLiteral text = new StringLiteral("text", "~", StringOptions.AllowsAllEscapes);
            IdentifierTerminal id = new IdentifierTerminal("id");
            RegexBasedTerminal variable = new RegexBasedTerminal("variable", "@[A-Za-z][A-Za-z0-9_]*");

            #region TERMINALES
            Terminal db = ToTerm("db"),
                nombre = ToTerm("nombre"),
                path = ToTerm("path"),
                usuario = ToTerm("usuario"),
                password = ToTerm("password"),

                obj = ToTerm("obj"),
                attr = ToTerm("attr"),
                usuarios = ToTerm("usuarios"),

                fun = ToTerm("fun"),
                retorno = ToTerm("retorno"),

                proc = ToTerm("proc"),
                paramss = ToTerm("params"),
                src = ToTerm("src"),
                row = ToTerm("row"),

                procedure = ToTerm("procedure"),
                function = ToTerm("function"),
                objectt = ToTerm("object"),
                tabla = ToTerm("tabla"),
                rows = ToTerm("rows"),
                complemento = ToTerm("complemento"),
                foranea = ToTerm("foranea"),

                punto = ToTerm("."),
                ti = ToTerm("<"),
                tf = ToTerm(">"),
                tc = ToTerm("/");
            #endregion

            INIT.Rule = MAESTRO
                      | DB
                      | LISTA_OBJETO
                      | LISTA_PROCEDIMIENTO
                      | LISTA_FUNCION
                      | REGISTROS  //REGISTROS DE UNA TABLA
                      ;


            /*====================================================*/
            MAESTRO.Rule = MakePlusRule(MAESTRO, ETIQUETA_MASTER);

            ETIQUETA_MASTER.Rule = MDB
                        | USUARIO
                        ;

            MDB.Rule = ti + db + tf + NOMBRE + PATH + ti + tc + db + tf;

            USUARIO.Rule = ti + usuario + tf + NOMBRE + PASSWORD + ti + tc + usuario + tf;
            /*====================================================*/



            PASSWORD.Rule = ti + password + tf + id + ti + tc + password + tf;

            NOMBRE.Rule = ti + nombre + tf + id + ti + tc + nombre + tf;

            PATH.Rule = ti + path + tf + text + ti + tc + path + tf;



            /*====================================================*/
            LISTA_OBJETO.Rule = MakePlusRule(LISTA_OBJETO, OBJETO);

            OBJETO.Rule = ti + obj + tf + CUERPO_OBJETO + ti + tc + obj + tf;

            CUERPO_OBJETO.Rule = MakePlusRule(CUERPO_OBJETO, ETIQUETA_OBJETO);

            ETIQUETA_OBJETO.Rule = NOMBRE
                                | ATTR
                                | USUARIOS
                                ;

            USUARIOS.Rule = ti + usuarios + tf + LISTA_USER + ti + tc + usuarios + tf;

            ATTR.Rule = ti + attr + tf + ATRIBUTOS + ti + tc + attr + tf;
            /*====================================================*/



            /*====================================================*/
            LISTA_PROCEDIMIENTO.Rule = MakePlusRule(LISTA_PROCEDIMIENTO, PROCEDIMIENTO);

            PROCEDIMIENTO.Rule = ti + proc + tf + CUERPO_PROCEDIMIENTO + ti + tc + proc + tf;

            CUERPO_PROCEDIMIENTO.Rule = MakePlusRule(CUERPO_PROCEDIMIENTO, ETIQUETA_PROCEDIMIENTO);

            ETIQUETA_PROCEDIMIENTO.Rule = NOMBRE
                                    | PARAMS
                                    | USUARIOS
                                    | SRC
                                    ;
            /*====================================================*/



            /*====================================================*/
            LISTA_FUNCION.Rule = MakePlusRule(LISTA_FUNCION, FUNCION);

            FUNCION.Rule = ti + fun + tf + CUERPO_FUNCION + ti + tc + fun + tf;

            CUERPO_FUNCION.Rule = MakePlusRule(CUERPO_FUNCION, ETIQUETA_FUNCION);

            ETIQUETA_FUNCION.Rule = NOMBRE
                            | PARAMS
                            | USUARIOS
                            | RETORNO
                            | SRC
                            ;

            RETORNO.Rule = ti + retorno + tf + id + ti + tc + retorno + tf;
            /*====================================================*/




            /*====================================================*/
            DB.Rule = MakePlusRule(DB, ETIQUETAS_DB);

            ETIQUETAS_DB.Rule = PROCEDURE
                            | FUNCION
                            | OBJECT
                            | TABLA
                            ;

            PROCEDURE.Rule = ti + procedure + tf + PATH + ti + tc + procedure + tf;

            FUNCTION.Rule = ti + function + tf + PATH + ti + tc + function + tf;

            OBJECT.Rule = ti + objectt + tf + PATH + ti + tc + objectt + tf;

            TABLA.Rule = ti + tabla + tf + CUERPO_TABLA + ti + tc + tf;

            CUERPO_TABLA.Rule = MakePlusRule(CUERPO_TABLA, ETIQUETA_TABLA);

            ETIQUETA_TABLA.Rule = NOMBRE
                            | PATH
                            | ROWS
                            ;

            ROWS.Rule = ti + rows + tf + ATRIS_TABLA + ti + tc + rows + tf;

            ATRIS_TABLA.Rule = MakePlusRule(ATRIS_TABLA, ATRI_TABLA);

            ATRI_TABLA.Rule = ATRIBUTO + OPCION_COMP;

            OPCION_COMP.Rule = ti + complemento + tf + id + ti + tc + complemento + tf
                            | ti + foranea + tf + id + punto + id + ti + tc + foranea + tf;
            /*====================================================*/





            /*POR LO MENOS UN USUARIO TIENE PERMISOS EN TODOS LOS OBJETOS POR EL ADMINISTRADOR*/
            LISTA_USER.Rule = MakePlusRule(LISTA_USER, USER);

            USER.Rule = ti + usuario + tf + id + ti + tc + usuario + tf;

            /*POR LO MENOS UN ATRIBUTO DEBE EXISTIR EN LOS OBJETOS Y TABLAS*/
            ATRIBUTOS.Rule = MakePlusRule(ATRIBUTOS, ATRIBUTO);

            ATRIBUTO.Rule = ti + id + tf + id + ti + tc + id + tf;

            SRC.Rule = ti + src + tf + text + ti + tc + src + tf;

            PARAMS.Rule = ti + paramss + tf + VARIABLES + ti + tc + paramss + tf;

            VARIABLES.Rule = MakePlusRule(VARIABLES, VARIABLE);

            VARIABLE.Rule = ti + id + tf + variable + ti + tc + id + tf;



            REGISTROS.Rule = MakePlusRule(REGISTROS, ROW); 

            ROW.Rule = ti + row + tf + REGISTRO + ti + tc + row + tf;

            REGISTRO.Rule = MakePlusRule(REGISTRO, ATTR_VAL);

            ATTR_VAL.Rule = ti + id + tf + text + ti + tc + id + tf;


            this.Root = INIT;
        }
    }
}
