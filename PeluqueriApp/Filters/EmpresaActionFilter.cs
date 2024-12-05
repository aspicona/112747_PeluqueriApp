using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using PeluqueriApp.Models;
using PeluqueriApp.Services;

public class EmpresaActionFilter : IAsyncActionFilter
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmpresaService _empresaService;

    public EmpresaActionFilter(UserManager<ApplicationUser> userManager, IEmpresaService empresaService)
    {
        _userManager = userManager;
        _empresaService = empresaService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var controller = context.Controller as Controller;
        if (controller != null)
        {
            var user = await _userManager.GetUserAsync(context.HttpContext.User) as ApplicationUser;
            if (user != null && user.IdEmpresa.HasValue)
            {
                var empresa = await _empresaService.GetEmpresaByIdAsync(user.IdEmpresa.Value);
                controller.ViewBag.Logo = empresa?.Logo;
                controller.ViewBag.ColorPrincipal = empresa?.ColorPrincipal ?? "#000";
                controller.ViewBag.EmpresaNombre = empresa.Nombre;
            }
            else
            {
                controller.ViewBag.Logo = "/images/logo.png"; // Logo por defecto
                controller.ViewBag.ColorPrincipal = "#000"; // Color por defecto
                controller.ViewBag.EmpresaNombre = "Menú";
            }
        }

        await next();
    }
}

