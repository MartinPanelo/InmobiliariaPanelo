using Microsoft.AspNetCore.Mvc;
using InmobiliariaPanelo.Models;


namespace InmobiliariaPanelo.Controllers{

    public class PagoController : Controller{


        private readonly RepositorioContrato repositorioContrato = new RepositorioContrato();

        private readonly RepositorioPago repositorioPago = new RepositorioPago();

        public ActionResult Index(){
		    var lista = repositorioContrato.ContratoObtenerTodos();
            
		//	Console.WriteLine(TempData["mensaje"]);
            return View(lista);
            
        }
    }
}