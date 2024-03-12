using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Añadimos la referencia
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Producto
    {

        //METODO LISTAR Producto

        public List<Producto> Listar()
        {
            List<Producto> listaProducto = new List<Producto>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("SELECT P.IdProducto, p.Nombre, p.Descripcion,");
                    sb.AppendLine("m.IdMarca, m.Descripcion[Marcas],");
                    sb.AppendLine("c.IdCategoria, c.Descripcion[Categorias],");
                    sb.AppendLine("p.Precio, p.Stock, p.RutaImagen, p.NombreImagen, p.Activo");
                    sb.AppendLine("FROM PRODUCTO p");
                    sb.AppendLine("inner join Marca m on m.IdMarca = P.IdMarca");
                    sb.AppendLine("inner join CATEGORIA C on C.IdCategoria = P.IdCategoria");


                    SqlCommand command = new SqlCommand(sb.ToString(), oconexion);
                    command.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader datareader = command.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaProducto.Add(
                                new Producto()
                                {
                                    IdProducto = Convert.ToInt32(datareader["IdProducto"]),
                                    Nombre = datareader["Nombre"].ToString(),
                                    Descripcion = datareader["Descripcion"].ToString(),
                                    oMarca = new Marca() { IdMarca = Convert.ToInt32(datareader["IdMarca"]), Descripcion = datareader["Marcas"].ToString() },
                                    oCategoria = new Categoria() { IdCategoria = Convert.ToInt32(datareader["IdCategoria"]), Descripcion = datareader["Categorias"].ToString() },
                                    Precio = Convert.ToDecimal(datareader["Precio"], new CultureInfo("es-PE")),   //esto para que los decimales tengan un formato agradable
                                    Stock = Convert.ToInt32(datareader["Stock"]),
                                    RutaImagen = datareader["RutaImagen"].ToString(),
                                    NombreImagen = datareader["NombreImagen"].ToString(),
                                    Activo = Convert.ToBoolean(datareader["Activo"])
                                });
                        }
                    }
                }
            }
            catch
            {
                listaProducto = new List<Producto>();
            }

            return listaProducto;

        }




        //METODO CREAR Producto

        public int Registrar(Producto obj, out string Mensaje)
        {

            int idAutogenerada = 0;
            Mensaje = string.Empty;  //mensaje estara vacio por ahora

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("sp_RegistrarProducto", oconexion);
                    command.Parameters.AddWithValue("Nombre", obj.Nombre);
                    command.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    command.Parameters.AddWithValue("IdMarca", obj.oMarca.IdMarca);
                    command.Parameters.AddWithValue("IdCategoria", obj.oCategoria.IdCategoria);
                    command.Parameters.AddWithValue("Precio", obj.Precio);
                    command.Parameters.AddWithValue("Stock", obj.Stock);
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
            catch (Exception ex)
            {
                idAutogenerada = 0;
                Mensaje = ex.Message;  //mensaje de la excepción
            }
            return idAutogenerada;
        }


        //EDITAR Producto

        public bool Editar(Producto obj, out string Mensaje)
        {

            bool resultado = false;   //POR AHORA FALSO, LUEGO CON LOS SCRIPT LO CAMBIA
            Mensaje = string.Empty;  //mensaje estara vacio por ahora

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("sp_EditarProducto", oconexion);
                    command.Parameters.AddWithValue("IdProducto", obj.IdProducto);
                    command.Parameters.AddWithValue("Nombre", obj.Nombre);
                    command.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    command.Parameters.AddWithValue("IdMarca", obj.oMarca.IdMarca);
                    command.Parameters.AddWithValue("IdCategoria", obj.oCategoria.IdCategoria);
                    command.Parameters.AddWithValue("Precio", obj.Precio);
                    command.Parameters.AddWithValue("Stock", obj.Stock);
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



        //METODO QUE PERMITA SUBIR IMAGENES

        public bool GuardarDatosImagenes(Producto obj, out string Mensaje)
        {
            bool resultado = false;   //POR AHORA FALSO, LUEGO CON LOS SCRIPT LO CAMBIA
            Mensaje = string.Empty;  //mensaje estara vacio por ahora

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {

                    string query = "update producto set RutaImagen = @rutaimagen, NombreImagen = @nombreimagen where IdProducto = @idproducto";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@rutaimagen", obj.RutaImagen);
                    cmd.Parameters.AddWithValue("@nombreimagen", obj.NombreImagen);
                    cmd.Parameters.AddWithValue("@idproducto", obj.IdProducto);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar imagen";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;  //mensaje de la excepción
            }
            return resultado;
        }


        //ELIMINAR PRODUCTO

        public bool Eliminar(int idProducto, out string Mensaje)
        {

            bool resultado = false;   //POR AHORA FALSO, LUEGO CON LOS SCRIPT LO CAMBIA
            Mensaje = string.Empty;  //mensaje estara vacio por ahora

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("sp_EliminarProducto", oconexion);
                    command.Parameters.AddWithValue("IdProducto", idProducto);
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

    }
}
