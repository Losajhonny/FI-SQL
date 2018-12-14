using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.analizadores.xml
{
    class RegGrammar : Grammar
    {
        NonTerminal ROW = new NonTerminal("ROW"),
            ATRIBUTOS = new NonTerminal("ATRIBUTOS"),
            ATRIBUTO = new NonTerminal("ATRIBUTO"),
            ROWS = new NonTerminal("ROWS");

        public RegGrammar() : base(false)
        {
            StringLiteral texto = new StringLiteral("texto", "~", StringOptions.AllowsAllEscapes);
            IdentifierTerminal id = new IdentifierTerminal("id");
            RegexBasedTerminal variable = new RegexBasedTerminal("variable", "@[A-Za-z][A-Za-z0-9_]*");


            Terminal row = ToTerm("row"),
                ti = ToTerm("<"),
                tf = ToTerm(">"),
                tc = ToTerm("/");

            ROWS.Rule = MakeStarRule(ROWS, ROW)
                ;

            ROW.Rule = ti + row + tf + ATRIBUTOS + ti + tc + row + tf
                | SyntaxError + ti + tc + row + tf;

            ATRIBUTOS.Rule = MakePlusRule(ATRIBUTOS, ATRIBUTO);

            ATRIBUTO.Rule = ti + id + tf + texto + ti + tc + id + tf;

            this.Root = ROWS;
        }
    }
}
