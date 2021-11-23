using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using wallet.Models;
using wallet.Services;

namespace wallet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            Api api = new();
            ViewBag.AliceAddress = "ZX_l5+DlJiQherZ+sbdAXCPZEuq8FQX8+GPuENm03Sc6lI=";
            ViewBag.AliceLicenceToken = await api.GetBalanceToken("ZX_l5+DlJiQherZ+sbdAXCPZEuq8FQX8+GPuENm03Sc6lI=");
            ViewBag.BobAddress = "ZX_BIzcoHrPfH4SSLXUrWOW2jOAwI14ptlCiP/3C2ZFEOQ=";
            ViewBag.BobLicenceToken = await api.GetBalanceToken("ZX_BIzcoHrPfH4SSLXUrWOW2jOAwI14ptlCiP/3C2ZFEOQ=");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
