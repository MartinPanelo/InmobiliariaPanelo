using System.Data;
using MySql.Data.MySqlClient;

namespace InmobiliariaPanelo.Models
{
	public class RepositorioInmueble
	{

		readonly string	connectionString = "server=localhost;user=root;database=inmobiliaria;port=3306;password=";

		public Dictionary<string, int> usos(){
			//List<string> res = new List<string>();

			Dictionary<string, int> res = new Dictionary<string, int>();


			using (var connection = new MySqlConnection(connectionString)){
				
				string sql = @"SELECT IdUsoInmueble, Uso FROM usosInmuebles";
				using (var command = new MySqlCommand(sql, connection))
				{
					connection.Open();
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read()){
							res.Add(reader.GetString("Uso"),reader.GetInt32("IdUsoInmueble"));							
						}
					}
				}
			}
			return res;
		}
		public Dictionary<string, int>  tipos(){
			//List<string> res = new List<string>();

			Dictionary<string, int> res = new Dictionary<string, int>();


			using (var connection = new MySqlConnection(connectionString)){
				
				string sql = @"SELECT IdTipoInmueble, Tipo FROM tiposInmuebles";
				using (var command = new MySqlCommand(sql, connection))
				{
					connection.Open();
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read()){
							res.Add(reader.GetString("Tipo"), reader.GetInt32("IdTipoInmueble"));							
						}
					}
				}
			}
			return res;
		}


		public List<Inmueble> InmuebleObtenerConFiltro(Inmueble datos,String FechaDesde, String FechaHasta){

			List<Inmueble> res = new List<Inmueble>();


			using (var connection = new MySqlConnection(connectionString))
			{
				
				 string sql = @"SELECT i.`IdInmueble`, i.`PropietarioId`, i.`CantidadAmbientes`, i.`Uso`, i.`Direccion`, i.`Tipo`, i.`Latitud`, i.`Longitud`, i.`Precio`, i.`Disponible`,
				 				T4.Nombre, T4.Apellido
								FROM `inmuebles` AS i
								INNER JOIN contratos AS c ON i.`IdInmueble` = c.InmuebleId 
								INNER JOIN propietarios AS T4 ON i.PropietarioId = T4.IdPropietario
								WHERE i.`CantidadAmbientes` = @CantidadAmbientes AND i.`Uso` = @Uso AND i.`Tipo` = @Tipo AND i.`Precio` BETWEEN (75000 - 5000) AND (75000 + 5000) AND i.`Disponible` = 1
								AND (c.FechaDesde > @FechaHasta OR c.FechaHasta < @FechaDesde)";

				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@CantidadAmbientes", datos.CantidadAmbientes);
					command.Parameters.AddWithValue("@Uso", datos.Uso);
					command.Parameters.AddWithValue("@Tipo", datos.Tipo);
					command.Parameters.AddWithValue("@FechaDesde", FechaDesde);
					command.Parameters.AddWithValue("@FechaHasta", FechaHasta);

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
							Precio = reader.GetDecimal("Precio"),
							Disponible = reader.GetBoolean("Disponible"),
							Propietario = new Propietario{
								IdPropietario = reader.GetInt32("PropietarioId"),
								Nombre = reader.GetString("Nombre"),
								Apellido = reader.GetString("Apellido")
							}
						};
						res.Add(inmueble);
					}
					connection.Close();
				}
			}

			return res;
		}


		public List<Inmueble> InmuebleObtenerTodos()
        {

			List<Inmueble> res = new List<Inmueble>();
			using (var connection = new MySqlConnection(connectionString))
			{
				/* string sql = @"SELECT IdInmueble, PropietarioId, CantidadAmbientes, Uso, Direccion, Tipo, Latitud, Longitud, Precio, Disponible,
				 P.Nombre, P.Apellido FROM inmuebles I INNER JOIN propietarios P ON I.PropietarioId = P.IdPropietario"; */
				 string sql = @"SELECT T1.IdInmueble, T1.PropietarioId, T1.CantidadAmbientes, T3.Uso, T1.Direccion, T2.Tipo, 
				 T1.Latitud, T1.Longitud, T1.Precio, T1.Disponible, T4.Nombre, T4.Apellido 
				 FROM inmuebles AS T1 
				 INNER JOIN tiposInmuebles AS T2 ON T1.Tipo = T2.IdTipoInmueble 
				 INNER JOIN usosInmuebles AS T3 ON T1.Uso = T3.IdUsoInmueble 
				 INNER JOIN propietarios AS T4 ON T1.PropietarioId = T4.IdPropietario;";

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
							Precio = reader.GetDecimal("Precio"),
							Disponible = reader.GetBoolean("Disponible"),
							Propietario = new Propietario{
								IdPropietario = reader.GetInt32("PropietarioId"),
								Nombre = reader.GetString("Nombre"),
								Apellido = reader.GetString("Apellido")
							}
						};
						res.Add(inmueble);
					}
					connection.Close();
				}
			}

			return res;
		}


		public int InmuebleAlta(Inmueble inmueble){
		 	int res = -1;

			using (var connection = new MySqlConnection(connectionString))
			{

				string sql = @"INSERT INTO inmuebles 
					(PropietarioId, CantidadAmbientes, Uso, Direccion, Tipo, Latitud, Longitud, Precio, Disponible) 
					VALUES (@PropietarioId, @CantidadAmbientes, @Uso, @Direccion, @Tipo, @Latitud, @Longitud, @Precio, @Disponible);
					SELECT LAST_INSERT_ID();";//devuelve el id insertado (SCOPE_IDENTITY para sql)
				 using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@PropietarioId", inmueble.PropietarioId);
					command.Parameters.AddWithValue("@CantidadAmbientes", inmueble.CantidadAmbientes);
					command.Parameters.AddWithValue("@Uso", inmueble.Uso);
					command.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
					command.Parameters.AddWithValue("@Tipo", inmueble.Tipo);
					command.Parameters.AddWithValue("@Latitud", inmueble.Latitud);
					command.Parameters.AddWithValue("@Longitud", inmueble.Longitud);
					command.Parameters.AddWithValue("@Precio", inmueble.Precio);
					command.Parameters.AddWithValue("@Disponible", inmueble.Disponible);
					
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					inmueble.IdInmueble = res;
					connection.Close();
				} 
			}
			return res; // devuelve el id del inmueble insertado 
		}

		public Inmueble InmuebleObtenerPorId(int id){
			Inmueble? i = null;
			using (var connection = new MySqlConnection(connectionString)){
				string sql = @"SELECT T1.IdInmueble, T1.PropietarioId, T1.CantidadAmbientes, T3.Uso, T1.Direccion, T2.Tipo, 
				 T1.Latitud, T1.Longitud, T1.Precio, T1.Disponible, 
				 T4.Nombre, T4.Apellido, T4.Dni, T4.Telefono, T4.Email
				 FROM inmuebles AS T1 
				 INNER JOIN tiposInmuebles AS T2 ON T1.Tipo = T2.IdTipoInmueble 
				 INNER JOIN usosInmuebles AS T3 ON T1.Uso = T3.IdUsoInmueble 
				 INNER JOIN propietarios AS T4 ON T1.PropietarioId = T4.IdPropietario AND T1.IdInmueble = @id";
				
				using (var command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					MySqlDataReader reader = command.ExecuteReader();
					
					if (reader.Read())
					{
						i = new Inmueble
						{
							IdInmueble = reader.GetInt32("IdInmueble"),
							PropietarioId = reader.GetInt32("PropietarioId"),
							CantidadAmbientes = reader.GetInt32("CantidadAmbientes"),
							Uso = reader.GetString("Uso"),
							Direccion = reader.GetString("Direccion"),
							Tipo = reader.GetString("Tipo"),
							Latitud = reader.GetDecimal("Latitud"),
							Longitud = reader.GetDecimal("Longitud"),
							Precio = reader.GetDecimal("Precio"),
							Disponible = reader.GetBoolean("Disponible"),
							Propietario = new Propietario{
								IdPropietario = reader.GetInt32("PropietarioId"),
								Nombre = reader.GetString("Nombre"),
								Apellido = reader.GetString("Apellido"),
								Dni = reader.GetString("Dni"),
								Telefono = reader.GetString("Telefono"),
								Email = reader.GetString("Email")
							}
							
						};
					}
					connection.Close();
				}			
			}
			return i!;
		}

		public int InmuebleEliminar(int id)
        {
            int res = 0;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @$"DELETE FROM inmuebles WHERE {nameof(Inmueble.IdInmueble)} = @id";
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


		public int InmuebleEditar(Inmueble inmueble)
		{
			int res = -1;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE inmuebles
					SET PropietarioId=@PropietarioId, CantidadAmbientes=@CantidadAmbientes, Uso=@Uso, Direccion=@Direccion, 
						Tipo=@Tipo, Latitud=@Latitud, Longitud=@Longitud, Precio=@Precio, Disponible=@Disponible WHERE IdInmueble=@id";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@PropietarioId", inmueble.PropietarioId);
					command.Parameters.AddWithValue("@CantidadAmbientes", inmueble.CantidadAmbientes);
					command.Parameters.AddWithValue("@Uso", inmueble.Uso);
					command.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
					command.Parameters.AddWithValue("@Tipo", inmueble.Tipo);
					command.Parameters.AddWithValue("@Latitud", inmueble.Latitud);
					command.Parameters.AddWithValue("@Longitud", inmueble.Longitud);
					command.Parameters.AddWithValue("@Precio", inmueble.Precio);
					command.Parameters.AddWithValue("@Disponible", inmueble.Disponible);
					command.Parameters.AddWithValue("@id", inmueble.IdInmueble);

					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}













    }
}