using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace APPProyecto.PaginaVentana
{
    /// <summary>
    /// Clase con la que se administrarán las peliculas
    /// </summary>
    public class Movie
    {
        private string? titulo;
        private int anio;
        private string? director;
        private string? reparto;
        private string? sinopsis;
        private string? calificacion;
        private string? imagen;
        private string? duracion;
        private string? genero;
        private string? pais;
        private string? premios;

        public Movie() { }

        /// <summary>
        /// Constructor con tres parámetros
        /// Este se usará para los user controls
        /// </summary>
        /// <param name="titulo">Titulo de la pelicula</param>
        /// <param name="poster">Imagen de la pelicula</param>
        /// <param name="gen">Genero de la pelicula</param>
        public Movie(string titulo, string poster, string gen)
        {
            this.titulo = titulo;
            imagen = poster;
            genero = gen;
        }

        /// <summary>
        /// Constructor final de la pelicula
        /// Este se usará para la información detallada
        /// </summary>
        /// <param name="titulo">Titulo de la pelicula</param>
        /// <param name="anio">Año de la pelicula</param>
        /// <param name="director">Director de la pelicula</param>
        /// <param name="reparto">Resparto de la pelicula</param>
        /// <param name="sinopsis">Sinopsis de la pelicula</param>
        /// <param name="calificacion">Calificación de la pelicula</param>
        /// <param name="imagen">Imagen de la pelicula</param>
        /// <param name="dureacion">Duracion de la pelicula</param>
        /// <param name="genero">Genero de la pelicula</param>
        /// <param name="pais">Pais de la pelicula</param>
        /// <param name="premios">Premios de la pelicula</param>
        public Movie(string titulo, int anio, string director, string reparto, string sinopsis, string calificacion, string imagen, string dureacion, string genero, string pais, string premios)
        {
            this.titulo = titulo;
            this.anio = anio;
            this.director = director;
            this.reparto = reparto;
            this.sinopsis = sinopsis;
            this.calificacion = calificacion;
            this.imagen = imagen;
            this.duracion = dureacion;
            this.genero = genero;
            this.pais = pais;
            this.premios = premios;
        }

        public string getTitulo()
        {
            return titulo;
        }

        public int getAnio()
        {
            return anio;
        }

        public string getDirector()
        {
            return director;
        }

        public string getReparto()
        {
            return reparto;
        }

        public string getSinopsis()
        {
            return sinopsis;
        }

        public string getCalificacion()
        {
            return calificacion;
        }

        public string ObtenerImagen()
        {
            return imagen;
        }

        public string getDuracion() {
            return duracion;
        }

        public string getGenero()
        {
            return genero;
        }

        public string getPais()
        {
            return pais;
        }

        public string getPremios()
        {
            return premios;
        }
    }
}
