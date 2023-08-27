using System.Data;
using MySql.Data.MySqlClient;

namespace InmobiliariaPanelo.Models
{
	public class RepositorioInmueble
	{

		readonly string	connectionString = "server=localhost;user=root;database=inmobiliaria;port=3306;password=";


		public List<Inmueble> InmuebleObtenerTodos()
        {

			List<Inmueble> res = new List<Inmueble>();
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT IdInmueble, PropietarioId, CantidadAmbientes, Uso, Direccion, Tipo, Latitud, Longitud, Precio, Disponible FROM inmuebles";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble inmueble = new Inmueble
						{
							IdInmueble = reader.GetInt32("IdInmueble"),
							PropietarioId = reader.GetInt32("PropietarioId"),
							CantidadAmbientes = reader.GetInt32("CantidadAmbientes"),
							Uso = reader.GetString("Uso"),
							Direccion = reader.GetString("Direccion"),
							Tipo = reader.GetString("Tipo"),
							Latitud = reader.GetDecimal("Latitud"),
							Longitud = reader.GetDecimal("Longitud"),
							Precio = reader.GetInt32("Precio"),
							Disponible = reader.GetBoolean("Disponible")
						};
						res.Add(inmueble);
					}
					connection.Close();
				}
			}

			return res;
		}


		public int PropietarioAlta(Propietario proietario){
		 	int res = -1;
		/*	using (var connection = new MySqlConnection(connectionString))
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
			}*/
			return res; // devuelve el id del propietario insertado 
		}

    }
}