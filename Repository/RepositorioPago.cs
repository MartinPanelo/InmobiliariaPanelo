using System.Data;
using MySql.Data.MySqlClient;

namespace InmobiliariaPanelo.Models
{
	public class RepositorioPago{
        readonly string	connectionString = "server=localhost;user=root;database=inmobiliaria;port=3306;password=";

	//	private readonly RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
	//	private readonly RepositorioInquilino repositorioInquilino = new RepositorioInquilino();  

        public List<Pago> PagoObtenerTodos()
        {

			List<Pago> res = new List<Pago>();

			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT T1.IdPago, T1.ContratoId, T1.Monto, T1.Fecha, T2.IdContrato, T2.InquilinoId, T2.InmuebleId, T2.FechaDesde, T2.FechaHasta, T2.Monto, T3.IdInquilino, T3.Nombre, T3.Apellido, T3.Dni, T3.Email, T3.Telefono
				 FROM pagos AS T1 
				 INNER JOIN contratos AS T2 ON T1.ContratoId = T2.IdContrato 
				 INNER JOIN inquilinos AS T3 ON T2.InquilinoId = T3.IdInquilino "; //AND T1.ContratoId  = @id
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago c = new Pago
						{
                            IdPago = reader.GetInt32("IdPago"),
                            ContratoId = reader.GetInt32("ContratoId"),
                            Monto = reader.GetDecimal("Monto"),
                            Fecha = reader.GetDateTime("Fecha"),
                             Contrato = new Contrato
                            {
                                IdContrato = reader.GetInt32("IdContrato"),
                                InquilinoId = reader.GetInt32("InquilinoId"),
                                InmuebleId = reader.GetInt32("InmuebleId"),
                                FechaDesde = reader.GetDateTime("FechaDesde"),
                                FechaHasta = reader.GetDateTime("FechaHasta"),
                                Monto = reader.GetDecimal("Monto"),
                                Inquilino = new Inquilino{
                                    IdInquilino = reader.GetInt32("IdInquilino"),
                                    Nombre = reader.GetString("Nombre"),
                                    Apellido = reader.GetString("Apellido"),
                                    Dni = reader.GetString("Dni"),
                                    Email = reader.GetString("Email"),
                                    Telefono = reader.GetString("Telefono")
                                },

                            } };
						res.Add(c);
					}
					connection.Close();
				}
			}

			return res;
		}
    }
}