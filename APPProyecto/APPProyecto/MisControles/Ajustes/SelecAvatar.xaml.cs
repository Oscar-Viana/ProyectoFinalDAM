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

namespace APPProyecto.MisControles.Ajustes
{
    /// <summary>
    /// User Control donde se almacenarán todos los usercontrols de avatares
    /// </summary>
    public partial class SelecAvatar : UserControl
    {
        private BD basedatos;
        private List<BitmapImage> avatares = new List<BitmapImage> { };
        private List<int> ides = new List<int> { };
        private Usuario user;
        /// <summary>
        /// Obtenemos todos los avatares de la base de datos y se alacenarán en la lista avaters
        /// Obtenemos todos los IDs de la base de datos y se alacenarán en la lista ides
        /// </summary>
        /// <param name="user">Usuario actual</param>
        public SelecAvatar(Usuario user)
        {
            InitializeComponent();
            basedatos = new BD();
            //obtenemos todos los avatares
            avatares = basedatos.AvatarTotal();
            ides = basedatos.SelecIdesAvatar();
            this.user = user;

            //colocamos los avatares para su seleccion
            ColocarSelecciones();
            
        }

        /// <summary>
        /// Por cada avatar en la lista avatares se creará un user control el cual se le asignará un id de la lista ides
        /// cada avatar se almacenará en un stackpanel creado previamente
        /// cada cinco avatares creados se generará un nuevo stackpanel
        /// al final se añadiran a un stackpanel final todos los stackpanels creados anterirormente
        /// Asi obtendremos una cuadriculo de avatares
        /// </summary>
        private void ColocarSelecciones()
        {
            int cont = 0;

            // Crear un nuevo StackPanel que contendrá todos los StackPanels de avatares
            StackPanel panelJefe = new StackPanel();
            panelJefe.Orientation = Orientation.Horizontal;

            // Contador para seguir la cantidad de avatares colocados
            int contador = 0;
            StackPanel panelHijo = new StackPanel();
            // Recorrer la lista de avatares
            foreach (var avatar in avatares)
            {
                // Crear un nuevo UCAvatares con el ancho y alto de 100 y pasar el avatar como parámetro
                UCAvatares ucAvatar = new UCAvatares(avatar, ides[cont]);
                cont++;
                ucAvatar.Width = 100;
                ucAvatar.Height = 100;
                ucAvatar.Margin = new Thickness(0, 0, 10, 0);
                ucAvatar.PasarRuta += Evento_PasarRuta;


                // Crear un nuevo StackPanel para cada conjunto de cinco avatares
                if (contador == 0)
                    panelHijo = new StackPanel();

                // Añadir el UserControl al StackPanel actual
                panelHijo.Children.Add(ucAvatar);

                // Añadir un separador después de cada UserControl (excepto el último)
                if (avatares.Last() != avatar)
                {
                    Separator separator = new Separator();
                    panelHijo.Children.Add(separator);
                }

                // Incrementar el contador
                contador++;

                // Si ya se han añadido cinco avatares, reiniciar el contador y crear un nuevo StackPanel
                if (contador == 5 || avatares.Last() == avatar)
                {
                    contador = 0;
                    panelJefe.Children.Add(panelHijo);
                    panelJefe.Children.Add(new Separator());
                }
            }

            // Añadir el StackPanel principal al Grid llamado GridAvatar
            GridAvatar.Children.Add(panelJefe);
        }

        /// <summary>
        /// Evento que se envía el ID de avatar de un User ControlSeleccionado
        /// </summary>
        private void Evento_PasarRuta(object sender, int id)
        {
            basedatos.cambiarAvatar(id, user);
        }
    }
}
