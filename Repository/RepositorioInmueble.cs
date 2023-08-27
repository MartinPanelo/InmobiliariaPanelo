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
							Dirreccion = reader.GetString("Direccion"),
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
    }
}