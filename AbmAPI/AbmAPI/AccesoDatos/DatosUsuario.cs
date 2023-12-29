
using AbmAPI.Entidades;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography.Xml;

namespace AbmAPI.AccesoDatos
{
    public class DatosUsuario
    {
        private string _connectionString;

        public void ConfigurarConexion(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Usuarios> ObtenerUsuarios()
        {
            var usuarios = new List<Usuarios>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("Sp_Mostrar", connection)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var usuario = new Usuarios
                            {
                                idUsuario = Convert.ToInt32(reader["idUsuario"]),
                                nombre = reader["nombre"].ToString(),
                                apellido = reader["apellido"].ToString(),
                                usuario = reader["usuario"].ToString(),
                                password = reader["password"].ToString(),
                                // Mapea otras propiedades según sea necesario
                            };

                            usuarios.Add(usuario);
                        }
                    }
                }
            }

            return usuarios;
        }

        public async Task Agregar(Usuarios usu)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("Sp_Agregar", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", usu.nombre);
                    cmd.Parameters.AddWithValue("@apellido", usu.apellido);
                    cmd.Parameters.AddWithValue("@usuario", usu.usuario);
                    cmd.Parameters.AddWithValue("@password", usu.password);
                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Editar(Usuarios usu)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("Sp_Editar", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idUsuario", usu.idUsuario);
                    cmd.Parameters.AddWithValue("@nombre", usu.nombre);
                    cmd.Parameters.AddWithValue("@apellido", usu.apellido);
                    cmd.Parameters.AddWithValue("@usuario", usu.usuario);
                    cmd.Parameters.AddWithValue("@password", usu.password);
                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Eliminar(Usuarios usu)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("Sp_Eliminar", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idUsuario", usu.idUsuario);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public  bool Login(Login log)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("Sp_Login", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usuario", log.usuario);
                    cmd.Parameters.AddWithValue("@password", log.password);

                    con.Open();
                    int count =(int)cmd.ExecuteScalar();

                    return count > 0;
                }
            }
        }

    }
}
