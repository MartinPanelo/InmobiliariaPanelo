using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaPanelo.Models{

    public class Contrato{
        [Key]
		[Display(Name = "Identificador")]
        public int IdContrato {get; set;}

       // [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha desde")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime FechaDesde { get; set; }
            
      //  [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)] 
        [Display(Name = "Fecha hasta")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime FechaHasta { get; set; }

        [Required]
        [Display(Name = "Costo Total")]
        public decimal Monto { get; set; }

        [Display(Name = "Estado del contrato")]
        public bool Vigente { get; set; }

		[Display(Name = "Inquilino")]
		public int InquilinoId { get; set; }
		[ForeignKey(nameof(InquilinoId))]
		public Inquilino? Inquilino{ get; set; }


        [Display(Name = "Inmueble")]
		public int InmuebleId { get; set; }
		[ForeignKey(nameof(InmuebleId))]
		public Inmueble? Inmueble{ get; set; }




    }
}