using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPProyecto.MVC
{
    /// <summary>
    /// Clase con la que se gestionan los datos de un usuario
    /// </summary>
    public class Usuario
    {
        private string correo;
        private string password;
        private string rol;
        private string nombre;
        private string avatar;
        private string estilo;
        private string fechaRegistro;
        private string pregunta;
        private string respuesta;

        public Usuario(string correo, string password, string nombre, string pregunta, string respuesta, int rol)
        {
            this.correo = correo;
            this.password = password;
            this.rol = rol.ToString();
            this.nombre = nombre;
            this.avatar = "17";
            this.estilo = "0";
            this.fechaRegistro = DateTime.Now.ToString("dd-MM-yyyy");
            this.pregunta = pregunta;
            this.respuesta = respuesta;
        }

        public Usuario(string correo, string password, string rol, string nombre, string avatar, string estilo, string fechaRegistro)
        {
            this.correo = correo;
            this.password = password;
            this.rol = rol;
            this.nombre = nombre;
            this.avatar = avatar;
            this.estilo = estilo;
            this.fechaRegistro = fechaRegistro;
        }

        public Usuario(string correo, string password, string rol, string nombre, string avatar, string estilo, string fechaRegistro, string pregunta, string respuesta)
        {
            this.correo = correo;
            this.password = password;
            this.rol = rol;
            this.nombre = nombre;
            this.avatar = avatar;
            this.estilo = estilo;
            this.fechaRegistro = fechaRegistro;
            this.pregunta = pregunta;
            this.respuesta = respuesta;
        }

        public string Correo
        {
            get { return correo; }
        }

        public string Password
        {
            get { return password; }
        }

        public string Rol
        {
            get { return rol; }
        }

        public string Nombre
        {
            get { return nombre; }
        }

        public string Avatar
        {
            get { return avatar; }
        }

        public string Estilo
        {
            get { return estilo; }
        }

        public string FechaRegistro
        {
            get { return fechaRegistro; }
        }

        public string Pregunta
        {
            get { return pregunta; }
        }

        public string Respuesta
        {
            get { return respuesta; }
        }
    }
}
