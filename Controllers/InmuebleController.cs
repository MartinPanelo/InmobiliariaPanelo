using Microsoft.AspNetCore.Mvc;
using InmobiliariaPanelo.Models;


namespace InmobiliariaPanelo.Controllers{

    public class InmuebleController : Controller{

        private readonly RepositorioInmueble repositorio = new RepositorioInmueble();
        private readonly RepositorioPropietario repositorioP = new RepositorioPropietario();


        public ActionResult Index(){
		    var lista = repositorio.InmuebleObtenerTodos();
            
		//	Console.WriteLine(TempData["mensaje"]);
            return View(lista);
            
        }

        public ActionResult VistaDetalles(int id){
		
		Inmueble i = repositorio.InmuebleObtenerPorId(id);
        ViewData["detalle"]="detalle del inmueble";
            return View("VistaDetalles",i);
            
        }

        public ActionResult InmuebleAgregar(){		

            ViewBag.Propietarios = repositorioP.PropietarioObtenerTodos();
            ViewBag.Usos = repositorio.usos();
            ViewBag.Tipos = repositorio.tipos();
            

            return View("VistaAgregar");            
        }





        [HttpPost]
        public ActionResult InmuebleAgregar(Inmueble inmueble){		

            try
			{
				if (ModelState.IsValid)
				{
					repositorio.InmuebleAlta(inmueble);
					//TempData["Id"] = propietario.IdPropietario;
					return RedirectToAction("Index");
				}
				else
					return View("VistaAgregar",inmueble);
			}
			catch (Exception ex)
			{
                TempData["Error"] = ex.Message;
                //mandar el error tambien
                return RedirectToAction("Index");
			}
        } 



        public ActionResult InmuebleEliminar(int id){
            //esto es la vista
            var InmuebleEliminar = repositorio.InmuebleObtenerPorId(id);
            return View("VistaEliminar", InmuebleEliminar);          
        }

        [HttpPost]
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


        public ActionResult InmuebleEditar(int id){

            var InmuebleEditar = repositorio.InmuebleObtenerPorId(id);
            ViewBag.Propietarios = repositorioP.PropietarioObtenerTodos();
            ViewBag.Usos = repositorio.usos();
            ViewBag.Tipos = repositorio.tipos();
            return View("VistaEditar", InmuebleEditar);        
        }


        [HttpPost]
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
