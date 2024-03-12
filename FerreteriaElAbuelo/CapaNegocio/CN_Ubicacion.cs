using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Agregamos las respectivas referencias
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Ubicacion
    {
        private CD_Ubicacion objCapaDato = new CD_Ubicacion();


        public List<Departamento> ObtenerDepartamento()
        {
            return objCapaDato.ObtenerDepartamento();
        }

        public List<Provincia> ObtenerProvincia(string idDepartamento)
        {
            return objCapaDato.ObtenerProvincia(idDepartamento);
        }

        public List<Distrito> ObtenerDistritos(string idDepartamento, string idProvincia)
        {
            return objCapaDato.ObtenerDistritos(idDepartamento, idProvincia);
        }
    }
}
