using Microsoft.AspNetCore.Mvc;
using InmobiliariaPanelo.Models;


namespace InmobiliariaPanelo.Controllers{

    public class PagoController : Controller{


     //   private readonly RepositorioContrato repositorioContrato = new RepositorioContrato();

        private readonly RepositorioPago repositorioPago = new RepositorioPago();

        public ActionResult Index(){
		    var lista = repositorioPago.PagoObtenerTodos();
            
		//	Console.WriteLine(TempData["mensaje"]);
            return View(lista);
            
        }


        public ActionResult VistaDetalles(int id){
		
		Pago pago = repositorioPago.PagoObtenerPorId(id);
        ViewData["detalle"]="detalle del pago";
            return View("VistaDetalles",pago);
            
        }

    }
}