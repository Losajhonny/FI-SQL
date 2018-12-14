﻿using ServidorDB.otros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.xml
{
    public class Objeto
    {
        /**
         * Esta clase realiza el almacenamiento a memoria de un objeto
         * @nombre      identificador del objeto
         * @atributos   lista de atributos de un objeto
         */

        protected string nombre;
        protected string ruta;
        protected List<Atributo> parametros;
        protected List<string> usuarios;

        public Objeto(string nombre)
        {
            this.nombre = nombre;
            this.ruta = Constante.RUTA_OBJETO + nombre + "." + Constante.EXTENSION;
            this.parametros = new List<Atributo>();
            this.usuarios = new List<string>();
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public List<Atributo> Parametros { get => parametros; set => parametros = value; }
        public List<string> Usuarios { get => usuarios; set => usuarios = value; }
        public string Ruta { get => ruta; set => ruta = value; }
    }
}