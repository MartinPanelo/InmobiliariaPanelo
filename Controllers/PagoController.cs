using Microsoft.AspNetCore.Mvc;
using InmobiliariaPanelo.Models;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaPanelo.Controllers{

    public class PagoController : Controller{

        private readonly RepositorioPago repositorioPago = new RepositorioPago();

        [Authorize]
        public ActionResult Index(){
		    var lista = repositorioPago.PagoObtenerTodos();

            return View(lista);            
        }
        [Authorize]
        public ActionResult VistaDetalles(int id){
		
		var pago = repositorioPago.PagosObtenerPorIdContrato(id);
        ViewData["detalle"]="detalle del pago";
        
        return View("VistaDetalles",pago);
            
        }

        [Authorize]
        public ActionResult VistaPago(int id){
		
		Pago pago = repositorioPago.PagoObtenerPorIdContrato(id);
        ViewData["detalle"]="detalle del pago";
        return View("VistaPago",pago);
            
        }


        [HttpPost]
        [Authorize]
		[ValidateAntiForgeryToken]
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


        [HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Administrador")]
        public ActionResult EliminarPago(int id,int id2=0){
            //esto es la accion
           try
			{
				repositorioPago.PagoEliminar(id);
				return VistaDetalles(id2);
			}            
			catch (Exception ex)
			{
                TempData["Error"] = ex.Message;
             //   return RedirectToAction("VistaDetalles");
             return RedirectToAction("Index");
			}
        }
    }
}