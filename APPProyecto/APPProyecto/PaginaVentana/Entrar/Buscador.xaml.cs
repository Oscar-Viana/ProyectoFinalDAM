using APPProyecto.MisControles.Entrar;
using APPProyecto.MVC;
using APPProyecto.PaginaVentana.Principal;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace APPProyecto.PaginaVentana.Entrar
{
    /// <summary>
    /// Página principal de la aplicación.
    /// En ella se encuentran cuatroa apartados.
    /// Apartado Usuario: Muestra la información del usuario como el nombre y el avatar además de un boton de opciones
    /// Apartado Recomendación: Muestra peliculas recomendadas segun las preferencias del usuario.
    /// Apartado Buscador: Muestra un textbox donde se insertará una cadena la cual se utilizará para buscar peliculas en la api OMDB.
    /// Apartado Comparación: Muetsra dos botones para comparar peliculas o para desselecionar las peliculas previamente seleccionadas.
    /// </summary>
    public partial class Buscador : Page
    {
        //Lista de peliculas para comprobar
        private List<string> titulosComprobar = new List<string>();
        private BD basedatos;
        private TMDB apidos;
        private Usuario user;

        /// <summary>
        /// Contructor de la página, crea el menu contextual, coloca los datos del usuario actual y busca las recomendaciones.
        /// Oculta el grid de comprobar peliculas porque no se ha sellecionado ninguna.
        /// </summary>
        /// <param name="usuario">Objeto Usuario que contiene el usuario que ha hecho login.</param>
        public Buscador(Usuario usuario)
        {
            InitializeComponent();
            user = usuario;
            basedatos = new BD();
            apidos = new TMDB();
            MenuContextual();
            Bienvenida(user);

            //Recomendamos peliculas
            RecomendarPelis();
            gridCompro.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Da la bienvenida al usuario actual y muestra su avatar.
        /// </summary>
        /// <param name="user">Usuario actual</param>
        private void Bienvenida(Usuario user) {
            //Damos la bienvenida
            lblBienvenida.Content = "Bienvenido/a " + user.Nombre;
            //Mostramos el avatar
            imgAvatar.Source = basedatos.CargarAvatar(Convert.ToInt32(user.Avatar));
        }

        //recogemos la pelicula que se ha seleccionado
        /// <summary>
        /// Controla el evento de clic derecho del user control para seleccionar o desseleccionar la pelicula.
        /// </summary>
        /// <param name="titulo">Titulo de la pelicula del user control.</param>
        /// <param name="sender"></param>
        private void UcPelicula_MandarPelicula(object sender, string titulo)
        {
            //si esta en la lista se elimina
            //si no esta en la lista se añade
            if (titulosComprobar.Contains(titulo))
                titulosComprobar.Remove(titulo);
            else
                titulosComprobar.Add(titulo);

            //comprobamos cuantos elementos tiene
            //si tiene mas de uno mostrara el boton para comprobar peliculas
            //si no tiene mas de uno ocultara el boton
            if (titulosComprobar.Count > 1)
            {
                gridCompro.Visibility = Visibility.Visible;
            }
            else
            {
                gridCompro.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Controla el vento de click izquierdo del user control, muestra una pagina con la información detallada de la pelicula del user control.
        /// </summary>
        /// <param name="peli">Titulo de la pelicula almacenada en el user control.</param>
        private void UcPelicula_ManejarMovie(object sender, Movie peli)
        {
            PaginaVentana.Peliculas.Informacion info = new PaginaVentana.Peliculas.Informacion(peli, user);
            this.NavigationService.Navigate(info);
            
        }

        //metodo que crea los UserControl dependiendo del numero de peliculas que contenga la cadena que se ha buscado
        /// <summary>
        /// Metodo que busca peliculas en la api OMDB dependiendo de la cadena de texto escrita previamente.
        /// Primero comprueba que el textbox no esté vacío.
        /// Busca una lista de peliculas en la api.
        /// Por cada pelicula en la lista crea un user control, suscribe sus eventos y los almacena en el grid
        /// </summary>
        private async void BtnBuscador_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBuscador.Text))
            {
                MessageBox.Show("Primero debes rellenar el campo de búsqueda");
            }
            else
            {
                List<Movie> peliculas = await omdb.PeliculasAmplias(txtBuscador.Text);
                scrollBuscador.Content = null;

                if (peliculas != null && peliculas.Count > 0)
                {
                    int userControlWidth = 160;
                    int userControlHeight = 200;
                    int marginBetweenUserControls = 15;
                    StackPanel panelPeliculas = new StackPanel();

                    for (int i = 0; i < peliculas.Count; i++)
                    {
                        //creamos el usercontrol de la pelicula
                        UCPelicula ucPelicula = new UCPelicula(user.Correo, peliculas[i].getTitulo(), peliculas[i].ObtenerImagen(), peliculas[i].getGenero());
                        ucPelicula.DataContext = peliculas[i];
                        ucPelicula.Width = userControlWidth;
                        ucPelicula.Height = userControlHeight;
                        panelPeliculas.Children.Add(ucPelicula);

                        //suscribimos a los eventos
                        ucPelicula.MandarPelicula += UcPelicula_MandarPelicula;
                        ucPelicula.MandarMovie += UcPelicula_ManejarMovie;

                        if (titulosComprobar.Contains(peliculas[i].getTitulo()))
                        {
                            ucPelicula.bordeComprobar.BorderBrush = Brushes.GreenYellow;
                            ucPelicula.ComproTrue();
                        }

                        if (i < peliculas.Count - 1)
                        {
                            panelPeliculas.Children.Add(new Separator { Height = marginBetweenUserControls });
                        }
                    }

                    scrollBuscador.Content = panelPeliculas;
                }
            }

        }

        /// <summary>
        /// Metodo que muestra el menu contextual en la posición del raton
        /// </summary>
        /// <param name="sender">El objeto que envía el evento.</param>
        /// <param name="e">Los datos del evento.</param>
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (toggleButton.ContextMenu != null)
            {
                Point point = toggleButton.PointToScreen(new Point(0, toggleButton.ActualHeight));
                toggleButton.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                toggleButton.ContextMenu.HorizontalOffset = point.X;
                toggleButton.ContextMenu.VerticalOffset = point.Y;

                toggleButton.ContextMenu.IsOpen = true;
            }
        }

        /// <summary>
        /// Metodo que elimina las peliculas seleccionadas para la comprobacion.
        /// Como no hay peliculas seleccionadas oculta este apartado.
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            titulosComprobar.Clear();

            foreach (var item in ((StackPanel)scrollBuscador.Content).Children)
            {
                if (item is UCPelicula ucPelicula)
                {
                    ucPelicula.bordeComprobar.BorderBrush = Brushes.Red;
                    ucPelicula.ComproFalse();
                }
            }

            gridCompro.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Boton que muestra la página de comprobacion de peliculas con las peliculas seleccionadas previaente
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Peliculas.Comparar compara = new Peliculas.Comparar(titulosComprobar);
            this.NavigationService.Navigate(compara);
        }

        //metodo para ir a la listas de peliculas del usuario
        /// <summary>
        /// Metodo que muestra la pagina de las peliculas del usuario actual.
        /// Este metodo se invoca cuando se pulsa el boton Mis Peliculas del apartado del usuario o en el menu contextual
        /// </summary>
        private void MisPelisClick(object sender, RoutedEventArgs e)
        {
            MisPeliculas.MisPeliculas mispelis = new MisPeliculas.MisPeliculas(user);
            this.NavigationService.Navigate(mispelis);
        }

        //metodo para ir a la página de ajustes del usuario
        /// <summary>
        /// Metodo que muestra la página de opciones
        /// Este metodo se invoca cuando se pulsa el boton Ajustes del apartado del usuario o en el menu contextual
        /// </summary>
        private void AjustesClick(object sender, RoutedEventArgs e)
        {
            Ajustes.Ajustes seetings = new Ajustes.Ajustes(user);
            this.NavigationService.Navigate(seetings);

        }

        //metodo para cerrar sesion, cierra la aplicacion
        /// <summary>
        /// Metodo que funciona diferente dependiendo del rol.
        /// Si es un usuario normal mostrará la página de Bloqueo la cual mostrará la página de Login.
        /// Si es un administrador se cerrará la ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CerrarSesionClick(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
            if (Convert.ToInt32(user.Rol) < 1)
            {
                Bloqueo bloc = new Bloqueo();
                this.NavigationService.Navigate(bloc);
            }
            else
            {
                Window estaventana = Window.GetWindow(this);
                estaventana.Close();
            }
        }

        //metodo que recomendará 5 peliculas con los siguientes criterios:
        //recomienda peliculas del genero favorito del usuario que no haya visto
        //si no hay mas de X registros recomendara peliculas más repetidas no vistas favoritas de entre todos los usuarios
        //si no hay mas de X registros recomendara peliculas más repetidas no vistas por el usuario y vistas de entre todos los usuarios
        //si no hay más de X registros recomendará peliculas no vistas del año actual y del anterior
        //si ha visto todas las peliculas anteriores recomendará peliculas no vistas de todas las épocas
        /// <summary>
        /// Metodo que recomienda peliculas al usuario usando el siguiente criterio:
        ///recomienda peliculas del genero favorito del usuario que no haya visto
        ///si no hay mas de X registros recomendara peliculas más repetidas no vistas favoritas de entre todos los usuarios
        ///si no hay mas de X registros recomendara peliculas más repetidas no vistas por el usuario y vistas de entre todos los usuarios
        ///si no hay más de X registros recomendará peliculas no vistas del año actual y del anterior
        ///si ha visto todas las peliculas anteriores recomendará peliculas no vistas de cualquier época.
        ///
        /// Una vez obtenidas las peliculas creará los usercontrols y los incorporará en su apartado.
        /// 
        /// </summary>
        private async void RecomendarPelis()
        {
            try
            {
                //Lista de peliculas
                List<string> generos;

                //Lista de peliculas buscadas
                List<Movie> peliculas = new List<Movie>();

                //Lista de peliculas que se colocan en el grid
                List<Movie> final = new List<Movie>();

                string popu;
                string idPopu;
                int[] anios = {DateTime.Now.Year, DateTime.Now.Year - 1};
                Random random = new Random();

                //Buscar por genero favorito del usuario
                generos = basedatos.getGenero(user, 3);
                if(generos.Count() > 10) {
                    //obtenemos el genero mas popular
                    popu = GeneroTrim(GenPopular(generos));
                    if (popu != "Desconocido")
                    {
                        idPopu = basedatos.getGenerosPorPopu(popu);
                        //obtenemos 5 peliculas aleatorias que no haya visto
                        final = await BuscaCinco(apidos, idPopu);
                        peliculas = final;
                    }


                }
                //Buscar peliculas vistas del usuario
                else
                {
                    generos = basedatos.getGenero(user, 2);
                    if(generos.Count > 10)
                    {
                        popu = GeneroTrim(GenPopular(generos));
                        if (popu != "Desconocido")
                        {
                            idPopu = basedatos.getGenerosPorPopu(popu);
                            final = await BuscaCinco(apidos, idPopu);
                            peliculas = final;
                        }
                    }
                    //peliculas favoritas de todos
                    else
                    {
                        generos = basedatos.getGeneroTodos(user, 3);
                        if (generos.Count > 20)
                        {
                            popu = GeneroTrim(GenPopular(generos));
                            if (popu != "Desconocido")
                            {
                                idPopu = basedatos.getGenerosPorPopu(popu);
                                final = await BuscaCinco(apidos, idPopu);
                                peliculas = final;
                            }
                        }
                        //Peliculas Vistas de todos
                        else {
                            generos = basedatos.getGeneroTodos(user, 2);
                            if (generos.Count > 20)
                            {
                                popu = GeneroTrim(GenPopular(generos));
                                if (popu != "Desconocido")
                                {
                                    idPopu = basedatos.getGenerosPorPopu(popu);
                                    final = await BuscaCinco(apidos, idPopu);
                                    peliculas = final;
                                }
                            }
                            else
                            {
                                //Peliculas de los dos ultimos años
                                if(random.Next(0, 2) % 2 == 0) {
                                    final = await BuscaCinco(apidos, anios[0]);
                                }
                                else {
                                    final = await BuscaCinco(apidos, anios[1]);
                                }

                                //si el total de peliculas es menos que 5
                                //mostraremos 5 peliculas de un año aleatorio que no haya visto el usuario
                                if(final.Count() < 5)
                                {
                                    final = await BuscaCinco(apidos, anios[0] + 1);
                                }

                            }
                        }
                    }
                }
                peliculas = final;
                //Colocamos las peliculas
                try
                {
                    StackPanel panelRecomendado = new StackPanel();
                    for (int i = 0; i < peliculas.Count;i ++)
                    {
                        UCPelicula ucPelicula2 = new UCPelicula(user.Correo, final[i].getTitulo(), final[i].ObtenerImagen(), final[i].getGenero());
                        ucPelicula2.DataContext = final[i];
                        ucPelicula2.Width = 160;
                        ucPelicula2.Height = 200;
                        panelRecomendado.Children.Add(ucPelicula2);
                        ucPelicula2.MandarMovie += UcPelicula_ManejarMovie;
                        ucPelicula2.MandarPelicula += UcPelicula_MandarPelicula;

                        if(i < peliculas.Count - 1) { }
                            panelRecomendado.Children.Add(new Separator {Height = 15});

                        scrollRecomendado.Content = panelRecomendado;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al mostrar la película: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al mostrar la película: {ex.Message}");
            }
        }

        //metodo que escogerá el primero metodo de lo devuelto en la base de datos
        /// <summary>
        /// Obtiene el primer género de el string de generos pasado por parámetro.
        /// </summary>
        /// <param name="cadena">Cadena de géneros.</param>
        /// <returns>Devuelve el primer género</returns>
        public string GeneroTrim(string cadena)
        {
            string[] generosArray = new string[10];
            generosArray = cadena.Split(',');
            return generosArray.FirstOrDefault()?.Trim();
        }

        //Metodo que utiliza LINQU para adivinar el genero más repetido de la lista
        /// <summary>
        /// Obtiene el genero más repetido de la lista de géneros.
        /// </summary>
        /// <param name="lista">Lista de generos.</param>
        /// <returns>Devuelve el genero más repetido</returns>
        private string GenPopular(List<string> lista)
        {
            return lista.GroupBy(x => x).OrderBy(y => y.Count()).Select(z => z.Key).First();
        }

        //metodo que busca 5 peliculas que no haya visto el usuario actual
        /// <summary>
        /// Busca y devuelve una lista de cinco películas populares del género pasado por parámetro.
        /// </summary>
        /// <param name="popu">Género a buscar.</param>
        /// <returns>Devuelve una lista de cinco peliculas</returns>
        private async Task<List<Movie>> BuscaCinco(TMDB apidos, string popu)
        {
            List<Movie> devolver = new List<Movie>();

            for (int i = 0; i < 5;)
            {
                Movie nuevaPelicula = await apidos.BuscarPorGenero(popu);

                int estado = basedatos != null ? basedatos.InfoUsuarioPelicula(user.Correo, nuevaPelicula.getTitulo()) : 0;

                if (estado != 2 && estado != 3)
                {
                    devolver.Add(nuevaPelicula);
                    i++;
                }
            }

            return devolver;
        }

        /// <summary>
        /// Busca y devuelve una lista de cinco películas buscada por un año pasado por parámetro.
        /// </summary>
        /// <param name="anio">Año que se ha de buscar</param>
        /// <returns></returns>
        private async Task<List<Movie>> BuscaCinco(TMDB apidos, int anio)
        {
            List<Movie> devolver = new List<Movie>();
            int estado;

            for (int i = 0; i < 5; i++)
            {
                Movie nuevaPelicula = await apidos.BuscarPorAnio(anio.ToString());

                if (nuevaPelicula != null) {
                    estado = basedatos != null ? basedatos.InfoUsuarioPelicula(user.Correo, nuevaPelicula.getTitulo()) : 0;

                    if (estado != 2 && estado != 3)
                    {
                        devolver.Add(nuevaPelicula);
                    }
                    else
                    {
                        i--;
                    }
                }

            }

            return devolver;
        }

        //metodo que crea un menu contextual con 4 opciones y añade sus eventos
        /// <summary>
        /// Metodo que crea el menu contextual y le añade cuatro opciones.
        /// Mis peliculas
        /// Ajustaes
        /// Cerrar Sesión
        /// Manual
        /// </summary>
        private void MenuContextual()
        {
            //Creamos el menu
            ContextMenu menuContextual = new ContextMenu();

            //Creamos las secciones del menu
            MenuItem ItmPelis = new MenuItem();
            ItmPelis.Header = "Mis Películas";
            ItmPelis.Click += MisPelisClick;
            //no se puede usar una ruta como con un Image
            //debemos crear un image y dentro un bitmapImage
            //misPeliculasItem.Icon = "/Resources/pelicula.png";
            ItmPelis.Icon = new Image
            {
                Source = new BitmapImage(new Uri("/Resources/pelicula.png", UriKind.Relative)),
                Stretch = System.Windows.Media.Stretch.Uniform
            };

            MenuItem ItmAjustes = new MenuItem();
            ItmAjustes.Header = "Ajustes";
            ItmAjustes.Click += AjustesClick;
            ItmAjustes.Icon = new Image { Source = new BitmapImage(new Uri("/Resources/ajustes.png", UriKind.Relative)) };

            MenuItem ItmCerrarSesion = new MenuItem();
            ItmCerrarSesion.Header = "Cerrar Sesión";
            ItmCerrarSesion.Click += CerrarSesionClick;
            ItmCerrarSesion.Icon = new Image { Source = new BitmapImage(new Uri("/Resources/cerrarsesion.png", UriKind.Relative)) };

            //añadimos una nueva opcion para diferenciar del menu de ajustes
            //opcion abrir manual
            /*
            MenuItem ItmManual = new MenuItem();
            ItmManual.Header = "Manual";
            ItmManual.Click += Manual_Click;
            ItmManual.Icon = new Image { Source = new BitmapImage(new Uri("/Resources/manual.png", UriKind.Relative)) };
            */

            //añadimos las opciones al menu
            menuContextual.Items.Add(ItmPelis);
            menuContextual.Items.Add(ItmAjustes);
            //menuContextual.Items.Add(ItmManual);
            menuContextual.Items.Add(ItmCerrarSesion);

            //Añadimos el menu al grid que reune todos los grid del xaml
            gridTotal.ContextMenu = menuContextual;
        }

        /// <summary>
        /// Metodo que muestra el pdf del manual.
        /// </summary>
        //Evento que abre un manual en pdf
        private void Manual_Click(object sender, RoutedEventArgs e)
        {
            //Process.Start("../../../Resources/FilmotecaCLM.pdf");
            //Process.Start(new ProcessStartInfo("../../../Resources/FilmotecaCLM.pdf") { UseShellExecute = true });
            string rutaPDF = System.IO.Path.Combine("Resources", "FilmotecaCLM.pdf");
            Process.Start(new ProcessStartInfo(rutaPDF) { UseShellExecute = true });
        }


    }
}
