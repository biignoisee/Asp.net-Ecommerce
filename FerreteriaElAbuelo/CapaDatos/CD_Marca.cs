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


namespace CapaDatos
{
    public class CD_Marca
    {
        //METODO LISTAR MARCAS

        public List<Marca> Listar()
        {
            List<Marca> listaMarca = new List<Marca>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    string query = "SELECT IdMarca, Descripcion, Activo FROM MARCA";

                    SqlCommand command = new SqlCommand(query, oconexion);
                    command.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader datareader = command.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaMarca.Add(
                                new Marca()
                                {
                                    IdMarca = Convert.ToInt32(datareader["IdMarca"]),
                                    Descripcion = datareader["Descripcion"].ToString(),
                                    Activo = Convert.ToBoolean(datareader["Activo"])
                                });
                        }
                    }
                }
            }
            catch
            {
                listaMarca = new List<Marca>();
            }

            return listaMarca;

        }

        //METODO CREAR MARCA

        public int Registrar(Marca obj, out string Mensaje)
        {

            int idAutogenerada = 0;
            Mensaje = string.Empty;  //mensaje estara vacio por ahora

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("sp_RegistrarMarca", oconexion);
                    command.Parameters.AddWithValue("Descripcion", obj.Descripcion);
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


        //EDITAR MARCA

        public bool Editar(Marca obj, out string Mensaje)
        {

            bool resultado = false;   //POR AHORA FALSO, LUEGO CON LOS SCRIPT LO CAMBIA
            Mensaje = string.Empty;  //mensaje estara vacio por ahora

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("sp_EditarMarca", oconexion);
                    command.Parameters.AddWithValue("IdMarca", obj.IdMarca);
                    command.Parameters.AddWithValue("Descripcion", obj.Descripcion);
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



        //ELIMINAR MARCA

        public bool Eliminar(int idMarca, out string Mensaje)
        {

            bool resultado = false;   //POR AHORA FALSO, LUEGO CON LOS SCRIPT LO CAMBIA
            Mensaje = string.Empty;  //mensaje estara vacio por ahora

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("sp_EliminarMarca", oconexion);
                    command.Parameters.AddWithValue("IdMarca", idMarca);
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



        //LISTAR PARA LA VIEW CLIENTE

        public List<Marca> ListarMarcaPorCategoria(int idCategoria)
        {
            List<Marca> listaMarca = new List<Marca>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("select DISTINCT m.IdMarca, m.Descripcion from Producto p");
                    sb.AppendLine("inner join Categoria c on c.IdCategoria = p.IdCategoria");
                    sb.AppendLine("inner Join Marca m on m.IdMarca = p.IdMarca and m.Activo = 1");
                    sb.AppendLine("where c.IdCategoria = iif(@idCategoria = 0, c.IdCategoria, @idCategoria)");


                    SqlCommand command = new SqlCommand(sb.ToString(), oconexion);
                    command.Parameters.AddWithValue("@idCategoria",idCategoria);
                    command.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader datareader = command.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaMarca.Add(
                                new Marca()
                                {
                                    IdMarca = Convert.ToInt32(datareader["IdMarca"]),
                                    Descripcion = datareader["Descripcion"].ToString(),
                                });
                        }
                    }
                }
            }
            catch
            {
                listaMarca = new List<Marca>();
            }

            return listaMarca;

        }






    }
}
