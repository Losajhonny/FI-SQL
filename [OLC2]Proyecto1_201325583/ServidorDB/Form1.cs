using Irony.Parsing;
using ServidorDB.analizadores.usql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServidorDB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ParseTreeNode raiz = uSintactico.analizar(richTextBox1.Text);
            uSintactico.uerrores.Clear();
            if (raiz == null)
            {
                string msg = "<< Mostrando errores >>\n";
                for(int i = 0; i < uSintactico.uerrores.Count; i++)
                {
                    msg += "Descripcion: " + uSintactico.uerrores[i].Descripcion + " Lexema: " + uSintactico.uerrores[i].Lexema + "\n";
                }
                richTextBox2.Text = msg;
            }
            else
            {
                //por el momento solo ejecuto expresion suma
                tabla_simbolos.Entorno ent = new tabla_simbolos.Entorno(null);
                arboles.usql.Expresiones.NodoExp ne = arboles.usql.uArbol.EXPRESION(raiz);
                arboles.usql.Expresiones.Resultado res = (arboles.usql.Expresiones.Resultado)ne.ejecutar(ent);

                string msg = (res != null) ? res.Tipo.ToString() + " -> " + res.Valor : "";
                string msg1 = "";
                for (int i = 0; i < uSintactico.uerrores.Count; i++)
                {
                    msg1 += "Descripcion: " + uSintactico.uerrores[i].Descripcion + " Lexema: " + uSintactico.uerrores[i].Lexema + "\n";
                }
                richTextBox2.Text = msg + "\n<< Mostrando errores >>\n" + msg1;
            }
        }
    }
}
