using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaPanelo.Models{

    public class Pago{
        [Key]
		[Display(Name = "Identificador")]
        public int IdPago {get; set;} 

        [ForeignKey(nameof(ContratoId))]
		[Display(Name = "Contrato")]
		public int ContratoId { get; set; }
        public Contrato? Contrato{ get; set; }
		public decimal Monto{ get; set; }

        public DateTime Fecha { get; set; }



    }
}