using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeluqueriApp.Models;
using PeluqueriApp.Services;

using System.Threading.Tasks;

[Authorize(Roles = "SuperAdmin")]
public class EmpresaController : Controller
{
    private readonly IEmpresaService _empresaService;

    public EmpresaController(IEmpresaService empresaService)
    {
        _empresaService = empresaService;
    }

    // Listar empresas
    public async Task<IActionResult> Index()
    {
        var empresas = await _empresaService.GetAllEmpresasAsync();
        return View(empresas);
    }

    // Crear empresa (GET)
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // Crear empresa (POST)
    [HttpPost]
    public async Task<IActionResult> Create(Empresa empresa)
    {
        if (ModelState.IsValid)
        {
            await _empresaService.AddEmpresaAsync(empresa);
            return RedirectToAction(nameof(Index));
        }
        return View(empresa);
    }

    // Editar empresa (GET)
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var empresa = await _empresaService.GetEmpresaByIdAsync(id);
        if (empresa == null)
        {
            return NotFound();
        }
        return View(empresa);
    }

    // Editar empresa (POST)
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Empresa empresa)
    {
        if (id != empresa.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _empresaService.UpdateEmpresaAsync(empresa);
            return RedirectToAction(nameof(Index));
        }
        return View(empresa);
    }

    // Eliminar empresa (GET)
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var empresa = await _empresaService.GetEmpresaByIdAsync(id);
        if (empresa == null)
        {
            return NotFound();
        }
        return View(empresa);
    }

    // Eliminar empresa (POST)
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _empresaService.DeleteEmpresaAsync(id);
        return RedirectToAction(nameof(Index));
    }
}



