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
    /// UserControl utilizado para almacenar un única petición
    /// </summary>
    public partial class UCPeticiones : UserControl
    {
        public event EventHandler<int> MandarID;
        private string correo;
        private string peticion;
        private int id;
        public UCPeticiones(Peticion peti, int iden)
        {
            InitializeComponent();

            lblCorreo.Content = peti.Correo;
            correo = peti.Correo;
            lblNombre.Content = peti.Nick;
            lblAsunto.Content = peti.Asunto;
            txtPeticion.Text = peti.Mensaje;
            peticion = peti.Mensaje;
            this.id = iden;
        }

        public int Id
        {
            get { return id; }
        }
        public string Correo
        {
            get { return correo; }
        }
        public string  Peticion
        {
            get { return peticion; }
        }

        /// <summary>
        /// Evento que al pulsar un boton se eliminará de la base de datos la peticion, gracias a que envñia el ID de la petición
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MandarID?.Invoke(this, id);
        }
    }
}
