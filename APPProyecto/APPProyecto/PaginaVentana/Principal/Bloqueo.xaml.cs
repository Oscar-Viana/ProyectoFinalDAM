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

namespace APPProyecto.PaginaVentana.Principal
{
    /// <summary>
    /// Página utilizada para bloquear el cirre de sesion, es decir,
    /// Cuando se cierra sesion en la aplicación se carga esta página la cual unicamente cargará la pagina de login,
    /// esto es para evitar que cuando el usuario utilce el atajo de escritorio alt + flecha izq pueda volver a la cuenta anterior.
    /// </summary>
    public partial class Bloqueo : Page
    {
        /// <summary>
        /// El contructor lo unico que hace es invocar al metodo Bloqueador una vez que esta página se ha cargado correctamente.
        /// Debemos esperar a que cargue la página porque puede saltar error.
        /// </summary>
        public Bloqueo()
        {
            InitializeComponent();
            Loaded += Bloqueador;
        }

        /// <summary>
        /// metodo que carga la página de login
        /// </summary>
        private void Bloqueador(object sender, RoutedEventArgs e)
        {
            PaginaVentana.Primera next = new Primera();
            this.NavigationService.Navigate(next);
        }
    }
}
