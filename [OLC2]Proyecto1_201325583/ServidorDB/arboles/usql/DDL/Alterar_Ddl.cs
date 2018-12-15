using ServidorDB.analizadores.usql;
using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.arboles.usql.SSL;
using ServidorDB.arboles.xml;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql.DDL
{
    class Alterar_Ddl : uInstruccion
    {
        /*Solo se pueden alterar las tablas, objetos y usuarios
         
         Para usuarios  = Es facil solo necesito acceder a los usuarios
                          y cambiarles la contraseña verficando
                          El usuario ADMIN puede modificar el password de 
                          diferentes usuarios
                          
                          Mientras que el usuario actual puede modificar
                          solamente su contraseña
                          
         
         Para objetos   = Tambien es facil solo es de eliminar atributos
                          que se estara agregando o quitando
                          
         
         Para Tablas    = Dificil por que al momento de eliminar una columna
                          necesito que los registros de esta tabla no se pierda
                          por lo que debo manipular los registros moviendo todo
                          lo que quiera eliminar mas a la derecha y los que no 
                          se quedan mas a la izq para poder obtener y leer
                          el atributo en una posicion e ignorar los demas
                          
                         para agregar un columna Facil por que solo debo agregar
                         al final*/

        private int tipo_alterar;       //objetos, tablas o usuarios
        private int tipo_alteracion;    //agregar, quitar o cambiar
        private List<Declarar> declaraciones;
        private List<Atributo> atributos;
        private List<string> lista_id;
        private string id;
        private NodoExp password;
        private int line;
        private int colm;

        public Alterar_Ddl(int tipo_alterar, string id, NodoExp pass,
            int line, int colm)
        {
            this.tipo_alterar = tipo_alterar;
            this.tipo_alteracion = Constante.tCAMBIAR;
            this.id = id;
            this.password = pass;
        }

        /// <summary>
        /// Para alterar tabla agregar mas campos
        /// </summary>
        /// <param name="tipo_alterar"></param>
        /// <param name="tipo_alteracion"></param>
        /// <param name="id"></param>
        /// <param name="atributos"></param>
        /// <param name="line"></param>
        /// <param name="colm"></param>
        public Alterar_Ddl(int tipo_alterar, string id, List<Atributo> atributos,
            int line, int colm)
        {
            this.tipo_alterar = tipo_alterar;
            this.tipo_alteracion = otros.Constante.tAGREGAR;
            this.id = id;
            this.atributos = atributos;
            this.line = line;
            this.colm = colm;
        }

        /// <summary>
        /// Para alterar objetos
        /// </summary>
        /// <param name="tipo_alterar"></param>
        /// <param name="id"></param>
        /// <param name="declaraciones"></param>
        /// <param name="line"></param>
        /// <param name="colm"></param>
        public Alterar_Ddl(int tipo_alterar, string id, List<Declarar> declaraciones,
            int line, int colm)
        {
            this.tipo_alterar = tipo_alterar;
            this.tipo_alteracion = otros.Constante.tAGREGAR;
            this.id = id;
            this.declaraciones = declaraciones;
            this.line = line;
            this.colm = colm;
        }

        /// <summary>
        /// Para quitar objeto y tabla
        /// </summary>
        /// <param name="tipo_alterar"></param>
        /// <param name="tipo_alteracion"></param>
        /// <param name="id"></param>
        /// <param name="lista"></param>
        /// <param name="line"></param>
        /// <param name="colm"></param>
        public Alterar_Ddl(int tipo_alterar, string id, List<string> lista,
            int line, int colm)
        {
            this.tipo_alterar = tipo_alterar;
            this.tipo_alteracion = otros.Constante.tQUITAR;
            this.id = id;
            this.lista_id = lista;
            this.line = line;
            this.colm = colm;
        }

        public int Tipo_alterar { get => tipo_alterar; set => tipo_alterar = value; }
        public int Tipo_alteracion { get => tipo_alteracion; set => tipo_alteracion = value; }
        public List<Atributo> Atributos { get => atributos; set => atributos = value; }
        public List<string> Lista_id { get => lista_id; set => lista_id = value; }
        public string Id { get => id; set => id = value; }
        public int Colm { get => colm; set => colm = value; }
        public int Line { get => line; set => line = value; }
        internal List<Declarar> Declaraciones { get => declaraciones; set => declaraciones = value; }

        public object ejecutar(Entorno ent)
        {
            if(tipo_alterar == Constante.tTABLA)
            {
                if(tipo_alteracion == Constante.tAGREGAR)
                {
                    agregarTabla();
                }
                else
                {
                    quitarTabla();
                }
            }
            else if (tipo_alterar == Constante.tOBJETO)
            {
                if (tipo_alteracion == Constante.tAGREGAR)
                {
                    agregarObjeto();
                }
                else
                {
                    quitarObjeto();
                }
            }
            else if (tipo_alterar == Constante.tUSUARIO)
            {
                if(tipo_alteracion == Constante.tCAMBIAR)
                {
                    cambiarPassword(ent);
                }
            }

            return null;
        }

        public void quitarTabla()
        {
            Peticion.quitarTabla(lista_id, id, line, colm);
        }

        public void agregarTabla()
        {
            Peticion.agregarTabla(id, atributos, line, colm);
        }

        public void quitarObjeto()
        {
            Peticion.quitarObjeto(lista_id, id, line, colm);
        }

        public void agregarObjeto()
        {
            //1. pasando las declaraciones como atributos
            List<Atributo> atrs = new List<Atributo>();
            foreach (Declarar dec in declaraciones)
            {
                Atributo a = new Atributo(dec.Tipo, dec.Variables[0]);
                //La variable se guarda en la lista de variables de la declaracion
                atrs.Add(a);
            }
            //2. lo realiza la peticion
            Peticion.agregarObjeto(id, atrs, line, colm);
        }

        public void cambiarPassword(Entorno ent)
        {
            //necesito el resultado de la expresion
            Resultado res = (Resultado)password.ejecutar(ent);
            if(res != null)
            {
                if(res.Tipo == Constante.TEXT)
                {
                    Peticion.cambiarPassword(id, res.Valor, line, colm);
                }
                else
                {
                    string msg = "El password debe ser de tipo entero";
                    uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, msg, id, line, colm));
                }
            }
        }
    }
}
