using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.plycs
{
    class Paquete
    {
        public const int LOGIN = 0;
        public const int FIN = 1;
        public const int ERROR = 2;
        public const int INST = 3;
        public const int REPORTE = 4;

        /// <summary>
        /// enviado al servidor db
        /// </summary>
        public const int ENVIO = 5;
        /// <summary>
        /// Recibido en el servidor web
        /// </summary>
        public const int RECIBIDO = 6;

        private int tipo_paquete; //tipo paquete LOGIN, FIN, ERROR, INST, REPORTE
        private int estado_paquete; //estado ENVIO, RECIBIDO

        //PARA LOGIN
        private string usuario;
        private string password;
        private string login;

        //PARA FIN
        private string fin; //xD no hace nada jaja solo sirve para reestablecer la conexion

        //PARA INSTRUCCION
        private string instruccion;
        private string respuesta;
        private List<List<Campo>> campos;

        //PARA ERROR
        private string tipo_error;
        private string msg; //ademas usa la lista de campos para la instruccion


        /*la lista de campos sirve para mostrar los datos recibidos desde usql*/
        //para informacion de error
        private int line;
        private int colm;

        public string Instruccion { get => instruccion; set => instruccion = value; }
        public string Respuesta { get => respuesta; set => respuesta = value; }
        public List<List<Campo>> Campos { get => campos; set => campos = value; }
        public int Tipo_paquete { get => tipo_paquete; set => tipo_paquete = value; }
        public int Estado_paquete { get => estado_paquete; set => estado_paquete = value; }
        public string Usuario { get => usuario; set => usuario = value; }
        public string Password { get => password; set => password = value; }
        public string Login { get => login; set => login = value; }
        public string Tipo_error { get => tipo_error; set => tipo_error = value; }
        public string Msg { get => msg; set => msg = value; }

        public Paquete(int line, int colm)
        {
            this.line = line;
            this.colm = colm;
        }

        /// <summary>
        /// Constructor para el error
        /// </summary>
        /// <param name="tipo_paquete"></param>
        /// <param name="estado_paquete"></param>
        /// <param name="tipo_error"></param>
        /// <param name="msg"></param>
        /// <param name="campos"></param>
        /// <param name="line"></param>
        /// <param name="colm"></param>
        /// <param name="nada"></param>
        public Paquete(int tipo_paquete, int estado_paquete, string tipo_error,
            string msg, List<List<Campo>> campos, int line, int colm, int nada)
        {
            this.tipo_paquete = tipo_paquete;
            this.estado_paquete = estado_paquete;
            this.tipo_error = tipo_error;
            this.msg = msg;
            this.line = line;
            this.colm = colm;
            this.campos = campos;
        }

        /// <summary>
        /// Constructor para instruccion
        /// </summary>
        /// <param name="tipo_paquete"></param>
        /// <param name="estado_paquete"></param>
        /// <param name="instruccion"></param>
        /// <param name="respuesta"></param>
        /// <param name="campos"></param>
        /// <param name="line"></param>
        /// <param name="colm"></param>
        public Paquete(int tipo_paquete, int estado_paquete,
            string instruccion, string respuesta, List<List<Campo>> campos,
            int line, int colm)
        {
            this.tipo_paquete = tipo_paquete;
            this.estado_paquete = estado_paquete;
            this.instruccion = instruccion;
            this.campos = campos;
            this.respuesta = respuesta;
            this.line = line;
            this.colm = colm;
        }

        /// <summary>
        /// Constructor para el fin
        /// </summary>
        /// <param name="tipo_paquete"></param>
        /// <param name="line"></param>
        /// <param name="colm"></param>
        public Paquete(int tipo_paquete, int  line, int colm)
        {
            this.tipo_paquete = tipo_paquete;
            this.estado_paquete = ENVIO;
        }

        /// <summary>
        /// Constructor para el login
        /// </summary>
        /// <param name="tipo_paquete"></param>
        /// <param name="estado_paquete"></param>
        /// <param name="usuario"></param>
        /// <param name="password"></param>
        /// <param name="login"></param>
        /// <param name="line"></param>
        /// <param name="colm"></param>
        public Paquete(int tipo_paquete, int estado_paquete, 
            string usuario, string password, string login, int line, int colm)
        {
            this.tipo_paquete = tipo_paquete;
            this.estado_paquete = estado_paquete;
            this.usuario = usuario;
            this.password = password;
            this.login = login;
            this.line = line;
            this.colm = colm;
        }
    }
}
