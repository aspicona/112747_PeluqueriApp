using Microsoft.AspNetCore.Mvc;
using PeluqueriApp.Models;
using PeluqueriApp.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;


public class CitaController : Controller
{
    private readonly ICitaService _citaService;
    private readonly IClienteService _clienteService;
    private readonly IEmpleadoService _empleadoService;
    private readonly IEmpresaService _empresaService;
    private readonly IEstadoCitaService _estadoCitaService;

    public CitaController(ICitaService citaService, IClienteService clienteService,
        IEmpleadoService empleadoService, IEmpresaService empresaService,
        IEstadoCitaService estadoCitaService)
    {
        _citaService = citaService;
        _clienteService = clienteService;
        _empleadoService = empleadoService;
        _empresaService = empresaService;
        _estadoCitaService = estadoCitaService;
    }

    private async Task SetViewBagsAsync()
    {
        var clientes = await _clienteService.GetAllClientesAsync();
        ViewBag.Clientes = new SelectList(clientes.Select(c => new
        {
            Id = c.Id,
            NombreCompleto = $"{c.Nombre} {c.Apellido}"
        }), "Id", "NombreCompleto");

        var empleados = await _empleadoService.GetAllEmpleadosAsync();
        ViewBag.Empleados = new SelectList(empleados.Select(e => new
        {
            Id = e.Id,
            NombreCompleto = $"{e.Nombre} {e.Apellido}"
        }), "Id", "NombreCompleto");

        var empresas = await _empresaService.GetAllEmpresasAsync();
        ViewBag.Empresas = new SelectList(empresas, "Id", "Nombre");

        var estadosCita = await _estadoCitaService.GetAllEstadosAsync();
        ViewBag.EstadosCita = new SelectList(estadosCita, "Id", "Descripcion");
    }

    public async Task<IActionResult> Index()
    {
        await SetViewBagsAsync();
        var citas = await _citaService.GetAllCitasAsync();
        return View(citas);
    }

    public async Task<IActionResult> Create()
    {
        await SetViewBagsAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Cita cita)
    {
        if (ModelState.IsValid)
        {
            await _citaService.AddCitaAsync(cita);
            return RedirectToAction(nameof(Index));
        }

        await SetViewBagsAsync();
        return View(cita);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var cita = await _citaService.GetCitaByIdAsync(id);
        if (cita == null)
        {
            return NotFound();
        }

        await SetViewBagsAsync();
        return View(cita);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Cita cita)
    {
        if (id != cita.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _citaService.UpdateCitaAsync(cita);
            return RedirectToAction(nameof(Index));
        }

        await SetViewBagsAsync();
        return View(cita);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var cita = await _citaService.GetCitaByIdAsync(id);
        if (cita == null)
        {
            return NotFound();
        }

        return View(cita);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _citaService.DeleteCitaAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
