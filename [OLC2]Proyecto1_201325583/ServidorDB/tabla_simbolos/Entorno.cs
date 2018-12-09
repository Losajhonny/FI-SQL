using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.tabla_simbolos
{
    class Entorno
    {
        /*
         * Maneja los ambitos de los simbolos.
         * 
         * @ant     entorno o bloque que guarda los simbolos.
         * @tabla   es la tabla del entorno o bloque actual.
         * 
         * Entorno(Entorno)         instancia un nuevo entorno donde se inicializa
         *                          la tabla de simbolos de ese entorno.
         *                          
         * agregar(string, Simbolo) agrega un simbolo a la tabla de simbolos con una llave.
         * 
         * getSimbolo(string)       si existe retorna un simbolo.
         * 
         * existe(string)           hace uso del metodo getSimbolo(string) si retorna null
         *                          entonces retornara  false de lo contrario retornara true.
         */

        private Entorno ant;
        private Hashtable tabla;

        public Entorno(Entorno ant)
        {
            this.ant = ant;
            this.tabla = new Hashtable();
        }

        public void agregar(string id, Simbolo s)
        {
            tabla.Add(id, s);
        }

        public Simbolo getSimbolo(string id)
        {
            for(Entorno e = this; e != null; e = e.ant)
            {
                Simbolo find = (Simbolo)tabla[id];
                if (find != null) return find;
            }
            return null;
        }

        public bool existe(string id)
        {
            return getSimbolo(id) != null;
        }
    }
}
