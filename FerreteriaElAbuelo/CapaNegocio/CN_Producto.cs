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
    public class CN_Producto
    {
        private CD_Producto objCapaDato = new CD_Producto();


        //Capa en donde aplicamos todas las reglas del negocio
        public List<Producto> Listar()
        {
            return objCapaDato.Listar();
        }


        public int Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción no puede ser vacio";
            }
            else if (obj.oMarca.IdMarca == 0)
            {
                Mensaje = "La Marca debe ser llenada";
            }
            else if (obj.oCategoria.IdCategoria == 0)
            {
                Mensaje = "La Categoria debe ser llenada";
            }
            else if (obj.Precio == 0)
            {
                Mensaje = "El precio no puede ser vacio";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "El Stock no puede ser vacio";
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


        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción no puede ser vacio";
            }
            else if (obj.oMarca.IdMarca == 0)
            {
                Mensaje = "La Marca debe ser llenada";
            }
            else if (obj.oCategoria.IdCategoria == 0)
            {
                Mensaje = "La Categoria debe ser llenada";
            }
            else if (obj.Precio == 0)
            {
                Mensaje = "El precio no puede ser vacio";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "El Stock no puede ser vacio";
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


        public bool GuardarDatosImagenes(Producto obj, out string Mensaje)
        {
            return objCapaDato.GuardarDatosImagenes(obj, out Mensaje);
        }





        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }

        

    }
}
