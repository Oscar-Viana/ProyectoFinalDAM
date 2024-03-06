using APPProyecto.MVC;
using APPProyecto.PaginaVentana.Entrar;
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
using System.Windows.Shapes;

namespace APPProyecto.PaginaVentana.Administradores
{
    /// <summary>
    /// Ventana que almacenará la página principal de la aplicación
    /// para que los administradores puedan buscar peliculas
    /// Se debe utilizar una ventana WPF por un tema de navegación.
    /// </summary>
    public partial class Ventana : Window
    {
        public Ventana(Usuario us)
        {
            InitializeComponent();
            Frame fran = new Frame();
            fran.Content = new Buscador(us);
            Content = fran;
        }
    }
}
