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
				if (ModelState.IsValid && contrato.FechaDesde < contrato.FechaHasta)
				{
                    
					repositorio.ContratoAlta(contrato);

					return RedirectToAction("Index");
				}
				else

                ViewBag.listaInmuebles = repositorioInmueble.InmuebleObtenerTodos();
                ViewBag.listaInquilinos = repositorioInquilino.InquilinoObtenerTodos();
                if(contrato.FechaDesde > contrato.FechaHasta){
                    TempData["mensaje"] = "La fecha de inicio debe ser menor a la fecha de finalización";
                }
					return View("VistaAgregar", contrato);
			}
			catch (Exception ex)
			{
                TempData["Error"] = ex.Message;
                //mandar el error tambien
                return RedirectToAction("Index");
			}
            
        }




         public ActionResult ContratoEliminar(int id){
            //esto es la vista
            var ContratoEliminar = repositorio.ContratoObtenerPorId(id);
            return View("VistaEliminar", ContratoEliminar);          
        }


        [HttpPost]
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




        public ActionResult VistaDetalles(int id){
		
		Contrato c = repositorio.ContratoObtenerPorId(id);

        

        ViewData["detalle"]="detalle del Contrato";
            return View("VistaDetalles",c);
            
        }



         public ActionResult ContratoEditar(int id){

            var ContratoEditar = repositorio.ContratoObtenerPorId(id);
            ViewBag.listaInmuebles = repositorioInmueble.InmuebleObtenerTodos();
            ViewBag.listaInquilinos = repositorioInquilino.InquilinoObtenerTodos();
            return View("VistaEditar", ContratoEditar);        
        }

 
        [HttpPost]
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