using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPProyecto.MVC
{
    /// <summary>
    /// Objeto comentario
    /// </summary>
    public class Comentario
    {
        public string Nombre { get; set; }
        public string ComentarioTexto { get; set; }

        public Comentario() { }

        public Comentario(string user, string coment)
        {
            Nombre = user;
            ComentarioTexto = coment;
        }
    }
}
