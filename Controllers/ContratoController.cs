using Microsoft.AspNetCore.Mvc;
using InmobiliariaPanelo.Models;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaPanelo.Controllers
{

    public class ContratoController : Controller
    {

        private readonly RepositorioContrato repositorio = new RepositorioContrato();
        private readonly RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
        private readonly RepositorioInquilino repositorioInquilino = new RepositorioInquilino();

        [Authorize]
        public ActionResult Index()
        {
            var lista = repositorio.ContratoObtenerTodos();
            return View(lista);
        }

        [Authorize]
        public ActionResult ContratoAgregar()
        {

            ViewBag.listaInmuebles = repositorioInmueble.InmuebleObtenerTodos();
            ViewBag.listaInquilinos = repositorioInquilino.InquilinoObtenerTodos();

            return View("VistaAgregar");
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ContratoAgregar(Contrato contrato)
        {

            try
            {

                if (contrato.IdContrato != 0)
                {

                    Contrato ContratoARenovar = repositorio.ContratoObtenerPorId(contrato.IdContrato);

                    contrato.InquilinoId = ContratoARenovar.InquilinoId;
                    contrato.InmuebleId = ContratoARenovar.InmuebleId;
                    contrato.Monto = ContratoARenovar.Monto;

                    contrato.FechaDesde = ContratoARenovar.FechaHasta;
                    
                }

                if (ModelState.IsValid && (contrato.FechaDesde < contrato.FechaHasta))
                {

                    if (repositorio.ConsultaDeFecha(contrato))
                    {
                        //si no viene con id es un nuevo contrato 
                        //si viene con id es una renovacion


                        if (contrato.IdContrato != 0)
                        {
                            TempData["msj"] = "Contrato renovado correctamente";
                        }
                        else
                        {
                            TempData["msj"] = "Contrato agregado correctamente";
                        }
                        repositorio.ContratoAlta(contrato);


                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.listaInmuebles = repositorioInmueble.InmuebleObtenerTodos();
                        ViewBag.listaInquilinos = repositorioInquilino.InquilinoObtenerTodos();
                        TempData["ErrFecha"] = "El inmueble esta ocupado en ese rango de fechas";
                        if (contrato.IdContrato != 0)
                        {
                            return View("VistaRenovar", contrato);
                        }
                        else
                        {
                            return View("VistaAgregar", contrato);
                        }

                    }
                }
                else
                {

                    ViewBag.listaInmuebles = repositorioInmueble.InmuebleObtenerTodos();
                    ViewBag.listaInquilinos = repositorioInquilino.InquilinoObtenerTodos();
                    if (contrato.FechaDesde > contrato.FechaHasta)
                    {
                        TempData["ErrFecha"] = "La fecha de inicio debe ser menor a la fecha de finalización";
                        if (contrato.IdContrato != 0)
                        {
                            return View("VistaRenovar", contrato);
                        }
                        else
                        {
                            return View("VistaAgregar", contrato);
                        }
                    }
                    else
                    {
                        TempData["mensajeGlobalFormulario"] = "El formulario no es válido";
                        if (contrato.IdContrato != 0)
                        {
                            return View("VistaRenovar", contrato);
                        }
                        else
                        {
                            return View("VistaAgregar", contrato);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msj"] = "Se ha producido un error, y el contrato no se ha agregado." + ex.Message;
                return RedirectToAction("Index");
            }
        }


        [Authorize(Policy = "Administrador")]
        public ActionResult ContratoEliminar(int id)
        {
            //esto es la vista
            var ContratoEliminar = repositorio.ContratoObtenerPorId(id);
            return View("VistaEliminar", ContratoEliminar);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult ContratoEliminar(int id, int id2 = 0)
        {
            //esto es la accion
            try
            {
                if (repositorio.ContratoEliminar(id) != 0)
                {
                    TempData["msj"] = "Se ha eliminado el contrato. ";
                }
                else
                {
                    TempData["msj"] = "No se ha eliminado el contrato. ";


                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["msj"] = "Se ha producido un error, y el contrato no se ha eliminado el contrato. :"+ex.Message;
                return RedirectToAction("Index");
            }
        }



        [Authorize]
        public ActionResult VistaDetalles(int id)
        {

            Contrato c = repositorio.ContratoObtenerPorId(id);
            ViewData["detalle"] = "detalle del Contrato";

            return View("VistaDetalles", c);

        }


        [Authorize]
        public ActionResult ContratoEditar(int id)
        {
            TempData["mensaje"] = null;
            var ContratoEditar = repositorio.ContratoObtenerPorId(id);
            ViewBag.listaInmuebles = repositorioInmueble.InmuebleObtenerTodos();
            ViewBag.listaInquilinos = repositorioInquilino.InquilinoObtenerTodos();

            return View("VistaEditar", ContratoEditar);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ContratoEditar(int id, Contrato contrato)
        {

            Contrato? c = null;
            try
            {
                c = repositorio.ContratoObtenerPorId(id);
                if (contrato.FechaDesde < contrato.FechaHasta)
                {

                    if (repositorio.ConsultaDeFecha(contrato))
                    {
                        c.FechaDesde = contrato.FechaDesde;
                        c.FechaHasta = contrato.FechaHasta;
                        c.InmuebleId = contrato.InmuebleId;
                        c.InquilinoId = contrato.InquilinoId;
                        c.Vigente = contrato.Vigente;
                        c.Monto = contrato.Monto;
                        Console.Write("c: " + c);
                        Console.Write("contrato: " + contrato);

                        repositorio.ContratoEditar(c);
                        TempData["msj"] = "Contrato editado correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["mensaje"] = "El inmueble esta ocupado en ese rango de fechas";
                        var ContratoEditar = repositorio.ContratoObtenerPorId(id);
                        ViewBag.listaInmuebles = repositorioInmueble.InmuebleObtenerTodos();
                        ViewBag.listaInquilinos = repositorioInquilino.InquilinoObtenerTodos();

                        return View("VistaEditar", ContratoEditar);


                    }
                }
                else
                {
                    TempData["mensaje"] = "La fecha de inicio debe ser menor a la fecha de finalización";
                    var ContratoEditar = repositorio.ContratoObtenerPorId(id);
                    ViewBag.listaInmuebles = repositorioInmueble.InmuebleObtenerTodos();
                    ViewBag.listaInquilinos = repositorioInquilino.InquilinoObtenerTodos();

                    return View("VistaEditar", ContratoEditar);

                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }






        [Authorize]
        public ActionResult ContratoRenovar(int id)
        {
            TempData["mensaje"] = null;
            var ContratoRenovar = repositorio.ContratoObtenerPorId(id);

            ContratoRenovar.FechaDesde = ContratoRenovar.FechaHasta;

            ViewBag.listaInmuebles = repositorioInmueble.InmuebleObtenerTodos();
            ViewBag.listaInquilinos = repositorioInquilino.InquilinoObtenerTodos();

            return View("VistaRenovar", ContratoRenovar);
        }


    }
}