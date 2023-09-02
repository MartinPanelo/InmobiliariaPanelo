using System.ComponentModel.DataAnnotations;

namespace InmobiliariaPanelo.Models{

    public class Inquilino{
        [Key]
		[Display(Name = "Identificador")]
        public int IdInquilino {get; set;}
		[Required(ErrorMessage = "El nombre es obligatorio."),RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El nombre debe contener solo letras.")]
		public string? Nombre { get; set; }
		[Required(ErrorMessage = "El apellido es obligatorio.")]
	    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El apellido debe contener solo letras.")]	
		public string? Apellido { get; set; }
		[Required(ErrorMessage = "El D.N.I. es obligatorio.")]
		[RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe contener 8 dígitos.")]
		[Display(Name = "D.N.I.")]
		public string? Dni { get; set; }
		[Required(ErrorMessage = "El teléfono es obligatorio."), RegularExpression(@"^[0-9]+$", ErrorMessage = "El teléfono debe contener solo numeros.")]
		[Display(Name = "Teléfono")]
		public string? Telefono { get; set; }
		[DataType(DataType.EmailAddress, ErrorMessage = "El email debe contener un formato valido.")]
		public string? Email { get; set; }

		public override string ToString()
		{
			return $"{Nombre} {Apellido}  DNI: {Dni}";
		}
    }
}