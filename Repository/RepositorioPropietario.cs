using System.Data;
using MySql.Data.MySqlClient;

namespace InmobiliariaPanelo.Models
{
	public class RepositorioPropietario : IPropietarioRepositorio
	{
        //protected readonly string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=inmobiliaria;Trusted_Connection=True;MultipleActiveResultSets=true";
        readonly string	connectionString = "server=localhost;user=root;database=inmobiliaria;port=3306;password=";



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
						(
							reader.GetInt32(nameof(Propietario.IdPropietario)),
							reader.GetString("Nombre"),
							reader.GetString("Apellido"),
							reader.GetString("Dni"),
							reader.GetString("Telefono"),
							reader.GetString("Email"),
							reader.GetString("Clave")
                        );
						res.Add(p);
					}
					connection.Close();
				}
			}

			return res;
		}

    }

}