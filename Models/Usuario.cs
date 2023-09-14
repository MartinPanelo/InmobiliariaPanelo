using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace InmobiliariaPanelo.Models
{
	public enum enRoles
	{
		Administrador = 1,
		Empleado = 2
	}

	public class Usuario
	{
		[Key]
		[Display(Name = "Código")]
		public int Id { get; set; }
		/* [Required] */
		[Required(ErrorMessage = "El nombre es obligatorio."),RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ]+$", ErrorMessage = "El nombre debe contener solo letras.")]
		public string Nombre { get; set; }
/* 		[Required] */
		[Required(ErrorMessage = "El apellido es obligatorio."),RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ]+$", ErrorMessage = "El apellido debe contener solo letras.")]	
		public string Apellido { get; set; }
		[Required(ErrorMessage = "El Email es obligatorio.")]
		[DataType(DataType.EmailAddress, ErrorMessage = "El email debe contener un formato valido.")]
		public string Email { get; set; }
		[Required(ErrorMessage = "La clave es obligatorio."), DataType(DataType.Password)]
		public string Clave { get; set; }

		public string Avatar { get; set; } = "";
		[Display(Name = "Imagen de perfil")]
		public IFormFile? AvatarFile { get; set; }
		public int Rol { get; set; }
	
		public string RolNombre => Rol > 0 ? ((enRoles)Rol).ToString() : "";

		public static IDictionary<int, string> ObtenerRoles()
		{
			SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
			Type tipoEnumRol = typeof(enRoles);
			foreach (var valor in Enum.GetValues(tipoEnumRol))
			{
				roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
			}
			return roles;
		}
	}
}
