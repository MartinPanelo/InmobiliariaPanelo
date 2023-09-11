using Microsoft.AspNetCore.Mvc;
using InmobiliariaPanelo.Models;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaPanelo.Controllers{

    public class InmuebleController : Controller{

        private readonly RepositorioInmueble repositorio = new RepositorioInmueble();
        private readonly RepositorioPropietario repositorioP = new RepositorioPropietario();

        [Authorize]
        public ActionResult Index(){
		    var lista = repositorio.InmuebleObtenerTodos();
            return View(lista);
            
        }

        [Authorize]
        public ActionResult InmuebleBuscador(){		

            ViewBag.Propietarios = repositorioP.PropietarioObtenerTodos();
            ViewBag.Usos = repositorio.usos();
            ViewBag.Tipos = repositorio.tipos();            

            return View("VistaBuscador");            
        }
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult InmuebleBuscador(Inmueble inmueble, String FechaDesde, String FechaHasta){		

            try
            {
            ViewBag.InmuebleFiltro = repositorio.InmuebleObtenerConFiltro(inmueble,FechaDesde,FechaHasta);
            ViewBag.Propietarios = repositorioP.PropietarioObtenerTodos();
            ViewBag.Usos = repositorio.usos();
            ViewBag.Tipos = repositorio.tipos();   
                return View("VistaBuscador");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        } 


























        [Authorize]
        public ActionResult VistaDetalles(int id){
		
		Inmueble i = repositorio.InmuebleObtenerPorId(id);
        ViewData["detalle"]="detalle del inmueble";
            return View("VistaDetalles",i);
            
        }

        [Authorize]
        public ActionResult InmuebleAgregar(){		

            ViewBag.Propietarios = repositorioP.PropietarioObtenerTodos();
            ViewBag.Usos = repositorio.usos();
            ViewBag.Tipos = repositorio.tipos();            

            return View("VistaAgregar");            
        }





        [HttpPost]
        [Authorize]
		[ValidateAntiForgeryToken]
        public ActionResult InmuebleAgregar(Inmueble inmueble){		

            try
			{
				if (ModelState.IsValid && inmueble.PropietarioId != 0)
				{
					repositorio.InmuebleAlta(inmueble);

					return RedirectToAction("Index");
				}
				else
                    ViewBag.Propietarios = repositorioP.PropietarioObtenerTodos();
                    ViewBag.Usos = repositorio.usos();
                    ViewBag.Tipos = repositorio.tipos();
                    return View("VistaAgregar");
			}
			catch (Exception ex)
			{
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
			}
        } 


        [Authorize(Policy = "Administrador")]
        public ActionResult InmuebleEliminar(int id){
            //esto es la vista
            var InmuebleEliminar = repositorio.InmuebleObtenerPorId(id);
            return View("VistaEliminar", InmuebleEliminar);          
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Administrador")]
        public ActionResult InmuebleEliminar(int id,int id2=0){
            //esto es la accion
           try
			{
                var InmuebleEliminar = repositorio.InmuebleObtenerPorId(id);
				repositorio.InmuebleEliminar(id);
				return RedirectToAction("Index");
			}            
			catch (Exception ex)
			{
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
			}
        }


        [Authorize]
        public ActionResult InmuebleEditar(int id){

            var InmuebleEditar = repositorio.InmuebleObtenerPorId(id);
            ViewBag.Propietarios = repositorioP.PropietarioObtenerTodos();
            ViewBag.Usos = repositorio.usos();
            ViewBag.Tipos = repositorio.tipos();
            return View("VistaEditar", InmuebleEditar);        
        }


        [HttpPost]
        [Authorize]
		[ValidateAntiForgeryToken]
        public ActionResult InmuebleEditar(int id, Inmueble inmueble){

            Inmueble? i = null;
            try{
                i = repositorio.InmuebleObtenerPorId(id);
                i.CantidadAmbientes = inmueble.CantidadAmbientes;
                i.Direccion = inmueble.Direccion;
                i.Precio = inmueble.Precio;
                i.Tipo = inmueble.Tipo;
                i.Uso = inmueble.Uso;
                i.PropietarioId = inmueble.PropietarioId;
                i.Latitud = inmueble.Latitud;
                i.Longitud = inmueble.Longitud;
                i.Disponible = inmueble.Disponible;
                repositorio.InmuebleEditar(i);
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
