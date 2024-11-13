using Microsoft.AspNetCore.Mvc;
using PeluqueriApp.Models;
using PeluqueriApp.Services;


public class ServicioController : Controller
{
    private readonly IServicioService _servicioService;
    private readonly IEmpresaService _empresaService;

    public ServicioController(IServicioService servicioService, IEmpresaService empresaService)
    {
        _servicioService = servicioService;
        _empresaService = empresaService;
    }

    public async Task<IActionResult> Index()
    {
        var servicios = await _servicioService.GetAllServiciosAsync();
        return View(servicios);
    }

    // Crear servicio (GET)
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Empresas = await _empresaService.GetAllEmpresasAsync();
        return View();
    }

    // Crear servicio (POST)
    [HttpPost]
    public async Task<IActionResult> Create(Servicio servicio)
    {
        if (ModelState.IsValid)
        {
            await _servicioService.AddServicioAsync(servicio);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Empresas = await _empresaService.GetAllEmpresasAsync();
        return View(servicio);
    }

    // Editar servicio (GET)
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var servicio = await _servicioService.GetServicioByIdAsync(id);
        if (servicio == null)
        {
            return NotFound();
        }
        ViewBag.Empresas = await _empresaService.GetAllEmpresasAsync();
        return View(servicio);
    }

    // Editar servicio (POST)
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Servicio servicio)
    {
        if (id != servicio.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _servicioService.UpdateServicioAsync(servicio);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Empresas = await _empresaService.GetAllEmpresasAsync();
        return View(servicio);
    }

    // Eliminar servicio (GET)
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var servicio = await _servicioService.GetServicioByIdAsync(id);
        if (servicio == null)
        {
            return NotFound();
        }

        return View(servicio);
    }

    // Eliminar servicio (POST)
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _servicioService.DeleteServicioAsync(id);
        return RedirectToAction(nameof(Index));
    }
}



