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
    public class CN_Marca
    {

        private CD_Marca objCapaDato = new CD_Marca();

        //Capa en donde aplicamos todas las reglas del negocio
        public List<Marca> Listar()
        {
            return objCapaDato.Listar();
        }


        public int Registrar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción no puede ser vacio";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Registrar(obj, out Mensaje);

            }
            else
            {
                return 0;
            }

        }


        public bool Editar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción no puede ser vacio";
            }



            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }


        public List<Marca> ListarMarcaPorCategoria(int idCategoria)
        {
            return objCapaDato.ListarMarcaPorCategoria(idCategoria);
        }

    }
}
