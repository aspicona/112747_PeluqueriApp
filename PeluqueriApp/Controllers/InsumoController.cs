using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PeluqueriApp.Models;
using PeluqueriApp.Services;
using System.Threading.Tasks;


public class InsumoController : Controller
{
    private readonly IInsumoService _insumoService;
    private readonly IEmpresaService _empresaService;
    private readonly IUnidadDeMedidaService _unidadDeMedidaService;

    public InsumoController(IInsumoService insumoService, IEmpresaService empresaService, IUnidadDeMedidaService unidadDeMedidaService)
    {
        _insumoService = insumoService;
        _empresaService = empresaService;
        _unidadDeMedidaService = unidadDeMedidaService;
    }

    // Listar insumos
    public async Task<IActionResult> Index()
    {
        var insumos = await _insumoService.GetAllInsumosAsync();
        return View(insumos);
    }

    private async Task SetViewBagsAsync()
    {
        ViewBag.Empresas = new SelectList(await _empresaService.GetAllEmpresasAsync(), "Id", "Nombre");
        ViewBag.UnidadesDeMedida = new SelectList(await _unidadDeMedidaService.GetAllUnidadesAsync(), "Id", "Nombre");
    }

    // Crear insumo (GET)
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await SetViewBagsAsync();
        return View();
    }

    // Crear insumo (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Insumo insumo)
    {
        if (ModelState.IsValid)
        {
            await _insumoService.AddInsumoAsync(insumo);
            return RedirectToAction(nameof(Index));
        }
        await SetViewBagsAsync();
        return View(insumo);
    }

    // Editar insumo (GET)
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var insumo = await _insumoService.GetInsumoByIdAsync(id);
        if (insumo == null)
        {
            return NotFound();
        }
        await SetViewBagsAsync();
        return View(insumo);
    }

    // Editar insumo (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Insumo insumo)
    {
        if (id != insumo.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _insumoService.UpdateInsumoAsync(insumo);
            return RedirectToAction(nameof(Index));
        }
        await SetViewBagsAsync();
        return View(insumo);
    }

    // Eliminar insumo (GET)
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var insumo = await _insumoService.GetInsumoByIdAsync(id);
        if (insumo == null)
        {
            return NotFound();
        }

        return View(insumo);
    }

    // Eliminar insumo (POST)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _insumoService.DeleteInsumoAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
