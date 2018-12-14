using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.analizadores.xml
{
    class DbGrammar : Grammar
    {
        NonTerminal PROCEDURE = new NonTerminal("PROCEDURE"),
            FUNCTION = new NonTerminal("FUNCTION"),
            OBJECT = new NonTerminal("OBJECT"),
            TABLA = new NonTerminal("TABLA"),
            COMPLE_FOR = new NonTerminal("COMPLE_FOR"),
            ROWS = new NonTerminal("ROWS"),
            USUARIOS = new NonTerminal("USUARIOS"),
            DB = new NonTerminal("DB"),
            LISTA = new NonTerminal("LISTA"),
            ATRIBUTO = new NonTerminal("ATRIBUTO"),
            ATRIBUTOS = new NonTerminal("ATRIBUTOS"),
            USUARIO = new NonTerminal("USUARIO"),
            ATRI = new NonTerminal("ATRI_TABLA"),
            NOMBRE = new NonTerminal("NOMBRE"),
            LISTA_USER = new NonTerminal("LISTA_USER"),
            ETIQUETA = new NonTerminal("ETIQUETA"),
            ETIQUETAS = new NonTerminal("ETIQUETAS"),
            PATH = new NonTerminal("PATH");

        public DbGrammar() : base(false)
        {
            StringLiteral texto = new StringLiteral("texto", "~", StringOptions.AllowsAllEscapes);
            IdentifierTerminal id = new IdentifierTerminal("id");
            RegexBasedTerminal variable = new RegexBasedTerminal("variable", "@[A-Za-z][A-Za-z0-9_]*");

            Terminal
                nombre = ToTerm("nombre"),
                path = ToTerm("path"),
                usuario = ToTerm("usuario"),
                procedure = ToTerm("procedure"),
                function = ToTerm("function"),
                objectt = ToTerm("object"),
                tabla = ToTerm("tabla"),
                complemento = ToTerm("complemento"),
                foranea = ToTerm("foranea"),
                rows = ToTerm("rows"),
                usuarios = ToTerm("usuarios"),

                ti = ToTerm("<"),
                tf = ToTerm(">"),
                punto = ToTerm("."),
                tc = ToTerm("/");

            LISTA.Rule = MakeStarRule(LISTA, DB)
                    ;

            DB.Rule = PROCEDURE
                    | FUNCTION
                    | OBJECT
                    | TABLA
                    | USUARIOS
                    | SyntaxError + ti + tc + procedure + tf
                    | SyntaxError + ti + tc + function + tf
                    | SyntaxError + ti + tc + objectt + tf
                    | SyntaxError + ti + tc + tabla + tf
                    ;

            PROCEDURE.Rule = ti + procedure + tf + PATH + ti + tc + procedure + tf;

            FUNCTION.Rule = ti + function + tf + PATH + ti + tc + function + tf;

            OBJECT.Rule = ti + objectt + tf + PATH + ti + tc + objectt + tf;

            USUARIOS.Rule = ti + usuarios + tf + LISTA_USER + ti + tc + usuarios + tf;

            TABLA.Rule = ti + tabla + tf + ETIQUETAS + ti + tc + tabla + tf;

            ETIQUETAS.Rule = MakePlusRule(ETIQUETAS, ETIQUETA);

            ETIQUETA.Rule = NOMBRE
                            | PATH
                            | ROWS
                            | USUARIOS
                            ;

            ROWS.Rule = ti + rows + tf + ATRIBUTOS + ti + tc + rows + tf;

            ATRIBUTOS.Rule = MakePlusRule(ATRIBUTOS, ATRI);

            ATRI.Rule = ATRIBUTO + COMPLE_FOR;

            COMPLE_FOR.Rule = ti + complemento + tf + id + ti + tc + complemento + tf
                            | ti + foranea + tf + id + punto + id + ti + tc + foranea + tf;

            LISTA_USER.Rule = MakePlusRule(LISTA_USER, USUARIO);

            USUARIO.Rule = ti + usuario + tf + id + ti + tc + usuario + tf;

            PATH.Rule = ti + path + tf + texto + ti + tc + path + tf;

            NOMBRE.Rule = ti + nombre + tf + id + ti + tc + nombre + tf;

            ATRIBUTO.Rule = ti + id + tf + id + ti + tc + id + tf;

            this.Root = LISTA;
        }
    }
}
