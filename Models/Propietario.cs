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
		[Required, EmailAddress]
		public string? Email { get; set; }


		public override string ToString()
		{
			return $"{Nombre} {Apellido}  DNI: {Dni}";
		}




		/* public Propietario(int id, string nombre, string apellido, string dni, string? telefono, string email){
			IdPropietario = id;
			Nombre = nombre;
			Apellido = apellido;
			Dni = dni;
			Telefono = telefono;
			Email = email;

		} */

    }
}