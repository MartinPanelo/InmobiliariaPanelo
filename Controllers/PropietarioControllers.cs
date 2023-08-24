using Microsoft.AspNetCore.Mvc;
using InmobiliariaPanelo.Models;


namespace InmobiliariaPanelo.Controllers{

    public class PropietarioController : Controller{

        private readonly RepositorioPropietario repositorio = new RepositorioPropietario();
		/* public PropietarioController(){
			repositorio = new RepositorioPropietario();
		} */

        public ActionResult VistaDetalles(int id){
		
        Propietario p = repositorio.PropietarioObtenerPorId(id);
        ViewData["detalle"]="detalles del propietario";


            return View("VistaDetalles",p);
            
        }
        public ActionResult Index(){
			var lista = repositorio.PropietarioObtenerTodos();
		//	Console.WriteLine(TempData["mensaje"]);
            return View(lista);
            
        }


        public ActionResult PropietarioEliminar(int id){
            //esto es la vista
            var PropietarioEliminar = repositorio.PropietarioObtenerPorId(id);
            return View("VistaEliminar", PropietarioEliminar);          
        }

        [HttpPost]
        public ActionResult PropietarioEliminar(int id,int id2=0){
            //esto es la accion
           try
			{
                var PropietarioEliminar = repositorio.PropietarioObtenerPorId(id);
    //TempData["Mensaje"] = "Se elimino correctamente al Propietario : " + PropietarioEliminar.Nombre + " " + PropietarioEliminar.Apellido;

				repositorio.PropietarioEliminar(id);
				return RedirectToAction("Index");
			}            
			catch (Exception ex)
			{
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
			}
        }

        public ActionResult PropietarioAgregar(){			
            return View("VistaAgregar");            
        }
        [HttpPost]
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
					return View(propietario);
			}
			catch (Exception ex)
			{
                TempData["Error"] = ex.Message;
                //mandar el error tambien
                return RedirectToAction("Index");
			}
            
        }

        public ActionResult PropietarioEditar(int id){

            var PropietarioEditar = repositorio.PropietarioObtenerPorId(id);
            return View("VistaEditar", PropietarioEditar);        
        }
        [HttpPost]
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
