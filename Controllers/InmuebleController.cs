using Microsoft.AspNetCore.Mvc;
using InmobiliariaPanelo.Models;


namespace InmobiliariaPanelo.Controllers{

    public class InmuebleController : Controller{

        private readonly RepositorioInmueble repositorio = new RepositorioInmueble();

      /*   public ActionResult VistaDetalles(int id){
		
		Inquilino i = repositorio.InquilinoObtenerPorId(id);
        ViewData["detalle"]="detalle del inquilino";
            return View("VistaDetalles",i);
            
        } */
        public ActionResult Index(){
		    var lista = repositorio.InmuebleObtenerTodos();
		//	Console.WriteLine(TempData["mensaje"]);
            return View(lista);
            
        }

/* 
        public ActionResult InquilinoEliminar(int id){
            //esto es la vista
            var InquilinoEliminar = repositorio.InquilinoObtenerPorId(id);
            return View("VistaEliminar", InquilinoEliminar);          
        }

        [HttpPost]
        public ActionResult InquilinoEliminar(int id,int id2=0){
            //esto es la accion
           try
			{
                var InquilinoEliminar = repositorio.InquilinoObtenerPorId(id);
				repositorio.InquilinoEliminar(id);
				return RedirectToAction("Index");
			}            
			catch (Exception ex)
			{
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
			}
        }

        public ActionResult InquilinoAgregar(){			
            return View("VistaAgregar");            
        }
        [HttpPost]
        public ActionResult InquilinoAgregar(Inquilino inquilino){			
            try
			{
				if (ModelState.IsValid)
				{
					repositorio.InquilinoAlta(inquilino);

					return RedirectToAction("Index");
				}
				else
					return View(inquilino);
			}
			catch (Exception ex)
			{
                TempData["Error"] = ex.Message;
                //mandar el error tambien
                return RedirectToAction("Index");
			}
            
        }

        public ActionResult InquilinoEditar(int id){

            var inquilinoEditar = repositorio.InquilinoObtenerPorId(id);
            return View("VistaEditar", inquilinoEditar);        
        }
        [HttpPost]
        public ActionResult InquilinoEditar(int id, Inquilino inquilino){

            Inquilino? i = null;
            try{
                i = repositorio.InquilinoObtenerPorId(id);
                i.Nombre = inquilino.Nombre;
                i.Apellido = inquilino.Apellido;
                i.Dni = inquilino.Dni;
                i.Telefono = inquilino.Telefono;
                i.Email = inquilino.Email;
                repositorio.InquilinoEditar(i);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        } */

    }
}
