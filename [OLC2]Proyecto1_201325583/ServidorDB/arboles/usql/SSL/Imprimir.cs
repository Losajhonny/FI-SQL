using ServidorDB.analizadores.usql;
using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql.SSL
{
    class Imprimir : uInstruccion
    {
        private NodoExp exp;
        private int line;
        private int colm;
        private string valor_ejecutado;

        public Imprimir(NodoExp exp, int line, int colm)
        {
            this.exp = exp;
            this.line = line;
            this.colm = colm;
        }

        public NodoExp Exp { get => exp; set => exp = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }
        public string Valor_ejecutado { get => valor_ejecutado; set => valor_ejecutado = value; }

        public object ejecutar(Entorno ent)
        {
            Resultado res = (Resultado)exp.ejecutar(ent);
            if(res.Tipo == Constante.TEXT)
            {
                Constante.informacion_consola += res.Valor + "\n";
                String fechahora = Convert.ToString(DateTime.Now);
                Constante.rtb_consola.Text += ">> " + fechahora + " " + Constante.usuario_actual + " [Impresion consola]\n"
                + res.Valor + "\n";
            }
            else
            {
                if(res.Tipo != Constante.ERROR)
                {
                    string descripcion = "Tipos incompatibles: " + Constante.getTipo(res.Tipo) + " no puede ser convertido a text";
                    uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                }
            }
            return null;
        }

        public object generar_booleano(Entorno ent)
        {
            throw new NotImplementedException();
        }
    }
}
