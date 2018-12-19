using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.analizadores.plycs
{
    class pGramatica : Grammar
    {
        NonTerminal INI = new NonTerminal("INI"),
            PAQUETE = new NonTerminal("PAQUETE"),
            PAQUETE_LOGIN = new NonTerminal("PAQUETE_LOGIN"),
            PAQUETE_FIN = new NonTerminal("PAQUETE_FIN"),
            PAQUETE_ERROR = new NonTerminal("PAQUETE_ERROR"),
            PAQUETE_INST = new NonTerminal("PAQUETE_INST"),
            //PAQUETE_REPORTE = new NonTerminal("PAQUETE_REPORTE"),
            BOOLEANO = new NonTerminal("BOOLEANO"),
            DATOS = new NonTerminal("DATOS"),
            FILAS = new NonTerminal("FILAS"),
            FILA = new NonTerminal("FILA"),
            CAMPOS = new NonTerminal("CAMPOS"),
            CAMPO = new NonTerminal("CAMPO")
            ;

        public pGramatica() : base(false)
        {
            NumberLiteral integer = new NumberLiteral("integer");
            StringLiteral text = new StringLiteral("text", "\"", StringOptions.AllowsAllEscapes);
            RegexBasedTerminal double_ = new RegexBasedTerminal("double", @"[0-9]+[\.][0-9]+");

            MarkReservedWords("\"paquete\"",
            	"\"paquete\"",
                "\"validar\"",
                "\"login\"",
                "\"usuario\"",
                "\"password\"",
                "\"usql\"",
                "\"instruccion\"",
                "true",
                "false",
                "\"datos\"",
                "\"fin\"",
                "\"tipo\"",
                "\"msg\"",
                "\"error\"");

            Terminal cori = ToTerm("["),
                cord = ToTerm("]"),
                dpts = ToTerm(":"),
                coma = ToTerm(","),
                paquete = ToTerm("\"paquete\""),
                validar = ToTerm("\"validar\""),
                login = ToTerm("\"login\""),
                username = ToTerm("\"usuario\""),
                password = ToTerm("\"password\""),
                usql = ToTerm("\"usql\""),
                instruccion = ToTerm("\"instruccion\""),
                pr_true = ToTerm("true"),
                pr_false = ToTerm("false"),
                datos = ToTerm("\"datos\""),
                fin = ToTerm("\"fin\""),
                tipo = ToTerm("\"tipo\""),
                msg = ToTerm("\"msg\""),
                error = ToTerm("\"error\"")
                ;

            INI.Rule = cori + validar + dpts + integer + coma
                + PAQUETE + cord;

            PAQUETE.Rule = PAQUETE_LOGIN
                | PAQUETE_FIN
                | PAQUETE_ERROR
                | PAQUETE_INST
                //| PAQUETE_REPORTE
                ;

            PAQUETE_ERROR.Rule = paquete + dpts + error + coma +
                tipo + dpts + text + coma +
                msg + dpts + text + coma +
                datos + dpts + FILAS;

            PAQUETE_LOGIN.Rule = login + dpts + cori +
                username + dpts + text + coma + password + dpts + text + cord
                | login + dpts + cori +
                username + dpts + text + coma + login + dpts + BOOLEANO + cord;

            BOOLEANO.Rule = pr_true
                | pr_false;

            PAQUETE_FIN.Rule = paquete + dpts + fin;


            PAQUETE_INST.Rule = paquete + dpts + usql + coma + instruccion + dpts + text
                | paquete + dpts + usql + coma + datos + dpts + DATOS;

            DATOS.Rule = cori + FILAS + cord
                | text;

            FILAS.Rule = MakePlusRule(FILAS, coma, FILA);

            FILA.Rule = cori + CAMPOS + cord;

            CAMPOS.Rule = MakePlusRule(CAMPOS, coma, CAMPO);

            CAMPO.Rule = text + dpts + text
                    | text + dpts + integer
                    | text + dpts + double_;

            this.Root = INI;
        }
    }
}
