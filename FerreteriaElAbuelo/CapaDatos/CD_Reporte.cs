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
    public class CD_Reporte
    {

        //METODO LISTAR MARCAS

        public List<Reporte> Ventas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<Reporte> listaReporte = new List<Reporte>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("sp_ReporteVentas", oconexion);
                    command.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@fechaFin", fechaFin);
                    command.Parameters.AddWithValue("@idTransaccion", idTransaccion);

                    command.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader datareader = command.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaReporte.Add(
                                new Reporte()
                                {
                                    FechaVenta = datareader["FechaVenta"].ToString(),
                                    Cliente = datareader["Cliente"].ToString(),
                                    Producto = datareader["Producto"].ToString(),
                                    Precio = Convert.ToDecimal(datareader["Precio"], new CultureInfo("es-PE")),
                                    Cantidad = Convert.ToInt32(datareader["Cantidad"]),
                                    Total = Convert.ToDecimal(datareader["Total"], new CultureInfo("es-PE")),
                                    IdTransaccion = datareader["IdTransaccion"].ToString(),
                                });
                        }
                    }
                }
            }
            catch
            {
                listaReporte = new List<Reporte>();
            }

            return listaReporte;

        }







        public Dashboard VerDashboard()
        {
            Dashboard objeto = new Dashboard();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("sp_ReporteDashboard", oconexion);
                    command.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader datareader = command.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            objeto=new Dashboard()
                            {
                                TotalCliente = Convert.ToInt32(datareader["TotalCliente"]),
                                TotalVenta = Convert.ToInt32(datareader["TotalVenta"]),
                                TotalProducto = Convert.ToInt32(datareader["TotalProducto"]),
                            };
                        }
                    }
                }
            }
                catch
                {
                objeto = new Dashboard();
            }
        return objeto;
        }
    }




}
