using ServidorDB.analizadores.plycs;
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
    public partial class ServidorWeb : Form
    {
        TabPage tfisql = null;
        TabPage tsesion = null;

        public ServidorWeb()
        {
            InitializeComponent();
            //guardo el tab para despues mostrarlo
            //tsesion = this.tab_inicial.TabPages[0];
            //tfisql = this.tab_inicial.TabPages[1];

            /*El tab va a estar jugando con las pestañans debido a que necesito remover*/
            //this.tab_inicial.TabPages.Remove(tfisql);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string text = richTextBox1.Text;
            //arboles.plycs.PlyAst pnu =  pSintactico.analizar(text);
            //if(pnu != null)
            //{
            //    MessageBox.Show("sin errores");
            //}
            //else
            //{
            //    MessageBox.Show("error");
            //}
        }

        private void btn_iniciar_Click(object sender, EventArgs e)
        {
            if(!txt_user.Text.Equals("") && !txt_pass.Equals(""))
            {
                //mandar peticion por plycs a lenguaje sql que manda peeticion al xml y asi devuelven los valores
                string peticion = "";

                //realizar la conexion

                //si viene con usuario true iniciar el tfisql
                //this.tab_inicial.TabPages.Add(tfisql);
            }
            else
            {
                MessageBox.Show("Debe ingresar el nombre de usuario y contraseña", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void fisql_Click(object sender, EventArgs e)
        {

        }
    }
}
