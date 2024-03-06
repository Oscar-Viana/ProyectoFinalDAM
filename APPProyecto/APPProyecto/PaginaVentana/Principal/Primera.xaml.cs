using APPProyecto.MVC;
using APPProyecto.PaginaVentana.Principal;
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

namespace APPProyecto.PaginaVentana
{
    /// <summary>
    /// Página utilizada para el registro, el login y el cambio de contraseña de usuarios
    /// </summary>
    public partial class Primera : Page
    {
        private BD basedatos;
        /// <summary>
        /// constructor de la página donde se instancia la base de datos
        /// </summary>
        public Primera()
        {
            InitializeComponent();
            basedatos = new BD();
        }

        //metodo que genera una contraseña
        //minimo 9 caracteres
        //minimo una mayuscula
        //minimo un numero
        //he optimizado quitando una string ocn las mayusculas y usando ToUpper
        /// <summary>
        /// Genera una contraseña con requisitos específicos y la asigna a los campos de contraseña en la interfaz.
        /// </summary>
        private void btnGenerar_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            const string minus = "abcdefghijklmnopqrstuvwxyz";
            const string num = "0123456789";

            //usamos toupper para asignar mayusculas
            string newpass = Char.ToUpper(minus[random.Next(minus.Length)]).ToString() + num[random.Next(num.Length)];

            while (newpass.Length < 9)
            {
                string caracteresPermitidos = minus + num;
                newpass += caracteresPermitidos[random.Next(caracteresPermitidos.Length)];
            }

            txtRegContra1.Password = newpass;
            txtRegContra2.Password = newpass;
        }

        /// <summary>
        /// Maneja el evento de clic en el botón de registro, valida los datos de registro y realiza el registro del usuario.
        /// Comprueba que los campos no estén vacios
        /// Comprueba que el correo no este en la lista de betados, en caso que este betado mostrará un aviso
        /// Comprueba que la contraseña tenga el formato correcto, en caso que no lo cumpla mostrará un aviso
        /// Comprueba que la contraseña coincida en ambos campos por seguridad, en caso de que no coincida mostrará un error
        /// Si todo lo anterior es correcto registrará un nuevo usuario
        /// </summary>
        private void btnRegistro_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRegCorreo.Text) || string.IsNullOrWhiteSpace(txtRegAlias.Text) || string.IsNullOrWhiteSpace(txtRegContra1.Password) || string.IsNullOrWhiteSpace(txtRegContra2.Password) || string.IsNullOrWhiteSpace(txtPregunta.Text) || string.IsNullOrWhiteSpace(txtRespuesta.Password))
            {
                //mostramos error
                MessageBox.Show("Debes rellenar todos los campos de registro", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //comprobamos si el correo esta betado
                if(basedatos.Betado(txtRegCorreo.Text) == 1)
                    MessageBox.Show("ERROR: Este usuario esta betado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    //Comprobar que solo tenga numeros y ketras
                    if (!FormatoContra(txtRegContra1.Password) || !FormatoContra(txtRegContra2.Password))
                    {
                        // Mostrar mensaje si las contraseñas no son alfanuméricas
                        MessageBox.Show("ERROR: La contraseña solo debe tner numeros y letras", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (txtRegContra1.Password != txtRegContra2.Password)
                    {
                        // Mostrar mensaje si las contraseñas no coinciden
                        MessageBox.Show("ERROR: Las contraseñas introucidas no coinciden", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    //Registro
                    else
                    {
                        Usuario usuario = new Usuario(txtRegCorreo.Text, txtRegContra1.Password, txtRegAlias.Text, txtPregunta.Text, txtRespuesta.Password, 0);
                        basedatos.RegistroBD(usuario);
                    }
                }
            }
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
            //Formato de contraseñas, solo letras mayusculas y minusculas y numeros
            Regex regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,}$");
            return regex.IsMatch(texto);
        }

        /// <summary>
        /// Evento del boton de inicio de sesión
        /// Comprueba que los campos no estén vacios, si lo están mostrará un aviso.
        /// Compruba que el usuario que se quiere registrar no esté en la lista de betados.
        /// Si el usuari es correcto filtrará por tipo de rol, usuario, administrador o superadministrador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //comprobamos que las casillas no esten vacias
            if (string.IsNullOrWhiteSpace(txtCorreo.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Error: Debes rellenar las casillas de login");
            }
            else
            {
                //comprobamos si el usuario esta betado
                if(basedatos.Betado(txtCorreo.Text) == 1)
                    MessageBox.Show("ERROR: Este usuario esta betado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    //comprobamos si el usuario existe en la tabla usuarios
                    if (basedatos.Login(txtCorreo.Text, txtPassword.Password) == 1)
                    {
                        //obtenemos el rol del usuario
                        int roluser = basedatos.tipoRol(txtCorreo.Text);
                        //abrimos una pagina nueva dependiendo si es usuario, administrador o superAdministrador
                        if (roluser == 0)
                        {
                            //Usuario
                            //Obtenemos el usuario completo de la base de datos
                            Entrar.Buscador buscar = new Entrar.Buscador(basedatos.getUser(txtCorreo.Text));
                            this.NavigationService.Navigate(buscar);
                        }
                        else if (roluser > 0)
                        {
                            //Administrador
                            Administradores.Administracion pagadmin = new Administradores.Administracion(basedatos.getUser(txtCorreo.Text));
                            this.NavigationService.Navigate(pagadmin);
                        }
                    }
                    else
                        MessageBox.Show("ERROR: Usuario no registrado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Boton que maneja el olvido de contraseña.
        /// Mostraá una nueva pagina donde se podrá cambiar la contraseña.
        /// </summary>
        private void btnOlvido_Click(object sender, RoutedEventArgs e)
        {
            Olvidada olvido = new Olvidada(basedatos);
            this.NavigationService.Navigate(olvido);
        }

        
    }
}
