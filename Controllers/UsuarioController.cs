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

		public ActionResult Index()
		{
			
			return View("Login");
		}
		public ActionResult Gestion()
		{
			IList<Usuario> usuarios = repositorioUsuario.ObtenerTodos();
			return View("Gestion",usuarios);
		}
		public ActionResult AgregarUsuario()
		{
			ViewBag.Roles = Usuario.ObtenerRoles();
			return View("VistaAgregar");
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
	//	[Authorize(Policy = "Administrador")]
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
				}
			//	TempData["returnUrl"] = returnUrl;
				return Gestion();
			//	return View("gestion");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View();
			}
		}


		public async Task<ActionResult> Logout()
		{
			await HttpContext.SignOutAsync(
					CookieAuthenticationDefaults.AuthenticationScheme);
			//return RedirectToAction("Index", "Home");
			return View("Login");
		}


		public ActionResult UsuarioEliminar(int id)
		{
			var u = repositorioUsuario.ObtenerPorId(id);
			return View("VistaEliminar",u);
		}

		[HttpPost]
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


		 public ActionResult VistaDetalles(int id){
		
		Usuario u = repositorioUsuario.ObtenerPorId(id);

        

      
            return View("VistaDetalles",u);
            
        }



		public ActionResult UsuarioEditar()
		{
			
		//	var u = repositorioUsuario.ObtenerPorEmail(User.Identity.Name); este seria para el caso de q
		// voy a editar el perfil del logeado
			ViewBag.Roles = Usuario.ObtenerRoles();
			return View("VistaEditar", u);
		}















    }
}
