using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PeluqueriApp.Models;
using PeluqueriApp.Services;
using System.Threading.Tasks;
public class EmpleadoController : Controller
{
    private readonly IEmpleadoService _empleadoService;
    private readonly IEmpresaService _empresaService;
    private readonly IEspecialidadService _especialidadService;
    private readonly UserManager<ApplicationUser> _userManager;

    public EmpleadoController(IEmpleadoService empleadoService, IEmpresaService empresaService, IEspecialidadService especialidadService, UserManager<ApplicationUser> userManager)
    {
        _empleadoService = empleadoService;
        _empresaService = empresaService;
        _especialidadService = especialidadService;
        _userManager = userManager;
    }

    private async Task<int?> GetEmpresaIdFromUser()
    {
        var user = await _userManager.GetUserAsync(User);
        return user?.IdEmpresa;
    }

    //public async Task<IActionResult> Index()
    //{
    //    var empleados = await _empleadoService.GetAllEmpleadosAsync();
    //    return View(empleados);
    //}

    public async Task<IActionResult> Index()
    {
        var empresaId = await GetEmpresaIdFromUser();
        if (empresaId == null)
        {
            return Unauthorized(); // Maneja el caso de que no haya empresa asignada
        }

        var empleados = await _empleadoService.GetEmpleadosByEmpresaIdAsync(empresaId.Value);
        return View(empleados);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        
        ViewBag.Especialidades = new SelectList(await _especialidadService.GetAllEspecialidadesAsync(), "Id", "Descripcion");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Empleado empleado)
    {
        if (ModelState.IsValid)
        {
            empleado.IdEmpresa = (await GetEmpresaIdFromUser()).GetValueOrDefault();
            await _empleadoService.AddEmpleadoAsync(empleado);
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Especialidades = new SelectList(await _especialidadService.GetAllEspecialidadesAsync(), "Id", "Descripcion");
        return View(empleado);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var empleado = await _empleadoService.GetEmpleadoByIdAsync(id);
        if (empleado == null)
        {
            return NotFound();
        }
        ViewBag.Empresas = new SelectList(await _empresaService.GetAllEmpresasAsync(), "Id", "Nombre", empleado.IdEmpresa);
        ViewBag.Especialidades = new SelectList(await _especialidadService.GetAllEspecialidadesAsync(), "Id", "Descripcion", empleado.IdEspecialidad);
        return View(empleado);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Empleado empleado)
    {
        if (id != empleado.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _empleadoService.UpdateEmpleadoAsync(empleado);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Empresas = new SelectList(await _empresaService.GetAllEmpresasAsync(), "Id", "Nombre", empleado.IdEmpresa);
        ViewBag.Especialidades = new SelectList(await _especialidadService.GetAllEspecialidadesAsync(), "Id", "Descripcion", empleado.IdEspecialidad);
        return View(empleado);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var empleado = await _empleadoService.GetEmpleadoByIdAsync(id);
        if (empleado == null)
        {
            return NotFound();
        }
        return View(empleado);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _empleadoService.DeleteEmpleadoAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
