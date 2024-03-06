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

namespace APPProyecto.MisControles
{
    /// <summary>
    /// UserControl que se utilizará para mostrar todos los avatares seleccionar uno nuevo
    /// </summary>
    public partial class UCAvatares : UserControl
    {
        private BitmapImage ruta;
        private int iden;
        private BD basedatos;
        public event EventHandler<int> PasarRuta;
        public UCAvatares(BitmapImage avatar, int id)
        {
            InitializeComponent();
            ruta = avatar;
            iden = id;
            imgAvatar.Source = ruta;
            basedatos = new BD();
        }

        /// <summary>
        /// metodo que envía el ID del avatar
        /// </summary>
        private void imgAvatar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PasarRuta?.Invoke(this, iden);
        }
    }
}
