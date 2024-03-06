using APPProyecto.MisControles.Entrar;
using APPProyecto.MisControles.Pelicula;
using APPProyecto.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace APPProyecto.PaginaVentana.Peliculas
{
    /// <summary>
    /// Página que muestra la información de una única película.
    /// </summary>
    public partial class Informacion : Page
    {
        private Movie movie;
        private BD basedatos;
        private TMDB apidos;
        private Usuario user;


        
        /// <summary>
        /// Constructor de la clase Informacion.
        /// Coloca la información de la película.
        /// Coloca la informacion del usuario. (Vista, pendiente... y la calificacion)
        /// Coloca la calificación total de todos los usuarios de la aplicación.
        /// Generá un hilo que busca el ID del director, y busca todas las peliculas donde haya trabajado, esto incluye como guionista, director, actor...
        /// Coloca el apartado donde el usuario podrá insertar los comentarios de la pelicula
        /// Coloca los comentarios que cada usuario ha realizado en la pelicula.
        /// </summary>
        /// <param name="movie">Objeto Movie el cual contiene la información de la pelicula</param>
        /// <param name="us">Objeto Usuario el cual contiene la información del usuario</param>

        public Informacion(Movie movie, Usuario us)
        {
            InitializeComponent();
            this.movie = movie;
            user = us;
            basedatos = new BD();
            apidos = new TMDB();

            //Colocar info de la peli solo textos y poster
            //Colocamos la calificacion INDIVIDUAL del usuario
            //Colocamos la calificacion GRUPAL de los usuario
            //Colocamos el estado de la pelicula
            ColocInfoPeli();
            ColocCaliUser();
            ColocCaliGrupo();
            ColocEstPeli();
            
            //Creamos los UserControl de las peliculas del mismo director utilizando un Hilo
            Thread hilo = new Thread(new ThreadStart(EjecutarHilo));
            hilo.Start();

            //Colocamos los comentarios
            ColocComent();

        }

        /// <summary>
        /// Metodo que coloca la información de la pelicula uscada. (Director, reparto, año...)
        /// </summary>
        //metodo que coloca la información de la peli
        private void ColocInfoPeli() {
            lblTitulo.Content = movie.getTitulo();
            lblAnio.Content = movie.getAnio();
            lblDirector.Content = movie.getDirector();
            lblDuracion.Content = movie.getDuracion();
            lblGenero.Content = movie.getGenero();
            lblPais.Content = movie.getPais();

            if(movie.ObtenerImagen() == "N/A")
                imgPoster.Source = new BitmapImage(new Uri("/Resources/notfound.png", UriKind.Relative));
            else
                imgPoster.Source = new BitmapImage(new Uri(movie.ObtenerImagen()));

            lblCaliApi.Content = (movie.getCalificacion() + "%");
            btnSinopsis.ToolTip = (movie.getSinopsis());
            btnPremios.ToolTip = (movie.getPremios());
        }

        /// <summary>
        /// Coloca la información del usuario en cada combobox, si el usuario no interactuado con la pelicula mostrará la opción por defecto.
        /// </summary>
        private void ColocCaliUser()
        {
            //calificacion del usuario
            int caliuser = basedatos.getCaliUser(user.Correo, movie.getTitulo());
            if (caliuser == -1)
            {
                comboCali.SelectedIndex = 0;
            }
            else
            {
                foreach (ComboBoxItem item in comboCali.Items)
                {
                    if (item.Content.ToString() == caliuser.ToString())
                    {
                        comboCali.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        //Colocamos la calificacion de todos los usuarios de la aplicacion sobre esta pelicula
        /// <summary>
        /// Calcula la calificación del total de usuarios para esta pelicula en concreto
        /// Obtiene el numero total de votos y la calificación de cada usuario (individualmente) calcula la media y muestra el resultado.
        /// Se controla que el divisor no sea 0
        /// </summary>
        private void ColocCaliGrupo()
        {
            //Primero obtenemos el numero de personas que han votado sobre esta pelicula
            int num = basedatos.getNumVotos(movie.getTitulo());
            //obtenemos la valoraciones individuales de todos los usuarios para esta pelicula
            List<int> valoraciones = basedatos.getValoraciones(movie.getTitulo());
            //sumamos todas las valoraciones
            int sum = 0;
            foreach (int voto in valoraciones)
                sum += voto;
            //hacemos la media, controlamos si el divisor es 0
            if (num == 0)
                lblCaliUser.Content = "0%";
            else
            lblCaliUser.Content = (sum * 10) / num + "%";
        }

        //colocamos el combobox dependiento el estado de la peli
        /// <summary>
        /// Coloca el estado de la pelicula del usuario actual en el combobox.
        /// </summary>
        private void ColocEstPeli()
        { 
            //lista pelicula
            int est = basedatos.InfoUsuarioPelicula(user.Correo, movie.getTitulo());
            foreach (ComboBoxItem item in comboEstado.Items)
            {
                //utilizo el metodo para no engordar este
                if (item.Content.ToString() == CompararNum(est))
                {
                    comboEstado.SelectedItem = item;
                    break;
                }
            }
        }

        private string CompararNum(int valor)
        {
            switch (valor)
            {
                case 0:
                    return "Nada";
                case 1:
                    return "Pendiente";
                case 2:
                    return "Vista";
                case 3:
                    return "Favorita";
                default:
                    return "Nada";
            }
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



        //insertamos el comentario
        /// <summary>
        /// Metodo que inserta un comentario en la base de datos.
        /// Comprueba si el checkbox esta activado en este caso el nombre de usuario será Anónimo
        /// Por ultimo inserta el comentario con el titulo de la pelicula, el correo del usuario, el nombre y la fecha actual en un formato en concreto.
        /// </summary>
        private void btnPublicar_Click(object sender, RoutedEventArgs e)
        {
            string nom;
            if ((bool)cbAnonimo.IsChecked)
                nom = "Anónimo";
            else
                nom = user.Nombre;

            basedatos.InsertComentario(movie.getTitulo(), user.Correo, nom, txtComentario.Text, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

            ColocComent();

        }
        
        /// <summary>
        /// Ejecuta un hilo para obtener las peliculas donde el director haya trabajado utilizando la API TMDB
        /// Se obtendrá el id del director de la pelicula actual, buscando en la API por el nombre del director.
        /// Utilizando el ID obtenido se realizará una busqueda de las peliculas
        /// Si el ID no se encuentra se ocultará este apartado
        /// </summary>
        private async void EjecutarHilo()
        {
            int id = await TMDB.IdDirector(movie.getDirector());
            if(id == -8888)
            {
                Dispatcher.Invoke(() =>
                {
                    gridDirector.Visibility = Visibility.Hidden;
                });
            }
            else
            {
                List<Movie> listamismodirec = new List<Movie>();
                for(int i = 4; i > 0; i--)
                {
                    Movie esperada = await TMDB.MismoDirector(id);
                    if (esperada == null)
                        i++;
                    else {
                        if (listamismodirec.Count == 0)
                            listamismodirec.Add(esperada);
                        else if (!listamismodirec.Any(p => p.getTitulo() == esperada.getTitulo()))
                        {
                            listamismodirec.Add(esperada);
                        }
                        else
                        {
                            i++;
                        }
                    }
                    
                }
                await crearUC(listamismodirec);
            }
        }

        /// <summary>
        /// Crea una serie de UserControls de las peliculas obtenidas con el mismo director y los coloca en la interfaz de usuario
        /// </summary>
        /// <param name="mismodir">Lista de películas del mismo director.</param>
        private async Task crearUC(List<Movie> mismodir)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;

                foreach (var pelicula in mismodir)
                {
                    UCPelicula ucPelicula = new UCPelicula(user.Correo, pelicula.getTitulo(), pelicula.ObtenerImagen(), pelicula.getGenero());
                    ucPelicula.Width = 160;
                    ucPelicula.Height = 200;
                    ucPelicula.Margin = new Thickness(0, 0, 10, 0);
                    ucPelicula.MandarMovie += UcPelicula_ManejarMovie;
                    ucPelicula.bordeComprobar.BorderBrush = Brushes.Gray;
                    stackPanel.Children.Add(ucPelicula);
                }

                gridDirector.Children.Add(stackPanel);
            }, System.Windows.Threading.DispatcherPriority.Background);
        }

        /// <summary>
        /// Evento de la picula si se hace clic en el se creará una nueva página con la información de la pelicula del user control.
        /// </summary>
        /// <param name="peli">Objeto Movie que contiene la pelicula del user control..</param>
        private void UcPelicula_ManejarMovie(object sender, Movie peli)
        {
            PaginaVentana.Peliculas.Informacion info = new PaginaVentana.Peliculas.Informacion(peli, user);
            this.NavigationService.Navigate(info);

        }

        private void btnPremios_MouseEnter(object sender, MouseEventArgs e)
        {
            //visibilizamos debajo del boton
            borderPremio.Visibility = Visibility.Visible;
            recPremio.Visibility = Visibility.Visible;
        }

        

        private void btnPremios_MouseLeave(object sender, MouseEventArgs e)
        {
            borderPremio.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Cada vez que cambie la selección del combobox de la calificación se actualizará en el registro de esta pelicula, con el
        /// usuario actual en la base de datos
        /// Actualiza el procentaje en la parte de calificación
        /// </summary>
        /// <param name="e">Argumentos del evento de cambio de selección.</param
        private void comboCali_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            basedatos.ModCaliUser(user.Correo, movie.getTitulo(), comboCali.SelectedIndex, movie.getGenero());

            //como se ha actualizado la calificación habrá que actualizar el porcentaje
            ActualizarCaliUser();
        }

        /// <summary>
        /// Cada vez que se camia la selección del combobox del estado de la pelicula se actualizará el registro en la base de datos
        /// </summary>
        private void comboEstado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            basedatos.ModEstadoPeli(user.Correo, movie.getTitulo(), comboEstado.SelectedIndex, movie.getGenero());
        }

        /// <summary>
        /// Actualiza el porcentaje de la calificacion grupal en el apartado de calificación.
        /// Obtiene el numero de votos totales y los votos individuales de todos los usuarios y hace la media.
        /// 
        /// </summary>
        private void ActualizarCaliUser()
        {
            //Debemos tener el numero de personas que han votado sobre la pelicula en concreto
            int votos = basedatos.getCaliUser(user.Correo, movie.getTitulo());

            //Debemos saber todas las valoraciones de la pelicula en concreto
            List<int> valoraciones = basedatos.getValoraciones(movie.getCalificacion());

            //Calculamos la valoracion de todos los usuarios
            int caliusers = 0;
            foreach (int voto in valoraciones)
            {
                caliusers += voto;
            }

            //asignamos
            if (votos < 1)
                lblCaliUser.Content = 0 + "%";
            else
                lblCaliUser.Content = (caliusers / votos) * 10 + "%";

        }

        //metodo que coloca los user control por cada comentario que exista de esta pelicula
        /// <summary>
        /// Coloca los comentarios de los usuarios en su apartado
        /// Limpia el contenido.
        /// Crea UserControls por cada comentario obtenido
        /// Los agrega al grid y los separa usando un separator
        /// </summary>
        public void ColocComent()
        {
            gridListarComentario.Children.Clear();
            List<Comentario> listaComentarios = basedatos.getComentarios(movie.getTitulo());
            StackPanel stackComent = new StackPanel();

            // Recorre la lista de comentarios y crea UCComentario para cada uno
            for (int i = 0; i < listaComentarios.Count; i++)
            {
                // Crea un nuevo UCComentario
                UCComentario ucComentario = new UCComentario(listaComentarios[i]);
                ucComentario.DataContext = listaComentarios[i];
                ucComentario.Width = 500;
                ucComentario.Height = 150;
                stackComent.Children.Add(ucComentario);

                if (i < listaComentarios.Count - 1)
                {
                    stackComent.Children.Add(new Separator { Height = 15 });
                }

            }
            gridListarComentario.Children.Add(stackComent);
        }
    }
}
