using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeluqueriApp.Models;
using PeluqueriApp.Services;
using System.Threading.Tasks;


public class ProductoController : Controller
{
    private readonly IProductoService _productoService;
    private readonly IEmpresaService _empresaService;

    public ProductoController(IProductoService productoService, IEmpresaService empresaService)
    {
        _productoService = productoService;
        _empresaService = empresaService;
    }

    // Listar productos
    public async Task<IActionResult> Index()
    {
        var productos = await _productoService.GetAllProductosAsync();
        return View(productos);
    }

    // Crear producto (GET)
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Empresas = await _empresaService.GetAllEmpresasAsync();
        return View();
    }

    // Crear producto (POST)
    [HttpPost]
    public async Task<IActionResult> Create(Producto producto)
    {
        if (ModelState.IsValid)
        {
            await _productoService.AddProductoAsync(producto);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Empresas = await _empresaService.GetAllEmpresasAsync();
        return View(producto);
    }

    // Editar producto (GET)
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var producto = await _productoService.GetProductoByIdAsync(id);
        if (producto == null)
        {
            return NotFound();
        }
        ViewBag.Empresas = await _empresaService.GetAllEmpresasAsync();
        return View(producto);
    }

    // Editar producto (POST)
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Producto producto)
    {
        if (id != producto.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _productoService.UpdateProductoAsync(producto);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Empresas = await _empresaService.GetAllEmpresasAsync();
        return View(producto);
    }

    // Eliminar producto (GET)
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var producto = await _productoService.GetProductoByIdAsync(id);
        if (producto == null)
        {
            return NotFound();
        }

        return View(producto);
    }

    // Eliminar producto (POST)
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _productoService.DeleteProductoAsync(id);
        return RedirectToAction(nameof(Index));
    }
}


