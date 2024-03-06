using APPProyecto.MisControles.Administracion;
using APPProyecto.MVC;
using APPProyecto.PaginaVentana.Entrar;
using APPProyecto.PaginaVentana.Principal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Página que se utilizará tanto por el administrador como el Super-Administrador
    /// Cambia ligeramente dependiendo del rol
    /// </summary>
    public partial class Administracion : Page
    {
        private Usuario user;
        private BD basedatos;
        private bool super;
        private Thread HiloBorrar;
        private Thread HiloComen;

        /// <summary>
        /// Constructor de la clase Administracion.
        /// Determina si el admin es normal o super
        /// Crea las columnas del datagrid de usuarios y comentarios.
        /// Crea los hilos necesarios para borrar usuario y comentarios seleccionados
        /// </summary>
        /// <param name="us">Usuario actual.</param>
        public Administracion(Usuario us)
        {
            InitializeComponent();
            user = us;
            basedatos = new BD();
            super = EsSuper();

            //Creamos las Columnas de los DataGrid
            CrearDataGrid();

            //Añadimos los usuarios
            DataUsuarios();

            //Hilos
            HiloBorrar = new Thread(BorrarUser);
            HiloComen = new Thread(BorrarComent);
        }

        //metodo que verifica si el administrador es normal o superadimistrador
        //si lo es muestra los botones relacionados con los administradores
        /// <summary>
        /// Si el usuario es Super-Administrador ocultará los botones para poder añadir o eliminar administrador
        /// </summary>
        /// <returns></returns>
        private bool EsSuper() {
            if (Convert.ToInt32(user.Rol) == 2)
            {
                btnAddAdmin.Visibility = Visibility.Visible;
                btnEraseAdmin.Visibility = Visibility.Visible;
                return true;
            }
            return false;
        }

        //metodo que crea las tablas en los dataGrid
        /// <summary>
        /// Metodo que crea las columnas de los data grid
        /// Datagrid usuarios: añade las columnas Correo, Nombre y Rol
        /// Datagrid comentarios: añade las columnas Nombre y Comentario
        /// </summary>
        public void CrearDataGrid()
        {
            //Usuario
            dataUsers.AutoGenerateColumns = false;

            DataGridTextColumn correoColumn = new DataGridTextColumn();
            correoColumn.Header = "Correo";
            correoColumn.Binding = new Binding("Correo");
            dataUsers.Columns.Add(correoColumn);

            DataGridTextColumn nombreColumn = new DataGridTextColumn();
            nombreColumn.Header = "Nombre";
            nombreColumn.Binding = new Binding("Nombre");
            dataUsers.Columns.Add(nombreColumn);

            DataGridTextColumn rolColumn = new DataGridTextColumn();
            rolColumn.Header = "Rol";
            rolColumn.Binding = new Binding("Rol");
            dataUsers.Columns.Add(rolColumn);

            //Comentario
            dataComentarios.AutoGenerateColumns = false;

            DataGridTextColumn Nombre = new DataGridTextColumn();
            Nombre.Header = "Nombre";
            Nombre.Binding = new Binding("Nombre");
            dataComentarios.Columns.Add(Nombre);

            DataGridTextColumn comentario = new DataGridTextColumn();
            comentario.Header = "Comentario";
            comentario.Binding = new Binding("ComentarioTexto");
            dataComentarios.Columns.Add(comentario);
        }

        //Añadimos los usuarios al primer dataGrid
        /// <summary>
        /// Añade al datagrid de usuarios todos los usuarios y ademas crea uno nuevo con el nombre Anónimo,
        /// este se utilizará para observar los comentarios de las personas anónimas.
        /// </summary>
        private void DataUsuarios()
        {
            dataUsers.ItemsSource = null;
            List<Usuario> listauser = basedatos.getTotalUsers();
            //Añadimos un usuario anónimo para que puedan seleccionarse los comentarios anonimos
            listauser.Add(new Usuario("---", "---", "Anónimo", "---", "---", 0));
            dataUsers.ItemsSource = listauser;
        }

        /// <summary>
        /// Evento que llama la metodo ActualizarComent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizarComent();
        }

        /// <summary>
        /// Cuando se pulsa en un usuario se mostrará todos sus comentarios de todas las peliculas en el datagrid de comentarios
        /// </summary>
        private void ActualizarComent()
        {
            try
            {
                if (dataUsers.SelectedItem is Usuario selectedUsuario)
                {
                    string correo = selectedUsuario.Correo;

                    if (!string.IsNullOrEmpty(correo))
                    {
                        List<Comentario> comentarios = basedatos.getComentariosUser(correo);

                        dataComentarios.ItemsSource = comentarios;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar comentarios: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento que llama al metodo VentanaADD
        /// </summary>
        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            VentanaADD();
        }

        /// <summary>
        /// Metodo que genera una nueva ventana para registrar un nuevo usuario
        /// </summary>
        private void VentanaADD()
        {

            Window registro = new Window();
            MisControles.Administracion.UCRegistro usercontrol = new MisControles.Administracion.UCRegistro(super);
            registro.VerticalContentAlignment = VerticalAlignment.Top;
            registro.HorizontalContentAlignment = HorizontalAlignment.Left;
            registro.Content = usercontrol;
            registro.Title = "REGISTRAR NUEVO USUARIO";
            registro.Width = 350;
            registro.Height = 500;
            registro.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            registro.ShowDialog();

        }

        /// <summary>
        /// Evento que mostrará una nueva ventana con el contenido de la página principal de la aplicacion, ya que un administrador
        /// también podrá buscar peliculas como un usuario normal.
        /// </summary>
        private void btnApp_Click(object sender, RoutedEventArgs e)
        {
            /*
            Window ventanabuscar = new Window();
            Entrar.Buscador buscapagina = new Entrar.Buscador(user);
            ventanabuscar.VerticalContentAlignment = VerticalAlignment.Top;
            ventanabuscar.HorizontalContentAlignment = HorizontalAlignment.Left;
            ventanabuscar.Content = buscapagina;
            ventanabuscar.Width = 800;
            ventanabuscar.Height = 450;
            ventanabuscar.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            

            ventanabuscar.ShowDialog();
            */
            Ventana nueva = new Ventana(user);
            nueva.Show();
        }

        //metodo que elimina el comentario seleccionado en el grid de comentarios
        /// <summary>
        /// Evento que lanza el hilo para eliminar un comentario
        /// </summary>
        private void btnEraseComent_Click(object sender, RoutedEventArgs e)
        {
            HiloComen.Start();
        }

        //Borrar usuario
        /// <summary>
        /// Evento que lanza un hilo para eliminar un usuario.
        /// </summary>
        private void btnEraseUser_Click(object sender, RoutedEventArgs e)
        {
            HiloBorrar.Start();
        }

        /// <summary>
        /// Hilo que elimina un usuario
        /// Comprueba que se seleccione un registro del data grid de usuarios
        /// Comprueba que si el usuario seleccionado es el super admin mostrará un mensaje de error (ya que no se podrá eliminar)
        /// Comprueba que si el user seleccionado es admin y el user actual es admin mostrara error (ya que un admin solo 
        /// lo podrá eliminar el Super-Admin)
        /// Si todo es correcto eliminará el usuario (no lo elimina, lo mueve a la tabla betados)
        /// </summary>
        private void BorrarUser()
        {
            if (dataUsers.SelectedItem == null)
            {
                MessageBox.Show("Primero selecciona el usuario a borrar");
                return;
            }
            else
            {
                try
                {
                    Usuario userselec = (Usuario)dataUsers.SelectedItem;
                    int rol = Convert.ToInt32(userselec.Rol);

                    if (rol > 1)
                    {
                        MessageBox.Show("No puedes borrar el Super-Administrador");
                    }
                    else if (rol == 1 && Convert.ToInt32(userselec.Rol) == 1)
                    {
                        MessageBox.Show("No puedes borrar un Administrador");
                    }
                    else
                    {
                        string correo = userselec.Correo;
                        basedatos.EraseUser(correo);
                        DataUsuarios();
                    }
                }
                catch (InvalidCastException ex)
                {
                    MessageBox.Show("Error al intentar borrar el usuario: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Hilo que elimina un comentario
        /// Comprueba que se haya seleccionado primero un usuario (para que se muestren los comentarios)
        /// Comprueba que se haya seleccionado un comentario
        /// Si todo es correcto elimina el comentario
        /// </summary>
        private void BorrarComent(){

            if (dataComentarios.Items.Count == 0)
            {
                MessageBox.Show("Primero selecciona a un usuario");
                return;
            }
            else
            {
                if (dataComentarios.SelectedItem == null)
                {
                    MessageBox.Show("Primero selecciona el comentario a borrar");
                    return;
                }
                else
                {
                    try
                    {
                        Comentario comentselec = (Comentario)dataComentarios.SelectedItem;
                        string nombre = comentselec.Nombre;
                        string comentario = comentselec.ComentarioTexto;

                        basedatos.EraseComentario(nombre, comentario);

                        ActualizarComent();
                    }
                    catch (InvalidCastException ex)
                    {
                        MessageBox.Show("Error al intentar borrar el comentario: " + ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Evento que inicia al HiloBorrar 
        /// </summary>
        private void btnEraseAdmin_Click(object sender, RoutedEventArgs e)
        {
            HiloBorrar.Start();
        }

        /// <summary>
        /// Evento que llama al metodo VentanaADD
        /// </summary>
        private void btnAddAdmin_Click(object sender, RoutedEventArgs e)
        {
            VentanaADD();
        }

        /// <summary>
        /// Evento que muestra una nueva ventana donde se mostrarán las peticines de los usuarios para resolverlas.
        /// </summary>
        private void btnPeticiones_Click(object sender, RoutedEventArgs e)
        {
            Window peticion = new Window();
            Administradores.Peticiones pagina = new Administradores.Peticiones();
            peticion.VerticalContentAlignment = VerticalAlignment.Top;
            peticion.HorizontalContentAlignment = HorizontalAlignment.Left;
            peticion.Content = pagina;
            peticion.Title = "PETICIONES DE USUARIO";
            peticion.Width = 600;
            peticion.Height = 500;
            peticion.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            peticion.ShowDialog();
        }

        /// <summary>
        /// Evento que cierra sesion
        /// </summary>
        private void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            Principal.Bloqueo bloc = new Principal.Bloqueo();
            this.NavigationService.Navigate(bloc);
        }
    }

}
