using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PeluqueriApp.Models;
using PeluqueriApp.Services;
using System.Threading.Tasks;


public class PagoController : Controller
{
    private readonly IPagoService _pagoService;
    private readonly ICitaService _citaService;
    private readonly IMetodoDePagoService _metodoDePagoService;
    private readonly IMercadoPagoService _mercadoPagoService;

    public PagoController(IPagoService pagoService, ICitaService citaService, IMetodoDePagoService metodoDePagoService, IMercadoPagoService mercadoPagoService)
    {
        _pagoService = pagoService;
        _citaService = citaService;
        _metodoDePagoService = metodoDePagoService;
        _mercadoPagoService = mercadoPagoService;
    }

    private async Task SetViewBagsAsync()
    {
        ViewBag.Citas = new SelectList(await _citaService.GetAllCitasAsync(), "Id", "Detalle");
        ViewBag.MetodosDePago = new SelectList(await _metodoDePagoService.GetAllMetodosDePagoAsync(), "Id", "Nombre");
    }

    // Listar pagos
    public async Task<IActionResult> Index()
    {
        var pagos = await _pagoService.GetAllPagosAsync();
        return View(pagos);
    }

    // Crear pago (GET)
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await SetViewBagsAsync();
        return View();
    }

    // Crear pago (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Pago pago)
    {
        if (ModelState.IsValid)
        {
            await _pagoService.AddPagoAsync(pago);
            return RedirectToAction(nameof(Index));
        }
        await SetViewBagsAsync();
        return View(pago);
    }

    // Editar pago (GET)
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var pago = await _pagoService.GetPagoByIdAsync(id);
        if (pago == null) return NotFound();
        await SetViewBagsAsync();
        return View(pago);
    }

    // Editar pago (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Pago pago)
    {
        if (id != pago.Id) return NotFound();
        if (ModelState.IsValid)
        {
            await _pagoService.UpdatePagoAsync(pago);
            return RedirectToAction(nameof(Index));
        }
        await SetViewBagsAsync();
        return View(pago);
    }

    // Eliminar pago (GET)
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var pago = await _pagoService.GetPagoByIdAsync(id);
        if (pago == null) return NotFound();
        return View(pago);
    }

    // Eliminar pago (POST)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _pagoService.DeletePagoAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult ConfirmacionPago(int idPago)
    {
        // Actualizar el estado del pago en la base de datos
        _pagoService.ActualizarEstadoPagoAsync(idPago, true); // Ejemplo de actualización
        return View("Confirmacion");
    }
}
