using APPProyecto.MisControles.Entrar;
using APPProyecto.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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


namespace APPProyecto.PaginaVentana.Pelicula
{
    /// <summary>
    /// Lógica de interacción para PeliculaInfo.xaml
    /// </summary>
    public partial class PeliculaInfo : Page
    {
        private Movie movie;
        private BD baseDatos;
        private string user;
        public PeliculaInfo(Movie movie, string us)
        {
            InitializeComponent();
            this.movie = movie;
            user = us;
            baseDatos = new BD();

            //comprobamos el estado de la pelicula respecto al usuario
            //su calificacion y si es pendiente, vista...
            setEstados();

            //Creamos los rectangulos
            RectanguloReparto();
            RectanguloSinopsis();
            //RectanguloPremios();

            //Creamos los UserControl de las peliculas del mismo director utilizando un Hilo
            Thread hilo = new Thread(new ThreadStart(EjecutarHilo));
            hilo.Start();

        }

        //metodos para el boton de la sinopsis
        private void btnSinopsis_MouseEnter(object sender, MouseEventArgs e)
        {
            //visibilizamos debajo del boton
            borderSinopsis.Visibility = Visibility.Visible;
            recSinopsis.Visibility = Visibility.Visible;
        }

        //creamos la informacion de los actores
        private void RectanguloSinopsis()
        {
            //metemos la sinopsis en el textbox
            txtSinopsis.Text = movie.getSinopsis();

            //ajustamos el rectangulo al texto
            double width = txtSinopsis.ActualWidth;
            double height = txtSinopsis.ActualHeight;
            recSinopsis.Width = width + 20;
            recSinopsis.Height = height + 20;
            recSinopsis.Margin = new Thickness(0, btnSinopsis.ActualHeight + 30, 0, 0);
        }

        private void btnSinopsis_MouseLeave(object sender, MouseEventArgs e)
        {
            borderSinopsis.Visibility = Visibility.Hidden;
        }


        //metodos para el boton del reparto
        private void btnReparto_MouseEnter(object sender, MouseEventArgs e)
        {
            //visibilizamos debajo del boton
            borderReparto.Visibility = Visibility.Visible;
            recReparto.Visibility = Visibility.Visible;
        }

        //creamos la informacion de los actores
        private void RectanguloReparto()
        {
            //metemos el reparto en el textbox
            //join: por cada elemento de la lista que devuelve getReparto() añadiremos un salto de linea
            txtReparto.Text = string.Join("\n", movie.getReparto());

            //ajustamos el rectangulo al texto
            double width = txtReparto.ActualWidth;
            double height = txtReparto.ActualHeight;
            recReparto.Width = width + 20;
            recReparto.Height = height + 20;
            recReparto.Margin = new Thickness(0, btnReparto.ActualHeight + 30, 0, 0);
        }

        private void btnReparto_MouseLeave(object sender, MouseEventArgs e)
        {
            borderReparto.Visibility = Visibility.Hidden;
        }

        //insertamos el comentario
        private void btnPublicar_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)cbAnonimo.IsChecked)
                user = "Anónimo";

            baseDatos.InsertComentario(movie.getTitulo(), user, txtComentario.Text, DateTime.Now.ToString("DD-MM-YYYY"), DateTime.Now.ToString("HH:mm:ss"));

        }

        private async void EjecutarHilo()
        {

            List<Movie> mismodir = new List<Movie>();
            await crearUC(mismodir);
            
        }

        private async Task crearUC(List<Movie> mismodir)
        {
            int filas = 0;
            int columnas = 0;


            foreach (Movie pelicula in mismodir)
            {
                UCPelicula ucPelicula = new UCPelicula(user, movie.getTitulo(), movie.ObtenerImagen(), movie.getGenero());

                // Agregamos el margen
                ucPelicula.Margin = new Thickness(10 + columnas * (ucPelicula.Width + 10), 10 + filas * (ucPelicula.Height + 10), 0, 0);

                // Añadimos al Border
                borderDirector.Child = ucPelicula;

                columnas++;
                if (columnas >= 3)
                {
                    columnas = 0;
                    filas++;
                }
            }

            // Colocamos el Border en el Grid
            gridDirector.Children.Add(borderDirector);
        }

        private void btnPremios_MouseEnter(object sender, MouseEventArgs e)
        {
            //visibilizamos debajo del boton
            borderPremio.Visibility = Visibility.Visible;
            recPremio.Visibility = Visibility.Visible;
        }

        //creamos la informacion de los actores
        private void RectanguloPremios()
        {
            //metemos el reparto en el textbox
            //join: por cada elemento de la lista que devuelve getReparto() añadiremos un salto de linea
            txtPremio.Text = string.Join("\n", movie.getPremios());

            //ajustamos el rectangulo al texto
            double width = txtPremio.ActualWidth;
            double height = txtPremio.ActualHeight;
            recPremio.Width = width + 20;
            recPremio.Height = height + 20;
            recPremio.Margin = new Thickness(0, btnPremios.ActualHeight + 30, 0, 0);
        }

        private void btnPremios_MouseLeave(object sender, MouseEventArgs e)
        {
            borderPremio.Visibility = Visibility.Hidden;
        }

        public void setEstados()
        {
            //Actualizamos el estado de la pelicula
            comboEstado.SelectedIndex = baseDatos.InfoUsuarioPelicula(user, movie.getTitulo());

            //Actualizamos el combo box con la calificacion del usuario
            int num = baseDatos.getCaliUser(user, movie.getTitulo());
            if (num != -1)
                comboCali.SelectedIndex = num;

        }

        private void comboCali_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            baseDatos.ModCaliUser(user, movie.getTitulo(), comboCali.SelectedIndex, movie.getGenero());

            //como se ha actualizado la calificación habrá que actualizar el porcentaje
            ActualizarCaliUser();
        }

        private void comboEstado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            baseDatos.ModEstadoPeli(user, movie.getTitulo(), comboEstado.SelectedIndex, movie.getGenero());
        }

        private void ActualizarCaliUser()
        {
            //Debemos tener el numero de personas que han votado sobre la pelicula en concreto
            int votos = baseDatos.getCaliUser(user, movie.getTitulo());

            //Debemos saber todas las valoraciones de la pelicula en concreto
            List<int> valoraciones = baseDatos.getValoraciones(movie.getCalificacion());

            //Calculamos la valoracion de todos los usuarios
            int caliusers = 0;
            foreach (int voto in valoraciones)
            {
                caliusers += voto;
            }

            //asignamos
            lblCaliUser.Content = (caliusers / votos) + "%";

        }
    }
}
