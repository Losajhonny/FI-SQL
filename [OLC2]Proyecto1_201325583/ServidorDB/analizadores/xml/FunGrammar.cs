using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.analizadores.xml
{
    class FunGrammar : Grammar
    {
        NonTerminal
            FUNCION = new NonTerminal("FUNCION"),
            LISTA = new NonTerminal("LISTA"),
            PARAMS = new NonTerminal("PARAMS"),
            USUARIOS = new NonTerminal("USUARIOS"),
            ATRIBUTO = new NonTerminal("ATRIBUTO"),
            ATRIBUTOS = new NonTerminal("ATRIBUTOS"),
            USUARIO = new NonTerminal("USUARIO"),
            SRC = new NonTerminal("SRC"),
            LFUNCION = new NonTerminal("LFUNCION"),
            CFUNCION = new NonTerminal("CFUNCION"),
            RETORNO = new NonTerminal("RETORNO"),
            NOMBRE = new NonTerminal("NOMBRE"),
            LISTA_USER = new NonTerminal("LISTA_USER");

        public FunGrammar() : base(false)
        {
            StringLiteral texto = new StringLiteral("texto", "~", StringOptions.AllowsAllEscapes);
            IdentifierTerminal id = new IdentifierTerminal("id");
            RegexBasedTerminal variable = new RegexBasedTerminal("variable", "@[A-Za-z][A-Za-z0-9_]*");

            Terminal
                nombre = ToTerm("nombre"),
                usuario = ToTerm("usuario"),
                usuarios = ToTerm("usuarios"),
                paramss = ToTerm("params"),
                fun = ToTerm("fun"),
                retorno = ToTerm("retorno"),
                src = ToTerm("src"),

                ti = ToTerm("<"),
                tf = ToTerm(">"),
                tc = ToTerm("/");

            LISTA.Rule = MakeStarRule(LISTA, FUNCION)
                ;

            FUNCION.Rule = ti + fun + tf + LFUNCION + ti + tc + fun + tf
                | SyntaxError + ti + tc + fun + tf
                ;

            LFUNCION.Rule = MakePlusRule(LFUNCION, CFUNCION);

            CFUNCION.Rule = NOMBRE
                        | PARAMS
                        | USUARIOS
                        | RETORNO
                        | SRC
                        ;

            USUARIOS.Rule = ti + usuarios + tf + LISTA_USER + ti + tc + usuarios + tf;

            NOMBRE.Rule = ti + nombre + tf + id + ti + tc + nombre + tf;

            LISTA_USER.Rule = MakePlusRule(LISTA_USER, USUARIO);

            USUARIO.Rule = ti + usuario + tf + id + ti + tc + usuario + tf;

            PARAMS.Rule = ti + paramss + tf + ATRIBUTOS + ti + tc + paramss + tf;

            ATRIBUTOS.Rule = MakePlusRule(ATRIBUTOS, ATRIBUTO);

            ATRIBUTO.Rule = ti + id + tf + variable + ti + tc + id + tf;

            RETORNO.Rule = ti + retorno + tf + id + ti + tc + retorno + tf;

            SRC.Rule = ti + src + tf + texto + ti + tc + src + tf;

            this.Root = LISTA;
        }
    }
}
