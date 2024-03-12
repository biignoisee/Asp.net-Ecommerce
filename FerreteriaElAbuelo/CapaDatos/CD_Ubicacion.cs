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
    public class CD_Ubicacion
    {
        //LISTAR DEPARTAMENTOS
        public List<Departamento> ObtenerDepartamento()
        {
            List<Departamento> listaDepartamentos = new List<Departamento>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    string query = "select DISTINCT * from DEPARTAMENTO";

                    SqlCommand command = new SqlCommand(query, oconexion);
                    command.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader datareader = command.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listaDepartamentos.Add(
                                new Departamento()
                                {
                                    IdDepartamento = datareader["IdDepartamento"].ToString(),
                                    Descripcion = datareader["Descripcion"].ToString(),

                                });
                        }
                    }
                }
            }
            catch
            {
                listaDepartamentos = new List<Departamento>();
            }

            return listaDepartamentos;

        }


        //LISTAR PROVINCIAS

        public List<Provincia> ObtenerProvincia(string idDepartamento)
        {
            List<Provincia> listarProvincias = new List<Provincia>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    string query = "select DISTINCT * from PROVINCIA where IdDepartamento = @idDepartamento";

                    SqlCommand command = new SqlCommand(query, oconexion);
                    command.Parameters.AddWithValue("@idDepartamento", idDepartamento);
                    command.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader datareader = command.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listarProvincias.Add(
                                new Provincia()
                                {
                                    IdProvincia = datareader["IdProvincia"].ToString(),
                                    Descripcion = datareader["Descripcion"].ToString(),

                                });
                        }
                    }
                }
            }
            catch
            {
                listarProvincias = new List<Provincia>();
            }

            return listarProvincias;

        }




        //LISTAR Distrito

        public List<Distrito> ObtenerDistritos(string idDepartamento, string idProvincia)
        {
            List<Distrito> listarDistritos = new List<Distrito>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.connection))
                {
                    string query = "select DISTINCT * from DISTRITO where IdProvincia = @idProvincia and IdDepartamento = @idDepartamento";

                    SqlCommand command = new SqlCommand(query, oconexion);
                    command.Parameters.AddWithValue("@idDepartamento", idDepartamento);
                    command.Parameters.AddWithValue("@idProvincia", idProvincia);
                    command.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader datareader = command.ExecuteReader())
                    {
                        while (datareader.Read())
                        {
                            listarDistritos.Add(
                                new Distrito()
                                {
                                    IdDistrito = datareader["IdDistrito"].ToString(),
                                    Descripcion = datareader["Descripcion"].ToString(),

                                });
                        }
                    }
                }
            }
            catch
            {
                listarDistritos = new List<Distrito>();
            }

            return listarDistritos;

        }

    }
}
