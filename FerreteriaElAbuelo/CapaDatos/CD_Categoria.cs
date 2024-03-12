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
    public class CD_Categoria
    {
        //METODO LISTAR CATEGORIAS

        public List<Categoria> Listar()
        {
            List<Categoria> listaCategoria = new List<Categoria>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    string query = "SELECT IdCategoria, Descripcion, Activo FROM CATEGORIA";

                    SqlCommand command = new SqlCommand(query, oconexion);
                    command.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader datareader = command.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaCategoria.Add(
                                new Categoria()
                                {
                                    IdCategoria = Convert.ToInt32(datareader["IdCategoria"]),
                                    Descripcion = datareader["Descripcion"].ToString(),
                                    Activo = Convert.ToBoolean(datareader["Activo"])
                                });
                        }
                    }
                }
            }
            catch
            {
                listaCategoria = new List<Categoria>();
            }

            return listaCategoria;

        }



        //METODO CREAR CATEGORIA

        public int Registrar(Categoria obj, out string Mensaje)
        {
            // o era la categoria dentro como parametro instanciando el obj, o podia haber sido asi:
            // Categoria obj = new Categoria();

            int idAutogenerada = 0;
            Mensaje = string.Empty;  //mensaje estara vacio por ahora

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("sp_RegistrarCategoria", oconexion);
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



        //EDITAR CATEGORIA

        public bool Editar(Categoria obj, out string Mensaje)
        {

            bool resultado = false;   //POR AHORA FALSO, LUEGO CON LOS SCRIPT LO CAMBIA
            Mensaje = string.Empty;  //mensaje estara vacio por ahora

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("sp_EditarCategoria", oconexion);
                    command.Parameters.AddWithValue("IdCategoria", obj.IdCategoria);
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



        //ELIMINAR CATEGORIA

        public bool Eliminar(int idCategoria, out string Mensaje)
        {

            bool resultado = false;   //POR AHORA FALSO, LUEGO CON LOS SCRIPT LO CAMBIA
            Mensaje = string.Empty;  //mensaje estara vacio por ahora

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    SqlCommand command = new SqlCommand("sp_EliminarCategoria", oconexion);
                    command.Parameters.AddWithValue("IdCategoria", idCategoria);
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
