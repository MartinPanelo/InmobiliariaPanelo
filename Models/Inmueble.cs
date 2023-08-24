using System.ComponentModel.DataAnnotations;

namespace InmobiliariaPanelo.Models{

    public class Inmueble{
        [Key]
		[Display(Name = "Identificador")]
        public int IdInmueble {get; set;}
        [Required]
		public int PropietarioId { get; set; }
		[Required]
		public int CantidadAmbientes { get; set; }
		[Required]
		public Enum? Uso { get; set; }
		[Required]
		public string? Dirreccion { get; set; }
		[Required]
		public decimal Tipo { get; set; }
    }
}