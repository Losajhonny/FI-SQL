using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;

namespace ServidorDB.arboles.usql.DCL
{
    class Permisos_Dcl : uInstruccion
    {
        /*Si el objeto es null simula el asterisco*/
        private string usuario;
        private int tipo_permiso;
        private string db;
        private string objeto;
        private int line;
        private int colm;

        public Permisos_Dcl(int tipo_permiso, string usuario, string db, string objeto, int line, int colm)
        {
            this.tipo_permiso = tipo_permiso;
            this.usuario = usuario;
            this.db = db;
            this.objeto = objeto;
            this.line = line;
            this.colm = colm;
        }
        
        public int Tipo_permiso { get => tipo_permiso; set => tipo_permiso = value; }
        public string Db { get => db; set => db = value; }
        public string Objeto { get => objeto; set => objeto = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }
        public string Usuario { get => usuario; set => usuario = value; }

        public object ejecutar(Entorno ent)
        {
            //para saber si viene con asterisco se debe de verificar el atributo objeto
            //se otorga permisos a un objeto de la base de datos
            if (tipo_permiso == Constante.OTORGAR)
            {
                PeticionDCL.otorgarUsuario(usuario, db, objeto, line, colm);
            }
            else
            {   //denegar
                PeticionDCL.denegarUsuario(usuario, db, objeto, line, colm);
            }
            return null;
        }
    }
}