﻿using ServidorDB.analizadores.usql;
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
    class Crear_Ddl : uInstruccion
    {
        /*La clase realiza la funcion de crear base de datos,
         tablas, objetos, usuarios, procedimientos, funciones ademas, de
         ejecutar la funcion usar*/


        private int tipo_crear;
        private string id;

        private List<Atributo> atributos;
        private List<Declarar> declaraciones;
        private List<uInstruccion> inst;    //al parecer siempre va a ser null
        private int tipo_dato;
        
        private NodoExp password;

        //ahora necesito guardar el script de las instrucciones
        //para eso necesito la posicion del archivo
        //entonces almaceno la linea inicial al igual que la columna
        //y la final

        //ademas de eso por si ocurre otros errores necesito
        //line y colm normales
        private int line_init;
        private int colm_init;
        private int line_fint;
        private int colm_fint;
        private int line;
        private int colm;

        /// <summary>
        /// Constructor para crear una base de datos o usarla
        /// </summary>
        /// <param name="tipo_crear"></param>
        /// <param name="id"></param>
        public Crear_Ddl(int tipo_crear, string id)
        {   //para crear base de datos o usar una base de datos
            this.tipo_crear = tipo_crear;
            this.id = id;
        }

        /// <summary>
        /// Constructor para funciones y procedimientos
        /// </summary>
        /// <param name="tipo_crear"></param>
        /// <param name="id"></param>
        /// <param name="decs"></param>
        /// <param name="inst"></param>
        /// <param name="linei"></param>
        /// <param name="linef"></param>
        /// <param name="colmi"></param>
        /// <param name="colmf"></param>
        public Crear_Ddl(int tipo_crear, int tipo_retorno, string id, List<Declarar> decs,
            List<uInstruccion> inst, int linei, int linef, int colmi, int colmf)
        {
            this.tipo_dato = tipo_retorno;
            this.tipo_crear = tipo_crear;
            this.id = id;
            this.declaraciones = decs;
            this.inst = inst;
            this.line_init = linei;
            this.line_fint = linef;
            this.colm_init = colmi;
            this.colm_fint = colmf;
        }

        /// <summary>
        /// Para objetos
        /// </summary>
        /// <param name="tipo_crear"></param>
        /// <param name="id"></param>
        /// <param name="decs"></param>
        /// <param name="line"></param>
        /// <param name="colm"></param>
        public Crear_Ddl(int tipo_crear, string id, List<Declarar> decs, int line, int colm)
        {
            this.tipo_crear = tipo_crear;
            this.id = id;
            this.declaraciones = decs;
            this.line = line;
            this.colm = colm;
        }

        /// <summary>
        /// Para Tablas
        /// </summary>
        /// <param name="tipo_crear"></param>
        /// <param name="id"></param>
        /// <param name="atris"></param>
        /// <param name="line"></param>
        /// <param name="colm"></param>
        public Crear_Ddl(int tipo_crear, string id, List<Atributo> atris, int line, int colm)
        {
            this.tipo_crear = tipo_crear;
            this.id = id;
            this.atributos = atris;
            this.line = line;
            this.colm = colm;
        }

        /// <summary>
        /// Constructor para el usuario
        /// </summary>
        /// <param name="tipo_crear"></param>
        /// <param name="id"></param>
        /// <param name="password"></param>
        public Crear_Ddl(int tipo_crear, string id, NodoExp password)
        {
            this.tipo_crear = tipo_crear;
            this.id = id;
            this.password = password;
        }

        /// <summary>
        /// Constructor para crear una base de datos o usarla
        /// </summary>
        /// <param name="tipo_crear"></param>
        /// <param name="id"></param>
        public Crear_Ddl(int tipo_crear, string id, int line, int colm)
        {   //para crear base de datos o usar una base de datos
            this.tipo_crear = tipo_crear;
            this.id = id;
            this.line = line;
            this.colm = colm;
        }

        /// <summary>
        /// Constructor para el usuario
        /// </summary>
        /// <param name="tipo_crear"></param>
        /// <param name="id"></param>
        /// <param name="password"></param>
        public Crear_Ddl(int tipo_crear, string id, NodoExp password, 
            int line, int colm)
        {
            this.tipo_crear = tipo_crear;
            this.id = id;
            this.password = password;
            this.line = line;
            this.colm = colm;
        }


        public int Tipo_crear { get => tipo_crear; set => tipo_crear = value; }
        public string Id { get => id; set => id = value; }
        public List<Atributo> Atributos { get => atributos; set => atributos = value; }
        public int Tipo_dato { get => tipo_dato; set => tipo_dato = value; }
        public int Line_init { get => line_init; set => line_init = value; }
        public int Colm_init { get => colm_init; set => colm_init = value; }
        public int Line_fint { get => line_fint; set => line_fint = value; }
        public int Colm_fint { get => colm_fint; set => colm_fint = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }
        internal List<uInstruccion> Inst { get => inst; set => inst = value; }
        internal NodoExp Password { get => password; set => password = value; }
        public List<Declarar> Declaraciones { get => declaraciones; set => declaraciones = value; }

        public object ejecutar(Entorno ent)
        {
            if(tipo_crear == Constante.tBASE_DATOS)
            {
                ejecutar_base_datos();
            }
            else if (tipo_crear == Constante.tUSUARIO)
            {
                ejecutar_usuario(ent);
            }
            else if (tipo_crear == Constante.tOBJETO)
            {
                ejecutar_objeto();
            }
            else if (tipo_crear == Constante.tUSAR)
            {
                ejecutar_usuarDb();
            }
            else if (tipo_crear == Constante.tTABLA)
            {
                ejecutar_Tabla();
            }
            return null;
        }
        
        public void ejecutar_base_datos()
        {
            Db db = new Db(id);
            //agregar permiso a la base de datos
            //agregar al admin y al usuario actual
            db.Usuarios.Add(Constante.usuario_admin);
            //el if es por cuestiones de pruebas
            //se puede quitar el if con equals ("")
            if (!Constante.usuario_actual.Equals("") &&
                !Constante.usuario_actual.Equals(Constante.usuario_admin))
            {
                db.Usuarios.Add(Constante.usuario_actual);
            }
            bool estado = Peticion.crearDb(db);

            if (!estado)
            {
                string msg = "La base de datos: " + id + " ya existe en el DBMS";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, id, line, colm));
            }
        }

        public void ejecutar_usuario(Entorno ent)
        {
            if (Constante.usuario_actual.Equals(Constante.usuario_admin))
            {
                Resultado res = (Resultado)password.ejecutar(ent);
                if (res != null)
                {
                    if(res.Tipo == Constante.TEXT)
                    {
                        Usuario usr = new Usuario(id, res.Valor);
                        bool estado = Peticion.crearUsuario(usr);

                        if (!estado)
                        {
                            string msg = "El usuario: " + id + " ya existe en el DBMS";
                            uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, id, line, colm));
                        }
                    }
                    else
                    {
                        string msg = "El password debe ser de tipo entero";
                        uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, msg, id, line, colm));
                    }
                }
            }
            else
            {
                string msg = "Unicamente el administrador del DBMS puede crear usuarios";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, id, line, colm));
            }
        }

        public void ejecutar_usuarDb()
        {
            //en este caso solo debo realizar la transaccion
            bool estado = Peticion.usarDb(id, line, colm);
            //debo validar si en la peticion se logro realizar
            if (!estado)
            {
                string msg = "La base de datos: " + id + " no existe en el DBMS";
                uSintactico.uerrores.Add(new uError(Constante.LOGICO, msg, id, line, colm));
            }
        }

        public void ejecutar_Tabla()
        {
            /*validar que nohaya atributos iguales*/
            //creo la tabla
            //le coloco los atributos
            Tabla t = new Tabla(id);
            t.Atributos = atributos;

            //realizar la peticion
            t.Line = line;
            t.Colm = colm;

            //ahora como ya lo agregue necesita
            //actualizar el datatable de la tabla
            t.crearDataTable();

            Peticion.crearTabla(t);
        }

        /*EJECUTAR PENDIENTE*/
        public void ejecutar_objeto()
        {
            //creo un nuevo objeto
            Objeto obj = new Objeto(id);

            /*En este caso los atributos de esta son la lista de declaraciones
             que se guardaron en esta clase por lo tanto tengo que transformar
             la declaracion en atributos*/

            List<Atributo> atrs = new List<Atributo>();
            for (int i = 0; i < declaraciones.Count; i++)
            {
                //en la declaracion tengo la variable guardada en la lista
                //posicion 0
                Atributo atr = new Atributo(declaraciones[i].Tipo, declaraciones[i].Variables[0]);
                atrs.Add(atr);
            }
            //ahora ya tengo los atributos para agregar a los objetos
            obj.Parametros = atrs;
            obj.Line = line;
            obj.Colm = colm;

            //hago el proceso de insertar el objeto
            bool estado = Peticion.crearObjeto(obj);
        }
    }
}