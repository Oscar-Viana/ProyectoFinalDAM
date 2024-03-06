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

namespace APPProyecto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class NavigationMainWindow : Window
    {
        public NavigationMainWindow()
        {
            //InitializeComponent();
            PreviewKeyDown += Quitaratajo;
        }

        private void Quitaratajo(object sender, KeyEventArgs e)
        {
            // Verifica si se presionó la tecla Alt junto con la Flecha Izquierda
            if (Keyboard.Modifiers == ModifierKeys.Alt && e.Key == Key.Left)
            {
                // Cancela el evento para evitar que la acción predeterminada ocurra
                e.Handled = false;
            }
        }
    }
}
