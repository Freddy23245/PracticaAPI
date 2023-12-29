using AbmAPI.Entidades;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AbmAPI.AccesoDatos
{
    public class DatosCliente
    {
        private string _connectionString;

        public void ConfigurarConexion(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Clientes> ObtenerCliente()
        {
            var clientes = new List<Clientes>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("Sp_MostrarClientes", connection)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cliente = new Clientes
                            {
                                idCliente = Convert.ToInt32(reader["idCliente"]),
                                nombre = reader["nombre"].ToString(),
                                apellido = reader["apellido"].ToString(),
                                dni = reader["dni"].ToString(),
                                telefono = reader["telefono"].ToString(),
                                direccion = reader["direccion"].ToString()
                                // Mapea otras propiedades según sea necesario
                            };

                            clientes.Add(cliente);
                        }
                    }
                }
            }
            return clientes;
        }

        public async Task AgregarCliente(Clientes cliente)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("Sp_AgregarClientes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", cliente.nombre);
                    cmd.Parameters.AddWithValue("@apellido", cliente.apellido);
                    cmd.Parameters.AddWithValue("@dni", cliente.dni); 
                    cmd.Parameters.AddWithValue("@telefono", cliente.telefono);
                    cmd.Parameters.AddWithValue("@direccion", cliente.direccion);
                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    //comentario
                }
            }
        }
        public async Task EditarCliente(Clientes cliente)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("Sp_editarCliente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCliente", cliente.idCliente);
                    cmd.Parameters.AddWithValue("@nombre", cliente.nombre);
                    cmd.Parameters.AddWithValue("@apellido", cliente.apellido);
                    cmd.Parameters.AddWithValue("@dni", cliente.dni);
                    cmd.Parameters.AddWithValue("@telefono", cliente.telefono);
                    cmd.Parameters.AddWithValue("@direccion", cliente.direccion);
                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task EliminarCliente(Clientes cliente)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("Sp_eliminarCliente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCliente", cliente.idCliente);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

    }
}
