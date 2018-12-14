using ServidorDB.otros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.xml
{
    public class Funcion : Objeto
    {
        /**
         * Herede de objeto por que tiene algunos parametros iguales como lo son
         * lista de parametros, lista de usuarios, nombre y la ruta.
         * Y ademas estan al mismo nivel de abstraccion
         * 
         * maneja otros campos aparte de eso tipo y src
         * @tipo    Representa el tipo de retorno de la funcion
         * @src     Instrucciones para la creacion de la funcion con sus instruciones
         */
        protected int tipo;
        protected string tipo_;
        protected string src;

        public Funcion(int tipo, string nombre, string src) : base(nombre)
        {
            this.ruta = Constante.RUTA_FUNCIONES + nombre + "." + Constante.EXTENSION;
            this.tipo = tipo;
            this.src = src;
        }

        public Funcion(int tipo, string nombre) : base(nombre)
        {
            this.tipo = tipo;
        }
        
        public string Src { get => src; set => src = value; }
        public int Tipo { get => tipo; set => tipo = value; }
        public string Tipo_ { get => tipo_; set => tipo_ = value; }

        public object cargar()
        {
            //cargo la variable del tipo de dato de retorno
            if(Tipo_ != null)
            {
                tipo = -1;
                for (int i = 0; i < Constante.TIPOS.Length; i++)
                {
                    if (Tipo_.Equals(Constante.TIPOS[i]))
                    {
                        tipo = i;
                        return i;
                    }
                }

                if(tipo == -1)
                {
                    //no encontro el tipo especificado
                    tipo = Constante.ID;
                    return tipo;
                }
            }
            return null;
        }
    }
}
