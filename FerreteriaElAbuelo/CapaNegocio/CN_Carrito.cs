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
    public class CN_Carrito
    {
        private CD_Carrito objCapaDato = new CD_Carrito();


        public bool ExisteCarrito(int idCliente, int idProducto)
        {
            return objCapaDato.ExisteCarrito(idCliente, idProducto);
        }

        public bool OperacionCarrito(int idCliente, int idProducto, bool sumar, out string Mensaje)
        {
            return objCapaDato.OperacionCarrito(idCliente, idProducto, sumar, out Mensaje);

        }

        public int CantidadEnCarrito(int idCliente)
        {
            return objCapaDato.CantidadEnCarrito(idCliente);
        }


        public List<Carrito> ListarProducto(int idCliente)
        {
            return objCapaDato.ListarProducto(idCliente);
        }

        public bool EliminarCarrito(int idCliente, int idProducto)
        {
            return objCapaDato.EliminarCarrito(idCliente, idProducto);
        }


    }
}
