using APPProyecto.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace APPProyecto.PaginaVentana.Peliculas
{
    /// <summary>
    /// Página que muestra la información de varias peliculas.
    /// </summary>
    public partial class Comparar : Page
    {
        private List<Movie> peliculas;
        private List<string> titulos;
        private omdb api;
        private BD basedatos;
        private int col = 1;
        private int fil = 1;
        private int barrera = 3;
        private ToolTip tool;

        /// <summary>
        /// Constructor de la clase Comparar.
        /// Inicializa la interfaz de usuario y la lista de títulos de películas con la lista proporcionada.
        /// Crea instancias de las clases Movie, omdb y BD utilizadas en la comparación de películas.
        /// Obtiene información detallada de las películas llamando al método ObtenerPelis().
        /// </summary>
        /// <param name="pelis">Lista de títulos de películas a comparar.</param>
        public Comparar(List<string> pelis)
        {
            InitializeComponent();
            titulos = pelis;
            peliculas = new List<Movie>();
            api = new omdb();
            basedatos = new BD();
            ObtenerPelis(titulos);
        }

        //metodo que obtiene la información de las peliculas de la lista
        //Si la pelicula esta en la base de datos la mete en la lista
        //si no lo esta la busca en la api, la inserta en la base de datos y luego la mete en la lista
        //y luego llama a los metodos para colocar la info
        /// <summary>
        /// Metodo que obtiene la informacion de cada pelicula de la lista pelis, las peliculas se consultarán en la base de datos, en caso de que
        /// exista, si no existe se buscará en la api OMDB y se guarda en la base de datos.
        /// Se invoca al metodo ColocarPoster para mostrar las imágenes de las peliculas
        /// Se invoca al metodo ColocarDatos para mostrar la informacion de las peliculas
        /// </summary>
        /// <param name="pelis">Lista de títulos de películas para obtener información.</param>
        public async void ObtenerPelis(List<string> pelis)
        {
            foreach(string titulo in pelis) {
                Movie esperada = basedatos.BuscarPeliBD(titulo);

                if (esperada == null)
                {
                    esperada = await omdb.PeliEspecifica(titulo);
                    basedatos.InsertatPeli(esperada);
                    peliculas.Add(esperada);
                }
                else
                {
                    peliculas.Add(esperada);
                }
            }
            ColocarPoster(peliculas);
            ColocarDatos(peliculas);
        }

        /// <summary>
        /// Coloca las imágenes de póster de las películas.
        /// Las imagenes se colocarán en una posición en concreto
        /// </summary>
        /// <param name="peliculas">Lista de objetos Movie, los cuales tienen la información de las peliculas a comprara.</param>
        public void ColocarPoster(List<Movie> peliculas) {
            col = 1;
            fil = 1;
            foreach (Movie pelicula in peliculas)
            {
                Image imgPelicula = new Image();

                if (pelicula.ObtenerImagen() == "N/A")
                    imgPelicula.Source = new BitmapImage(new Uri("/Resources/notfound.png", UriKind.Relative));
                else
                    imgPelicula.Source = new BitmapImage(new Uri(pelicula.ObtenerImagen()));


                // Configurar la posición en el grid
                Grid.SetRow(imgPelicula, fil);
                Grid.SetColumn(imgPelicula, col);

                // Agregar la imagen al grid
                gridCompare.Children.Add(imgPelicula);

                // Actualizar las posiciones para la siguiente iteración
                col += 4;
            }
        }

        
        /// <sumary>
        /// Coloca la información de cada pelicula en su posición correcta.
        /// Ademas se asigann tootips con el titulo de la pelicula a cada campo por si no sabemos a que pelicula pertenece.
        /// </sumary>
        /// <param name="peliculas">Lista de objetos Movie que representan las películas a mostrar.</param>
        public void ColocarDatos(List<Movie> peliculas) {
            col = 1;
            fil = 3;
            foreach (Movie pelicula in peliculas)
            {
                //Titulo
                Label lblTitulo = new Label();
                lblTitulo.Content = pelicula.getTitulo();
                Grid.SetRow(lblTitulo, fil);
                Grid.SetColumn(lblTitulo, col);
                tool = new ToolTip();
                tool.Content = pelicula.getTitulo();
                lblTitulo.ToolTip = tool;
                gridCompare.Children.Add(lblTitulo);
                fil += 4;

                //Año
                Label lblAnio = new Label();
                lblAnio.Content = pelicula.getAnio();
                Grid.SetRow(lblAnio, fil);
                Grid.SetColumn(lblAnio, col);
                tool = new ToolTip();
                tool.Content = pelicula.getTitulo();
                lblAnio.ToolTip = tool;
                gridCompare.Children.Add(lblAnio);
                fil += 4;

                //Director
                Label lblDirec = new Label();
                lblDirec.Content = pelicula.getDirector();
                Grid.SetRow(lblDirec, fil);
                Grid.SetColumn(lblDirec, col);
                tool = new ToolTip();
                tool.Content = pelicula.getTitulo();
                lblDirec.ToolTip = tool;
                gridCompare.Children.Add(lblDirec);
                fil += 4;

                //Reparto
                Label lblReparto = new Label();
                lblReparto.Content = "Pulsame";
                Grid.SetRow(lblReparto, fil);
                Grid.SetColumn(lblReparto, col);
                tool = new ToolTip();
                tool.Content = string.Join("\n", pelicula.getReparto());
                lblReparto.ToolTip = tool;
                gridCompare.Children.Add(lblReparto);
                fil += 4;

                //Sinopsis
                Label lblSinopsis = new Label();
                lblSinopsis.Content = "Pulsame";
                Grid.SetRow(lblSinopsis, fil);
                tool = new ToolTip();
                Grid.SetColumn(lblSinopsis, col);
                tool.Content = string.Join("\n", pelicula.getSinopsis());
                lblSinopsis.ToolTip = tool;
                gridCompare.Children.Add(lblSinopsis);
                fil += 4;

                //Calificacion
                Label lblCali = new Label();
                lblCali.Content = pelicula.getCalificacion(); ;
                Grid.SetRow(lblCali, fil);
                Grid.SetColumn(lblCali, col);
                tool = new ToolTip();
                tool.Content = pelicula.getCalificacion();
                lblCali.ToolTip = tool;
                gridCompare.Children.Add(lblCali);
                fil += 4;

                //Duracion
                Label lblDura = new Label();
                lblDura.Content = pelicula.getDuracion(); ;
                Grid.SetRow(lblDura, fil);
                Grid.SetColumn(lblDura, col);
                tool = new ToolTip();
                tool.Content = pelicula.getTitulo();
                lblDura.ToolTip = tool;
                gridCompare.Children.Add(lblDura);
                fil += 4;

                //Genero
                Label lblgen = new Label();
                lblgen.Content = pelicula.getGenero(); ;
                Grid.SetRow(lblgen, fil);
                Grid.SetColumn(lblgen, col);
                tool = new ToolTip();
                tool.Content = pelicula.getTitulo();
                lblgen.ToolTip = tool;
                gridCompare.Children.Add(lblgen);
                fil += 4;

                //Pais
                Label lblp = new Label();
                lblp.Content = pelicula.getPais(); ;
                Grid.SetRow(lblp, fil);
                Grid.SetColumn(lblp, col);
                tool = new ToolTip();
                tool.Content = pelicula.getTitulo();
                lblp.ToolTip = tool;
                gridCompare.Children.Add(lblp);
                fil += 4;

                //Premios
                Label lblpremio = new Label();
                lblpremio.Content = "Pulsame";
                Grid.SetRow(lblpremio, fil);
                Grid.SetColumn(lblpremio, col);
                tool = new ToolTip();
                tool.Content = string.Join("\n", pelicula.getPremios());
                lblpremio.ToolTip = tool;
                gridCompare.Children.Add(lblpremio);
                
                col += 4;
                fil = 3;
            }
        }
    }
}
