using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeluqueriApp.Models;
using PeluqueriApp.Services;
using System.Diagnostics;

namespace PeluqueriApp.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmpresaService _empresaService;


        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, IEmpresaService empresaService)
        {
            _logger = logger;
            _userManager = userManager;
            _empresaService = empresaService;
        }

        private async Task<Empresa> GetEmpresaFromUserAsync()
        {
            var user = await _userManager.GetUserAsync(User) as ApplicationUser;
            if (user?.IdEmpresa == null)
            {
                return null;
            }

            return await _empresaService.GetEmpresaByIdAsync(user.IdEmpresa.Value);
        }

        public async Task<IActionResult> Index()
        {
            var empresa = await GetEmpresaFromUserAsync();
            if (empresa != null)
            {
                ViewBag.ColorPrincipal = empresa.ColorPrincipal;
                ViewBag.Logo = empresa.Logo;
            }
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

        public IActionResult TermsAndConditions()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }
    }
}