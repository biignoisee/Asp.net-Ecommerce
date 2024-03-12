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
    public class CD_Cliente
    {

        //METODO LISTAR USUARIOS

        public List<Cliente> Listar()
        {
            List<Cliente> listaCliente = new List<Cliente>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    string query = "select IdCliente, Nombres, Apellidos, Correo, Clave, Reestablecer from Cliente";

                    SqlCommand command = new SqlCommand(query, oconexion);
                    command.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader datareader = command.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaCliente.Add(
                                new Cliente()
                                {
                                    IdCliente = Convert.ToInt32(datareader["IdCliente"]),
                                    Nombres = datareader["Nombres"].ToString(),
                                    Apellidos = datareader["Apellidos"].ToString(),
                                    Correo = datareader["Correo"].ToString(),
                                    Clave = datareader["Clave"].ToString(),
                                    Reestablecer = Convert.ToBoolean(datareader["Reestablecer"])
                                });
                        }
                    }
                }
            }
            catch
            {
                listaCliente = new List<Cliente>();
            }

            return listaCliente;

        }



        //METODO CREAR USUARIO

        public int Registrar(Cliente obj, out string Mensaje)
        {
            // o era el cliente dentro como parametro instanciando el obj, o podia haber sido asi:
            // Cliente obj = new Cliente();

            int idAutogenerada = 0;
            Mensaje = string.Empty;  //mensaje estara vacio por ahora

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("sp_RegistrarCliente", oconexion);
                    command.Parameters.AddWithValue("Nombres", obj.Nombres);
                    command.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    command.Parameters.AddWithValue("Correo", obj.Correo);
                    command.Parameters.AddWithValue("Clave", obj.Clave);
                    command.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    command.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    command.ExecuteNonQuery();

                    idAutogenerada = Convert.ToInt32(command.Parameters["Resultado"].Value);
                    Mensaje = command.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                idAutogenerada = 0;
                Mensaje = ex.Message;  //mensaje de la excepción
            }
            return idAutogenerada;
        }


        //Creamos el metodo de cambiar clave que permita al cliente luego de loguearse cambiar clave

        public bool CambiarClave(int idCliente, string nuevaClave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("update cliente set clave = @nuevaClave, reestablecer = 0 where idCliente = @id", oconexion);
                    command.Parameters.AddWithValue("@id", idCliente);
                    command.Parameters.AddWithValue("@nuevaClave", nuevaClave);
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



        //Ahora toca el metodo Reestablecer Contraseña
        public bool ReestablecerClave(int idCliente, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("update cliente set clave = @clave, reestablecer = 1 where idCliente = @id", oconexion);
                    command.Parameters.AddWithValue("@id", idCliente);
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
