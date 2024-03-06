using APPProyecto.MisControles.Administracion;
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

namespace APPProyecto.PaginaVentana.Administradores
{
    /// <summary>
    /// Página donde se gestionan las peticiones del usuario
    /// </summary>
    public partial class Peticiones : Page
    {
        private List<Peticion> peti = new List<Peticion>();
        private List<UCPeticiones> peticiones = new List<UCPeticiones>();
        private BD basedatos;
        public Peticiones()
        {
            InitializeComponent();
            basedatos = new BD();
            ObtenerPeticiones();
        }

        /// <summary>
        /// Metodo que obtiene todas las peticiones de la base de datos
        /// Crea un user control por cada peticion
        /// Actualiza el grid de peticiones
        /// </summary>
        public void ObtenerPeticiones()
        {
            peti = basedatos.TotalPeticion();
            int cont = 0;
            foreach (Peticion peticion in peti)
            {
                UCPeticiones ucPeticion = new UCPeticiones(peticion, cont);
                ucPeticion.MandarID += UcPeticion_MandarID;
                peticiones.Add(ucPeticion);
                cont++;
            }
            ActualizarPeti();
        }

        /// <summary>
        /// Actualiza el grid de peticiones
        /// 
        /// </summary>
        private void ActualizarPeti()
        {
            stackPadre.Children.Clear();

            foreach (UCPeticiones ucPeticion in peticiones)
            {
                stackPadre.Children.Add(ucPeticion);
                Separator separator = new Separator();
                separator.Margin = new Thickness(0, 10, 0, 10);
                stackPadre.Children.Add(separator);
            }
        }

        /// <summary>
        /// Evento que recoge el ID de la peticion.
        /// Busca la peticion en la lista y la elimina
        /// Actualiza el grid de peticiones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="id"></param>
        private void UcPeticion_MandarID(object sender, int id)
        {
            UCPeticiones Eliminar = peticiones.Find(uc => uc.Id == id);
            string cor = Eliminar.Correo;
            string pet = Eliminar.Peticion;

            if (Eliminar != null)
            {
                peticiones.Remove(Eliminar);
                basedatos.BorrarPeticion(cor, pet);
                ActualizarPeti();
            }

        }
    }
}
