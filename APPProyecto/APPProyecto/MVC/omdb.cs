using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using APPProyecto.PaginaVentana;
using Newtonsoft.Json;

namespace APPProyecto.MVC
{
    /// <summary>
    /// Clase con la que se gestionará las consultas a la API OMDB
    /// </summary>
    public class omdb
    {
        private BD? bd2;

        //Variables de informacion de la API
        private static string titulo = "";
        private static int anio = 0;
        private static string director = "";
        private static string reparto;
        private static string sinopsis = "";
        private static string calificacion = "";
        private static string imagen;
        private static string duracion = "";
        private static string pais = "";
        private static string genero = "";
        private static string premios;

        public omdb() { }

        /// <summary>
        /// Realiza una conexión a una API.
        /// </summary>
        /// <param name="apiUrl">URL de la API a la que se va a realizar la conexión.</param>
        /// <returns>Devuleve la respuesta de la API como una cadena.</returns>
        public static async Task<string> ConexionApi(string apiUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return null;
                }
            }
        }

        //Metodo que obtiene la información de una película en especifico
        //gracias al operador 't' que figura tras la APIKEY
        /// <summary>
        /// Obtiene información detallada de una película específica mediante su titulo.
        /// </summary>
        /// <param name="cad">Título de la película</param>
        /// <returns>Devuelve un objeto Movie con la información de la pelicula.</returns>
        public static async Task<Movie> PeliEspecifica(string cad)
        {
            try
            {
                string apiUrl = $"http://www.omdbapi.com/?apikey=347c11a6&t=" + cad;

                string respuesta = await ConexionApi(apiUrl);

                if (respuesta != null)
                {
                    var jsResultado = JsonDocument.Parse(respuesta).RootElement;

                    try
                    {
                        titulo = jsResultado.GetProperty("Title").GetString();
                    }
                    catch (Exception)
                    {
                        titulo = "Desconocido";
                    }
                    try
                    {
                        anio = Convert.ToInt32(jsResultado.GetProperty("Year").GetString());
                    }
                    catch (Exception)
                    {
                        anio = -1;
                    }
                    try
                    {
                        director = jsResultado.GetProperty("Director").GetString();
                    }
                    catch (Exception)
                    {
                        director = "Desconocido";
                    }
                    try
                    {
                        reparto = jsResultado.GetProperty("Actors").GetString();
                    }
                    catch (Exception)
                    {
                        reparto = null;
                    }
                    try
                    {
                        sinopsis = jsResultado.GetProperty("Plot").GetString();
                    }
                    catch (Exception)
                    {
                        sinopsis = "Desconocida";
                    }
                    try
                    {
                        calificacion = jsResultado.GetProperty("imdbRating").GetString();
                    }
                    catch (Exception)
                    {
                        calificacion = "Desconocida";
                    }
                    try
                    {
                        //imagen = jsResultado.GetProperty("Poster").GetString();
                        var js = JsonConvert.DeserializeObject<dynamic>(respuesta);
                        imagen = js.Poster;
                    }
                    catch (Exception)
                    {
                        imagen = "desconocida";
                    }
                    try
                    {
                        duracion = jsResultado.GetProperty("Runtime").GetString();
                    }
                    catch (Exception)
                    {
                        duracion = "Desconocida";
                    }

                    try
                    {
                        genero = jsResultado.GetProperty("Genre").GetString();
                    }
                    catch (Exception)
                    {
                        genero = "Desconocido";
                    }

                    try
                    {
                        pais = jsResultado.GetProperty("Country").GetString();
                    }
                    catch (Exception)
                    {
                        pais = "Desconocido";
                    }
                    try
                    {
                        premios = jsResultado.GetProperty("Awards").GetString();
                    }
                    catch (Exception)
                    {
                        premios = null;
                    }
                    //si no se coloca el finally salta error
                    finally { }

                    // Crear un objeto Pelicula con la información obtenida
                    Movie peliEspecifica = new Movie(titulo, anio, director, reparto, sinopsis, calificacion, imagen, duracion, genero, pais, premios);

                    return peliEspecifica;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
            }
            return null;
        }

        //Metodo que devuelve todas las peliculas que contengan el parametro cadena
        //gracias al operador 's' que figura tras la APIKEY
        /// <summary>
        /// Busca varis peliculas que contngan la cadena pasada por parametro en el titulo.
        /// </summary>
        /// <param name="cadena">Cadenala cual debe estar en el titulo.</param>
        /// <returns>devuelve una lista de objetos Movie con la información de las películas encontradas.</returns>
        public static async Task<List<Movie>> PeliculasAmplias(string cadena)
        {
            string apiUrl = $"http://www.omdbapi.com/?apikey=347c11a6&s=" + cadena;
            string respuesta = await ConexionApi(apiUrl);

            if (respuesta != null)
            {
                var jsResultados = JsonDocument.Parse(respuesta).RootElement;
                try {
                    var resultados = jsResultados.GetProperty("Search");

                    if (resultados.ValueKind == JsonValueKind.Array)
                    {
                        List<Movie> peliculas = new List<Movie>();

                        foreach (var resultado in resultados.EnumerateArray())
                        {
                            try
                            {
                                titulo = resultado.GetProperty("Title").GetString();
                            }
                            catch (Exception)
                            {
                                titulo = "Desconocido";
                            }

                            try
                            {
                                imagen = resultado.GetProperty("Poster").GetString();
                            }
                            catch (Exception)
                            {
                                imagen = null;
                                MessageBox.Show("No pasa");
                            }
                            try
                            {
                                genero = resultado.GetProperty("Genre").GetString();
                            }
                            catch (Exception)
                            {
                                genero = "Desconocido";
                            }
                            Movie PeliBusc = new Movie(titulo, imagen, genero);

                            peliculas.Add(PeliBusc);
                        }

                        return peliculas;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (KeyNotFoundException)
                {
                    MessageBox.Show("Sin datos");
                }

                return null;
            }
            else
            {
                return null;
            }
        }

        
    }
}
