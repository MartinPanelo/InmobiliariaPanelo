using System.Security.Claims;
using InmobiliariaPanelo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaPanelo.Controllers
{
	public class UsuarioController : Controller
	{


		private readonly IWebHostEnvironment environment;
	

		public UsuarioController(IWebHostEnvironment environment)
		{
	
			this.environment = environment;
		
		}

	private readonly RepositorioUsuario repositorioUsuario = new RepositorioUsuario();  

		[AllowAnonymous]
		public ActionResult Index()
		{
			
			return View("Login");
		}
		
		[Authorize]
		public ActionResult Gestion()
		{
			IList<Usuario> usuarios = repositorioUsuario.ObtenerTodos();
			return View("Gestion",usuarios);
		}

		[Authorize(Policy = "Administrador")]
		public ActionResult AgregarUsuario()
		{
			ViewBag.Roles = Usuario.ObtenerRoles();
			return View("VistaAgregar");
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Administrador")]
		public ActionResult UsuarioAgregar(Usuario u)
		{
			if (ModelState.IsValid){
			
				try
				{
					string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
									password: u.Clave,
									salt: System.Text.Encoding.ASCII.GetBytes("Agridulce"),
									prf: KeyDerivationPrf.HMACSHA1,
									iterationCount: 1000,
									numBytesRequested: 256 / 8));
					u.Clave = hashed;
					u.Rol = User.IsInRole("Administrador") ? u.Rol : (int)enRoles.Empleado;
				//	var nbreRnd = Guid.NewGuid();//posible nombre aleatorio
					int res = repositorioUsuario.UsuarioAlta(u);
					if (u.AvatarFile != null && u.Id > 0)
					{
						string wwwPath = environment.WebRootPath;
						string path = Path.Combine(wwwPath, "Uploads");
						if (!Directory.Exists(path))
						{
							Directory.CreateDirectory(path);
						}
						//Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
						string fileName = "avatar_" + u.Id + Path.GetExtension(u.AvatarFile.FileName);
						string pathCompleto = Path.Combine(path, fileName);
						u.Avatar = Path.Combine("/Uploads", fileName);
						// Esta operación guarda la foto en memoria en la ruta que necesitamos
						using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
						{
							u.AvatarFile.CopyTo(stream);
						}
						repositorioUsuario.Modificacion(u);
					}
					return Gestion(); 
				}
				catch (Exception ex)
				{
					ViewBag.Roles = Usuario.ObtenerRoles();
					return View("VistaAgregar");
				}
			}else{
				var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y=>y.Count>0)
                           .ToList();
					AgregarUsuario();
					
			}
			
			return View("VistaAgregar");
		}












        [HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginView login)
		{
			try
			{
			//	var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Login" : TempData["returnUrl"].ToString();
				if (ModelState.IsValid)
				{
					string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: login.Clave,
						salt: System.Text.Encoding.ASCII.GetBytes("Agridulce"),
						prf: KeyDerivationPrf.HMACSHA1,
						iterationCount: 1000,
						numBytesRequested: 256 / 8));

					var e = repositorioUsuario.ObtenerPorEmail(login.Usuario);
					if (e == null || e.Clave != hashed)
					{
						ModelState.AddModelError("", "El email o la clave no son correctos");
				//		TempData["returnUrl"] = returnUrl;
						return View();
					}

					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, e.Email),
						new Claim(ClaimTypes.Role, e.RolNombre)
					};

					var claimsIdentity = new ClaimsIdentity(
							claims, CookieAuthenticationDefaults.AuthenticationScheme);

					await HttpContext.SignInAsync(
							CookieAuthenticationDefaults.AuthenticationScheme,
							new ClaimsPrincipal(claimsIdentity));
				//	TempData.Remove("returnUrl");
				//	return Redirect(returnUrl);
				IList<Usuario> usuarios = repositorioUsuario.ObtenerTodos();
				return  Redirect("Gestion");
				}
			//	TempData["returnUrl"] = returnUrl;
				return View();

			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View();
			}
		}

		[Authorize]
		public async Task<ActionResult> Logout()
		{
			await HttpContext.SignOutAsync(
					CookieAuthenticationDefaults.AuthenticationScheme);
			//return RedirectToAction("Index", "Home");
			return Redirect("Index");
		}

		[Authorize(Policy = "Administrador")]
		public ActionResult UsuarioEliminar(int id)
		{
			var u = repositorioUsuario.ObtenerPorId(id);
			return View("VistaEliminar",u);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Administrador")]
		public ActionResult UsuarioEliminar(int id, Usuario usuario)
		{


			try
			{
				var UsuarioEliminar = repositorioUsuario.ObtenerPorId(id);
				int res = repositorioUsuario.UsuarioBorrar(id);
				if(res > 0){
					var ruta = Path.Combine(environment.WebRootPath, "Uploads", $"avatar_{id}" + Path.GetExtension(usuario.Avatar));
					if (System.IO.File.Exists(ruta))
						System.IO.File.Delete(ruta);
				}

				return Gestion();
			}
			catch
			{
				return View();
			}
		}

		[Authorize]
		 public ActionResult VistaDetalles(int id){
		
		Usuario u = repositorioUsuario.ObtenerPorId(id);

        

      
            return View("VistaDetalles",u);
            
        }

//------------------------------------------------------------
		[Authorize]
		public ActionResult MiPerfil()
		{
			ViewData["Title"] = "Mi perfil";
			var u = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
			ViewBag.Roles = Usuario.ObtenerRoles();
			return View("VistaEditar", u);
		}


		[Authorize(Policy = "Administrador")]
		public ActionResult UsuarioEditar(int id)
		{
			ViewData["Title"] = "Editar usuario";
			var u = repositorioUsuario.ObtenerPorId(id);
			ViewBag.Roles = Usuario.ObtenerRoles();
			return View("VistaEditar",u);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult UsuarioEditarContrasena(int id, Usuario ContrasenaNueva)
		{
			Usuario usuarioActual = null;
			if (!User.IsInRole("Administrador"))//no soy admin
			// sin embargo tengo que ver si esta tratando de modificando a otro usuario lo que no esta permitido
			// 
				{
					usuarioActual = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
					if (usuarioActual.Id != id) // estoy editando mi perfil o quiero editar el de alguien mas ( no puedo)
					{

						return View("AccesoDenegado");
					}
				}else{
					usuarioActual = repositorioUsuario.ObtenerPorId(ContrasenaNueva.Id);
				}


					try{
						string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: ContrasenaNueva.Clave,
						salt: System.Text.Encoding.ASCII.GetBytes("Agridulce"),
						prf: KeyDerivationPrf.HMACSHA1,
						iterationCount: 1000,
						numBytesRequested: 256 / 8));

						usuarioActual.Clave = hashed;

						repositorioUsuario.Modificacion(usuarioActual);
						return Gestion();
					}
					catch (Exception ex)
					{
						TempData["Error"] = ex.Message;
						return RedirectToAction("Gestion");
					}
					
		//	return View("VistaEditar");
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult UsuarioEditarPerfil(int id, Usuario PerfilNuevo)
		{
			Usuario usuarioActual = null;
			if (!User.IsInRole("Administrador"))//no soy admin
			// sin embargo tengo que ver si esta tratando de modificando a otro usuario lo que no esta permitido
			// 
				{
					usuarioActual = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
					if (usuarioActual.Id != id) // estoy editando mi perfil o quiero editar el de alguien mas ( no puedo)
					{

						return View("AccesoDenegado");
					}
				}else{
					usuarioActual = repositorioUsuario.ObtenerPorId(PerfilNuevo.Id);
				}

					try{

						usuarioActual.Nombre = PerfilNuevo.Nombre;
						usuarioActual.Apellido = PerfilNuevo.Apellido;
						usuarioActual.Email = PerfilNuevo.Email;
						if(User.IsInRole("Administrador")){
							usuarioActual.Rol = PerfilNuevo.Rol;
						}
						
						

						repositorioUsuario.Modificacion(usuarioActual);
						return Gestion();
					}
					catch (Exception ex)
					{
						TempData["Error"] = ex.Message;
						return RedirectToAction("Gestion");
					}
					
		//	return View("VistaEditar");
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult UsuarioEditarAvatar(int id, Usuario AvatarNuevo)
		{

			Usuario usuarioActual ;
			if (!User.IsInRole("Administrador"))//no soy admin
			// sin embargo tengo que ver si esta tratando de modificando a otro usuario lo que no esta permitido
			// 
				{
					usuarioActual = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
					if (usuarioActual.Id != id) // estoy editando mi perfil o quiero editar el de alguien mas ( no puedo)
					{

						return View("AccesoDenegado");
					}
				}else{
					usuarioActual = repositorioUsuario.ObtenerPorId(AvatarNuevo.Id);
				}
				
				
				try{

			
					if (AvatarNuevo.AvatarFile != null && AvatarNuevo.Id > 0)
					{
						string wwwPath = environment.WebRootPath;
						string path = Path.Combine(wwwPath, "Uploads");
						/* if (!Directory.Exists(path))
						{
							Directory.CreateDirectory(path);
						} */
						//Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
						string fileName = "avatar_" + AvatarNuevo.Id + Path.GetExtension(AvatarNuevo.AvatarFile.FileName);
						string pathCompleto = Path.Combine(path, fileName);
						AvatarNuevo.Avatar = Path.Combine("/Uploads", fileName);
						// Esta operación guarda la foto en memoria en la ruta que necesitamos
						using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
						{
							AvatarNuevo.AvatarFile.CopyTo(stream);
						}
						repositorioUsuario.ModificacionAvatar(AvatarNuevo);
					}


						return Gestion();
					}
					catch (Exception ex)
					{
						TempData["Error"] = ex.Message;
						return RedirectToAction("Gestion");
					}



		}







    }
}

		
/* 			if (!User.IsInRole("Administrador"))//no soy admin
			// sin embargo tengo que ver si esta tratando de modificando a otro usuario lo que no esta permitido
			// 
				{
					var usuarioActual = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
					if (usuarioActual.Id != id) // estoy editando mi perfil o quiero editar el de alguien mas ( no puedo)
					{

						return View("AccesoDenegado");
					}
				}


					try{
						string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: ContrasenaNueva.Clave,
						salt: System.Text.Encoding.ASCII.GetBytes("Agridulce"),
						prf: KeyDerivationPrf.HMACSHA1,
						iterationCount: 1000,
						numBytesRequested: 256 / 8));

						usuarioActual.Clave = hashed;

						repositorioUsuario.Modificacion(usuarioActual);
						return Gestion();
					}
					catch (Exception ex)
					{
						TempData["Error"] = ex.Message;
						return RedirectToAction("Index");
					}
					
			return View("VistaEditar");
		} */