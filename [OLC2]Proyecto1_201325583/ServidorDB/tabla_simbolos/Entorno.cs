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
        private string tent;

        private List<Simbolo> tablahash;

        public Entorno Ant { get => ant; set => ant = value; }
        public Hashtable Tabla { get => tabla; set => tabla = value; }
        public string Tent { get => tent; set => tent = value; }
        public List<Simbolo> Tablahash { get => tablahash; set => tablahash = value; }

        public Entorno(Entorno ant)
        {
            this.ant = ant;
            this.tabla = new Hashtable();
            this.tablahash = new List<Simbolo>();
        }

        public void agregar(Simbolo s)
        {
            tablahash.Add(s);
        }

        //public void agregar(string id, Simbolo s)
        //{
        //    tabla.Add(id, s);
        //}
        
        public Simbolo getSimbolo_Entorno(string id, int tipo_simbolo)
        {
            for(Entorno e = this; e != null; e = e.Ant)
            {
                Simbolo s = null;
                for(int i = 0; i < e.Tablahash.Count; i++)
                {
                    s = e.Tablahash[i];
                    if(s.Id.Equals(id) && s.Tipo_simbolo == tipo_simbolo)
                    {
                        return s;
                    }
                }
            }
            return null;
        }

        //public Simbolo getSimbolo_Entorno(string id, int tipo_simbolo)
        //{
        //    for (Entorno e = this; e != null; e = e.Ant)
        //    {
        //        Simbolo find = (Simbolo)e.Tabla[id];
        //        if (find != null)
        //        {
        //            //como encontro el simbolo debo verificar el tipo_simbolo
        //            if(find.Tipo_simbolo == tipo_simbolo)
        //            {
        //                return find;
        //            }
        //        }
        //    }
        //    return null;
        //}

        public Simbolo getSimbolo_Entorno_Actual(string id, int tipo_simbolo)
        {
            Entorno e = this;
            Simbolo s = null;
            for (int i = 0; i < e.Tablahash.Count; i++)
            {
                s = e.Tablahash[i];
                if (s.Id.Equals(id) && s.Tipo_simbolo == tipo_simbolo)
                {
                    return s;
                }
            }
            return null;
        }


        /// <summary>
        /// Este metodo me elimina todas las funciones, proc, objetos de una base de datos seleccionada
        /// en el entonrno global
        /// </summary>
        public void deleteSimbolDB()
        {
            Entorno e = null;
            for(e = this; e != null; e = e.Ant)
            {
                if (e.Ant == null) break;
            }

            //entonces ya estoy en el entorno global
            //ahora a eliminar

            List<Simbolo> tmp = new List<Simbolo>();
            for (int i = 0; i < e.Tablahash.Count; i++)
            {
                if(e.Tablahash[i].Tipo_simbolo == Simbolo.FUNCION || e.Tablahash[i].Tipo_simbolo == Simbolo.OBJETO || e.Tablahash[i].Tipo_simbolo == Simbolo.PROCEDIMIENTO)
                {
                    tmp.Add(e.Tablahash[i]);
                }
            }

            for (int i = 0; i < tmp.Count; i++)
            {
                e.Tablahash.Remove(tmp[i]);
            }
        }

        //public Simbolo getSimbolo_Entorno_Actual(string id, int tipo_simbolo)
        //{
        //    Entorno e = this;
        //    Simbolo find = (Simbolo)e.Tabla[id];
        //    if (find != null)
        //    {
        //        if(find.Tipo_simbolo == tipo_simbolo)
        //        {
        //            return find;
        //        }
        //    }
        //    return null;
        //}

        //public Simbolo getSimbolo_Entorno(string id)
        //{
        //    for(Entorno e = this; e != null; e = e.Ant)
        //    {
        //        Simbolo find = (Simbolo)e.Tabla[id];
        //        if (find != null) return find;
        //    }
        //    return null;
        //}

        //public Simbolo getSimbolo_Entorno_Actual(string id)
        //{
        //    Entorno e = this;
        //    Simbolo find = (Simbolo)e.Tabla[id];
        //    if (find != null) return find;
        //    return null;
        //}

        //public bool existe(string id)
        //{
        //    return getSimbolo_Entorno_Actual(id) != null;
        //}

        public bool existe(string id, int tipo_simbolo)
        {
            return getSimbolo_Entorno_Actual(id, tipo_simbolo) != null;
        }
    }
}
