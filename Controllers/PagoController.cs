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
            ViewData["detalle"]="Detalles de los pagos";

            int cantpagos =   repositorioPago.VerCantidadDePagos(id);
            Pago pagodetalle = repositorioPago.PagoDetallePorIdContrato(id);
            int cuotas = 12 * (pagodetalle.Contrato.FechaHasta.Year - pagodetalle.Contrato.FechaDesde.Year) + Math.Abs(pagodetalle.Contrato.FechaHasta.Month - pagodetalle.Contrato.FechaDesde.Month);

            ViewBag.pagos = cantpagos;
            ViewBag.cuotas = cuotas;

            return View("VistaDetalles",pago);
            
        }

        [Authorize]
        public ActionResult VistaPago(int id){
		
            Pago pago = repositorioPago.PagoDetallePorIdContrato(id);
            ViewData["detalle"]="Detalle del pago";

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
                        int meses = 12 * (pagodetalle.Contrato.FechaHasta.Year - pagodetalle.Contrato.FechaDesde.Year) + Math.Abs(pagodetalle.Contrato.FechaHasta.Month - pagodetalle.Contrato.FechaDesde.Month);

                        if(cantpagos >= meses){

                            pagodetalle.Contrato.Vigente = false;
                            repositorioContrato.ContratoEditar(pagodetalle.Contrato);
                        }
                    }
                    TempData["msj"] = "Pago realizado correctamente";
					return RedirectToAction("Index");
				}
				else{
                    TempData["msj"] = "No se ha realizado el pago";
                    return RedirectToAction("Index");
				//	return View("VistaPago", pago); 
                }
			}
			catch (Exception ex)
			{
                TempData["msj"] = "Se ha producido un error al realizar el pago";

                return RedirectToAction("Index");
			}
            
        }


        [HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Administrador")]
        public ActionResult EliminarPago(int id,int id2){
            //esto es la accion
           try
			{
				repositorioPago.PagoEliminar(id);
                TempData["msj"] = "Se ha eliminado el pago.";
                return VistaDetalles(id2);
			}            
			catch (Exception ex)
			{
                TempData["msj"] = "Se ha producido un error, y el pago no se ha eliminado.";
             //   return RedirectToAction("VistaDetalles");
             return RedirectToAction("Index");
			}
        }
    }
}