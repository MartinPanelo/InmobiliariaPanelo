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
							FechaFin = reader.GetDateTime("FechaHasta"),
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

    }
}