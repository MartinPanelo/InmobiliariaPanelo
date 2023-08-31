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
					return View(inmueble);
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

    }
}
