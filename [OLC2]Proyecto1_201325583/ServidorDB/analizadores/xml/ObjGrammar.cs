using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.analizadores.xml
{
    class ObjGrammar : Grammar
    {
        NonTerminal
            OBJ = new NonTerminal("OBJ"),
            LISTA = new NonTerminal("LISTA"),
            ATTR = new NonTerminal("ATTR"),
            USUARIOS = new NonTerminal("USUARIOS"),
            ATRIBUTO = new NonTerminal("ATRIBUTO"),
            ATRIBUTOS = new NonTerminal("ATRIBUTOS"),
            USUARIO = new NonTerminal("USUARIO"),
            LOBJ = new NonTerminal("LOBJ"),
            COBJ = new NonTerminal("COBJ"),
            NOMBRE = new NonTerminal("NOMBRE"),
            LISTA_USER = new NonTerminal("LISTA_USER");

        public ObjGrammar() : base(false)
        {
            StringLiteral texto = new StringLiteral("texto", "~", StringOptions.AllowsAllEscapes);
            IdentifierTerminal id = new IdentifierTerminal("id");
            RegexBasedTerminal variable = new RegexBasedTerminal("variable", "@[A-Za-z][A-Za-z0-9_]*");

            Terminal
                nombre = ToTerm("nombre"),
                usuario = ToTerm("usuario"),
                usuarios = ToTerm("usuarios"),
                attr = ToTerm("attr"),
                obj = ToTerm("obj"),

                ti = ToTerm("<"),
                tf = ToTerm(">"),
                tc = ToTerm("/");

            LISTA.Rule = MakeStarRule(LISTA, OBJ)
                | SyntaxError + ti + tc + obj + tf;

            OBJ.Rule = ti + obj + tf + LOBJ + ti + tc + obj + tf
                ;

            LOBJ.Rule = MakePlusRule(LOBJ, COBJ);

            COBJ.Rule = NOMBRE
                        | ATTR
                        | USUARIOS
                        ;

            USUARIOS.Rule = ti + usuarios + tf + LISTA_USER + ti + tc + usuarios + tf;

            NOMBRE.Rule = ti + nombre + tf + id + ti + tc + nombre + tf;

            LISTA_USER.Rule = MakePlusRule(LISTA_USER, USUARIO);

            USUARIO.Rule = ti + usuario + tf + id + ti + tc + usuario + tf;

            ATTR.Rule = ti + attr + tf + ATRIBUTOS + ti + tc + attr + tf;

            ATRIBUTOS.Rule = MakePlusRule(ATRIBUTOS, ATRIBUTO);

            ATRIBUTO.Rule = ti + id + tf + id + ti + tc + id + tf;

            this.Root = LISTA;
        }
    }
}
