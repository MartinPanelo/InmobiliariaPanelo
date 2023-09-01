using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaPanelo.Models{

    public class Inmueble{
        [Key]
		[Display(Name = "Identificador")]
        public int IdInmueble {get; set;}
		[Display(Name = "Cantidad de ambientes")]
		[Required]
		public int CantidadAmbientes { get; set; }
		[Required]
		public string? Uso { get; set; }//tabla o enum
		[Required]
		public string? Tipo { get; set; }//tabla o enum
		[Required]
		public string? Direccion { get; set; }
		[Required]
		public decimal Latitud { get; set; }
		[Required]
		public decimal Longitud { get; set; }
		[Required]
		public decimal Precio { get; set; }
		[Required]
		public bool Disponible { get; set; }



		[Display(Name = "Propietario")]
		public int PropietarioId { get; set; }
		[ForeignKey(nameof(PropietarioId))]
		public Propietario? Propietario{ get; set; }


		public override string ToString()
		{
			return $"{Direccion} - {Propietario?.Nombre} {Propietario?.Apellido}";
		}
		
    }
}