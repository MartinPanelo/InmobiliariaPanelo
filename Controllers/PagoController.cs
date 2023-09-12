using Microsoft.AspNetCore.Mvc;
using InmobiliariaPanelo.Models;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaPanelo.Controllers{

    public class PagoController : Controller{

        private readonly RepositorioPago repositorioPago = new RepositorioPago();
        private readonly RepositorioContrato repositorioContrato = new RepositorioContrato();

        [Authorize]
        public ActionResult Index(){
		    var lista = repositorioContrato.ContratoObtenerTodos();

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
		
		Pago pago = repositorioPago.PagoDetallePorIdContrato(id);
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
                    
					if(repositorioPago.PagoAlta(pago) != 1){ // se realizo el pago

                        int cantpagos =   repositorioPago.VerCantidadDePagos(pago.ContratoId);

                        Pago pagodetalle = repositorioPago.PagoDetallePorIdContrato(pago.ContratoId);

                        if(cantpagos >= (pagodetalle.Contrato.FechaHasta.Month - pagodetalle.Contrato.FechaHasta.Month)){

                            pagodetalle.Contrato.Vigente = false;
                            repositorioContrato.ContratoEditar(pagodetalle.Contrato);

                        }
                    


                    
                    }



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