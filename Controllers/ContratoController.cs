using Microsoft.AspNetCore.Mvc;
using InmobiliariaPanelo.Models;


namespace InmobiliariaPanelo.Controllers{

    public class ContratoController : Controller{

        private readonly RepositorioContrato repositorio = new RepositorioContrato();
      //  private readonly RepositorioPropietario repositorioP = new RepositorioPropietario();


        public ActionResult Index(){
		    var lista = repositorio.ContratoObtenerTodos();
            
		//	Console.WriteLine(TempData["mensaje"]);
            return View(lista);
            
        }
    }
}