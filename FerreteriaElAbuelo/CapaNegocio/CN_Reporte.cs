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
    public class CN_Reporte
    {
        private CD_Reporte objCapaDato = new CD_Reporte();


        //CAPA NEGOCIO MANEJO DE DATO

        public List<Reporte> Ventas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            return objCapaDato.Ventas(fechaInicio, fechaFin, idTransaccion);
        }







        //Capa en donde aplicamos todas las reglas del negocio
        public Dashboard VerDashboard()
        {
            return objCapaDato.VerDashboard();
        }


    }
}
