using Microsoft.AspNetCore.Mvc;
using InmobiliariaPanelo.Models;


namespace InmobiliariaPanelo.Controllers{

    public class PropietarioController : Controller{

        private readonly IPropietarioRepositorio repositorio;
		public PropietarioController(){
			repositorio = new RepositorioPropietario();
		}


        public ActionResult Index(){
			var lista = repositorio.PropietarioObtenerTodos();
		//	Console.WriteLine(TempData["mensaje"]);
            return View(lista);
        }
    }
}
