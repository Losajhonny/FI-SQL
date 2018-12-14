using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.analizadores.xml
{
    class MasterGrammar : Grammar
    {
        NonTerminal MAESTRO = new NonTerminal("MAESTRO"),
            LISTA = new NonTerminal("LISTA"),
            DB = new NonTerminal("DB"),
            USUARIO = new NonTerminal("USUARIO"),
            NOMBRE = new NonTerminal("NOMBRE"),
            PASSWORD = new NonTerminal("PASSWORD"),
            PATH = new NonTerminal("PATH");

        public MasterGrammar() : base(false)
        {
            StringLiteral texto = new StringLiteral("texto", "~", StringOptions.AllowsAllEscapes);
            IdentifierTerminal id = new IdentifierTerminal("id");
            RegexBasedTerminal variable = new RegexBasedTerminal("variable", "@[A-Za-z][A-Za-z0-9_]*");

            Terminal 
                nombre = ToTerm("nombre"),
                path = ToTerm("path"),
                db = ToTerm("db"),
                password = ToTerm("password"),
                usuario = ToTerm("usuario"),

                ti = ToTerm("<"),
                tf = ToTerm(">"),
                tc = ToTerm("/");

            LISTA.Rule = MakeStarRule(LISTA, MAESTRO);

            MAESTRO.Rule = DB
                        | USUARIO
                        | SyntaxError + ti + tc + db + tf
                        | SyntaxError + ti + tc + usuario + tf;

            DB.Rule = ti + db + tf + NOMBRE + PATH + ti + tc + db + tf;

            USUARIO.Rule = ti + usuario + tf + NOMBRE + PASSWORD + ti + tc + usuario + tf;

            PATH.Rule = ti + path + tf + texto + ti + tc + path + tf;

            NOMBRE.Rule = ti + nombre + tf + id + ti + tc + nombre + tf;

            PASSWORD.Rule = ti + password + tf + texto + ti + tc + password + tf;

            this.Root = LISTA;
        }
    }
}
