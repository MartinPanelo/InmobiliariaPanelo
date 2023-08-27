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

            

            return View("VistaAgregar");            
        }





        [HttpPost]
        public ActionResult InmuebleAgregar(Propietario propietario){			
            try
			{
				if (ModelState.IsValid)
				{
					repositorio.PropietarioAlta(propietario);
					//TempData["Id"] = propietario.IdPropietario;
					return RedirectToAction("Index");
				}
				else
					return View(propietario);
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
