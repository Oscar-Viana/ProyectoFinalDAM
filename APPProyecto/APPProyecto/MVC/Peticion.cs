using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPProyecto.MVC
{
    /// <summary>
    /// Clase con la que se gestionarán las peticiones
    /// </summary>
    public class Peticion
    {
        public string Correo { get; set; }
        public string Nick { get; set; }
        public string Asunto { get; set; }
        public string Mensaje { get; set; }

        /// <summary>
        /// Único onstructor.
        /// </summary>
        /// <param name="correo">Correo del usuario</param>
        /// <param name="nick">Nombre del usuario</param>
        /// <param name="asunto">Asunto de la peticion</param>
        /// <param name="mensaje">Petición</param>
        public Peticion(string correo, string nick, string asunto, string mensaje)
        {
            Correo = correo;
            Nick = nick;
            Asunto = asunto;
            Mensaje = mensaje;
        }
    }
}
