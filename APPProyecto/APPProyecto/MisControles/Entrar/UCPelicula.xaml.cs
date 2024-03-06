using APPProyecto.MVC;
using APPProyecto.PaginaVentana;
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

namespace APPProyecto.MisControles.Entrar
{
    /// <summary>
    /// User control que se utilizará para gestionar las peliculas
    /// Incluye dos eventos para gestionar la informacion entre páginas
    /// </summary>
    public partial class UCPelicula : UserControl
    {
        private BD basedatos;
        private string titulo;
        private string imagen;
        private string correo;
        private string genero;
        public event EventHandler<string>? MandarPelicula;
        public event EventHandler<Movie>? MandarMovie;
        private bool compro;

        /// <summary>
        /// Constructor del User Control
        /// Comprueba si el poste de la pelicula es null.
        /// Comprueba el estado de la pelicula para establecer correctamente los botones de favorito o pendiente
        /// </summary>
        /// <param name="corr">Correo del usuario</param>
        /// <param name="titulo">Titulo de la pelicula</param>
        /// <param name="imagen">Imagend e la pelicula</param>
        /// <param name="genero">Genero de la pelicula</param>
        public UCPelicula(string corr, string titulo, string imagen, string genero)
        {
            InitializeComponent();
            this.titulo = titulo;
            this.imagen = imagen;
            this.genero = genero;
            correo = corr;
            compro = false;
            basedatos = new BD();

            //Asignar la imagen bitmap a Image del xaml
            if (imagen != null && imagen != "N/A")
                Poster.Source = new BitmapImage(new Uri(imagen));
            else
                Poster.Source = new BitmapImage(new Uri("/Resources/notfound.png", UriKind.Relative));

            //comprobamos los datos del usuario sobre esa pelicula
            int estado = basedatos.InfoUsuarioPelicula(correo, titulo);
            //int estado = 1;
            if (estado == 1)
            {
                pendienteTrue.Visibility = Visibility.Visible;
                pendienteFalse.Visibility = Visibility.Hidden;
                favoritoFalse.Visibility = Visibility.Visible;
                favoritoTrue.Visibility = Visibility.Hidden;
            }
            else if (estado == 3)
            {
                pendienteTrue.Visibility = Visibility.Hidden;
                pendienteFalse.Visibility = Visibility.Visible;
                favoritoFalse.Visibility = Visibility.Hidden;
                favoritoTrue.Visibility = Visibility.Visible;
            }
            else
            {
                favoritoFalse.Visibility = Visibility.Visible;
                pendienteFalse.Visibility = Visibility.Visible;
                favoritoTrue.Visibility = Visibility.Hidden;
                pendienteTrue.Visibility = Visibility.Hidden;
            }

            Poster.MouseRightButtonDown += Poster_MouseRightButtonDown;
        }

        private void Poster_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //mostrara la informacion de la pelicula
        }

        /// <summary>
        /// Evento en el que al hacer click dereccho e mandará el titulo de la pelicula a otra página WPF
        /// Tambien controlaremos el estado del border del user control para saber si la pelicula esta
        /// seleccionada o no
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Poster_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Al hacer clic derecho, invocamos el evento para mandar el título a la página WPF
            MandarPelicula?.Invoke(this, titulo);
            if (compro)
            {
                bordeComprobar.BorderBrush = Brushes.Red;
                compro = false;
            }
            else
            {
                bordeComprobar.BorderBrush = Brushes.GreenYellow;
                compro = true;
            }
        }

        public void ComproTrue()
        {
            compro = true;
        }

        public void ComproFalse()
        {
            compro = false;
        }

        /// <summary>
        /// Evento que actualiza la pelicula como pendiente y deje visible el boton de pendiente
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pendienteTrue.Visibility = Visibility.Visible;
            pendienteFalse.Visibility = Visibility.Hidden;
            favoritoFalse.Visibility = Visibility.Visible;
            favoritoTrue.Visibility = Visibility.Hidden;

            basedatos.ModEstadoPeli(correo, titulo, 1, genero);
        }

        /// <summary>
        /// Evento que quita de pendiente la pelicula y actualiza los botones
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            pendienteTrue.Visibility = Visibility.Hidden;
            pendienteFalse.Visibility = Visibility.Visible;
            favoritoFalse.Visibility = Visibility.Hidden;
            favoritoTrue.Visibility = Visibility.Hidden;

            basedatos.ModEstadoPeli(correo, titulo, 0, genero);
        }

        //se añade de la lista de favoritos y ademas se debe ocultar y dejar visible el otro boton
        /// <summary>
        /// Evento que pone la pelicula en favoritos y actualiza los botones
        /// </summary>
        private void favoritoFalse_Click(object sender, RoutedEventArgs e)
        {
            //dejamos visible el otro boton
            favoritoFalse.Visibility = Visibility.Hidden;
            favoritoTrue.Visibility = Visibility.Visible;
            pendienteTrue.Visibility = Visibility.Hidden;
            pendienteFalse.Visibility = Visibility.Visible;

            //eliminamos de la lista de favoritos
            basedatos.ModEstadoPeli(correo, titulo, 3, genero);
        }

        //se quita de la lista de favoritos y ademas se debe ocultar y dejar visible el otro boton
        /// <summary>
        /// Evento que quita la peli de favoritos y actualiza los botones
        /// </summary>
        private void favoritoTrue_Click(object sender, RoutedEventArgs e)
        {
            //dejamos visible el otro boton
            favoritoFalse.Visibility = Visibility.Visible;
            favoritoTrue.Visibility = Visibility.Hidden;
            pendienteTrue.Visibility = Visibility.Hidden;
            pendienteFalse.Visibility = Visibility.Hidden;

            basedatos.ModEstadoPeli(correo, titulo, 2, genero);
        }

        /// <summary>
        /// Evento que al pulsar en la imagen busca la info detallada en la API y la envía
        /// </summary>
        private async void Poster_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Movie esperada = basedatos.BuscarPeliBD(titulo);

            if (esperada == null)
            {
                esperada = await omdb.PeliEspecifica(titulo);
                basedatos.InsertatPeli(esperada);
                MandarMovie?.Invoke(this, esperada);
            }
            else
            {
                MandarMovie?.Invoke(this, esperada);
            }
        }
    }
}
