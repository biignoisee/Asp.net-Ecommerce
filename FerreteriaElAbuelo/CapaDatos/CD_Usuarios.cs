using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Añadimos la referencia
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;



namespace CapaDatos
{
    public class CD_Usuarios
    {
        //METODO LISTAR USUARIOS

        public List<Usuario> Listar()
        {
            List<Usuario> listaUsuario = new List<Usuario>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    string query = "select IdUsuario, Nombres, Apellidos, Correo, Clave, Reestablecer, Activo from Usuario";

                    SqlCommand command = new SqlCommand(query, oconexion);
                    command.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader datareader = command.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaUsuario.Add(
                                new Usuario()
                                {
                                    IdUsuario = Convert.ToInt32(datareader["IdUsuario"]),
                                    Nombres = datareader["Nombres"].ToString(),
                                    Apellidos = datareader["Apellidos"].ToString(),
                                    Correo = datareader["Correo"].ToString(),
                                    Clave = datareader["Clave"].ToString(),  
                                    Reestablecer = Convert.ToBoolean(datareader["Reestablecer"]),
                                    Activo = Convert.ToBoolean(datareader["Activo"])
                                });
                        }
                    }
                } 
            }
            catch
            {
                listaUsuario = new List<Usuario>();
            }

            return listaUsuario;

        }

        //METODO CREAR USUARIO

        public int Registrar(Usuario obj, out string Mensaje)
        {
            // o era el usuario dentro como parametro instanciando el obj, o podia haber sido asi:
            // Usuario obj = new Usuario();

            int idAutogenerada = 0;
            Mensaje = string.Empty;  //mensaje estara vacio por ahora

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("sp_RegistrarUsuario", oconexion);
                    command.Parameters.AddWithValue("Nombres", obj.Nombres);
                    command.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    command.Parameters.AddWithValue("Correo", obj.Correo);
                    command.Parameters.AddWithValue("Clave", obj.Clave);
                    command.Parameters.AddWithValue("Activo", obj.Activo);
                    command.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    command.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    command.ExecuteNonQuery();

                    idAutogenerada = Convert.ToInt32(command.Parameters["Resultado"].Value);
                    Mensaje = command.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch(Exception ex)
            {
                idAutogenerada = 0;
                Mensaje = ex.Message;  //mensaje de la excepción
            }
            return idAutogenerada;
        }



        public bool Editar(Usuario obj, out string Mensaje)
        {
            // o era el usuario dentro como parametro instanciando el obj, o podia haber sido asi:
            // Usuario obj = new Usuario();  

            bool resultado = false;   //POR AHORA FALSO, LUEGO CON LOS SCRIPT LO CAMBIA
            Mensaje = string.Empty;  //mensaje estara vacio por ahora

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("sp_EditarUsuario", oconexion);
                    command.Parameters.AddWithValue("IdUsuario", obj.IdUsuario);
                    command.Parameters.AddWithValue("Nombres", obj.Nombres);
                    command.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    command.Parameters.AddWithValue("Correo", obj.Correo);
                    //command.Parameters.AddWithValue("Clave", obj.Clave);
                    command.Parameters.AddWithValue("Activo", obj.Activo);
                    command.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    command.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    command.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(command.Parameters["Resultado"].Value);
                    Mensaje = command.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }


            return resultado;
        }


        //Eliminar
        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try{
                using(SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("delete top (1) from USUARIO where IdUsuario = @id", oconexion);
                    command.Parameters.AddWithValue("@id", id);
                    command.CommandType = CommandType.Text;

                    oconexion.Open();
                    resultado = command.ExecuteNonQuery() > 0 ? true : false; 
                    // executeNonQuery devuelve el numero de filas afectadas, por lo que pedimos > 0, si no devuelve 
                    // más de 0 es porque no fue afectada ninguna fila, por ende returna false
                }
            }
            catch(Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }
        
        //Creamos el metodo de cambiar clave que permita al usuario luego de loguearse cambiar clave

        public bool CambiarClave(int idUsuario, string nuevaClave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using(SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("update usuario set clave = @nuevaClave, reestablecer = 0 where idUsuario = @id", oconexion);
                    command.Parameters.AddWithValue("@id", idUsuario);
                    command.Parameters.AddWithValue("@nuevaClave", nuevaClave);
                    command.CommandType = CommandType.Text;

                    oconexion.Open();
                    resultado = command.ExecuteNonQuery() > 0 ? true : false;
                    // executeNonQuery devuelve el numero de filas afectadas, por lo que pedimos > 0, si no devuelve 
                    // más de 0 es porque no fue afectada ninguna fila, por ende returna false
                }

            } catch( Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }


            return resultado;
        }


        //Ahora toca el metodo Reestablecer Contraseña
        public bool ReestablecerClave(int idUsuario, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("update usuario set clave = @clave, reestablecer = 1 where idUsuario = @id", oconexion);
                    command.Parameters.AddWithValue("@id", idUsuario);
                    command.Parameters.AddWithValue("@clave", clave);
                    command.CommandType = CommandType.Text;

                    oconexion.Open();
                    resultado = command.ExecuteNonQuery() > 0 ? true : false;
                    // executeNonQuery devuelve el numero de filas afectadas, por lo que pedimos > 0, si no devuelve 
                    // más de 0 es porque no fue afectada ninguna fila, por ende returna false
                }

            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }


            return resultado;
        }


    }
}
