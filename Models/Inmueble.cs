using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaPanelo.Models{

    public class Inmueble{
        [Key]
		[Display(Name = "Identificador")]
        public int IdInmueble {get; set;}
		[Display(Name = "Cantidad de ambientes")]
		[RegularExpression(@"^\d+$", ErrorMessage = "Debe ser un número entero.")]
		[Required(ErrorMessage = "Este campo es obligatorio.")]
		public int CantidadAmbientes { get; set; }
		[Required(ErrorMessage = "Este campo es obligatorio.")]
		public string? Uso { get; set; }//tabla o enum
		[Required(ErrorMessage = "Este campo es obligatorio.")]
		public string? Tipo { get; set; }//tabla o enum
		[Required(ErrorMessage = "Este campo es obligatorio.")]
		[Display(Name = "Dirección")]
		public string? Direccion { get; set; }
		
		
		
		[RegularExpression(@"^[0-9]+([.,][0-9]{0,2})?$", ErrorMessage = "Debe ser un número con hasta dos decimales.")]
		[Required(ErrorMessage = "Este campo es obligatorio.")]
		[Display(Name = "Latitud")] // Personaliza el nombre del campo
		public decimal Latitud { get; set; }
		
		
		[RegularExpression(@"^[0-9]+([.,][0-9]{1,2})?$", ErrorMessage = "Debe ser un número con hasta dos decimales.")]

		[Required(ErrorMessage = "Este campo es obligatorio.")]
		public decimal Longitud { get; set; }
		
		
		
		[RegularExpression(@"^[0-9]+([.,][0-9]{1,2})?$", ErrorMessage = "Debe ser un número con hasta dos decimales.")]

		[Required(ErrorMessage = "Este campo es obligatorio.")]
		[Display(Name = "Costo Mensual")]
		public decimal Precio { get; set; }
		
		
		
		
		[Required(ErrorMessage = "Este campo es obligatorio.")]
		public bool Disponible { get; set; }



		[Display(Name = "Propietario")]
		[RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Debe seleccionar un propietario.")]

		public int PropietarioId { get; set; }
		[ForeignKey(nameof(PropietarioId))]
		public Propietario? Propietario{ get; set; }


		public override string ToString()
		{
			return $"{Direccion} - {Propietario?.Nombre} {Propietario?.Apellido}";
		}
		
    }
}




