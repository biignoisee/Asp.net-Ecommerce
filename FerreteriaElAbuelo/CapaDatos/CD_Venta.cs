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
    public class CD_Venta
    {
        public bool Registrar(Venta obj, DataTable DetalleVenta,out string Mensaje)
        {
            bool respuesta = false;   //POR AHORA FALSO, LUEGO CON LOS SCRIPT LO CAMBIA
            Mensaje = string.Empty;  //mensaje estara vacio por ahora

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("usp_RegistrarVenta", oconexion);
                    command.Parameters.AddWithValue("IdCliente", obj.IdCliente);
                    command.Parameters.AddWithValue("TotalProducto", obj.TotalProducto);
                    command.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
                    command.Parameters.AddWithValue("Contacto", obj.Contacto);
                    command.Parameters.AddWithValue("IdDistrito", obj.IdDistrito);
                    command.Parameters.AddWithValue("Telefono", obj.Telefono);
                    command.Parameters.AddWithValue("Direccion", obj.Direccion);
                    command.Parameters.AddWithValue("IdTransaccion", obj.IdTransaccion);
                    command.Parameters.AddWithValue("DetalleVenta", DetalleVenta);
                    command.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    command.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    command.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    command.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(command.Parameters["Resultado"].Value);
                    Mensaje = command.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }


            return respuesta;
        }


        public List<DetalleVenta> ListarCompras(int idCliente)
        {
            List<DetalleVenta> listarCompras = new List<DetalleVenta>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    string query = "select * from fn_ListarCompra(@idCliente)";


                    SqlCommand command = new SqlCommand(query, oconexion);
                    command.Parameters.AddWithValue("@idCliente", idCliente);
                    command.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader datareader = command.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listarCompras.Add(
                                new DetalleVenta()
                                {
                                    oProducto = new Producto()
                                    {
                                        Nombre = datareader["Nombre"].ToString(),
                                        Precio = Convert.ToDecimal(datareader["Precio"], new CultureInfo("es-PE")),   //esto para que los decimales tengan un formato agradable
                                        RutaImagen = datareader["RutaImagen"].ToString(),
                                        NombreImagen = datareader["NombreImagen"].ToString(),
                                    },
                                    Cantidad = Convert.ToInt32(datareader["Cantidad"]),
                                    Total = Convert.ToDecimal(datareader["Total"], new CultureInfo("es-PE")),
                                    IdTransaccion = datareader["IdTransaccion"].ToString()
                                });
                        }
                    }
                }
            }
            catch
            {
                listarCompras = new List<DetalleVenta>();
            }

            return listarCompras;

        }


    }
}
