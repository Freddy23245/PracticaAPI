
using AbmAPI.Entidades;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AbmAPI.AccesoDatos
{
    public class DatosProducto
    {
        private string _connectionString;
        public void ConfigurarConexion(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Productos> ObtenerProductos()
        {
            var productos = new List<Productos>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("Sp_mostrarProductos", connection)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    using (var reader = command.ExecuteReader())
                    {
                        decimal num = 0;
                        while (reader.Read())
                        {
                            var producto = new Productos
                            {
                                idProducto = Convert.ToInt32(reader["idProducto"]),
                                nombre = reader["nombre"].ToString(),
                                precioCompra = Convert.ToDecimal(reader["precioCompra"].ToString()),
                                precioVenta = Convert.ToDecimal(reader["precioVenta"].ToString()),
                                stock = Convert.ToDecimal(reader["stock"].ToString()),
                               
                                // Mapea otras propiedades según sea necesario
                            };

                            productos.Add(producto);
                        }
                    }
                }
            }
            return productos;
        }


    }
}
