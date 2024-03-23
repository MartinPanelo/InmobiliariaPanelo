using Microsoft.AspNetCore.Mvc;
using InmobiliariaPanelo.Models;
using Microsoft.AspNetCore.Authorization;
using Org.BouncyCastle.Asn1.Icao;

namespace InmobiliariaPanelo.Controllers
{

    public class PagoController : Controller
    {

        private readonly RepositorioPago repositorioPago = new RepositorioPago();
        private readonly RepositorioContrato repositorioContrato = new RepositorioContrato();

        [Authorize]
        public ActionResult Index()
        {

            var lista = repositorioContrato.ContratoObtenerTodos();
            return View(lista);
        }


        [Authorize]
        public ActionResult VistaDetalles(int id)
        {

            Pago pagodetalle = repositorioPago.PagoDetallePorIdContrato(id);


            ViewBag.cantpagos = repositorioPago.VerCantidadDePagos(id);

            TimeSpan diferencia = pagodetalle.Contrato.FechaHasta - pagodetalle.Contrato.FechaDesde;

            int cuotas = diferencia.Days / 30;

            if (diferencia.Days % 30 != 0)
            {
                cuotas++;
            }

            ViewBag.cuotas = cuotas;

            var pago = repositorioPago.PagosObtenerPorIdContrato(id);
            ViewBag.ListaDePagos = pago;
            var Contrato = repositorioContrato.ContratoObtenerPorId(id);
            ViewData["detalle"] = "Detalles de los pagos";


            return View("VistaDetalles", Contrato);

        }

        [Authorize]
        public ActionResult VistaPago(int id)
        {

            Pago pago = repositorioPago.PagoDetallePorIdContrato(id);
            ViewData["detalle"] = "Detalle del pago";



            Pago pagodetalle = repositorioPago.PagoDetallePorIdContrato(id);            
            TimeSpan diferencia = pagodetalle.Contrato.FechaHasta - pagodetalle.Contrato.FechaDesde;

            int cuotas = diferencia.Days / 30;

            if (diferencia.Days % 30 != 0)
            {
                cuotas++;
            }

            int cantpagos = repositorioPago.VerCantidadDePagos(id);

            if(cantpagos == (cuotas-1)){
                ViewBag.MontoAPagar = Math.Round(pago.Contrato.Inmueble.Precio / 30 * (diferencia.Days % 30), 2);/*  + pago.Contrato.Inmueble.Precio / 30 * (diferencia.Days % 30); */
            }else{
                ViewBag.MontoAPagar = pago.Contrato.Inmueble.Precio;
            }

            


            return View("VistaPago", pago);

        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult RealizarPago(Pago pago)
        {


            try
            {
                if (ModelState.IsValid)
                {



                    Pago pagodetalle = repositorioPago.PagoDetallePorIdContrato(pago.ContratoId);

                    TimeSpan diferencia = pagodetalle.Contrato.FechaHasta - pagodetalle.Contrato.FechaDesde;

                    int cuotas = diferencia.Days / 30;
                    
                    
                    //Console.WriteLine(pagodetalle.Monto + " " + pago.Monto + " " + cuotas);   
                    if (diferencia.Days % 30 != 0)
                    {
                        cuotas++;
                    }
                    int cantpagos = repositorioPago.VerCantidadDePagos(pago.ContratoId);

                    if(cantpagos == (cuotas-1)){
                        pago.Monto = pago.Monto / 30 * (diferencia.Days % 30);
                    }else{
                        pago.Monto = pago.Monto;
                    }
                    //pago.Monto = pagodetalle.Monto / cuotas;
                    if (repositorioPago.PagoAlta(pago) != 1)
                    { // se realizo el pago

                        int cantpago = repositorioPago.VerCantidadDePagos(pago.ContratoId);

                       
                        if (cantpago >= cuotas)
                        {

                            pagodetalle.Contrato.Vigente = false;

                            repositorioContrato.ContratoEditar(pagodetalle.Contrato);
                        }
                    }
                    TempData["msj"] = "Pago realizado correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["msj"] = "No se ha realizado el pago";
                    return RedirectToAction("Index");
                    //	return View("VistaPago", pago); 
                }
            }
            catch (Exception ex)
            {
                TempData["msj"] = "Se ha producido un error al realizar el pago : " + ex.Message;

                return RedirectToAction("Index");
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult EliminarPago(int id, int id2)
        {
            //esto es la accion
            try
            {
                repositorioPago.PagoEliminar(id);
                TempData["msj"] = "Se ha eliminado el pago.";
                return VistaDetalles(id2);
            }
            catch (Exception ex)
            {
                TempData["msj"] = "Se ha producido un error, y el pago no se ha eliminado : " + ex.Message;
                //   return RedirectToAction("VistaDetalles");
                return RedirectToAction("Index");
            }
        }
    }
}