
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


namespace APPProyecto.PaginaVentana.Ajustes
{
    /// <summary>
    /// Página de ajustes
    /// </summary>
    public partial class Ajustes : Page
    {
        private Usuario usuario;
        private BD basedatos;
        public Ajustes(Usuario user)
        {
            InitializeComponent();

            usuario = user;
            basedatos = new BD();

            //comprobamos que hoja de estilos tiene el usuario activada
            //comprobarEstilos(Convert.ToInt32(usuario.Estilo));
            gridEstilos.Visibility = Visibility.Hidden;

            //Colocar avatar actual del usuario
            ColocarAvatar(user);
        }

        /// <summary>
        /// Metodo que carga el avatar actual del usuario actual.
        /// </summary>
        /// <param name="user"></param>
        public void ColocarAvatar(Usuario user)
        {
            imgAvatar.Source = basedatos.CargarAvatar(Convert.ToInt32(user.Avatar));
        }

        //Metodo que selecciona la hoja de estilos del usuario actual
        private void comprobarEstilos(int num){
            switch (num)
            {
                case 0:
                    rbClaro.IsChecked = true;
                    break;

                case 1:
                    rbOscuro.IsChecked = true;
                    break;

                case 2:
                    rbAzul.IsChecked = true;
                    break;

                case 3:
                    rbVerde.IsChecked = true;
                    break;

                default:
                    rbClaro.IsChecked = true;
                    break;
            }
        }

        /// <summary>
        /// Metodo que cambia el nombre del usuario.
        /// Comprueba que el texto no este en blanco y que no sea el nombre actual.
        /// </summary>
        //Metodo que cambia el nombre del usuario.
        private void btnNomCambiar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
                MessageBox.Show("La casilla de texto no debe estar vacía");
            else if (txtNombre.Text == usuario.Nombre)
                MessageBox.Show("El texto introducido es igual al nombre actual de tu cuenta");
            else
            {
                basedatos.CambiarNombre(usuario.Correo, txtNombre.Text);
            }
        }

        private void btnCambiarEstilo_Click(object sender, RoutedEventArgs e)
        {
            string estilo = "0";
            if (rbClaro.IsChecked == true)
            {
                estilo = "0";
            }
            else if (rbOscuro.IsChecked == true)
            {
                estilo = "1";
            }
            else if (rbAzul.IsChecked == true)
            {
                estilo = "2";
            }
            else if (rbVerde.IsChecked == true)
            {
                estilo = "3";
            }

            basedatos.CambiarEstilo(usuario.Correo, estilo);
        }

        /// <summary>
        /// Metodo que abre una nueva ventana con todos los avatares de la vase de datos para que el usuario pueda cambiarlo a placer.
        /// </summary>
        private void btnCambiar_Click(object sender, RoutedEventArgs e)
        {
            Window ventanaAvatares = new Window();
            MisControles.Ajustes.SelecAvatar seleccion = new MisControles.Ajustes.SelecAvatar(usuario);
            ventanaAvatares.VerticalContentAlignment = VerticalAlignment.Top;
            ventanaAvatares.HorizontalContentAlignment = HorizontalAlignment.Left;
            ventanaAvatares.Content = seleccion;
            ventanaAvatares.Title = "ELIGE TU NUEVO AVATAR";
            ventanaAvatares.Width = 400;
            ventanaAvatares.Height = 300;
            ventanaAvatares.WindowStartupLocation = WindowStartupLocation.Manual;
            ventanaAvatares.Left = 150;
            ventanaAvatares.Top = 150;

            ventanaAvatares.ShowDialog();
        }

        /// <summary>
        /// Metodo que te devuelve a la página anterior
        /// </summary>
        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            usuario = basedatos.getUser(usuario.Correo);
            Entrar.Buscador regreso = new Entrar.Buscador(usuario);
            this.NavigationService.Navigate(regreso);
        }

        /// <summary>
        /// Evento que nos permite añadir un avatar a la base de datos
        /// </summary>
        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            basedatos.InsertarAvatar();
        }

        /// <summary>
        /// Evento que nos permite realizar una petición a los administradores
        /// </summary>m>
        /// <param name="e"></param>
        private void btnPeticion_Click(object sender, RoutedEventArgs e)
        {
            string asunto = ((ComboBoxItem)cbPeticion.SelectedItem).Content.ToString();
            basedatos.InsertarPeticion(usuario.Correo, usuario.Nombre, asunto, txtPeticion.Text);
        }
    }
}
