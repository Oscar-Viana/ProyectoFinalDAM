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

namespace APPProyecto.MisControles.Administracion
{
    /// <summary>
    /// User control que se utilizará para registrar un usuario nuevo
    /// (Solo lo podrán usar los administradores)
    /// </summary>
    public partial class UCRegistro : UserControl
    {
        private BD basedatos;
        /// <summary>
        /// Dependiendo del rol del administrador se mostrará o no el checkbox de administrador
        /// este checkbox permitirá al super usuario crear nuevo administradores.
        /// </summary>
        /// <param name="rol"></param>
        public UCRegistro(bool rol)
        {
            InitializeComponent();
            if(rol)
            {
                chRol.Visibility = Visibility.Visible;
            }
            basedatos = new BD();
        }

        /// <summary>
        /// Evento para registrar el usuario donde se comprobará si el checkbox esta seleccionado, si no lo esta creará
        /// un usuario normal, si lo está creará un usuario administrador.
        /// Comprobará si los campos de la contraseña son iguales (seguridad)
        /// Si todo es correcto se registrará un nuevo usuario
        /// </summary>
        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            //si el rol esta activado sera adiministrador, sino, usuario normal
            //ERROR, no detecta el checkbox BUSCAR SOLUCION.
            int admin = chRol.IsChecked == true ? 1:0;
            if(txtContraseña.Password != txtSeguridad.Password)
            {
                MessageBox.Show("Los campos de contraseña no coinciden");
            }
            else
            {
                Usuario registrar = new Usuario(txtCorreo.Text, txtContraseña.Password, txtNombre.Text, txtPregunta.Text, txtRespuesta.Text, admin);
                basedatos.RegistroBD(registrar);
            }

        }
    }
}
