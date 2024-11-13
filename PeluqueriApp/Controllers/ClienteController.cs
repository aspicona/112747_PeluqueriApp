using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PeluqueriApp.Models;
using PeluqueriApp.Services;
using System.Threading.Tasks;


public class ClienteController : Controller
{
    private readonly IClienteService _clienteService;
    private readonly IEmpresaService _empresaService;

    public ClienteController(IClienteService clienteService, IEmpresaService empresaService)
    {
        _clienteService = clienteService;
        _empresaService = empresaService;
    }

    // Listar todos los clientes
    public async Task<IActionResult> Index()
    {
        var clientes = await _clienteService.GetAllClientesAsync();
        return View(clientes);
    }

    // Crear cliente (GET)
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        // Cargar las empresas en el ViewBag para el dropdown en el formulario
        ViewBag.Empresas = new SelectList(await _empresaService.GetAllEmpresasAsync(), "Id", "Nombre");
        return View();
    }

    // Crear cliente (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Cliente cliente)
    {
        cliente.FechaCreacion = DateTime.Now;
        if (ModelState.IsValid)
        {
            await _clienteService.AddClienteAsync(cliente);
            return RedirectToAction(nameof(Index));
        }

        // Si hay errores, recargar las empresas en el ViewBag
        ViewBag.Empresas = new SelectList(await _empresaService.GetAllEmpresasAsync(), "Id", "Nombre");
        return View(cliente);
    }

    // Editar cliente (GET)
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var cliente = await _clienteService.GetClienteByIdAsync(id);
        if (cliente == null)
        {
            return NotFound();
        }

        ViewBag.Empresas = new SelectList(await _empresaService.GetAllEmpresasAsync(), "Id", "Nombre", cliente.EmpresaId);
        return View(cliente);
    }

    // Editar cliente (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Cliente cliente)
    {
        if (id != cliente.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _clienteService.UpdateClienteAsync(cliente);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Empresas = new SelectList(await _empresaService.GetAllEmpresasAsync(), "Id", "Nombre", cliente.EmpresaId);
        return View(cliente);
    }

    // Eliminar cliente (GET)
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var cliente = await _clienteService.GetClienteByIdAsync(id);
        if (cliente == null)
        {
            return NotFound();
        }

        return View(cliente);
    }

    // Eliminar cliente (POST)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _clienteService.DeleteClienteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
