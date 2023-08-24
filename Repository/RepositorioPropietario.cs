using System.Data;
using MySql.Data.MySqlClient;

namespace InmobiliariaPanelo.Models
{
	public class RepositorioPropietario// : IPropietarioRepositorio
	{
        //protected readonly string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=inmobiliaria;Trusted_Connection=True;MultipleActiveResultSets=true";
        readonly string	connectionString = "server=localhost;user=root;database=inmobiliaria;port=3306;password=";

		public Propietario PropietarioObtenerPorId(int id){
			Propietario? p = null;
			using (var connection = new MySqlConnection(connectionString)){
				string sql = @"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email FROM propietarios WHERE IdPropietario = @id";
				
				using (var command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					MySqlDataReader reader = command.ExecuteReader();
					
					if (reader.Read())
					{
						p = new Propietario
						{
							IdPropietario = reader.GetInt32("IdPropietario"),
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
			return p!;
		}

        public int PropietarioEliminar(int id)
        {
            int res = 0;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @$"DELETE FROM propietarios WHERE {nameof(Propietario.IdPropietario)} = @id";
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

        public List<Propietario> PropietarioObtenerTodos()
        {

			List<Propietario> res = new List<Propietario>();
		//	Propietario p = new Propietario(1,"martin","Panelo","123","26666","vU5X6@example.com","123");

			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email FROM propietarios";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Propietario p = new Propietario
						{
							IdPropietario = reader.GetInt32("IdPropietario"),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Dni = reader.GetString("Dni"),
							Telefono = reader.GetString("Telefono"),
							Email = reader.GetString("Email"),
						};
						res.Add(p);
					}
					connection.Close();
				}
			}

			return res;
		}

		public int PropietarioAlta(Propietario propietario){
			int res = -1;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO propietarios 
					(Nombre, Apellido, Dni, Telefono, Email) 
					VALUES (@nombre, @apellido, @dni, @telefono, @email);
					SELECT LAST_INSERT_ID();";//devuelve el id insertado (SCOPE_IDENTITY para sql)
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", propietario.Nombre);
					command.Parameters.AddWithValue("@apellido", propietario.Apellido);
					command.Parameters.AddWithValue("@dni", propietario.Dni);
					command.Parameters.AddWithValue("@telefono", propietario.Telefono);
					command.Parameters.AddWithValue("@email", propietario.Email);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					propietario.IdPropietario = res;
					connection.Close();
				}
			}
			return res; // devuelve el id del propietario insertado
		}

		public int PropietarioEditar(Propietario propietario)
		{
			int res = -1;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE propietarios
					SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Email=@email WHERE IdPropietario = @id";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", propietario.Nombre);
					command.Parameters.AddWithValue("@apellido", propietario.Apellido);
					command.Parameters.AddWithValue("@dni", propietario.Dni);
					command.Parameters.AddWithValue("@telefono", propietario.Telefono);
					command.Parameters.AddWithValue("@email", propietario.Email);
					command.Parameters.AddWithValue("@id", propietario.IdPropietario);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		}

    }

