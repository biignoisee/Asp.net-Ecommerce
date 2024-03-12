using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Agregaremos los respesctivos complementos
using System.Configuration;

namespace CapaDatos
{
    //Manejaremos la cadena de conexión respectiva
    public class Conexion
    {
        public static string connection = ConfigurationManager.ConnectionStrings["cadena"].ToString();
    }
}
