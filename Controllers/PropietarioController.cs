using Microsoft.AspNetCore.Mvc;
using InmobiliariaPanelo.Models;


namespace InmobiliariaPanelo.Controllers{

    public class PropietarioController : Controller{

        private readonly RepositorioPropietario repositorio = new RepositorioPropietario();
		/* public PropietarioController(){
			repositorio = new RepositorioPropietario();
		} */


        public ActionResult Index(){
			var lista = repositorio.PropietarioObtenerTodos();
		//	Console.WriteLine(TempData["mensaje"]);
            return View(lista);
            
        }

        public ActionResult PropietarioEliminar(int id){

            var PropietarioEliminar = repositorio.PropietarioObtenerPorId(id);


            return View("VistaEliminar", PropietarioEliminar);
        
        
        
        }
        [HttpPost]
        public ActionResult PropietarioEliminar(int id,int id2=0){
          
           try
			{
				repositorio.PropietarioEliminar(id);
				TempData["Mensaje"] = "Eliminación realizada correctamente";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
                TempData["Mensaje"] = ex.Message;
                return RedirectToAction(nameof(Index));
			}
        }


        public ActionResult PropietarioEditar(int id){

            var PropietarioEditar = repositorio.PropietarioObtenerPorId(id);


            return View("VistaEditar", PropietarioEditar);
        
        
        
        }
    }
}
