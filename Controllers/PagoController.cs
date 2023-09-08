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
		
		var pago = repositorioPago.PagosObtenerPorIdContrato(id);
        ViewData["detalle"]="detalle del pago";
            return View("VistaDetalles",pago);
            
        }


        public ActionResult VistaPago(int id){
		
		Pago pago = repositorioPago.PagoObtenerPorIdContrato(id);
        ViewData["detalle"]="detalle del pago";
            return View("VistaPago",pago);
            
        }

        [HttpPost]
        public ActionResult RealizarPago(Pago pago){
            
        			
            try
			{

				if (ModelState.IsValid)
				{
                    
					repositorioPago.PagoAlta(pago);

					return RedirectToAction("Index");
				}
				else{

					return View("VistaPago", pago);
                }
			}
			catch (Exception ex)
			{
                TempData["Error"] = ex.Message;
                //mandar el error tambien
                return RedirectToAction("Index");
			}
            
        }



    }
}