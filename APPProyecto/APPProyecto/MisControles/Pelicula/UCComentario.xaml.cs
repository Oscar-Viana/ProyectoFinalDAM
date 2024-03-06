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
using APPProyecto.MVC;

namespace APPProyecto.MisControles.Pelicula
{
    /// <summary>
    /// User Control que se utilizará para almacenar un unico comentario
    /// </summary>
    public partial class UCComentario : UserControl
    {
        public UCComentario(Comentario com)
        {
            InitializeComponent();
            lblUser.Content = com.Nombre;
            lblComent.Text = com.ComentarioTexto;
        }
    }
}
