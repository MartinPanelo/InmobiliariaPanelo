using System.Data;
using MySql.Data.MySqlClient;

namespace InmobiliariaPanelo.Models
{
	public class RepositorioInquilino{
        readonly string	connectionString = "server=localhost;user=root;database=inmobiliaria;port=3306;password=";

		public Inquilino InquilinoObtenerPorId(int id){
			Inquilino? i = null;
			using (var connection = new MySqlConnection(connectionString)){
				string sql = @"SELECT IdInquilino, Nombre, Apellido, Dni, Telefono, Email FROM inquilinos WHERE IdInquilino = @id";
				
				using (var command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					MySqlDataReader reader = command.ExecuteReader();
					
					if (reader.Read())
					{
						i = new Inquilino
						{
							IdInquilino = reader.GetInt32("IdInquilino"),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Dni = reader.GetString("Dni"),
							Telefono = reader.GetString("Telefono"),
							Email = reader.GetString("Email"),
						};
					}
					connection.Close();
				}			
			}
			return i!;
		}

        public int InquilinoEliminar(int id)
        {
            int res = 0;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @$"DELETE FROM inquilinos WHERE {nameof(Inquilino.IdInquilino)} = @id";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
        }

        public List<Inquilino> InquilinoObtenerTodos()
        {

			List<Inquilino> res = new List<Inquilino>();

			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT IdInquilino, Nombre, Apellido, Dni, Telefono, Email FROM inquilinos";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inquilino i = new Inquilino
						{
							IdInquilino = reader.GetInt32("IdInquilino"),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Dni = reader.GetString("Dni"),
							Telefono = reader.GetString("Telefono"),
							Email = reader.GetString("Email"),
						};
						res.Add(i);
					}
					connection.Close();
				}
			}

			return res;
		}

		public int InquilinoAlta(Inquilino inquilino){
			int res = -1;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO inquilinos 
					(Nombre, Apellido, Dni, Telefono, Email) 
					VALUES (@nombre, @apellido, @dni, @telefono, @email);
					SELECT LAST_INSERT_ID();";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", inquilino.Nombre);
					command.Parameters.AddWithValue("@apellido", inquilino.Apellido);
					command.Parameters.AddWithValue("@dni", inquilino.Dni);
					command.Parameters.AddWithValue("@telefono", inquilino.Telefono);
					command.Parameters.AddWithValue("@email", inquilino.Email);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					inquilino.IdInquilino = res;
					connection.Close();
				}
			}
			return res;
		}

		public int InquilinoEditar(Inquilino inquilino)
		{
			int res = -1;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE inquilinos
					SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Email=@email WHERE IdInquilino = @id";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", inquilino.Nombre);
					command.Parameters.AddWithValue("@apellido", inquilino.Apellido);
					command.Parameters.AddWithValue("@dni", inquilino.Dni);
					command.Parameters.AddWithValue("@telefono", inquilino.Telefono);
					command.Parameters.AddWithValue("@email", inquilino.Email);
					command.Parameters.AddWithValue("@id", inquilino.IdInquilino);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		}

    }

