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
				string sql = @"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email, Clave FROM propietarios WHERE IdPropietario = @id";
				
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
							Clave = reader.GetString("Clave")
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
				string sql = @"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email, Clave FROM propietarios";
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
							Clave = reader.GetString("Clave")
						};
						res.Add(p);
					}
					connection.Close();
				}
			}

			return res;
		}

    }

}