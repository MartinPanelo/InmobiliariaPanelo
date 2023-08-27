using System.ComponentModel.DataAnnotations;

namespace InmobiliariaPanelo.Models{

    public class Inmueble{
        [Key]
		[Display(Name = "Identificador")]
        public int IdInmueble {get; set;}
        [Required]
		public int PropietarioId { get; set; }
		[Display(Name = "Cantidad de ambientes")]
		[Required]
		public int CantidadAmbientes { get; set; }
		[Required]
		public string? Uso { get; set; }
		[Required]
		public string? Direccion { get; set; }
		[Required]
		public string? Tipo { get; set; }
		[Required]
		public decimal Latitud { get; set; }
		[Required]
		public decimal Longitud { get; set; }
		[Required]
		public decimal Precio { get; set; }
		[Required]
		public bool Disponible { get; set; }
    }
}