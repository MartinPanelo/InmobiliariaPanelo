using System.Data;
using MySql.Data.MySqlClient;

namespace InmobiliariaPanelo.Models
{
	public class RepositorioUsuario 
	{
	        readonly string	connectionString = "server=localhost;user=root;database=inmobiliaria;port=3306;password=";


		public int UsuarioAlta(Usuario e)
		{


			int res = -1;
			using (var connection = new MySqlConnection(connectionString)){
			
				string sql = @"INSERT INTO usuarios 
					(Nombre, Apellido, Email, Clave, Rol) 
					VALUES (@nombre, @apellido, @email, @clave, @rol);
					SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", e.Nombre);
					command.Parameters.AddWithValue("@apellido", e.Apellido);
					if (String.IsNullOrEmpty(e.Avatar))
						command.Parameters.AddWithValue("@avatar", DBNull.Value);
					else
						command.Parameters.AddWithValue("@avatar", e.Avatar);
					command.Parameters.AddWithValue("@email", e.Email);
					command.Parameters.AddWithValue("@clave", e.Clave);
					command.Parameters.AddWithValue("@rol", e.Rol);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					e.Id = res;
					connection.Close();
				}
			}   
			return res;
		    
        }


		public Usuario ObtenerPorEmail(string email)
		{
			Usuario? e = null;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT
					Id, Nombre, Apellido, Avatar, Email, Clave, Rol FROM usuarios
					WHERE Email=@email";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Usuario
						{
							Id = reader.GetInt32("Id"),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Avatar = reader.GetString("Avatar"),
							Email = reader.GetString("Email"),
							Clave = reader.GetString("Clave"),
							Rol = reader.GetInt32("Rol"),
						};
					}
					connection.Close();
				}
			}
			return e;
		}


		public int Modificacion(Usuario e)
		{
			int res = -1;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE usuarios 
					SET Nombre=@nombre, Apellido=@apellido, Avatar=@avatar, Email=@email, Clave=@clave, Rol=@rol
					WHERE Id = @id";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", e.Nombre);
					command.Parameters.AddWithValue("@apellido", e.Apellido);
					command.Parameters.AddWithValue("@avatar", e.Avatar);
					command.Parameters.AddWithValue("@email", e.Email);
					command.Parameters.AddWithValue("@clave", e.Clave);
					command.Parameters.AddWithValue("@rol", e.Rol);
					command.Parameters.AddWithValue("@id", e.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}



    }
}