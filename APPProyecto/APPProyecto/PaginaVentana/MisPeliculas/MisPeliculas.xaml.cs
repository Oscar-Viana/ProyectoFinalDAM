using APPProyecto.MisControles.Entrar;
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

namespace APPProyecto.PaginaVentana.MisPeliculas
{
    /// <summary>
    /// Pagina que muestra las peliculas pendientes, vistas y favoritas del usuario actual.
    /// </summary>
    public partial class MisPeliculas : Page
    {
        private BD basedatos;
        private Usuario user;
        private omdb apiomdb;

        private List<string> pendientes;
        private List<string> vistas;
        private List<string> favorita;
        private List<Movie> PeliPend;
        private List<Movie> PeliVista;
        private List<Movie> PeliFav;


        public MisPeliculas(Usuario us)
        {
            InitializeComponent();
            basedatos = new BD();
            user = us;
            apiomdb = new omdb();

            //Rellenar Grid
            RellenarGrid();

            pelisde.Content = "Peliculas de " + us.Nombre;
        }

        /// <summary>
        /// Evento que se genera cunado se pulsa clic izquierdo sobre la pelicula.
        /// Mostrará una página con la información detallada de la pelicula almacenada en el user control
        /// </summary>
        private void UcPelicula_ManejarMovie(object sender, Movie peli)
        {
            PaginaVentana.Peliculas.Informacion info = new PaginaVentana.Peliculas.Informacion(peli, user);
            this.NavigationService.Navigate(info);
        }

        //metodo que rellena los grid de pendiente vistas y favoritas
        /// <summary>
        /// Almacena en cada lista las peliculas del usuario corrspondiente a ellas 
        /// Busca cada pelicula en la api omdb
        /// Invoca los metodos para colocar las peliculas en su apartado concreto
        /// </summary>
        public async void RellenarGrid()
        {
            pendientes = basedatos.getListasUser(user.Correo, "1");
            vistas = basedatos.getListasUser(user.Correo, "2");
            favorita = basedatos.getListasUser(user.Correo, "3");
            PeliPend = new List<Movie>();
            PeliVista = new List<Movie>();
            PeliFav = new List<Movie>();

            if(pendientes != null)
            {
                for (int i = 0; i < pendientes.Count; i++)
                {
                    Movie esperada = await omdb.PeliEspecifica(pendientes[i]);
                    PeliPend.Add(esperada);
                }
                GPendiente();
            }

            if(vistas != null)
            {
                for (int i = 0; i < vistas.Count; i++)
                {
                    Movie esperada = await omdb.PeliEspecifica(vistas[i]);
                    PeliVista.Add(esperada);
                }
                GVista();
            }

            if(favorita != null)
            {
                for (int i = 0; i < favorita.Count; i++)
                {
                    Movie esperada = await omdb.PeliEspecifica(favorita[i]);
                    PeliFav.Add(esperada);
                }
                GFavorita();
            }

        }

        /// <summary>
        /// Coloca las películas pendientes del usuario en la interfaz creando user controls
        /// </summary>
        public void GPendiente()
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;

            foreach (Movie pelicula in PeliPend)
            {
                UCPelicula ucPelicula = new UCPelicula(user.Correo, pelicula.getTitulo(), pelicula.ObtenerImagen(), pelicula.getGenero());
                ucPelicula.DataContext = pelicula;
                ucPelicula.Width = 160;
                ucPelicula.Height = 200;
                ucPelicula.pendienteFalse.Visibility = Visibility.Collapsed;
                ucPelicula.pendienteTrue.Visibility = Visibility.Collapsed;
                ucPelicula.favoritoFalse.Visibility = Visibility.Collapsed;
                ucPelicula.favoritoTrue.Visibility = Visibility.Collapsed;


                //Solo suscribimos el evento de ver info peli
                ucPelicula.MandarMovie += UcPelicula_ManejarMovie;

                //Agrega el UserControl al StackPanel
                panel.Children.Add(ucPelicula);

                //Agrega un separador si no es la última película
                if (PeliPend.IndexOf(pelicula) < PeliPend.Count - 1)
                {
                    panel.Children.Add(new Separator { Height = 15 });
                }
            }

            // Limpia el contenido actual del GridPendiente y agrega el nuevo StackPanel
            gridPendientes.Children.Clear();
            gridPendientes.Children.Add(panel);
        }

        /// <summary>
        /// Coloca las películas vistas del usuario en la interfaz creando user controls
        /// </summary>
        public void GVista()
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;

            foreach (Movie pelicula in PeliVista)
            {
                UCPelicula ucPelicula = new UCPelicula(user.Correo, pelicula.getTitulo(), pelicula.ObtenerImagen(), pelicula.getGenero());
                ucPelicula.DataContext = pelicula;
                ucPelicula.Width = 160;
                ucPelicula.Height = 200;
                ucPelicula.pendienteFalse.Visibility = Visibility.Collapsed;
                ucPelicula.pendienteTrue.Visibility = Visibility.Collapsed;
                ucPelicula.favoritoFalse.Visibility = Visibility.Collapsed;
                ucPelicula.favoritoTrue.Visibility = Visibility.Collapsed;

                ucPelicula.MandarMovie += UcPelicula_ManejarMovie;

                panel.Children.Add(ucPelicula);

                if (PeliPend.IndexOf(pelicula) < PeliPend.Count - 1)
                {
                    panel.Children.Add(new Separator { Height = 15 });
                }
            }

            gridVistas.Children.Clear();
            gridVistas.Children.Add(panel);
        }

        /// <summary>
        /// Coloca las películas favoritas del usuario en la interfaz creando user controls
        /// </summary>
        public void GFavorita()
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;

            foreach (Movie pelicula in PeliFav)
            {
                UCPelicula ucPelicula = new UCPelicula(user.Correo, pelicula.getTitulo(), pelicula.ObtenerImagen(), pelicula.getGenero());
                ucPelicula.DataContext = pelicula;
                ucPelicula.Width = 160;
                ucPelicula.Height = 200;
                ucPelicula.pendienteFalse.Visibility = Visibility.Collapsed;
                ucPelicula.pendienteTrue.Visibility = Visibility.Collapsed;
                ucPelicula.favoritoFalse.Visibility = Visibility.Collapsed;
                ucPelicula.favoritoTrue.Visibility = Visibility.Collapsed;

                ucPelicula.MandarMovie += UcPelicula_ManejarMovie;

                panel.Children.Add(ucPelicula);

                if (PeliPend.IndexOf(pelicula) < PeliPend.Count - 1)
                {
                    panel.Children.Add(new Separator { Height = 15 });
                }
            }

            gridFavoritas.Children.Clear();
            gridFavoritas.Children.Add(panel);

        }
    }
}
