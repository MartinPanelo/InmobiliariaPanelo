using Microsoft.AspNetCore.Mvc;
using InmobiliariaPanelo.Models;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaPanelo.Controllers{

    public class ContratoController : Controller{

        private readonly RepositorioContrato repositorio = new RepositorioContrato();
        private readonly RepositorioInmueble repositorioInmueble= new RepositorioInmueble();
        private readonly RepositorioInquilino repositorioInquilino = new RepositorioInquilino();

        [Authorize]
        public ActionResult Index(){
		    var lista = repositorio.ContratoObtenerTodos();
            return View(lista);            
        }

        [Authorize]
        public ActionResult ContratoAgregar(){		

            ViewBag.listaInmuebles = repositorioInmueble.InmuebleObtenerTodos();
            ViewBag.listaInquilinos = repositorioInquilino.InquilinoObtenerTodos();
            
            return View("VistaAgregar");            
        }


        [HttpPost]
        [Authorize]
		[ValidateAntiForgeryToken]
        public ActionResult ContratoAgregar(Contrato contrato){            
        			
            try
			{
				if (ModelState.IsValid && contrato.FechaDesde < contrato.FechaHasta)
				{                    
					repositorio.ContratoAlta(contrato);
					return RedirectToAction("Index");
				}
				else{

                ViewBag.listaInmuebles = repositorioInmueble.InmuebleObtenerTodos();
                ViewBag.listaInquilinos = repositorioInquilino.InquilinoObtenerTodos();
                if(contrato.FechaDesde > contrato.FechaHasta){
                    TempData["mensaje"] = "La fecha de inicio debe ser menor a la fecha de finalización";
                }
				return View("VistaAgregar", contrato);
                }
			}
			catch (Exception ex)
			{
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
			}            
        }


        [Authorize(Policy = "Administrador")]
         public ActionResult ContratoEliminar(int id){
            //esto es la vista
            var ContratoEliminar = repositorio.ContratoObtenerPorId(id);
            return View("VistaEliminar", ContratoEliminar);          
        }


        [HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Administrador")]
        public ActionResult ContratoEliminar(int id,int id2=0){
            //esto es la accion
           try
			{
                var ContratoEliminar = repositorio.ContratoObtenerPorId(id);
				repositorio.ContratoEliminar(id);
				return RedirectToAction("Index");
			}            
			catch (Exception ex)
			{
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
			}
        }



        [Authorize]
        public ActionResult VistaDetalles(int id){
		
		Contrato c = repositorio.ContratoObtenerPorId(id);  
        ViewData["detalle"]="detalle del Contrato";

        return View("VistaDetalles",c);
            
        }


        [Authorize]
        public ActionResult ContratoEditar(int id){

            var ContratoEditar = repositorio.ContratoObtenerPorId(id);
            ViewBag.listaInmuebles = repositorioInmueble.InmuebleObtenerTodos();
            ViewBag.listaInquilinos = repositorioInquilino.InquilinoObtenerTodos();

            return View("VistaEditar", ContratoEditar);        
        }

 
        [HttpPost]
        [Authorize]
		[ValidateAntiForgeryToken]
        public ActionResult ContratoEditar(int id, Contrato contrato){

            Contrato? c = null;
            try{
                c = repositorio.ContratoObtenerPorId(id);
                c.InquilinoId = contrato.InquilinoId;
                c.InmuebleId = contrato.InmuebleId;
                c.FechaDesde = contrato.FechaDesde;
                c.FechaHasta = contrato.FechaHasta;
                repositorio.ContratoEditar(c);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}