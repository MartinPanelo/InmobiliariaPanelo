using System.ComponentModel.DataAnnotations;

namespace InmobiliariaPanelo.Models{

    public class Propietario{
        [Key]
        [Display(Name = "Identificador")]
        public int IdPropietario {get; set;}
        [Required]
		public string? Nombre { get; set; }// = ""; / = String.Empty; 
		[Required]
		public string? Apellido { get; set; }
		[Required]
		public string? Dni { get; set; }
		[Required]
		public string? Telefono { get; set; }
		[EmailAddress]
		public string? Email { get; set; }
		[Required, DataType(DataType.Password)]
		public string? Clave { get; set; }


		/* public Propietario(int id, string nombre, string apellido, string dni, string? telefono, string email, string clave){
			IdPropietario = id;
			Nombre = nombre;
			Apellido = apellido;
			Dni = dni;
			Telefono = telefono;
			Email = email;
			Clave = clave;
		} */

    }
}