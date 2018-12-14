using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.arbol
{
    public class Usuario
    {
        private List<string> permisos;  //pendiente
        private string nombre;
        private string password;

        public Usuario(string nombre, string password, List<string> permisos)
        {
            this.nombre = nombre;
            this.password = password;
            this.permisos = permisos;
        }

        public List<string> Permisos { get => permisos; set => permisos = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Password { get => password; set => password = value; }
    }
}
