using APPProyecto.PaginaVentana;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace APPProyecto.MVC
{
    /// <summary>
    /// Clase con las que se gestionará las peticiones a la API TMDB
    /// </summary>
    class TMDB
    {
        public TMDB() { }

        /// <summary>
        /// Realiza una conexión a una API y obtiene la respuesta como una cadena de texto.
        /// </summary>
        /// <param name="apiUrl">URL de la API.</param>
        /// <returns>Devuelve la respuesta de la API como una cadena de texto.</returns>
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

        //metodo que busca peliculas aleatorias por genero
        /// <summary>
        /// Busca peliculas aleatorias por genero
        /// Obtiene el total de las paginas, selecciona una pagina aleatoria
        /// Selecciona la pelicula
        /// Comprueba que esta en la api omdb
        /// Si existe devuelve la pelicula
        /// </summary>
        /// <param name="genero">Genero a buscar</param>
        /// <returns>Objeto Movie con la info de la pelicula</returns>
        public async Task<Movie> BuscarPorGenero(string genero)
        {
            try
            {
                string apiUrl = "https://api.themoviedb.org/3/discover/movie?api_key=9a547d6ef653deb7f314070aad45f918&with_genres=" + genero;
                var respuesta = await ConexionApi(apiUrl);

                if (respuesta != null)
                {
                    var jsResultado = JsonDocument.Parse(respuesta).RootElement;

                    try
                    {
                        var totalPages = jsResultado.GetProperty("total_pages").GetInt32();

                        // Intentar buscar en OMDB hasta encontrar una película válida
                        for (int i = 0; i < totalPages; i++)
                        {
                            // Seleccionar una página aleatoria
                            var random = new Random();
                            var randomPage = random.Next(1, totalPages + 1);

                            // Consultar la API con la página aleatoria
                            apiUrl = $"https://api.themoviedb.org/3/discover/movie?api_key=9a547d6ef653deb7f314070aad45f918&with_genres=" + genero + "&page=" + randomPage;
                            respuesta = await ConexionApi(apiUrl);

                            var jsResultadoPagina = JsonDocument.Parse(respuesta).RootElement;
                            var resultados = jsResultadoPagina.GetProperty("results");

                            if (resultados.GetArrayLength() > 0)
                            {
                                // Buscar en OMDB hasta encontrar una película válida
                                foreach (var peliculaSeleccionada in resultados.EnumerateArray())
                                {
                                    var titulo = peliculaSeleccionada.GetProperty("title").GetString();
                                    var imagenPath = peliculaSeleccionada.GetProperty("poster_path").GetString();
                                    var imagenUrl = $"https://image.tmdb.org/t/p/w500{imagenPath}";
                                    var generos = peliculaSeleccionada.GetProperty("genre_ids").EnumerateArray();
                                    var primerGenero = generos.FirstOrDefault();
                                    var generoID = "Desconocido";

                                    if (primerGenero.ValueKind.Equals(JsonValueKind.Null))
                                        generoID = primerGenero.ToString();

                                    // Verificar si existe en la API de OMDB
                                    var omdbMovie = await BuscarEnOMDB(titulo);

                                    // Devolver la película de TMDb si se encuentra en OMDB
                                    if (omdbMovie != null)
                                    {
                                        return omdbMovie;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
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

        //metodo que busca peliculas por año
        /// <summary>
        /// /// Busca peliculas aleatorias por año
        /// Obtiene el total de las paginas, selecciona una pagina aleatoria
        /// Selecciona la pelicula
        /// Comprueba que esta en la api omdb
        /// Si existe devuelve la pelicula
        /// </summary>
        /// <param name="anio">año a buscar</param>
        /// <returns>Devuleve objeto Moovie con la información de la pelicula</returns>
        public async Task<Movie> BuscarPorAnio(string anio)
        {
            try
            {
                string apiUrl = $"https://api.themoviedb.org/3/discover/movie?api_key=9a547d6ef653deb7f314070aad45f918&primary_release_year={anio}";
                var respuesta = await ConexionApi(apiUrl);

                if (respuesta != null)
                {
                    var jsResultado = JsonDocument.Parse(respuesta).RootElement;

                    try
                    {
                        var totalPages = jsResultado.GetProperty("total_pages").GetInt32();
                        var random = new Random();

                        // Intentar buscar en OMDB hasta encontrar una película válida
                        for (int i = 0; i < totalPages; i++)
                        {
                            // Seleccionar una página aleatoria
                            var randomPage = random.Next(1, totalPages + 1);

                            // Consultar la API con la página aleatoria
                            apiUrl = $"https://api.themoviedb.org/3/discover/movie?api_key=9a547d6ef653deb7f314070aad45f918&primary_release_year={anio}&page={randomPage}";
                            respuesta = await ConexionApi(apiUrl);

                            var jsResultadoPagina = JsonDocument.Parse(respuesta).RootElement;
                            var resultados = jsResultadoPagina.GetProperty("results");

                            if (resultados.GetArrayLength() > 0)
                            {
                                // Obtener una película aleatoria de la página actual
                                var peliculaSeleccionada = resultados[random.Next(0, resultados.GetArrayLength())];

                                var titulo = peliculaSeleccionada.GetProperty("title").GetString();
                                var imagenPath = peliculaSeleccionada.GetProperty("poster_path").GetString();
                                var imagenUrl = $"https://image.tmdb.org/t/p/w500{imagenPath}";

                                // Verificar si existe en la API de OMDB
                                var omdbMovie = await BuscarEnOMDB(titulo);

                                // Devolver la película de TMDb si se encuentra en OMDB
                                if (omdbMovie != null)
                                {
                                    return omdbMovie;
                                }
                                else
                                {
                                    // Si no está en OMDB, buscar otra página
                                    continue;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Manejar excepciones según sea necesario
                    }
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

        /// <summary>
        /// Metodo que busca en la ficha de un director buscando por su nombre y devuelve el id
        /// </summary>
        /// <param name="director">Nombre del director</param>
        /// <returns>Devuleve el ID del director</returns>
        public static async Task<int> IdDirector(string director)
        {
            try
            {
                string apiUrl = "https://api.themoviedb.org/3/search/person?api_key=9a547d6ef653deb7f314070aad45f918&query=" + director;

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage respuesta = await client.GetAsync(apiUrl);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        using (Stream stream = await respuesta.Content.ReadAsStreamAsync())
                        {
                            using (JsonDocument document = await JsonDocument.ParseAsync(stream))
                            {
                                var resultados = document.RootElement.GetProperty("results");

                                if (resultados.GetArrayLength() > 0)
                                {
                                    int directorId = resultados[0].GetProperty("id").GetInt32();
                                    return directorId;
                                }
                            }
                        }
                    }
                    else
                    {
                        // Manejar la respuesta no exitosa si es necesario
                        MessageBox.Show($"Error en la solicitud: {respuesta.StatusCode}");
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Error al obtener el ID del director:\n" + ex.Message);
            }
            return -8888;
        }

        /// <summary>
        /// Metodo que busca una pelicula de un director, buscando por el ID del director
        /// </summary>
        /// <param name="id">ID del director</param>
        /// <returns>Objeto Movie con la info de una pelicula</returns>
        public static async Task<Movie> MismoDirector (int id){
            try
            {
                string apiUrl = $"https://api.themoviedb.org/3/discover/movie?api_key=9a547d6ef653deb7f314070aad45f918&with_crew=" + id;

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage respuesta = await client.GetAsync(apiUrl);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        using (Stream stream = await respuesta.Content.ReadAsStreamAsync())
                        {
                            using (JsonDocument document = await JsonDocument.ParseAsync(stream))
                            {
                                var resultados = document.RootElement.GetProperty("results");

                                if (resultados.GetArrayLength() > 0)
                                {
                                    var random = new Random();
                                    var indiceAleatorio = random.Next(0, resultados.GetArrayLength());
                                    var peliculaSeleccionada = resultados[indiceAleatorio];

                                    var titulo = peliculaSeleccionada.GetProperty("title").GetString();
                                    var imagenPath = peliculaSeleccionada.GetProperty("poster_path").GetString();
                                    var generos = peliculaSeleccionada.GetProperty("genre_ids").EnumerateArray();
                                    var primerGenero = generos.FirstOrDefault();
                                    var generoID = primerGenero.ValueKind != JsonValueKind.Null ? primerGenero.ToString() : "Desconocido";

                                    var omdbPelicula = await omdb.PeliEspecifica(titulo);

                                    if (omdbPelicula == null)
                                    {
                                        return await MismoDirector(id);
                                    }
                                    else
                                    {
                                        return new Movie(titulo, $"https://image.tmdb.org/t/p/w500" + imagenPath, generoID);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener película aleatoria: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Metodo que comprueba si la pelicula buscada existe en la api OMDB
        /// </summary>
        /// <param name="titulo">Titulo de la pelicula</param>
        /// <returns>Objeto Movie con la info de la Pelicula</returns>
        private async Task<Movie> BuscarEnOMDB(string titulo)
        {
            try
            {
                string omdbApiUrl = $"http://www.omdbapi.com/?apikey=347c11a6&t&t={titulo}";
                var omdbRespuesta = await ConexionApi(omdbApiUrl);

                if (omdbRespuesta != null)
                {
                    var jsOmdbResultado = JsonDocument.Parse(omdbRespuesta).RootElement;

                    // Verificar si la respuesta de OMDB contiene datos de la película
                    if (jsOmdbResultado.GetProperty("Response").GetString() == "True")
                    {
                        var omdbTitulo = jsOmdbResultado.GetProperty("Title").GetString();
                        var omdbImagenUrl = jsOmdbResultado.GetProperty("Poster").GetString();
                        var omdbGenero = jsOmdbResultado.GetProperty("Genre").GetString();

                        return new Movie(omdbTitulo, omdbImagenUrl, omdbGenero);
                    }
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

    }
}
