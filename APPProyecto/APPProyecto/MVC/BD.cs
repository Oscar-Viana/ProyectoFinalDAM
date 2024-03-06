using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Media.Imaging;
using System.Security.RightsManagement;
using System.Windows.Media.Media3D;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.IO;
using APPProyecto.PaginaVentana;
using System.Windows.Media;
using System.Collections;
using APPProyecto.MisControles.Administracion;
using APPProyecto.PaginaVentana.Administradores;

namespace APPProyecto.MVC
{
    /// <summary>
    /// Clase que se utilizará para manejar la base de datos
    /// </summary>
    public class BD
    {
        private SQLiteConnection conexion = null;
        private SQLiteCommand comando;
        private SQLiteDataReader dr;

        //datos del usuario
        private string Correo;
        private string Password;
        private string Rol;
        private string Nombre;
        private string Avatar;
        private string Estilo;
        private string Fecha;

        //Constructor con la conexion
        /// <summary>
        /// Constructor de la clase el cual creará la conexión con la base de datos
        /// </summary>
        public BD() {
            ConexionBD();
        }

        //Metodo para la conexion a la Base de datos
        /// <summary>
        /// Metodo que crea la conexion con la base de datos.
        /// </summary>
        public void ConexionBD()
        {
            try
            {
                //MessageBox.Show("Antes de la base de datos");
                //conexion = new SQLiteConnection("Data Source = C:/Users/Usuario/Desktop/proyecto/base de datos/FilmotecaCLMSQLite.db; Version = 3; New = False; Compress = True");
                string rutaBaseDatos = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "FilmotecaCLMSQLite.db");
                conexion = new SQLiteConnection($"Data Source={rutaBaseDatos}; Version=3; New=False; Compress=False");
                //MessageBox.Show("Despues de la base de datos");

                //conexion.Open();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("ERROR: No se pudo conectar con la base de datos\n" + ex.Message);
            }
            catch(Exception ex) {
                MessageBox.Show("Error accesibilidad: \n" + ex.Message);
            }
        }

        //Metodo para hacer Login
        /// <summary>
        /// Realiza el login de un usuario
        /// Se controla SQLInjection.
        /// 
        /// </summary>
        /// <param name="correo">Correo electrónico del usuario.</param>
        /// <param name="contraseña">Contraseña del usuario.</param>
        /// <returns>Devuelve 1 si el inicio de sesión es correcto, 0 en caso incorrecto.</returns>
        public int Login(string correo, string contraseña) {
            //Utilizamos la segunda forma para evitar ataque SQLINJECTION
            //comando = new SQLiteCommand("SELECT `Nombre` FROM `Usuario` WHERE `Correo` = '" + correo + "' AND `Password` = '" + contraseña + "'", conexion);
            using (comando = new SQLiteCommand("SELECT Nombre FROM Usuario WHERE Correo = @Correo AND Password = @Contra", conexion))
            {
                comando.Parameters.AddWithValue("@Correo", correo);
                comando.Parameters.AddWithValue("@Contra", contraseña);

                conexion.Open();
                try
                {
                    dr = comando.ExecuteReader();
                    if (dr.Read())
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (System.NullReferenceException ex)
                {
                    MessageBox.Show("Usuario no encontrado\n" + ex.Message);
                }
                finally
                {
                    dr.Close();
                    conexion.Close();
                }
            }

            
            return 0;
        }

        //Metodo que comprueba si el usuario esta betado, es decir, se encuentra en la tabla Borrados
        //Si devuelve 1 esta betado
        public int Betado(string correo)
        {
            //comando = new SQLiteCommand("SELECT `Correo` FROM `Borrados` WHERE `Correo` = '" + correo + "'", conexion);
            using (comando = new SQLiteCommand("SELECT Nombre FROM Borrados WHERE Correo = @Correo", conexion))
            {
                comando.Parameters.AddWithValue("@Correo", correo);
                conexion.Open();
                try
                {
                    dr = comando.ExecuteReader();
                    if (dr.Read())
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (System.NullReferenceException ex)
                {
                    MessageBox.Show("Usuario no encontrado\n" + ex.Message);
                }
                finally
                {
                    dr.Close();
                    conexion.Close();
                }
            }
            return 0;
        }

        //Metodo para registrar usuarios en la Base de Datos
        /// <summary>
        /// Metodo que registra un usuario
        /// Comprueba si el nombre de usuario ya existe, si existe no lo registra
        /// </summary>
        /// <param name="user">Objeto Usuario a registrar</param>
        public void RegistroBD(Usuario user) {
            try
            {
                conexion.Open();
                int count;

                //Buscamos si algun usuario utiliza ese nombre
                using (comando = new SQLiteCommand("SELECT COUNT(*) FROM Usuario WHERE Nombre = @NuevoNombre", conexion))
                {
                    comando.Parameters.AddWithValue("@NuevoNombre", user.Nombre);
                    count = Convert.ToInt32(comando.ExecuteScalar());
                }

                if (count > 0)
                    MessageBox.Show("El nombre de usuario ya esta en uso, porfavor, prueba con un nuevo nombre");
                else
                {
                    //comando = new SQLiteCommand("INSERT INTO Usuario (Correo, Password, Rol, Nombre, Avatar, Estilo, FechaRegis) VALUES ('" + user.Correo + "', '" + user.Password + "', '" + user.Rol + "', '" + user.Nombre + "', '" + user.Avatar + "', '" + user.Estilo + "', '" + user.FechaRegistro + "');", conexion);
                    using (comando = new SQLiteCommand("INSERT INTO Usuario (Correo, Password, Rol, Nombre, Avatar, Estilo, FechaRegis, Pregunta, Respuesta) VALUES (@Correo, @Contra, @Rol, @Nombre, @Avatar, @Estilo, @Fecha, @Pregunta, @Respuesta);", conexion))
                    {
                        comando.Parameters.AddWithValue("@Correo", user.Correo);
                        comando.Parameters.AddWithValue("@Contra", user.Password);
                        comando.Parameters.AddWithValue("@Rol", user.Rol);
                        comando.Parameters.AddWithValue("@Nombre", user.Nombre);
                        comando.Parameters.AddWithValue("@Avatar", user.Avatar);
                        comando.Parameters.AddWithValue("@Estilo", user.Estilo);
                        comando.Parameters.AddWithValue("@Fecha", user.FechaRegistro);
                        comando.Parameters.AddWithValue("@Pregunta", user.Pregunta);
                        comando.Parameters.AddWithValue("@Respuesta", user.Respuesta);


                        comando.ExecuteNonQuery();
                        MessageBox.Show("El usuario " + user.Nombre + " se ha registrado correctamente");
                    }
                }
                

            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: No se ha podido insertar el usuario\n" + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        /// <summary>
        /// Metodo que cambia la contraseña del usuario
        /// </summary>
        /// <param name="correo">Correo del usuario al que cambiarle la contraseña</param>
        /// <param name="contra">Nueva contraseña</param>
        public void CambiarContra(string correo, string contra) {
            try
            {
                conexion.Open();
                //comando = new SQLiteCommand("UPDATE Usuario SET Password = '" + contra + "' WHERE Correo = '" + correo + "';", conexion);
                using (comando = new SQLiteCommand("UPDATE Usuario SET Password = @Contra WHERE Correo = @Correo;", conexion))
                {
                    comando.Parameters.AddWithValue("@Contra", contra);
                    comando.Parameters.AddWithValue("@Correo", correo);


                    comando.ExecuteNonQuery();
                    MessageBox.Show("Se ha cambiado la contraseña correctamente");
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: No se ha podido cambiar la contraseña\n" + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        //Metodo que comprueba que tipo de ROL tiene el Usuario en conectarse
        //0 - Usuario normal
        //1 - Usuario Administrador
        //2 - Usuario Super-Administrador
        /// <summary>
        /// Metodo que devuelve el rol del usuario
        /// </summary>
        /// <param name="correo">Correo del usuario</param>
        /// <returns>Devuleve 0 si es un usuario normal, 1 si es admin, 2 si es Super-Admin</returns>
        public int tipoRol(string correo) {
            comando = new SQLiteCommand("SELECT Rol FROM Usuario WHERE Correo = '" + correo + "';", conexion);
            conexion.Open();
            try
            {
                if (Convert.ToInt32(comando.ExecuteScalar()) == 0)
                    return 0;
                else if (Convert.ToInt32(comando.ExecuteScalar()) == 1)
                    return 1;
                else
                    return 2;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("ERROR de comprobacion de rol del usuario con correo:\n " + correo + "\n", ex.Message);
                return -1;
            }
            finally
            {
                conexion.Close();
            }
        }

        //Metodo que obtiene los datos del usuario
        /// <summary>
        /// Metodo que devuelve un objeto Usuario
        /// </summary>
        /// <param name="correo">Correo del usuario a buscar</param>
        public Usuario getUser(string correo)
        {
            comando = new SQLiteCommand("SELECT * FROM `Usuario` WHERE `Correo` = '" + correo + "'", conexion);
            conexion.Open();
            Usuario userBuscado = null;
            try
            {
                dr = comando.ExecuteReader();
                if (dr.Read())
                {
                    Correo = dr["Correo"].ToString();
                    Password = dr["Password"].ToString();
                    Rol = dr["Rol"].ToString();
                    Nombre = dr["Nombre"].ToString();
                    Avatar = dr["Avatar"].ToString();
                    Estilo = dr["Estilo"].ToString();
                    Fecha = dr["Fecharegis"].ToString();
                    string Pregunta = dr["Pregunta"].ToString();
                    string Respuesta = dr["Respuesta"].ToString();
                    userBuscado = new Usuario(Correo, Password, Rol, Nombre, Avatar, Estilo, Fecha, Pregunta, Respuesta);
                }
            }
            catch (System.NullReferenceException ex)
            {
                MessageBox.Show("Usuario no encontrado\n" + ex.Message);
            }
            finally
            {
                dr.Close();
                conexion.Close();
            }
            return userBuscado;
        }

        //Metodo que obtiene todos los datos de todos los usuarios
        /// <summary>
        /// Metodo que devuelve una lista con todos los usuarios
        /// </summary>
        /// <returns>Devuelve una lista con todos los usuarios</returns>
        public List<Usuario> getTotalUsers()
        {
            List<Usuario> listauser = new List<Usuario>();
            comando = new SQLiteCommand("SELECT * FROM `Usuario`", conexion);
            conexion.Open();
            try
            {
                dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    Correo = dr["Correo"].ToString();
                    Password = dr["Password"].ToString();
                    Rol = dr["Rol"].ToString();
                    Nombre = dr["Nombre"].ToString();
                    Avatar = dr["Avatar"].ToString();
                    Estilo = dr["Estilo"].ToString();
                    Fecha = dr["Fecharegis"].ToString();
                    string Pregunta = dr["Pregunta"].ToString();
                    string Respuesta = dr["Respuesta"].ToString();
                    listauser.Add(new Usuario(Correo, Password, Rol, Nombre, Avatar, Estilo, Fecha, Pregunta, Respuesta));

                }
            }
            catch (System.NullReferenceException ex)
            {
                MessageBox.Show("Usuario no encontrado\n" + ex.Message);
            }
            finally
            {
                dr.Close();
                conexion.Close();
            }
            return listauser;
        }

        //metodo que cambia el nombre del usuario
        /// <summary>
        /// Metodo que cambia el nombre del usuario y comprueba que ese nombre no este en uso
        /// Actualiza el nombre en la tabla usuarios, comentarios y peticiones.
        /// No cambia en la tabla betados ya que un usuario betado no puede cambiar el nombre porque no puede acceder
        /// </summary>
        /// <param name="correo">Correo del usuario que quiere cambiar el nombre</param>
        /// <param name="nuevoNombre">Nuevo nombre</param>
        public void CambiarNombre(string correo, string nuevoNombre)
        {
            //Tenemos que modificar el nombre en usuarios y en Comentarios (En betados no, porque si esta en betados no podria acceder al cambio de nick)
            try
            {
                conexion.Open();
                int count;

                //Buscamos si algun usuario utiliza ese nombre
                using (comando = new SQLiteCommand("SELECT COUNT(*) FROM Usuario WHERE Nombre = @NuevoNombre", conexion))
                {
                    comando.Parameters.AddWithValue("@NuevoNombre", nuevoNombre);
                    count = Convert.ToInt32(comando.ExecuteScalar());
                }

                if (count > 0)
                    MessageBox.Show("El nombre de usuario ya esta en uso, porfavor, prueba con un nuevo nombre");
                else
                {
                    //Usuarios
                    using (SQLiteCommand comando = new SQLiteCommand("UPDATE Usuario SET Nombre = @NuevoNombre WHERE Correo = @Correo", conexion))
                    {
                        comando.Parameters.AddWithValue("@NuevoNombre", nuevoNombre);
                        comando.Parameters.AddWithValue("@Correo", correo);

                        comando.ExecuteNonQuery();
                    }

                    //Comentario
                    using (SQLiteCommand comando = new SQLiteCommand("UPDATE Cometario SET Nombre = @NuevoNombre WHERE Correo = @Correo", conexion))
                    {
                        comando.Parameters.AddWithValue("@NuevoNombre", nuevoNombre);
                        comando.Parameters.AddWithValue("@Correo", correo);

                        comando.ExecuteNonQuery();
                    }

                    //Peticiones
                    using (SQLiteCommand comando = new SQLiteCommand("UPDATE Peticiones SET Nick = @NuevoNombre WHERE Correo = @Correo", conexion))
                    {
                        comando.Parameters.AddWithValue("@NuevoNombre", nuevoNombre);
                        comando.Parameters.AddWithValue("@Correo", correo);

                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: No se ha cambiado el nombre de usuario, prueba con otro\n" + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        //metodo que cambia el estilo del usuario
        public void CambiarEstilo(string correo, string estilo)
        {
            conexion.Open();
            comando = new SQLiteCommand("UPDATE Usuario SET Estilo = '" + estilo + "' WHERE Correo = '" + correo + "'", conexion);
            try
            {
                comando.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: No se ha podido cambiar el estilo\n" + ex.Message);
            }
            finally
            {
                conexion.Close();
            }

        }

        /// <summary>
        /// Metodo que devuelve la pregunta de seguridad de un usurio.
        /// </summary>
        /// <param name="corr">Correo del usuario</param>
        /// <returns>Devuelve la pregunta de seguridad</returns>
        public string getPregunta(string corr)
        {
            string preg = "0";
            try
            {
                conexion.Open();

                using (comando = new SQLiteCommand("SELECT COUNT(*) FROM Usuario WHERE Correo = @Correo", conexion))
                {
                    comando.Parameters.AddWithValue("Correo", corr);
                    int count = Convert.ToInt32(comando.ExecuteScalar());

                    if (count == 0)
                        return null;
                }

                using (comando = new SQLiteCommand("SELECT Pregunta FROM Usuario WHERE Correo = @Correo", conexion)) {
                    comando.Parameters.AddWithValue("Correo", corr);
                    preg = comando.ExecuteScalar().ToString();

                    return preg != null ? preg : null;
                }
            }
            catch (SQLiteException ex)
            {
                return null;
            }
            finally
            {
                conexion.Close();
            }
            return null;
        }

        /// <summary>
        /// Metodo que devuelve la respuesta de seguridad de un usurio.
        /// </summary>
        /// <param name="corr">Correo del usuario</param>
        /// <returns>Devuelve la respuesta de seguridad</returns>
        public string getRespuesta(string corr)
        {
            try
            {
                conexion.Open();

                using (comando = new SQLiteCommand("SELECT COUNT(*) FROM Usuario WHERE Correo = @Correo", conexion))
                {
                    comando.Parameters.AddWithValue("Correo", corr);
                    int count = Convert.ToInt32(comando.ExecuteScalar());

                    if (count == 0)
                        return null;
                }

                using (comando = new SQLiteCommand("SELECT Respuesta FROM Usuario WHERE Correo = @Correo", conexion))
                {
                    comando.Parameters.AddWithValue("Correo", corr);
                    return comando.ExecuteScalar().ToString();
                }
            }
            catch (SQLiteException ex)
            {
                return null;
            }
            finally
            {
                conexion.Close();
            }
            return null;
        }

        //metodo que inserta una pelicula en la base de datos
        /// <summary>
        /// Inserta una pelicula en la tabla Peliculas
        /// </summary>
        /// <param name="peli">Objeto Movie a insertar</param>
        public void InsertatPeli(Movie peli)
        {
            if (conexion != null)
                conexion.Close();

            try
            {
                conexion.Open();
                if (peli.getTitulo() != "Desconocido") {
                    comando = new SQLiteCommand("INSERT INTO Peliculas (Titulo, Anio, Director, Reparto, Sinopsis, Calificacion, Poster, Duracion, Genero, Pais, Premios) VALUES (@Titulo, @Anio, @Director, @Reparto, @Sinopsis, @Calificacion, @Poster, @Duracion, @Genero, @Pais, @Premios)", conexion);

                    comando.Parameters.AddWithValue("@Titulo", peli.getTitulo());
                    comando.Parameters.AddWithValue("@Anio", peli.getAnio());
                    comando.Parameters.AddWithValue("@Director", peli.getDirector());
                    comando.Parameters.AddWithValue("@Reparto", peli.getReparto());
                    comando.Parameters.AddWithValue("@Sinopsis", peli.getSinopsis());
                    comando.Parameters.AddWithValue("@Calificacion", peli.getCalificacion());
                    comando.Parameters.AddWithValue("@Poster", peli.ObtenerImagen());
                    comando.Parameters.AddWithValue("@Duracion", peli.getDuracion());
                    comando.Parameters.AddWithValue("@Genero", peli.getGenero());
                    comando.Parameters.AddWithValue("@Pais", peli.getPais());
                    comando.Parameters.AddWithValue("@Premios", peli.getPremios());

                    comando.ExecuteNonQuery();
                }
                
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: No se ha podido insertar la pelicula\n" + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        //MEtodo que "borra" un usuario
        /// <summary>
        /// Metodo que mueve un usuario de la tabla Usuarios a la tabla Borrados
        /// </summary>
        /// <param name="correo"></param>
        public void EraseUser(string correo) {

            Usuario borr = getUser(correo);
            try
            {

                conexion.Open();
                using (comando = new SQLiteCommand("INSERT INTO Borrados (Correo, Password, Rol, Nombre, Avatar, Estilo, FechaRegis, Pregunta, Respuesta) VALUES (@Cor, @Pass, @Rol, @Nom, @Ava, @Est, @Fec, @Pre, @Res)", conexion))
                {
                    comando.Parameters.AddWithValue("@Cor", borr.Correo);
                    comando.Parameters.AddWithValue("@Pass", borr.Password);
                    comando.Parameters.AddWithValue("@Rol", borr.Rol);
                    comando.Parameters.AddWithValue("@Nom", borr.Nombre);
                    comando.Parameters.AddWithValue("@Ava", borr.Avatar);
                    comando.Parameters.AddWithValue("@Est", borr.Estilo);
                    comando.Parameters.AddWithValue("@Fec", borr.FechaRegistro);
                    comando.Parameters.AddWithValue("@Pre", borr.Pregunta);
                    comando.Parameters.AddWithValue("@Res", borr.Respuesta);

                    comando.ExecuteNonQuery();
                }

                using(comando = new SQLiteCommand("DELETE FROM Usuario WHERE Correo = @Correo", conexion))
                {
                    comando.Parameters.AddWithValue("Correo", correo);

                    comando.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("ERROR: No se ha podido borrar el usuario " + borr.Nombre + " por motivo:\n" + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
            
            
        }

        //metodo que busca una pelicula en la tabla peliculas
        /// <summary>
        /// Metodo que devuelve una pelicula de la tabla Peliculas
        /// </summary>
        /// <param name="titulo">Titulo de la pelicula a buscar</param>
        /// <returns>Devuleve un objeto Movie con la pelicula buscada</returns>
        public Movie BuscarPeliBD(string titulo)
        {
            comando = new SQLiteCommand("SELECT * FROM Peliculas WHERE Titulo = '" + titulo + "'", conexion);
            conexion.Open();
            Movie movie = null;
            try
            {
                dr = comando.ExecuteReader();
                if (dr.Read())
                {
                    var Poster = dr["Poster"].ToString();
                    var Titulo = dr["Titulo"].ToString();
                    var Anio = Convert.ToInt32(dr["Anio"].ToString());
                    var Director = dr["Director"].ToString();
                    var Reparto = dr["Reparto"].ToString();
                    var Sinopsis = dr["Sinopsis"].ToString();
                    var Calificacion = dr["Calificacion"].ToString();
                    var Duracion = dr["Duracion"].ToString();
                    var Genero = dr["Genero"].ToString();
                    var Pais = dr["Pais"].ToString();
                    var Premios = dr["Premios"].ToString();
                    movie = new Movie(Titulo, Anio, Director, Reparto, Sinopsis, Calificacion, Poster, Duracion, Genero, Pais, Premios);
                }
            }
            catch (System.NullReferenceException ex)
            {
                MessageBox.Show("Pelicula no encontrada\n" + ex.Message);
            }
            finally
            {
                dr.Close();
                conexion.Close();
            }
            return movie;
        }

        //Metodo que obtiene el numero de registros de la tabla Peliculas
        /// <summary>
        /// Metodo que devuelve el numero de todos los registros de la tabla Peliculas
        /// </summary>
        /// <returns>Numero total de registros de la tabla</returns>
        public int TotalPeliculas() {
            int registros = 0;
            try {
                comando = new SQLiteCommand("SELECT COUNT(*) FROM Peliculas", conexion);
                conexion.Open();
                registros = Convert.ToInt32(comando.ExecuteScalar());
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("ERROR al obtener el numero de peliculas almacenadas en la tabla 'Peliculas':\n" + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
            return registros;
        }

        

        /// <summary>
        /// Obtiene la informacion del estado de una pelicula de un usuario en concreto buscado por el titulo y el correo
        /// </summary>
        /// <param name="correo">Correo del usuario</param>
        /// <param name="titulo">Titulo de la pelicula</param>
        /// <returns>Devuelve el estado</returns>
        public int InfoUsuarioPelicula(string correo, string titulo)
        {
            try {
                conexion.Open();
                comando = new SQLiteCommand("SELECT COUNT(*) FROM PelisEstado WHERE CorreoUser = '" + correo + "' AND Titulo = '" + titulo + "';", conexion);
                if (Convert.ToInt32(comando.ExecuteScalar()) > 0)
                {
                    //comprobamos el estado de la pelicula
                    comando = new SQLiteCommand("SELECT estado FROM PelisEstado WHERE CorreoUser = '" + correo + "' AND Titulo = '" + titulo + "';", conexion);
                    try
                    {
                        //si esta en pendientes no esta en favoritos
                        if (Convert.ToInt32(comando.ExecuteScalar()) == 1)
                            return 1;
                        else if (Convert.ToInt32(comando.ExecuteScalar()) == 2)
                            return 2;
                        //si esta en favoritos no puede estar en pendiente
                        else if (Convert.ToInt32(comando.ExecuteScalar()) == 3)
                            return 3;
                    }
                    catch (SQLiteException)
                    {
                        return 0;
                    }
                    finally
                    {
                        conexion.Close();
                    }
                }
            }
            catch (SQLiteException)
            {
                return 0;
            }
            finally
            {
                conexion.Close();
            }

            return 0;
        }

        //metodo que obtiene las listas de peliculas de un usuario dependiendo de si son vistas, pendientes o favoritas.
        /// <summary>
        /// Metodo que obtiene las listas de peliculas de un usuario dependiendo de si son vistas, pendientes o favoritas.
        /// </summary>
        /// <param name="correo">Correo del usuario</param>
        /// <param name="estado">Estado de la pelicula (Vista, pendiente...)</param>
        /// <returns>Devuelve una lista con las calificaciones</returns>
        public List<string> getListasUser(string correo, string estado) {
            List<string> devolver = new List<string>();
            conexion.Open();
            using (comando = new SQLiteCommand("SELECT Titulo FROM PelisEstado WHERE CorreoUser = @CorreoUser AND Estado = @Est", conexion)) {
                comando.Parameters.AddWithValue("@CorreoUser", correo);
                comando.Parameters.AddWithValue("@Est", estado);

                try
                {
                    dr = comando.ExecuteReader();
                    while (dr.Read())
                    {
                        devolver.Add(dr["Titulo"].ToString());
                    }
                }
                catch (SQLiteException)
                {
                }
                finally
                {
                    conexion.Close();
                }

            }

            return devolver;

        }

        //Metodo que devuelve la calificacion que el usuario a dado de una pelicula en concreto
        /// <summary>
        /// Metodo que devuelve la calificacion que el usuario a dado de una pelicula en concreto
        /// </summary>
        /// <param name="correo">Correo del usuario</param>
        /// <param name="pelicula">Titulo de una pelicula</param>
        /// <returns>devuelve la calificaion de una pelicula de un usuario</returns>
        public int getCaliUser(string correo, string pelicula) {

            //comprobamos el estado de la pelicula
            comando = new SQLiteCommand("SELECT calificacion FROM PelisEstado WHERE CorreoUser = '" + correo + "' AND titulo = '" + pelicula + "';", conexion);
            conexion.Open();
            try
            {
                int cali;
                try
                {
                    cali = Convert.ToInt32(comando.ExecuteScalar());
                }
                catch (System.InvalidCastException)
                {
                    cali = 0;
                }

                switch (cali) {
                    case 0:
                        return 0;
                    case 1:
                        return 1;
                    case 2:
                        return 2;
                    case 3:
                        return 3;
                    case 4:
                        return 4;
                    case 5:
                        return 5;
                    case 6:
                        return 6;
                    case 7:
                        return 7;
                    case 8:
                        return 8;
                    case 9:
                        return 9;
                    case 10:
                        return 10;
                    default:
                        return -1;
                }
            }
            catch (SQLiteException)
            {
                return 0;
            }
            finally
            {
                conexion.Close();
            }
            return 0;
        }

        //Metodo que modifica el estado de una pelicula, si no se encuentra se añade
        //Estados:
        //0 - nada
        //1 - pendiente
        //2 - vista
        //3 - fav
        /// <summary>
        /// Metodo que modifica el estado de una pelicula de un usuario en concreto.
        /// Si el registro de la pelicula no existe, se añade a la tabla
        /// </summary>
        /// <param name="correo">Correo del usuario</param>
        /// <param name="titulo">Titulo de la pelicula</param>
        /// <param name="est">Estado de la peliculas</param>
        /// <param name="gener">Genero de la pelicula</param>
        public void ModEstadoPeli(string correo, string titulo, int est, string gener){
            using (comando = new SQLiteCommand("SELECT COUNT(*) FROM PelisEstado WHERE CorreoUser = @correo AND Titulo = @titulo", conexion)) {
                comando.Parameters.AddWithValue("@correo", correo);
                comando.Parameters.AddWithValue("@titulo", titulo);
                conexion.Close();
                conexion.Open();

                try {
                    int existe = Convert.ToInt32(comando.ExecuteScalar());
                    if (existe < 1)
                    {
                        using (comando = new SQLiteCommand("INSERT INTO PelisEstado (CorreoUser, Titulo, Estado, Genero) VALUES (@correo, @titulo, @est, @gen)", conexion))
                        {
                            comando.Parameters.AddWithValue("@correo", correo);
                            comando.Parameters.AddWithValue("@titulo", titulo);
                            comando.Parameters.AddWithValue("@est", est);
                            comando.Parameters.AddWithValue("@gen", gener);
                            comando.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (comando = new SQLiteCommand("UPDATE PelisEstado SET Estado = @Estado WHERE CorreoUser = @correo AND Titulo = @titulo", conexion))
                        {
                            comando.Parameters.AddWithValue("@correo", correo);
                            comando.Parameters.AddWithValue("@titulo", titulo);
                            comando.Parameters.AddWithValue("@Estado", est);
                            comando.ExecuteNonQuery();
                        }
                    }
                }
                    catch (SQLiteException ex)
                {
                    MessageBox.Show("ERROR al modificar la calificación de la película " + titulo + " de " + correo + "\n" + ex.Message);
                }
                finally {
                    conexion.Close();
                }
            }
        }

        //Metodo que modifica la calificacion de la pelicula segun el usuario y el titulo
        /// <summary>
        /// Metodo que modifica la calificación del usuario de una pelicula.
        /// </summary>
        /// <param name="correo">Correo del usuarios</param>
        /// <param name="titulo">Titulo de la pelicula</param>
        /// <param name="calificacion">Nueva calificación</param>
        /// <param name="genero">Genero de la pelicula</param>
        public void ModCaliUser(string correo, string titulo, int calificacion, string genero)
        {
            int cali;

            // Comprobamos si la película se encuentra en la lista
            using (comando = new SQLiteCommand("SELECT COUNT(*) FROM PelisEstado WHERE CorreoUser = @correo AND Titulo = @titulo", conexion))
            {
                comando.Parameters.AddWithValue("@correo", correo);
                comando.Parameters.AddWithValue("@titulo", titulo);
                conexion.Close();
                conexion.Open();

                try
                {
                    int existe = Convert.ToInt32(comando.ExecuteScalar());

                    if (existe < 1)
                    {
                        using (comando = new SQLiteCommand("INSERT INTO PelisEstado (CorreoUser, Titulo, Calificacion, Estado, Genero) VALUES (@correo, @titulo, @calificacion, @est, @gen)", conexion))
                        {
                            comando.Parameters.AddWithValue("@correo", correo);
                            comando.Parameters.AddWithValue("@titulo", titulo);
                            comando.Parameters.AddWithValue("@calificacion", calificacion);
                            comando.Parameters.AddWithValue("@est", 0);
                            comando.Parameters.AddWithValue("@gen", genero);
                            comando.ExecuteNonQuery();
                        }
                    }
                    // Se actualiza el estado
                    else
                    {
                        using (comando = new SQLiteCommand("UPDATE PelisEstado SET Calificacion = @calificacion WHERE CorreoUser = @correo AND Titulo = @titulo", conexion))
                        {
                            comando.Parameters.AddWithValue("@correo", correo);
                            comando.Parameters.AddWithValue("@titulo", titulo);
                            comando.Parameters.AddWithValue("@calificacion", calificacion);
                            comando.ExecuteNonQuery();
                        }
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("ERROR al modificar la calificación de la película " + titulo + " de " + correo + "\n" + ex.Message);
                }
                finally
                {
                    conexion.Close();
                }
            }
        }

        //Metodo que devuelve el numero de personas que han votado por la pelicula
        /// <summary>
        /// Metodo que devuelve el numero de personas que han votado por la pelicula
        /// </summary>
        /// <param name="titulo">Titulo de la pelicula</param>
        /// <returns>Numero total de votos de personas vieron la pelicula</returns>
        public int getNumVotos(string titulo) {
            int votos;

            comando = new SQLiteCommand("SELECT COUNT(*) FROM PelisEstado WHERE Titulo = '" + titulo + "'", conexion);
            conexion.Open();
            try
            {
                votos = Convert.ToInt32(comando.ExecuteScalar());
            }
            catch (SQLiteException)
            {
                votos = 0;
            }
            finally {
                conexion.Close();
            }

            return votos;
        }

        //Metodo que devuelve una lista con todas las calificaciones de la pelicula
        /// <summary>
        /// Metodo que devuelve una lista con todas las calificaciones de la pelicula
        /// </summary>
        /// <param name="titulo">Titulo de la pelicula</param>
        /// <returns>Lista de valoraciones</returns>
        public List<int> getValoraciones(string titulo)
        {
            List<int> votos = new List<int>();

            comando = new SQLiteCommand("SELECT calificacion FROM PelisEstado WHERE titulo = '" + titulo + "'", conexion);
            conexion.Open();

            try
            {
                using (SQLiteDataReader dr = comando.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["calificacion"] != DBNull.Value)
                        {
                            votos.Add(Convert.ToInt32(dr["calificacion"]));
                        }
                        else
                        {
                            votos.Add(0);
                        }
                    }
                }
            }
            catch (SqlException) { }
            finally
            {
                conexion.Close();
            }

            return votos;
        }

        //metodo que devuelve una lista con los comentarios de una pelicula en concreto
        /// <summary>
        /// metodo que devuelve una lista con los comentarios de una pelicula en concreto
        /// </summary>
        /// <param name="titulo">Titulo de la pelicula</param>
        /// <returns>Lista de obetos comentario</returns>
        public List<Comentario> getComentarios(string titulo)
        {

            Comentario com;
            List<Comentario> listcoment = new List<Comentario>();
            //los comentarios se ordenaran for fecha y hora ascendente
            using (comando = new SQLiteCommand("SELECT Nombre, Comentario FROM Cometario WHERE titulo = @TituPeli ORDER BY FechaHora DESC", conexion)) {
                comando.Parameters.AddWithValue("@TituPeli", titulo);
                try
                {
                    conexion.Open();
                    dr = comando.ExecuteReader();
                    while (dr.Read())
                    {
                        com = new Comentario(dr["Nombre"].ToString(), dr["Comentario"].ToString());
                        listcoment.Add(com);
                    }
                }
                catch (SqlException)
                {

                }
                finally
                {
                    conexion.Close();
                }
            }
                
            return listcoment;
        }

        //metodo que devuelve una lista con los comentarios de un usuario en concreto
        /// <summary>
        /// metodo que devuelve una lista con los comentarios de un usuario en concreto
        /// </summary>
        /// <param name="correo">Correo del usuario</param>
        /// <returns>Lista de objetos Comentario de un usuario</returns>
        public List<Comentario> getComentariosUser(string correo)
        {

            Comentario com;
            List<Comentario> listcoment = new List<Comentario>();
            //los comentarios se ordenaran for fecha y hora ascendente
            
            using (comando = new SQLiteCommand("SELECT Nombre, Comentario FROM Cometario WHERE Correo = @CorreoUser ORDER BY FechaHora DESC", conexion)) {
                try
                {
                    comando.Parameters.AddWithValue("@CorreoUser", correo);
                    conexion.Open();
                    dr = comando.ExecuteReader();
                    while (dr.Read())
                    {
                        com = new Comentario(dr["Nombre"].ToString(), dr["Comentario"].ToString());
                        listcoment.Add(com);
                    }
                }
                catch (SqlException)
                {

                }
                finally { conexion.Close(); }
            }
                
            return listcoment;
        }

        //metodo que inserta un comentario nuevo
        /// <summary>
        /// Inserta un comentario en la tabla Cometario
        /// </summary>
        /// <param name="titulo">Titulo de la pelicula</param>
        /// <param name="correo">Correo del usuario</param>
        /// <param name="nombre">Nombre del usuario (puede ser anónimo)</param>
        /// <param name="comentario">Comentario de la pelicula</param>
        /// <param name="fechaHora">Fecha en la que se ha insertado</param>
        public void InsertComentario(string titulo, string correo, string nombre, string comentario, string fechaHora)
        {
            conexion.Open();
            try
            {
                using (SQLiteCommand comando = new SQLiteCommand("INSERT INTO Cometario (Titulo, Correo, Nombre, Comentario, FechaHora) VALUES (@Titulo, @Correo, @Nombre, @Comentario, @FechaHora)", conexion))
                {
                    comando.Parameters.AddWithValue("@Titulo", titulo);
                    comando.Parameters.AddWithValue("@Correo", correo);
                    comando.Parameters.AddWithValue("@Nombre", nombre);
                    comando.Parameters.AddWithValue("@Comentario", comentario);
                    comando.Parameters.AddWithValue("@FechaHora", fechaHora);

                    comando.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: No se ha podido insertar el comentario\n" + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        //metodo que elimina un comentario por adiministrador
        /// <summary>
        /// Metodo que borra un comentario específico de un usuario
        /// </summary>
        /// <param name="nombre">Nombre del usuario</param>
        /// <param name="comen">Comnetario</param>
        public void EraseComentario(string nombre, string comen)
        {
            conexion.Open();
            try
            {
                using (SQLiteCommand comando = new SQLiteCommand("DELETE FROM Cometario WHERE Nombre = @Nombre AND Comentario = @Comentario", conexion))
                {
                    comando.Parameters.AddWithValue("@Nombre", nombre);
                    comando.Parameters.AddWithValue("@Comentario", comen);

                    comando.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: No se ha podido borrar el comentario\n" + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        //Metodo para insertar una imagen para usarla de avatar
        /// <summary>
        /// Metodo que inserta en la base de datos una imagen seleccionada por el usuario
        /// </summary>
        public void InsertarAvatar() {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                try
                {
                    string fileName = System.IO.Path.GetFileName(imagePath);
                    byte[] imageData = File.ReadAllBytes(imagePath);
                    conexion.Open();

                    using (SQLiteCommand command = new SQLiteCommand("INSERT INTO Avatar (Imagen) VALUES ( @Imagen)", conexion))
                    {
                        command.Parameters.AddWithValue("@Imagen", imageData);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("La imagen se ha guardado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar la imagen: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            conexion.Close();
        }

        //metodo que busca el avatar del usuario
        /// <summary>
        /// Metodo que devuelve el avatar de un usuario por el id de su campo avatar.
        /// </summary>
        /// <param name="idUser">ID del avatar</param>
        /// <returns>Devuelve una imagen del avatar</returns>
        public BitmapImage CargarAvatar(int idUser)
        {
            BitmapImage bitmapImage = new BitmapImage();
            try
            {
                conexion.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT Imagen FROM Avatar WHERE Id = '" + idUser + "'", conexion))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            byte[] imageData = (byte[])reader["Imagen"];

                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = new MemoryStream(imageData);
                            bitmapImage.EndInit();
                        }
                        else
                        {
                            MessageBox.Show("Error: No se encontró ninguna imagen en la base de datos.", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: gagaga" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conexion.Close();
            }

            return bitmapImage;
        }

        //Devolvemos todos los avatares de la base de datos
        /// <summary>
        /// Metodo que devuleve todos los avatares
        /// </summary>
        /// <returns>Lista de Imagenes en modo BitmapImage de todos los avatares de la tabla Avatar</returns>
        public List<BitmapImage> AvatarTotal()
        {
            List<BitmapImage> ListaAvatar = new List<BitmapImage>();

            using (var comando = new SQLiteCommand("SELECT * FROM Avatar", conexion))
            {
                conexion.Open();

                using (var dr = comando.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        byte[] bytesImagen = (byte[])dr["Imagen"];

                        BitmapImage imagen = new BitmapImage();

                        try
                        {
                            using (var stream = new System.IO.MemoryStream(bytesImagen))
                            {
                                stream.Position = 0;
                                imagen.BeginInit();
                                imagen.CacheOption = BitmapCacheOption.OnLoad;
                                imagen.StreamSource = stream;
                                imagen.EndInit();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("ERROR: no se pudiero obtener los avatares" + ex.Message);
                        }

                        ListaAvatar.Add(imagen);
                    }
                }

                conexion.Close();
            }

            return ListaAvatar;
        }

        /// <summary>
        /// Actualiza el avatar del usuario
        /// </summary>
        /// <param name="id">ID del avatar seleccionado</param>
        /// <param name="user">usuario que cambia el avatar</param>
        public void cambiarAvatar(int id, Usuario user)
        {
            conexion.Open();
            try {
                using (comando = new SQLiteCommand("UPDATE Usuario SET Avatar = @id WHERE Correo = @correo", conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    comando.Parameters.AddWithValue("@correo", user.Correo);

                    comando.ExecuteNonQuery();
                    MessageBox.Show("Avatar cambiado correctamente.");
                }
            }
            catch (SqlException ex) {
                MessageBox.Show("ERROR: no se pudo cambiar el avatar del usuario " + user.Nombre);
            }
            finally
            {
                conexion.Close();
            }
            
        }

        /// <summary>
        /// Selecciona todos los ids de los avatares
        /// </summary>
        /// <returns>Lista numerica con los ids de avatares</returns>
        public List<int> SelecIdesAvatar()
        {
            List<int> ides = new List<int>();

            using (var comando = new SQLiteCommand("SELECT * FROM Avatar", conexion))
            {
                conexion.Open();

                using (var dr = comando.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ides.Add(Convert.ToInt32(dr["Id"]));
                    }
                }

                conexion.Close();
            }

            return ides;
        }

        //metodo que obtiene los generos de la tabla PelisEstado
        //SIEMPRE Y CUANDO EL ESTADO SEA FAVORITO O VISTO
        //este metodo se usara para saber el genero más visto por la persona para recomendarle peliculas nuevas
        /// <summary>
        /// Metodo que obtiene los generos de la tabla PelisEstado
        /// </summary>
        /// <param name="user">usuario actual</param>
        /// <param name="opc">Estado de la pelicula</param>
        /// <returns>Lista de string con los generos</returns>
        public List<string> getGenero(Usuario user, int opc)
        {
            conexion.Open();
            List<string> listagen = new List<string>();
            using (comando = new SQLiteCommand("SELECT Genero FROM PelisEstado WHERE CorreoUser = @Correo AND Estado = @Estado", conexion))
            {

                comando.Parameters.AddWithValue("@Correo", user.Correo);
                comando.Parameters.AddWithValue("@Estado", opc);

                try
                {
                    dr = comando.ExecuteReader();

                    while (dr.Read())
                    {
                        listagen.Add(dr["Genero"].ToString());
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("Error: No se ha podido obtener los generos de la base de datos\n" + ex.Message);
                }
                finally
                {
                    conexion.Close();
                }

            }
            return listagen;
        }

        /// <summary>
        /// Obtiene la lista de géneros de las películas en una lista específica (pendientes, vistas o favoritas) para un usuario.
        /// </summary>
        /// <param name="user">Usuario actual</param>
        /// <param name="opc">Estado de la pelicula.</param>
        /// <returns>Lista de géneros de las películas segun un estado</returns>
        public List<string> getGeneroTodos(Usuario user, int opc)
        {
            conexion.Open();
            List<string> listagen = new List<string>();
            using (comando = new SQLiteCommand("SELECT Genero FROM PelisEstado WHERE Estado = @Estado", conexion))
            {

                comando.Parameters.AddWithValue("@Estado", opc);

                try
                {
                    dr = comando.ExecuteReader();

                    while (dr.Read())
                    {
                        listagen.Add(dr["Genero"].ToString());
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("Error: No se ha podido obtener los generos de la base de datos\n" + ex.Message);
                }
                finally
                {
                    conexion.Close();
                }

            }
            return listagen;
        }

        
        /// <summary>
        /// Obtiene los id de los generos de la api TMDB
        /// </summary>
        /// <param name="popu">genero tipo string (Comedia, Terror...)</param>
        /// <returns>devuelve el ID del genero buscdo</returns>
        //obtener los id de los generos de la api TMBD
        public string getGenerosPorPopu(string popu)
        {
            string id = null;
            conexion.Open();
            using (comando = new SQLiteCommand("SELECT Id FROM Generos WHERE Tipo = @Tipus", conexion))
            {
                comando.Parameters.AddWithValue("@Tipus", popu);
                try
                {
                    dr = comando.ExecuteReader();

                    while (dr.Read())
                    {
                        id = (dr["Id"].ToString());
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("Error: No se ha podido obtener el id del genero popular\n" + ex.Message);
                }
                finally
                {
                    conexion.Close();
                }
            }
            return id;
        }

        //metodo que inserta la peticion de un usuario
        /// <summary>
        /// Metodo que inserta la Peticion de un usuario
        /// </summary>
        /// <param name="correo">Correo del solicitante</param>
        /// <param name="nick">Nombre del solicitante</param>
        /// <param name="asunto">Asunto del mensaje</param>
        /// <param name="mensaje">Peticion</param>
        public void InsertarPeticion(string correo, string nick, string asunto, string mensaje)
        {
            try
            {
                conexion.Open();

                using (comando = new SQLiteCommand("INSERT INTO Peticiones (Correo, Nick, Asunto, Mensaje) VALUES (@Correo, @Nick, @Asunto, @Mensaje)",conexion))
                {
                    comando.Parameters.AddWithValue("@Correo", correo);
                    comando.Parameters.AddWithValue("@Nick", nick);
                    comando.Parameters.AddWithValue("@Asunto", asunto);
                    comando.Parameters.AddWithValue("@Mensaje", mensaje);

                    comando.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                // Manejar la excepción según tus necesidades, puedes mostrar un mensaje o hacer algún registro.
                Console.WriteLine("Error al insertar en la tabla Peticiones: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        //Borra una peticion
        /// <summary>
        /// MEtodo que elimina una peticion
        /// </summary>
        /// <param name="correo">Correo del solicitante</param>
        /// <param name="mensaje">Petición</param>
        public void BorrarPeticion(string correo, string mensaje)
        {
            try
            {
                conexion.Open();

                using (comando = new SQLiteCommand("DELETE FROM Peticiones WHERE Correo = @Correo AND Mensaje = @Mensaje", conexion))
                {
                    comando.Parameters.AddWithValue("@Correo", correo);
                    comando.Parameters.AddWithValue("@Mensaje", mensaje);

                    comando.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                // Manejar la excepción según tus necesidades, puedes mostrar un mensaje o hacer algún registro.
                MessageBox.Show("No se pudo eliminar petición:\n" + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        /// <summary>
        /// Metodo que obtiene todos las peticiones 
        /// </summary>
        /// <returns>Lista de objetos Petición</returns>
        public List<Peticion> TotalPeticion()
        {
            List<Peticion> listaPet = new List<Peticion>();
            string corr;
            string nick;
            string asunto;
            string mensaje;
            int id = 0;
            try
            {
                conexion.Open();

                using (comando = new SQLiteCommand("SELECT * FROM Peticiones", conexion))
                {
                    dr = comando.ExecuteReader();
                    while(dr.Read())
                    {
                        
                        corr = dr["Correo"].ToString();
                        nick = dr["Nick"].ToString();
                        asunto = dr["Asunto"].ToString();
                        mensaje = dr["Mensaje"].ToString();
                        id += 1;
                        Peticion nuevo = new Peticion(corr, nick, asunto, mensaje);
                        listaPet.Add(nuevo);
                    }
                    return listaPet;
                }
            }
            catch (SQLiteException ex)
            {
                // Manejar la excepción según tus necesidades, puedes mostrar un mensaje o hacer algún registro.
                Console.WriteLine("Error al resolverla peticion: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
            return null;
        }

    }
}
