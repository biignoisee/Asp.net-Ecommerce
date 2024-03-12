using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{

    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Marca oMarca { get; set; }
        public Categoria oCategoria { get; set; }
        public decimal Precio { get; set; }

        //Para poder enviar a c# el precio en string y este que lo convierta a el valor local que sería en soles
        public string PrecioTexto { get; set; } 

        public int Stock { get; set; }
        public string RutaImagen { get; set; }
        public string NombreImagen { get; set; }
        public bool Activo { get; set; }

        //Para trabajar con las imagenes que llegan a ser enviadas 
        public string Base64 { get; set; }  


        //Tipo de extension de la imagen
        public string Extension { get; set; }

    }
}
