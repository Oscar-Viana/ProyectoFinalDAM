using APPProyecto.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace APPProyecto.PaginaVentana.Principal
{
    /// <summary>
    /// Pagina utilizada para el cambio de contraseña del usuario
    /// </summary>
    public partial class Olvidada : Page
    {
        private BD basedatos;
        public Olvidada(BD baseda)
        {
            InitializeComponent();
            basedatos = baseda;
        }

        /// <summary>
        /// Comprueba que el texto pasado por parámetro cumple los requisitos de la contraseña
        /// </summary>
        /// <param name="texto">Contraseña a verificar</param>
        /// <returns>
        ///   <c>true</c> devuelve true si la contraseña tiene un minimo de 6 carácteres, almenos una minuscula y almenos una mayuscula,
        ///   si no lo cumple devolverá false.
        /// </returns>
        private bool FormatoContra(string texto)
        {
            Regex regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,}$");
            return regex.IsMatch(texto);
        }

        /// <summary>
        /// Por cada vez que se cambia el campo del Correo electronico realizará una consulta a la base de datos, si el correo es correcto
        /// mostrará los campos de la pregunta de seguridad, en caso incorrecto los ocultará no solo la pregunta de seguridad sino también los
        /// campos de contraseña y el boton para cambiarla.
        /// </summary>
        /// <param name="sender">.</param>
        /// <param name="e"></param>
        private void txtCorreo_TextChanged(object sender, TextChangedEventArgs e)
        {
            string pregunta = basedatos.getPregunta(txtCorreo.Text);
            if(pregunta != null)
            {
                lblPregu.Visibility = Visibility.Visible;
                lblPregunta.Visibility = Visibility.Visible;
                lblPregunta.Content = pregunta;
                lblRespuesta.Visibility = Visibility.Visible;
                txtRespuesta.Visibility = Visibility.Visible;
            }
            else
            {
                lblPregu.Visibility = Visibility.Hidden;
                lblPregunta.Visibility = Visibility.Hidden;
                lblRespuesta.Visibility = Visibility.Hidden;
                txtRespuesta.Visibility = Visibility.Hidden;
                lblCont.Visibility = Visibility.Hidden;
                txtContra.Visibility = Visibility.Hidden;
                txtContraSegura.Visibility = Visibility.Hidden;
                btnCambiar.Visibility = Visibility.Hidden;
                lblRepiite.Visibility = Visibility.Hidden;

            }
        }

        /// <summary>
        /// Comprueba si los campos de la contraseña coinciden, en caso correcto mostrará el botón para el cambio de contraseña.
        /// </summary>
        private void txtContraSegura_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(FormatoContra(txtContraSegura.Password) && txtContraSegura.Password == txtContra.Password)
            {
                btnCambiar.Visibility = Visibility.Visible;
            }
            else
                btnCambiar.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Llama al metodo CambiarContra de la base de datos para realizar el cambio de contraseña.
        /// </summary>
        private void btnCambiar_Click(object sender, RoutedEventArgs e)
        {
            basedatos.CambiarContra(txtCorreo.Text, txtContra.Password);
        }

        /// <summary>
        /// Cada vez que cambia el contenido del campo de la respuesta a la pregunta de seguridad se comprobará si la respuesta es correcta
        /// en ese caso mostraá los cmpos de contraseña para realizar el cambio y en caso incorrecto los ocultará.
        /// </summary>
        private void txtRespuesta_TextChanged(object sender, RoutedEventArgs e)
        {
            string resp = basedatos.getRespuesta(txtCorreo.Text);
            if (resp != null && txtRespuesta.Password == resp)
            {
                lblCont.Visibility = Visibility.Visible;
                txtContra.Visibility = Visibility.Visible;
                txtContraSegura.Visibility = Visibility.Visible;
                lblRepiite.Visibility = Visibility.Visible;
            }
            else
            {
                lblCont.Visibility = Visibility.Hidden;
                txtContra.Visibility = Visibility.Hidden;
                txtContraSegura.Visibility = Visibility.Hidden;
                btnCambiar.Visibility = Visibility.Hidden;
                lblRepiite.Visibility = Visibility.Hidden;
            }
        }
    }
}
