﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaPanelo.Models;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaPanelo.Controllers;

public class HomeController : Controller
{
/*     private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    } */
   // [Authorize]
    [AllowAnonymous]
    public ActionResult Index()
    {
        return View();
    }
    public ActionResult AccesoDenegado()
		{
			
			return View("AccesoDenegado");
		}

/*     public IActionResult Privacy()
    {
        return View();
    } */
/* 
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    } */
}
