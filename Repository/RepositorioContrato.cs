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
				string sql = @"SELECT IdContrato, InquilinoId, InmuebleId, FechaDesde, FechaHasta, Monto, Vigente
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
							Monto = reader.GetDecimal("Monto"),
							Vigente = reader.GetBoolean("Vigente"),
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


		public bool ConsultaDeFecha(Contrato contrato){

			bool res = false;
		

			
			using (var connection = new MySqlConnection(connectionString))
			{

				

			//	string sql = @"SELECT * FROM `contratos` WHERE (FechaDesde < @FechaHasta OR FechaHasta > @FechaDesde)
			//	 AND InmuebleId = @Id AND Vigente = 1 AND InquilinoId != @InquilinoId";// busco todos los fechas que se superpongan
				string sql = @"SELECT CASE WHEN EXISTS ( 
					SELECT 1 
					FROM contratos 
					WHERE (FechaDesde < @FechaHasta AND FechaHasta > @FechaDesde)
						AND InmuebleId = @Id 
						AND Vigente = 1 
						AND InquilinoId != @InquilinoId
					)	
					THEN 'Sí' ELSE 'No' END AS Resultado";
				 using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@Id", contrato.InmuebleId);
					command.Parameters.AddWithValue("@InquilinoId", contrato.InquilinoId);
					command.Parameters.AddWithValue("@FechaDesde", contrato.FechaDesde);
					command.Parameters.AddWithValue("@FechaHasta", contrato.FechaHasta);
					
					connection.Open();
					MySqlDataReader reader = command.ExecuteReader();
					
					if (reader.Read()){
						//String a = reader.GetString("Resultado");
						
						if(reader.GetString("Resultado").Equals("Sí")){
							res = false;
						}else{
							res =  true;
						}


					connection.Close();
					}	
				} 
			}
			return res;
		
		}


		public int ContratoAlta(Contrato contrato){
		 	int res = -1;

			using (var connection = new MySqlConnection(connectionString))
			{

				string sql = @"INSERT INTO contratos (InquilinoId, InmuebleId, FechaDesde, FechaHasta, Monto, Vigente)
					VALUES (@InquilinoId, @InmuebleId, @FechaDesde, @FechaHasta, @Monto, @Vigente);
					SELECT LAST_INSERT_ID();";//devuelve el id insertado (SCOPE_IDENTITY para sql)


				contrato.Monto = repositorioInmueble.InmuebleObtenerPorId(contrato.InmuebleId).Precio;

				int mesesDeDiferencia = ((contrato.FechaHasta.Year - contrato.FechaDesde.Year) * 12) + contrato.FechaHasta.Month - contrato.FechaDesde.Month;

				int diasDeDiferencia = (int)(contrato.FechaHasta - contrato.FechaDesde).TotalDays;

				diasDeDiferencia -= mesesDeDiferencia * 30;

				decimal montoTotal = contrato.Monto * mesesDeDiferencia + (contrato.Monto/30 * diasDeDiferencia);


			/* 	TimeSpan DiasDeAlquiler = contrato.FechaHasta - contrato.FechaDesde;
				int dias = (int)DiasDeAlquiler.TotalDays;
				contrato.Monto = repositorioInmueble.InmuebleObtenerPorId(contrato.InmuebleId).Precio/30 * dias;
 */

				 using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@InquilinoId", contrato.InquilinoId);
					command.Parameters.AddWithValue("@InmuebleId", contrato.InmuebleId);
					command.Parameters.AddWithValue("@FechaDesde", contrato.FechaDesde);
					command.Parameters.AddWithValue("@FechaHasta", contrato.FechaHasta);
					command.Parameters.AddWithValue("@Vigente", true);
					command.Parameters.AddWithValue("@Monto", montoTotal);

					
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
				//string sql = @"SELECT IdContrato, InquilinoId, InmuebleId, FechaDesde, FechaHasta FROM contratos WHERE IdContrato = @id";
				
				 string sql = @"SELECT T1.IdContrato, T1.InquilinoId, T1.InmuebleId, T1.FechaDesde, T1.FechaHasta, T1.Vigente, T1.Monto,
				 T2.IdInquilino, T2.Nombre, T2.Apellido, T2.Dni, T2.Telefono, T2.Email,
				 T3.IdInmueble, T3.Direccion, T3.Tipo, T3.Latitud, T3.Longitud, T3.Precio, T3.Disponible
				 FROM contratos AS T1 
				 INNER JOIN inquilinos AS T2 ON T1.InquilinoId = T2.IdInquilino 
				 INNER JOIN inmuebles AS T3 ON T1.InmuebleId = T3.IdInmueble AND T1.IdContrato = @id";



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
							Monto = reader.GetDecimal("Monto"),
							Vigente = reader.GetBoolean("Vigente"),
							Inmueble = repositorioInmueble.InmuebleObtenerPorId(reader.GetInt32("InmuebleId")),
							Inquilino = repositorioInquilino.InquilinoObtenerPorId(reader.GetInt32("InquilinoId")),

						};
					}
					connection.Close();
				}			
			}
			return c!;
		}


		public int ContratoEditar(Contrato contrato)
		{
			int res = -1;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE contratos
					SET	InquilinoId=@InquilinoId, InmuebleId=@InmuebleId, FechaDesde=@FechaDesde, FechaHasta=@FechaHasta, Vigente=@Vigente
					WHERE IdContrato=@IdContrato;";


				
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@IdContrato", contrato.IdContrato);
					command.Parameters.AddWithValue("@InquilinoId", contrato.InquilinoId);
					command.Parameters.AddWithValue("@InmuebleId", contrato.InmuebleId);
					command.Parameters.AddWithValue("@FechaDesde", contrato.FechaDesde);
					command.Parameters.AddWithValue("@FechaHasta", contrato.FechaHasta);
					command.Parameters.AddWithValue("@Vigente", contrato.Vigente);

					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}


		public int ContratoRenovar(Contrato contrato)
		{
			int res = -1;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE contratos
					SET	InquilinoId=@InquilinoId, InmuebleId=@InmuebleId, FechaDesde=@FechaDesde, FechaHasta=@FechaHasta, Vigente=@Vigente
					WHERE IdContrato=@IdContrato;";


				
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@IdContrato", contrato.IdContrato);
					command.Parameters.AddWithValue("@InquilinoId", contrato.InquilinoId);
					command.Parameters.AddWithValue("@InmuebleId", contrato.InmuebleId);
					command.Parameters.AddWithValue("@FechaDesde", contrato.FechaDesde);
					command.Parameters.AddWithValue("@FechaHasta", contrato.FechaHasta);
					command.Parameters.AddWithValue("@Vigente", contrato.Vigente);

					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		
    }
}