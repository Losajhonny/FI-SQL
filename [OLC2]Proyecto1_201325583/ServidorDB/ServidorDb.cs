using Irony.Parsing;
using ServidorDB.analizadores.usql;
using ServidorDB.otros;
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
    public partial class ServidorDb : Form
    {
        public ServidorDb()
        {
            Constante.rtb_consola.AcceptsTab = true;
            Constante.rtb_consola.Location = new System.Drawing.Point(12, 422);
            Constante.rtb_consola.Name = "rtb_consola";
            Constante.rtb_consola.Size = new System.Drawing.Size(776, 221);
            Constante.rtb_consola.TabIndex = 3;
            Constante.rtb_consola.Text = "";
            this.Controls.Add(Constante.rtb_consola);
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ParseTreeNode raiz = uSintactico.analizar(richTextBox1.Text);
            Constante.rtb_consola.Text = "";
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
                List<arboles.usql.uInstruccion> inst = arboles.usql.uArbol.SENTENCIAS(raiz);

                for(int i = 0; i < inst.Count; i++)
                {
                    inst[i].ejecutar(ent);
                }

                string msg1 = "";
                for (int i = 0; i < uSintactico.uerrores.Count; i++)
                {
                    msg1 += "Descripcion: " + uSintactico.uerrores[i].Descripcion + " Lexema: " + uSintactico.uerrores[i].Lexema + "\n";
                }
                richTextBox2.Text = "\n<< Mostrando errores >>\n" + msg1;
            }
        }
        
    }
}
