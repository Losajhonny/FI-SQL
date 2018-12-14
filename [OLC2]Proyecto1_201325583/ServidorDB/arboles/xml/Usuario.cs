using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.xml
{
    public class Usuario
    {
        /**
         * Solo almacena el nombre y password de un usuario
         * esta clase no genera nada solamente referencia a
         * un usuario en el archivo maestro
         */
        private string nombre;
        private string password;
        private int line;
        private int colm;

        public Usuario(string nombre, string password)
        {
            this.nombre = nombre;
            this.password = password;
        }
        
        public string Nombre { get => nombre; set => nombre = value; }
        public string Password { get => password; set => password = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }
    }
}
