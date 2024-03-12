using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Añadimos la referencia
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Carrito
    {
        public bool ExisteCarrito(int idCliente, int idProducto)
        {
            bool resultado = true;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("sp_ExisteCarrito", oconexion);
                    command.Parameters.AddWithValue("IdCliente", idCliente);
                    command.Parameters.AddWithValue("IdProducto",idProducto);
                    command.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    command.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();


                    command.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(command.Parameters["Resultado"].Value);
                }
            }
            catch (Exception ex)
            {
                resultado = false;

            }
            return resultado;
        }



        public bool OperacionCarrito(int idCliente, int idProducto, bool sumar, out string Mensaje)
        {

            bool resultado = true;
            Mensaje = string.Empty;  //mensaje estara vacio por ahora

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("sp_OperacionCarrito", oconexion);
                    command.Parameters.AddWithValue("IdCliente", idCliente);
                    command.Parameters.AddWithValue("IdProducto", idProducto);
                    command.Parameters.AddWithValue("Sumar", sumar);

                    command.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
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
                Mensaje = ex.Message;  //mensaje de la excepción
            }
            return resultado;
        }



        //select count(*)from Carrito where IdCliente = 1

        public int CantidadEnCarrito(int idCliente)
        {
            int resultado = 0;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("select count(*)from Carrito where idCliente = @idCliente", oconexion);
                    command.Parameters.AddWithValue("@idCliente", idCliente);
                    command.CommandType = CommandType.Text;

                    oconexion.Open();
                    resultado = Convert.ToInt32(command.ExecuteScalar());

                }
            }
            catch (Exception ex)
            {
                resultado = 0;
            }

            return resultado;
        }



        public List<Carrito> ListarProducto(int idCliente)
        {
            List<Carrito> listaProducto = new List<Carrito>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    string query = "select * from fn_obtenerCarritoCliente(@idCliente)";


                    SqlCommand command = new SqlCommand(query, oconexion);
                    command.Parameters.AddWithValue("@idCliente", idCliente);
                    command.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader datareader = command.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaProducto.Add(
                                new Carrito()
                                {
                                    oProducto = new Producto()
                                    {
                                        IdProducto = Convert.ToInt32(datareader["IdProducto"]),
                                        Nombre = datareader["Nombre"].ToString(),
                                        Precio = Convert.ToDecimal(datareader["Precio"], new CultureInfo("es-PE")),   //esto para que los decimales tengan un formato agradable
                                        RutaImagen = datareader["RutaImagen"].ToString(),
                                        NombreImagen = datareader["NombreImagen"].ToString(),
                                        oMarca = new Marca()
                                        {
                                            Descripcion = datareader["DesMarca"].ToString()
                                        }
                                    },
                                    Cantidad = Convert.ToInt32(datareader["Cantidad"])
                                });
                        }
                    }
                }
            }
            catch
            {
                listaProducto = new List<Carrito>();
            }

            return listaProducto;

        }


        public bool EliminarCarrito(int idCliente, int idProducto)
        {
            bool resultado = true;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("sp_EliminarCarrito", oconexion);
                    command.Parameters.AddWithValue("IdCliente", idCliente);
                    command.Parameters.AddWithValue("IdProducto", idProducto);
                    command.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    command.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();


                    command.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(command.Parameters["Resultado"].Value);
                }
            }
            catch (Exception ex)
            {
                resultado = false;

            }
            return resultado;
        }

    }
}
