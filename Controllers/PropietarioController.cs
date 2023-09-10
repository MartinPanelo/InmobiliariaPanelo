using Microsoft.AspNetCore.Mvc;
using InmobiliariaPanelo.Models;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaPanelo.Controllers{

    public class PropietarioController : Controller{

        private readonly RepositorioPropietario repositorio = new RepositorioPropietario();

        [Authorize]
        public ActionResult VistaDetalles(int id){
		    Propietario p = repositorio.PropietarioObtenerPorId(id);
            return View("VistaDetalles",p);            
        }
        [Authorize]
        public ActionResult Index(){
			var lista = repositorio.PropietarioObtenerTodos();
            return View(lista);
            
        }

        [Authorize(Policy = "Administrador")]
        public ActionResult PropietarioEliminar(int id){
            //esto es la vista
            var PropietarioEliminar = repositorio.PropietarioObtenerPorId(id);
            return View("VistaEliminar", PropietarioEliminar);          
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Administrador")]
        public ActionResult PropietarioEliminar(int id,int id2=0){
            //esto es la accion
           try
			{
              //  var PropietarioEliminar = repositorio.PropietarioObtenerPorId(id);

				repositorio.PropietarioEliminar(id);
				return RedirectToAction("Index");
			}            
			catch (Exception ex)
			{
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
			}
        }
        [Authorize]
        public ActionResult PropietarioAgregar(){			
            return View("VistaAgregar");            
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult PropietarioAgregar(Propietario propietario){			
            try
			{
				if (ModelState.IsValid)
				{
					repositorio.PropietarioAlta(propietario);
					//TempData["Id"] = propietario.IdPropietario;
					return RedirectToAction("Index");
				}
				else
					return View("VistaAgregar",propietario);
			}
			catch (Exception ex)
			{
                TempData["Error"] = ex.Message;
                //mandar el error tambien
                return RedirectToAction("Index");
			}
            
        }

        [Authorize]
        public ActionResult PropietarioEditar(int id){

            var PropietarioEditar = repositorio.PropietarioObtenerPorId(id);
            return View("VistaEditar", PropietarioEditar);        
        }



        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult PropietarioEditar(int id, Propietario propietario){

            Propietario? p = null;
            try{
                p = repositorio.PropietarioObtenerPorId(id);
                p.Nombre = propietario.Nombre;
                p.Apellido = propietario.Apellido;
                p.Dni = propietario.Dni;
                p.Telefono = propietario.Telefono;
                p.Email = propietario.Email;
                repositorio.PropietarioEditar(p);
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
