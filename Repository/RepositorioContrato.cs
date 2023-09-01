using System.Data;
using MySql.Data.MySqlClient;

namespace InmobiliariaPanelo.Models
{
	public class RepositorioContrato{
        readonly string	connectionString = "server=localhost;user=root;database=inmobiliaria;port=3306;password=";

		private readonly RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
		private readonly RepositorioInquilino repositorioInquilino = new RepositorioInquilino();  

        public List<Contrato> ContratoObtenerTodos()
        {

			List<Contrato> res = new List<Contrato>();

			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT IdContrato, InquilinoId, InmuebleId, FechaDesde, FechaHasta
								FROM contratos;";
/* 									contratos AS T1
									INNER JOIN inquilinos AS T2 ON T1.InquilinoId = T2.IdInquilino
									INNER JOIN inmuebles AS T3 ON T1.InmuebleId = T3.IdInmueble;"; */
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato c = new Contrato
						{
							IdContrato = reader.GetInt32("IdContrato"),
							InquilinoId = reader.GetInt32("InquilinoId"),
							InmuebleId = reader.GetInt32("InmuebleId"),
							FechaDesde = reader.GetDateTime("FechaDesde"),
							FechaHasta = reader.GetDateTime("FechaHasta"),
							Inmueble = repositorioInmueble.InmuebleObtenerPorId(reader.GetInt32("InmuebleId")),
							Inquilino = repositorioInquilino.InquilinoObtenerPorId(reader.GetInt32("InquilinoId")),
						
							
						};
						res.Add(c);
					}
					connection.Close();
				}
			}

			return res;
		}



		public int ContratoAlta(Contrato contrato){
		 	int res = -1;

			using (var connection = new MySqlConnection(connectionString))
			{

				string sql = @"INSERT INTO contratos (InquilinoId, InmuebleId, FechaDesde, FechaHasta)
					VALUES (@InquilinoId, @InmuebleId, @FechaDesde, @FechaHasta);
					SELECT LAST_INSERT_ID();";//devuelve el id insertado (SCOPE_IDENTITY para sql)
				 using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@InquilinoId", contrato.InquilinoId);
					command.Parameters.AddWithValue("@InmuebleId", contrato.InmuebleId);
					command.Parameters.AddWithValue("@FechaDesde", contrato.FechaDesde);
					command.Parameters.AddWithValue("@FechaHasta", contrato.FechaHasta);

					
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					contrato.IdContrato = res;
					connection.Close();
				} 
			}
			return res; // devuelve el id del contrato insertado 
		}


		public int ContratoEliminar(int id)
        {
            int res = 0;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @$"DELETE FROM contratos WHERE {nameof(Contrato.IdContrato)} = @id";
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


		public Contrato ContratoObtenerPorId(int id){
			Contrato? c = null;
			using (var connection = new MySqlConnection(connectionString)){
				string sql = @"SELECT IdContrato, InquilinoId, InmuebleId, FechaDesde, FechaHasta FROM contratos WHERE IdContrato = @id";
				
				using (var command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					MySqlDataReader reader = command.ExecuteReader();
					
					if (reader.Read())
					{
						c = new Contrato
						{
							IdContrato = reader.GetInt32("IdContrato"),
							InquilinoId = reader.GetInt32("InquilinoId"),
							InmuebleId = reader.GetInt32("InmuebleId"),
							FechaDesde = reader.GetDateTime("FechaDesde"),
							FechaHasta = reader.GetDateTime("FechaHasta"),

						};
					}
					connection.Close();
				}			
			}
			return c!;
		}

    }
}