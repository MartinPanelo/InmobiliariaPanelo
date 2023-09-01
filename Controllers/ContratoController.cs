using Microsoft.AspNetCore.Mvc;
using InmobiliariaPanelo.Models;


namespace InmobiliariaPanelo.Controllers{

    public class ContratoController : Controller{

        private readonly RepositorioContrato repositorio = new RepositorioContrato();
      //  private readonly RepositorioPropietario repositorioP = new RepositorioPropietario();
      private readonly RepositorioInmueble repositorioInmueble= new RepositorioInmueble();
      private readonly RepositorioInquilino repositorioInquilino = new RepositorioInquilino();


        public ActionResult Index(){
		    var lista = repositorio.ContratoObtenerTodos();
            
		//	Console.WriteLine(TempData["mensaje"]);
            return View(lista);
            
        }

        public ActionResult ContratoAgregar(){		

           // ViewBag.Propietarios = repositorioP.PropietarioObtenerTodos();
            ViewBag.listaInmuebles = repositorioInmueble.InmuebleObtenerTodos();
            ViewBag.listaInquilinos = repositorioInquilino.InquilinoObtenerTodos();
            

            return View("VistaAgregar");            
        }


        [HttpPost]
        public ActionResult ContratoAgregar(Contrato contrato){
            
        			
            try
			{
				if (ModelState.IsValid)
				{
					repositorio.ContratoAlta(contrato);

					return RedirectToAction("Index");
				}
				else
					return View(contrato);
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